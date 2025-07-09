using System.Net;

namespace Local_Keylogger_Agent
{
    internal class HTTPServer
    {
        private HttpListener httpListener;

        public void StartHTTPServer()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://+:5000/"); // Set the prefix for the HTTP server
            httpListener.Start();

            Task.Run(async () =>
            {
                while (httpListener.IsListening)
                {
                    HttpListenerContext httpListenerContext = await httpListener.GetContextAsync();
                }
            });
        }
        private void HandleRequest(HttpListenerContext context)
        {
            // Handle the incoming request here
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;


        }
    }
}
