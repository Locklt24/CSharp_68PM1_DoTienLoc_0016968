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

        // ==================== BIẾN PHÂN TRANG ====================
        private int currentPage = 1;        // Trang hiện tại
        private int pageSize = 10;           // Số dòng mỗi trang
        private int totalRecords = 0;        // Tổng số bản ghi
        private int totalPages = 0;           // Tổng số trang
        private string currentSearchKeyword = ""; // Lưu từ khóa tìm kiếm

        public UCQLSV()
        {
            InitializeComponent();

            // CHẠY TỰ ĐỘNG: Vừa mở giao diện lên là nạp dữ liệu ngay lập tức
            LoadDataToComboBoxLop();
            LoadDataToDataGridView();

            // Gán sự kiện CellClick cho DataGridView
            dgv_sinhvien.CellClick += dgv_sinhvien_CellClick;
        }

        // --- 1. HÀM TẢI DỮ LIỆU LÊN DATAGRIDVIEW (Có JOIN và PHÂN TRANG) ---
        private void LoadDataToDataGridView()
        {
            string connectionString = Database.connectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // BƯỚC 1: ĐẾM TỔNG SỐ BẢN GHI
                    string countQuery = "";

                    if (!string.IsNullOrEmpty(currentSearchKeyword))
                    {
                        countQuery = @"SELECT COUNT(*) 
                                       FROM SinhVien sv
                                       LEFT JOIN LopHoc lh ON sv.MaLop = lh.MaLop
                                       WHERE sv.MaSV LIKE @tuKhoa 
                                          OR sv.HoTen LIKE @tuKhoa 
                                          OR lh.TenLop LIKE @tuKhoa";
                    }
                    else
                    {
                        countQuery = "SELECT COUNT(*) FROM SinhVien";
                    }

                    using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                    {
                        if (!string.IsNullOrEmpty(currentSearchKeyword))
                        {
                            countCmd.Parameters.AddWithValue("@tuKhoa", "%" + currentSearchKeyword + "%");
                        }
                        totalRecords = (int)countCmd.ExecuteScalar();
                    }

                    // BƯỚC 2: TÍNH TỔNG SỐ TRANG
                    if (totalRecords > 0)
                    {
                        totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    }
                    else
                    {
                        totalPages = 1;
                    }

                    // BƯỚC 3: ĐIỀU CHỈNH TRANG HIỆN TẠI
                    if (currentPage > totalPages)
                        currentPage = totalPages;
                    if (currentPage < 1)
                        currentPage = 1;

                    // BƯỚC 4: TÍNH VỊ TRÍ BẮT ĐẦU VÀ KẾT THÚC
                    int startRow = (currentPage - 1) * pageSize + 1;
                    int endRow = Math.Min(currentPage * pageSize, totalRecords);

                    // BƯỚC 5: LẤY DỮ LIỆU CHO TRANG HIỆN TẠI
                    string dataQuery = "";

                    if (!string.IsNullOrEmpty(currentSearchKeyword))
                    {
                        dataQuery = @"SELECT * FROM (
                                        SELECT ROW_NUMBER() OVER (ORDER BY sv.MaSV) AS RowNum,
                                               sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, 
                                               lh.TenLop, sv.MaLop
                                        FROM SinhVien sv
                                        LEFT JOIN LopHoc lh ON sv.MaLop = lh.MaLop
                                        WHERE sv.MaSV LIKE @tuKhoa 
                                           OR sv.HoTen LIKE @tuKhoa 
                                           OR lh.TenLop LIKE @tuKhoa
                                    ) AS PagedData
                                    WHERE RowNum BETWEEN @StartRow AND @EndRow";
                    }
                    else
                    {
                        dataQuery = @"SELECT * FROM (
                                        SELECT ROW_NUMBER() OVER (ORDER BY sv.MaSV) AS RowNum, 
                                               sv.MaSV, sv.HoTen, sv.NgaySinh, sv.GioiTinh, 
                                               lh.TenLop, sv.MaLop
                                        FROM SinhVien sv
                                        LEFT JOIN LopHoc lh ON sv.MaLop = lh.MaLop
                                    ) AS PagedData
                                    WHERE RowNum BETWEEN @StartRow AND @EndRow";
                    }

                    using (SqlCommand dataCmd = new SqlCommand(dataQuery, connection))
                    {
                        if (!string.IsNullOrEmpty(currentSearchKeyword))
                        {
                            dataCmd.Parameters.AddWithValue("@tuKhoa", "%" + currentSearchKeyword + "%");
                        }
                        dataCmd.Parameters.AddWithValue("@StartRow", startRow);
                        dataCmd.Parameters.AddWithValue("@EndRow", endRow);

                        SqlDataAdapter adapter = new SqlDataAdapter(dataCmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgv_sinhvien.DataSource = dataTable;

                        // Đặt tiêu đề cột và ẩn cột
                        if (dgv_sinhvien.Columns.Count > 0)
                        {
                            if (dgv_sinhvien.Columns.Contains("MaSV"))
                                dgv_sinhvien.Columns["MaSV"].HeaderText = "Mã SV";
                            if (dgv_sinhvien.Columns.Contains("HoTen"))
                                dgv_sinhvien.Columns["HoTen"].HeaderText = "Họ và tên";
                            if (dgv_sinhvien.Columns.Contains("NgaySinh"))
                                dgv_sinhvien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                            if (dgv_sinhvien.Columns.Contains("GioiTinh"))
                                dgv_sinhvien.Columns["GioiTinh"].HeaderText = "Giới tính";
                            if (dgv_sinhvien.Columns.Contains("TenLop"))
                                dgv_sinhvien.Columns["TenLop"].HeaderText = "Tên lớp";
                            if (dgv_sinhvien.Columns.Contains("MaLop"))
                                dgv_sinhvien.Columns["MaLop"].Visible = false;
                            if (dgv_sinhvien.Columns.Contains("RowNum"))
                                dgv_sinhvien.Columns["RowNum"].Visible = false;
                        }
                    }

                    // BƯỚC 6: CẬP NHẬT GIAO DIỆN PHÂN TRANG
                    UpdatePaginationUI();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- HÀM CẬP NHẬT GIAO DIỆN PHÂN TRANG ---
        private void UpdatePaginationUI()
        {
            // Cập nhật label thông tin trang (nếu có label)
            // Bạn có thể thêm label tên là lblPageInfo để hiển thị
            // lblPageInfo.Text = $"Trang {currentPage} / {totalPages} (Tổng: {totalRecords})";

            // Cập nhật trạng thái các nút phân trang
            btnFirst.Enabled = (currentPage > 1 && totalRecords > 0);
            btnPrevious.Enabled = (currentPage > 1 && totalRecords > 0);
            btnNext.Enabled = (currentPage < totalPages && totalRecords > 0);
            btnLast.Enabled = (currentPage < totalPages && totalRecords > 0);
        }

        // --- HÀM LÀM MỚI SAU KHI THÊM/SỬA/XÓA ---
        private void RefreshAfterDataChange()
        {
            currentPage = 1;
            currentSearchKeyword = "";
            txt_timkiem.Clear();
            LoadDataToDataGridView();
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
                            RefreshAfterDataChange(); // Tải lại bảng dữ liệu
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
                            RefreshAfterDataChange(); // Tải lại danh sách
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

        // --- 6. CHỨC NĂNG XÓA SINH VIÊN ---
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn sinh viên chưa
            if (string.IsNullOrEmpty(currentMaSV))
            {
                MessageBox.Show("Vui lòng chọn sinh viên cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xác nhận xóa
            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên '{txt_hovaten.Text.Trim()}' (Mã: {currentMaSV})?",
                                                   "Xác nhận xóa",
                                                   MessageBoxButtons.YesNo,
                                                   MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string connectionString = Database.connectionString;
                string deleteQuery = "DELETE FROM SinhVien WHERE MaSV = @MaSV";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                        {
                            command.Parameters.AddWithValue("@MaSV", currentMaSV);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RefreshAfterDataChange(); // Tải lại danh sách
                                ClearTextBoxes(); // Làm mới form
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sinh viên để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // --- 7. CHỨC NĂNG LÀM MỚI (REFRESH) ---
        private void btn_lammoi_Click(object sender, EventArgs e)
        {
            // Xóa trắng các ô nhập
            ClearTextBoxes();

            // Reset phân trang
            currentPage = 1;
            currentSearchKeyword = "";
            txt_timkiem.Clear();

            // Reload lại dữ liệu từ database
            LoadDataToDataGridView();

            // Đặt con trỏ vào ô Mã SV
            txt_masv.Focus();

            // Thông báo (tùy chọn)
            MessageBox.Show("Đã làm mới dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- 8. SỰ KIỆN CLICK TRÊN DATAGRIDVIEW ---
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

        // --- 9. SỰ KIỆN CELL CONTENT CLICK (XỬ LÝ KHI CLICK VÀO NỘI DUNG CELL) ---
        private void dgv_sinhvien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Gọi lại CellClick để xử lý tương tự
            dgv_sinhvien_CellClick(sender, e);
        }

        // --- 10. CÁC NÚT PHÂN TRANG ---
        private void btnFirst_Click(object sender, EventArgs e)
        {
            if (currentPage != 1 && totalRecords > 0)
            {
                currentPage = 1;
                LoadDataToDataGridView();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1 && totalRecords > 0)
            {
                currentPage--;
                LoadDataToDataGridView();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages && totalRecords > 0)
            {
                currentPage++;
                LoadDataToDataGridView();
            }
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            if (currentPage != totalPages && totalRecords > 0)
            {
                currentPage = totalPages;
                LoadDataToDataGridView();
            }
        }

        // --- 11. CÁC SỰ KIỆN KHÁC ---
        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            currentSearchKeyword = txt_timkiem.Text.Trim();
            currentPage = 1; // Về trang đầu khi tìm kiếm
            LoadDataToDataGridView();

            if (!string.IsNullOrEmpty(currentSearchKeyword))
            {
                MessageBox.Show("Đang tìm kiếm với từ khóa: " + currentSearchKeyword, "Thông báo");
            }
        }

        private void cbo_lop_SelectedIndexChanged(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e) { }

        private void textBox3_TextChanged(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void label4_Click(object sender, EventArgs e) { }

        private void label5_Click(object sender, EventArgs e) { }

        private void label5_Click_1(object sender, EventArgs e) { }

        private void txt_timkiem_TextChanged(object sender, EventArgs e) { }

        private void label7_Click(object sender, EventArgs e) { }
    }
}