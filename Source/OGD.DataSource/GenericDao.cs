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
        private ISessionFactory SessionFactory
        {
            get
            {
                if (NHibernateHelper != null)
                {
                    return NHibernateHelper.SessionFactory;
                }
                else
                {
                    return null;
                }
            }
        }

        private INHibernateHelper NHibernateHelper { get; set; }

        private IQueryable<T> Collection { get; set; }

        internal GenericDao() : this(null, null) { }

        internal GenericDao(
            IQueryable<T> initialCollection,
            INHibernateHelper nHibernateHelper
        )
        {
            NHibernateHelper = nHibernateHelper ?? new NHibernateHelper();
            Collection = initialCollection ?? SessionFactory.GetCurrentSession().Query<T>();
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
