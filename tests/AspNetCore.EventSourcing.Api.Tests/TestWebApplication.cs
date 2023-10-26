using AspNetCore.EventSourcing.Application.Abstractions.Repositories;
using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using AspNetCore.EventSourcing.Core.Tests.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using AspNetCore.EventSourcing.Core.Customers.Entities;
using AspNetCore.EventSourcing.Core.Accounts.Entities;
using AspNetCore.EventSourcing.Core.Accounts.ReadModels;
using AspNetCore.EventSourcing.Core.Abstractions.ReadModels;
using AspNetCore.EventSourcing.Core.Tests.Builders;

namespace AspNetCore.EventSourcing.Api.Tests
{
    internal class TestWebApplication : WebApplicationFactory<Program>
    {
        public readonly Mock<IEventStreamRepository<Account>> AccountsRepository;
        public readonly List<Account> TestAccounts = new List<Account>();

        public readonly Mock<IReadModelRepository<AccountReadModel>> AccountReadModelsRepository;
        public readonly List<AccountReadModel> TestAccountReadModels = new List<AccountReadModel>();

        public readonly Mock<IReadModelRepository<TransactionReadModel>> TransactionReadModelsRepository;
        public readonly List<TransactionReadModel> TestTransactionReadModels = new List<TransactionReadModel>();

        public readonly Mock<IRepository<Customer>> CustomersRepository;
        public readonly List<Customer> TestCustomers = new List<Customer>();

        public readonly Mock<IUnitOfWork> UnitOfWork = new Mock<IUnitOfWork>();

        private readonly List<(object Service, Type Type)> _replaceServices = new List<(object Service, Type Type)>();

        public TestWebApplication()
        {
            AccountsRepository = CreateMockEventStreamRepository(TestAccounts);
            AccountReadModelsRepository = CreateMockReadModelRepository(TestAccountReadModels);
            TransactionReadModelsRepository = CreateMockReadModelRepository(TestTransactionReadModels);
            CustomersRepository = CreateMockRepository(TestCustomers);
            SeedData();
        }

        private void SeedData()
        {
            var customer = new CustomerBuilder().Build();
            TestCustomers.Add(customer);

            var accountBuilder = new AccountBuilder().WithCustomer(customer.Id).WithAccountNumber("12345678");
            var account1 = accountBuilder.Build();
            TestAccounts.Add(account1);
            TestAccountReadModels.Add(accountBuilder.BuildReadModel());
            TestTransactionReadModels.Add(new TransactionBuilder().WithAccount(account1.Id).Build());

            accountBuilder = new AccountBuilder().WithCustomer(customer.Id).WithAccountNumber("87654321");
            var account2 = accountBuilder.Build();
            TestAccounts.Add(account2);
            TestAccountReadModels.Add(accountBuilder.BuildReadModel());
            TestTransactionReadModels.Add(new TransactionBuilder().WithAccount(account2.Id).Build());
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.SetBasePath(Directory.GetCurrentDirectory())
                      .AddJsonFile("appsettings.test.json", false);
            });
            builder.UseEnvironment("Test");
            builder.ConfigureContainer<ContainerBuilder>(container =>
            {
                container.RegisterInstance(UnitOfWork.Object);
            });
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                foreach (var service in _replaceServices)
                {
                    ReplaceService(services, service.Service, service.Type);
                }
            });
            base.ConfigureWebHost(builder);
        }

        public TestWebApplication WithReplacementService<TService>(TService service) where TService : class
        {
            _replaceServices.Add((service, typeof(TService)));
            return this;
        }

        private void ReplaceService(IServiceCollection services, object service, Type serviceType)
        {
            var descriptor = services.FirstOrDefault(d => d.ServiceType.IsAssignableTo(serviceType));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddSingleton(serviceType, service);
        }

        public Mock<IRepository<T>> CreateMockRepository<T>(IEnumerable<T> items) where T : AggregateRoot
        {
            var repository = MockRepositoryFactory.Create(items);
            _replaceServices.Add((repository.Object, typeof(IRepository<T>)));
            return repository;
        }

        public Mock<IReadModelRepository<T>> CreateMockReadModelRepository<T>(IEnumerable<T> items) where T : ReadModelBase
        {
            var repository = MockRepositoryFactory.CreateReadModel(items);
            _replaceServices.Add((repository.Object, typeof(IReadModelRepository<T>)));
            return repository;
        }

        public Mock<IEventStreamRepository<T>> CreateMockEventStreamRepository<T>(IEnumerable<T> items) where T : EventSourcedAggregate
        {
            var repository = MockRepositoryFactory.CreateEventStream(items);
            _replaceServices.Add((repository.Object, typeof(IEventStreamRepository<T>)));
            return repository;
        }

        public StringContent GetStringContent<T>(T item)
        {
            return new StringContent(JsonSerializer.Serialize(item, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }), Encoding.UTF8, "application/json");
        }

        public async Task<MultipartFormDataContent> GetTestFileFormAsync(string fileName = "example.mp4", string mediaContentType = "video/mp4", int sizeInMb = 1)
        {
            var form = new MultipartFormDataContent();
            var file = await GetTestFileAsync(sizeInMb);
            var fileContent = new ByteArrayContent(file, 0, file.Length);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(mediaContentType);
            form.Add(fileContent, "file", fileName);
            return form;
        }

        public async Task<byte[]> GetTestFileAsync(int sizeInMb)
        {
            const int blockSize = 1024 * 8;
            const int blocksPerMb = (1024 * 1024) / blockSize;
            byte[] data = new byte[blockSize];
            Random rng = new Random();
            var stream = new MemoryStream();
            for (int i = 0; i < sizeInMb * blocksPerMb; i++)
            {
                rng.NextBytes(data);
                await stream.WriteAsync(data, 0, data.Length);
            }
            return stream.ToArray();
        }
    }
}
