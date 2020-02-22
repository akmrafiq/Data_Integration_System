using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data_Integration.API
{
    public class DynamoDbHelper : IDisposable
    {
        AmazonDynamoDBClient _client;
        public DynamoDbHelper()
        {
            _client = new AmazonDynamoDBClient();
        }
        public async Task Insert(DbInsertItem item)
        {
            var request = new PutItemRequest
            {
                TableName = item.TableName,
            };

            foreach (var field in item.Values)
            {
                request.Item.Add(field.Key, GetValue(field.Value));
            }

            try
            {
                var response = await _client.PutItemAsync(request);
            }
            catch (Exception ex)
            {
                var x = ex.Message;
            }
        }
        private AttributeValue GetValue(object value)
        {
            if (value.GetType() == typeof(int))
                return new AttributeValue { N = (string)value };
            else if (value.GetType() == typeof(string))
                return new AttributeValue { S = (string)value };
            else if (value.GetType() == typeof(DateTimeOffset))
                return new AttributeValue { S = value.ToString() };
            else
                return null;
        }
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
