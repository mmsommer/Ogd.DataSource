using Moq;
using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    public class DaoFactoryTest : IDaoFactoryTest
    {
        internal override IDaoFactory CreateDaoFactoryImplementation()
        {
            return new DaoFactory();
        }

        internal IDaoFactory CreateDaoFactoryImplementation(IDaoFactory daoFactory)
        {
            return new DaoFactory(daoFactory);
        }

        [Test]
        public void Test_Constructor_FactorySupplied_InternalFactorySet()
        {
            var factory = new Mock<IDaoFactory>().Object;

            var sut = new DaoFactory(factory);

            Assert.That(sut.Factory, Is.SameAs(factory));
        }

        [Test]
        public void Test_CreateDao_FactorySupplied_VerifySuppliedFactoryCreateIsCalled()
        {
            var factoryMock = new Mock<IDaoFactory>();
            var factory = factoryMock.Object;

            var sut = new DaoFactory(factory);

            sut.CreateDao<IIdentifiable>();

            factoryMock.Verify(x => x.CreateDao<IIdentifiable>());
        }
    }
}
