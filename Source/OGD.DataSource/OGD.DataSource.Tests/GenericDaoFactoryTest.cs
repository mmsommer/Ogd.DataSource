using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    public class GenericDaoFactoryTest : IDaoFactoryTest
    {
        internal override IDaoFactory CreateDaoFactoryImplementation()
        {
            return new GenericDaoFactory();
        }

        [Test]
        public void Test_CreateDao_ReturnsGenericDao()
        {
            var sut = CreateDaoFactoryImplementation();

            Assert.That(sut.CreateDao<IIdentifiable>(), Is.InstanceOf<GenericDao<IIdentifiable>>());
        }
    }
}
