using System;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

namespace OGD
{
    public class NHibernateHelper
    {
        private static volatile ISessionFactory _instance;

        private static object syncRoot = new Object();

        private static bool _configured = false;
        private static Configuration _configuration;

        public static Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    _configuration = new Configuration();
                }
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        public static ISessionFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    // The instance of a SessionFactory can only be accessed when the instance variable completes.
                    // The syncRoot instance is locked to avoid deadlocks.
                    lock (syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = BuildFactory();
                        }
                    }
                }
                return _instance;
            }
        }

        public static bool SessionIsBound
        {
            get
            {
                if (_instance == null)
                {
                    return false;
                }
                return CurrentSessionContext.HasBind(Instance);
            }
        }

        public static ISession GetCurrentSession()
        {
            Bind();

            return Instance.GetCurrentSession();
        }

        public static void Bind()
        {
            if (!SessionIsBound)
            {
                var session = Instance.OpenSession();
                session.BeginTransaction();
                CurrentSessionContext.Bind(session);
            }
        }

        public static void RollBack(ISession session = null)
        {
            var currentSession = session ?? GetCurrentSession();
            if (NHibernateHelper.SessionIsBound)
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance);
                if (currentSession.IsOpen)
                {
                    Rollback(currentSession.Transaction);
                    currentSession.Flush();
                }
                currentSession.Close();
            }
        }

        public static void Flush(ISession session = null)
        {
            var currentSession = session ?? GetCurrentSession();
            if (NHibernateHelper.SessionIsBound)
            {
                CurrentSessionContext.Unbind(NHibernateHelper.Instance);
                if (currentSession.IsOpen)
                {
                    Commit(currentSession.Transaction);
                    currentSession.Flush();
                }
                currentSession.Close();
            }
        }

        private static void Commit(ITransaction transaction)
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }
        }

        private static void Rollback(ITransaction transaction)
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Rollback();
            }
        }

        private static ISessionFactory BuildFactory()
        {
            Configure();

            return Configuration.BuildSessionFactory();
        }

        public static Configuration Configure(Configuration configuration = null)
        {
            if (configuration != null)
            {
                Configuration = configuration;
            }

            if (!_configured)
            {
                Configuration.Configure();
                _configured = true;
            }

            return Configuration;
        }
    }
}
