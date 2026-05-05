namespace TriQue.Forms
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            LoginPanel = new Panel();
            label2 = new Label();
            panel3 = new Panel();
            panel5 = new Panel();
            textBoxPassword = new TextBox();
            label1 = new Label();
            panel2 = new Panel();
            panel4 = new Panel();
            textBox1 = new TextBox();
            checkBoxShowPassword = new CheckBox();
            LoginBtn = new Guna.UI2.WinForms.Guna2Button();
            pictureBox1 = new PictureBox();
            guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(components);
            LoginPanel.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // LoginPanel
            // 
            LoginPanel.BackColor = Color.White;
            LoginPanel.Controls.Add(label2);
            LoginPanel.Controls.Add(panel3);
            LoginPanel.Controls.Add(label1);
            LoginPanel.Controls.Add(panel2);
            LoginPanel.Controls.Add(checkBoxShowPassword);
            LoginPanel.Controls.Add(LoginBtn);
            LoginPanel.Controls.Add(pictureBox1);
            LoginPanel.Location = new Point(364, -1);
            LoginPanel.Name = "LoginPanel";
            LoginPanel.Size = new Size(431, 652);
            LoginPanel.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(91, 91, 91);
            label2.Location = new Point(75, 363);
            label2.Name = "label2";
            label2.Size = new Size(91, 20);
            label2.TabIndex = 6;
            label2.Text = "Password";
            // 
            // panel3
            // 
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(textBoxPassword);
            panel3.Location = new Point(75, 384);
            panel3.Name = "panel3";
            panel3.Size = new Size(329, 26);
            panel3.TabIndex = 5;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(91, 91, 91);
            panel5.Dock = DockStyle.Bottom;
            panel5.Location = new Point(0, 24);
            panel5.Name = "panel5";
            panel5.Size = new Size(329, 2);
            panel5.TabIndex = 7;
            // 
            // textBoxPassword
            // 
            textBoxPassword.BorderStyle = BorderStyle.None;
            textBoxPassword.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxPassword.ForeColor = Color.FromArgb(91, 91, 91);
            textBoxPassword.Location = new Point(0, 3);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(326, 20);
            textBoxPassword.TabIndex = 5;
            textBoxPassword.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(91, 91, 91);
            label1.Location = new Point(75, 267);
            label1.Name = "label1";
            label1.Size = new Size(94, 20);
            label1.TabIndex = 4;
            label1.Text = "Username";
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(textBox1);
            panel2.Location = new Point(75, 288);
            panel2.Name = "panel2";
            panel2.Size = new Size(329, 26);
            panel2.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(91, 91, 91);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 24);
            panel4.Name = "panel4";
            panel4.Size = new Size(329, 2);
            panel4.TabIndex = 7;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.FromArgb(91, 91, 91);
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(323, 20);
            textBox1.TabIndex = 5;
            // 
            // checkBoxShowPassword
            // 
            checkBoxShowPassword.AutoSize = true;
            checkBoxShowPassword.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBoxShowPassword.ForeColor = Color.FromArgb(91, 91, 91);
            checkBoxShowPassword.Location = new Point(78, 418);
            checkBoxShowPassword.Name = "checkBoxShowPassword";
            checkBoxShowPassword.Size = new Size(138, 20);
            checkBoxShowPassword.TabIndex = 2;
            checkBoxShowPassword.Text = "Show Password";
            checkBoxShowPassword.UseVisualStyleBackColor = true;
            checkBoxShowPassword.CheckedChanged += checkBoxShowPassword_CheckedChanged;
            // 
            // LoginBtn
            // 
            LoginBtn.BorderRadius = 15;
            LoginBtn.CustomizableEdges = customizableEdges1;
            LoginBtn.DisabledState.BorderColor = Color.DarkGray;
            LoginBtn.DisabledState.CustomBorderColor = Color.DarkGray;
            LoginBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            LoginBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            LoginBtn.FillColor = Color.FromArgb(55, 91, 231);
            LoginBtn.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LoginBtn.ForeColor = Color.White;
            LoginBtn.HoverState.BorderColor = Color.FromArgb(0, 50, 125);
            LoginBtn.HoverState.CustomBorderColor = Color.FromArgb(0, 50, 125);
            LoginBtn.HoverState.FillColor = Color.FromArgb(0, 50, 125);
            LoginBtn.HoverState.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LoginBtn.HoverState.ForeColor = Color.White;
            LoginBtn.Location = new Point(75, 486);
            LoginBtn.Name = "LoginBtn";
            LoginBtn.PressedColor = Color.FromArgb(0, 50, 125);
            LoginBtn.ShadowDecoration.CustomizableEdges = customizableEdges2;
            LoginBtn.Size = new Size(329, 56);
            LoginBtn.TabIndex = 1;
            LoginBtn.Text = "Login";
            LoginBtn.Click += LoginBtn_Click_1;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(132, 133);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(227, 104);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(11F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1182, 653);
            Controls.Add(LoginPanel);
            Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(91, 91, 91);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TriQUE";
            LoginPanel.ResumeLayout(false);
            LoginPanel.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel LoginPanel;
        private PictureBox pictureBox1;
        private CheckBox checkBoxShowPassword;
        private Guna.UI2.WinForms.Guna2Button LoginBtn;
        private Panel panel2;
        private Label label1;
        private TextBox textBox1;
        private Panel panel4;
        private Label label2;
        private Panel panel3;
        private Panel panel5;
        private TextBox textBoxPassword;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
    }
}
