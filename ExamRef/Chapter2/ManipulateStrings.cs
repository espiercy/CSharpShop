using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Chapter2
{
    public class ManipulateStrings //objective 2.7 (?)
    {
        public static void CompositeStringFormatting()
        {
            int a = 1;
            int b = 2;
            string result = string.Format("a: {0}, b:{1}", a, b);
            Console.WriteLine(result);
        }
        public static void DateTimeFormateDemo()
        {
            DateTime d = new DateTime(2013, 4, 22);
            CultureInfo provider = new CultureInfo("en-US");
            Console.WriteLine(d.ToString("d", provider));
            Console.WriteLine(d.ToString("D", provider));
            Console.WriteLine(d.ToString("M", provider));
        }
        public static void CurrencyFormatDemo()
        {
            double cost = 1234.56;
            Console.WriteLine(cost.ToString("C", new System.Globalization.CultureInfo("en-US")));
        }
        public static void ToStringOverrideDemo()
        {
            Person2 p = new Person2("Evan", "Piercy");
            Console.WriteLine(p.ToString());
        }
        public static void StringIterationDemo()
        {
            string value = "My Custom Value";
            foreach (char c in value)
                Console.WriteLine(c);
            foreach (string word in "My sentence separated by spaces".Split(' ')) ;
        }
        public static void RegularExpressionDemo()
        {
            string pattern = "(Mr\\.? |Mrs\\.? |Miss |Ms\\.? )";
            string[] names = { "Mr. Henry Hunt", "Ms. Sara Samuels", "Abaraham Adams", "Ms. Nicole Norris", "Miss Tuffet" };
            foreach (string name in names)
            {
                Console.WriteLine(Regex.Replace(name, pattern, String.Empty));
            }

        }
        public static void SubStringDemo()
        {
            string value = "My Sample Value";
            string subString = value.Substring(3, 6);
            Console.WriteLine(subString);
        }
        public static void StartsAndEndsDemo()
        {
            string value = "<mycustominput>";
            if (value.StartsWith("<")) { }
            if (value.EndsWith(">")) { }
        }

        public static void IndexOfDemo()
        {
            string value = "My Sample Value";
            int indexOfp = value.IndexOf('p');
            int lastIndexOfm = value.LastIndexOf('m');
        }
        public static void StringReaderDemo()
        {
            var stringWriter = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                writer.WriteStartElement("book");
                writer.WriteElementString("price", "19.95");
                writer.WriteEndElement();
                writer.Flush();
            }

            string xml = stringWriter.ToString();
            //reader starts here
            var stringReader = new StringReader(xml);
            using (XmlReader reader = XmlReader.Create(stringReader))
            {
                reader.ReadToFollowing("price");
                decimal price = decimal.Parse(reader.ReadInnerXml(), new CultureInfo("en-US")); // Make sure to read the decimal part correctly.
                Console.WriteLine(price);
            }
        }
        public static void StringWriterDemo()
        {
            var stringWriter = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(stringWriter))
            {
                writer.WriteStartElement("book");
                writer.WriteElementString("price", "19.95");
                writer.WriteEndElement();
                writer.Flush();
            }

            string xml = stringWriter.ToString();
            Console.WriteLine(xml);
        }
        public static void StringBuilderLoopDemo()
        {
            StringBuilder sb = new StringBuilder(string.Empty);

            for (int i = 0; i < 10000; i++)
            {
                sb.Append("x");
            }
        }
        public static void StringBuilderDemo()
        {
            StringBuilder sb = new StringBuilder("A Initial Value");
            sb[0] = 'B';
        }
        public static void LargeMemUseDemo()
        {
            string s = string.Empty;

            for (int i = 0; i < 10000; i++)
            {
                s += "x";
            }

            Console.Write(s);
        }
    }

    class Person3
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Person3(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

        public string ToString(string format)
        {
            if (string.IsNullOrWhiteSpace(format) || format == "G") format = "FL";
            format = format.ToUpperInvariant();
            switch (format)
            {
                case "FL":
                    return FirstName + " " + LastName;
                case "LF":
                    return LastName + " " + FirstName;
                case "FSL":
                    return FirstName + ", " + LastName;
                case "LSF":
                    return LastName + ", " + FirstName;
                default:
                    throw new FormatException(String.Format("The '{0}' format string is not supported.", format));
            }
        }
    }

    class Person2
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Person2(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
/*
 * Thought Experiment: You are working on a localized application to be used for time tracking. A team can use it to track time for various projects and tasks, and make sure
 * that all billable time is correct. The application consists of a web front end based on ASP.NET and a desktop application that uses C#. Suddenly, your manager
 * announces that your application that uses C#. Suddenly, your manager announces that your application is going global. You currently support only the English language;
 * you didn't take globalization into account when first architecting the application. Now you have to support Spanish and German.
 * 
 * 1. Make a list of things you have to keep in mind when updating your application for globalization:
 *  Formatting: is there a good way to return different strings to the desktop application using a ToString method that formats?
 *  Date-time presentations?
 *  
 *  Review:
 *      1. You want to display only the date portion of a DateTime according to the French culture. What method should you use?
 *          2. dt.ToString("M", new CultureInfo("fr-FR"));
 *          
 *      2. You want your type to be able to be converted from a string. Which interface should you implement?
 *          1. IFormattable
 *          
 *      3. You are parsing a large piece of text to replace values used based on some complex algorithm. Which class should you use?
 *          2. StringBuilder
 * 
 * 
 * */
