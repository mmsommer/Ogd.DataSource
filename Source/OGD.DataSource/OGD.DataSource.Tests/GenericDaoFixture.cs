using System.Collections.Generic;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    [TestFixture]
    public class GenericDaoFixture
    {
        [Test]
        public void TestSave_ReturnsNHibernateResult()
        {
            var mockCountry = new Mock<IIdentifiable>();
            mockCountry.SetupProperty(t => t.Id);

            var country = mockCountry.Object;

            country.Id = 0;

            var mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.Save(mockCountry.Object)).Returns(1);

            var mockSessionFactory = new Mock<ISessionFactory>();
            mockSessionFactory.Setup(f => f.GetCurrentSession()).Returns(mockSession.Object);

            var dao = new GenericDao<IIdentifiable>(mockSessionFactory.Object);

            country = dao.Save(country);

            Assert.That(country.Id, Is.EqualTo(1));
        }

        [Test]
        public void TestUpdate_ReturnsTrue()
        {
            var mockTransaction = new Mock<ITransaction>();
            mockTransaction.Setup(t => t.Commit());

            var mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.BeginTransaction()).Returns(mockTransaction.Object);
            mockSession.Setup(s => s.Update(It.IsAny<object>()));

            var mockSessionFactory = new Mock<ISessionFactory>();
            mockSessionFactory.Setup(f => f.GetCurrentSession()).Returns(mockSession.Object);

            var dao = new GenericDao<IIdentifiable>(mockSessionFactory.Object);

            var result = dao.Update(new Mock<IIdentifiable>().Object);

            Assert.That(result, Is.True);

            mockSession.Verify(s => s.Update(It.IsAny<object>()), Times.Once());
        }

        [Test]
        public void TestDelete_ReturnsTrue()
        {
            var mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.Delete(It.IsAny<object>()));

            var mockSessionFactory = new Mock<ISessionFactory>();
            mockSessionFactory.Setup(f => f.GetCurrentSession()).Returns(mockSession.Object);

            var dao = new GenericDao<IIdentifiable>(mockSessionFactory.Object);

            var result = dao.Delete(new Mock<IIdentifiable>().Object);

            Assert.That(result, Is.True);

            mockSession.Verify(s => s.Delete(It.IsAny<object>()), Times.Once());
        }

        [Test]
        public void TestSaveOrUpdateAll_ReturnsTrue()
        {
            var mockIntervention1 = new Mock<IIdentifiable>();
            mockIntervention1.SetupProperty(t => t.Id);
            mockIntervention1.Object.Id = 0;

            var mockTempObject2 = new Mock<IIdentifiable>();
            mockTempObject2.SetupProperty(t => t.Id);
            mockTempObject2.Object.Id = 0;

            var mockTempObject3 = new Mock<IIdentifiable>();
            mockTempObject3.SetupProperty(t => t.Id);
            mockTempObject3.Object.Id = 1;

            var mockTempObject4 = new Mock<IIdentifiable>();
            mockTempObject4.SetupProperty(t => t.Id);
            mockTempObject4.Object.Id = 2;

            var mockTheses = new List<IIdentifiable>();
            mockTheses.Add(mockIntervention1.Object);
            mockTheses.Add(mockTempObject2.Object);
            mockTheses.Add(mockTempObject3.Object);
            mockTheses.Add(mockTempObject4.Object);

            var mockTransaction = new Mock<ITransaction>();
            mockTransaction.Setup(t => t.Commit());

            var mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.BeginTransaction()).Returns(mockTransaction.Object);
            mockSession.Setup(s => s.Save(It.IsAny<object>())).Returns(1);
            mockSession.Setup(s => s.Update(It.IsAny<object>()));

            var mockSessionFactory = new Mock<ISessionFactory>();
            mockSessionFactory.Setup(f => f.GetCurrentSession()).Returns(mockSession.Object);

            var dao = new GenericDao<IIdentifiable>(mockSessionFactory.Object);

            var result = dao.SaveOrUpdateAll(mockTheses);

            Assert.That(result, Is.True);

            mockSession.Verify(s => s.Save(It.IsAny<object>()), Times.Exactly(2));
            mockSession.Verify(s => s.Update(It.IsAny<object>()), Times.Exactly(2));
        }

        [Test]
        public void TestDeleteAll_ReturnsTrue()
        {
            var mockTheses = new List<IIdentifiable>();
            mockTheses.Add(new Mock<IIdentifiable>().Object);
            mockTheses.Add(new Mock<IIdentifiable>().Object);
            mockTheses.Add(new Mock<IIdentifiable>().Object);
            mockTheses.Add(new Mock<IIdentifiable>().Object);

            var mockSession = new Mock<ISession>();
            mockSession.Setup(s => s.Delete(It.IsAny<object>()));

            var mockSessionFactory = new Mock<ISessionFactory>();
            mockSessionFactory.Setup(f => f.GetCurrentSession()).Returns(mockSession.Object);

            var dao = new GenericDao<IIdentifiable>(mockSessionFactory.Object);

            var result = dao.DeleteAll(mockTheses);

            Assert.That(result, Is.True);

            mockSession.Verify(s => s.Delete(It.IsAny<object>()), Times.Exactly(4));
        }
    }
}
