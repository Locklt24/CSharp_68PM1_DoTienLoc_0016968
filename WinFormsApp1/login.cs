using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Quanlysinhvien;

namespace WinFormsApp1
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            String username = txt_username.Text;
            String password = txt_password.Text;
            if (username == "0016968@st.huce.edu.vn" || password == "0016968")
            {
                MessageBox.Show("dang nhap thanh cong");
                AddDemoLayoutQLSV frmMain = new AddDemoLayoutQLSV();

                // 2. Hiển thị Form mới
                frmMain.Show();

                // 3. Ẩn Form đăng nhập hiện tại (this ở đây là Form Login)
                this.Hide();
            }
            else
            {
                MessageBox.Show("dang nhap that bai");
                Application.Exit();
            }
            //DoTienLoc
        }
    }
}
