using static System.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2;

namespace DynamoDbLocalConnect
{
    public class CRUD
    {
        public async Task CreateTableItem(AmazonDynamoDBClient client, Book book)
        {
            var request = new PutItemRequest
            {
                TableName = "ProductCatalog", //dumb dumb
                Item = new Dictionary<string, AttributeValue>()
                {
                    { "Id", new AttributeValue {
                          N = Convert.ToString(book.Id)
                      }},
                    { "Title", new AttributeValue {
                          S = book.Title
                      }},
                    { "ISBN", new AttributeValue {
                          S = book.ISBN
                      }},
                    { "Authors", new AttributeValue {
                          //L = book.BookAuthors
                      }},
                     { "ProductCategory", new AttributeValue {
                          S = book.ProductCategory
                     }}
                }
            };
            await client.PutItemAsync(request);
        }
        public async Task ReadTableItem(DynamoDBContext context, string title)
        {
            var itemsWithSpecificTitle = await context.ScanAsync<Book>(new List<ScanCondition>()
            {
                new ScanCondition("Title", ScanOperator.Contains, title)
            }).GetRemainingAsync();
            Console.WriteLine("\nReadTableItem: Printing result......");
            foreach (Book book in itemsWithSpecificTitle)
            {
                Console.WriteLine("{0}\t{1}\t{2}", book.Id, book.Title, book.ISBN);
            }
        }
        public async Task ReadTable(DynamoDBContext context)
        {
            var conditions = new List<ScanCondition>();
            // you can add scan conditions, or leave empty
            var allDocs = await context.ScanAsync<Book>(conditions).GetRemainingAsync();
            foreach (Book book in allDocs) WriteLine(book.Title);
        }
    }
}
