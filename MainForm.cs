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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnShifts_Click(object sender, EventArgs e)
        {
            var shiftForm = new ShiftForm();
            shiftForm.Show();
        }

        private void btnReceipts_Click(object sender, EventArgs e)
        {
            var receiptForm = new ReceiptForm();
            receiptForm.Show();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportsForm();
            reportForm.Show();
        }
    }
}