using System;
using System.IO;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace HttpListenerExample
{
    class HttpServer
    {
        public static HttpListener listener;
        public static string url = "http://localhost:8000/";
        public static int pageViews = 0;
        public static int requestCount = 0;
        public static string pageData =
            "<!DOCTYPE>" +
            "<html>" +
            " <head>" +
            "  <title>Http Listener Example</title>" +
            "</head>" +
            "<body>" +
            "  <p>Page VIews: {0}</p>" +
            "  <form method=\"post\" action=\"shutdown\">" +
            "    <input type=\"submit\" value=\"Shutdown\" {1}>" +
            "  </form>" +
            "</body>" +
            "</html>";

        public static async Task HandleIncomingConnections()
        {
            bool runServer = true;

            // while user hasn't visited shutdown url, keep handling requests

            while (runServer)
            {
                // wait until we hear a connection
                HttpListenerExampleContext ctx = await listener.GetContextAsync();

                HttpListenerRequest req = ctx.Request;
                HttpListenerResponse res = ctx.Response;

                Console.WriteLine("Request #: {0}", ++requestCount);
                Console.WriteLine(req.Url.ToString());
                Console.WriteLine(req.HttpMethod);
                Console.WriteLine(req.UserHostName);
                Console.WriteLine(req.UserAgent);
                
                // shutdown server if /shutdown requested
                if ((req.HttpMethod == "POST") && (req.Url.AbsolutePath == "/shutdown"))
                {
                    Console.WriteLine("Shutdown requested");
                    runServer = false;
                }

                // increment page count, but dont increment if favicon is requested
                if (req.Url.AbsolutePath != "/favicon.ico")
                {
                    pageViews += 1;
                }

                // write response info
                string disableSubmit = !runServer ? "disabled" : "";
                byte[] data = Encoding.UTF8.GetBytes(string.Format(pageData, pageViews, disableSubmit));
                resp.ContentType = "text/html";
                resp.ContentEncoding = Encoding.UTF8;
                resp.ContentLength64 = data.LongLength;

                await resp.OutputStream.WriteAsync(data, 0, data.Length);
                resp.Close();
            }
        }

        public static void Main(string[] args)
        {
            // create Http server and listen
            listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();
            Console.WriteLine("Listening for connections on {0}", url);

            // handle requests
            Task listenTask = HandleIncomingConnections();
            listenTask.GetAwaiter().getResult();

            // close listener
            listener.Close();
        }
    }
}