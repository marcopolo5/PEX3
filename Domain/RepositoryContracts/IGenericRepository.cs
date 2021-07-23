using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task CreateAsync(T t);
        Task<T> ReadAsync(int id);
        Task<IEnumerable<T>> ReadAllAsync();
        Task UpdateAsync(T t);
        Task DeleteAsync(int id);
    }
}
