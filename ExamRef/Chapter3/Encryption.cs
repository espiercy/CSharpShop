using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;

namespace Chapter3
{
    public class Encryption //3.2
    {
        public static void ConvertToUnsecureString(SecureString securePassword)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                Console.WriteLine(Marshal.PtrToStringUni(unmanagedString));
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        public static void SecureStringDemo()
        {
            using (SecureString ss = new SecureString())
            {
                Console.WriteLine("Please enter password: ");
                while (true)
                {
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Enter) break;
                    ss.AppendChar(cki.KeyChar);
                    Console.Write("*");
                }
                ss.MakeReadOnly();
            }
        }
        public static void SignAndVerify()
        {
            string textToSign = "Test paragraph";
            byte[] signature = Sign(textToSign, "cn=WouterDeKort");
            Console.WriteLine(Verify(textToSign, signature));
        }

        static byte[] Sign(string text, string certSubject)
        {
            X509Certificate2 cert = GetCertificate();
            var csp = (RSACryptoServiceProvider)cert.PrivateKey;
            byte[] hash = HashData(text);
            return csp.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
        }

        static bool Verify(string text, byte[] signature)
        {
            X509Certificate2 cert = GetCertificate();
            var csp = (RSACryptoServiceProvider)cert.PublicKey.Key;
            byte[] hash = HashData(text);
            return csp.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature);
        }

        private static byte[] HashData(string text)
        {
            HashAlgorithm hashAlgorithm = new SHA1Managed();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] data = encoding.GetBytes(text);
            byte[] hash = hashAlgorithm.ComputeHash(data);
            return hash;
        }

        private static X509Certificate2 GetCertificate()
        {
            X509Store my = new X509Store("testCertStore", StoreLocation.CurrentUser);
            my.Open(OpenFlags.ReadOnly);

            var certificate = my.Certificates[0];

            return certificate;
        }
        public static void HashCalculation()
        {
            UnicodeEncoding byteConverter = new UnicodeEncoding();
            SHA256 sha256 = SHA256.Create();

            string data = "A paragraph of text";
            byte[] hashA = sha256.ComputeHash(byteConverter.GetBytes(data));

            data = "A paragraph of changed text";
            byte[] hashB = sha256.ComputeHash(byteConverter.GetBytes(data));

            data = "A paragraph of text";
            byte[] hashC = sha256.ComputeHash(byteConverter.GetBytes(data));

            Console.WriteLine(hashA.SequenceEqual(hashB));
            Console.WriteLine(hashA.SequenceEqual(hashC));
        }
        public static void KeyContainerDemo()
        {
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes("My Secret Data!");
            string containerName = "SecretContainer";
            CspParameters csp = new CspParameters() { KeyContainerName = containerName };
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(csp))
            {
                encryptedData = RSA.Encrypt(dataToEncrypt, false);
            }
        }
        public static void EncryptDecrypt()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKeyXML = rsa.ToXmlString(false);
            string privateKeyXML = rsa.ToXmlString(true);
            UnicodeEncoding ByteConverter = new UnicodeEncoding();
            byte[] dataToEncrypt = ByteConverter.GetBytes("My Secret Data!");
            byte[] encryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(publicKeyXML);
                encryptedData = RSA.Encrypt(dataToEncrypt, false);
            }

            byte[] decryptedData;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                RSA.FromXmlString(privateKeyXML);
                decryptedData = RSA.Decrypt(encryptedData, false);
            }

            string decryptedString = ByteConverter.GetString(decryptedData);
            Console.WriteLine(decryptedString);
        }
        public static void ExportPublicKey()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            string publicKeyXML = rsa.ToXmlString(false);
            string privateKeyXML = rsa.ToXmlString(true);

            Console.WriteLine(publicKeyXML);
            Console.WriteLine(privateKeyXML);
        }
        public static void EncryptSomeText()
        {
            string original = "My secret data!";

            using (SymmetricAlgorithm symmetricAlgorithm = new AesManaged())
            {
                byte[] encrypted = Encrypt(symmetricAlgorithm, original);
                string roundtrip = Decrypt(symmetricAlgorithm, encrypted);

                Console.WriteLine("Original: {0}", original);
                Console.WriteLine("Round Trip: {0}", roundtrip);
            }
        }

        static byte[] Encrypt(SymmetricAlgorithm aesAlg, string plainText)
        {
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        static string Decrypt(SymmetricAlgorithm aesAlg, byte[] cipherText)
        {
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }

    class ImperativeCASDemo
    {
        public void Demo()
        {
            FileIOPermission f = new FileIOPermission(PermissionState.None);
            f.AllLocalFiles = FileIOPermissionAccess.Read;
            try
            {
                f.Demand();
            }
            catch (SecurityException s)
            {
                Console.WriteLine(s.Message);
            }
        }
    }

    class DeclariativeCASDemo
    {
        [FileIOPermission(SecurityAction.Demand, AllLocalFiles = FileIOPermissionAccess.Read)]
        public void DeclarativeCAS() { }
    }

    class SetWithHashing<T>
    {
        private List<T>[] buckets = new List<T>[100];

        public void Insert(T item)
        {
            int bucket = GetBucket(item.GetHashCode());
            if (Contains(item, bucket))
                return;
            if (buckets[bucket] == null)
                buckets[bucket] = new List<T>();
            buckets[bucket].Add(item);
        }

        public bool Contains(T item)
        {
            return Contains(item, GetBucket(item.GetHashCode()));
        }

        private int GetBucket(int hashcode)
        {
            unchecked
            {
                return (int)((uint)hashcode % (uint)buckets.Length);
            }
        }

        private bool Contains(T item, int bucket)
        {
            if (buckets[bucket] != null)
                foreach (T member in buckets[bucket])
                    if (member.Equals(item))
                        return true;
            return false;
        }
    }

    class Set<T>
    {
        private List<T> list = new List<T>();

        public void Insert(T item)
        {
            if (!Contains(item))
                list.Add(item);
        }

        public bool Contains(T item)
        {
            foreach (T member in list)
                if (member.Equals(item))
                    return true;
            return false;
        }
    }
}

/*
 * Thought experiment: You are working on an application that helps users track their time and shows them when they are most
 * productive. The application runs on a server with a web, desktop, and mobile front end being developed with the .NET framework.
 * You are assigned the task of determining which security features should be used in the application. Make a list of possible technologies
 * that can be used to secure the application:
 * 
 * This is a difficult one. I think Advanced Encryption Standard would be important in sending data to and from the client / database, assuming
 * the data needs encryption. Hashing would also be useful in order to map this data into smaller sets (though I couldn't tell you exactly how atm)
 * Digital certificates would handle both of the above. Code access permissions don't seem like they'd be terrible helpful unless this data was being stored in
 * files somewhere. I also don't see much value in Secure Strings here.
 * 
 * Review:
 * 
 * 1. Bob and Alice are using an asymmetric algorithm to exchange data. Which key should they send to the other party to make this possible?
 *  3. They both send each other their public keys. You don't share private ones.
 * 2. You need to encrypt a large amount of data. Which algorithm do you use?
 *  1. SHA256
 * 3. You need to send sensitive data to another party and you want to make sure that no one tampers with the data.
 *  1. X509Certificate2.SignHash
 * */
