using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrainFuzz
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                newMain();
            }
            catch (System.IO.FileNotFoundException fnfe)
            {
                MessageBox.Show("Could not load required files. Make sure BrainFuzzInterpreter.dll is in the same directory.",
                    "Error Loading Files", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        public static void newMain()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

    }
}
