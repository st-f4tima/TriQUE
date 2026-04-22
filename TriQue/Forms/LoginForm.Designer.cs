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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            panel1 = new Panel();
            label2 = new Label();
            panel3 = new Panel();
            panel5 = new Panel();
            textBoxPassword = new TextBox();
            label1 = new Label();
            panel2 = new Panel();
            panel4 = new Panel();
            textBox1 = new TextBox();
            checkBoxShowPassword = new CheckBox();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            pictureBox1 = new PictureBox();
            guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(components);
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label2);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(checkBoxShowPassword);
            panel1.Controls.Add(guna2Button1);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(263, -1);
            panel1.Name = "panel1";
            panel1.Size = new Size(431, 652);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.FromArgb(91, 91, 91);
            label2.Location = new Point(52, 363);
            label2.Name = "label2";
            label2.Size = new Size(94, 20);
            label2.TabIndex = 6;
            label2.Text = "Password";
            label2.Click += label2_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(textBoxPassword);
            panel3.Location = new Point(52, 384);
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
            textBoxPassword.Font = new Font("Roboto", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBoxPassword.ForeColor = Color.FromArgb(91, 91, 91);
            textBoxPassword.Location = new Point(3, 3);
            textBoxPassword.Name = "textBoxPassword";
            textBoxPassword.Size = new Size(281, 21);
            textBoxPassword.TabIndex = 5;
            textBoxPassword.UseSystemPasswordChar = true;
            textBoxPassword.TextChanged += textBoxPassword_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.FromArgb(91, 91, 91);
            label1.Location = new Point(52, 267);
            label1.Name = "label1";
            label1.Size = new Size(94, 20);
            label1.TabIndex = 4;
            label1.Text = "Username";
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(textBox1);
            panel2.Location = new Point(52, 288);
            panel2.Name = "panel2";
            panel2.Size = new Size(329, 26);
            panel2.TabIndex = 3;
            panel2.Paint += panel2_Paint;
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
            textBox1.Font = new Font("Roboto", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.ForeColor = Color.FromArgb(91, 91, 91);
            textBox1.Location = new Point(3, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(281, 21);
            textBox1.TabIndex = 5;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // checkBoxShowPassword
            // 
            checkBoxShowPassword.AutoSize = true;
            checkBoxShowPassword.Font = new Font("Roboto", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBoxShowPassword.ForeColor = Color.FromArgb(91, 91, 91);
            checkBoxShowPassword.Location = new Point(55, 418);
            checkBoxShowPassword.Name = "checkBoxShowPassword";
            checkBoxShowPassword.Size = new Size(134, 19);
            checkBoxShowPassword.TabIndex = 2;
            checkBoxShowPassword.Text = "Show Password";
            checkBoxShowPassword.UseVisualStyleBackColor = true;
            checkBoxShowPassword.CheckedChanged += checkBoxShowPassword_CheckedChanged;
            // 
            // guna2Button1
            // 
            guna2Button1.BorderRadius = 15;
            guna2Button1.CustomizableEdges = customizableEdges3;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.FromArgb(55, 91, 231);
            guna2Button1.Font = new Font("Roboto", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.HoverState.BorderColor = Color.FromArgb(0, 50, 125);
            guna2Button1.HoverState.CustomBorderColor = Color.FromArgb(0, 50, 125);
            guna2Button1.HoverState.FillColor = Color.FromArgb(0, 50, 125);
            guna2Button1.HoverState.Font = new Font("Roboto", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            guna2Button1.HoverState.ForeColor = Color.White;
            guna2Button1.Location = new Point(57, 486);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.PressedColor = Color.FromArgb(0, 50, 125);
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button1.Size = new Size(314, 56);
            guna2Button1.TabIndex = 1;
            guna2Button1.Text = "Login";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(90, 88);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(262, 134);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(950, 650);
            Controls.Add(panel1);
            Font = new Font("Roboto", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(91, 91, 91);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private CheckBox checkBoxShowPassword;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
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
