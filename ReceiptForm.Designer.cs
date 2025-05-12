using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ksushenka
{
    partial class ReceiptForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private ComboBox cmbShift;
        private ComboBox cmbMenuItems;
        private NumericUpDown numQuantity;
        private NumericUpDown numDiscount;
        private Button btnAddItem;
        private DataGridView dgvReceipts;
        private GroupBox groupPaymentType;
        private RadioButton rbCash;
        private RadioButton rbCard;
        private Button btnCloseReceipt;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Настройка формы
            this.Text = "Управление чеками";
            this.Size = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Группа выбора смены
            GroupBox groupShift = new GroupBox
            {
                Text = "Выберите смену",
                Location = new Point(20, 20),
                Width = 850,
                Height = 70
            };

            Label lblShift = new Label { Text = "Смена:", Location = new Point(10, 30), Width = 80 };
            cmbShift = new ComboBox { Location = new Point(100, 27), Width = 200 };
            cmbShift.SelectedIndexChanged += cmbShift_SelectedIndexChanged;

            groupShift.Controls.Add(lblShift);
            groupShift.Controls.Add(cmbShift);
            this.Controls.Add(groupShift);

            // Группа добавления товара
            GroupBox groupAddItem = new GroupBox
            {
                Text = "Добавить товар в чек",
                Location = new Point(20, 100),
                Width = 850,
                Height = 100
            };

            Label lblItem = new Label { Text = "Товар:", Location = new Point(10, 30), Width = 80 };
            cmbMenuItems = new ComboBox { Location = new Point(100, 27), Width = 200 };

            Label lblQuantity = new Label { Text = "Количество:", Location = new Point(320, 30), Width = 80 };
            numQuantity = new NumericUpDown { Location = new Point(410, 27), Width = 60, Minimum = 1 };

            Label lblDiscount = new Label { Text = "Скидка (%):", Location = new Point(490, 30), Width = 80 };
            numDiscount = new NumericUpDown { Location = new Point(580, 27), Width = 60, Minimum = 0, Maximum = 100 };

            btnAddItem = new Button
            {
                Text = "Добавить товар",
                Location = new Point(660, 25),
                Width = 120,
                Height = 30
            };
            btnAddItem.Click += btnAddItem_Click;

            groupAddItem.Controls.Add(lblItem);
            groupAddItem.Controls.Add(cmbMenuItems);
            groupAddItem.Controls.Add(lblQuantity);
            groupAddItem.Controls.Add(numQuantity);
            groupAddItem.Controls.Add(lblDiscount);
            groupAddItem.Controls.Add(numDiscount);
            groupAddItem.Controls.Add(btnAddItem);
            this.Controls.Add(groupAddItem);

            // Группа выбора типа оплаты
            groupPaymentType = new GroupBox
            {
                Text = "Тип оплаты",
                Location = new Point(20, 210),
                Width = 200,
                Height = 80
            };

            rbCash = new RadioButton
            {
                Text = "Наличные",
                Location = new Point(20, 20),
                Width = 100,
                Checked = true
            };

            rbCard = new RadioButton
            {
                Text = "Карта",
                Location = new Point(20, 45),
                Width = 100
            };

            groupPaymentType.Controls.Add(rbCash);
            groupPaymentType.Controls.Add(rbCard);
            this.Controls.Add(groupPaymentType);

            // Таблица чеков
            dgvReceipts = new DataGridView
            {
                Location = new Point(20, 300),
                Width = 850,
                Height = 250,
                AutoGenerateColumns = true,
                ReadOnly = true
            };

            // Добавление элементов на форму
            this.Controls.Add(dgvReceipts);

            // Кнопка закрытия чека
            btnCloseReceipt = new Button
            {
                Text = "Оплатить чек",
                Location = new Point(240, 230),
                Width = 150,
                Height = 40,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)
            };
            btnCloseReceipt.Click += btnCloseReceipt_Click;
            this.Controls.Add(btnCloseReceipt);
        }
    }
}