using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionTrees
{
    class Program
    {
        static void Main()
        {
            var books = new List<Book>
            {
                new Book("0001-1", "The Spook's", "Apprentice", "Joseph Delaney", 325),
                new Book("0002-1", "The Spook's", "Curse", "Joseph Delaney", 432),
                new Book("0003-1", "The Spook's", "Secret", "Joseph Delaney", 464),
                new Book("0004-1", "The Spook's", "Battle", "Joseph Delaney", 496),
                new Book("0005-1", "The Spook's", "Mistake", "Joseph Delaney", 448),
                new Book("0006-1", "The Spook's", "Sacrifice", "Joseph Delaney", 384),
                new Book("0007-1", "The Spook's", "Nightmare", "Joseph Delaney", 400),
                new Book("0001-2", "Wheel of Time", "Eye of the World", "Robert Jordan", 782),
                new Book("0001-3", "Jurassic Park", "Michael Crichton", 400),
                new Book("0002-3", "The Lost World", "Michael Crichton", 430),
                new Book("0001-4", "The Kane Chronicles", "The Red Pyramid", "Rick Riordan", 516),
                new Book("0002-4", "The Kane Chronicles", "The Throne of Fire", "Rick Riodan", 446),
                new Book("0003-4", "The Kane Chronicles", "The Serpent's Shadow", "Rick Riodan", 429)
            }.AsQueryable();

            Print(books);

            while (true)
            {
                Console.Write("Search: ");
                var search = Console.ReadLine();

                Filter(search, books);
            }
        }

        static void Filter(string search, IQueryable<Book> books)
        {
            search = search.ToLower();

            int number;
            var filterByNumber = int.TryParse(search, out number);

            var stringCode = search.Replace("-", "");
            int code;
            var filterByCode = stringCode.Length == 5 && int.TryParse(stringCode, out code);

            var predicate = new PredicateBuilder<Book>();

            if (filterByNumber)
                predicate &= b => b.NumerOfPages > number - 100 && b.NumerOfPages < number + 100;
            else
                predicate &= b => b.FullTitle.ToLower().Contains(search) || b.Author.ToLower().Contains(search);

            if (filterByCode)
                predicate |= b => b.Code.Replace("-", "") == stringCode;

            Print(books.Where(predicate));
        }

        //static Expression<Func<Book, bool>> ManualyCombine(Expression<Func<Book, bool>> exp1, Expression<Func<Book, bool>> exp2)
        //{
        //    return Expression.Lambda<Func<Book, bool>>(Expression.OrElse(
        //        exp1.Body,
        //        new ExpressionParameterReplacer(exp2.Parameters, exp1.Parameters).Visit(exp2.Body)
        //    ), exp1.Parameters);
        //}

        static void Print(IEnumerable<Book> books)
        {
            Console.WriteLine("----------------------------------------------------------------------------");

            books = books.ToList();

            if (!books.Any())
            {
                Console.WriteLine("No rows found..");
            }
            else
            {
                foreach (var book in books)
                    Console.WriteLine("{0} | {1} | {2} | {3}", book.Code, book.FullTitle.PadRight(42), book.Author.PadRight(16), book.NumerOfPages);
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
