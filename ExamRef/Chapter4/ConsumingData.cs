using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.XPath;

namespace Chapter4
{
    public class ConsumingData
    {
        public static void XPathQueryDemo()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XPathNavigator nav = doc.CreateNavigator();
            string query = "//People/Person[@firstName='Jane']";
            XPathNodeIterator iterator = nav.Select(query);

            Console.WriteLine(iterator.Count);

            while (iterator.MoveNext())
            {
                string firstName = iterator.Current.GetAttribute("firstName", "");
                string lastName = iterator.Current.GetAttribute("lastName", "");
                Console.WriteLine("Name: {0} {1}", firstName, lastName);
            }
        }

        public static void UsingXMLDocumentDemo()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList nodes = doc.GetElementsByTagName("Person");

            //Output the names
            foreach (XmlNode node in nodes)
            {
                string firstName = node.Attributes["firstName"].Value;
                string lastName = node.Attributes["lastName"].Value;
                Console.WriteLine("Name: {0} {1}", firstName, lastName);
            }

            //start creating new node
            XmlNode newNode = doc.CreateNode(XmlNodeType.Element, "Person", "");
            XmlAttribute firstNameAttribute = doc.CreateAttribute("firstName");
            firstNameAttribute.Value = "Foo";
            XmlAttribute lastNameAttribute = doc.CreateAttribute("lastName");
            lastNameAttribute.Value = "Bar";

            newNode.Attributes.Append(firstNameAttribute);
            newNode.Attributes.Append(lastNameAttribute);

            doc.DocumentElement.AppendChild(newNode);
            Console.WriteLine("Modified xml...");
            doc.Save(Console.Out);
        }
        public static void XMLWriterDemo()
        {
            StringWriter stream = new StringWriter();
            using (XmlWriter writer = XmlWriter.Create(stream, new XmlWriterSettings() { Indent = true }))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("People");
                writer.WriteStartElement("Person");
                writer.WriteAttributeString("firstName", "John");
                writer.WriteAttributeString("laststName", "Doe");
                writer.WriteStartElement("ContactDetails");
                writer.WriteElementString("EmailAddress", "john@unknown.com");
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
            Console.WriteLine(stream.ToString());
        }
        public static void XMLReaderDemo()
        {
            using (StringReader stringReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader, new XmlReaderSettings() { IgnoreWhitespace = true }))
                {
                    xmlReader.MoveToContent();
                    xmlReader.ReadStartElement("people");
                    string firstName = xmlReader.GetAttribute("firstName");
                    string lastName = xmlReader.GetAttribute("lastName");

                    Console.WriteLine("Person: {0} {1}", firstName, lastName);
                    xmlReader.ReadStartElement("person");
                    Console.WriteLine("ContactDetails");
                    xmlReader.ReadStartElement("contactdetails");
                    string emailAddress = xmlReader.ReadContentAsString();
                    Console.WriteLine("Email address: {0}");
                }
            }
        }
        public static void WCFProxyClient()
        {
            /*ExternalService.MyServiceClient client = new ExternalService.MyService Client();
             * string result = client.DoWork("John", "Doe");
             * Console.WriteLine(result);
             * */
        }
        public static void DbContextDemo()
        {
            using (PeopleContext ctx = new PeopleContext())
            {
                ctx.People.Add(new Person() { Id = 1, Name = "John Doe" });
                ctx.SaveChanges();
            }

            using (PeopleContext ctx = new PeopleContext())
            {
                Person person = ctx.People.SingleOrDefault(p => p.Id == 1);
                Console.WriteLine(person.Name);
            }
        }
        public static void TransactionScopeDemo()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command1 =
                        new SqlCommand("INSERT INTO People ([FirstName], [LastName], [MiddleInitial]) VALUES('John', 'Doe', null)", connection);
                    SqlCommand command2 =
                        new SqlCommand("INSERT INTO People ([FirstName], [LastName], [MiddleInitial]) VALUES('Jane', 'Doe', null)", connection);
                    command1.ExecuteNonQuery();
                    command2.ExecuteNonQuery();
                }
                transactionScope.Complete();
            }
        }
        public async Task InsertRowWithParameterizedQuery()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("INSERT INTO People([FirstName], [LastName], [MiddleName]) VALUES(@ firstName, @lastName, @middleName",
                    connection);
                await connection.OpenAsync();
                command.Parameters.AddWithValue("@firstName", "John");
                command.Parameters.AddWithValue("@lastName", "Doe");
                command.Parameters.AddWithValue("@middleName", "Little");

                int numberOfInsertedRows = await command.ExecuteNonQueryAsync();
                Console.WriteLine("Inserted {0} rows", numberOfInsertedRows);
            }
        }
        public async Task UpdateRows()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UPDATE People SET FirstName='John'", connection);
                await connection.OpenAsync();
                int numberOfUpdatedRows = await command.ExecuteNonQueryAsync();
                Console.WriteLine("Updated {0} rows", numberOfUpdatedRows);
            }
        }
        public async Task SelectMultipleResultSets()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM People; SELECT TOP 1 * FROM People ORDER BY LastName", connection);
                await connection.OpenAsync();
                SqlDataReader dataReader = await command.ExecuteReaderAsync();
                await ReadQueryResults(dataReader);
                dataReader.Close();
            }
        }

        private static async Task ReadQueryResults(SqlDataReader dataReader)
        {
            while (await dataReader.ReadAsync())
            {
                string formatStringWithMiddleName = "Person ({0}) is named {1} {2} {3}";
                string formatStringWithoutMiddleName = "Person ({0}) is named {1} {3}";

                if ((dataReader["middlename"] == null))
                {
                    Console.WriteLine(formatStringWithoutMiddleName, dataReader["id"], dataReader["firstname"], dataReader["lastname"]);
                }
                else
                {
                    Console.WriteLine(formatStringWithMiddleName, dataReader["id"], dataReader["firstname"], dataReader["middlename"], dataReader["lastname"]);
                }
            }
        }
        public async Task SelectDataFromTable()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM People", connection);
                await connection.OpenAsync();

                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                while (await dataReader.ReadAsync())
                {
                    string formatStringWithMiddleName = "Person ({0}) is named {1} {2} {3}";
                    string formatStringWithoutMiddleName = "Person ({0}) is named {1} {3}";

                    if ((dataReader["middlename"] == null))
                    {
                        Console.WriteLine(formatStringWithoutMiddleName, dataReader["id"], dataReader["firstname"], dataReader["lastname"]);
                    }
                    else
                    {
                        Console.WriteLine(formatStringWithMiddleName, dataReader["id"], dataReader["firstname"], dataReader["middlename"], dataReader["lastname"]);
                    }
                }
                dataReader.Close();
            }
        }
        public static void UseExternalConnectionFile()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ProgrammingInCSharpConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
            }
        }
        public static void SqlConnectionStringBuilderDemo()
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder();

            sqlConnectionStringBuilder.DataSource = @"(localdb)\v11.0";
            sqlConnectionStringBuilder.InitialCatalog = "ProgrammingInCSharp";

            string connectionString = sqlConnectionStringBuilder.ToString();
        }
        public static void SqlConnectionDemo()
        {
            string connectionString = "not a functioning connection string";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
            }
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

    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class PeopleContext : DbContext
    {
        public IDbSet<Person> People { get; set; }
    }

    [ServiceContract]
    public class MyService
    {
        [OperationContract]
        public string DoWork(string left, string right)
        {
            return left + right;
        }
    }
}

