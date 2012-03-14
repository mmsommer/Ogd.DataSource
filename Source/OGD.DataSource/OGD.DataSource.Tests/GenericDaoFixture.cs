using Moq;
using NHibernate;
using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    [TestFixture]
    public class GenericDaoFixture : IDaoTests<IIdentifiable>
    {
        [Test]
        public void Test_Save_EntityGiven_VerifySessionSaveIsCalled()
        {
            var stubObject = new Mock<IIdentifiable>();
            var mockSession = new Mock<ISession>();
            mockSession.Setup(x => x.Save(It.IsAny<IIdentifiable>())).Returns(() => 1);
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => mockSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);
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

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);
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

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);
            sut.Save(mockObject.Object);

            Assert.That(mockObject.Object.Id, Is.EqualTo(newId));
        }

        protected override IDao<IIdentifiable> CreateIDaoImplementation()
        {
            var daoStub = new GenericDao<IIdentifiable>(initialCollection: Collection);

            return daoStub;
        }

        protected override IDao<IIdentifiable> CreateIDaoImplementation(ISessionFactory sessionFactory)
        {
            var daoStub = new GenericDao<IIdentifiable>(sessionFactory: sessionFactory);

            return daoStub;
        }

    }
}
