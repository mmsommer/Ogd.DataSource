using Moq;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Context;

namespace Ogd.DataSource.Tests
{
    [TestFixture]
    public abstract class INHibernateHelperTests
    {
        public abstract INHibernateHelper CreateINHibernateImplementation();

        [Test]
        public void Test_Configuration_IsNotNull()
        {
            var sut = CreateINHibernateImplementation();

            Assert.That(sut.Configuration, Is.Not.Null);
        }

        [Test]
        public void Test_Configuration_IsOfTypeConfiguration()
        {
            var sut = CreateINHibernateImplementation();

            Assert.That(sut.Configuration, Is.InstanceOf<Configuration>());
        }

        [Test]
        public void Test_SessionFactory_IsNotNull()
        {
            var sut = CreateINHibernateImplementation();

            Assert.That(sut.SessionFactory, Is.Not.Null);
        }

        [Test]
        public void Test_SessionFactory_IsOfTypeISessionFactory()
        {
            var sut = CreateINHibernateImplementation();

            Assert.That(sut.SessionFactory, Is.InstanceOf<ISessionFactory>());
        }

        [Test]
        public void Test_SessionIsBound_SessionNotBound_ReturnsFalse()
        {
            var sut = CreateINHibernateImplementation();
            CurrentSessionContext.Unbind(sut.SessionFactory);

            Assert.That(sut.SessionIsBound, Is.False);
        }

        [Test]
        public void Test_SessionIsBound_SessionBound_ReturnsTrue()
        {
            var sut = CreateINHibernateImplementation();
            sut.Bind();

            Assert.That(sut.SessionIsBound, Is.True);
        }

        [Test]
        public void Test_CurrentSession_IsNotNull()
        {
            var sut = CreateINHibernateImplementation();

            Assert.That(sut.CurrentSession, Is.Not.Null);
        }

        [Test]
        public void Test_CurrentSession_IsOfTypeISession()
        {
            var sut = CreateINHibernateImplementation();

            Assert.That(sut.CurrentSession, Is.InstanceOf<ISession>());
        }

        [Test]
        public void Test_Bind_SessionIsBound()
        {
            var sut = CreateINHibernateImplementation();
            sut.Bind();

            Assert.That(sut.SessionIsBound, Is.True);
        }

        [Test]
        public void Test_RollBack_SessionNotGiven_DoesNotThrowException()
        {
            var sut = CreateINHibernateImplementation();

            sut.RollBack();
        }

        [Test]
        public void Test_RollBack_SessionGiven_DoesNotThrowException()
        {
            var sessionMock = new Mock<ISession>();
            var sut = CreateINHibernateImplementation();

            sut.RollBack(sessionMock.Object);
        }

        [Test]
        public void Test_Flush_SessionNotGiven_DoesNotThrowException()
        {
            var sut = CreateINHibernateImplementation();

            sut.Flush();
        }

        [Test]
        public void Test_Flush_SessionGiven_DoesNotThrowException()
        {
            var sessionMock = new Mock<ISession>();
            var sut = CreateINHibernateImplementation();

            sut.Flush(sessionMock.Object);
        }

        [Test]
        public void Test_Configure_ConfigurationNotGiven_DoesNotTrowException()
        {
            var sut = CreateINHibernateImplementation();

            sut.Configure();
        }

        [Test]
        public void Test_Configure_ConfigurationNotGiven_ReturnsNotNull()
        {
            var sut = CreateINHibernateImplementation();

            Assert.That(sut.Configure(), Is.Not.Null);
        }

        [Test]
        public void Test_Configure_ConfigurationNotGiven_ReturnsInstanceOfConfiguration()
        {
            var sut = CreateINHibernateImplementation();

            Assert.That(sut.Configure(), Is.InstanceOf<Configuration>());
        }
    }
}
