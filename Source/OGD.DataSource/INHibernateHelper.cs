using System.Runtime.CompilerServices;
using NHibernate;
using NHibernate.Cfg;

[assembly: InternalsVisibleTo("Ogd.DataSource.Tests")]
namespace Ogd.DataSource
{
    public interface INHibernateHelper
    {
        Configuration Configuration { get; }

        ISessionFactory SessionFactory { get; }

        bool SessionIsBound { get; }

        ISession CurrentSession { get; }

        void Bind();

        void RollBack(ISession session = null);

        void Flush(ISession session = null);

        Configuration Configure(Configuration configuration = null);
    }
}
