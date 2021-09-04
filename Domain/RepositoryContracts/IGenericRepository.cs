using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Interface for the GenericRepository class.
    /// </summary>
    /// <typeparam name="T">Domain level class</typeparam>
    public interface IGenericRepository<T> where T : class
    {
        /// <summary>
        /// Inserts a generic entity into the database.
        /// </summary>
        /// <param name="t">Generic entity</param>
        Task CreateAsync(T t);
        /// <summary>
        /// Reads a generic entity from the database.
        /// </summary>
        /// <param name="id">Entity's identifier</param>
        /// <returns>Read entity</returns>
        Task<T> ReadAsync(int id);
        /// <summary>
        /// Reads all generic entities from the database.
        /// </summary>
        /// <returns>IEnumerable of generic entities</returns>
        Task<IEnumerable<T>> ReadAllAsync();
        /// <summary>
        /// Updates a generic entity by it's ID.
        /// Entity's ID cannot be changed.
        /// </summary>
        /// <param name="t">Generic entity to be changed</param>
        Task UpdateAsync(T t);
        /// <summary>
        /// Deletes a generic entity from the database.
        /// </summary>
        /// <param name="id">To be deleted entity's ID</param>
        Task DeleteAsync(int id);
    }
}
