using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Chapter2
{
    public class Reflection //Objective 2.5
    {
        public static void ExpressionTreeDemo()
        {
            BlockExpression blockExpr = Expression.Block(Expression.Call(null, typeof(Console).GetMethod("Write", new Type[] { typeof(String) }),
                Expression.Constant("Hello ")), Expression.Call(null, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(String) }),
                Expression.Constant("World!")));

            Expression.Lambda<Action>(blockExpr).Compile()();
        }
        public static void LambdaDemo()
        {
            Func<int, int, int> addFunc = (x, y) => x + y;
            Console.WriteLine(addFunc(2, 3));
        }
        public static void HelloWithCodeDOMDemo()
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();
            CodeNamespace myNamespace = new CodeNamespace("MyNamespace");
            myNamespace.Imports.Add(new CodeNamespaceImport("System"));
            CodeTypeDeclaration myClass = new CodeTypeDeclaration("MyClass");
            CodeEntryPointMethod start = new CodeEntryPointMethod();
            CodeMethodInvokeExpression cs1 = new CodeMethodInvokeExpression(new CodeTypeReferenceExpression("Console"),
                "WriteLine", new CodePrimitiveExpression("Hello World!"));

            compileUnit.Namespaces.Add(myNamespace);
            myNamespace.Types.Add(myClass);
            myClass.Members.Add(start);
            start.Statements.Add(cs1);

            //now, 2-75: create .cs file from a code compile unit
            CSharpCodeProvider provider = new CSharpCodeProvider();

            using (StreamWriter sw = new StreamWriter("HelloWorld.cs", false))
            {
                IndentedTextWriter tw = new IndentedTextWriter(sw, " ");
                provider.GenerateCodeFromCompileUnit(compileUnit, tw, new CodeGeneratorOptions());
                tw.Close();
            }
        }
        public static void MethodThroughReflectionDemo()
        {
            int i = 42;
            MethodInfo compareToMethod = i.GetType().GetMethod("CompareTo", new Type[] { typeof(int) });
            int result = (int)compareToMethod.Invoke(i, new object[] { 41 });
        }
        public static void DumpObject(object obj)
        {
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType == typeof(int))
                {
                    Console.WriteLine(field.GetValue(obj));
                }
            }
        }
        public static void AssemblyInspectionDemo()
        {
            Assembly pluginAssembly = Assembly.Load("assemblyname");

            var plugins = from type in pluginAssembly.GetTypes()
                          where typeof(IPlugin).IsAssignableFrom(type)
                          && !type.IsInterface
                          select type;

            foreach (Type pluginType in plugins)
            {
                IPlugin plugin = Activator.CreateInstance(pluginType) as IPlugin;
            }
        }

        /*[Fact]
        [UnitTest]
        public void MySecondUnitTest();*/
        public static void SpecificAttributeDemo()
        {
            ConditionalAttribute conditionalAttribute = (ConditionalAttribute)Attribute.GetCustomAttribute(typeof(ConditionalClass), typeof(ConditionalAttribute));
            string condition = conditionalAttribute.ConditionString;
            Console.WriteLine(condition);
        }
        public static void DefinedAttributeDemo()
        {
            if (Attribute.IsDefined(typeof(Person1), typeof(SerializableAttribute)))
            {
                Console.WriteLine("Person1 objects are serializable");
            }
        }
    }

    public class MyPlugin : IPlugin
    {
        public string Name
        {
            get { return "My Plugin"; }
        }

        public string Description
        {
            get { return "My Sample Plugin"; }
        }

        public bool Load(MyApplication application)
        {
            return true;
        }
    }

    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        bool Load(MyApplication application);
    }

    public class MyApplication { }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    class CompleteCustomAttribute : Attribute
    {
        public string Description { get; set; }
        public CompleteCustomAttribute(string description)
        {
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class MyMethodAndParameterAttribute : Attribute { }

    /*public class UnitTestAttribute : CategoryAttribute
    {
        public UnitTestAttribute()
            : base("Unit Test") { }
    }

    public class CategoryAttribute : TraitAttribute
    {
        public CategoryAttribute(string value)
            : base("Category", value) { }
    }*/

    class ConditionalClass
    {
        [Conditional("CONDITION1")]
        static void MyMethod() { }

        //[Fact]
        //[Trait("Category", "Unit Test")]
        public void MyUnitTest() { }

        //[Fact]
        //[Trait("Category", "Integration Test")]
        public void MyIntegrationTest() { }
    }

    //Multiple Attributes syntax
    //[Conditional(CONDITION1), Conditional("CONDITION2")]
    //static void MyMethod(){}

    [Serializable]
    class Person1
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
/*Thought experiment: You are creating your own optimized object-relational mapper. You allow the user to map types one-on-one to a table in the database. You also
 * use special attributes for security reasons. For example, a type can be decorated with an AuthorizeAttribute to make sure that only specific users
 * can access a certain table. You use a lot of reflection in your app and you start seeing some performance problems. You are also thinking about a generator that will
 * create types that map exactly to an existing database.
 * 
 * 1. Why do you use an attribute instead of inheriting from an interface? Wouldn't that be easier than adding a whole new concept to C#?
 *  Attributes can be given different values in different environments...these values can also be queried explicitly during runtime, so that's useful if you need
 *  this information.
 * 2. What can you do about the performance problems with using reflection?
 *  ???
 * 3. Which technique would you use to create a generator? Well I think I'd have to use the CodeDOM technique...
 * 
 * 1. You want to read the value of a private field on a class. Which BindingFlags do you need? (Choose all that apply.)
 *  1. Instance
 *  2. DeclaredOnly
 *  3. Static
 *  4. NonPublic <=
 *  
 * 2. You need to create an attribute that can be applied multiple times on method or parameter. Which syntax should you use?
 *  2. [AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
 *  
 * 3. You want to create a delegate that can filter a list of strings on a specific value. Which type should you use?
 *  3.Func<string, IEnumerable<string>, IEnumerable<string>>
 * 
 **/
