using SchoolISSupport;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolISSupport_UserVersion
{
    public partial class DataEntryForm : Form
    {
        private string _tableName;
        private int _editId = -1;
        private FlowLayoutPanel mainPanel;
        private Button btnSave;
        private Button btnCancel;

        public DataEntryForm(string tableName, int editId = -1)
        {
            _tableName = tableName;
            _editId = editId;
            InitializeComponent();
            CreateControls();
            if (editId != -1) LoadDataForEdit();
        }

        private void CreateControls()
        {
            this.Text = _editId == -1 ? "➕ Добавление заявки" : "✏️ Редактирование заявки";
            this.Size = new Size(550, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimumSize = new Size(500, 500);

            Label lblTitle = new Label
            {
                Text = _editId == -1 ? "📝 Создание новой заявки" : "📝 Редактирование заявки",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 20),
                Size = new Size(500, 40),
                TextAlign = ContentAlignment.MiddleCenter
            };

            mainPanel = new FlowLayoutPanel
            {
                Location = new Point(20, 70),
                Size = new Size(500, 420),
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                BackColor = Color.White,
                Padding = new Padding(15),
                BorderStyle = BorderStyle.FixedSingle
            };

            btnSave = new Button
            {
                Text = "💾 Сохранить",
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(46, 204, 113),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(150, 500),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "❌ Отмена",
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(290, 500),
                Cursor = Cursors.Hand
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(mainPanel);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            CreateInputFields();
        }

        private void CreateInputFields()
        {
            AddDateField("RequestDate", "Дата заявки", DateTime.Now);
            AddComboBoxFromTable("EquipmentID", "Оборудование", "Equipment", "EquipmentID", "InventoryNumber");
            AddComboBoxFromTable("EmployeeID", "Заявитель", "Employees", "EmployeeID", "FullName");
            AddTextField("ProblemDescription", "Описание проблемы", 450);
            AddComboBoxField("Status", "Статус", new[] { "Открыта", "В работе", "Выполнена" });
            AddDateField("CompletionDate", "Дата выполнения", DateTime.Now);
        }

        private void AddTextField(string fieldName, string labelText, int width)
        {
            Panel panel = new Panel { Width = width, Height = 65, Margin = new Padding(0, 0, 0, 8) };

            Label lbl = new Label
            {
                Text = labelText + ":",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(0, 5),
                Size = new Size(140, 25)
            };

            TextBox txt = new TextBox
            {
                Name = fieldName,
                Tag = fieldName,
                Font = new Font("Segoe UI", 10),
                Location = new Point(145, 3),
                Size = new Size(width - 150, fieldName == "ProblemDescription" ? 50 : 30),
                BorderStyle = BorderStyle.FixedSingle,
                Multiline = fieldName == "ProblemDescription",
                ScrollBars = fieldName == "ProblemDescription" ? ScrollBars.Vertical : ScrollBars.None
            };

            panel.Controls.Add(lbl);
            panel.Controls.Add(txt);
            mainPanel.Controls.Add(panel);
        }

        private void AddDateField(string fieldName, string labelText, DateTime defaultValue)
        {
            Panel panel = new Panel { Width = 450, Height = 50, Margin = new Padding(0, 0, 0, 8) };

            Label lbl = new Label
            {
                Text = labelText + ":",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(0, 5),
                Size = new Size(140, 25)
            };

            DateTimePicker dtp = new DateTimePicker
            {
                Name = fieldName,
                Tag = fieldName,
                Font = new Font("Segoe UI", 10),
                Location = new Point(145, 3),
                Size = new Size(300, 30),
                Format = DateTimePickerFormat.Short,
                Value = defaultValue
            };

            panel.Controls.Add(lbl);
            panel.Controls.Add(dtp);
            mainPanel.Controls.Add(panel);
        }

        private void AddComboBoxField(string fieldName, string labelText, string[] items)
        {
            Panel panel = new Panel { Width = 450, Height = 50, Margin = new Padding(0, 0, 0, 8) };

            Label lbl = new Label
            {
                Text = labelText + ":",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(0, 5),
                Size = new Size(140, 25)
            };

            ComboBox cb = new ComboBox
            {
                Name = fieldName,
                Tag = fieldName,
                Font = new Font("Segoe UI", 10),
                Location = new Point(145, 3),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cb.Items.AddRange(items);
            if (cb.Items.Count > 0) cb.SelectedIndex = 0;

            panel.Controls.Add(lbl);
            panel.Controls.Add(cb);
            mainPanel.Controls.Add(panel);
        }

        private void AddComboBoxFromTable(string fieldName, string labelText, string tableName, string valueField, string displayField)
        {
            Panel panel = new Panel { Width = 450, Height = 50, Margin = new Padding(0, 0, 0, 8) };

            Label lbl = new Label
            {
                Text = labelText + ":",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(0, 5),
                Size = new Size(140, 25)
            };

            ComboBox cb = new ComboBox
            {
                Name = fieldName,
                Tag = fieldName,
                Font = new Font("Segoe UI", 10),
                Location = new Point(145, 3),
                Size = new Size(300, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                DisplayMember = displayField,
                ValueMember = valueField
            };

            try
            {
                var data = DatabaseHelper.ExecuteQuery($"SELECT {valueField}, {displayField} FROM {tableName} ORDER BY {displayField}");
                cb.DataSource = data;

                if (cb.Items.Count > 0) cb.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            panel.Controls.Add(lbl);
            panel.Controls.Add(cb);
            mainPanel.Controls.Add(panel);
        }

        private void LoadDataForEdit()
        {
            string sql = $"SELECT * FROM SupportRequests WHERE [RequestID] = {_editId}";
            var data = DatabaseHelper.ExecuteQuery(sql);
            if (data.Rows.Count > 0)
            {
                var row = data.Rows[0];
                foreach (Control panel in mainPanel.Controls)
                {
                    foreach (Control control in panel.Controls)
                    {
                        if (control is TextBox txt && row.Table.Columns.Contains(txt.Name))
                        {
                            txt.Text = row[txt.Name]?.ToString() ?? "";
                        }
                        else if (control is DateTimePicker dtp && row.Table.Columns.Contains(dtp.Name))
                        {
                            if (row[dtp.Name] != DBNull.Value && row[dtp.Name] != null)
                                dtp.Value = Convert.ToDateTime(row[dtp.Name]);
                        }
                        else if (control is ComboBox cb && row.Table.Columns.Contains(cb.Name))
                        {
                            if (row[cb.Name] != DBNull.Value && row[cb.Name] != null)
                            {
                                for (int i = 0; i < cb.Items.Count; i++)
                                {
                                    var item = cb.Items[i];
                                    var rowView = item as DataRowView;
                                    if (rowView != null)
                                    {
                                        if (rowView[cb.ValueMember].ToString() == row[cb.Name].ToString())
                                        {
                                            cb.SelectedIndex = i;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string requestDate = "", equipmentId = "", employeeId = "", problemDesc = "", status = "", completionDate = "";

                foreach (Control panel in mainPanel.Controls)
                {
                    foreach (Control control in panel.Controls)
                    {
                        string fieldName = control.Name;
                        string value = "";

                        if (control is TextBox txt) value = txt.Text;
                        else if (control is DateTimePicker dtp) value = dtp.Value.ToString("yyyy-MM-dd");
                        else if (control is ComboBox cb)
                        {
                            if (cb.SelectedValue != null && cb.SelectedValue != DBNull.Value)
                            {
                                value = cb.SelectedValue.ToString();
                            }
                        }

                        if (fieldName == "RequestDate") requestDate = value;
                        else if (fieldName == "EquipmentID") equipmentId = value;
                        else if (fieldName == "EmployeeID") employeeId = value;
                        else if (fieldName == "ProblemDescription") problemDesc = value;
                        else if (fieldName == "Status") status = value;
                        else if (fieldName == "CompletionDate") completionDate = value;
                    }
                }

                int result = DatabaseHelper.AddRequest(requestDate, equipmentId, employeeId, problemDesc, status, completionDate);

                if (result > 0)
                {
                    MessageBox.Show("✅ Заявка успешно создана!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Ошибка при создании заявки!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeComponent()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 600);
        }
    }
}