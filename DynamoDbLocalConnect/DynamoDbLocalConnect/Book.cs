using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace DynamoDbLocalConnect
{
    [DynamoDBTable("ProductCatalog")]
    public class Book
    {
        [DynamoDBHashKey]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string ProductCategory { get; set; }

        [DynamoDBProperty("Authors")]
        public List<string> BookAuthors { get; set; }

        [DynamoDBIgnore]
        public string CoverPage { get; set; }
    }
}