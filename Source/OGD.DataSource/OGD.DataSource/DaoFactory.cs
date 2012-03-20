using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ogd.DataSource.Tests")]
namespace Ogd.DataSource
{
    public class DaoFactory : IDaoFactory
    {
        internal IDaoFactory Factory { get; private set; }

        public DaoFactory() : this(null, null, null) { }

        internal DaoFactory(
            IDaoFactory wrappedFactory,
            IQueryable initialCollection,
            INHibernateHelper nHibernateHelper
        )
        {
            Factory = wrappedFactory ?? new GenericDaoFactory(initialCollection, nHibernateHelper);
        }

        public IDao<T> CreateDao<T>()
            where T : class, IIdentifiable
        {
            return Factory.CreateDao<T>();
        }
    }
}