/*4-40...
 *
 *A generated WCF proxy client example
 *
 *namespace Service.Client.ExternalService
 * {
 * 
 *  [System.CodeDom.Compiler.GeneratedCodeAttribute(<<System.ServiceModel>>, <<4.0.0.0>>)]
 *  public partial class MyServiceClient : System.ServiceModel.ClientBase<Service.Client.ExternalService.MyService>, Service.Cleint.ExternalService.MyService
 *  {
 *      public MyServiceClient(){}
 *      public MyServiceClient(string endpointConfigurationName) : base(endpointConfigurationName){}
 *      public MyServiceClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress){}
 *      public MyServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
 *          base(endpointConfigurationName, remoteAddress){}
 *      public MyServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
 *          base(binding, remoteAddress){}
 *      public string DoWork(string left, string right)
 *      {
 *          return base.Channel.DoWork(left, right);
 *      }
 *      public System.Threading.Tasks.Task<string> DoWorkAsync(
 *          string left, string right)
 *      {
 *          return base.Channel.DoWorkAsync(left, right);
 *      }
 *   }
 *}
 * */

/*Thought experiment
 You are designing a new application that stores data about weather conditions throughout the world. Your business model relies on selling this data to customers
 so they can use it in their own applications. You use a relational database to store your data, and you access your data through web services that can
 be accessed by authorized users. You also create regular XML dumps for users that want a local copy of data.

    1. What are the advantages/disadvantages, working with the Entity Framework? Good development time, slower application time.
    2. Which technologies do you plan to use for your web services? The ones that allow me to build the webservice, and deliver XML documents to the client.
    3. How will you expose your data as XML? Pull it from db using EF, convert to XML w/XMLDocument.
     
    Review:
    1. You want to update a specific row in the database, which object should you use?
        1. SqlCommand, 3. SqlConnection, 4. TransactionScope
    2. You are planning to build an application that will use an object-oriented design. It will be used by multiple users at the same time. Which
        technology should you use? 1. XML Files 2. EntityFramework, 4. Web Service
    3. You need to process a large number of XML files in a scheduled service to extract some data. Which class should you use?
        2.XmlDocument 
     
     
     */
