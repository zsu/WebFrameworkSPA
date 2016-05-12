﻿using System;
using System.Web.Http.Dependencies;
namespace App.Common.InversionOfControl
{


    public interface ICustomDependencyResolver : IDependencyResolver,IServiceResolver, IServiceRegistrar, IServiceInjector, IDisposable
    {
        object Container { get;}
    }
}