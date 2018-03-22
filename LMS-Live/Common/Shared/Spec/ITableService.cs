using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace LMS.Shared.Spec
{
    public interface ITableService<TEntity> where TEntity: TableEntity
    {
        Task<TEntity> FindAsync(string partitionKey, string rowKey);
        Task<TableEntitiesResult<TEntity>> GetAsync(int top = 1000, string filter = null, string continuationToken = null);
        Task UpsertAsync(TEntity entity);
        Task DeleteAsync(string partitionKey, string rowKey);
    }
}