using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;

namespace SchoolISSupport
{
    public static class DatabaseHelper
    {
        private static string _connectionString = null;

        private static string GetConnectionString()
        {
            if (_connectionString != null) return _connectionString;

            string startupPath = Application.StartupPath;
            string[] possibleNames = { "SchoolIS.mdb", "SchoolIS.accdb", "database.mdb", "database.accdb" };
            string foundPath = null;

            foreach (string fileName in possibleNames)
            {
                string fullPath = Path.Combine(startupPath, fileName);
                if (File.Exists(fullPath))
                {
                    foundPath = fullPath;
                    break;
                }
            }

            if (foundPath == null && Directory.Exists(startupPath))
            {
                string[] mdbFiles = Directory.GetFiles(startupPath, "*.mdb");
                if (mdbFiles.Length > 0) foundPath = mdbFiles[0];
                else
                {
                    string[] accdbFiles = Directory.GetFiles(startupPath, "*.accdb");
                    if (accdbFiles.Length > 0) foundPath = accdbFiles[0];
                }
            }

            if (foundPath == null)
            {
                MessageBox.Show($"Файл базы данных не найден!\nИскали в: {startupPath}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }

            string extension = Path.GetExtension(foundPath).ToLower();
            if (extension == ".mdb")
                _connectionString = $@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={foundPath};";
            else
                _connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={foundPath};";

            return _connectionString;
        }

        public static DataTable ExecuteQuery(string sql)
        {
            DataTable dataTable = new DataTable();
            string connString = GetConnectionString();
            if (string.IsNullOrEmpty(connString)) return dataTable;

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    connection.Open();
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(sql, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка БД",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return dataTable;
        }

        public static int ExecuteNonQuery(string sql)
        {
            int rowsAffected = 0;
            string connString = GetConnectionString();
            if (string.IsNullOrEmpty(connString)) return 0;

            try
            {
                using (OleDbConnection connection = new OleDbConnection(connString))
                {
                    connection.Open();
                    using (OleDbCommand command = new OleDbCommand(sql, connection))
                    {
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}\n\nSQL: {sql}", "Ошибка БД",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return rowsAffected;
        }

        public static DataTable GetEmployees() => ExecuteQuery("SELECT * FROM Employees ORDER BY [FullName]");
        public static DataTable GetDepartments() => ExecuteQuery("SELECT * FROM Departments ORDER BY [DepartmentName]");
        public static DataTable GetEquipment() => ExecuteQuery("SELECT * FROM Equipment ORDER BY [InventoryNumber]");
        public static DataTable GetSoftware() => ExecuteQuery("SELECT * FROM Software ORDER BY [SoftwareName]");
        public static DataTable GetSupportRequests() => ExecuteQuery("SELECT * FROM SupportRequests ORDER BY [RequestDate] DESC");

        public static int AddEmployee(string fullName, string position, string phone, string email, string room)
        {
            string sql = $"INSERT INTO Employees ([FullName], [Position], [Phone], [Email], [Room]) VALUES " +
                $"('{Escape(fullName)}', '{Escape(position)}', '{Escape(phone)}', '{Escape(email)}', '{Escape(room)}')";
            return ExecuteNonQuery(sql);
        }

        public static int AddDepartment(string name)
        {
            string sql = $"INSERT INTO Departments ([DepartmentName]) VALUES ('{Escape(name)}')";
            return ExecuteNonQuery(sql);
        }

        public static int AddEquipment(string invNumber, string type, string model, string purchaseDate, string employeeId, string departmentId, string notes)
        {
            string empId = (employeeId == "NULL" || string.IsNullOrEmpty(employeeId)) ? "NULL" : employeeId;
            string depId = (departmentId == "NULL" || string.IsNullOrEmpty(departmentId)) ? "NULL" : departmentId;
            string notesVal = string.IsNullOrEmpty(notes) ? "NULL" : $"'{Escape(notes)}'";

            string sql = $"INSERT INTO Equipment ([InventoryNumber], [Type], [Model], [PurchaseDate], [EmployeeID], [DepartmentID], [Notes]) " +
                $"VALUES ('{Escape(invNumber)}', '{Escape(type)}', '{Escape(model)}', #{purchaseDate}#, {empId}, {depId}, {notesVal})";
            return ExecuteNonQuery(sql);
        }

        public static int AddSoftware(string name, string licenseKey, string expirationDate, string licenseCount)
        {
            string licCount = string.IsNullOrEmpty(licenseCount) ? "NULL" : licenseCount;
            string expDate = string.IsNullOrEmpty(expirationDate) ? "NULL" : $"#{expirationDate}#";

            string sql = $"INSERT INTO Software ([SoftwareName], [LicenseKey], [ExpirationDate], [LicenseCount]) " +
                $"VALUES ('{Escape(name)}', '{Escape(licenseKey)}', {expDate}, {licCount})";
            return ExecuteNonQuery(sql);
        }

        public static int AddRequest(string requestDate, string equipmentId, string employeeId, string problemDesc, string status, string completionDate)
        {
            string eqId = (equipmentId == "NULL" || string.IsNullOrEmpty(equipmentId)) ? "NULL" : equipmentId;
            string empId = (employeeId == "NULL" || string.IsNullOrEmpty(employeeId)) ? "NULL" : employeeId;
            string compDate = (completionDate == "NULL" || string.IsNullOrEmpty(completionDate)) ? "NULL" : $"#{completionDate}#";

            string sql = $"INSERT INTO SupportRequests ([RequestDate], [EquipmentID], [EmployeeID], [ProblemDescription], [Status], [CompletionDate]) " +
                $"VALUES (#{requestDate}#, {eqId}, {empId}, '{Escape(problemDesc)}', '{Escape(status)}', {compDate})";
            return ExecuteNonQuery(sql);
        }

        public static int UpdateEmployee(int id, string fullName, string position, string phone, string email, string room)
        {
            string sql = $"UPDATE Employees SET [FullName]='{Escape(fullName)}', [Position]='{Escape(position)}', " +
                $"[Phone]='{Escape(phone)}', [Email]='{Escape(email)}', [Room]='{Escape(room)}' WHERE [EmployeeID]={id}";
            return ExecuteNonQuery(sql);
        }

        public static int UpdateDepartment(int id, string name)
        {
            string sql = $"UPDATE Departments SET [DepartmentName]='{Escape(name)}' WHERE [DepartmentID]={id}";
            return ExecuteNonQuery(sql);
        }

        public static int UpdateEquipment(int id, string invNumber, string type, string model, string purchaseDate, string employeeId, string departmentId, string notes)
        {
            string empId = (employeeId == "NULL" || string.IsNullOrEmpty(employeeId)) ? "NULL" : employeeId;
            string depId = (departmentId == "NULL" || string.IsNullOrEmpty(departmentId)) ? "NULL" : departmentId;
            string notesVal = string.IsNullOrEmpty(notes) ? "NULL" : $"'{Escape(notes)}'";

            string sql = $"UPDATE Equipment SET [InventoryNumber]='{Escape(invNumber)}', [Type]='{Escape(type)}', " +
                $"[Model]='{Escape(model)}', [PurchaseDate]=#{purchaseDate}#, [EmployeeID]={empId}, " +
                $"[DepartmentID]={depId}, [Notes]={notesVal} WHERE [EquipmentID]={id}";
            return ExecuteNonQuery(sql);
        }

        public static int UpdateSoftware(int id, string name, string licenseKey, string expirationDate, string licenseCount)
        {
            string licCount = string.IsNullOrEmpty(licenseCount) ? "NULL" : licenseCount;
            string expDate = string.IsNullOrEmpty(expirationDate) ? "NULL" : $"#{expirationDate}#";

            string sql = $"UPDATE Software SET [SoftwareName]='{Escape(name)}', [LicenseKey]='{Escape(licenseKey)}', " +
                $"[ExpirationDate]={expDate}, [LicenseCount]={licCount} WHERE [SoftwareID]={id}";
            return ExecuteNonQuery(sql);
        }

        public static int UpdateRequest(int id, string requestDate, string equipmentId, string employeeId, string problemDesc, string status, string completionDate)
        {
            string eqId = (equipmentId == "NULL" || string.IsNullOrEmpty(equipmentId)) ? "NULL" : equipmentId;
            string empId = (employeeId == "NULL" || string.IsNullOrEmpty(employeeId)) ? "NULL" : employeeId;
            string compDate = (completionDate == "NULL" || string.IsNullOrEmpty(completionDate)) ? "NULL" : $"#{completionDate}#";

            string sql = $"UPDATE SupportRequests SET [RequestDate]=#{requestDate}#, [EquipmentID]={eqId}, " +
                $"[EmployeeID]={empId}, [ProblemDescription]='{Escape(problemDesc)}', [Status]='{Escape(status)}', " +
                $"[CompletionDate]={compDate} WHERE [RequestID]={id}";
            return ExecuteNonQuery(sql);
        }

        public static int DeleteEmployee(int id) => ExecuteNonQuery($"DELETE FROM Employees WHERE [EmployeeID]={id}");
        public static int DeleteDepartment(int id) => ExecuteNonQuery($"DELETE FROM Departments WHERE [DepartmentID]={id}");
        public static int DeleteEquipment(int id) => ExecuteNonQuery($"DELETE FROM Equipment WHERE [EquipmentID]={id}");
        public static int DeleteSoftware(int id) => ExecuteNonQuery($"DELETE FROM Software WHERE [SoftwareID]={id}");
        public static int DeleteRequest(int id) => ExecuteNonQuery($"DELETE FROM SupportRequests WHERE [RequestID]={id}");

        private static string Escape(string value)
        {
            return value?.Replace("'", "''") ?? "";
        }

        public static bool TestConnection()
        {
            try
            {
                using (OleDbConnection connection = new OleDbConnection(GetConnectionString()))
                {
                    connection.Open();
                    return true;
                }
            }
            catch { return false; }
        }
    }
}