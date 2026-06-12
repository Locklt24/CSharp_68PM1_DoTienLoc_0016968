using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class UCXemSinhVienTheoLop : UserControl
    {
        private string maLop = "";
        private string tenLop = "";

        // Biến phân trang cho User Control này
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;
        private string currentSearchKeyword = "";

        public UCXemSinhVienTheoLop()
        {
            InitializeComponent();
        }

        // Hàm nhận dữ liệu từ form chính
        public void SetLopInfo(string maLop, string tenLop)
        {
            this.maLop = maLop;
            this.tenLop = tenLop;
            lblTitle.Text = $"DANH SÁCH SINH VIÊN LỚP {tenLop} (Mã: {maLop})";
            LoadDataToDataGridView();
        }

        // Tải dữ liệu lên DataGridView (có phân trang)
        private void LoadDataToDataGridView()
        {
            if (string.IsNullOrEmpty(maLop)) return;

            string connectionString = Database.connectionString;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Đếm tổng số bản ghi
                    string countQuery = "";

                    if (!string.IsNullOrEmpty(currentSearchKeyword))
                    {
                        countQuery = @"SELECT COUNT(*) 
                                       FROM SinhVien 
                                       WHERE MaLop = @MaLop 
                                         AND (MaSV LIKE @Keyword OR HoTen LIKE @Keyword)";
                    }
                    else
                    {
                        countQuery = "SELECT COUNT(*) FROM SinhVien WHERE MaLop = @MaLop";
                    }

                    using (SqlCommand countCmd = new SqlCommand(countQuery, connection))
                    {
                        countCmd.Parameters.AddWithValue("@MaLop", maLop);
                        if (!string.IsNullOrEmpty(currentSearchKeyword))
                        {
                            countCmd.Parameters.AddWithValue("@Keyword", "%" + currentSearchKeyword + "%");
                        }
                        totalRecords = (int)countCmd.ExecuteScalar();
                    }

                    // Tính tổng số trang
                    if (totalRecords > 0)
                    {
                        totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                    }
                    else
                    {
                        totalPages = 1;
                    }

                    // Điều chỉnh trang hiện tại
                    if (currentPage > totalPages) currentPage = totalPages;
                    if (currentPage < 1) currentPage = 1;

                    // Tính vị trí
                    int startRow = (currentPage - 1) * pageSize + 1;
                    int endRow = Math.Min(currentPage * pageSize, totalRecords);

                    if (totalRecords == 0)
                    {
                        dgvSinhVien.DataSource = null;
                        UpdatePaginationUI();
                        return;
                    }

                    // Lấy dữ liệu
                    string dataQuery = "";

                    if (!string.IsNullOrEmpty(currentSearchKeyword))
                    {
                        dataQuery = @"SELECT * FROM (
                                        SELECT ROW_NUMBER() OVER (ORDER BY MaSV) AS RowNum,
                                               MaSV, HoTen, NgaySinh, GioiTinh
                                        FROM SinhVien 
                                        WHERE MaLop = @MaLop 
                                          AND (MaSV LIKE @Keyword OR HoTen LIKE @Keyword)
                                    ) AS PagedData
                                    WHERE RowNum BETWEEN @StartRow AND @EndRow";
                    }
                    else
                    {
                        dataQuery = @"SELECT * FROM (
                                        SELECT ROW_NUMBER() OVER (ORDER BY MaSV) AS RowNum,
                                               MaSV, HoTen, NgaySinh, GioiTinh
                                        FROM SinhVien 
                                        WHERE MaLop = @MaLop
                                    ) AS PagedData
                                    WHERE RowNum BETWEEN @StartRow AND @EndRow";
                    }

                    using (SqlCommand dataCmd = new SqlCommand(dataQuery, connection))
                    {
                        dataCmd.Parameters.AddWithValue("@MaLop", maLop);
                        if (!string.IsNullOrEmpty(currentSearchKeyword))
                        {
                            dataCmd.Parameters.AddWithValue("@Keyword", "%" + currentSearchKeyword + "%");
                        }
                        dataCmd.Parameters.AddWithValue("@StartRow", startRow);
                        dataCmd.Parameters.AddWithValue("@EndRow", endRow);

                        SqlDataAdapter adapter = new SqlDataAdapter(dataCmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dgvSinhVien.DataSource = dataTable;

                        // Đặt tiêu đề cột
                        if (dgvSinhVien.Columns.Count > 0)
                        {
                            if (dgvSinhVien.Columns.Contains("MaSV"))
                                dgvSinhVien.Columns["MaSV"].HeaderText = "Mã sinh viên";
                            if (dgvSinhVien.Columns.Contains("HoTen"))
                                dgvSinhVien.Columns["HoTen"].HeaderText = "Họ và tên";
                            if (dgvSinhVien.Columns.Contains("NgaySinh"))
                                dgvSinhVien.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                            if (dgvSinhVien.Columns.Contains("GioiTinh"))
                                dgvSinhVien.Columns["GioiTinh"].HeaderText = "Giới tính";
                            if (dgvSinhVien.Columns.Contains("RowNum"))
                                dgvSinhVien.Columns["RowNum"].Visible = false;
                        }
                    }

                    UpdatePaginationUI();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdatePaginationUI()
        {
            lblPageInfo.Text = $"Trang {currentPage} / {totalPages} | {totalRecords} sinh viên";

            btnFirst.Enabled = (currentPage > 1 && totalRecords > 0);
            btnPrevious.Enabled = (currentPage > 1 && totalRecords > 0);
            btnNext.Enabled = (currentPage < totalPages && totalRecords > 0);
            btnLast.Enabled = (currentPage < totalPages && totalRecords > 0);
        }

        // ==================== SỰ KIỆN TÌM KIẾM ====================
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            currentSearchKeyword = txtTimKiem.Text.Trim();
            currentPage = 1;
            LoadDataToDataGridView();
        }

        // ==================== SỰ KIỆN LÀM MỚI ====================
        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            currentSearchKeyword = "";
            currentPage = 1;
            txtTimKiem.Clear();
            LoadDataToDataGridView();
        }

        // ==================== SỰ KIỆN ĐÓNG ====================
        private void btnDong_Click(object sender, EventArgs e)
        {
            // Tìm form cha và đóng form đó
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Close();
            }
        }

        // ==================== CÁC NÚT PHÂN TRANG ====================
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

        // ==================== SỰ KIỆN NHẤN ENTER TRONG Ô TÌM KIẾM ====================
        private void txtTimKiem_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnTimKiem_Click(sender, e);
            }
        }
    }
}