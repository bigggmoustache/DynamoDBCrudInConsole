using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

namespace DynamoDbLocalConnect
{
    public class TableFunctions
    {


        /// <summary>
        /// Creates a new Amazon DynamoDB table and then waits for the new
        /// table to become active.
        /// </summary>
        /// <param name="client">An initialized Amazon DynamoDB client object.</param>
        /// <param name="tableName">The name of the table to create.</param>
        /// <returns>A Boolean value indicating the success of the operation.</returns>
        public async Task<bool> CreateTableAsync(AmazonDynamoDBClient client, string tableName)
        {
            var deleteTable = await DeleteTable(client, "ProductCatalog");

            if (deleteTable is not null && deleteTable.HttpStatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine("Could not delete ProductCatalog table");
            }
            var response = await client.CreateTableAsync(new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition
                    {
                        AttributeName = "Id",
                        AttributeType = "N",
                    }
                },
                KeySchema = new List<KeySchemaElement>()
                {
                    new KeySchemaElement
                    {
                        AttributeName = "Id",
                        KeyType = "HASH",
                    },
                },
                ProvisionedThroughput = new ProvisionedThroughput
                {
                    ReadCapacityUnits = 5,
                    WriteCapacityUnits = 5,
                },
            });

            // Wait until the table is ACTIVE and then report success.
            Console.Write("Waiting for table to become active...");

            var request = new DescribeTableRequest
            {
                TableName = response.TableDescription.TableName,
            };

            TableStatus status;

            int sleepDuration = 2000;

            do
            {
                System.Threading.Thread.Sleep(sleepDuration);

                var describeTableResponse = await client.DescribeTableAsync(request);
                status = describeTableResponse.Table.TableStatus;

                Console.Write(".");
            }
            while (status != "ACTIVE");

            Console.WriteLine("Go to powershell and insert your data.");

            return status == TableStatus.ACTIVE;

        }
        public static async Task<DeleteTableResponse> DeleteTable(IAmazonDynamoDB client, string tableName)
        {
            try
            {
                var response = await client.DeleteTableAsync(new DeleteTableRequest
                {
                    TableName = tableName,
                });

                return response;
            }
            catch (ResourceNotFoundException)
            {
                // There is no such table.
                return null;
            }
        }

    }
}
