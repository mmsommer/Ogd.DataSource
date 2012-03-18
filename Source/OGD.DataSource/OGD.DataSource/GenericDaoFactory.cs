using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Ogd.DataSource.Tests")]
namespace Ogd.DataSource
{
    internal class GenericDaoFactory : IDaoFactory
    {
        public IDao<T> CreateDao<T>()
            where T : class, IIdentifiable
        {
            return new GenericDao<T>();
        }
    }
}
