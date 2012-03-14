using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NHibernate;
using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    [TestFixture]
    public abstract class IDaoTests<T>
        where T : class, IIdentifiable
    {
        public static IQueryable<T> Collection { get; protected set; }

        protected abstract IDao<T> CreateIDaoImplementation();

        protected abstract IDao<T> CreateIDaoImplementation(ISessionFactory sessionFactory);

        [Test]
        public void Test_GetAll_NoEntities_ReturnsEmptyCollection()
        {
            Collection = new List<T>().AsQueryable();
            var sut = CreateIDaoImplementation();

            Assert.That(sut.GetAll(), Is.Empty);
        }

        [Test]
        public void Test_GetAll_OneEntity_ReturnsCollectionWithOneEntity()
        {
            var stubObject = new Mock<T>();
            Collection = new List<T> { { stubObject.Object } }.AsQueryable();
            var sut = CreateIDaoImplementation();

            Assert.That(sut.GetAll(), Is.EquivalentTo(Collection));
        }

        [Test]
        public void Test_GetAll_MultipleEntities_ReturnsCollectionWithMultipleEntities()
        {
            var stubObject = new Mock<T>();
            var stubObject1 = new Mock<T>();
            var stubObject2 = new Mock<T>();
            var stubObject3 = new Mock<T>();
            var stubObject4 = new Mock<T>();
            Collection = new List<T> {
                { stubObject.Object }, 
                { stubObject1.Object }, 
                { stubObject2.Object },
                { stubObject3.Object }, 
                { stubObject4.Object }
            }.AsQueryable();
            var sut = CreateIDaoImplementation();

            Assert.That(sut.GetAll(), Is.EquivalentTo(Collection));
        }

        [Test]
        public void Test_GetById_IdNotPresent_ReturnsNull()
        {
            var searchedId = 1;
            var id = 2;
            var stubObject = new Mock<T>();
            stubObject.SetupProperty(x => x.Id, id);
            Collection = new List<T> { { stubObject.Object } }.AsQueryable();
            var sut = CreateIDaoImplementation();

            Assert.That(sut.GetById(searchedId), Is.Null);
        }

        [Test]
        public void Test_GetById_IdPresent_ReturnsObject()
        {
            var searchedId = 1;
            var id = 1;
            var stubObject = new Mock<T>();
            stubObject.SetupProperty(x => x.Id, id);
            Collection = new List<T> { { stubObject.Object } }.AsQueryable();
            var sut = CreateIDaoImplementation();

            Assert.That(sut.GetById(searchedId), Is.SameAs(stubObject.Object));
        }

        [Test]
        public void Test_Save_EntityIsNull_ThrowsArgumentNullException()
        {
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.Throws<ArgumentNullException>(() => sut.Save(null));
        }

        [Test]
        public void Test_Save_EntityIsNotNull_ReturnsEntity()
        {
            var stubObject = new Mock<T>();
            var stubSession = new Mock<ISession>();
            stubSession.Setup(x => x.Save(It.IsAny<T>())).Returns(() => 1);
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.That(sut.Save(stubObject.Object), Is.SameAs(stubObject.Object));
        }

        [Test]
        public void Test_Update_EntityIsNull_ThrowsArgumentNullException()
        {
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.Throws<ArgumentNullException>(() => sut.Update(null));
        }

        [Test]
        public void Test_Update_EntityIsNotNull_ReturnsEntity()
        {
            var stubObject = new Mock<T>();
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.That(sut.Update(stubObject.Object), Is.True);
        }

        [Test]
        public void Test_Delete_EntityIsNull_ThrowsArgumentNullException()
        {
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.Throws<ArgumentNullException>(() => sut.Delete(null));
        }

        [Test]
        public void Test_Delete_EntityIsNotNull_ReturnsEntity()
        {
            var stubObject = new Mock<T>();
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.That(sut.Delete(stubObject.Object), Is.True);
        }

        [Test]
        public void Test_SaveOrUpdateAll_CollectionIsNull_ThrowsArgumentNullException()
        {
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.Throws<ArgumentNullException>(() => sut.SaveOrUpdateAll(null));
        }

        [Test]
        public void Test_SaveOrUpdateAll_CollectionIsEmpty_ReturnsTrue()
        {
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.That(sut.SaveOrUpdateAll(new T[] { }), Is.True);
        }

        [Test]
        public void Test_SaveOrUpdateAll_CollectionHasOneOrMoreEntities_ReturnsTrue()
        {
            var stubObject = new Mock<T>();
            stubObject.SetupProperty(x => x.Id, 0);
            var stubSession = new Mock<ISession>();
            stubSession.Setup(x => x.Save(It.IsAny<T>())).Returns(() => 1);
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.That(sut.SaveOrUpdateAll(new T[] { stubObject.Object }), Is.True);
        }

        [Test]
        public void Test_DeleteAll_CollectionIsNull_ThrowsArgumentNullException()
        {
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.Throws<ArgumentNullException>(() => sut.DeleteAll(null));
        }

        [Test]
        public void Test_DeleteAll_CollectionIsEmpty_ReturnsTrue()
        {
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.That(sut.SaveOrUpdateAll(new T[] { }), Is.True);
        }

        [Test]
        public void Test_DeleteAll_CollectionHasOneOrMoreEntities_ReturnsTrue()
        {
            var stubObject = new Mock<T>();
            var stubSession = new Mock<ISession>();
            var stubSessionFactory = new Mock<ISessionFactory>();
            stubSessionFactory.Setup(x => x.GetCurrentSession()).Returns(() => stubSession.Object);

            var sut = CreateIDaoImplementation(stubSessionFactory.Object);

            Assert.That(sut.DeleteAll(new T[] { stubObject.Object }), Is.True);
        }
    }

    public static class SessionExtensions
    {
        internal static Func<ISession, IQueryable<IIdentifiable>> QueryAll = (session) => IDaoTests<IIdentifiable>.Collection.AsQueryable();

        public static IQueryable<IIdentifiable> Query(this ISession session)
        {
            return QueryAll(session);
        }
    }
}
