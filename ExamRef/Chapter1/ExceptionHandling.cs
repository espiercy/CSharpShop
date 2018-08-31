using System;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;

namespace Chapter1
{
    public class ExceptionHandling //objective 1.5
    {
        public static void ExceptionDispatchInfoDemo()
        {
            ExceptionDispatchInfo possibleException = null;

            try
            {
                string s = Console.ReadLine();
                int.Parse(s);
            }
            catch (FormatException ex)
            {
                possibleException = ExceptionDispatchInfo.Capture(ex);
            }

            if (possibleException != null)
            {
                possibleException.Throw();
            }
        }
        public static void ThrowNewExceptionDemo()
        {
            try
            {
                int i = 2 + 2;
                i++;
            }
            catch (Exception logEx)
            {
                Console.WriteLine(logEx.Message);
                throw new NullReferenceException("Made up Error for demo", logEx);
            }
        }
        public static void RethrowingExceptionDemo()
        {
            try
            {
                int i = 2 + 2;
                i++;
            }
            catch (Exception logEx)
            {
                Console.WriteLine(logEx.Message);
                throw;
            }
        }
        public static void ThrowExceptionDemo(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException("fileName", "Filename is required");
        }
        public static void ExceptionInspectionDemo()
        {
            try
            {
                int i = ReadAndParse();
                Console.WriteLine("Parsed: {0}", i);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Message: {0}", e.Message);
                Console.WriteLine("StackTace: {0}", e.StackTrace);
                Console.WriteLine("HelpLink: {0}", e.HelpLink);
                Console.WriteLine("InnerException: {0}", e.InnerException);
                Console.WriteLine("TargetSite: {0}", e.TargetSite);
                Console.WriteLine("Source: {0}", e.Source);

            }
        }

        private static int ReadAndParse()
        {
            string s = Console.ReadLine();
            int i = int.Parse(s);
            return i;
        }
        public static void FailFastDemo()
        {
            string s = Console.ReadLine();

            try
            {
                int i = int.Parse(s);
                if (i == 42) Environment.FailFast("Special number entered");
            }
            finally
            {
                Console.WriteLine("Program complete.");
            }
        }
        public static void FinallyDemo()
        {
            string s = Console.ReadLine();

            try
            {
                int i = int.Parse(s);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("You need to enter a value");
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not a valid number. Please try again.", s);
            }
            finally
            {
                Console.WriteLine("Program Complete.");
            }
        }
        public static void CatchDifferentExceptionsDemo()
        {
            string s = Console.ReadLine();

            try
            {
                int i = int.Parse(s);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("You need to enter a value");
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not a valid number. Please try again.", s);
            }
        }
        public static void CatchFormatExceptionDemo()
        {
            while (true)
            {
                string s = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(s)) break;

                try
                {
                    int i = int.Parse(s);
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not a valid number. Please try again.", s);
                }
            }
        }
        public static void InvalidNumberParseDemo()
        {
            string s = "NaN";
            int i = int.Parse(s);
        }
    }

    public class OrderProcessingException : Exception, ISerializable
    {
        public OrderProcessingException(int orderId)
        {
            OrderId = orderId;
            this.HelpLink = "http://www.mydomain.com/infoaboutexception";
        }

        public OrderProcessingException(int orderId, string message) : base(message)
        {
            OrderId = orderId;
            this.HelpLink = "http://www.mydomain.com/infoaboutexception";
        }

        public OrderProcessingException(int orderId, string message, Exception innerException) : base(message, innerException)
        {
            OrderId = orderId;
            this.HelpLink = "http://www.mydomain.com/infoaboutexception";
        }

        protected OrderProcessingException(SerializationInfo info, StreamingContext context)
        {
            OrderId = (int)info.GetValue("OrderId", typeof(int));
        }

        public int OrderId { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("OrderId", OrderId, typeof(int));
        }
    }

    /*
     Thought experiment: You are designing a new application and you want to implement a proper error-handling strategy. You are discussing the topic with some
     colleagues, and one of them says that you should use regular error codes to signal errors because that's faster and have used it in the past.

    You are also having a discussion about when to create a custom exception and when o use the built-in .NET Framework exceptions.

    1. Explain to your colleague the advantages of Exceptions compared to error codes: They are more specific and contain much more detailed information about the error.
    2. When should you create a custom exception? When you want to signal useful information for an error in a more meaningful way than the .NET framework's.

    Review
    1. You are checking the arguments of your method for illegal null values. If you encounter a null value, which exception do you throw?
        NullReferenceException.
    2. Your code catches an IOException when a file cannot be accessed. You want to give more information to the caller of your code. What do you do?
        Throw a new exception with extra information that has the IOException as the InnerException.
    3. You are creating a custom exception called LogonFailedException. Which constructors should you at least add?
        2. LogonFailed(string message)
        3. LogonFailed(string message, Exception innerException)
        4. LoginFailed(Exception innerException)
     */
}
