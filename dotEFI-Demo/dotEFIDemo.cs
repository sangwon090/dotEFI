using System;
using System.Windows.Forms;

namespace dotEFIDemo
{
    static class dotEFIDemo
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
