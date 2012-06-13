using System.Collections.Generic;
using System.Linq;

namespace Ogd.DataSource
{
    public interface IDao<T>
        where T : class, IIdentifiable
    {
        /// <summary>
        /// Gets all the objects of type T.
        /// </summary>
        /// <returns>An <see cref="IQueryable"/> with the objects of type T.</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets the element of type T, with the id.
        /// </summary>
        /// <param name="id">The id that has to be equal to the return object's.</param>
        /// <returns>The object with the same id as the parameter.</returns>
        T GetById(int id);

        /// <summary>
        /// Stores a new object in the database.
        /// </summary>
        /// <param name="entity">The object that has to be saved.</param>
        /// <returns>The newly saved object if the save action is succeeded.</returns>
        T Save(T entity);

        /// <summary>
        /// Updates an existing object.
        /// </summary>
        /// <param name="entity">The object that has to be updated.</param>
        /// <param name="connectionInfo">The <see cref="IConnectionInfo"/> to be used for the connection.</param>
        /// <returns>True if the update action is succeeded.</returns>
        bool Update(T entity);

        /// <summary>
        /// Deletes the <see cref="ShippingOrder"/> with the specified id.
        /// </summary>
        /// <param name="entity">The object that has to be deleted.</param>
        /// <returns>True if the delete action is succeeded.</returns>
        bool Delete(T entity);

        /// <summary>
        /// Stores or updates all given entities.
        /// </summary>
        /// <param name="entities">The collection of entities to be saved or updated.</param>
        /// <returns>True if saves and or updates have succeeded.</returns>
        bool SaveOrUpdateAll(IEnumerable<T> entities);

        /// <summary>
        /// Deletes the collection of entities.
        /// </summary>
        /// <param name="entities">The entities to be deleted.</param>
        /// <returns>True if the delete actions have succeeded.</returns>
        bool DeleteAll(IEnumerable<T> entities);
    }
}
