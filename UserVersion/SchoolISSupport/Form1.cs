using SchoolISSupport;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchoolISSupport_UserVersion
{
    public partial class Form1 : Form
    {
        private TabControl tabControl;

        // Вкладки
        private TabPage tabPageDashboard;
        private TabPage tabPageEmployees;
        private TabPage tabPageDepartments;
        private TabPage tabPageEquipment;
        private TabPage tabPageSoftware;
        private TabPage tabPageRequests;

        // Dashboard элементы
        private Button btnBestRequester;
        private Button btnTopSoftware;
        private Button btnNoProblems;
        private Button btnRefreshAll;
        private DataGridView dgvBestRequester;
        private DataGridView dgvTopSoftware;
        private DataGridView dgvNoProblems;
        private Label lblBestRequesterResult;
        private Label lblTopSoftwareResult;
        private Label lblNoProblemsResult;
        private Panel card1, card2, card3;

        // DataGridView для таблиц
        private DataGridView dgvEmployees;
        private DataGridView dgvDepartments;
        private DataGridView dgvEquipment;
        private DataGridView dgvSoftware;
        private DataGridView dgvRequests;

        // Панели с кнопками
        private FlowLayoutPanel flpEmployees, flpDepartments, flpEquipment, flpSoftware, flpRequests;

        // CRUD кнопки
        private Button btnAddEmployee, btnEditEmployee, btnDeleteEmployee;
        private Button btnAddDepartment, btnEditDepartment, btnDeleteDepartment;
        private Button btnAddEquipment, btnEditEquipment, btnDeleteEquipment;
        private Button btnAddSoftware, btnEditSoftware, btnDeleteSoftware;
        private Button btnAddRequest, btnEditRequest, btnDeleteRequest;

        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolStripStatusLabel userLabel;

        // Информация о текущем пользователе
        private string _userRole;
        private string _userName;
        private int _userId;

        public Form1(string userRole, string userName, int userId)
        {
            _userRole = userRole;
            _userName = userName;
            _userId = userId;
            CreateControls();
            ApplyRoleRestrictions();
            SetupEvents();
            this.Load += Form1_Load;
        }

        private void ApplyRoleRestrictions()
        {
            // Если пользователь, а не админ
            if (_userRole != "Admin")
            {
                // Отключаем кнопки добавления/редактирования/удаления для справочников
                btnAddEmployee.Visible = false;
                btnEditEmployee.Visible = false;
                btnDeleteEmployee.Visible = false;

                btnAddDepartment.Visible = false;
                btnEditDepartment.Visible = false;
                btnDeleteDepartment.Visible = false;

                btnAddEquipment.Visible = false;
                btnEditEquipment.Visible = false;
                btnDeleteEquipment.Visible = false;

                btnAddSoftware.Visible = false;
                btnEditSoftware.Visible = false;
                btnDeleteSoftware.Visible = false;

                btnEditRequest.Visible = false;
                btnDeleteRequest.Visible = false;

                // DataGridView только для чтения
                dgvEmployees.ReadOnly = true;
                dgvDepartments.ReadOnly = true;
                dgvEquipment.ReadOnly = true;
                dgvSoftware.ReadOnly = true;
                dgvRequests.ReadOnly = true;
            }
        }

        private void CreateControls()
        {
            this.Text = _userRole == "Admin" ? "👑 Администратор | Управление ИС" : "👤 Сотрудник | Создание заявок";
            this.Size = new Size(1300, 800);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1000, 700);
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Меню
            MenuStrip menuStrip = new MenuStrip();
            menuStrip.Font = new Font("Segoe UI", 10);
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("📁 Файл");
            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("🚪 Выход");
            exitMenuItem.Click += (s, e) => Application.Exit();
            fileMenu.DropDownItems.Add(exitMenuItem);

            ToolStripMenuItem helpMenu = new ToolStripMenuItem("❓ Справка");
            ToolStripMenuItem aboutMenuItem = new ToolStripMenuItem("ℹ️ О программе");
            aboutMenuItem.Click += (s, e) => MessageBox.Show(
                "Сопровождение ИС образовательной организации\nВерсия 2.0\n\n" +
                $"Пользователь: {_userName}\nРоль: {_userRole}",
                "О программе", MessageBoxButtons.OK, MessageBoxIcon.Information);
            helpMenu.DropDownItems.Add(aboutMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(helpMenu);

            // TabControl
            tabControl = new TabControl { Dock = DockStyle.Fill };

            // ========== Вкладка Dashboard ==========
            tabPageDashboard = new TabPage("📊 Дашборд");
            tabPageDashboard.BackColor = Color.FromArgb(245, 245, 250);
            tabPageDashboard.Padding = new Padding(20);
            tabPageDashboard.AutoScroll = true;

            Label lblDashboardTitle = new Label
            {
                Text = "📈 Аналитика информационной системы",
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 15),
                AutoSize = true
            };
            tabPageDashboard.Controls.Add(lblDashboardTitle);

            // Карточки (как в первой версии)
            card1 = CreateDashboardCard(20, 80, 380, 440);
            Label card1Title = new Label
            {
                Text = "👥 Самый активный заявитель",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(15, 15),
                AutoSize = true
            };
            btnBestRequester = CreateCardButton("📊 Показать результат", 15, 55, 160, 40, Color.FromArgb(52, 152, 219));
            lblBestRequesterResult = new Label
            {
                Text = "▶ Нажмите кнопку для расчёта",
                Location = new Point(15, 105),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            dgvBestRequester = CreateDataGridView(15, 145, 350, 270);
            card1.Controls.Add(card1Title);
            card1.Controls.Add(btnBestRequester);
            card1.Controls.Add(lblBestRequesterResult);
            card1.Controls.Add(dgvBestRequester);

            card2 = CreateDashboardCard(430, 80, 380, 440);
            Label card2Title = new Label
            {
                Text = "💻 Самая популярная программа",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(15, 15),
                AutoSize = true
            };
            btnTopSoftware = CreateCardButton("📊 Показать результат", 15, 55, 160, 40, Color.FromArgb(46, 204, 113));
            lblTopSoftwareResult = new Label
            {
                Text = "▶ Нажмите кнопку для расчёта",
                Location = new Point(15, 105),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            dgvTopSoftware = CreateDataGridView(15, 145, 350, 270);
            card2.Controls.Add(card2Title);
            card2.Controls.Add(btnTopSoftware);
            card2.Controls.Add(lblTopSoftwareResult);
            card2.Controls.Add(dgvTopSoftware);

            card3 = CreateDashboardCard(840, 80, 380, 440);
            Label card3Title = new Label
            {
                Text = "🏫 Аудитории без проблем",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(15, 15),
                AutoSize = true
            };
            btnNoProblems = CreateCardButton("📊 Показать результат", 15, 55, 160, 40, Color.FromArgb(155, 89, 182));
            lblNoProblemsResult = new Label
            {
                Text = "▶ Нажмите кнопку для расчёта",
                Location = new Point(15, 105),
                Size = new Size(350, 30),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            dgvNoProblems = CreateDataGridView(15, 145, 350, 270);
            card3.Controls.Add(card3Title);
            card3.Controls.Add(btnNoProblems);
            card3.Controls.Add(lblNoProblemsResult);
            card3.Controls.Add(dgvNoProblems);

            btnRefreshAll = new Button
            {
                Text = "🔄 Обновить всё",
                Location = new Point(20, 540),
                Size = new Size(170, 48),
                BackColor = Color.FromArgb(241, 76, 33),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefreshAll.FlatAppearance.BorderSize = 0;

            tabPageDashboard.Controls.Add(card1);
            tabPageDashboard.Controls.Add(card2);
            tabPageDashboard.Controls.Add(card3);
            tabPageDashboard.Controls.Add(btnRefreshAll);

            // ========== Вкладка Сотрудники ==========
            tabPageEmployees = CreateCrudTab("👥 Сотрудники", out dgvEmployees, out flpEmployees);
            btnAddEmployee = AddCrudButton(flpEmployees, "➕ Добавить", Color.FromArgb(46, 204, 113));
            btnEditEmployee = AddCrudButton(flpEmployees, "✏️ Редактировать", Color.FromArgb(241, 196, 15));
            btnDeleteEmployee = AddCrudButton(flpEmployees, "🗑️ Удалить", Color.FromArgb(231, 76, 60));
            Button btnRefreshEmployee = AddCrudButton(flpEmployees, "🔄 Обновить", Color.FromArgb(52, 152, 219));
            btnRefreshEmployee.Click += (s, e) => LoadEmployees();

            // ========== Вкладка Подразделения ==========
            tabPageDepartments = CreateCrudTab("🏢 Подразделения", out dgvDepartments, out flpDepartments);
            btnAddDepartment = AddCrudButton(flpDepartments, "➕ Добавить", Color.FromArgb(46, 204, 113));
            btnEditDepartment = AddCrudButton(flpDepartments, "✏️ Редактировать", Color.FromArgb(241, 196, 15));
            btnDeleteDepartment = AddCrudButton(flpDepartments, "🗑️ Удалить", Color.FromArgb(231, 76, 60));
            Button btnRefreshDepartment = AddCrudButton(flpDepartments, "🔄 Обновить", Color.FromArgb(52, 152, 219));
            btnRefreshDepartment.Click += (s, e) => LoadDepartments();

            // ========== Вкладка Оборудование ==========
            tabPageEquipment = CreateCrudTab("🖥️ Оборудование", out dgvEquipment, out flpEquipment);
            btnAddEquipment = AddCrudButton(flpEquipment, "➕ Добавить", Color.FromArgb(46, 204, 113));
            btnEditEquipment = AddCrudButton(flpEquipment, "✏️ Редактировать", Color.FromArgb(241, 196, 15));
            btnDeleteEquipment = AddCrudButton(flpEquipment, "🗑️ Удалить", Color.FromArgb(231, 76, 60));
            Button btnRefreshEquipment = AddCrudButton(flpEquipment, "🔄 Обновить", Color.FromArgb(52, 152, 219));
            btnRefreshEquipment.Click += (s, e) => LoadEquipment();

            // ========== Вкладка Программное обеспечение ==========
            tabPageSoftware = CreateCrudTab("💾 Программное обеспечение", out dgvSoftware, out flpSoftware);
            btnAddSoftware = AddCrudButton(flpSoftware, "➕ Добавить", Color.FromArgb(46, 204, 113));
            btnEditSoftware = AddCrudButton(flpSoftware, "✏️ Редактировать", Color.FromArgb(241, 196, 15));
            btnDeleteSoftware = AddCrudButton(flpSoftware, "🗑️ Удалить", Color.FromArgb(231, 76, 60));
            Button btnRefreshSoftware = AddCrudButton(flpSoftware, "🔄 Обновить", Color.FromArgb(52, 152, 219));
            btnRefreshSoftware.Click += (s, e) => LoadSoftware();

            // ========== Вкладка Заявки ==========
            tabPageRequests = CreateCrudTab("📋 Заявки на сопровождение", out dgvRequests, out flpRequests);
            btnAddRequest = AddCrudButton(flpRequests, "➕ Добавить", Color.FromArgb(46, 204, 113));
            btnEditRequest = AddCrudButton(flpRequests, "✏️ Редактировать", Color.FromArgb(241, 196, 15));
            btnDeleteRequest = AddCrudButton(flpRequests, "🗑️ Удалить", Color.FromArgb(231, 76, 60));
            Button btnRefreshRequest = AddCrudButton(flpRequests, "🔄 Обновить", Color.FromArgb(52, 152, 219));
            btnRefreshRequest.Click += (s, e) => LoadRequests();

            tabControl.Controls.Add(tabPageDashboard);
            tabControl.Controls.Add(tabPageEmployees);
            tabControl.Controls.Add(tabPageDepartments);
            tabControl.Controls.Add(tabPageEquipment);
            tabControl.Controls.Add(tabPageSoftware);
            tabControl.Controls.Add(tabPageRequests);

            // StatusStrip
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel($"✅ Добро пожаловать, {_userName} ({_userRole})");
            statusLabel.Spring = true;
            userLabel = new ToolStripStatusLabel(_userRole == "Admin" ? "👑 Полный доступ" : "👤 Только создание заявок");
            statusStrip.Items.Add(statusLabel);
            statusStrip.Items.Add(userLabel);

            this.Controls.Add(tabControl);
            this.Controls.Add(statusStrip);
            this.Controls.Add(menuStrip);
        }

        private Button CreateCardButton(string text, int x, int y, int width, int height, Color backColor)
        {
            Button btn = new Button
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(width, height),
                BackColor = backColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            return btn;
        }

        private DataGridView CreateDataGridView(int x, int y, int width, int height)
        {
            DataGridView dgv = new DataGridView
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            StyleDataGridView(dgv);
            return dgv;
        }

        private Panel CreateDashboardCard(int x, int y, int width, int height)
        {
            Panel card = new Panel
            {
                Size = new Size(width, height),
                Location = new Point(x, y),
                BackColor = Color.White,
                Padding = new Padding(10)
            };

            card.Paint += (s, e) =>
            {
                var path = new GraphicsPath();
                int radius = 20;
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddArc(width - radius, 0, radius, radius, 270, 90);
                path.AddArc(width - radius, height - radius, radius, radius, 0, 90);
                path.AddArc(0, height - radius, radius, radius, 90, 90);
                path.CloseFigure();
                card.Region = new Region(path);
            };

            return card;
        }

        private void StyleDataGridView(DataGridView dgv)
        {
            if (dgv == null) return;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(226, 232, 240);
            dgv.RowHeadersVisible = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.AllowUserToAddRows = false;
            dgv.ReadOnly = true;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(67, 97, 238);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 40;

            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(45, 55, 72);
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(67, 97, 238);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 35;
        }

        private TabPage CreateCrudTab(string title, out DataGridView dgv, out FlowLayoutPanel flp)
        {
            TabPage tab = new TabPage(title);
            tab.BackColor = Color.FromArgb(245, 245, 250);
            tab.Padding = new Padding(15);

            flp = new FlowLayoutPanel
            {
                Location = new Point(15, 15),
                Size = new Size(600, 50),
                FlowDirection = FlowDirection.LeftToRight
            };

            dgv = new DataGridView
            {
                Location = new Point(15, 75),
                Size = new Size(tabControl.Width - 45, tabControl.Height - 110),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            StyleDataGridView(dgv);

            tab.Controls.Add(flp);
            tab.Controls.Add(dgv);
            return tab;
        }

        private Button AddCrudButton(FlowLayoutPanel panel, string text, Color backColor)
        {
            Button btn = new Button
            {
                Text = text,
                Size = new Size(115, 38),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            panel.Controls.Add(btn);
            return btn;
        }

        private void SetupEvents()
        {
            btnBestRequester.Click += BtnBestRequester_Click;
            btnTopSoftware.Click += BtnTopSoftware_Click;
            btnNoProblems.Click += BtnNoProblems_Click;
            btnRefreshAll.Click += BtnRefreshAll_Click;

            if (_userRole == "Admin")
            {
                btnAddEmployee.Click += (s, e) => ShowInputForm("Employee");
                btnEditEmployee.Click += (s, e) => EditRecord("Employee");
                btnDeleteEmployee.Click += (s, e) => DeleteRecord("Employee");

                btnAddDepartment.Click += (s, e) => ShowInputForm("Department");
                btnEditDepartment.Click += (s, e) => EditRecord("Department");
                btnDeleteDepartment.Click += (s, e) => DeleteRecord("Department");

                btnAddEquipment.Click += (s, e) => ShowInputForm("Equipment");
                btnEditEquipment.Click += (s, e) => EditRecord("Equipment");
                btnDeleteEquipment.Click += (s, e) => DeleteRecord("Equipment");

                btnAddSoftware.Click += (s, e) => ShowInputForm("Software");
                btnEditSoftware.Click += (s, e) => EditRecord("Software");
                btnDeleteSoftware.Click += (s, e) => DeleteRecord("Software");

                btnEditRequest.Click += (s, e) => EditRecord("Request");
                btnDeleteRequest.Click += (s, e) => DeleteRecord("Request");
            }

            btnAddRequest.Click += (s, e) => ShowInputForm("Request");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAllTables();
        }

        private void LoadAllTables()
        {
            LoadEmployees();
            LoadDepartments();
            LoadEquipment();
            LoadSoftware();
            LoadRequests();
        }

        private void LoadEmployees() => dgvEmployees.DataSource = DatabaseHelper.GetEmployees();
        private void LoadDepartments() => dgvDepartments.DataSource = DatabaseHelper.GetDepartments();
        private void LoadEquipment() => dgvEquipment.DataSource = DatabaseHelper.GetEquipment();
        private void LoadSoftware() => dgvSoftware.DataSource = DatabaseHelper.GetSoftware();
        private void LoadRequests()
        {
            if (_userRole == "Admin")
                dgvRequests.DataSource = DatabaseHelper.GetSupportRequests();
            else
            {
                string sql = $"SELECT * FROM SupportRequests WHERE EmployeeID = {_userId} ORDER BY RequestDate DESC";
                dgvRequests.DataSource = DatabaseHelper.ExecuteQuery(sql);
            }
        }

        private void BtnBestRequester_Click(object sender, EventArgs e)
        {
            string sql = @"
                SELECT TOP 1 Employees.FullName AS Заявитель, 
                       Count(SupportRequests.RequestID) AS КоличествоЗаявок
                FROM Employees 
                INNER JOIN SupportRequests ON Employees.EmployeeID = SupportRequests.EmployeeID
                WHERE Month(SupportRequests.RequestDate) = Month(Date()) 
                  AND Year(SupportRequests.RequestDate) = Year(Date())
                GROUP BY Employees.FullName
                ORDER BY Count(SupportRequests.RequestID) DESC;";

            DataTable result = DatabaseHelper.ExecuteQuery(sql);
            if (result != null && result.Rows.Count > 0)
            {
                dgvBestRequester.DataSource = result;
                string name = result.Rows[0]["Заявитель"].ToString();
                int count = Convert.ToInt32(result.Rows[0]["КоличествоЗаявок"]);
                lblBestRequesterResult.Text = $"🏆 Самый активный заявитель: {name} — {count} заявок";
            }
            else
            {
                dgvBestRequester.DataSource = null;
                lblBestRequesterResult.Text = "⚠ За текущий месяц нет заявок";
            }
        }

        private void BtnTopSoftware_Click(object sender, EventArgs e)
        {
            string sql = @"
                SELECT TOP 1 Software.SoftwareName AS НазваниеПрограммы, 
                       Count(Installations.InstallationID) AS КоличествоУстановок
                FROM Software 
                INNER JOIN Installations ON Software.SoftwareID = Installations.SoftwareID
                GROUP BY Software.SoftwareName
                ORDER BY Count(Installations.InstallationID) DESC;";

            DataTable result = DatabaseHelper.ExecuteQuery(sql);
            if (result != null && result.Rows.Count > 0)
            {
                dgvTopSoftware.DataSource = result;
                string name = result.Rows[0]["НазваниеПрограммы"].ToString();
                int count = Convert.ToInt32(result.Rows[0]["КоличествоУстановок"]);
                lblTopSoftwareResult.Text = $"💻 Самая популярная программа: {name} — {count} установок";
            }
            else
            {
                dgvTopSoftware.DataSource = null;
                lblTopSoftwareResult.Text = "⚠ Данные о программах не найдены";
            }
        }

        private void BtnNoProblems_Click(object sender, EventArgs e)
        {
            string sql = @"
                SELECT DISTINCT Departments.DepartmentName AS Аудитория
                FROM Departments 
                WHERE Departments.DepartmentID NOT IN (
                    SELECT DISTINCT Equipment.DepartmentID
                    FROM Equipment 
                    INNER JOIN SupportRequests ON Equipment.EquipmentID = SupportRequests.EquipmentID
                    WHERE SupportRequests.RequestID Is Not Null
                )
                ORDER BY Departments.DepartmentName;";

            DataTable result = DatabaseHelper.ExecuteQuery(sql);
            if (result != null && result.Rows.Count > 0)
            {
                dgvNoProblems.DataSource = result;
                lblNoProblemsResult.Text = $"📚 Найдено аудиторий без проблем: {result.Rows.Count}";
            }
            else
            {
                dgvNoProblems.DataSource = null;
                lblNoProblemsResult.Text = "⚠ Нет аудиторий без проблем";
            }
        }

        private void BtnRefreshAll_Click(object sender, EventArgs e)
        {
            btnRefreshAll.Enabled = false;
            btnRefreshAll.Text = "⏳ Обновление...";
            Application.DoEvents();

            BtnBestRequester_Click(sender, e);
            BtnTopSoftware_Click(sender, e);
            BtnNoProblems_Click(sender, e);
            LoadAllTables();

            btnRefreshAll.Text = "🔄 Обновить всё";
            btnRefreshAll.Enabled = true;
            statusLabel.Text = "✅ Все данные обновлены";
        }

        // ==================== CRUD Методы ====================

        private void ShowInputForm(string tableName)
        {
            var form = new DataEntryForm(tableName);
            if (form.ShowDialog() == DialogResult.OK)
            {
                RefreshTable(tableName);
                statusLabel.Text = $"✅ Запись добавлена в таблицу {GetTableName(tableName)}";
            }
        }

        private void EditRecord(string tableName)
        {
            DataGridView dgv = GetDataGridView(tableName);
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите запись для редактирования.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = GetIdFromDataGridView(dgv, tableName);
            if (id > 0)
            {
                var form = new DataEntryForm(tableName, id);
                if (form.ShowDialog() == DialogResult.OK)
                {
                    RefreshTable(tableName);
                    statusLabel.Text = $"✅ Запись обновлена в таблице {GetTableName(tableName)}";
                }
            }
        }

        private void DeleteRecord(string tableName)
        {
            DataGridView dgv = GetDataGridView(tableName);
            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления.", "Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = GetIdFromDataGridView(dgv, tableName);
            if (id > 0)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?\n\n" +
                    "ВНИМАНИЕ: Это действие нельзя отменить!", "Подтверждение удаления",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    int rows = 0;
                    switch (tableName)
                    {
                        case "Employee": rows = DatabaseHelper.DeleteEmployee(id); break;
                        case "Department": rows = DatabaseHelper.DeleteDepartment(id); break;
                        case "Equipment": rows = DatabaseHelper.DeleteEquipment(id); break;
                        case "Software": rows = DatabaseHelper.DeleteSoftware(id); break;
                        case "Request": rows = DatabaseHelper.DeleteRequest(id); break;
                    }

                    if (rows > 0)
                    {
                        RefreshTable(tableName);
                        statusLabel.Text = $"✅ Запись удалена из таблицы {GetTableName(tableName)}";
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при удалении записи. Возможно, есть связанные данные.",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RefreshTable(string tableName)
        {
            switch (tableName)
            {
                case "Employee": LoadEmployees(); break;
                case "Department": LoadDepartments(); break;
                case "Equipment": LoadEquipment(); break;
                case "Software": LoadSoftware(); break;
                case "Request": LoadRequests(); break;
            }
        }

        private DataGridView GetDataGridView(string tableName)
        {
            switch (tableName)
            {
                case "Employee": return dgvEmployees;
                case "Department": return dgvDepartments;
                case "Equipment": return dgvEquipment;
                case "Software": return dgvSoftware;
                case "Request": return dgvRequests;
                default: return null;
            }
        }

        private int GetIdFromDataGridView(DataGridView dgv, string tableName)
        {
            if (dgv.SelectedRows.Count == 0) return -1;

            string idField = "";
            switch (tableName)
            {
                case "Employee": idField = "EmployeeID"; break;
                case "Department": idField = "DepartmentID"; break;
                case "Equipment": idField = "EquipmentID"; break;
                case "Software": idField = "SoftwareID"; break;
                case "Request": idField = "RequestID"; break;
            }

            if (dgv.SelectedRows[0].Cells[idField]?.Value != null)
            {
                return Convert.ToInt32(dgv.SelectedRows[0].Cells[idField].Value);
            }
            return -1;
        }

        private string GetTableName(string shortName)
        {
            switch (shortName)
            {
                case "Employee": return "Сотрудники";
                case "Department": return "Подразделения";
                case "Equipment": return "Оборудование";
                case "Software": return "Программное обеспечение";
                case "Request": return "Заявки";
                default: return shortName;
            }
        }

        private void InitializeComponent() { }
    }
}