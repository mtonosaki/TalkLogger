// (c) 2020 Manabu Tonosaki
// Licensed under the MIT license.

using System;
using System.Windows.Forms;

namespace TalkLoggerWinform
{
    static internal class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
