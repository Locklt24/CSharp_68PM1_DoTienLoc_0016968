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
        private string currentMaSV = ""; // Lưu mã SV đang chọn để sửa/xóa

        public UCQLSV()
        {
            InitializeComponent();

            // CHẠY TỰ ĐỘNG: Vừa mở giao diện lên là nạp dữ liệu ngay lập tức
            LoadDataToComboBoxLop();
            LoadDataToDataGridView();

            // Gán sự kiện CellClick cho DataGridView
            dgv_sinhvien.CellClick += dgv_sinhvien_CellClick;
        }

        // --- 1. HÀM TẢI DỮ LIỆU LÊN DATAGRIDVIEW (Có JOIN để hiển thị tên lớp) ---
        private void LoadDataToDataGridView()
        {
            string connectionString = Database.connectionString;
            // JOIN với bảng LopHoc để lấy tên lớp thay vì mã lớp
            string query = @"SELECT sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, 
                                    lh.TenLop, sv.MaLop
                             FROM SinhVien sv
                             LEFT JOIN LopHoc lh ON sv.MaLop = lh.MaLop";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgv_sinhvien.DataSource = dataTable;

                    // Đặt tiêu đề cột và ẩn cột MaLop
                    if (dgv_sinhvien.Columns.Count > 0)
                    {
                        dgv_sinhvien.Columns["MaSV"].HeaderText = "Mã SV";
                        dgv_sinhvien.Columns["HoTen"].HeaderText = "Họ và tên";
                        dgv_sinhvien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                        dgv_sinhvien.Columns["GioiTinh"].HeaderText = "Giới tính";
                        dgv_sinhvien.Columns["TenLop"].HeaderText = "Tên lớp";
                        dgv_sinhvien.Columns["MaLop"].Visible = false; // Ẩn cột MaLop
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải lại bảng dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- 2. HÀM TẢI DỮ LIỆU LỚP HỌC VÀO COMBOBOX ---
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

            // Đưa ComboBox về trạng thái chưa chọn mục nào
            cbo_gioitinh.SelectedIndex = -1;
            cbo_lop.SelectedIndex = -1;

            currentMaSV = ""; // Xóa mã SV đang lưu
            txt_masv.Enabled = true; // Cho phép nhập mã mới (khi thêm)
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

                // Kiểm tra chọn giới tính
                if (cbo_gioitinh.SelectedIndex == -1)
                {
                    MessageBox.Show("Vui lòng chọn Giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        // --- 5. CHỨC NĂNG SỬA SINH VIÊN ---
        private void btn_sua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentMaSV))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_hovaten.Text))
            {
                MessageBox.Show("Vui lòng nhập Họ và tên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbo_lop.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Lớp học!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbo_gioitinh.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Giới tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = Database.connectionString;
            string updateQuery = @"UPDATE SinhVien 
                                   SET HoTen = @HoTen, 
                                       NgaySinh = @NgaySinh, 
                                       GioiTinh = @GioiTinh, 
                                       MaLop = @MaLop
                                   WHERE MaSV = @MaSV";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        // Thêm tham số cho câu lệnh SQL
                        command.Parameters.AddWithValue("@MaSV", currentMaSV);
                        command.Parameters.AddWithValue("@HoTen", txt_hovaten.Text.Trim());
                        command.Parameters.AddWithValue("@NgaySinh", dtp_ngaysinh.Value);
                        command.Parameters.AddWithValue("@GioiTinh", cbo_gioitinh.Text);
                        command.Parameters.AddWithValue("@MaLop", cbo_lop.SelectedValue);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadDataToDataGridView(); // Tải lại danh sách
                            ClearTextBoxes(); // Làm mới form
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy sinh viên để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_lammoi_Click(object sender, EventArgs e)
        {
            ClearTextBoxes();
            txt_masv.Focus();
        }

        private void dgv_sinhvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có click vào dòng hợp lệ không (không phải header)
            if (e.RowIndex >= 0)
            {
                // Lấy dòng được chọn
                DataGridViewRow row = dgv_sinhvien.Rows[e.RowIndex];

                // Lấy dữ liệu từ các cột
                currentMaSV = row.Cells["MaSV"].Value.ToString();
                txt_masv.Text = currentMaSV;
                txt_hovaten.Text = row.Cells["HoTen"].Value.ToString();

                // Xử lý ngày sinh
                if (row.Cells["NgaySinh"].Value != DBNull.Value)
                {
                    dtp_ngaysinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);
                }

                // Xử lý giới tính
                if (row.Cells["GioiTinh"].Value != DBNull.Value)
                {
                    string gioiTinh = row.Cells["GioiTinh"].Value.ToString();
                    // Tìm và chọn giới tính trong ComboBox
                    if (cbo_gioitinh.Items.Contains(gioiTinh))
                    {
                        cbo_gioitinh.SelectedItem = gioiTinh;
                    }
                }

                // Xử lý lớp (dùng MaLop - cột ẩn)
                if (row.Cells["MaLop"].Value != DBNull.Value)
                {
                    string maLop = row.Cells["MaLop"].Value.ToString();
                    cbo_lop.SelectedValue = maLop;
                }
                else
                {
                    cbo_lop.SelectedIndex = -1;
                }

                // Khóa ô mã sinh viên khi sửa (không cho phép thay đổi mã)
                txt_masv.Enabled = false;
            }
        }

        // --- 8. SỰ KIỆN CELL CONTENT CLICK (XỬ LÝ KHI CLICK VÀO NỘI DUNG CELL) ---
        private void dgv_sinhvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Gọi lại CellClick để xử lý tương tự
            dgv_sinhvien_CellClick(sender, e);
        }

        // --- 9. CÁC SỰ KIỆN KHÁC ---
        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txt_timkiem.Text;
            MessageBox.Show("Đang tìm kiếm với từ khóa: " + tuKhoa, "Thông báo");
        }

        private void btn_xoa_Click(object sender, EventArgs e) { }

        private void cbo_lop_SelectedIndexChanged(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void textBox3_TextChanged(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void label5_Click(object sender, EventArgs e) { }

        private void label5_Click_1(object sender, EventArgs e) { }

        private void txt_timkiem_TextChanged(object sender, EventArgs e) { }
    }
}