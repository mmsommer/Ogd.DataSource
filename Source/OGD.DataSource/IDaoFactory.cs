namespace Ogd.DataSource
{
    public interface IDaoFactory
    {
        IDao<T> GetDao<T>()
            where T : class, IIdentifiable;
    }
}
