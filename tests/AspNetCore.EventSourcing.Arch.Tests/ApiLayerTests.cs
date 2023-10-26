using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.EventSourcing.Arch.Tests
{
    [Collection("Sequential")]
    public class ApiLayerTests : BaseTests
    {
        [Fact]
        public void Api_Controllers_ShouldOnlyResideInApi()
        {
            AllTypes.That().Inherit(typeof(ControllerBase))
                .Should().ResideInNamespaceStartingWith("AspNetCore.EventSourcing.Api")
                .AssertIsSuccessful();
        }

        [Fact]
        public void Api_Controllers_ShouldInheritFromControllerBase()
        {
            Types.InAssembly(ApiAssembly)
                .That().HaveNameEndingWith("Controller")
                .Should().Inherit(typeof(ControllerBase))
                .AssertIsSuccessful();
        }

        [Fact]
        public void Api_Controllers_ShouldEndWithController()
        {
            AllTypes.That().Inherit(typeof(ControllerBase))
                .Should().HaveNameEndingWith("Controller")
                .AssertIsSuccessful();
        }
    }
}