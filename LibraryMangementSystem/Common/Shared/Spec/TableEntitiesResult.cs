using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace LMS.Shared.Spec
{
    public class TableEntitiesResult<TEntity> where TEntity: TableEntity
    {
        public string ContinuationToken { get; set; }
        public List<TEntity> Results { get; set; }
    }
}