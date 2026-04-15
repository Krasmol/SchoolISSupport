using SchoolISSupport;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchoolISSupport_UserVersion
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private Panel cardPanel;

        public bool IsAuthenticated { get; private set; } = false;
        public string CurrentUserRole { get; private set; } = "";
        public string CurrentUserName { get; private set; } = "";
        public int CurrentUserId { get; private set; } = 0;

        public LoginForm()
        {
            InitializeComponent();
            CreateControls();
        }

        private void CreateControls()
        {
            this.Text = "Авторизация";
            this.Size = new Size(500, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(67, 97, 238);

            cardPanel = new Panel
            {
                Size = new Size(420, 520),
                Location = new Point(40, 40),
                BackColor = Color.White
            };
            StyleRoundedPanel(cardPanel, 25);

            PictureBox logo = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(170, 30),
                BackColor = Color.Transparent
            };
            Bitmap bmp = new Bitmap(80, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                g.FillEllipse(new SolidBrush(Color.FromArgb(67, 97, 238)), 10, 10, 60, 60);
                using (Font iconFont = new Font("Segoe UI", 32, FontStyle.Bold))
                {
                    g.DrawString("📚", iconFont, new SolidBrush(Color.White), 22, 18);
                }
            }
            logo.Image = bmp;

            Label lblTitle = new Label
            {
                Text = "Сопровождение ИС",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 55, 72),
                Location = new Point(20, 120),
                Size = new Size(380, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblSubTitle = new Label
            {
                Text = "образовательной организации",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(113, 128, 150),
                Location = new Point(20, 155),
                Size = new Size(380, 28),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblUsername = new Label
            {
                Text = "Логин",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 55, 72),
                Location = new Point(40, 220),
                Size = new Size(340, 25)
            };

            txtUsername = new TextBox
            {
                Location = new Point(40, 248),
                Size = new Size(340, 38),
                Text = "user1",
                Font = new Font("Segoe UI", 11)
            };
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.BackColor = Color.FromArgb(247, 250, 252);

            Label lblPassword = new Label
            {
                Text = "Пароль",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 55, 72),
                Location = new Point(40, 310),
                Size = new Size(340, 25)
            };

            txtPassword = new TextBox
            {
                Location = new Point(40, 338),
                Size = new Size(340, 38),
                UseSystemPasswordChar = true,
                Text = "user123",
                Font = new Font("Segoe UI", 11)
            };
            txtPassword.BorderStyle = BorderStyle.FixedSingle;
            txtPassword.BackColor = Color.FromArgb(247, 250, 252);
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(s, e); };

            btnLogin = new Button
            {
                Text = "Войти",
                Location = new Point(40, 410),
                Size = new Size(340, 48),
                BackColor = Color.FromArgb(46, 204, 113),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += BtnLogin_Click;

            btnExit = new Button
            {
                Text = "Выход",
                Location = new Point(40, 470),
                Size = new Size(340, 40),
                BackColor = Color.Transparent,
                ForeColor = Color.FromArgb(113, 128, 150),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Cursor = Cursors.Hand
            };
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Click += (s, e) => Application.Exit();

            cardPanel.Controls.Add(logo);
            cardPanel.Controls.Add(lblTitle);
            cardPanel.Controls.Add(lblSubTitle);
            cardPanel.Controls.Add(lblUsername);
            cardPanel.Controls.Add(txtUsername);
            cardPanel.Controls.Add(lblPassword);
            cardPanel.Controls.Add(txtPassword);
            cardPanel.Controls.Add(btnLogin);
            cardPanel.Controls.Add(btnExit);

            this.Controls.Add(cardPanel);

            this.Opacity = 0;
            Timer fadeTimer = new Timer { Interval = 20 };
            fadeTimer.Tick += (s, e) => {
                if (this.Opacity < 1) this.Opacity += 0.05;
                else fadeTimer.Stop();
            };
            fadeTimer.Start();
        }

        private void StyleRoundedPanel(Panel panel, int radius)
        {
            var path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            panel.Region = new Region(path);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string login = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            string sql = $"SELECT * FROM Users WHERE Login = '{login}' AND Password = '{password}'";
            DataTable result = DatabaseHelper.ExecuteQuery(sql);

            if (result != null && result.Rows.Count > 0)
            {
                IsAuthenticated = true;
                CurrentUserRole = result.Rows[0]["Role"].ToString();
                CurrentUserName = result.Rows[0]["FullName"].ToString();
                CurrentUserId = Convert.ToInt32(result.Rows[0]["UserID"]);
                this.Close();
            }
            else
            {
                MessageBox.Show("❌ Неверный логин или пароль!", "Ошибка авторизации",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtPassword.Focus();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Pen pen = new Pen(Color.FromArgb(100, 255, 255, 255), 2))
            {
                e.Graphics.DrawEllipse(pen, -100, -100, 250, 250);
                e.Graphics.DrawEllipse(pen, this.Width - 150, this.Height - 150, 200, 200);
            }
        }
    }
}