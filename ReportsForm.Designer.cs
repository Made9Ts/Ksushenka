using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ksushenka
{
    partial class ReportsForm
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

        private Button btnSalesReport;
        private Button btnTopMenu;
        private DataGridView dgvReport;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Настройка формы
            this.Text = "Отчеты";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Группа кнопок отчетов
            GroupBox groupButtons = new GroupBox
            {
                Text = "Типы отчетов",
                Location = new Point(20, 20),
                Width = 750,
                Height = 80
            };

            btnSalesReport = new Button
            {
                Text = "Выручка по сменам",
                Location = new Point(20, 30),
                Width = 200,
                Height = 30
            };
            btnSalesReport.Click += btnSalesReport_Click;

            btnTopMenu = new Button
            {
                Text = "Топ-5 товаров",
                Location = new Point(240, 30),
                Width = 200,
                Height = 30
            };
            btnTopMenu.Click += btnTopMenu_Click;

            groupButtons.Controls.Add(btnSalesReport);
            groupButtons.Controls.Add(btnTopMenu);
            this.Controls.Add(groupButtons);

            // Таблица отчетов
            dgvReport = new DataGridView
            {
                Location = new Point(20, 110),
                Width = 750,
                Height = 400,
                AutoGenerateColumns = true,
                ReadOnly = true
            };

            // Добавление элементов на форму
            this.Controls.Add(dgvReport);
        }
    }
}