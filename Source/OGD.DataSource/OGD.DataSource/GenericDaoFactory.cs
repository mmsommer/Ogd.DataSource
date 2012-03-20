using System.Linq;
using System.Runtime.CompilerServices;
using NHibernate;

[assembly: InternalsVisibleTo("Ogd.DataSource.Tests")]
namespace Ogd.DataSource
{
    internal class GenericDaoFactory : IDaoFactory
    {
        private IQueryable InitialCollection { get; set; }

        private INHibernateHelper NHibernateHelper { get; set; }

        public GenericDaoFactory() : this(null, null) { }

        public GenericDaoFactory(
            IQueryable initialCollection,
            INHibernateHelper nHibernateHelper
        )
        {
            InitialCollection = initialCollection;
            NHibernateHelper = nHibernateHelper;
        }

        public IDao<T> CreateDao<T>()
            where T : class, IIdentifiable
        {
            return new GenericDao<T>((IQueryable<T>)InitialCollection, NHibernateHelper);
        }
    }
}
