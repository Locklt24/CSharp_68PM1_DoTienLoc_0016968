namespace WinFormsApp1
{
    partial class UCQLLH
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            txtID = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtGhiChu = new TextBox();
            label1 = new Label();
            txtTenLop = new TextBox();
            txtMaLop = new TextBox();
            dgvLopHoc = new DataGridView();
            btn_timkiem = new Button();
            txtTimKiem = new TextBox();
            label6 = new Label();
            btn_lammoi = new Button();
            btn_xoa = new Button();
            btn_sua = new Button();
            btn_them = new Button();
            numPageSize = new NumericUpDown();
            btnLast = new Button();
            btnNext = new Button();
            btnPrevious = new Button();
            btnFirst = new Button();
            lblPageInfo = new Label();
            btnXemSinhVien = new Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPageSize).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtID);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtGhiChu);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txtTenLop);
            groupBox1.Controls.Add(txtMaLop);
            groupBox1.Location = new Point(3, 106);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(407, 360);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin lớp học";
            // 
            // txtID
            // 
            txtID.Location = new Point(39, 283);
            txtID.Name = "txtID";
            txtID.Size = new Size(263, 23);
            txtID.TabIndex = 2;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(39, 265);
            label4.Name = "label4";
            label4.Size = new Size(116, 15);
            label4.TabIndex = 1;
            label4.Text = "\tHiển thị ID (tự động)";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(39, 188);
            label3.Name = "label3";
            label3.Size = new Size(137, 15);
            label3.TabIndex = 1;
            label3.Text = "Nhập ghi chú (tùy chọn)";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(39, 117);
            label2.Name = "label2";
            label2.Size = new Size(76, 15);
            label2.TabIndex = 1;
            label2.Text = "Nhập tên lớp";
            // 
            // txtGhiChu
            // 
            txtGhiChu.Location = new Point(39, 206);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.Size = new Size(263, 23);
            txtGhiChu.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 39);
            label1.Name = "label1";
            label1.Size = new Size(76, 15);
            label1.TabIndex = 1;
            label1.Text = "Nhập mã lớp";
            label1.Click += label1_Click_1;
            // 
            // txtTenLop
            // 
            txtTenLop.Location = new Point(39, 135);
            txtTenLop.Name = "txtTenLop";
            txtTenLop.Size = new Size(263, 23);
            txtTenLop.TabIndex = 0;
            // 
            // txtMaLop
            // 
            txtMaLop.Location = new Point(39, 66);
            txtMaLop.Name = "txtMaLop";
            txtMaLop.Size = new Size(263, 23);
            txtMaLop.TabIndex = 0;
            // 
            // dgvLopHoc
            // 
            dgvLopHoc.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLopHoc.Location = new Point(532, 114);
            dgvLopHoc.Name = "dgvLopHoc";
            dgvLopHoc.Size = new Size(728, 457);
            dgvLopHoc.TabIndex = 3;
            dgvLopHoc.CellContentClick += dataGridView1_CellContentClick;
            // 
            // btn_timkiem
            // 
            btn_timkiem.Location = new Point(1172, 50);
            btn_timkiem.Name = "btn_timkiem";
            btn_timkiem.Size = new Size(72, 36);
            btn_timkiem.TabIndex = 8;
            btn_timkiem.Text = "Tìm";
            btn_timkiem.UseVisualStyleBackColor = true;
            // 
            // txtTimKiem
            // 
            txtTimKiem.Location = new Point(734, 58);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.Size = new Size(426, 23);
            txtTimKiem.TabIndex = 7;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(563, 61);
            label6.Name = "label6";
            label6.Size = new Size(94, 15);
            label6.TabIndex = 6;
            label6.Text = "Tìm kiếm ( Lớp):";
            label6.Click += label6_Click;
            // 
            // btn_lammoi
            // 
            btn_lammoi.Location = new Point(208, 558);
            btn_lammoi.Name = "btn_lammoi";
            btn_lammoi.Size = new Size(122, 37);
            btn_lammoi.TabIndex = 9;
            btn_lammoi.Text = "làm mới";
            btn_lammoi.UseVisualStyleBackColor = true;
            // 
            // btn_xoa
            // 
            btn_xoa.Location = new Point(73, 558);
            btn_xoa.Name = "btn_xoa";
            btn_xoa.Size = new Size(102, 37);
            btn_xoa.TabIndex = 10;
            btn_xoa.Text = "Xóa";
            btn_xoa.UseVisualStyleBackColor = true;
            // 
            // btn_sua
            // 
            btn_sua.Location = new Point(208, 509);
            btn_sua.Name = "btn_sua";
            btn_sua.Size = new Size(122, 33);
            btn_sua.TabIndex = 11;
            btn_sua.Text = "Sửa";
            btn_sua.UseVisualStyleBackColor = true;
            btn_sua.Click += btn_sua_Click_1;
            // 
            // btn_them
            // 
            btn_them.Location = new Point(73, 509);
            btn_them.Name = "btn_them";
            btn_them.Size = new Size(102, 33);
            btn_them.TabIndex = 12;
            btn_them.Text = "Thêm";
            btn_them.UseVisualStyleBackColor = true;
            btn_them.Click += btn_them_Click;
            // 
            // numPageSize
            // 
            numPageSize.Location = new Point(910, 631);
            numPageSize.Name = "numPageSize";
            numPageSize.Size = new Size(120, 23);
            numPageSize.TabIndex = 17;
            // 
            // btnLast
            // 
            btnLast.Location = new Point(1130, 629);
            btnLast.Name = "btnLast";
            btnLast.Size = new Size(75, 23);
            btnLast.TabIndex = 13;
            btnLast.Text = ">>";
            btnLast.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(1049, 629);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(75, 23);
            btnNext.TabIndex = 14;
            btnNext.Text = ">";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new Point(822, 629);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(75, 23);
            btnPrevious.TabIndex = 15;
            btnPrevious.Text = "<";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnFirst
            // 
            btnFirst.Location = new Point(718, 629);
            btnFirst.Name = "btnFirst";
            btnFirst.Size = new Size(75, 23);
            btnFirst.TabIndex = 16;
            btnFirst.Text = "<<";
            btnFirst.UseVisualStyleBackColor = true;
            btnFirst.Click += btnFirst_Click_1;
            // 
            // lblPageInfo
            // 
            lblPageInfo.AutoSize = true;
            lblPageInfo.Location = new Point(951, 592);
            lblPageInfo.Name = "lblPageInfo";
            lblPageInfo.Size = new Size(38, 15);
            lblPageInfo.TabIndex = 18;
            lblPageInfo.Text = "label7";
            // 
            // btnXemSinhVien
            // 
            btnXemSinhVien.Location = new Point(73, 615);
            btnXemSinhVien.Name = "btnXemSinhVien";
            btnXemSinhVien.Size = new Size(257, 37);
            btnXemSinhVien.TabIndex = 9;
            btnXemSinhVien.Text = "Xem danh sách sinh viên";
            btnXemSinhVien.UseVisualStyleBackColor = true;
            btnXemSinhVien.Click += btnXemSinhVien_Click;
            // 
            // UCQLLH
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblPageInfo);
            Controls.Add(numPageSize);
            Controls.Add(btnLast);
            Controls.Add(btnNext);
            Controls.Add(btnPrevious);
            Controls.Add(btnFirst);
            Controls.Add(btnXemSinhVien);
            Controls.Add(btn_lammoi);
            Controls.Add(btn_xoa);
            Controls.Add(btn_sua);
            Controls.Add(btn_them);
            Controls.Add(btn_timkiem);
            Controls.Add(txtTimKiem);
            Controls.Add(label6);
            Controls.Add(dgvLopHoc);
            Controls.Add(groupBox1);
            Name = "UCQLLH";
            Size = new Size(1243, 679);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvLopHoc).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPageSize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1;
        private ComboBox cbo_lop;
        private ComboBox cbo_gioitinh;
        private DateTimePicker dtp_ngaysinh;
        private Label label5;
        private Label label2;
        private Label label1;
        private TextBox txtTenLop;
        private TextBox txtMaLop;
        private Label label4;
        private Label label3;
        private TextBox txtGhiChu;
        private DataGridView dgvLopHoc;
        private Button btn_timkiem;
        private TextBox txtTimKiem;
        private Label label6;
        private Button btn_lammoi;
        private Button btn_xoa;
        private Button btn_sua;
        private Button btn_them;
        private NumericUpDown numPageSize;
        private Button btnLast;
        private Button btnNext;
        private Button btnPrevious;
        private Button btnFirst;
        private TextBox txtID;
        private Label lblPageInfo;
        private Button btnXemSinhVien;
    }
}
