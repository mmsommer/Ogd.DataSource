using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NHibernate;
using NHibernate.Linq;

[assembly: InternalsVisibleTo("Ogd.DataSource.Tests")]
namespace Ogd.DataSource
{
    public class GenericDao<T> : IDao<T>
        where T : class, IIdentifiable
    {
        private ISessionFactory _sessionFactory = null;

        private ISessionFactory SessionFactory
        {
            // SessionFactory has to be injected for test, but cannot be configured by constructor.
            // In this way the injected factory is returned or the static instance property.
            get { return _sessionFactory ?? NHibernateHelper.Instance; }
        }

        private IQueryable<T> _collection = null;

        private IQueryable<T> Collection
        {
            // Collection has to be injected for test, but cannot be configured by constructor.
            // In this way the injected collection is returned or the static instance property.
            get { return _collection ?? SessionFactory.GetCurrentSession().Query<T>(); }
        }

        internal GenericDao() : this(null, null) { }

        internal GenericDao(IQueryable<T> initialCollection = null, ISessionFactory sessionFactory = null)
        {
            _collection = initialCollection;
            _sessionFactory = sessionFactory;
        }

        public virtual IQueryable<T> GetAll()
        {
            return Collection;
        }

        public virtual T GetById(int id)
        {
            return GetAll().SingleOrDefault(t => t.Id == id);
        }

        public virtual T Save(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var session = SessionFactory.GetCurrentSession();

            entity.Id = (int)session.Save(entity);

            return entity;
        }

        public virtual bool Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var session = SessionFactory.GetCurrentSession();

            session.Update(entity);

            return true;
        }

        public virtual bool Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            var session = SessionFactory.GetCurrentSession();

            session.Delete(entity);

            return true;
        }

        public virtual bool SaveOrUpdateAll(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            var returnValue = true;

            foreach (var entity in entities)
            {
                if (entity.Id < 1)
                {
                    returnValue &= this.Save(entity).Id > 0;
                }
                else
                {
                    returnValue &= this.Update(entity);
                }
            }

            return returnValue;
        }

        public virtual bool DeleteAll(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            var returnValue = true;

            foreach (var entity in entities)
            {
                returnValue &= this.Delete(entity);
            }

            return returnValue;
        }
    }
}
