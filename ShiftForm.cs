using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ksushenka
{
    public partial class ShiftForm : Form
    {
        private DatabaseContext db = new DatabaseContext();

        public ShiftForm()
        {
            InitializeComponent();
            LoadEmployees();
            LoadCashRegisters();
            LoadShifts();
        }

        private void LoadEmployees()
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand("SELECT employee_id, full_name FROM Employees", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbEmployee.Items.Add(new { Id = reader.GetInt32(0), Name = reader.GetString(1) });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке сотрудников: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCashRegisters()
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand("SELECT cash_register_id, location FROM CashRegisters", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbCashRegister.Items.Add(new { Id = reader.GetInt32(0), Location = reader.GetString(1) });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке касс: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadShifts()
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(@"
                    SELECT s.shift_id, e.full_name, s.start_time, s.end_time, s.total_sales 
                    FROM Shifts s
                    JOIN Employees e ON s.employee_id = e.employee_id", conn);
                    var adapter = new NpgsqlDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    dgvShifts.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке смен: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStartShift_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbEmployee.SelectedItem == null)
                {
                    MessageBox.Show("Выберите сотрудника", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cmbCashRegister.SelectedItem == null)
                {
                    MessageBox.Show("Выберите кассу", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var employee = cmbEmployee.SelectedItem as dynamic;
                var cashRegister = cmbCashRegister.SelectedItem as dynamic;

                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(@"
                    INSERT INTO Shifts (employee_id, cash_register_id, start_time, status)
                    VALUES (@employeeId, @cashRegisterId, NOW(), 'active')", conn);
                    cmd.Parameters.AddWithValue("employeeId", employee.Id);
                    cmd.Parameters.AddWithValue("cashRegisterId", cashRegister.Id);
                    cmd.ExecuteNonQuery();
                }
                LoadShifts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEndShift_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvShifts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите смену для закрытия", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                var shiftId = Convert.ToInt32(dgvShifts.SelectedRows[0].Cells["shift_id"].Value);

                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(@"
                    UPDATE Shifts 
                    SET end_time = NOW(), status = 'closed' 
                    WHERE shift_id = @shiftId", conn);
                    cmd.Parameters.AddWithValue("shiftId", shiftId);
                    cmd.ExecuteNonQuery();
                }
                LoadShifts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
