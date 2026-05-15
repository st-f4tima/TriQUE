namespace TriQue.Forms
{
    partial class SetPasswordModal
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetPasswordModal));
            lblNewPassword = new Label();
            txtNewPassword = new Guna.UI2.WinForms.Guna2TextBox();
            lblConfirmPassword = new Label();
            chkShowNew = new CheckBox();
            txtConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();
            chkShowConfirm = new CheckBox();
            label2 = new Label();
            lblError = new Label();
            ConfirmBtn = new Guna.UI2.WinForms.Guna2Button();
            SuspendLayout();
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNewPassword.ForeColor = Color.FromArgb(91, 91, 91);
            lblNewPassword.Location = new Point(36, 42);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(133, 20);
            lblNewPassword.TabIndex = 5;
            lblNewPassword.Text = "New Password";
            // 
            // txtNewPassword
            // 
            txtNewPassword.BorderColor = Color.DarkGray;
            txtNewPassword.BorderRadius = 5;
            txtNewPassword.CustomizableEdges = customizableEdges1;
            txtNewPassword.DefaultText = "";
            txtNewPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtNewPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtNewPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtNewPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtNewPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtNewPassword.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            txtNewPassword.ForeColor = Color.FromArgb(91, 91, 91);
            txtNewPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtNewPassword.Location = new Point(36, 70);
            txtNewPassword.Margin = new Padding(4);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PlaceholderForeColor = Color.Gray;
            txtNewPassword.PlaceholderText = "Enter new password";
            txtNewPassword.SelectedText = "";
            txtNewPassword.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtNewPassword.Size = new Size(411, 35);
            txtNewPassword.TabIndex = 6;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConfirmPassword.ForeColor = Color.FromArgb(91, 91, 91);
            lblConfirmPassword.Location = new Point(36, 167);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(163, 20);
            lblConfirmPassword.TabIndex = 7;
            lblConfirmPassword.Text = "Confirm Password";
            // 
            // chkShowNew
            // 
            chkShowNew.AutoSize = true;
            chkShowNew.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkShowNew.ForeColor = Color.FromArgb(91, 91, 91);
            chkShowNew.Location = new Point(36, 112);
            chkShowNew.Name = "chkShowNew";
            chkShowNew.Size = new Size(138, 20);
            chkShowNew.TabIndex = 8;
            chkShowNew.Text = "Show Password";
            chkShowNew.UseVisualStyleBackColor = true;
            chkShowNew.CheckedChanged += chkShowNew_CheckedChanged_1;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderColor = Color.DarkGray;
            txtConfirmPassword.BorderRadius = 5;
            txtConfirmPassword.CustomizableEdges = customizableEdges3;
            txtConfirmPassword.DefaultText = "";
            txtConfirmPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtConfirmPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtConfirmPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtConfirmPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtConfirmPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtConfirmPassword.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            txtConfirmPassword.ForeColor = Color.FromArgb(91, 91, 91);
            txtConfirmPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtConfirmPassword.Location = new Point(36, 191);
            txtConfirmPassword.Margin = new Padding(4);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PlaceholderForeColor = Color.Gray;
            txtConfirmPassword.PlaceholderText = "Re-enter new password";
            txtConfirmPassword.SelectedText = "";
            txtConfirmPassword.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtConfirmPassword.Size = new Size(411, 35);
            txtConfirmPassword.TabIndex = 9;
            // 
            // chkShowConfirm
            // 
            chkShowConfirm.AutoSize = true;
            chkShowConfirm.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkShowConfirm.ForeColor = Color.FromArgb(91, 91, 91);
            chkShowConfirm.Location = new Point(36, 233);
            chkShowConfirm.Name = "chkShowConfirm";
            chkShowConfirm.Size = new Size(138, 20);
            chkShowConfirm.TabIndex = 10;
            chkShowConfirm.Text = "Show Password";
            chkShowConfirm.UseVisualStyleBackColor = true;
            chkShowConfirm.CheckedChanged += chkShowConfirm_CheckedChanged_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.White;
            label2.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(36, 265);
            label2.Name = "label2";
            label2.Size = new Size(0, 18);
            label2.TabIndex = 12;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.BackColor = Color.White;
            lblError.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            lblError.ForeColor = Color.Gray;
            lblError.Location = new Point(36, 265);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 18);
            lblError.TabIndex = 13;
            // 
            // ConfirmBtn
            // 
            ConfirmBtn.BorderRadius = 10;
            ConfirmBtn.CustomizableEdges = customizableEdges5;
            ConfirmBtn.DisabledState.BorderColor = Color.DarkGray;
            ConfirmBtn.DisabledState.CustomBorderColor = Color.DarkGray;
            ConfirmBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            ConfirmBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            ConfirmBtn.FillColor = Color.FromArgb(55, 91, 231);
            ConfirmBtn.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ConfirmBtn.ForeColor = Color.White;
            ConfirmBtn.Location = new Point(36, 296);
            ConfirmBtn.Name = "ConfirmBtn";
            ConfirmBtn.ShadowDecoration.CustomizableEdges = customizableEdges6;
            ConfirmBtn.Size = new Size(411, 48);
            ConfirmBtn.TabIndex = 18;
            ConfirmBtn.Text = "Confirm";
            ConfirmBtn.Click += ConfirmBtn_Click;
            // 
            // SetPasswordModal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(493, 356);
            Controls.Add(ConfirmBtn);
            Controls.Add(lblError);
            Controls.Add(label2);
            Controls.Add(chkShowConfirm);
            Controls.Add(txtConfirmPassword);
            Controls.Add(chkShowNew);
            Controls.Add(lblConfirmPassword);
            Controls.Add(txtNewPassword);
            Controls.Add(lblNewPassword);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SetPasswordModal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TriQUE - Set Up Password";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblNewPassword;
        private Guna.UI2.WinForms.Guna2TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private CheckBox chkShowNew;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmPassword;
        private CheckBox chkShowConfirm;
        private Label label2;
        private Label lblError;
        private Guna.UI2.WinForms.Guna2Button ConfirmBtn;
    }
}