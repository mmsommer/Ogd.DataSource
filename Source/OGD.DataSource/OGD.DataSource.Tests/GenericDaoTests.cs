using System.Collections.Generic;
using System.Linq;
using Moq;
using NHibernate;
using NUnit.Framework;
using NHibernate.Cfg;

namespace Ogd.DataSource.Tests
{
    [TestFixture]
    public class GenericDaoTests : IDaoTests<IIdentifiable>
    {
        private Mock<INHibernateHelper> stubNHibernateHelper { get; set; }

        private INHibernateHelper NHibernateHelper { get; set; }

        [SetUp]
        public void Init()
        {
            var stubConfiguration = new Mock<Configuration>();
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory
                .Setup(x => x.GetCurrentSession())
                .Returns(() => stubSession.Object);
            stubNHibernateHelper = new Mock<INHibernateHelper>();
            stubNHibernateHelper
                .Setup(x => x.Configure(null))
                .Returns(() => stubConfiguration.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);
            NHibernateHelper = stubNHibernateHelper.Object;
        }

        [Test]
        public void Test_GetAll_CollectionSet_ReturnsCollection()
        {
            Collection = new List<IIdentifiable>{
                { new Mock<IIdentifiable>().Object },
                { new Mock<IIdentifiable>().Object },
                { new Mock<IIdentifiable>().Object }
            }.AsQueryable();

            var sut = new GenericDao<IIdentifiable>(Collection, NHibernateHelper);

            Assert.That(sut.GetAll(), Is.EquivalentTo(Collection));
        }

        [Test]
        public void Test_GetAll_SessionFactorySet_ReturnsCollection()
        {
            Collection = new List<IIdentifiable>{
                { new Mock<IIdentifiable>().Object },
                { new Mock<IIdentifiable>().Object },
                { new Mock<IIdentifiable>().Object }
            }.AsQueryable();
            var mockSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory
                .Setup(x => x.GetCurrentSession())
                .Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(Collection, NHibernateHelper);

            Assert.That(sut.GetAll(), Is.EquivalentTo(Collection));
        }

        [Test]
        public void Test_GetById_SessionFactorySet_IdentityPresent_ReturnsObject()
        {
            var id = 1;
            var stubObject = new Mock<IIdentifiable>();
            stubObject.Setup(x => x.Id).Returns(id);
            var stubObject1 = new Mock<IIdentifiable>();
            stubObject1.Setup(x => x.Id).Returns(id + 1);
            var stubObject2 = new Mock<IIdentifiable>();
            stubObject2.Setup(x => x.Id).Returns(id + 2);
            Collection = new List<IIdentifiable>{
                { stubObject.Object },
                { stubObject1.Object },
                { stubObject2.Object }
            }.AsQueryable();
            var mockSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(Collection, NHibernateHelper);

            Assert.That(sut.GetById(id), Is.SameAs(stubObject.Object));
        }

        [Test]
        public void Test_GetById_SessionFactorySet_IdentityNotPresent_ReturnsObject()
        {
            var id = 1;
            var stubObject = new Mock<IIdentifiable>();
            stubObject.Setup(x => x.Id).Returns(id + 1);
            var stubObject1 = new Mock<IIdentifiable>();
            stubObject1.Setup(x => x.Id).Returns(id + 2);
            var stubObject2 = new Mock<IIdentifiable>();
            stubObject2.Setup(x => x.Id).Returns(id + 3);
            Collection = new List<IIdentifiable>{
                { stubObject.Object },
                { stubObject1.Object },
                { stubObject2.Object }
            }.AsQueryable();
            var mockSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(Collection, NHibernateHelper);

            Assert.That(sut.GetById(id), Is.Null);
        }

        [Test]
        public void Test_Save_EntityGiven_VerifySessionSaveIsCalled()
        {
            var stubObject = new Mock<IIdentifiable>();
            var mockSession = new Mock<ISession>();
            mockSession.Setup(x => x.Save(It.IsAny<IIdentifiable>())).Returns(() => 1);
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(null, NHibernateHelper);
            sut.Save(stubObject.Object);
            mockSession.Verify(x => x.Save(It.IsAny<IIdentifiable>()));
        }

