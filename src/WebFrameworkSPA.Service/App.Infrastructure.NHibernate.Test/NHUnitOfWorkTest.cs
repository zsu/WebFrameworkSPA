using System;
using NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Infrastructure.NHibernate;
using Moq;

namespace NCommon.Data.NHibernate.Test
{
    [TestClass()]
    public class NHUnitOfWorkTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ctor_throws_ArgumentNullException_when_sessionResolver_parameter_is_null()
        {
            new NHUnitOfWork(null);
        }

        [TestMethod]
        public void GetSessionFor_returns_session_for_type()
        {
            var resolver = new Mock<INHSessionResolver>();
            resolver.Setup(x => x.GetSessionKeyFor<string>()).Returns(Guid.NewGuid());
            resolver.Setup(x => x.OpenSessionFor<string>()).Returns(new Mock<ISession>().Object);

            var unitOfWork = new NHUnitOfWork(resolver.Object);
            var session = unitOfWork.GetSession<string>();
            Assert.IsNotNull(session);
        }

        [TestMethod]
        public void GetSessionFor_returns_same_session_for_types_handled_by_same_factory()
        {
            var sessionKey = Guid.NewGuid();
            var resolver = new Mock<INHSessionResolver>();
            resolver.Setup(x => x.GetSessionKeyFor<string>()).Returns(sessionKey);
            resolver.Setup(x => x.GetSessionKeyFor<int>()).Returns(sessionKey);
            resolver.Setup(x => x.OpenSessionFor<string>()).Returns(new Mock<ISession>().Object);
            resolver.Setup(x => x.OpenSessionFor<int>()).Returns(new Mock<ISession>().Object);

            var unitOfWork = new NHUnitOfWork(resolver.Object);
            var stringSession = unitOfWork.GetSession<string>();
            var intSession = unitOfWork.GetSession<int>();

            Assert.AreSame(stringSession, intSession);
            resolver.Verify(x => x.GetSessionKeyFor<string>());
            resolver.Verify(x => x.GetSessionKeyFor<int>());
            resolver.Verify(x => x.OpenSessionFor<string>(), Times.Exactly(1));
        }

        [TestMethod]
        public void GetSessionFor_returns_different_session_for_types_handled_by_different_factory()
        {
            var resolver = new Mock<INHSessionResolver>();
            resolver.Setup(x => x.GetSessionKeyFor<string>()).Returns(Guid.NewGuid());
            resolver.Setup(x => x.GetSessionKeyFor<int>()).Returns(Guid.NewGuid());
            resolver.Setup(x => x.OpenSessionFor<string>()).Returns(new Mock<ISession>().Object);
            resolver.Setup(x => x.OpenSessionFor<int>()).Returns(new Mock<ISession>().Object);

            var unitOfWork = new NHUnitOfWork(resolver.Object);
            var stringSession = unitOfWork.GetSession<string>();
            var intSession = unitOfWork.GetSession<int>();

            Assert.AreNotSame(stringSession, intSession);
            resolver.Verify(x => x.GetSessionKeyFor<string>());
            resolver.Verify(x => x.GetSessionKeyFor<int>());
            resolver.Verify(x => x.OpenSessionFor<string>());
            resolver.Verify(x => x.OpenSessionFor<int>());
        }

        [TestMethod]
        public void Flush_calls_flush_on_all_open_ISession_instances()
        {
            var resolver = new Mock<INHSessionResolver>();
            resolver.Setup(x => x.GetSessionKeyFor<string>()).Returns(Guid.NewGuid());
            resolver.Setup(x => x.GetSessionKeyFor<int>()).Returns(Guid.NewGuid());
            var session = new Mock<ISession>();
            resolver.Setup(x => x.OpenSessionFor<string>()).Returns(session.Object);
            resolver.Setup(x => x.OpenSessionFor<int>()).Returns(session.Object);

            var unitOfWork = new NHUnitOfWork(resolver.Object);
            unitOfWork.GetSession<string>();
            unitOfWork.GetSession<int>();

            unitOfWork.Flush();
            resolver.Object.OpenSessionFor<string>();
            session.Verify(x => x.Flush());
            resolver.Object.OpenSessionFor<int>();
            session.Verify(x => x.Flush());
        }

        [TestMethod]
        public void Dispose_disposes_all_open_ISession_instances()
        {
            var resolver = new Mock<INHSessionResolver>();
            resolver.Setup(x => x.GetSessionKeyFor<string>()).Returns(Guid.NewGuid());
            resolver.Setup(x => x.GetSessionKeyFor<int>()).Returns(Guid.NewGuid());
            var session=new Mock<ISession>();
            resolver.Setup(x => x.OpenSessionFor<string>()).Returns(session.Object);
            resolver.Setup(x => x.OpenSessionFor<int>()).Returns(session.Object);

            var unitOfWork = new NHUnitOfWork(resolver.Object);
            unitOfWork.GetSession<string>();
            unitOfWork.GetSession<int>();

            unitOfWork.Dispose();
            resolver.Object.OpenSessionFor<string>();
            session.Verify(x => x.Dispose());
            resolver.Object.OpenSessionFor<int>();
            session.Verify(x => x.Dispose());
        }
    }
}