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
    public partial class ReceiptForm : Form
    {
        private DatabaseContext db = new DatabaseContext();
        private int currentShiftId;

        public ReceiptForm()
        {
            InitializeComponent();
            LoadShifts();
            LoadMenuItems();
        }

        private void LoadShifts()
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand("SELECT shift_id FROM Shifts WHERE status = 'active'", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbShift.Items.Add(reader.GetInt32(0));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке смен: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMenuItems()
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand("SELECT item_id, name, price FROM MenuItems WHERE is_available = TRUE", conn);
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        cmbMenuItems.Items.Add(new
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Price = reader.GetDecimal(2)
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке пунктов меню: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbShift_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentShiftId = Convert.ToInt32(cmbShift.SelectedItem);
            LoadReceipts(currentShiftId);
        }

        private void LoadReceipts(int shiftId)
        {
            try
            {
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(@"
                    SELECT r.receipt_id, r.total_amount, 
                           CASE 
                                WHEN r.payment_type = 'cash' THEN 'Наличные'
                                WHEN r.payment_type = 'card' THEN 'Карта'
                                ELSE r.payment_type
                           END as payment_type,
                           r.status,
                           r.created_at
                    FROM Receipts r
                    WHERE r.shift_id = @shiftId", conn);
                    cmd.Parameters.AddWithValue("shiftId", shiftId);
                    var adapter = new NpgsqlDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    
                    if (table.Columns.Contains("receipt_id"))
                        table.Columns["receipt_id"].ColumnName = "№ чека";
                    if (table.Columns.Contains("total_amount"))
                        table.Columns["total_amount"].ColumnName = "Сумма";
                    if (table.Columns.Contains("payment_type"))
                        table.Columns["payment_type"].ColumnName = "Тип оплаты";
                    if (table.Columns.Contains("status"))
                        table.Columns["status"].ColumnName = "Статус";
                    if (table.Columns.Contains("created_at"))
                        table.Columns["created_at"].ColumnName = "Дата создания";
                        
                    dgvReceipts.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке чеков: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbMenuItems.SelectedItem == null)
                {
                    MessageBox.Show("Выберите пункт меню", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cmbShift.SelectedItem == null)
                {
                    MessageBox.Show("Выберите смену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int receiptId;
                var item = cmbMenuItems.SelectedItem as dynamic;
                int quantity = (int)numQuantity.Value;
                decimal discount = numDiscount.Value;
                string paymentType = rbCash.Checked ? "cash" : "card";

                decimal total = item.Price * quantity * (1 - discount / 100);

                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    
                    // Создаем новый чек или получаем ID текущего открытого чека
                    var receiptCmd = new NpgsqlCommand(@"
                    SELECT receipt_id 
                    FROM Receipts 
                    WHERE shift_id = @shiftId AND status = 'open'
                    LIMIT 1", conn);
                    receiptCmd.Parameters.AddWithValue("shiftId", currentShiftId);
                    
                    var result = receiptCmd.ExecuteScalar();
                    
                    if (result == null)
                    {
                        // Создаем новый чек
                        var newReceiptCmd = new NpgsqlCommand(@"
                        INSERT INTO Receipts (shift_id, total_amount, status, created_at, payment_type)
                        VALUES (@shiftId, 0, 'open', NOW(), @paymentType)
                        RETURNING receipt_id", conn);
                        newReceiptCmd.Parameters.AddWithValue("shiftId", currentShiftId);
                        newReceiptCmd.Parameters.AddWithValue("paymentType", paymentType);
                        receiptId = Convert.ToInt32(newReceiptCmd.ExecuteScalar());
                    }
                    else
                    {
                        receiptId = Convert.ToInt32(result);
                        
                        // Обновляем тип оплаты существующего чека
                        var updatePaymentTypeCmd = new NpgsqlCommand(@"
                        UPDATE Receipts
                        SET payment_type = @paymentType
                        WHERE receipt_id = @receiptId", conn);
                        updatePaymentTypeCmd.Parameters.AddWithValue("receiptId", receiptId);
                        updatePaymentTypeCmd.Parameters.AddWithValue("paymentType", paymentType);
                        updatePaymentTypeCmd.ExecuteNonQuery();
                    }
                    
                    // Добавляем товар в чек
                    var cmd = new NpgsqlCommand(@"
                    INSERT INTO ReceiptDetails (receipt_id, item_id, quantity, price_at_time, discount)
                    VALUES (@receiptId, @itemId, @quantity, @price, @discount)", conn);
                    cmd.Parameters.AddWithValue("receiptId", receiptId);
                    cmd.Parameters.AddWithValue("itemId", item.Id);
                    cmd.Parameters.AddWithValue("quantity", quantity);
                    cmd.Parameters.AddWithValue("price", item.Price);
                    cmd.Parameters.AddWithValue("discount", discount);
                    cmd.ExecuteNonQuery();
                    
                    // Обновляем сумму чека
                    var updateTotalCmd = new NpgsqlCommand(@"
                    UPDATE Receipts
                    SET total_amount = (
                        SELECT SUM(quantity * price_at_time * (1 - discount/100))
                        FROM ReceiptDetails
                        WHERE receipt_id = @receiptId
                    )
                    WHERE receipt_id = @receiptId", conn);
                    updateTotalCmd.Parameters.AddWithValue("receiptId", receiptId);
                    updateTotalCmd.ExecuteNonQuery();
                }
                
                LoadReceipts(currentShiftId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCloseReceipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvReceipts.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Выберите чек для оплаты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                var receiptIdValue = dgvReceipts.SelectedRows[0].Cells["№ чека"].Value;
                if (receiptIdValue == null)
                {
                    MessageBox.Show("Ошибка при определении номера чека", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                int receiptId = Convert.ToInt32(receiptIdValue);
                string paymentType = rbCash.Checked ? "cash" : "card";
                
                using (var conn = db.GetConnection())
                {
                    conn.Open();
                    var cmd = new NpgsqlCommand(@"
                    UPDATE Receipts
                    SET status = 'closed', payment_type = @paymentType
                    WHERE receipt_id = @receiptId", conn);
                    cmd.Parameters.AddWithValue("receiptId", receiptId);
                    cmd.Parameters.AddWithValue("paymentType", paymentType);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Чек успешно оплачен", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadReceipts(currentShiftId);
                    }
                    else
                    {
                        MessageBox.Show("Не удалось оплатить чек", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
