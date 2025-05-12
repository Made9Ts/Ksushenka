using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ksushenka
{
    partial class ShiftForm
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

        private ComboBox cmbEmployee;
        private ComboBox cmbCashRegister;
        private DataGridView dgvShifts;
        private Button btnStartShift;
        private Button btnEndShift;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Настройка формы
            this.Text = "Управление сменами";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Группа для выбора сотрудника и кассы
            GroupBox groupBox = new GroupBox
            {
                Text = "Начать новую смену",
                Location = new Point(20, 20),
                Width = 750,
                Height = 100
            };

            // Выбор сотрудника
            Label lblEmployee = new Label { Text = "Сотрудник:", Location = new Point(10, 30), Width = 80 };
            cmbEmployee = new ComboBox { Location = new Point(100, 27), Width = 200 };

            // Выбор кассы
            Label lblCashRegister = new Label { Text = "Касса:", Location = new Point(320, 30), Width = 80 };
            cmbCashRegister = new ComboBox { Location = new Point(400, 27), Width = 200 };

            // Кнопка "Начать смену"
            btnStartShift = new Button
            {
                Text = "Начать смену",
                Location = new Point(620, 25),
                Width = 100,
                Height = 30
            };
            btnStartShift.Click += btnStartShift_Click;

            // Добавление элементов в группу
            groupBox.Controls.Add(lblEmployee);
            groupBox.Controls.Add(cmbEmployee);
            groupBox.Controls.Add(lblCashRegister);
            groupBox.Controls.Add(cmbCashRegister);
            groupBox.Controls.Add(btnStartShift);
            this.Controls.Add(groupBox);

            // Таблица смен
            dgvShifts = new DataGridView
            {
                Location = new Point(20, 130),
                Width = 750,
                Height = 350,
                AutoGenerateColumns = true,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Кнопка "Закончить смену"
            btnEndShift = new Button
            {
                Text = "Закончить смену",
                Location = new Point(20, 500),
                Width = 150,
                Height = 40
            };
            btnEndShift.Click += btnEndShift_Click;

            // Добавление элементов на форму
            this.Controls.Add(dgvShifts);
            this.Controls.Add(btnEndShift);
        }
    }
}