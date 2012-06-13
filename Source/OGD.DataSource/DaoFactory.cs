using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ogd.DataSource.Tests")]
namespace Ogd.DataSource
{
    public class DaoFactory : IDaoFactory
    {
        internal IDaoFactory Factory { get; private set; }

        private IDictionary<Type, object> DaoCache { get; set; }

        public DaoFactory() : this(null, null, null) { }

        internal DaoFactory(
            IDaoFactory wrappedFactory,
            IQueryable initialCollection,
            INHibernateHelper nHibernateHelper
        )
        {
            Factory = wrappedFactory ?? new GenericDaoFactory(initialCollection, nHibernateHelper);
            DaoCache = new Dictionary<Type, object>();
        }

        public IDao<T> GetDao<T>()
            where T : class, IIdentifiable
        {
            if (!DaoCache.ContainsKey(typeof(T)))
            {
                DaoCache.Add(typeof(T), Factory.GetDao<T>());
            }
            return (IDao<T>)DaoCache[typeof(T)];
        }
    }
}
