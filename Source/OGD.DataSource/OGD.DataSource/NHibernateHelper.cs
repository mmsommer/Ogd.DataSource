using System;
using System.Runtime.CompilerServices;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Context;

[assembly: InternalsVisibleTo("Ogd.DataSource.Tests")]
namespace Ogd.DataSource
{
    public class NHibernateHelper : INHibernateHelper
    {
        public NHibernateHelper() { }

        public NHibernateHelper(Configuration configuration)
        {
            _configuration = configuration;
        }

        internal static volatile ISessionFactory _instance;

        private static object _syncRoot = new Object();

        private static bool _configured = false;

        internal static Configuration _configuration;

        public Configuration Configuration
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

        public ISessionFactory SessionFactory
        {
            get
            {
                if (_instance == null)
                {
                    // The instance of a SessionFactory can only be accessed when the instance variable completes.
                    // The syncRoot instance is locked to avoid deadlocks.
                    lock (_syncRoot)
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

        public bool SessionIsBound
        {
            get
            {
                if (_instance == null)
                {
                    return false;
                }
                return CurrentSessionContext.HasBind(SessionFactory);
            }
        }

        public ISession CurrentSession
        {
            get
            {
                Bind();

                return SessionFactory.GetCurrentSession();
            }
        }

        public void Bind()
        {
            if (!SessionIsBound)
            {
                var session = SessionFactory.OpenSession();
                session.BeginTransaction();
                CurrentSessionContext.Bind(session);
            }
        }

        public void RollBack(ISession session = null)
        {
            var currentSession = session ?? CurrentSession;
            if (SessionIsBound)
            {
                CurrentSessionContext.Unbind(SessionFactory);
                if (currentSession.IsOpen)
                {
                    Rollback(currentSession.Transaction);
                    currentSession.Flush();
                }
                currentSession.Close();
            }
        }

        public void Flush(ISession session = null)
        {
            var currentSession = session ?? CurrentSession;
            if (SessionIsBound)
            {
                CurrentSessionContext.Unbind(SessionFactory);

                try
                {
                    if (currentSession.IsOpen)
                    {
                        Commit(currentSession.Transaction);
                        currentSession.Flush();
                    }
                }
                catch
                {
                    Rollback(currentSession.Transaction);
                    throw;
                }
                finally
                {
                    currentSession.Close();
                }
            }
        }

        private void Commit(ITransaction transaction)
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }
        }

        private void Rollback(ITransaction transaction)
        {
            if (transaction != null && transaction.IsActive)
            {
                transaction.Rollback();
            }
        }

        private ISessionFactory BuildFactory()
        {
            Configure();

            return Configuration.BuildSessionFactory();
        }

        public Configuration Configure(Configuration configuration = null)
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
