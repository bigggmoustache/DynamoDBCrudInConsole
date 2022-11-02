using static System.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace DynamoDbLocalConnect
{
    public class PreCrud
    {
        public static Book CreateBook()
        {
            WriteLine("Enter the following Book attributes:");
            Book book = new Book();
            Write("ID number: ");
            int idNum;
            int.TryParse(ReadLine(), out idNum);
            book.Id = idNum;
            Write("\nTitle: ");
            book.Title = ReadLine();
            Write("\nISBN (ex: 11-11-11-11): ");
            book.ISBN = ReadLine();
            Write("\nProduct Category: ");
            book.ProductCategory = ReadLine();
            Write("\nAuthor(s): ");
            string authors = ReadLine();
            List<string> authorsList = new List<string>();
            authorsList.Add(authors);
            book.BookAuthors = authorsList;
            return book;
        }

        
    }
}
