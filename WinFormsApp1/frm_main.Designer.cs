namespace WinFormsApp1
{
    partial class frm_main
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            quanLiSinhVienToolStripMenuItem = new ToolStripMenuItem();
            quanLiLopHocToolStripMenuItem = new ToolStripMenuItem();
            dangXuatToolStripMenuItem = new ToolStripMenuItem();
            pnl_content = new Panel();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { quanLiSinhVienToolStripMenuItem, quanLiLopHocToolStripMenuItem, dangXuatToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // quanLiSinhVienToolStripMenuItem
            // 
            quanLiSinhVienToolStripMenuItem.Name = "quanLiSinhVienToolStripMenuItem";
            quanLiSinhVienToolStripMenuItem.Size = new Size(107, 20);
            quanLiSinhVienToolStripMenuItem.Text = "Quan li sinh vien";
            quanLiSinhVienToolStripMenuItem.Click += quanLiSinhVienToolStripMenuItem_Click;
            // 
            // quanLiLopHocToolStripMenuItem
            // 
            quanLiLopHocToolStripMenuItem.Name = "quanLiLopHocToolStripMenuItem";
            quanLiLopHocToolStripMenuItem.Size = new Size(100, 20);
            quanLiLopHocToolStripMenuItem.Text = "Quan li lop hoc";
            quanLiLopHocToolStripMenuItem.Click += quanLiLopHocToolStripMenuItem_Click;
            // 
            // dangXuatToolStripMenuItem
            // 
            dangXuatToolStripMenuItem.Name = "dangXuatToolStripMenuItem";
            dangXuatToolStripMenuItem.Size = new Size(72, 20);
            dangXuatToolStripMenuItem.Text = "Dang xuat";
            dangXuatToolStripMenuItem.Click += dangXuatToolStripMenuItem_Click;
            // 
            // pnl_content
            // 
            pnl_content.Dock = DockStyle.Fill;
            pnl_content.Location = new Point(0, 24);
            pnl_content.Name = "pnl_content";
            pnl_content.Size = new Size(800, 426);
            pnl_content.TabIndex = 1;
            pnl_content.Paint += panel1_Paint;
            // 
            // frm_main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnl_content);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "frm_main";
            Text = "frm_main";
            Load += frm_main_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem quanLiSinhVienToolStripMenuItem;
        private ToolStripMenuItem quanLiLopHocToolStripMenuItem;
        private ToolStripMenuItem dangXuatToolStripMenuItem;
        private Panel pnl_content;
    }
}