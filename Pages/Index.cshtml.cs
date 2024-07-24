using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Management.Automation;

namespace DotNetPowerShellApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Name { get; set; }

        public string Output { get; set; }

        public void OnGet()
        {
        }

        public void OnPost()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                using (PowerShell powerShell = PowerShell.Create())
                {
                    // Concatenate the user input with custom text
                    string script = $"\"{Name} - has been entered successfully\"";

                    // Execute the PowerShell script
                    powerShell.AddScript(script);

                    // Collect the results
                    var results = powerShell.Invoke();

                    // Build the output string
                    Output = string.Join("\n", results.Select(r => r.ToString()));
                }
            }
        }
    }
}