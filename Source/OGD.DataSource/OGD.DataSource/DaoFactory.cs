using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ogd.DataSource.Tests")]
namespace Ogd.DataSource
{
    public class DaoFactory : IDaoFactory
    {
        internal IDaoFactory Factory { get; private set; }

        public DaoFactory() : this(null) { }

        internal DaoFactory(IDaoFactory wrappedFactory)
        {
            Factory = wrappedFactory ?? new GenericDaoFactory();
        }

        public IDao<T> CreateDao<T>()
            where T : class, IIdentifiable
        {
            return Factory.CreateDao<T>();
        }
    }
}
