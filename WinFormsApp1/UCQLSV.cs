using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WinFormsApp1
{
    public partial class UCQLSV : UserControl
    {
        private List<SinhVien> danhSachSV = new List<SinhVien>();

        public UCQLSV()
        {
            InitializeComponent();

            // CHẠY TỰ ĐỘNG: Vừa mở giao diện lên là nạp dữ liệu ngay lập tức
            LoadDataToComboBoxLop();
            LoadDataToDataGridView();
        }

        // --- 1. HÀM TẢI DỮ LIỆU LÊN DATAGRIDVIEW ---
        private void LoadDataToDataGridView()
        {
            string connectionString = Database.connectionString;
            string query = "SELECT * FROM SinhVien";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgv_sinhvien.DataSource = dataTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải lại bảng dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- 2. HÀM TẢI DỮ LIỆU LỚP HỌC VÀO COMBOBOX (Đã sửa đổi vị trí) ---
        private void LoadDataToComboBoxLop()
        {
            string connectionString = Database.connectionString;
            string query = "SELECT MaLop, TenLop FROM LopHoc";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Gán nguồn dữ liệu cho ComboBox
                        cbo_lop.DataSource = dataTable;

                        // Cấu hình hiển thị tên lớp và giữ mã lớp ẩn bên dưới
                        cbo_lop.DisplayMember = "TenLop";
                        cbo_lop.ValueMember = "MaLop";

                        // Mặc định ban đầu không chọn lớp nào để trống ô nhập
                        cbo_lop.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải danh sách lớp học: " + ex.Message, "Lỗi Tải Dữ Liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- 3. HÀM XÓA TRỐNG CÁC Ô NHẬP LIỆU ---
        private void ClearTextBoxes()
        {
            txt_masv.Clear();
            txt_hovaten.Clear();
            dtp_ngaysinh.Value = DateTime.Now;

            // Đưa ComboBox về trạng thái chưa chọn mục nào thay vì ép chọn mục 0
            cbo_gioitinh.SelectedIndex = -1;
            cbo_lop.SelectedIndex = -1;
        }

        // --- 4. SỰ KIỆN NÚT THÊM SINH VIÊN ---
        private void btn_them_Click(object sender, EventArgs e)
        {
            string connectionString = Database.connectionString;

            string checkQuery = "SELECT COUNT(*) FROM SinhVien WHERE MaSV = @MaSV";
            string insertQuery = "INSERT INTO SinhVien (MaSV, HoTen, NgaySinh, GioiTinh, MaLop) VALUES (@MaSV, @HoVaTen, @NgaySinh, @GioiTinh, @MaLop)";

            try
            {
                if (string.IsNullOrWhiteSpace(txt_masv.Text) || string.IsNullOrWhiteSpace(txt_hovaten.Text))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Mã sinh viên và Họ và tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Kiểm tra người dùng đã chọn lớp học chưa
                if (cbo_lop.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn Lớp học cho sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // BƯỚC KIỂM TRA TRÙNG MÃ KHÓA CHÍNH:
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@MaSV", txt_masv.Text.Trim());
                        int count = (int)checkCommand.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show($"Mã sinh viên '{txt_masv.Text.Trim()}' đã tồn tại trong hệ thống. Vui lòng nhập mã khác!", "Trùng khóa chính", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // BƯỚC THÊM MỚI SINH VIÊN:
                    using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@MaSV", txt_masv.Text.Trim());
                        insertCommand.Parameters.AddWithValue("@HoVaTen", txt_hovaten.Text.Trim());
                        insertCommand.Parameters.AddWithValue("@NgaySinh", dtp_ngaysinh.Value);
                        insertCommand.Parameters.AddWithValue("@GioiTinh", cbo_gioitinh.Text);

                        // Lấy chính xác mã lớp ngầm (ValueMember) từ ComboBox
                        insertCommand.Parameters.AddWithValue("@MaLop", cbo_lop.SelectedValue);

                        int rowsAffected = insertCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataToDataGridView(); // Tải lại bảng dữ liệu
                            ClearTextBoxes(); // Làm trống các ô nhập
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- 5. SỰ KIỆN NÚT LÀM MỚI ---
        private void btn_lammoi_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            txt_masv.Focus();
        }

        // --- 6. CÁC SỰ KIỆN HỆ THỐNG KHÁC (GIỮ NGUYÊN) ---
        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txt_timkiem.Text;
            MessageBox.Show("Đang tìm kiếm với từ khóa: " + tuKhoa, "Thông báo");
        }

        private void btn_xoa_Click(object sender, EventArgs e) { }
        private void btn_sua_Click(object sender, EventArgs e) { }
        private void dgv_sinhvien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void cbo_lop_SelectedIndexChanged(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label5_Click_1(object sender, EventArgs e) { }
    }
}