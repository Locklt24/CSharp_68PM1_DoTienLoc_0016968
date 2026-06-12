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
    public partial class UCQLLH : UserControl
    {
        // ==================== BIẾN TOÀN CỤC ====================
        private int currentID = -1;           // Lưu ID đang chọn để sửa/xóa
        private string currentMaLop = "";     // Lưu mã lớp đang chọn

        // ==================== BIẾN PHÂN TRANG ====================
        private int currentPage = 1;           // Trang hiện tại
        private int pageSize = 10;              // Số dòng mỗi trang
        private int totalRecords = 0;           // Tổng số bản ghi
        private int totalPages = 0;              // Tổng số trang
        private string currentSearchKeyword = ""; // Từ khóa tìm kiếm

        public UCQLLH()
        {
            InitializeComponent();
            LoadDataToDataGridView();
            SetupPaginationControls();
            SetupButtonEvents();
        }

        // ==================== KHỞI TẠO SỰ KIỆN CHO CÁC NÚT ====================
        private void SetupButtonEvents()
        {
            // Gán sự kiện cho các nút CRUD
            if (btn_them != null) btn_them.Click += btn_them_Click;
            if (btn_sua != null) btn_sua.Click += btn_sua_Click_1;
            if (btn_xoa != null) btn_xoa.Click += btn_xoa_Click;
            if (btn_lammoi != null) btn_lammoi.Click += btn_lammoi_Click;
            if (btn_timkiem != null) btn_timkiem.Click += btn_timkiem_Click;
        }

        // ==================== KHỞI TẠO ĐIỀU KHIỂN PHÂN TRANG ====================
        private void SetupPaginationControls()
        {
            if (btnFirst != null) btnFirst.Click += btnFirst_Click_1;
            if (btnPrevious != null) btnPrevious.Click += btnPrevious_Click;
            if (btnNext != null) btnNext.Click += btnNext_Click;
            if (btnLast != null) btnLast.Click += btnLast_Click;

            if (numPageSize != null)
            {
                numPageSize.Value = pageSize;
                numPageSize.Minimum = 1;
                numPageSize.Maximum = 100;
                numPageSize.ValueChanged += numPageSize_ValueChanged;
            }

            if (dgvLopHoc != null) dgvLopHoc.CellClick += dgvLopHoc_CellClick;
            if (lblPageInfo != null) lblPageInfo.Click += lblPageInfo_Click;
        }

        // ==================== 1. HÀM TẢI DỮ LIỆU LÊN DATAGRIDVIEW (CÓ PHÂN TRANG) ====================
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
                                       FROM LopHoc
                                       WHERE MaLop LIKE @tuKhoa 
                                          OR TenLop LIKE @tuKhoa";
                    }
                    else
                    {
                        countQuery = "SELECT COUNT(*) FROM LopHoc";
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

                    if (totalRecords == 0)
                    {
                        dgvLopHoc.DataSource = null;
                        UpdatePaginationUI();
                        return;
                    }

                    // BƯỚC 5: LẤY DỮ LIỆU CHO TRANG HIỆN TẠI
                    string dataQuery = "";

                    if (!string.IsNullOrEmpty(currentSearchKeyword))
                    {
                        dataQuery = @"SELECT * FROM (
                                        SELECT ROW_NUMBER() OVER (ORDER BY ID) AS RowNum,
                                               ID, MaLop, TenLop, ISNULL(GhiChu, '') AS GhiChu
                                        FROM LopHoc
                                        WHERE MaLop LIKE @tuKhoa 
                                           OR TenLop LIKE @tuKhoa
                                    ) AS PagedData
                                    WHERE RowNum BETWEEN @StartRow AND @EndRow";
                    }
                    else
                    {
                        dataQuery = @"SELECT * FROM (
                                        SELECT ROW_NUMBER() OVER (ORDER BY ID) AS RowNum,
                                               ID, MaLop, TenLop, ISNULL(GhiChu, '') AS GhiChu
                                        FROM LopHoc
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

                        dgvLopHoc.DataSource = dataTable;

                        // Đặt tiêu đề cột
                        if (dgvLopHoc.Columns.Count > 0)
                        {
                            if (dgvLopHoc.Columns.Contains("ID"))
                                dgvLopHoc.Columns["ID"].HeaderText = "Mã ID";
                            if (dgvLopHoc.Columns.Contains("MaLop"))
                                dgvLopHoc.Columns["MaLop"].HeaderText = "Mã lớp";
                            if (dgvLopHoc.Columns.Contains("TenLop"))
                                dgvLopHoc.Columns["TenLop"].HeaderText = "Tên lớp";
                            if (dgvLopHoc.Columns.Contains("GhiChu"))
                                dgvLopHoc.Columns["GhiChu"].HeaderText = "Ghi chú";
                            if (dgvLopHoc.Columns.Contains("RowNum"))
                                dgvLopHoc.Columns["RowNum"].Visible = false;
                        }
                    }

                    UpdatePaginationUI();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải dữ liệu: " + ex.Message, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== CẬP NHẬT GIAO DIỆN PHÂN TRANG ====================
        private void UpdatePaginationUI()
        {
            if (lblPageInfo != null)
            {
                if (totalRecords == 0)
                    lblPageInfo.Text = "Không có dữ liệu";
                else
                    lblPageInfo.Text = $"Trang {currentPage} / {totalPages} | {totalRecords} bản ghi";
            }

            if (btnFirst != null) btnFirst.Enabled = (currentPage > 1 && totalRecords > 0);
            if (btnPrevious != null) btnPrevious.Enabled = (currentPage > 1 && totalRecords > 0);
            if (btnNext != null) btnNext.Enabled = (currentPage < totalPages && totalRecords > 0);
            if (btnLast != null) btnLast.Enabled = (currentPage < totalPages && totalRecords > 0);
        }

        // ==================== LÀM MỚI SAU KHI THÊM/SỬA/XÓA ====================
        private void RefreshAfterDataChange()
        {
            currentPage = 1;
            currentSearchKeyword = "";
            if (txtTimKiem != null) txtTimKiem.Clear();
            LoadDataToDataGridView();
        }

        // ==================== XÓA TRỐNG CÁC Ô NHẬP ====================
        private void ClearInputs()
        {
            if (txtID != null) txtID.Clear();
            if (txtMaLop != null) txtMaLop.Clear();
            if (txtTenLop != null) txtTenLop.Clear();
            if (txtGhiChu != null) txtGhiChu.Clear();
            currentID = -1;
            currentMaLop = "";
            if (txtMaLop != null) txtMaLop.Enabled = true;
            if (txtID != null) txtID.Enabled = true;
        }

        // ==================== KIỂM TRA DỮ LIỆU NHẬP ====================
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtMaLop.Text))
            {
                MessageBox.Show("Vui lòng nhập Mã lớp!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaLop.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtTenLop.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên lớp!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenLop.Focus();
                return false;
            }

            return true;
        }

        // ==================== 2. CHỨC NĂNG THÊM LỚP HỌC ====================
        private void btn_them_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            string connectionString = Database.connectionString;
            string checkQuery = "SELECT COUNT(*) FROM LopHoc WHERE MaLop = @MaLop";
            string insertQuery = "INSERT INTO LopHoc (MaLop, TenLop, GhiChu) VALUES (@MaLop, @TenLop, @GhiChu)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            MessageBox.Show($"Mã lớp '{txtMaLop.Text.Trim()}' đã tồn tại!",
                                          "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            txtMaLop.Focus();
                            return;
                        }
                    }

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@TenLop", txtTenLop.Text.Trim());
                        insertCmd.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm lớp học thành công!", "Thông báo",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshAfterDataChange();
                            ClearInputs();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi Hệ Thống",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== 3. CHỨC NĂNG SỬA LỚP HỌC ====================
        private void btn_sua_Click_1(object sender, EventArgs e)
        {
            if (currentID == -1)
            {
                MessageBox.Show("Vui lòng chọn lớp cần sửa từ danh sách!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ValidateInput()) return;

            string connectionString = Database.connectionString;
            string updateQuery = "UPDATE LopHoc SET TenLop = @TenLop, GhiChu = @GhiChu WHERE ID = @ID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", currentID);
                        command.Parameters.AddWithValue("@TenLop", txtTenLop.Text.Trim());
                        command.Parameters.AddWithValue("@GhiChu", txtGhiChu.Text.Trim());

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Cập nhật lớp học thành công!", "Thông báo",
                                          MessageBoxButtons.OK, MessageBoxIcon.Information);
                            RefreshAfterDataChange();
                            ClearInputs();
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy lớp học để cập nhật!", "Lỗi",
                                          MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi Hệ Thống",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== 4. CHỨC NĂNG XÓA LỚP HỌC (ĐÃ FIX) ====================
        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (currentID == -1)
            {
                MessageBox.Show("Vui lòng chọn lớp cần xóa từ danh sách!", "Thông báo",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = Database.connectionString;
            string checkStudentQuery = "SELECT COUNT(*) FROM SinhVien WHERE MaLop = @MaLop";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Lấy mã lớp từ currentMaLop
                    string maLopToCheck = currentMaLop;

                    using (SqlCommand checkCmd = new SqlCommand(checkStudentQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@MaLop", maLopToCheck);
                        int studentCount = (int)checkCmd.ExecuteScalar();

                        if (studentCount > 0)
                        {
                            MessageBox.Show($"Không thể xóa lớp '{txtTenLop.Text}' vì có {studentCount} sinh viên đang thuộc lớp này!",
                                          "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa lớp '{txtTenLop.Text}' (Mã: {currentMaLop})?",
                                                       "Xác nhận xóa",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        string deleteQuery = "DELETE FROM LopHoc WHERE ID = @ID";
                        using (SqlCommand deleteCmd = new SqlCommand(deleteQuery, connection))
                        {
                            deleteCmd.Parameters.AddWithValue("@ID", currentID);
                            int rowsAffected = deleteCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Xóa lớp học thành công!", "Thông báo",
                                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RefreshAfterDataChange();
                                ClearInputs();
                            }
                            else
                            {
                                MessageBox.Show("Không thể xóa lớp này!", "Lỗi",
                                              MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi Hệ Thống",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ==================== 5. CHỨC NĂNG LÀM MỚI (ĐÃ FIX) ====================
        private void btn_lammoi_Click(object sender, EventArgs e)
        {
            ClearInputs();
            currentSearchKeyword = "";
            currentPage = 1;
            if (txtTimKiem != null) txtTimKiem.Clear();
            LoadDataToDataGridView();
            if (txtMaLop != null) txtMaLop.Focus();
        }

        // ==================== 6. CHỨC NĂNG TÌM KIẾM (ĐÃ FIX) ====================
        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            if (txtTimKiem != null)
            {
                currentSearchKeyword = txtTimKiem.Text.Trim();
                currentPage = 1;
                LoadDataToDataGridView();
            }
        }

        // ==================== 7. SỰ KIỆN CLICK TRÊN DATAGRIDVIEW ====================
        private void dgvLopHoc_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLopHoc.Rows[e.RowIndex];

                currentID = Convert.ToInt32(row.Cells["ID"].Value);
                currentMaLop = row.Cells["MaLop"].Value.ToString();

                if (txtID != null) txtID.Text = currentID.ToString();
                if (txtMaLop != null) txtMaLop.Text = currentMaLop;
                if (txtTenLop != null) txtTenLop.Text = row.Cells["TenLop"].Value.ToString();
                if (txtGhiChu != null) txtGhiChu.Text = row.Cells["GhiChu"].Value.ToString();

                if (txtID != null) txtID.Enabled = false;
                if (txtMaLop != null) txtMaLop.Enabled = false;
            }
        }

        // ==================== 8. CÁC NÚT PHÂN TRANG ====================
        private void btnFirst_Click_1(object sender, EventArgs e)
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

        // ==================== 9. THAY ĐỔI SỐ DÒNG MỖI TRANG ====================
        private void numPageSize_ValueChanged(object sender, EventArgs e)
        {
            if (numPageSize != null)
            {
                pageSize = (int)numPageSize.Value;
                currentPage = 1;
                LoadDataToDataGridView();
            }
        }

        // ==================== 10. CLICK VÀO LABEL ĐỂ NHẢY TRANG ====================
        private void lblPageInfo_Click(object sender, EventArgs e)
        {
            if (totalRecords == 0) return;

            Form inputForm = new Form();
            inputForm.Text = "Đi đến trang";
            inputForm.Size = new System.Drawing.Size(300, 120);
            inputForm.StartPosition = FormStartPosition.CenterParent;
            inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputForm.MaximizeBox = false;
            inputForm.MinimizeBox = false;

            Label lbl = new Label()
            {
                Text = $"Nhập số trang (1 - {totalPages}):",
                Location = new System.Drawing.Point(10, 15),
                Size = new System.Drawing.Size(180, 25)
            };

            NumericUpDown numPage = new NumericUpDown()
            {
                Location = new System.Drawing.Point(200, 13),
                Size = new System.Drawing.Size(60, 22),
                Minimum = 1,
                Maximum = totalPages,
                Value = currentPage
            };

            Button btnOK = new Button()
            {
                Text = "OK",
                Location = new System.Drawing.Point(200, 45),
                Size = new System.Drawing.Size(60, 30),
                DialogResult = DialogResult.OK
            };

            inputForm.Controls.Add(lbl);
            inputForm.Controls.Add(numPage);
            inputForm.Controls.Add(btnOK);

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                currentPage = (int)numPage.Value;
                LoadDataToDataGridView();
            }
        }

        // ==================== 11. CÁC SỰ KIỆN KHÁC ====================
        private void label1_Click(object sender, EventArgs e) { }
        private void label1_Click_1(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgvLopHoc_CellClick(sender, e);
        }

        // ==================== XEM DANH SÁCH SINH VIÊN (BẢN ĐƠN GIẢN) ====================
        private void btnXemSinhVien_Click(object sender, EventArgs e)
        {
            // Kiểm tra đã chọn lớp chưa
            if (currentID == -1 || string.IsNullOrEmpty(currentMaLop))
            {
                MessageBox.Show("Vui lòng chọn lớp để xem danh sách sinh viên!",
                               "Thông báo",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Tạo form mới
                Form frmContainer = new Form();
                frmContainer.Text = $"Danh sách sinh viên lớp {txtTenLop.Text}";
                frmContainer.Size = new System.Drawing.Size(950, 600);
                frmContainer.StartPosition = FormStartPosition.CenterParent;

                // Tạo User Control
                UCXemSinhVienTheoLop ucXemSV = new UCXemSinhVienTheoLop();
                ucXemSV.Dock = DockStyle.Fill;
                ucXemSV.SetLopInfo(currentMaLop, txtTenLop.Text);

                // Thêm vào form
                frmContainer.Controls.Add(ucXemSV);

                // Hiển thị
                frmContainer.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}