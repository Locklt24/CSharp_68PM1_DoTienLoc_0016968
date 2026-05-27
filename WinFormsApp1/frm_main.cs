using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq; 

namespace WinFormsApp1
{
    public partial class frm_main : Form
    {
        public frm_main()
        {
            InitializeComponent();
        }
        private void HienThiUserControl(UserControl uc)
        {
            pnl_content.Controls.Clear(); // Xóa trang cũ
            uc.Dock = DockStyle.Fill;     // Ép trang mới tràn đầy khung
            pnl_content.Controls.Add(uc);  // Nạp trang mới vào
        }
        private void frm_main_Load(object sender, EventArgs e)
        {
            HienThiUserControl(new UCQLSV());
        }
        private void quảnLýSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HienThiUserControl(new UCQLSV());
        }
        private void quảnLýLớpHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HienThiUserControl(new UCQLLH());
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void quanLiSinhVienToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HienThiUserControl(new UCQLSV());
        }

        private void quanLiLopHocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HienThiUserControl(new UCQLLH());
        }

        private void dangXuatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm_login frmLogin = new frm_login();
        }
    }
}
