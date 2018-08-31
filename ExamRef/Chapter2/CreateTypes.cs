using System;
using System.Collections.Generic;

namespace Chapter2
{
    public class CreateTypes //2.1
    {
        public static void DeclaringAndUsingFieldDemo()
        {
            MyClass instance = new MyClass();
            instance.MyInstanceField = "Some New Value";
        }

        public static void MethodWithoutAnyReturnValue()
        {

        }
        public static int MethodWithReturnValue()
        {
            return 42;
        }
        public static void NamedAndOptionalArgumentsDemo()
        {
            MyMethod(1, thirdArgument: true);
        }
        private static void MyMethod(int firstArgument, string secondArgument = "default value", bool thirdArgument = false) { }
        public Distance CalculateDistanceTo(Address address)
        {
            Distance result = new Distance(); //actually perform some calc on address <= preferred

            return result;
        }
        public Distance CaclulateDistanceTo(Customer customer)
        {
            Distance result = new Distance(); //actually perform some calc on customer.Address

            return result;
        }
        public static void MethodDemo()
        {
            Console.WriteLine("I'm calling a method!");
        }
        public static void FlagAttributeDemo()
        {
            Days readingDays = Days.Monday | Days.Saturday;
        }
    }

    class Derived2 : Derived0
    {
        //would give compiler error
        //public override int MyMethod(){ return 1; }
    }

    class Derived0 : Base
    {
        public sealed override int MyMethod()
        {
            return base.MyMethod() * 2;
        }
    }

    class Derived : Base
    {
        public override int MyMethod()
        {
            return base.MyMethod() * 2;
        }
    }

    class Base
    {
        public virtual int MyMethod()
        {
            return 42;
        }
    }

    public class Calculator
    {
        public decimal CalculateDiscount(Product p)
        {
            return p.Discount();
        }
    }

    public class Product
    {
        public decimal Price { get; set; }
    }

    public static class MyExtensions
    {
        public static decimal Discount(this Product product)
        {
            return product.Price * .9M;
        }
    }

    public class MyClass<T> where T : class, new()
    {
        public MyClass()
        {
            MyProperty = new T();
        }
        T MyProperty { get; set; }
    }

    struct Nullable<T> where T : struct
    {
        private bool hasValue;
        private T value;


        public Nullable(T value)
        {
            this.hasValue = true;
            this.value = value;
        }

        public bool HasValue { get { return this.hasValue; } }

        public T Value
        {
            get
            {
                if (!this.HasValue) throw new ArgumentException();
                return this.value;
            }
        }

        public T GetValueOrDefault()
        {
            return this.value;
        }
    }

    public class ConstructorChaining
    {
        private int _p;

        public ConstructorChaining() : this(3) { }
        public ConstructorChaining(int p)
        {
            this._p = p;
        }
    }

    class Card { }

    class Deck
    {
        private int _maximumNumberOfCards;

        public List<Card> Cards { get; set; }

        public Deck(int maximumNumberOfCards)
        {
            _maximumNumberOfCards = maximumNumberOfCards;
            Cards = new List<Card>();
        }

    }

    class Deck0
    {
        public ICollection<Card> Cards { get; private set; }
    }

    public class MyClass
    {
        public string MyInstanceField;

        public string Concatenate(string valueToAppend)
        {
            return MyInstanceField + valueToAppend;
        }
    }

    public class Address
    {

    }

    public class Customer
    {
        public Address Address { get; set; }
    }

    public class Distance { }

    public class Calculator0
    {
        public int Add(int x, int y)
        {
            return x + y;
        }
    }
    public struct Point
    {
        public int x, y;

        public Point(int p1, int p2)
        {
            x = p1;
            y = p2;
        }
    }

    [Flags]
    enum Days
    {
        None = 0x0,
        Sunday = 0x1,
        Monday = 0x2,
        Tuesday = 0x4,
        Wednesday = 0x8,
        Thursday = 0x10,
        Friday = 0x20,
        Saturday = 0x40
    }
}
/*
 * Thought Exp: You are tasked with creating the basic types for a new web shop. As a customer, you can search through the existing product database and
 * compare different items by reviewing specifications and reviews from other users. The system should keep track of popular products and make recommendations
 * to the customer. Of course, the customer can then select the products he wants and place an order. There are also some business rules that you need to be
 * aware of. A new customer is not allowed to place an order that exceeds $500. An order should be at least $10 to qualify for free shipping. More busines
 * rules will be added, but are not clear at the moment.
 * 
 * 1. Which basic types are you going to use to build your web shop? Reference types (???)
 * 2. How can you make sure that your types contain both behavior and data? By including both values and methods in them
 * 3. How can you improve the usability of your types? By making sure that they are extensible in the future.
 * 
 * 1. You are creating a new collection type and you want to make sure the elements in it can be easily accessed. What should you add to the type?
 *      Indexer property.
 * 2. You are creating a generic class that should work only with reference types. Which type constraint should you add?
 *      where T:class
 * 3. You pass a struct variable into a method as an argument. The method changes the variable; however, when the method returns, the variable has not changed.
 *      What happened?
 *          Passing a value type makes a copy of the data. The original wasn't changed.
 */