        [Test]
        public void Test_Save_EntityGiven_VerifyEntitySetIdIsCalled()
        {
            var mockObject = new Mock<IIdentifiable>();
            mockObject.SetupProperty(x => x.Id, 0);
            var stubSession = new Mock<ISession>();
            stubSession.Setup(x => x.Save(It.IsAny<IIdentifiable>())).Returns(() => 1);
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(null, NHibernateHelper);
            sut.Save(mockObject.Object);
            mockObject.VerifySet(x => x.Id = It.IsAny<int>());
        }

        [Test]
        public void Test_Save_EntityGiven_NewIdIsSet()
        {
            var oldId = 0;
            var newId = 1;
            var mockObject = new Mock<IIdentifiable>();
            mockObject.SetupProperty(x => x.Id, oldId);
            var stubSession = new Mock<ISession>();
            stubSession.Setup(x => x.Save(It.IsAny<IIdentifiable>())).Returns(() => newId);
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(null, NHibernateHelper);
            sut.Save(mockObject.Object);

            Assert.That(mockObject.Object.Id, Is.EqualTo(newId));
        }

        [Test]
        public void Test_Update_EntityGiven_VerifySessionUpdateIsCalled()
        {
            var stubObject = new Mock<IIdentifiable>();
            var mockSession = new Mock<ISession>();
            mockSession.Setup(x => x.Update(It.IsAny<IIdentifiable>()));
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(null, NHibernateHelper);
            sut.Update(stubObject.Object);
            mockSession.Verify(x => x.Update(It.IsAny<IIdentifiable>()));
        }

        [Test]
        public void Test_Delete_EntityGiven_VerifySessionDeleteIsCalled()
        {
            var stubObject = new Mock<IIdentifiable>();
            var mockSession = new Mock<ISession>();
            mockSession.Setup(x => x.Delete(It.IsAny<IIdentifiable>()));
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(null, NHibernateHelper);
            sut.Delete(stubObject.Object);
            mockSession.Verify(x => x.Delete(It.IsAny<IIdentifiable>()));
        }

        [Test]
        public void Test_SaveOrUpdateAll_OneToBeSaved_VerifySessionSaveIsCalled()
        {
            var stubObject = new Mock<IIdentifiable>();
            var mockSession = new Mock<ISession>();
            mockSession.Setup(x => x.Save(It.IsAny<IIdentifiable>())).Returns(() => 1);
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(null, NHibernateHelper);
            sut.SaveOrUpdateAll(new IIdentifiable[] { stubObject.Object });
            mockSession.Verify(x => x.Save(It.IsAny<IIdentifiable>()));
        }

        [Test]
        public void Test_SaveOrUpdateAll_OneToBeUpdated_VerifySessionUpdateIsCalled()
        {
            var stubObject = new Mock<IIdentifiable>();
            stubObject.Setup(x => x.Id).Returns(() => 1);
            var mockSession = new Mock<ISession>();
            mockSession.Setup(x => x.Update(It.IsAny<IIdentifiable>()));
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(null, NHibernateHelper);
            sut.SaveOrUpdateAll(new IIdentifiable[] { stubObject.Object });
            mockSession.Verify(x => x.Update(It.IsAny<IIdentifiable>()));
        }

        [Test]
        public void Test_DeleteAll_OneToBeSaved_VerifySessionDeleteIsCalled()
        {
            var stubObject = new Mock<IIdentifiable>();
            var mockSession = new Mock<ISession>();
            mockSession.Setup(x => x.Save(It.IsAny<IIdentifiable>())).Returns(() => 1);
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);

            var sut = new GenericDao<IIdentifiable>(null, NHibernateHelper);
            sut.DeleteAll(new IIdentifiable[] { stubObject.Object });
            mockSession.Verify(x => x.Delete(It.IsAny<IIdentifiable>()));
        }

        protected override IDao<IIdentifiable> CreateIDaoImplementation()
        {
            var daoStub = new GenericDao<IIdentifiable>(Collection, NHibernateHelper);

            return daoStub;
        }

        protected override IDao<IIdentifiable> CreateIDaoImplementation(ISessionFactory sessionFactory)
        {
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => sessionFactory);
            var daoStub = new GenericDao<IIdentifiable>(null, NHibernateHelper);

            return daoStub;
        }

    }
}
