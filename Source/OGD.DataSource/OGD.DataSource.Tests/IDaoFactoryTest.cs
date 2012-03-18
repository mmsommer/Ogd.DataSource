using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    [TestFixture]
    public abstract class IDaoFactoryTest
    {
        internal abstract IDaoFactory CreateDaoFactoryImplementation();

        [Test]
        public void Test_CreateDao_ShouldNotThrowException()
        {
            var sut = CreateDaoFactoryImplementation();

            sut.CreateDao<IIdentifiable>();
        }

        [Test]
        public void Test_CreateDao_ShouldReturnIDao()
        {
            var sut = CreateDaoFactoryImplementation();

            Assert.That(sut.CreateDao<IIdentifiable>(), Is.InstanceOf<IDao<IIdentifiable>>());
        }
    }
}
