using AspNetCore.EventSourcing.Application.Abstractions.Commands;
using AspNetCore.EventSourcing.Application.Abstractions.Queries;
using AutoMapper;

namespace AspNetCore.EventSourcing.Arch.Tests
{
    [Collection("Sequential")]
    public class ApplicationLayerTests : BaseTests
    {
        [Fact]
        public void ApplicationLayer_Cqrs_QueriesEndWithQuery()
        {
            AllTypes.That().Inherit(typeof(Query<>))
            .Should().HaveNameEndingWith("Query")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_ContainsAllQueries()
        {
            AllTypes.That().HaveNameEndingWith("Query")
            .Should().ResideInNamespace("AspNetCore.EventSourcing.Application")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_CommandsEndWithCommand()
        {
            AllTypes.That().Inherit(typeof(Command))
            .Should().HaveNameEndingWith("Command")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_ContainsAllCommands()
        {
            AllTypes.That().HaveNameEndingWith("Command")
            .Should().ResideInNamespace("AspNetCore.EventSourcing.Application")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_QueryHandlersEndWithQueryHandler()
        {
            AllTypes.That().Inherit(typeof(QueryHandler<,>))
            .Should().HaveNameEndingWith("QueryHandler")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_ContainsAllQueryHandlers()
        {
            AllTypes.That().HaveNameEndingWith("QueryHandler")
            .Should().ResideInNamespace("AspNetCore.EventSourcing.Application")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_CommandHandlersEndWithCommandHandler()
        {
            AllTypes.That().Inherit(typeof(CommandHandler<>))
            .Should().HaveNameEndingWith("CommandHandler")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Cqrs_ContainsAllCommandHandlers()
        {
            AllTypes.That().HaveNameEndingWith("CommandHandler")
            .Should().ResideInNamespace("AspNetCore.EventSourcing.Application")
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_Dtos_ShouldBeMutable()
        {
            AllTypes.That().HaveNameEndingWith("Dto")
                           .And().DoNotHaveName("IntegrationSupportGroupUserDto")
            .Should().BeMutable()
            .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_MappingProfiles_ShouldOnlyResideInApplication()
        {
            AllTypes.That().Inherit(typeof(Profile))
                .Should().ResideInNamespaceStartingWith("AspNetCore.EventSourcing.Application")
                .AssertIsSuccessful();
        }

        [Fact]
        public void ApplicationLayer_MappingProfiles_ShouldEndWithProfile()
        {
            AllTypes.That().Inherit(typeof(Profile))
                .Should().HaveNameEndingWith("Profile")
                .AssertIsSuccessful();
        }
    }
}