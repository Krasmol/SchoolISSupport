using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchoolISSupport
{
    public static class ThemeHelper
    {
        // Основные цвета
        public static readonly Color PrimaryColor = Color.FromArgb(67, 97, 238);
        public static readonly Color SecondaryColor = Color.FromArgb(76, 201, 240);
        public static readonly Color SuccessColor = Color.FromArgb(72, 187, 120);
        public static readonly Color WarningColor = Color.FromArgb(250, 177, 60);
        public static readonly Color DangerColor = Color.FromArgb(235, 77, 75);
        public static readonly Color DarkColor = Color.FromArgb(45, 55, 72);
        public static readonly Color LightColor = Color.FromArgb(247, 250, 252);
        public static readonly Color GrayColor = Color.FromArgb(113, 128, 150);

        // Градиенты
        public static LinearGradientBrush GetPrimaryGradient(Rectangle rect)
        {
            return new LinearGradientBrush(rect, PrimaryColor, SecondaryColor, 45f);
        }

        // Создание закруглённой кнопки
        public static void StyleButton(Button btn, Color backColor, int radius = 10)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = backColor;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Height = 40;

            // Закругление
            var path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(btn.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(btn.Width - radius, btn.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, btn.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            btn.Region = new Region(path);
        }

        // Создание закруглённой панели
        public static void StylePanel(Panel panel, Color backColor, int radius = 15)
        {
            panel.BackColor = backColor;
            var path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(panel.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(panel.Width - radius, panel.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, panel.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            panel.Region = new Region(path);
        }

        // Создание закруглённого DataGridView
        public static void StyleDataGridView(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(226, 232, 240);
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;

            // Стиль заголовков
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(67, 97, 238);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Стиль ячеек
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(45, 55, 72);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(67, 97, 238);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(5);
            dgv.RowTemplate.Height = 35;
        }

        // Создание закруглённого TextBox
        public static void StyleTextBox(TextBox txt, bool multiline = false)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Font = new Font("Segoe UI", 10);
            txt.BackColor = Color.White;
            if (!multiline) txt.Height = 35;
        }

        // Создание закруглённого ComboBox
        public static void StyleComboBox(ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.Font = new Font("Segoe UI", 10);
            cb.Height = 35;
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // Создание заголовка
        public static Label CreateHeader(string text, int fontSize = 18)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", fontSize, FontStyle.Bold),
                ForeColor = DarkColor,
                AutoSize = true
            };
        }

        // Создание карточки с тенью
        public static Panel CreateCard(int width, int height)
        {
            return new Panel
            {
                Width = width,
                Height = height,
                BackColor = Color.White,
                Padding = new Padding(15)
            };
        }
    }
}