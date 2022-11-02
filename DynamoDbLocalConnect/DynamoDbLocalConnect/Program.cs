using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Console;
using Amazon.DynamoDBv2.Model;
using static DynamoDbLocalConnect.PreCrud;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace DynamoDbLocalConnect
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            WriteLine("Don't forget to start DynamoDb!");
            WriteLine("Press any key to continue....");
            ReadKey();

            
            PreCrud preCrud = new PreCrud();
            CRUD crud = new CRUD();
            TableFunctions tableFunctions = new TableFunctions();
            //CreateClient
            AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
            clientConfig. ServiceURL = "http://localhost:8000";
            AmazonDynamoDBClient client = new AmazonDynamoDBClient(clientConfig);
            DynamoDBContext context = new DynamoDBContext(client);


            //check for table, if null create
            await TableCreator(client, tableFunctions);

            bool exitCheck = false;
            while (!exitCheck)
            {
                Book book;
                Write("\nSelect an item operation to perform:\n1. Create\n2. Read Item\n3. Read Table\n4. Update\n5. Delete\n6. Exit\n");
                string ?answer = ReadLine();
                string? switchAnswer;
                int choiceInt;
                bool answerCheck = int.TryParse(answer, out choiceInt);
                switch (choiceInt)
                {
                    case 1:
                        Console.WriteLine("1. Create Table Item");
                        book = PreCrud.CreateBook();
                        await context.SaveAsync(book);
                        break;
                    case 2:
                        Console.Write("2. Enter name of item for lookup: ");
                        switchAnswer = ReadLine();
                        await crud.ReadTableItem(context, switchAnswer);
                        break;
                    case 3:
                        WriteLine("Fetching table contents.....");
                        await crud.ReadTable(context);
                        break;
                    case 4:
                        Console.Write("4. Enter ID number to update: \n");
                        //show table
                        await crud.ReadTable(context);
                        
                        choiceInt = 0;
                        Int32.TryParse(ReadLine(), out choiceInt);
                        //create object of book to be updated
                        Book oldBook = await context.LoadAsync<Book>(choiceInt);
                        //create new book
                        book = PreCrud.CreateBook();
                        oldBook.Title = book.Title;
                        oldBook.ISBN = book.ISBN;
                        oldBook.ProductCategory = book.ProductCategory;
                        oldBook.BookAuthors = book.BookAuthors;
                        await context.SaveAsync(oldBook);
                        break;
                    case 5:
                        Console.WriteLine("5. Delete Table Item: ");
                        choiceInt = 0;
                        Int32.TryParse(ReadLine(), out choiceInt);
                        book = await context.LoadAsync<Book>(choiceInt);
                        await context.DeleteAsync<Book>(book);
                        break;
                    case 6:
                        exitCheck = true;
                        break;
                    default:
                        Console.WriteLine("Value does not match.");
                        break;
                }
            }

        }

        public static async Task TableCreator(AmazonDynamoDBClient client, TableFunctions tableFunctions)
        {
            string tableName = "ProductCatalog";
            WriteLine("Checking for table.....\n");
            await tableFunctions.CreateTableAsync(client, tableName);
            {
                WriteLine("Table exists. Press any key to continue.");
            }
            ReadKey();
        }
    }
}