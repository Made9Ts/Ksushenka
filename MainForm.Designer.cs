using System;
using System.Drawing;
using System.Windows.Forms;

namespace Ksushenka
{
    partial class MainForm
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

        private Button btnShifts;
        private Button btnReceipts;
        private Button btnReports;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Настройка формы
            this.Text = "Ресторанное ПО";
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Кнопка "Смены"
            btnShifts = new Button
            {
                Text = "Смены",
                Location = new Point(50, 50),
                Width = 120,
                Height = 40,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular)
            };
            btnShifts.Click += btnShifts_Click;

            // Кнопка "Чеки"
            btnReceipts = new Button
            {
                Text = "Чеки",
                Location = new Point(50, 100),
                Width = 120,
                Height = 40,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular)
            };
            btnReceipts.Click += btnReceipts_Click;

            // Кнопка "Отчеты"
            btnReports = new Button
            {
                Text = "Отчеты",
                Location = new Point(50, 150),
                Width = 120,
                Height = 40,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular)
            };
            btnReports.Click += btnReports_Click;

            // Добавление элементов на форму
            this.Controls.Add(btnShifts);
            this.Controls.Add(btnReceipts);
            this.Controls.Add(btnReports);

            // Стиль фона
            this.BackColor = Color.LightBlue;
        }
    }
}

