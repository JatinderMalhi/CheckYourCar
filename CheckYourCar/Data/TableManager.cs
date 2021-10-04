using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineBusTransportation.Data
{
    public class TableManager
    {
        private CloudTable table;
        public TableManager(string _CloudTableName)
        {
            if (string.IsNullOrEmpty(_CloudTableName))
            {
                throw new ArgumentNullException("Table", "Table Name can't be empty");
            }
            try
            {
                string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=busstoragedata;AccountKey=PqCL014jS0eG9xBVDyfOhERRPuRkU4Jk14to3+2tQjuu+mjrASgtCsXsaEX94XKQMDdI4X5U4PJ+8EZyXIJspA==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                table = tableClient.GetTableReference(_CloudTableName);
                table.CreateIfNotExistsAsync();
            }
            catch (StorageException StorageExceptionObj)
            {
                throw StorageExceptionObj;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        public void InsertEntity<T>(T entity, bool forInsert = true) where T : TableEntity, new()
        {
            try
            {
                if (forInsert)
                {
                    var insertOperation = TableOperation.Insert(entity);
                    table.ExecuteAsync(insertOperation);
                }
                else
                {
                    var insertOrMergeOperation = TableOperation.InsertOrReplace(entity);
                    table.ExecuteAsync(insertOrMergeOperation);
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        public List<T> RetrieveEntity<T>(string Query = null) where T : TableEntity, new()
        {
            try
            {
                // Create the Table Query Object for Azure Table Storage  
                TableContinuationToken continuationToken = null;

                TableQuery<T> DataTableQuery = new TableQuery<T>();
                if (!String.IsNullOrEmpty(Query))
                {
                    DataTableQuery = new TableQuery<T>().Where(Query);
                }
                do
                {

                    TableQuerySegment<T> tableQueryResult = table.ExecuteQuerySegmentedAsync(DataTableQuery, continuationToken).Result;

                //IEnumerable<T> IDataList = table.ExecuteQuery(DataTableQuery);
                List<T> DataList = new List<T>();
                foreach (var singleData in tableQueryResult)
                    DataList.Add(singleData);
                return DataList;
                    continuationToken = tableQueryResult.ContinuationToken;
                } while (continuationToken != null);

            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }

    }
}
