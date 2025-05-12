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
    public partial class ReportsForm : Form
    {
        private DatabaseContext db = new DatabaseContext();

        public ReportsForm()
        {
            InitializeComponent();
        }

        private void btnSalesReport_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(@"
                    SELECT s.shift_id, SUM(r.total_amount) AS total_sales
                    FROM Shifts s
                    JOIN Receipts r ON s.shift_id = r.shift_id
                    GROUP BY s.shift_id", conn);
                    var adapter = new NpgsqlDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    dgvReport.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета по продажам: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTopMenu_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(@"
                    SELECT m.name, SUM(rd.quantity) AS total_sold
                    FROM ReceiptDetails rd
                    JOIN MenuItems m ON rd.item_id = m.item_id
                    GROUP BY m.name
                    ORDER BY total_sold DESC LIMIT 5", conn);
                    var adapter = new NpgsqlDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    dgvReport.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета по популярным позициям: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
