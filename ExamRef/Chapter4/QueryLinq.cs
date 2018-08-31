using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Chapter4
{
    public class QueryLinq
    {
        public static void TransformXMLDemo()
        {
            XElement root = XElement.Parse(xml);
            XElement newTree = new XElement("people", from p in root.Descendants("person")
                                                      let name = (string)p.Attribute("firstName") + (string)p.Attribute("lastName")
                                                      let contactDetails = p.Element("ContactDetails")
                                                      select new XElement("Person", new XAttribute("IsMale", name.Contains("John")),
                                                      p.Attributes(),
                                                      new XElement("ContactDetails", contactDetails.Element("EmailAddress"),
                                                      contactDetails.Element("PhoneNumber") ?? new XElement("PhoneNumber", "1122334455")
                                                      )));
        }
        public static void UpdateXMLProcedurallyDemo()
        {
            XElement root = XElement.Parse(xml);

            foreach (XElement p in root.Descendants("person"))
            {
                string name = (string)p.Attribute("firstName") + " " + (string)p.Attribute("lastName");
                p.Add(new XAttribute("IsMale", name.Contains("John")));
                XElement contactDetails = p.Element("contactdetails");
                if (!contactDetails.Descendants("PhoneNumber").Any())
                {
                    contactDetails.Add(new XElement("PhoneNumber", "001122334455"));
                }
            }
        }
        public static void CreateXMLWithLinq()
        {
            XElement root = new XElement("Root", new List<XElement>
            {
                new XElement("Child1"),
                new XElement("Child2"),
                new XElement("Child3"),
            },
            new XAttribute("MyAttribute", 42));
            root.Save("test.xml");
        }
        public static void QueryXMLWithWhereAndOrderByDemo()
        {
            XDocument doc = XDocument.Parse(xml);
            IEnumerable<string> personNames = from p in doc.Descendants("person")
                                              where p.Descendants("PhoneNumber").Any()
                                              let name = (string)p.Attribute("firstName") + " " + (string)p.Attribute("lastName")
                                              orderby name
                                              select name;

        }
        public static void QueryXMLWithLinqDemo()
        {
            XDocument doc = XDocument.Parse(xml);
            IEnumerable<string> personNames = from p in doc.Descendants("person") select (string)p.Attribute("firstName") + " " + (string)p.Attribute("lastName");
            foreach (string s in personNames)
            {
                Console.WriteLine(s);
            }
        }
        public static void LinqMultipleFromDemo()
        {
            int[] data1 = { 1, 2, 3 };
            int[] data2 = { 2, 4, 6 };

            var result = from d1 in data1 from d2 in data2 select d1 * d2;
            Console.WriteLine(string.Join(", ", result));
        }
        public static void LinqOrderByDemo()
        {
            int[] data = { 1, 2, 5, 8, 11 };
            var result = from d in data where d > 5 orderby d descending select d;
            Console.WriteLine(string.Join(", ", result));
        }
        public static void LinqWhereOperatorDemo()
        {
            int[] data = { 1, 2, 5, 8, 11 };
            var result = from d in data where d > 5 select d;
            Console.WriteLine(string.Join(", ", result));
        }
        public static void LinqSelectQuery0()
        {
            int[] data = { 1, 2, 5, 8, 11 };
            var result = from d in data select d;
            Console.WriteLine(string.Join(", ", result));
        }
        public static void LinqSelectQuery()
        {
            int[] data = { 1, 2, 5, 8, 11 };

            var result = from d in data
                         where d % 2 == 0
                         select d;

            foreach (int i in result)
            {
                Console.WriteLine(i);
            }
        }
        public static void CreateAnonymousTypeDemo()
        {
            var person = new
            {
                FirstName = "John",
                LastName = "Doe"
            };

            Console.WriteLine(person.GetType().Name);
        }
        public static void ExtensionMethodDemo()
        {
            int x = 2;
            Console.WriteLine(x.Multiply(3));
        }
        public static void AnonymousMethodDemo()
        {
            Func<int, int> myDelegate = delegate (int x)
             {
                 return x * 2;
             };

            Console.WriteLine(myDelegate(21));
        }
        public static void UseCollectionInitDemo()
        {
            var people = new List<Person0>
            {
                new Person0
                {
                    FirstName = "John",
                    LastName = "Doe"
                },
                new Person0
                {
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            };
        }
        public static void UseObjectInitDemo()
        {
            Person0 p = new Person0()
            {
                FirstName = "John",
                LastName = "Doe"
            };
        }
        public static void CreateAndInitDemo()
        {
            Person0 p = new Person0();
            p.FirstName = "John";
            p.LastName = "Doe";
        }
        private static string xml = @"<?xml version=""1.0"" encoding=""utf-8"" ?>
                                <people> 
                                    <person firstName=""John"" lastName=""Doe"">
                                        <contactdetails>
                                            <emailaddress>john@unknown.com</emailaddress>
                                        </contactdetails>
                                    </person>
                                    <person firstName=""Jane"" lastName=""Doe"">
                                        <contactdetails>
                                            <emailaddress>jane@unknown.com</emailaddress>
                                            <phonenumber>001122334455</phonenumber>
                                        </contactdetails>
                                    </person>
                                </people>";
    }

    internal class TSource
    {
    }

    public class Product
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
    }

    public class OrderLine
    {
        public int Amount { get; set; }
        public Product product { get; set; }
    }

    public class Order
    {
        public List<OrderLine> OrderLines { get; set; }
    }

    public class Person0
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public static class IntExtensions
    {
        public static int Multiply(this int x, int y)
        {
            return x * y;
        }
    }
    public static class LinqExtensions
    {
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (TSource item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }
    }

    /*Examples. Book disconnected on some stuff. I decided to write the examples here rather than try to approach their omissions
     * 
     * Example 4 - 60 using group by and projection
     * 
     * var result = from o in orders from l in o.OrderLines group l by l.Product into p select new
     *  {Product = p.Key,
     *      Amount = p.Sum(x=>x.Amount)
     *   };
     *   
     * Example 4-61 using join
     * string[] popularProductNames = { "A", "B" };
     * var popular products = from p in products join n in popularProductNames on p.Description equals n select p;
     * 
     * Example 4-62 Using skip and take
     * 
     * var pagedOrders = orders.Skip((pageIndex - 1) * pageSize).Take(pageSize);
     * */

    /*
     * Thought Experiment:
     * 
     * You are starting a new project in which you can use LINQ for the first time. You have never worked with LINQ before, but
     * you have studied it on your own time and you see its advantages. You see possibilities for using LINQ Entities, LINQ to
     * objects, and LINQ to XML in your projects and you try to introduce them to your company
     * 
     * Some of your coworkers are having some doubts. Will LINQ be fast enough? Is it easy to maintain? Do we need to use the method or query syntax?
     * Answer these questions:
     * 
     * 1. Does LINQ have any performance problems? If so, should it be avoided? Yes, it has performance problems, but it makes coding much easier, so don't avoid.
     * 2. Is LINQ easy to maintain? Not from what I've seen, especially where more complex queries are concerned. However, it bets hardocded sql.
     * 3. What are the differences between method and query syntax? Which should you use? Method syntax is more verbose. Try to use query syntax when possible.
     * 
     * Review:
     * 1.You have a list of dates and want to filter the dates to the current year and then select the highest date. Which query do you use?
     *  3
     * 2. You are trying to use a LINQ query, but you are getting a compile error that the Where method cannot be found. What should you do?
     *  1. Add a using System.Lionq statement
     *  3. You are using the following LINQ to entities query:
     *  suffering performance problems. How can you improve? Don't execute ToList() on query
     *  
     * 
     * */
}
