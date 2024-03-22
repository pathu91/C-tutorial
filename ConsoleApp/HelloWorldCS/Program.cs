using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace HttpClientExample
{
    class Downloader
    {
        public static string urlToDownload = "https://books.toscrape.com/";
        public static string filename = "index.html";
        
        public static async Task DownloadWebPage()
        {
            Console.WriteLine("Starting download...");
            // Setup the HttpClient
            using (HttpClient httpClient = new HttpClient())
            {
                // get webpage async
                HttpResponseMessage resp = await httpClient.GetAsync(urlToDownload);

                // if 200
                if (resp.IsSuccessStatusCode)
                {
                    Console.WriteLine("Got website");

                    Console.WriteLine(Directory.GetCurrentDirectory());
                    // Get data
                    byte[] data = await resp.Content.ReadAsByteArrayAsync();

                    // save it to a file
                    try
                    {

                    FileStream fStream = File.Create(filename);
                    await fStream.WriteAsync(data, 0, data.Length);
                    fStream.Close();
                    Console.WriteLine("Done!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }


                }
            }
        }


        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();

            // Check if the directory is writable
            if (IsDirectoryWritable(currentDirectory))
            {
                Task dlTask = DownloadWebPage();

                Console.WriteLine("Holding for at least 5 seconds...");
                Thread.Sleep(TimeSpan.FromSeconds(5));

                dlTask.GetAwaiter().GetResult();
            }
            else
            {
                Console.WriteLine("Current directory is not writable or does not exist.");
            }
        }

        static bool IsDirectoryWritable(string path)
        {
            try
            {
                // Attempt to create a test file in the directory
                using (FileStream fs = File.Create(Path.Combine(path, Path.GetRandomFileName()), 1, FileOptions.DeleteOnClose))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


    }
}