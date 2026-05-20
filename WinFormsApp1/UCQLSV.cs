using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class UCQLSV : UserControl
    {
        private List<SinhVien> danhSachSV = new List<SinhVien>();
        public UCQLSV()
        {
            InitializeComponent();
            TaoDuLieuMau();
        }
        private void TaoDuLieuMau()
        {
            // Thêm vài sinh viên mẫu để test giao diện
            danhSachSV.Add(new SinhVien { MaSV = "SV001", HoTen = "Đỗ Tiến Lộc", GioiTinh = "Nam", NgaySinh = new DateTime(2005, 1, 1), Lop = "67TH1" });
            danhSachSV.Add(new SinhVien { MaSV = "SV002", HoTen = "Nguyen Van A", GioiTinh = "Nam", NgaySinh = new DateTime(2005, 5, 15), Lop = "67TH2" });

            HienThiLenBang();
        }

        // Hàm làm mới và đẩy dữ liệu từ List vào DataGridView
        private void HienThiLenBang()
        {
            dgv_sinhvien.DataSource = null; // Khởi động lại nguồn dữ liệu
            dgv_sinhvien.DataSource = danhSachSV; // Đổ danh sách vào bảng
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void btn_timkiem_Click(object sender, EventArgs e)
        {
            string tuKhoa = txt_timkiem.Text;
            MessageBox.Show("Đang tìm kiếm với từ khóa: " + tuKhoa, "Thông báo");
        }

        private void btn_them_Click(object sender, EventArgs e)
        {
            SinhVien sv = new SinhVien();
            sv.MaSV = txt_masv.Text;
            sv.HoTen = txt_hovaten.Text;
            sv.GioiTinh = cbo_gioitinh.Text;
            sv.NgaySinh = dtp_ngaysinh.Value;
            sv.Lop = cbo_lop.Text;
            if (string.IsNullOrEmpty(sv.MaSV) || string.IsNullOrEmpty(sv.HoTen) || string.IsNullOrEmpty(sv.GioiTinh) || string.IsNullOrEmpty(sv.Lop))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                // Thêm vào danh sách tổng
                danhSachSV.Add(sv);

                // Cập nhật lại bảng hiển thị
                HienThiLenBang();
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo");
            }
        }

        private void btn_lammoi_Click(object sender, EventArgs e)
        {
            txt_masv.Clear();
            txt_hovaten.Clear();
            dtp_ngaysinh.Value = DateTime.Now;
            cbo_gioitinh.SelectedIndex = -1;
            cbo_lop.SelectedIndex = -1;
            txt_masv.Focus();
        }

        private void btn_xoa_Click(object sender, EventArgs e)
        {
            if (dgv_sinhvien.CurrentRow != null)
            {
                // Lấy mã sinh viên của dòng đang được chọn trên bảng
                string maCanXoa = dgv_sinhvien.CurrentRow.Cells["MaSV"].Value.ToString();

                // Tìm sinh viên đó trong List và xóa đi
                danhSachSV.RemoveAll(sv => sv.MaSV == maCanXoa);

                // Cập nhật lại bảng
                HienThiLenBang();
                MessageBox.Show("Đã xóa sinh viên thành công!", "Thông báo");
            }
        }

        private void btn_sua_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đang thực hiện sửa thông tin sinh viên!", "Thông báo");
        }
    }
}
