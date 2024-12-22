using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PauGO
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var processName = Process.GetCurrentProcess().ProcessName;
            foreach (var process in Process.GetProcessesByName(processName))
            {
                if (process.Id != Process.GetCurrentProcess().Id)
                {
                    process.Kill(); // Çalışan diğer kopyayı sonlandır
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
