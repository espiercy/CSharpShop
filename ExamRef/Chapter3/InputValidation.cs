using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Chapter3
{
    public class InputValidation
    {
        public static void ValidateXML()
        {
            string xsdPath = "Person.xsd";
            string xmlPath = "Person.xml";

            XmlReader reader = XmlReader.Create(xmlPath);
            XmlDocument document = new XmlDocument();
            document.Schemas.Add("", xsdPath);
            document.Load(reader);

            ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }
        }
        public static void DeserializeJsonObject(string json)
        {
            var serializer = new JavaScriptSerializer();
            var result = serializer.Deserialize<Dictionary<string, object>>(json);
        }
        public static bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                || input.StartsWith("[") && input.EndsWith("]");
        }
        public static void RemoveWhiteSpaceWithRegexDemo()
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex(@"[ ]{2,}", options);

            string input = "1 2 3 4  5";
            string result = regex.Replace(input, " ");
            Console.WriteLine(result);
        }
        static bool RegexValidateZipCode(string zipCode)
        {
            Match match = Regex.Match(zipCode, @"^[1-9][0-9]{3}\s?[a-zA-Z]{2}$", RegexOptions.IgnoreCase);
            return match.Success;
        }
        static bool ManuallyValidateZipCode(string zipCode)
        {
            //Valid zipcodes: 1234AB | 1234 AB | 1001 AB
            if (zipCode.Length < 6) return false;

            string numberPart = zipCode.Substring(0, 4);
            int number;
            if (!int.TryParse(numberPart, out number)) return false;
            string characterPart = zipCode.Substring(4);
            if (numberPart.StartsWith("0")) return false;
            if (characterPart.Trim().Length < 2) return false;
            if (characterPart.Length == 3 && characterPart.Trim().Length != 2) return false;
            return true;
        }
        public static void ConvertDoubleToInt()
        {
            double d = 21.15;
            int i = Convert.ToInt32(d);
            Console.WriteLine(i); //Displays 23
        }
        public static void ConvertDemo()
        {
            int i = Convert.ToInt32(null);
            Console.WriteLine(i); // Displays 0
        }
        public static void ConfigurationOptionsParseDemo()
        {
            CultureInfo english = new CultureInfo("En");
            CultureInfo dutch = new CultureInfo("Nl");
            string value = "€19,95";
            decimal d = decimal.Parse(value, NumberStyles.Currency, dutch);
            Console.WriteLine(d.ToString(english));
        }
        public static void TryParseDemo()
        {
            string value = "1";
            int result;
            bool success = int.TryParse(value, out result);

            if (success) Console.WriteLine("Success");
            else Console.WriteLine("Failure");
        }
        public static void ParseDemo()
        {
            string value = "true";
            bool b = bool.Parse(value);
            Console.WriteLine(b); //displays true
        }
        public static void SaveToDbDemo()
        {
            using (ShopContext ctx = new ShopContext())
            {
                Address a = new Address
                {
                    AddressLine1 = "Somewhere 1",
                    AddressLine2 = "At some floor",
                    City = "SomeCity",
                    ZipCode = "1111AA"
                };

                Customer c = new Customer()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    BillingAddress = a,
                    ShippingAddress = a,
                };

                ctx.Customers.Add(c);
                ctx.SaveChanges();
            }
        }
    }

    public static class GenericValidator<T>
    {
        public static IList<ValidationResult> Validate(T entity)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(entity, null, null);
            Validator.TryValidateObject(entity, context, results);
            return results;
        }
    }

    public class ShopContext : DbContext
    {
        public IDbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Make sure the database knows how to handle the duplicate address property
            modelBuilder.Entity<Customer>().HasRequired(bm => bm.BillingAddress).WithMany().WillCascadeOnDelete(false);
        }
    }

    public class Customer
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string FirstName { get; set; }
        [Required, MaxLength(20)]
        public string LastName { get; set; }
        [Required]
        public Address ShippingAddress { get; set; }
        [Required]
        public Address BillingAddress { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string AddressLine1 { get; set; }
        [Required, MaxLength(20)]
        public string AddressLine2 { get; set; }
        [Required, MaxLength(20)]
        public string City { get; set; }
        [RegularExpression(@"^[1-9][0-9]{3}\s?[a-zA-Z]{2}$")]
        public string ZipCode { get; set; }
    }
}
/*
 * 
 * You have developed a complex web application and deployed it to production. The application is a new hybrid of wiki and a forum. Users can
 * use it to brainstorm on ideas and write a document together.
 * Suddenly users start contacting your support desk. They are all reporting that your application looks strange. It suddenly contains
 * extra URLs that link to external websites that are mixed with the original website's layout.
 * 
 * 1. What could be the problem? Sounds like incoming data is not being parsed correctly and that is creating additional html tags.
 * 2. To fix this I will add validation methods to make sure that these html elements are not being rendered directly to my page.
 * 
 * Review:
 * 
 * 1. A user needs to enter a DateTime in a text field. You need to parse the value in Code. Which method do you use?
 *      2. DateTime.TryParse
 * 2. You are working on a globalized web application. You need to parse a text field where the use enters an amount of money. Which
 * method do you use?
 *      2. decimal.TryParse(value, NumberStyles.Currency, UICulture);
 * 3. You need to validate and XML file. Use none of the options provided in the guide. None of them make any sense, except potentially RegEx, but...
 * 
 * */
