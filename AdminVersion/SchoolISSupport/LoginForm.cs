using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchoolISSupport
{
    public partial class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnExit;
        private Panel cardPanel;

        public bool IsAuthenticated { get; private set; } = false;

        public LoginForm()
        {
            InitializeComponent();
            CreateControls();
        }

        private void CreateControls()
        {
            this.Text = "Авторизация";
            this.Size = new Size(500, 620);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(67, 97, 238);

            cardPanel = new Panel
            {
                Size = new Size(420, 540),
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
                    g.DrawString("📚", iconFont, new SolidBrush(Color.White), 19, 10);
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
                Location = new Point(40, 210),
                Size = new Size(340, 25)
            };

            txtUsername = new TextBox
            {
                Location = new Point(40, 240),
                Size = new Size(340, 38),
                Text = "admin",
                Font = new Font("Segoe UI", 11)
            };
            StyleTextBox(txtUsername);
            txtUsername.BackColor = Color.FromArgb(247, 250, 252);

            Label lblPassword = new Label
            {
                Text = "Пароль",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(45, 55, 72),
                Location = new Point(40, 295),
                Size = new Size(340, 25)
            };

            txtPassword = new TextBox
            {
                Location = new Point(40, 325),
                Size = new Size(340, 38),
                UseSystemPasswordChar = true,
                Text = "admin123",
                Font = new Font("Segoe UI", 11)
            };
            StyleTextBox(txtPassword);
            txtPassword.BackColor = Color.FromArgb(247, 250, 252);
            txtPassword.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) BtnLogin_Click(s, e); };

            btnLogin = new Button
            {
                Text = "Войти в систему",
                Location = new Point(40, 390),
                Size = new Size(340, 48),
                BackColor = Color.FromArgb(67, 97, 238)
            };
            StyleButton(btnLogin, Color.FromArgb(67, 97, 238), 12);
            btnLogin.Click += BtnLogin_Click;

            btnExit = new Button
            {
                Text = "Выход",
                Location = new Point(40, 450),
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

        private void StyleTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 11);
        }

        private void StyleButton(Button btn, Color backColor, int radius)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            var path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            btn.Region = new Region(path);
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "admin" && txtPassword.Text == "admin123")
            {
                IsAuthenticated = true;
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

        private void InitializeComponent() { }
    }
}