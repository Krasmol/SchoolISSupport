using System;
using System.Windows.Forms;

namespace SchoolISSupport
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

            if (loginForm.IsAuthenticated)
            {
                Form1 mainForm = new Form1();
                mainForm.Show();
                Application.Run();
            }
            else
            {
                Application.Exit();
            }
        }
    }
}