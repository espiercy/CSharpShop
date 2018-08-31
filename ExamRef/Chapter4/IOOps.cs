using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Chapter4
{
    public class IOOps
    {
        public async Task ExecuteMultipleRequestsInParallel()
        {
            HttpClient client = new HttpClient();

            Task microsoft = client.GetStringAsync("http://microsoft.com");
            Task msdn = client.GetStringAsync("http://msdn.microsoft.com");
            Task blogs = client.GetStringAsync("http://blogs.msdn.com/");

            await Task.WhenAll(microsoft, msdn, blogs);
        }
        public async Task ExecuteMultipleRequests()
        {
            HttpClient client = new HttpClient();

            string microsoft = await client.GetStringAsync("http://www.microsoft.com");
            string msdn = await client.GetStringAsync("http://msdn.microsoft.com");
            string blogs = await client.GetStringAsync("http://blogs.msdn.com/");
        }
        public async Task ReadAsyncHttpRequest()
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("http://microsoft.com");
        }
        public async Task CreateAndWriteAsyncToFile()
        {
            using (FileStream stream = new FileStream("text.dat", FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
            {
                byte[] data = new byte[100000];
                new Random().NextBytes(data);

                await stream.WriteAsync(data, 0, data.Length);
            }
        }
        public static void WebRequestDemo()
        {
            WebRequest request = WebRequest.Create("http://www,microsoft.com");
            WebResponse response = request.GetResponse();

            StreamReader responseStream = new StreamReader(response.GetResponseStream());
            string responseText = responseStream.ReadToEnd();

            Console.WriteLine(responseText);

            response.Close();
        }
        public static string FileExceptionHandling()
        {
            string path = @"C:\temp\test.txt";
            try
            {
                return File.ReadAllText(path);
            }
            catch (DirectoryNotFoundException) { }
            catch (FileNotFoundException) { }
            return string.Empty;
        }
        private static string ReadAllText()
        {
            string path = @"C:\temp\test.txt";

            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }
            return string.Empty;
        }
        public static void BufferedStreamDemo()
        {
            string path = @"c:\temp\bufferedStream.txt";

            using (FileStream fileStream = File.Create(path))
            {
                using (BufferedStream bufferedStream = new BufferedStream(fileStream))
                {
                    using (StreamWriter streamWriter = new StreamWriter(bufferedStream))
                    {
                        streamWriter.WriteLine("A line of text.");
                    }
                }
            }
        }
        public static void CompressDataWithGZipStreamDemo()
        {
            string folder = @"c:\temp";
            string uncompressedFilePath = Path.Combine(folder, "uncompressed.dat");
            string compressedFilePath = Path.Combine(folder, "compressed.gz");
            byte[] dataToCompress = Enumerable.Repeat((byte)'a', 1024 * 1024).ToArray();

            using (FileStream uncompressedFileStream = File.Create(uncompressedFilePath))
            {
                uncompressedFileStream.Write(dataToCompress, 0, dataToCompress.Length);
            }
            using (FileStream compressedFileStream = File.Create(compressedFilePath))
            {
                using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                {
                    compressionStream.Write(dataToCompress, 0, dataToCompress.Length);
                }
            }

            FileInfo uncompressedFile = new FileInfo(uncompressedFilePath);
            FileInfo compressedFile = new FileInfo(compressedFilePath);

            Console.WriteLine(uncompressedFile.Length);
            Console.WriteLine(compressedFile.Length);
        }
        public static void OpenAndReadTextFileDemo()
        {
            string path = @"c:\temp\test.dat";
            using (StreamReader streamReader = File.OpenText(path))
            {
                Console.WriteLine(streamReader.ReadLine());
            }
        }
        public static void DecodeFromFileStreamDemo()
        {
            string path = @"c:\temp\test.dat";
            using (FileStream fileStream = File.OpenRead(path))
            {
                byte[] data = new byte[fileStream.Length];
                for (int index = 0; index < fileStream.Length; index++)
                {
                    data[index] = (byte)fileStream.ReadByte();
                }
                Console.WriteLine(Encoding.UTF8.GetString(data));
            }
        }
        public static void CreateTextWithStreamWriterDemo()
        {
            string path = @"c:\temp\test.dat";
            using (StreamWriter writer = File.CreateText(path))
            {
                string value = "MyValue";
                writer.Write(value);
            }
        }
        public static void OtherPathMethods()
        {
            string path = @"C:\temp\subdir\file.txt";
            Console.WriteLine(Path.GetDirectoryName(path));
            Console.WriteLine(Path.GetExtension(path));
            Console.WriteLine(Path.GetFileName(path));
            Console.WriteLine(Path.GetPathRoot(path));
        }
        public static void CombineDemo()
        {
            string folder = @"C:\temp";
            string fileName = "test.dat";

            string fullPath = Path.Combine(folder, fileName);
        }
        public static void ManualPathEntryError()
        {
            string folder = @"C:\temp";
            string fileName = "test.dat";

            string fullPath = folder + fileName;
        }
        public static void CopyFileDemo()
        {
            string path = @"c:\temp\test.txt";
            string destPath = @"c:\temp\destTest.txt";
            File.CreateText(path).Close();
            File.Copy(path, destPath);

            FileInfo fileInfo = new FileInfo(path);
            fileInfo.CopyTo(destPath);
        }
        public static void MoveFileDemo()
        {
            string path = @"c:\temp\test.txt";
            string destPath = @"c:\temp\destTest.txt";
            File.CreateText(path).Close();
            File.Move(path, destPath);

            FileInfo fileInfo = new FileInfo(path);
            fileInfo.MoveTo(destPath);
        }
        public static void DeleteFileDemo()
        {
            string path = @"c:\temp\test.txt";

            if (File.Exists(path)) File.Delete(path);

            FileInfo fileInfo = new FileInfo(path);
            if (fileInfo.Exists) fileInfo.Delete();
        }
        public static void ListAllFilesInDirectory()
        {
            foreach (string file in Directory.GetFiles(@"C:\Windows")) Console.WriteLine(file);

            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Windows");
            foreach (FileInfo fileInfo in directoryInfo.GetFiles())
            {
                Console.WriteLine(fileInfo.FullName);
            }
        }
        public static void MoveDirectoryDemo()
        {
            Directory.Move(@"C:\source", @"c:\destination");

            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Source");
            directoryInfo.MoveTo(@"C:\destination");
        }
        public static void ListDirectoriesDemo()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Program Files");
            ListDirectories(directoryInfo, "*a*", 7, 0);
        }
        private static void ListDirectories(DirectoryInfo directoryInfo, string searchPattern, int maxLevel, int currentLevel)
        {
            if (currentLevel >= maxLevel) return;

            string indent = new string('-', currentLevel);

            try
            {
                DirectoryInfo[] subDirectories = directoryInfo.GetDirectories(searchPattern);

                foreach (DirectoryInfo subDirectory in subDirectories)
                {
                    Console.WriteLine(indent + subDirectory.Name);
                    ListDirectories(subDirectory, searchPattern, maxLevel, currentLevel + 1);
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(indent + "Can't access: " + directoryInfo.Name);
                return;
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine(indent + "Can't find: " + directoryInfo.Name);
                return;
            }
        }
        public static void SetAccessControlDirectory()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo("TestDirectory");
            directoryInfo.Create();
            DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
            directorySecurity.AddAccessRule(new FileSystemAccessRule("everyone", FileSystemRights.ReadAndExecute, AccessControlType.Allow));
            directoryInfo.SetAccessControl(directorySecurity);
        }
        public static void DeleteExistingDirectoryDemo()
        {
            if (Directory.Exists(@"C:\Temp\ProgrammingInCSharp\Directory"))
            {
                Directory.Delete(@"C:\Temp\ProgrammingInCSharp\Directory");
            }

            var directoryInfo = new DirectoryInfo(@"C:\Temp\ProgrammingInCSharp\DirectoryInfo");

            if (directoryInfo.Exists)
            {
                directoryInfo.Delete();
            }
        }
        public static void CreateDirectoryDemo()
        {
            var directory = Directory.CreateDirectory(@"C:\Temp\ProgrammingInCSharp\Directory");

            var directoryInfo = new DirectoryInfo(@"C:\Temp\ProgrammingInCSharp\DirectoryInfo");

            directoryInfo.Create();
        }
        public static void DriveInfoDemo()
        {
            DriveInfo[] drivesInfo = DriveInfo.GetDrives();

            foreach (DriveInfo driveInfo in drivesInfo)
            {
                Console.WriteLine("Drive {0}", driveInfo.Name);
                Console.WriteLine(" File type: {0}", driveInfo.DriveType);

                if (driveInfo.IsReady == true)
                {
                    Console.WriteLine(" Volume Label: {0}", driveInfo.VolumeLabel);
                    Console.WriteLine(" File System: {0}", driveInfo.DriveFormat);
                    Console.WriteLine(" Available space to current user:{0,15} bytes", driveInfo.AvailableFreeSpace);
                    Console.WriteLine(" Total available space: {0,15} bytes", driveInfo.TotalFreeSpace);
                    Console.WriteLine(" Total size of drive: {0,15} bytes", driveInfo.TotalSize);

                }
            }
        }
    }
}

/*Thought experiment: You need to create a custom File Explorer that uses WPF for a customer. Other members of your team work on the user
 * interface of the File Explorer. You are tasked with creating the code that handles all the I/O. The File Explorer should be an
 * abstraction over the file system. It shouldn't show any drives; instead, it should group files into categories that depend on the
 * location and file type. The categories and locations are given to you by the customer. For example, you have a category "Administration"
 * that contains Microsoft Office Documents from multiple locations.
 * 
 * 1. Which classes do you plan to use? Dumb question. You will be using a wide array of questions.
 * 2. How will you filter the files by specific file types? The class that has the ability to parse paths. Probably the path class...
 * 3. Do you need asynchronous code? Yes of course you do, especially for the I/O portion...
 * 
 * Review:
 * 
 * 1. You are creating a new file to store some log data. Each time a new log entry is necessary, you write a string to the file
 * Which method do you use?
 *  4. File.AppendText
 * 
 * 2. You have built a complex calculation algorithm. It takes quite some time to complete and you want to make sure that your
 * application remains responsive. What do you do?
 *  1. Use async/await
 *  
 * 3. You are writing an application that will be deployed to Western countries. It outputs user activity to a text file. Which
 * encoding should you use?
 *  1. UTF-8
 */
