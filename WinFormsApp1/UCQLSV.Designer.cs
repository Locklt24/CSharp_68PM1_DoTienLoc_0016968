namespace WinFormsApp1
{
    partial class UCQLSV : UserControl
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
            dgv_sinhvien = new DataGridView();
            btn_them = new Button();
            btn_sua = new Button();
            btn_xoa = new Button();
            btn_lammoi = new Button();
            groupBox1 = new GroupBox();
            cbo_lop = new ComboBox();
            cbo_gioitinh = new ComboBox();
            dtp_ngaysinh = new DateTimePicker();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            txt_masv = new TextBox();
            txt_hovaten = new TextBox();
            label6 = new Label();
            txt_timkiem = new TextBox();
            btn_timkiem = new Button();
            ((System.ComponentModel.ISupportInitialize)dgv_sinhvien).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // dgv_sinhvien
            // 
            dgv_sinhvien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv_sinhvien.Location = new Point(485, 70);
            dgv_sinhvien.Name = "dgv_sinhvien";
            dgv_sinhvien.Size = new Size(711, 465);
            dgv_sinhvien.TabIndex = 0;
            dgv_sinhvien.CellContentClick += dgv_sinhvien_CellContentClick;
            // 
            // btn_them
            // 
            btn_them.Location = new Point(72, 552);
            btn_them.Name = "btn_them";
            btn_them.Size = new Size(102, 33);
            btn_them.TabIndex = 1;
            btn_them.Text = "Thêm";
            btn_them.UseVisualStyleBackColor = true;
            btn_them.Click += btn_them_Click;
            // 
            // btn_sua
            // 
            btn_sua.Location = new Point(207, 552);
            btn_sua.Name = "btn_sua";
            btn_sua.Size = new Size(122, 33);
            btn_sua.TabIndex = 1;
            btn_sua.Text = "Sửa";
            btn_sua.UseVisualStyleBackColor = true;
            btn_sua.Click += btn_sua_Click;
            // 
            // btn_xoa
            // 
            btn_xoa.Location = new Point(72, 601);
            btn_xoa.Name = "btn_xoa";
            btn_xoa.Size = new Size(102, 37);
            btn_xoa.TabIndex = 1;
            btn_xoa.Text = "Xóa";
            btn_xoa.UseVisualStyleBackColor = true;
            btn_xoa.Click += btn_xoa_Click;
            // 
            // btn_lammoi
            // 
            btn_lammoi.Location = new Point(207, 601);
            btn_lammoi.Name = "btn_lammoi";
            btn_lammoi.Size = new Size(122, 37);
            btn_lammoi.TabIndex = 1;
            btn_lammoi.Text = "làm mới";
            btn_lammoi.UseVisualStyleBackColor = true;
            btn_lammoi.Click += btn_lammoi_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbo_lop);
            groupBox1.Controls.Add(cbo_gioitinh);
            groupBox1.Controls.Add(dtp_ngaysinh);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(txt_masv);
            groupBox1.Controls.Add(txt_hovaten);
            groupBox1.Location = new Point(27, 70);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(407, 465);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông tin sinh viên";
            // 
            // cbo_lop
            // 
            cbo_lop.FormattingEnabled = true;
            cbo_lop.Location = new Point(39, 376);
            cbo_lop.Name = "cbo_lop";
            cbo_lop.Size = new Size(263, 23);
            cbo_lop.TabIndex = 4;
            cbo_lop.SelectedIndexChanged += cbo_lop_SelectedIndexChanged;
            // 
            // cbo_gioitinh
            // 
            cbo_gioitinh.FormattingEnabled = true;
            cbo_gioitinh.Location = new Point(39, 293);
            cbo_gioitinh.Name = "cbo_gioitinh";
            cbo_gioitinh.Size = new Size(263, 23);
            cbo_gioitinh.TabIndex = 3;
            // 
            // dtp_ngaysinh
            // 
            dtp_ngaysinh.Location = new Point(39, 212);
            dtp_ngaysinh.Name = "dtp_ngaysinh";
            dtp_ngaysinh.Size = new Size(263, 23);
            dtp_ngaysinh.TabIndex = 2;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(39, 358);
            label5.Name = "label5";
            label5.Size = new Size(33, 15);
            label5.TabIndex = 1;
            label5.Text = "Lớp :";
            label5.Click += label5_Click_1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(39, 275);
            label4.Name = "label4";
            label4.Size = new Size(58, 15);
            label4.TabIndex = 1;
            label4.Text = "Giới tinh :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(39, 194);
            label3.Name = "label3";
            label3.Size = new Size(66, 15);
            label3.TabIndex = 1;
            label3.Text = "Ngày sinh :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(39, 117);
            label2.Name = "label2";
            label2.Size = new Size(40, 15);
            label2.TabIndex = 1;
            label2.Text = "Mssv :";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(39, 39);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 1;
            label1.Text = "Họ và tên :";
            // 
            // txt_masv
            // 
            txt_masv.Location = new Point(39, 135);
            txt_masv.Name = "txt_masv";
            txt_masv.Size = new Size(263, 23);
            txt_masv.TabIndex = 0;
            // 
            // txt_hovaten
            // 
            txt_hovaten.Location = new Point(39, 66);
            txt_hovaten.Name = "txt_hovaten";
            txt_hovaten.Size = new Size(263, 23);
            txt_hovaten.TabIndex = 0;
            txt_hovaten.TextChanged += textBox1_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(515, 23);
            label6.Name = "label6";
            label6.Size = new Size(165, 15);
            label6.TabIndex = 3;
            label6.Text = "Tìm kiếm (Tên / Mã SV / Lớp):";
            // 
            // txt_timkiem
            // 
            txt_timkiem.Location = new Point(686, 20);
            txt_timkiem.Name = "txt_timkiem";
            txt_timkiem.Size = new Size(426, 23);
            txt_timkiem.TabIndex = 4;
            // 
            // btn_timkiem
            // 
            btn_timkiem.Location = new Point(1124, 12);
            btn_timkiem.Name = "btn_timkiem";
            btn_timkiem.Size = new Size(72, 36);
            btn_timkiem.TabIndex = 5;
            btn_timkiem.Text = "Tìm";
            btn_timkiem.UseVisualStyleBackColor = true;
            btn_timkiem.Click += btn_timkiem_Click;
            // 
            // UCQLSV
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btn_timkiem);
            Controls.Add(txt_timkiem);
            Controls.Add(label6);
            Controls.Add(groupBox1);
            Controls.Add(btn_lammoi);
            Controls.Add(btn_xoa);
            Controls.Add(btn_sua);
            Controls.Add(btn_them);
            Controls.Add(dgv_sinhvien);
            Name = "UCQLSV";
            Size = new Size(1243, 679);
            ((System.ComponentModel.ISupportInitialize)dgv_sinhvien).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgv_sinhvien;
        private Button btn_them;
        private Button btn_sua;
        private Button btn_xoa;
        private Button btn_lammoi;
        private GroupBox groupBox1;
        private TextBox txt_masv;
        private TextBox txt_hovaten;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private DateTimePicker dtp_ngaysinh;
        private ComboBox cbo_gioitinh;
        private ComboBox cbo_lop;
        private Label label6;
        private TextBox txt_timkiem;
        private Button btn_timkiem;
    }
}
