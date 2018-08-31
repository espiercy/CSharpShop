using System;
using System.Collections.Generic;
using System.Text;

namespace Chapter4
{
    public class StoreAndRetrieveData
    {
        public static void UsingCollectionDemo()
        {
            Person5 p1 = new Person5
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 42
            };

            Person5 p2 = new Person5
            {
                FirstName = "Jane",
                LastName = "Doe",
                Age = 42
            };

            PeopleCollection people = new PeopleCollection { p1, p2 };
            people.RemoveByAge(42);
            Console.WriteLine(people.Count);
        }
        public static void StackDemo()
        {
            Stack<string> myStack = new Stack<string>();
            myStack.Push("Hello");
            myStack.Push("World");
            myStack.Push("From");
            myStack.Push("A");
            myStack.Push("Queue");

            foreach (string s in myStack)
                Console.Write(s + " ");
        }
        public static void QueueDemo()
        {
            Queue<string> myQueue = new Queue<string>();
            myQueue.Enqueue("Hello");
            myQueue.Enqueue("World");
            myQueue.Enqueue("From");
            myQueue.Enqueue("A");
            myQueue.Enqueue("Queue");

            foreach (string s in myQueue)
                Console.Write(s + " ");
        }
        public static void useHashSet()
        {
            HashSet<int> oddSet = new HashSet<int>();
            HashSet<int> evenSet = new HashSet<int>();

            for(int x =1; x <= 10; x++)
            {
                if (x % 2 == 0) evenSet.Add(x);
                else oddSet.Add(x);
            }

           
        }

        private static void DisplaySet(HashSet<int> set)
        {
            Console.Write("{");
            foreach(int i in set)
            {
                Console.Write(" {0}", i);
            }
            Console.WriteLine(" }");
        }

        public static void UsingDictionaryDemo()
        {
            Person p1 = new Person { Id = 1, Name = "Name1" };
            Person p2 = new Person { Id = 2, Name = "Name2" };
            Person p3 = new Person { Id = 3, Name = "Name3" };

            var dict = new Dictionary<int, Person>();
            dict.Add(p1.Id, p1);
            dict.Add(p2.Id, p2);
            dict.Add(p3.Id, p3);

            foreach (KeyValuePair<int, Person> v in dict) Console.WriteLine("{0}: {1}", v.Key, v.Value.Name);

            dict[0] = new Person { Id = 4, Name = "Name4" };

            Person result;
            if (!dict.TryGetValue(5, out result)) Console.WriteLine("No person with a key of 5 can be found");
        }
        public static void ListDemo()
        {
            List<string> listOfStrings = new List<string> { "A", "B", "C", "D", "E" };
            for (int x = 0; x < listOfStrings[x].Length; x++) Console.WriteLine(listOfStrings[x]);
            listOfStrings.Remove("A");
            Console.WriteLine(listOfStrings[0]);
            listOfStrings.Add("F");
            Console.WriteLine(listOfStrings.Count);
            bool hasC = listOfStrings.Contains("C");
            Console.WriteLine(hasC);
        }
        public static void JaggedArrayDemo()
        {
            int[][] jaggedArray =
            {
                new int[] {1,3,5,7,9},
                new int[] {0,2,4,6},
                new int[] {42,21}
            };
        }
        public static void TwoDimensionalArrayDemo()
        {
            string[,] array2D = new string[3, 2] { { "one", "two" }, { "three", "four" }, { "five", "six" } };
            Console.WriteLine(array2D[0, 0]);
            Console.WriteLine(array2D[0, 1]);
            Console.WriteLine(array2D[1, 0]);
            Console.WriteLine(array2D[1, 1]);
            Console.WriteLine(array2D[2, 0]);
            Console.WriteLine(array2D[2, 1]);
        }
        public static void UseArrayDemo()
        {
            int[] arrayOfInt = new int[10];
            for (int x = 0; x < arrayOfInt.Length; x++)
            {
                arrayOfInt[x] = x;
            }

            foreach (int i in arrayOfInt) Console.WriteLine(i);
        }
    }

    public class PeopleCollection : List<Person5>
    {
        public void RemoveByAge(int age)
        {
            for(int index = this.Count - 1; index >= 0; index--)
            {
                if (this[index].Age == age) this.RemoveAt(index);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Person5 p in this)
            {
                sb.AppendFormat("{0} {1} is {2}", p.FirstName, p.LastName, p.Age);
            }
            return sb.ToString();
        }
    }

    public class Person5
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}

/*
 Listings for IList<T> and ICollection<T>
     public interface IList<T>  : ICollection<T>, IEnumerable<T>, IEnumerable
     {
        T this[int index]{get; set;}
        int IndexOf(T item);
        void Insert(int index, T item);
        void RemoveAt(int index);
     }

    public interface ICollection<T> : IEnumerable<T>,
    IEnumerable
    {
    
        int Count { get; }
        bool IsReadOnly { get; }
        void Add(T item);
        void Clear();
        bool Contains(T item);
        void CopyTo(T[] array, int arrayIndex);
        bool Remove(T item);
    }
     
    Dictionaries class sig
    public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey,TValue>>, IDictionary, ICollection,
        IReadOnlyDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable,
        ISerializable, IDeserializationCallback
     
    ISet<T> interface:

    public interface ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        bool Add(T item);
        void ExceptWith(IEnumerable<T> other);
        void IntersectWith(IEnumerable<T> other);
        bool IsProperSubsetOf(IEnumerable<T> other);
        bool IsProperSuperSetOf(IEnumberable<T> other):
        bool IsSubsetOf(IEnumerable<T> other);
        bool IsSuperSetOf(IEnumerable<T> other);
        bool Overlaps(IEnumerable<T> other);
        bool SetEquals(IEnumerable<T> other);
        void SymmetricExceptWith(IEnumerable<T> other);
        void UnionWith(IEnumerable<T> other);
    }

    Thought experiment: You are trying to determine the different collections that .NET uses. You try to come up with a comparison of the different collection types
    by performance and use case.

    1. When should you use a generic or nongeneric collection? Use a generic colleciton for flexibility. Use a nongeneric collection when you know the type that's going
    to be used will not change. Heavily favor generics.

    2. What's the difference between the Dictionary and List-based collections? When should you use one or the other?
        Dictionaries are series of key-value pairs. Use dictionaries for read efficiency. Use lists if you need to store duplicate items in the collection.
    3. What's the difference between the Stack / Queue / List collections?
        Stack is lifo, Queue is fifo. You can always access any item in a list.

    Objective review: 
        1. You want to store a group of orders and make sure that a user can easily select an order by its order number. Which collection do you use?
            2. Dictionary
        2. You are using a queue and you want to add a new item. Which method?
            4. Enqueue
        3. You are working with a large group of family name objects. You need to remove all duplications and then group them by last name. Which collections should you use?
            1 List<T> and 3. Dictionary<T>
     
     */
