using System;
using System.Windows.Forms;

namespace SchoolISSupport_UserVersion
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
                Form1 mainForm = new Form1(
                    loginForm.CurrentUserRole,
                    loginForm.CurrentUserName,
                    loginForm.CurrentUserId
                );
                Application.Run(mainForm);
            }
            else
            {
                Application.Exit();
            }
        }
    }
}