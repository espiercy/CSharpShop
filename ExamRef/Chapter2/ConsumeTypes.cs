using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Net.Http;

namespace Chapter2
{
    public class ConsumeTypes //obj 2.2
    {

        /*public ActionResult Index()
        {
            ViewBag.MyDynamicValue = "This property is not statically type";
            return View();
        }*/
        public static void DynamicObjectDemo()
        {
            dynamic obj = new SampleObject();
            Console.WriteLine(obj.SomeProperty);
        }
        static void DisplayInExcelCont()
        {
            var entities = new List<dynamic>
            {
                new
                {
                    ColumnA = 1,
                    ColumnB = "Foo"
                },
                new
                {
                    ColumnA = 2,
                    ColumnB = "Bar"
                }
            };

            //DisplayInExcel(entities);
        }
        static void DisplayInExcel(IEnumerable<dynamic> entities)
        {
            /*var excelApp = new Excel.Application();
            excelApp.Visible = true;

            excelApp.Workbooks.Add();

            dynamic workSheet = excelApp.ActiveSheet;

            workSheet.Cells[1, "A"] = "Header A";
            workSheet.Cells[1, "B"] = "Header B";

            var row = 1;

            foreach (var entity in entities)
            {
                row++;
                workSheet.Cells[row, "A"] = entity.ColumnA;
                workSheet.Cells[row, "B"] = entity.ColumnB;
            }

            workSheet.Columns[1].AutoFit();
            workSheet.Columns[2].AutoFit();*/
        }
        void LogStream(Stream stream)
        {
            MemoryStream memoryStream = stream as MemoryStream;
            if (memoryStream != null)
            {
                //do something, more efficient if you want to use value afterward
            }
        }
        void OpenConnection(DbConnection connection)
        {
            if (connection is SqlConnection)
            {
                //do something special, use if you just want to check value
            }
        }
        public static void ConvertAndParseDemo()
        {
            int value = Convert.ToInt32("42");
            value = int.Parse("42");
            bool success = int.TryParse("42", out value);
        }
        public static void ExplicitImplicitDemo()
        {
            Money m = new Money(42.42M);
            decimal amount = m;
            int truncatedAmount = (int)m;
        }
        public static void ExplicitConversion2Demo()
        {
            Object stream = new MemoryStream();
            MemoryStream memoryStream = (MemoryStream)stream;
        }
        public static void ExplicitConversionDemo()
        {
            double x = 1234.7;
            int a;
            a = (int)x; //1234 lossy
        }
        public static void ImplicitConversion2Demo()
        {
            HttpClient client = new HttpClient();
            object o = client;
            //IDisposable d = client;
        }
        public static void ImplicitConversionDemo()
        {
            int i = 42;
            double d = i;
        }
        public static void BoxingDemo()
        {
            int i = 42;
            object o = i;
            int x = (int)o;
        }
    }
    public class SampleObject : DynamicObject
    {
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = binder.Name;
            return true;
        }
    }
    public class Money
    {
        public decimal Amount { get; set; }
        public Money(decimal amount)
        {
            Amount = amount;
        }

        public static implicit operator decimal(Money money)
        {
            return money.Amount;
        }

        public static explicit operator int(Money money)
        {
            return (int)money.Amount;
        }
    }
}
/*
 * Thought experiment: You are developing a reusable library for doing complex calculations. Your application is gaining popularity, but you are starting to
 * hear some negative responses. Some say that your types cannot be used easily. When displaying the end results of calculations to the end user, there is
 * a lot of manual work involved. Others experience performance problems and want you to do something about it. You started developing your application
 * with C# 1.0, and your application uses ArrayLists to keep track of all the parameters needed for the calculations.
 * 
 * Your parameters are implemented as a struct. Your algorithms are implemented in a class hierarchy and you often need to cast a base type to a derived type.
 * Because this isn't always possible, you have added a lot of try/catch statements to recover from errors. Answer the following questions:
 * 
 * 1. How can a generic collection improve performance? By ensuring that all types in the collection are the same.
 * 2. Is there anything you can do to avoid the exceptions when converting between types? Yes, use is/using as
 * 3. How can you ensure your type is better converted to the basic CLR types? I DO NOT UNDERSTAND THIS QUESTION, but use dynamic keyword???
 * 
 * Review:
 * 
 * 1.You are creating a custom Distance class. You want to ease the conversion from your Distance class to a double. What should you add? Implicit operators.
 * 2.You want to determine whether the value of an object reference is derived from a particular type. Which C# language feature can you use? (Choose all that apply)
 * You can use an as operator, an is operator, or an implicit cast.
 * 
 * 3. You are using an ArrayList as a collection for a list of Points, which are a custom struct. You are experiencing performance problems when working with
 * a large amount of Points. Use a generic collection instead of ArrayList.
 */
