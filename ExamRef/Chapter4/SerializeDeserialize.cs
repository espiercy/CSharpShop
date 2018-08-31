using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Security.Permissions;
using System.Xml.Serialization;

namespace Chapter4
{
    public class SerializeDeserialize
    {
        public static void DataContractJsonSerializerDemo()
        {
            PersonDataContractJson p = new PersonDataContractJson
            {
                Id = 1,
                Name = "John Doe"
            };

            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(PersonDataContractJson));
                ser.WriteObject(stream, p);

                stream.Position = 0;
                StreamReader streamReader = new StreamReader(stream);
                Console.WriteLine(streamReader.ReadToEnd());

                stream.Position = 0;

                Person result = (Person)ser.ReadObject(stream);
            }
        }

        public static void DataContractDemo()
        {
            PersonDataContract p = new PersonDataContract
            {
                Id = 1,
                Name = "John Doe"
            };

            using (Stream stream = new FileStream("data.xml", FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(PersonDataContract));
                ser.WriteObject(stream, p);
            }

            using (Stream stream = new FileStream("data.xml", FileMode.Open))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(PersonDataContract));
                PersonDataContract result = (PersonDataContract)ser.ReadObject(stream);
            }
        }
        public static void BinarySerializationDemo()
        {
            Person2 p = new Person2
            {
                Id = 1,
                Name = "John Doe"
            };

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new FileStream("data.bin", FileMode.Create))
            {
                formatter.Serialize(stream, p);
            }
            using (Stream stream = new FileStream("data.bin", FileMode.Open))
            {
                Person2 dp = (Person2)formatter.Deserialize(stream);
            }
        }
        public static void DerivedSerializationDemo()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Order0), new Type[] { typeof(VIPOrder) });
            string xml;
            using (StringWriter stringWriter = new StringWriter())
            {
                Order0 order = CreateOrder();
                serializer.Serialize(stringWriter, order);
                xml = stringWriter.ToString();
            }

            using (StringReader stringReader = new StringReader(xml))
            {
                Order o = (Order)serializer.Deserialize(stringReader);
            }
        }
        private static Order0 CreateOrder()
        {
            Product0 p1 = new Product0 { ID = 1, Description = "p2", Price = 9 };
            Product0 p2 = new Product0 { ID = 2, Description = "p3", Price = 6 };

            Order0 order = new VIPOrder
            {
                ID = 4,
                Description = "Order for John Doe. Use nice giftwrap",
                OrderLines = new List<OrderLine0>
                {
                    new OrderLine0 {ID = 5, Amount = 1, Product = p1 },
                    new OrderLine0 {ID = 6, Amount = 10, Product = p2 },

                }
            };

            return order;
        }
        public static void XmlSerializerDemo()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Person1));
            string xml;
            using (StringWriter stringWriter = new StringWriter())
            {
                Person1 p = new Person1
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Age = 42
                };
                serializer.Serialize(stringWriter, p);
                xml = stringWriter.ToString();
            }

            Console.WriteLine(xml);

            using (StringReader stringReader = new StringReader(xml))
            {
                Person1 p = (Person1)serializer.Deserialize(stringReader);
                Console.WriteLine("{0} {1} is {2} years old", p.FirstName, p.LastName, p.Age);
            }
        }
    }

    public class PersonDataContractJson
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    }

    [DataContract]
    public class PersonDataContract
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        private bool isDirty = false;
    }

    [Serializable]
    public class PersonComplex : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool isDirty = false;

        public PersonComplex() { }
        protected PersonComplex(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetInt32("Value1");
            Name = info.GetString("Value2");
            isDirty = info.GetBoolean("Value3");
        }
        [System.Security.Permissions.SecurityPermission(SecurityAction.Demand,
            SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value1", Id);
            info.AddValue("Value2", Name);
            info.AddValue("Value3", isDirty);
        }
    }

    [Serializable]
    public class Person4
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [NonSerialized]
        private bool isDirty = false;

        [OnSerializing()]
        internal void OnSerializingMethod(StreamingContext context)
        {
            Console.WriteLine("OnSerializing.");
        }
        [OnSerialized()]
        internal void OnSerializedMethod(StreamingContext context)
        {
            Console.WriteLine("OnSerialized.");
        }
        [OnDeserializing()]
        internal void OnDeserializingdMethod(StreamingContext context)
        {
            Console.WriteLine("OnDeserializing.");
        }
        [OnSerialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            Console.WriteLine("OnDeserialized.");
        }
    }
    [Serializable]
    public class Person3
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [NonSerialized]
        private bool isDirty = false;
    }
    [Serializable]
    public class Person2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        private bool isDirty = false;
    }

    [Serializable]
    public class Order0
    {
        [XmlAttribute]
        public int ID { get; set; }
        [XmlIgnore]
        public bool IsDirty { get; set; }
        [XmlArray("Lines")]
        [XmlArrayItem("OrderLine")]
        public List<OrderLine0> OrderLines { get; set; }
    }

    [Serializable]
    public class VIPOrder : Order0
    {
        public string Description { get; set; }
    }

    [Serializable]
    public class OrderLine0
    {
        [XmlAttribute]
        public int ID { get; set; }
        [XmlAttribute]
        public int Amount { get; set; }
        [XmlElement("OrderedProduct")]
        public Product0 Product { get; set; }
    }

    [Serializable]
    public class Product0
    {
        [XmlAttribute]
        public int ID { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
    [Serializable]
    public class Person1
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
/*
 * You need to serialize some data to a file. The file can then be processed by another .NET application. Your data consists of personal
 * records that store important information such as names, addresses, logon creds, and contact details. You are wondering which serialization would
 * be best. You think about XML binary, JSON, or using a Data Contract.
 * 
 * 1. To which format should you serialize the data? XML Binary. It's secure.
 * 2. Which serializer should you use? ISerializable so you can use security permissions.
 * 3. Do you need to implement any specific serialization methods on your type? Yes, those associated w/ISerializable
 * 
 * Review:
 * 
 * 1. You need to store a large amount of data, and you want to do this in the most optimal way. Which serializer should you use?
 *  4. DataContractJsonSerializer
 *  
 * 2. You are serializing some sensitive data to binary format. What should you use?
 *  2. Iserializable
 *  
 * 3. You want to serialize some data to XML, and you need to make sure that a certain property is not serialzed. Which attribute should you use?
 *      4. NonSerialized.
 * 
 * 
 * */
