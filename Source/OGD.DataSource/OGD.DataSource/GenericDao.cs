using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

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

        public GenericDao() { }

        public GenericDao(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory ?? NHibernateHelper.Instance;
        }

        public virtual IQueryable<T> GetAll()
        {
            var session = SessionFactory.GetCurrentSession();

            return session.Query<T>();
        }

        public virtual T GetById(int id)
        {
            return GetAll().SingleOrDefault(t => t.Id == id);
        }

        public virtual T Save(T entity)
        {
            var session = SessionFactory.GetCurrentSession();

            entity.Id = (int)session.Save(entity);

            return entity;
        }

        public virtual bool Update(T entity)
        {
            var session = SessionFactory.GetCurrentSession();

            session.Update(entity);

            return true;
        }

        public virtual bool Delete(T entity)
        {
            var session = SessionFactory.GetCurrentSession();

            session.Delete(entity);

            return true;
        }

        public virtual bool SaveOrUpdateAll(IEnumerable<T> entities)
        {
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
            var returnValue = true;

            foreach (var entity in entities)
            {
                returnValue &= this.Delete(entity);
            }

            return returnValue;
        }
    }
}
