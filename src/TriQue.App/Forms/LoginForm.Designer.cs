using TriQue.Helpers;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblPass = new Label();
            lblUsername = new Label();
            LoginBtn = new Guna.UI2.WinForms.Guna2Button();
            pictureBox1 = new PictureBox();
            guna2AnimateWindow1 = new Guna.UI2.WinForms.Guna2AnimateWindow(components);
            lblGreeting = new Label();
            lblLoginDesc = new Label();
            guna2ContextMenuStrip1 = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
            tbUsername = new Guna.UI2.WinForms.Guna2TextBox();
            checkBox1 = new CheckBox();
            tbPassword1 = new Guna.UI2.WinForms.Guna2TextBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // lblPass
            // 
            lblPass.AutoSize = true;
            lblPass.ForeColor = Color.FromArgb(91, 91, 91);
            lblPass.Location = new Point(406, 344);
            lblPass.Name = "lblPass";
            lblPass.Size = new Size(91, 20);
            lblPass.TabIndex = 6;
            lblPass.Text = "Password";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.ForeColor = Color.FromArgb(91, 91, 91);
            lblUsername.Location = new Point(403, 248);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(94, 20);
            lblUsername.TabIndex = 4;
            lblUsername.Text = "Username";
            // 
            // LoginBtn
            // 
            LoginBtn.BorderRadius = 10;
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
            LoginBtn.Location = new Point(406, 464);
            LoginBtn.Name = "LoginBtn";
            LoginBtn.PressedColor = Color.FromArgb(0, 50, 125);
            LoginBtn.ShadowDecoration.CustomizableEdges = customizableEdges2;
            LoginBtn.Size = new Size(385, 56);
            LoginBtn.TabIndex = 1;
            LoginBtn.Text = "Login";
            LoginBtn.Click += LoginBtn_Click_1;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(527, 80);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(124, 56);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // lblGreeting
            // 
            lblGreeting.AutoSize = true;
            lblGreeting.Font = new Font("Microsoft Sans Serif", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGreeting.ForeColor = Color.FromArgb(64, 64, 64);
            lblGreeting.Location = new Point(450, 139);
            lblGreeting.Name = "lblGreeting";
            lblGreeting.Size = new Size(301, 46);
            lblGreeting.TabIndex = 1;
            lblGreeting.Text = "Welcome Back";
            // 
            // lblLoginDesc
            // 
            lblLoginDesc.AutoSize = true;
            lblLoginDesc.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblLoginDesc.ForeColor = Color.Gray;
            lblLoginDesc.Location = new Point(479, 198);
            lblLoginDesc.Name = "lblLoginDesc";
            lblLoginDesc.Size = new Size(238, 20);
            lblLoginDesc.TabIndex = 2;
            lblLoginDesc.Text = "Login to you account below";
            // 
            // guna2ContextMenuStrip1
            // 
            guna2ContextMenuStrip1.ImageScalingSize = new Size(20, 20);
            guna2ContextMenuStrip1.Name = "guna2ContextMenuStrip1";
            guna2ContextMenuStrip1.RenderStyle.ArrowColor = Color.FromArgb(151, 143, 255);
            guna2ContextMenuStrip1.RenderStyle.BorderColor = Color.Gainsboro;
            guna2ContextMenuStrip1.RenderStyle.ColorTable = null;
            guna2ContextMenuStrip1.RenderStyle.RoundedEdges = true;
            guna2ContextMenuStrip1.RenderStyle.SelectionArrowColor = Color.White;
            guna2ContextMenuStrip1.RenderStyle.SelectionBackColor = Color.FromArgb(100, 88, 255);
            guna2ContextMenuStrip1.RenderStyle.SelectionForeColor = Color.White;
            guna2ContextMenuStrip1.RenderStyle.SeparatorColor = Color.Gainsboro;
            guna2ContextMenuStrip1.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            guna2ContextMenuStrip1.Size = new Size(61, 4);
            // 
            // tbUsername
            // 
            tbUsername.BorderColor = Color.DarkGray;
            tbUsername.BorderRadius = 5;
            tbUsername.CustomizableEdges = customizableEdges3;
            tbUsername.DefaultText = "";
            tbUsername.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            tbUsername.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            tbUsername.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            tbUsername.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            tbUsername.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            tbUsername.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            tbUsername.ForeColor = Color.FromArgb(91, 91, 91);
            tbUsername.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            tbUsername.Location = new Point(403, 275);
            tbUsername.Margin = new Padding(4);
            tbUsername.Name = "tbUsername";
            tbUsername.PlaceholderText = "";
            tbUsername.SelectedText = "";
            tbUsername.ShadowDecoration.CustomizableEdges = customizableEdges4;
            tbUsername.Size = new Size(388, 45);
            tbUsername.TabIndex = 15;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            checkBox1.ForeColor = Color.FromArgb(91, 91, 91);
            checkBox1.Location = new Point(403, 420);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(138, 20);
            checkBox1.TabIndex = 6;
            checkBox1.Text = "Show Password";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // tbPassword1
            // 
            tbPassword1.BorderColor = Color.DarkGray;
            tbPassword1.BorderRadius = 5;
            tbPassword1.CustomizableEdges = customizableEdges5;
            tbPassword1.DefaultText = "";
            tbPassword1.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            tbPassword1.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            tbPassword1.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            tbPassword1.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            tbPassword1.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            tbPassword1.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            tbPassword1.ForeColor = Color.FromArgb(91, 91, 91);
            tbPassword1.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            tbPassword1.Location = new Point(403, 368);
            tbPassword1.Margin = new Padding(4);
            tbPassword1.Name = "tbPassword1";
            tbPassword1.PlaceholderText = "";
            tbPassword1.SelectedText = "";
            tbPassword1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            tbPassword1.Size = new Size(388, 45);
            tbPassword1.TabIndex = 16;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(11F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1182, 653);
            Controls.Add(tbPassword1);
            Controls.Add(checkBox1);
            Controls.Add(tbUsername);
            Controls.Add(LoginBtn);
            Controls.Add(lblPass);
            Controls.Add(lblLoginDesc);
            Controls.Add(lblGreeting);
            Controls.Add(lblUsername);
            Controls.Add(pictureBox1);
            Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(91, 91, 91);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new Size(1200, 700);
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TriQUE";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox1;
        private CheckBox checkBoxShowPassword;
        private Guna.UI2.WinForms.Guna2Button LoginBtn;
        private Label lblUsername;
        private Label lblPass;
        private Panel panel3;
        private Panel panel5;
        private TextBox tbPassword;
        private Guna.UI2.WinForms.Guna2AnimateWindow guna2AnimateWindow1;
        private Label lblGreeting;
        private Label lblLoginDesc;
        private Guna.UI2.WinForms.Guna2ContextMenuStrip guna2ContextMenuStrip1;
        private Guna.UI2.WinForms.Guna2TextBox tbUsername;
        private CheckBox checkBox1;
        private Guna.UI2.WinForms.Guna2TextBox tbPassword1;
    }
}
