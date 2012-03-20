using Moq;
using NHibernate;
using NHibernate.Cfg;
using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    public class DaoFactoryTest : IDaoFactoryTest
    {
        private INHibernateHelper NHibernateHelper { get; set; }

        [SetUp]
        public void Init()
        {
            var stubConfiguration = new Mock<Configuration>();
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession())
                .Returns(() => stubSession.Object);
            var stubNHibernateHelper = new Mock<INHibernateHelper>();
            stubNHibernateHelper
                .Setup(x => x.Configure(null))
                .Returns(() => stubConfiguration.Object);
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);
            NHibernateHelper = stubNHibernateHelper.Object;
        }

        internal override IDaoFactory CreateDaoFactoryImplementation()
        {
            return new DaoFactory(null, null, NHibernateHelper);
        }

        internal IDaoFactory CreateDaoFactoryImplementation(IDaoFactory daoFactory)
        {
            return new DaoFactory(daoFactory, null, NHibernateHelper);
        }

        [Test]
        public void Test_Constructor_FactorySupplied_InternalFactorySet()
        {
            var factory = new Mock<IDaoFactory>().Object;

            var sut = new DaoFactory(factory, null, NHibernateHelper);

            Assert.That(sut.Factory, Is.SameAs(factory));
        }

        [Test]
        public void Test_CreateDao_FactorySupplied_VerifySuppliedFactoryCreateIsCalled()
        {
            var factoryMock = new Mock<IDaoFactory>();
            var factory = factoryMock.Object;

            var sut = new DaoFactory(factory, null, NHibernateHelper);

            sut.CreateDao<IIdentifiable>();

            factoryMock.Verify(x => x.CreateDao<IIdentifiable>());
        }
    }
}
