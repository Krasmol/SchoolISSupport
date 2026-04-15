using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SchoolISSupport
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
            CreateControls();
            if (editId != -1) LoadDataForEdit();
        }

        private void CreateControls()
        {
            this.BackColor = ThemeHelper.LightColor;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            this.Text = _editId == -1 ? $"➕ Добавление записи - {GetTableTitle()}" : $"✏️ Редактирование записи - {GetTableTitle()}";
            this.Size = new Size(550, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(245, 245, 250);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimumSize = new Size(500, 450);

            Label lblTitle = new Label
            {
                Text = _editId == -1 ? $"📝 Добавление новой записи" : $"📝 Редактирование записи",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(20, 20),
                Size = new Size(500, 35),
                TextAlign = ContentAlignment.MiddleCenter
            };

            mainPanel = new FlowLayoutPanel
            {
                Location = new Point(20, 70),
                Size = new Size(500, 380),
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
                Location = new Point(150, 460),
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "❌ Отмена",
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(149, 165, 166),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(290, 460),
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(mainPanel);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);

            CreateInputFields();

            ThemeHelper.StyleButton(btnSave, ThemeHelper.SuccessColor, 10);
            ThemeHelper.StyleButton(btnCancel, ThemeHelper.GrayColor, 10);

            foreach (Control panel in mainPanel.Controls)
            {
                foreach (Control control in panel.Controls)
                {
                    if (control is TextBox txt) ThemeHelper.StyleTextBox(txt);
                    if (control is ComboBox cb) ThemeHelper.StyleComboBox(cb);
                    if (control is DateTimePicker dtp) dtp.Font = new Font("Segoe UI", 10);

                }
            }
        }

        private string GetTableTitle()
        {
            switch (_tableName)
            {
                case "Employee": return "Сотрудники";
                case "Department": return "Подразделения";
                case "Equipment": return "Оборудование";
                case "Software": return "Программное обеспечение";
                case "Request": return "Заявки";
                default: return _tableName;
            }
        }

        private void CreateInputFields()
        {
            switch (_tableName)
            {
                case "Employee":
                    AddTextField("FullName", "ФИО", 450);
                    AddTextField("Position", "Должность", 450);
                    AddTextField("Phone", "Телефон", 450);
                    AddTextField("Email", "Email", 450);
                    AddTextField("Room", "Кабинет", 450);
                    break;

                case "Department":
                    AddTextField("DepartmentName", "Название подразделения", 450);
                    break;

                case "Equipment":
                    AddTextField("InventoryNumber", "Инвентарный номер", 450);
                    AddComboBoxField("Type", "Тип оборудования", new[] { "Системный блок", "Ноутбук", "МФУ", "Сервер", "Проектор", "Планшет" });
                    AddTextField("Model", "Модель", 450);
                    AddDateField("PurchaseDate", "Дата приобретения", DateTime.Now);
                    AddComboBoxFromTable("EmployeeID", "Ответственный сотрудник", "Employees", "EmployeeID", "FullName");
                    AddComboBoxFromTable("DepartmentID", "Подразделение", "Departments", "DepartmentID", "DepartmentName");
                    AddTextField("Notes", "Примечание", 450);
                    break;

                case "Software":
                    AddTextField("SoftwareName", "Название программы", 450);
                    AddTextField("LicenseKey", "Лицензионный ключ", 450);
                    AddDateField("ExpirationDate", "Дата окончания лицензии", DateTime.Now.AddYears(1));
                    AddTextField("LicenseCount", "Количество лицензий", 450);
                    break;

                case "Request":
                    AddDateField("RequestDate", "Дата заявки", DateTime.Now);
                    AddComboBoxFromTable("EquipmentID", "Оборудование", "Equipment", "EquipmentID", "InventoryNumber");
                    AddComboBoxFromTable("EmployeeID", "Заявитель", "Employees", "EmployeeID", "FullName");
                    AddTextField("ProblemDescription", "Описание проблемы", 450);
                    AddComboBoxField("Status", "Статус", new[] { "Открыта", "В работе", "Выполнена" });
                    AddDateField("CompletionDate", "Дата выполнения", DateTime.Now);
                    break;
            }
        }

        private void AddTextField(string fieldName, string labelText, int width)
        {
            Panel panel = new Panel { Width = width, Height = 45, Margin = new Padding(0, 0, 0, 8) };

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
                Size = new Size(width - 150, 30),
                BorderStyle = BorderStyle.FixedSingle
            };

            panel.Controls.Add(lbl);
            panel.Controls.Add(txt);
            mainPanel.Controls.Add(panel);
        }

        private void AddDateField(string fieldName, string labelText, DateTime defaultValue)
        {
            Panel panel = new Panel { Width = 450, Height = 45, Margin = new Padding(0, 0, 0, 8) };

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
            Panel panel = new Panel { Width = 450, Height = 45, Margin = new Padding(0, 0, 0, 8) };

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
            if (items.Length > 0) cb.SelectedIndex = 0;

            panel.Controls.Add(lbl);
            panel.Controls.Add(cb);
            mainPanel.Controls.Add(panel);
        }

        private void AddComboBoxFromTable(string fieldName, string labelText, string tableName, string valueField, string displayField)
        {
            Panel panel = new Panel { Width = 450, Height = 45, Margin = new Padding(0, 0, 0, 8) };

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

            var data = DatabaseHelper.ExecuteQuery($"SELECT {valueField}, {displayField} FROM {tableName} ORDER BY {displayField}");
            cb.DataSource = data;

            if (fieldName == "EmployeeID" || fieldName == "DepartmentID")
            {
                DataRow emptyRow = data.NewRow();
                emptyRow[valueField] = DBNull.Value;
                emptyRow[displayField] = "(Не выбрано)";
                data.Rows.InsertAt(emptyRow, 0);
                cb.SelectedIndex = 0;
            }
            else if (data.Rows.Count > 0)
            {
                cb.SelectedIndex = 0;
            }

            panel.Controls.Add(lbl);
            panel.Controls.Add(cb);
            mainPanel.Controls.Add(panel);
        }

        private void LoadDataForEdit()
        {
            string sql = "";
            switch (_tableName)
            {
                case "Employee": sql = $"SELECT * FROM Employees WHERE [EmployeeID] = {_editId}"; break;
                case "Department": sql = $"SELECT * FROM Departments WHERE [DepartmentID] = {_editId}"; break;
                case "Equipment": sql = $"SELECT * FROM Equipment WHERE [EquipmentID] = {_editId}"; break;
                case "Software": sql = $"SELECT * FROM Software WHERE [SoftwareID] = {_editId}"; break;
                case "Request": sql = $"SELECT * FROM SupportRequests WHERE [RequestID] = {_editId}"; break;
            }

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
                int result = 0;

                switch (_tableName)
                {
                    case "Employee": result = SaveEmployee(); break;
                    case "Department": result = SaveDepartment(); break;
                    case "Equipment": result = SaveEquipment(); break;
                    case "Software": result = SaveSoftware(); break;
                    case "Request": result = SaveRequest(); break;
                }

                if (result > 0)
                {
                    MessageBox.Show("✅ Данные успешно сохранены!", "Успех",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Ошибка при сохранении данных!", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFieldValue(Control panel, string fieldName)
        {
            foreach (Control control in panel.Controls)
            {
                if (control.Name == fieldName)
                {
                    if (control is TextBox txt)
                        return txt.Text;
                    if (control is DateTimePicker dtp)
                        return dtp.Value.ToString("yyyy-MM-dd");
                    if (control is ComboBox cb)
                    {
                        if (cb.SelectedValue == null || cb.SelectedValue == DBNull.Value)
                            return "";
                        if (cb.SelectedItem is DataRowView rowView)
                        {
                            if (rowView[cb.DisplayMember]?.ToString() == "(Не выбрано)")
                                return "";
                        }
                        return cb.SelectedValue?.ToString() ?? "";
                    }
                }
            }
            return "";
        }

        private int SaveEmployee()
        {
            string fullName = "", position = "", phone = "", email = "", room = "";

            foreach (Control panel in mainPanel.Controls)
            {
                string fieldName = "";
                foreach (Control control in panel.Controls)
                {
                    if (control is TextBox txt)
                    {
                        fieldName = txt.Name;
                        string value = txt.Text;

                        if (fieldName == "FullName") fullName = value;
                        else if (fieldName == "Position") position = value;
                        else if (fieldName == "Phone") phone = value;
                        else if (fieldName == "Email") email = value;
                        else if (fieldName == "Room") room = value;
                    }
                }
            }

            if (_editId == -1)
                return DatabaseHelper.AddEmployee(fullName, position, phone, email, room);
            else
                return DatabaseHelper.UpdateEmployee(_editId, fullName, position, phone, email, room);
        }

        private int SaveDepartment()
        {
            string name = "";

            foreach (Control panel in mainPanel.Controls)
            {
                foreach (Control control in panel.Controls)
                {
                    if (control is TextBox txt && txt.Name == "DepartmentName")
                    {
                        name = txt.Text;
                    }
                }
            }

            if (_editId == -1)
                return DatabaseHelper.AddDepartment(name);
            else
                return DatabaseHelper.UpdateDepartment(_editId, name);
        }

        private int SaveEquipment()
        {
            string invNumber = "", type = "", model = "", purchaseDate = "", notes = "";
            string employeeId = "", departmentId = "";

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
                            if (cb.SelectedItem is DataRowView rowView)
                            {
                                if (rowView[cb.DisplayMember]?.ToString() == "(Не выбрано)")
                                    value = "";
                                else
                                    value = cb.SelectedValue.ToString();
                            }
                            else
                            {
                                value = cb.SelectedValue.ToString();
                            }
                        }
                    }

                    if (fieldName == "InventoryNumber") invNumber = value;
                    else if (fieldName == "Type") type = value;
                    else if (fieldName == "Model") model = value;
                    else if (fieldName == "PurchaseDate") purchaseDate = value;
                    else if (fieldName == "EmployeeID") employeeId = value;
                    else if (fieldName == "DepartmentID") departmentId = value;
                    else if (fieldName == "Notes") notes = value;
                }
            }

            if (_editId == -1)
                return DatabaseHelper.AddEquipment(invNumber, type, model, purchaseDate, employeeId, departmentId, notes);
            else
                return DatabaseHelper.UpdateEquipment(_editId, invNumber, type, model, purchaseDate, employeeId, departmentId, notes);
        }

        private int SaveSoftware()
        {
            string name = "", licenseKey = "", expirationDate = "", licenseCount = "";

            foreach (Control panel in mainPanel.Controls)
            {
                foreach (Control control in panel.Controls)
                {
                    string fieldName = control.Name;
                    string value = "";

                    if (control is TextBox txt) value = txt.Text;
                    else if (control is DateTimePicker dtp) value = dtp.Value.ToString("yyyy-MM-dd");

                    if (fieldName == "SoftwareName") name = value;
                    else if (fieldName == "LicenseKey") licenseKey = value;
                    else if (fieldName == "ExpirationDate") expirationDate = value;
                    else if (fieldName == "LicenseCount") licenseCount = value;
                }
            }

            if (_editId == -1)
                return DatabaseHelper.AddSoftware(name, licenseKey, expirationDate, licenseCount);
            else
                return DatabaseHelper.UpdateSoftware(_editId, name, licenseKey, expirationDate, licenseCount);
        }

        private int SaveRequest()
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
                            if (cb.SelectedItem is DataRowView rowView)
                            {
                                if (rowView[cb.DisplayMember]?.ToString() == "(Не выбрано)")
                                    value = "";
                                else
                                    value = cb.SelectedValue.ToString();
                            }
                            else
                            {
                                value = cb.SelectedValue.ToString();
                            }
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

            if (_editId == -1)
                return DatabaseHelper.AddRequest(requestDate, equipmentId, employeeId, problemDesc, status, completionDate);
            else
                return DatabaseHelper.UpdateRequest(_editId, requestDate, equipmentId, employeeId, problemDesc, status, completionDate);
        }
    }
}