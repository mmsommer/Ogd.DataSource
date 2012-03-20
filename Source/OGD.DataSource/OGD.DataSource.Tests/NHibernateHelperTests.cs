using NHibernate.Cfg;
using NUnit.Framework;

namespace Ogd.DataSource.Tests
{
    public class NHibernateHelperTests : INHibernateHelperTests
    {
        public override INHibernateHelper CreateINHibernateImplementation()
        {
            return new NHibernateHelper();
        }
    }
}
