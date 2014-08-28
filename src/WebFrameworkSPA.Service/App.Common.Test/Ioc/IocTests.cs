using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Common.InversionOfControl;

namespace App.Common.Test
{
    [TestClass]
    public class IocTests : BaseTest
    {
        public override void Dispose()
        {
            resolver.Verify();
        }

        [TestMethod]
        public void InitializeWith_Should_Call_Factory_CreateInstance()
        {
            var factory = new Mock<IDependencyResolverFactory>();

            factory.Setup(f => f.CreateInstance()).Returns(resolver.Object).Verifiable();

            IoC.InitializeWith(factory.Object);
            factory.Verify();
        }

        [TestMethod]
        public void Container_Should_User_Resolver()
        {
            resolver.Setup(r => r.Container).Returns((IDummyObject)null).Verifiable();

            IoC.GetContainer<IDummyObject>();
        }

        [TestMethod]
        public void Register_Should_Use_Resolver()
        {
            resolver.Setup(r => r.RegisterType(null,typeof(IDummyObject),typeof(DummyObject),LifetimeType.Transient));

            IoC.RegisterType(null,typeof(IDummyObject),typeof(DummyObject),LifetimeType.Transient);
        }

        [TestMethod]
        public void Inject_Should_Use_Resolver()
        {
            resolver.Setup(r => r.Inject(It.IsAny<DummyObject>()));

            IoC.Inject(new DummyObject());
        }

        [TestMethod]
        public void Parameterless_Resolve_Should_Use_Resolver()
        {
            resolver.Setup(r => r.GetService(typeof(IDummyObject))).Returns((IDummyObject)null).Verifiable();

            IoC.GetService(typeof(IDummyObject));
        }

        [TestMethod]
        public void Parameterized_Resolve_Should_Use_Resolver()
        {
            resolver.Setup(r => r.GetService(typeof(IDummyObject),"foo")).Returns((IDummyObject)null).Verifiable();

            IoC.GetService<IDummyObject>(typeof(IDummyObject), "foo");
        }

        [TestMethod]
        public void Parameterless_Generic_Resolve_Should_Use_Resolve()
        {
            resolver.Setup(r => r.GetService(typeof(IDummyObject))).Returns((IDummyObject)null).Verifiable();

            IoC.GetService<IDummyObject>();
        }

        [TestMethod]
        public void Parameterized_Generic_Resolve_Should_Use_Resolver()
        {
            resolver.Setup(r => r.GetService(typeof(IDummyObject), "foo")).Returns((IDummyObject)null).Verifiable();

            IoC.GetService<IDummyObject>(typeof(IDummyObject), "foo");
        }

        [TestMethod]
        public void ResolveAll_Should_Use_Resolver()
        {
            resolver.Setup(r => r.GetServices(typeof(IDummyObject))).Returns((IDummyObject[]) null).Verifiable();

            IoC.GetServices(typeof(IDummyObject));
        }

        [TestMethod]
        public void Reset_Should_Use_Resolver()
        {
            resolver.Setup(r => r.Dispose()).Verifiable();

            IoC.Reset();
        }
    }
}