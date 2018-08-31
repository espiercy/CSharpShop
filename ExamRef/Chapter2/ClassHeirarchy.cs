using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Chapter2
{
    public class ClassHeirarchy //obj 2.4
    {
        IAnimal animal = new Dog0();

        public static void SyntacticSugarForEachDemo()
        {
            List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 7, 9 };
            using (List<int>.Enumerator enumerator = numbers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    Console.WriteLine(enumerator.Current);
            }
        }

        public static void IComparableDemo()
        {
            List<Order0> orders = new List<Order0>
            {
                new Order0 { Created = new DateTime(2012, 12, 1) },
                new Order0 { Created = new DateTime(2012, 1, 6) },
                new Order0 { Created = new DateTime(2012, 7, 8) },
                new Order0 { Created = new DateTime(2012, 2, 20) },
            };

            orders.Sort();
        }

        public static void LiskovViolationDemo()
        {
            Rectangle rectangle = new Square();
            rectangle.Width = 10;
            rectangle.Height = 5;
            Console.WriteLine(rectangle.Area);
        }

        void MoveAnimal(IAnimal animal)
        {
            animal.Move();
        }

        public static void NewKeyWordDemo()
        {
            Base2 b = new Base2();
            b.Execute();

            b = new Derived4();
            b.Execute();
        }
    }

    interface IDisposable0
    {
        void Dispose();
    }

    public class People : IEnumerable<Person0>
    {
        Person0[] people;
        public People(Person0[] people)
        {
            this.people = people;
        }

        public IEnumerator<Person0> GetEnumerator()
        {
            for (int index = 0; index < people.Length; index++)
            {
                yield return people[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class Person0
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Person0(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }

    public class Order0 : IComparable
    {
        public DateTime Created { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            Order0 o = obj as Order0;
            if (o == null)
            {
                throw new ArgumentException("Object is not an Order");
            }

            return this.Created.CompareTo(o.Created);
        }
    }


    public interface IComparable0
    {
        int CompareTo(object obj);
    }

    class Square : Rectangle
    {
        public Square() { }
        public Square(int size)
            : base(size, size)
        {

        }

        public override int Width
        {
            get
            {
                return base.Width;
            }

            set
            {
                base.Width = value;
                base.Height = value;
            }
        }

        public override int Height
        {
            get
            {
                return base.Height;
            }

            set
            {
                base.Width = value;
                base.Height = value;
            }
        }
    }

    class Rectangle
    {
        public Rectangle() { }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Area
        {
            get { return Height * Width; }
        }

        public virtual int Height { get; set; }
        public virtual int Width { get; set; }
    }

    class Derived5 : Base3
    {
        public override void AbstractMethod()
        {
            throw new NotImplementedException();
        }
    }

    abstract class Base3
    {
        public virtual void MethodWithImplementation() { }

        public abstract void AbstractMethod();
    }

    class Derived4 : Base2
    {
        public new void Execute()
        {
            Console.WriteLine("Derived.Execute");
        }
    }

    class Base2
    {
        public void Execute()
        {
            Console.WriteLine("Base.Execute");
        }
    }

    class Derived3 : Base1
    {
        protected override void Execute()
        {
            Log("Before Executing");
            base.Execute();
            Log("After Executing");
        }

        private void Log(string message) { }
    }

    class Base1
    {
        protected virtual void Execute() { }
    }

    class OrderRepository : Repository<Order>
    {
        public OrderRepository(IEnumerable<Order> orders)
            : base(orders) { }

        public IEnumerable<Order> FilterOrdersOnAmount(decimal amount)
        {
            List<Order> result = null;
            return result;
        }
    }

    class Order : IEntity
    {
        public int Id { get; }
    }
    class Repository<T> where T : IEntity
    {
        protected IEnumerable<T> _elements;

        public Repository(IEnumerable<T> elements)
        {
            _elements = elements;
        }

        public T FindById(int id)
        {
            return _elements.SingleOrDefault(e => e.Id == id);
        }
    }

    interface IEntity
    {
        int Id { get; }
    }

    class Dog0 : IAnimal
    {
        public void Move() { }
        public void Bark() { }
    }

    interface IAnimal
    {
        void Move();
    }

    interface IRepository<T>
    {
        T FindById(int id);
        IEnumerable<T> All();
    }

    interface IReadOnlyInterface
    {
        int Value { get; }
    }

    struct ReadAndWriteImplementation : IReadOnlyInterface
    {
        public int Value { get; set; }
    }

    interface IExample
    {
        string GetResult();
        int Value { get; set; }

        event EventHandler ResultRetrieved;

        int this[string index] { get; set; }
    }

    class ExampleImplementation : IExample
    {
        public string GetResult()
        {
            return "result";
        }

        public int Value { get; set; }

        public event EventHandler CalculationPerformed;

        public event EventHandler ResultRetrieved;

        public int this[string index]
        {
            get
            {
                return 42;
            }
            set { }
        }
    }
}

/*
 * Thought Experiment: You are working on a brand-new web application for a real estate agent. The agent wants to display his property on a
 * website and ensure that users can easily search for it. For example, a user will be able to filter the results on location, size, property type,
 * and price. You need to create the infrastructure that uses all the selected criteria to filter the list of available houses.
 * 
 * You want to see whether you can use some of the standard interfaces from the .NET Framework to implement your infrastructure.
 * 
 * 1. Why does the .NET Framework offer some interfaces without any implementation? Wouldn't it be easier if the .NET Framework used abstract base classes?
 *      Absolutely not. Using abstract base classes to provide this functionality might be useful in some cases, but that would prevent classes using this
 *      functionality to be derived from other classes.
 *      
 * 2. Would you use interface or class inheritance to create your search criteria?
 *  Interfaces.
 *  
 *  3. IComparable for sorting/filtering would be useful. IEnumerable will also be useful for collections manipulation.
 *  
 *  Review: You want to create a hierarchy of types because you have some implementation code you want to share between all types. You also have some method
 *  signatures you want to share. What should you use?
 *      3. An Abstract class.
 *      
 *      You want to create a type that can be easily sorted. Which interface should you implement: 2. IComparable
 *      
 *      You want to inherit from an existing class and add some behavior to a method. Which steps do you have to take?
 *      
 *      1. Use the abstract keyword on the base type.
 *      2. Use the virtual keyword on the base method. <=
 *      3. Use the new keyword on the derived method.
 *      4. Use the override keyword on the derived method. <=
 * 
 * 
 * */