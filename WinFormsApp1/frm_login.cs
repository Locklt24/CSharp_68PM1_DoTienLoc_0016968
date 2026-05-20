using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class frm_login : Form
    {
        public frm_login()
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
            // 1. Lấy dữ liệu từ TextBox và dùng hàm Trim() để loại bỏ các khoảng trắng thừa ở đầu/cuối
            string username = txt_username.Text.Trim();
            string password = txt_password.Text.Trim();

            // 2. Kiểm tra nghiệp vụ (Validation): Ngăn chặn việc để trống dữ liệu
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tài khoản và mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Thoát hàm ngay lập tức, không chạy tiếp các lệnh phía dưới
            }

            // 3. Xác thực tài khoản (Đúng thông tin MSSV HUCE)
            if (username == "0016968@st.huce.edu.vn" && password == "0016968")
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Khởi tạo thực thể cho Form chính của dự án
                frm_main frmMain = new frm_main();

                // Đăng ký sự kiện: Nếu Form chính bị đóng (bấm nút X đỏ), ứng dụng sẽ tắt hoàn toàn các tiến trình ngầm
                frmMain.FormClosed += (s, args) => Application.Exit();

                // Hiển thị màn hình chính lên cho người dùng tương tác
                frmMain.Show();

                // Ẩn Form đăng nhập hiện tại vào bộ nhớ RAM ngầm
                this.Hide();
            }
            else
            {
                // 4. Xử lý khi nhập sai tài khoản hoặc mật khẩu
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);

                txt_password.Clear(); // Xóa sạch chữ ở ô mật khẩu cũ
                txt_password.Focus(); // Đưa con trỏ chuột tự động nhấp nháy tại ô mật khẩu để người dùng gõ lại ngay
            }
        }
    }
}
