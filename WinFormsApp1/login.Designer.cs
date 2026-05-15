namespace WinFormsApp1
{
    partial class login
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
            label1 = new Label();
            txt_username = new TextBox();
            txt_password = new TextBox();
            label2 = new Label();
            label3 = new Label();
            btnlogin = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.HotTrack;
            label1.Location = new Point(319, 70);
            label1.Name = "label1";
            label1.Size = new Size(144, 15);
            label1.TabIndex = 0;
            label1.Text = "he thong quan li sinh vien";
            label1.Click += label1_Click;
            // 
            // txt_username
            // 
            txt_username.Location = new Point(389, 158);
            txt_username.Name = "txt_username";
            txt_username.Size = new Size(241, 23);
            txt_username.TabIndex = 1;
            txt_username.TextChanged += textBox1_TextChanged;
            // 
            // txt_password
            // 
            txt_password.Location = new Point(389, 201);
            txt_password.Name = "txt_password";
            txt_password.Size = new Size(241, 23);
            txt_password.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(296, 166);
            label2.Name = "label2";
            label2.Size = new Size(90, 15);
            label2.TabIndex = 3;
            label2.Text = "ten dang nhap :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(296, 209);
            label3.Name = "label3";
            label3.Size = new Size(63, 15);
            label3.TabIndex = 4;
            label3.Text = "mat khau :";
            // 
            // btnlogin
            // 
            btnlogin.Location = new Point(407, 242);
            btnlogin.Name = "btnlogin";
            btnlogin.Size = new Size(105, 44);
            btnlogin.TabIndex = 5;
            btnlogin.Text = "dang nhap";
            btnlogin.UseVisualStyleBackColor = true;
            btnlogin.Click += btnlogin_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnlogin);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txt_password);
            Controls.Add(txt_username);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txt_username;
        private TextBox txt_password;
        private Label label2;
        private Label label3;
        private Button btnlogin;
    }
}