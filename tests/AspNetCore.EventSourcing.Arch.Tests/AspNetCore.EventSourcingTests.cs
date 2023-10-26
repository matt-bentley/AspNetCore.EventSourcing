using AspNetCore.EventSourcing.Application.Abstractions.Repositories;

namespace AspNetCore.EventSourcing.Arch.Tests
{
    [Collection("Sequential")]
    public class EventSourcingTests : BaseTests
    {
        [Fact]
        public void EventSourcing_Layers_ApplicationDoesNotReferenceInfrastructure()
        {
            AllTypes.That().ResideInNamespace("AspNetCore.EventSourcing.Application")
            .ShouldNot().HaveDependencyOn("AspNetCore.EventSourcing.Infrastructure")
            .AssertIsSuccessful();
        }

        [Fact]
        public void EventSourcing_Layers_CoreDoesNotReferenceOuter()
        {
            var coreTypes = AllTypes.That().ResideInNamespace("AspNetCore.EventSourcing.Core");

            coreTypes.ShouldNot().HaveDependencyOn("AspNetCore.EventSourcing.Infrastructure")
                .AssertIsSuccessful();

            coreTypes.ShouldNot().HaveDependencyOn("AspNetCore.EventSourcing.Application")
                .AssertIsSuccessful();
        }

        [Fact]
        public void EventSourcing_Repositories_OnlyInInfrastructure()
        {
            AllTypes.That().HaveNameEndingWith("Repository")
            .Should().ResideInNamespaceStartingWith("AspNetCore.EventSourcing.Infrastructure")
            .AssertIsSuccessful();

            AllTypes.That().HaveNameEndingWith("Repository")
                .And().AreClasses()
                .Should().ImplementInterface(typeof(IRepository<>))
                .AssertIsSuccessful();
        }

        [Fact]
        public void EventSourcing_Repositories_ShouldEndWithRepository()
        {
            AllTypes.That().Inherit(typeof(IRepository<>))
                .Should().HaveNameEndingWith("Repository")
                .AssertIsSuccessful();
        }
    }
}