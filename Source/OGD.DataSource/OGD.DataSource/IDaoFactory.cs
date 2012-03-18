namespace Ogd.DataSource
{
    public interface IDaoFactory
    {
        IDao<T> CreateDao<T>()
            where T : class, IIdentifiable;
    }
}
