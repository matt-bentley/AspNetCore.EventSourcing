using AspNetCore.EventSourcing.Core.Customers.Entities;
using AspNetCore.EventSourcing.Core.Tests.Builders;
using AspNetCore.EventSourcing.Infrastructure.Tests.Repositories.Abstract;

namespace AspNetCore.EventSourcing.Infrastructure.Tests.Repositories
{
    public class RepositoryTests : BaseRepositoryTests
    {
        [Fact]
        public async Task GivenRepository_WhenInsert_ThenInserted()
        {
            var repository = GetRepository<Customer>();
            var entity = new CustomerBuilder().Build();

            repository.Insert(entity);
            await GetUnitOfWork().CommitAsync();

            var inserted = await repository.GetByIdAsync(entity.Id);
            inserted.Should().NotBeNull();
        }

        [Fact]
        public async Task GivenRepository_WhenInsertMultiple_ThenInserted()
        {
            var repository = GetRepository<Customer>();

            var entities = new List<Customer>()
            {
                new CustomerBuilder().WithEmail("tester1@test.com").Build(),
                new CustomerBuilder().WithEmail("tester2@test.com").Build()
            };

            repository.Insert(entities);
            await GetUnitOfWork().CommitAsync();

            var inserted = await repository.GetByIdAsync(entities.Last().Id);
            inserted.Should().NotBeNull();
        }

        [Fact]
        public async Task GivenRepository_WhenUpdate_ThenUpdated()
        {
            var repository = GetRepository<Customer>();
            var date = DateTime.UtcNow;
            var entity = new CustomerBuilder().Build();

            repository.Insert(entity);
            await GetUnitOfWork().CommitAsync();

            var inserted = await repository.GetByIdAsync(entity.Id);
            inserted?.Leave();

            await GetUnitOfWork().CommitAsync();

            var updated = await repository.GetByIdAsync(entity.Id);
            updated!.Id.Should().Be(entity.Id);
            updated.HasLeft.Should().BeTrue();
        }

        [Fact]
        public async Task GivenRepository_WhenDelete_ThenDeleted()
        {
            var repository = GetRepository<Customer>();
            var entity = new CustomerBuilder().Build();

            repository.Insert(entity);
            var id = entity.Id;
            await GetUnitOfWork().CommitAsync();

            repository.Delete(entity);
            await GetUnitOfWork().CommitAsync();
            var inserted = await repository.GetByIdAsync(id);
            inserted.Should().BeNull();
        }

        [Fact]
        public async Task GivenRepository_WhenRemove_ThenRemoved()
        {
            var repository = GetRepository<Customer>();
            var entity = new CustomerBuilder().Build();
            repository.Insert(entity);
            var id = entity.Id;
            await GetUnitOfWork().CommitAsync();

            repository.Remove(new List<Customer>() { entity });
            await GetUnitOfWork().CommitAsync();
            var inserted = await repository.GetByIdAsync(id);
            inserted.Should().BeNull();
        }

        [Fact]
        public async Task GivenRepository_WhenGetAll_ThenGetAll()
        {
            var repository = GetRepository<Customer>();
            var entity1 = new CustomerBuilder().WithEmail("tester1@test.com").Build();
            var entity2 = new CustomerBuilder().WithEmail("tester2@test.com").Build();
            repository.Insert(entity1);
            repository.Insert(entity2);
            await GetUnitOfWork().CommitAsync();

            var inserted = repository.GetAll();
            inserted.ToList().Count.Should().Be(2);
        }

        [Fact]
        public async Task GivenRepository_WhenGetAllTracked_ThenChangesCommitted()
        {
            var repository = GetRepository<Customer>();
            var entity1 = new CustomerBuilder().WithEmail("tester1@test.com").Build();
            var entity2 = new CustomerBuilder().WithEmail("tester2@test.com").Build();
            var date = DateTime.UtcNow;

            repository.Insert(entity1);
            var id1 = entity1.Id;
            repository.Insert(entity2);
            await GetUnitOfWork().CommitAsync();

            var inserted = repository.GetAll(false).Where(u => u.Id == id1).FirstOrDefault();
            inserted?.Leave();
            await GetUnitOfWork().CommitAsync();

            var updated = await repository.GetByIdAsync(id1);
            updated!.HasLeft.Should().BeTrue();
        }
    }
}
