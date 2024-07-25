using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace PowerShellWebApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string UserInput { get; set; }
        public string ScriptOutput { get; set; }

        public void OnPost()
        {
            if (!string.IsNullOrEmpty(UserInput))
            {
                ScriptOutput = RunPowerShellScript(UserInput);
            }
        }

        private string RunPowerShellScript(string input)
        {
            string scriptPath = "scripts/run-mfa.ps1"; // Adjust the path as needed
            ProcessStartInfo startInfo = new ProcessStartInfo()
            {
                FileName = "pwsh", // Use pwsh (PowerShell Core) which is supported in Azure App Service
                Arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptPath}\" \"{input}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    return $"Error: {error}";
                }

                return output;
            }
        }
    }
}