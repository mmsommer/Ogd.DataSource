using NHibernate.Linq;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    public class GenericDaoFactoryTest : IDaoFactoryTest
    {
        private INHibernateHelper NHibernateHelper { get; set; }

        [SetUp]
        public void Init()
        {
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory
                .Setup(x => x.GetCurrentSession())
                .Returns(() => stubSession.Object);
            var stubNHibernateHelper = new Mock<INHibernateHelper>();
            stubNHibernateHelper
                .Setup(x => x.SessionFactory)
                .Returns(() => stubSessionFactory.Object);
            NHibernateHelper = stubNHibernateHelper.Object;
        }

        internal override IDaoFactory CreateDaoFactoryImplementation()
        {
            return new GenericDaoFactory(null, NHibernateHelper);
        }

        [Test]
        public void Test_CreateDao_ReturnsGenericDao()
        {
            var sut = CreateDaoFactoryImplementation();

            Assert.That(sut.CreateDao<IIdentifiable>(), Is.InstanceOf<GenericDao<IIdentifiable>>());
        }
    }
}
