﻿using AspNetCore.EventSourcing.Application.AutofacModules;
using AspNetCore.EventSourcing.Core.Abstractions.Entities;
using AspNetCore.EventSourcing.Infrastructure.AutofacModules;
using System.Reflection;

namespace AspNetCore.EventSourcing.Arch.Tests
{
    public abstract class BaseTests
    {
        protected static Assembly ApiAssembly = typeof(Api.Controllers.CustomersController).Assembly;
        protected static Assembly ApplicationAssembly = typeof(ApplicationModule).Assembly;
        protected static Assembly InfrastuctureAssembly = typeof(InfrastructureModule).Assembly;
        protected static Assembly CoreAssembly = typeof(EntityBase).Assembly;
        protected static Types AllTypes = Types.InAssemblies(new List<Assembly> { ApiAssembly, ApplicationAssembly, InfrastuctureAssembly, CoreAssembly });
    }
}
