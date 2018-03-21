using LMS.Shared.Spec;
using Newtonsoft.Json;
using System.Threading.Tasks;
using LMS.Shared.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace LMS.Data
{
    public class TableService<TEntity>: ITableService<TEntity> where TEntity: TableEntity, new()
    {
        private CloudTable _table;

        public TableService(AzureStorageConfiguration storageConfiguration, string tableName)
        {
            var credentials = new StorageCredentials(storageConfiguration.AccountName, storageConfiguration.AccountKey);
            var storageAccount = new CloudStorageAccount(credentials, useHttps: true);
            CreateTable(storageAccount, tableName);
        }

        private void CreateTable(CloudStorageAccount storageAccount, string tableName)
        {
            var tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(tableName);
            _table.CreateIfNotExistsAsync().Wait();
        }

        public async Task<TEntity> FindAsync(string partitionKey, string rowKey)
        {
            var operation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
            var tableResult = await _table.ExecuteAsync(operation);
            if (tableResult == null)
                return null;
            return tableResult.Result as TEntity;
        }

        public async Task<TableEntitiesResult<TEntity>> GetAsync(int top = 1000, string filter = null, string continuationToken = null)
        {
            var query = string.IsNullOrEmpty(filter)
                ? new TableQuery<TEntity>()
                    .Take(top)
                : new TableQuery<TEntity>()
                    .Where(filter)
                    .Take(top);

            TableContinuationToken token = null;
            if (!(string.IsNullOrEmpty(continuationToken)))
                token = JsonConvert.DeserializeObject<TableContinuationToken>(continuationToken);

            var tableResult = await _table.ExecuteQuerySegmentedAsync<TEntity>(query, token);

            var result = new TableEntitiesResult<TEntity>()
            {
                Results = tableResult.Results,
                ContinuationToken = tableResult.ContinuationToken != null ? JsonConvert.SerializeObject(tableResult.ContinuationToken) : null
            };
            return result;
        }

        public async Task UpsertAsync(TEntity entity)
        {
            var operation = TableOperation.InsertOrMerge(entity);
            await _table.ExecuteAsync(operation);
        }

        public async Task DeleteAsync(string partitionKey, string rowKey)
        {
            var entity = await FindAsync(partitionKey, rowKey);
            if (entity != null)
            {
                var operation = TableOperation.Delete(entity);
                await _table.ExecuteAsync(operation);
            }
        }
    }
}