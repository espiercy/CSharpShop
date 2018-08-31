using System;

namespace Chapter2
{
    class EnforceEncapsulation
    {
    }

    class MoveableObject : ILeft, IRight
    {
        void ILeft.Move() { }
        void IRight.Move() { }
    }
    interface ILeft
    {
        void Move();
    }

    interface IRight
    {
        void Move();
    }

    interface IInterfaceA
    {
        void MyMethod();
    }

    class Implementation : IInterfaceA
    {
        void IInterfaceA.MyMethod() { }
    }
    /*public interface IObjectContextAdapter
    {
        ObjectContext ObjectContext { get; }
    }*/


    public class Person
    {
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();
                _firstName = value;
            }
        }
    }

    public class EncapWithCustomMethods
    {
        private int _field;
        public void SetValue(int value) { _field = value; }
        public int GetValue() { return _field; }
    }

    internal class MyInternalClass
    {
        public void MyMethod() { }
    }

    public class Derived1 : Base0
    {
        public void MyDerivedMethod()
        {
            // _privateField = 41; not ok

            _protectedField = 43;

            //MyPrivateMethod();

            MyProtectedMethod(); //OK, can be accessed
        }
    }

    public class Base0
    {
        private int _privateField = 42;
        protected int _protectedField = 42;

        private void MyPrivateMethod() { }
        protected void MyProtectedMethod() { }
    }
    public class Dog
    {
        public void Bark() { }
    }

    public class Accessibility0
    {
        private string[] _myField;

        public string MyProperty
        {
            get { return _myField[0]; }
            set { _myField[0] = value; }
        }
    }

    public class Accessibility
    {
        private string _myField;

        public string MyProperty
        {
            get { return _myField; }
            set { _myField = value; }
        }
    }


}

/*
 * You are working with a team to create a new application for keeping track of the inventory of a chemistry lab. Your application consists of lots
 * of types that all participate in making sure that items are not used by unqualified personnel,
 * dangerous chemicals are not used in the same room, and items are ordered as soon as the stock
 * is running low. Currently, all types in the system have different access modifiers. Some types are completely immutable; others expose all their data
 *You are discussing the current problems with a colleague. He argues that all types and all members should be public.
 * 
 * 1. Explain why making all members public isn't the solution: This would allow users to modify data that shouldn't be modified.
 * 2. Give an example of how a property can help with encapsulating data while still improving usability: allows you to control access to a field and update
 * it accordingly.
 * 3. How can interfaces be used to improve the design? If different classes can't descend from the same class but still have comparable functionality interfaces
 * can be used to enforce this commonality.
 * 
 * Review: 
 * 1. What access modifier should you use to make sure that a method in a class can only be accessed inside the same assembly by derived types.
 *      3. Class interal and members internal.
 * 2. You need to expose some data from a class. The data can be read by other types but can be changed only by derived types. A protected property with a public get modifier
 * 3. You have a class that implements two interfaces that both have a method with the same name. Interface IA should be the defaul implementation. Interface IB should be used
 * only in special situations. How do you implement?
 *  IA implicit, IB explicit.
 * */
