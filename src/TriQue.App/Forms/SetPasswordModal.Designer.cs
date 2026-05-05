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
            usernamelb = new Label();
            txtNewPassword = new Guna.UI2.WinForms.Guna2TextBox();
            label1 = new Label();
            chkShowNew = new CheckBox();
            txtConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();
            chkShowConfirm = new CheckBox();
            ConfirmBtn = new Guna.UI2.WinForms.Guna2Button();
            label2 = new Label();
            lblError = new Label();
            SuspendLayout();
            // 
            // usernamelb
            // 
            usernamelb.AutoSize = true;
            usernamelb.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            usernamelb.ForeColor = Color.FromArgb(91, 91, 91);
            usernamelb.Location = new Point(29, 42);
            usernamelb.Name = "usernamelb";
            usernamelb.Size = new Size(133, 20);
            usernamelb.TabIndex = 5;
            usernamelb.Text = "New Password";
            // 
            // txtNewPassword
            // 
            txtNewPassword.BorderColor = Color.FromArgb(217, 221, 226);
            txtNewPassword.BorderRadius = 5;
            txtNewPassword.CustomizableEdges = customizableEdges1;
            txtNewPassword.DefaultText = "";
            txtNewPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtNewPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtNewPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtNewPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtNewPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtNewPassword.Font = new Font("Segoe UI", 9F);
            txtNewPassword.ForeColor = Color.FromArgb(91, 91, 91);
            txtNewPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtNewPassword.Location = new Point(29, 78);
            txtNewPassword.Margin = new Padding(3, 4, 3, 4);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.PlaceholderText = "Enter new password";
            txtNewPassword.SelectedText = "";
            txtNewPassword.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtNewPassword.Size = new Size(435, 39);
            txtNewPassword.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(91, 91, 91);
            label1.Location = new Point(29, 167);
            label1.Name = "label1";
            label1.Size = new Size(163, 20);
            label1.TabIndex = 7;
            label1.Text = "Confirm Password";
            // 
            // chkShowNew
            // 
            chkShowNew.AutoSize = true;
            chkShowNew.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkShowNew.ForeColor = Color.FromArgb(91, 91, 91);
            chkShowNew.Location = new Point(29, 124);
            chkShowNew.Name = "chkShowNew";
            chkShowNew.Size = new Size(138, 20);
            chkShowNew.TabIndex = 8;
            chkShowNew.Text = "Show Password";
            chkShowNew.UseVisualStyleBackColor = true;
            chkShowNew.CheckedChanged += chkShowNew_CheckedChanged_1;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.BorderColor = Color.FromArgb(217, 221, 226);
            txtConfirmPassword.BorderRadius = 5;
            txtConfirmPassword.CustomizableEdges = customizableEdges3;
            txtConfirmPassword.DefaultText = "";
            txtConfirmPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtConfirmPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtConfirmPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtConfirmPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtConfirmPassword.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtConfirmPassword.Font = new Font("Segoe UI", 9F);
            txtConfirmPassword.ForeColor = Color.FromArgb(91, 91, 91);
            txtConfirmPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtConfirmPassword.Location = new Point(29, 202);
            txtConfirmPassword.Margin = new Padding(3, 4, 3, 4);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.PlaceholderText = "Re-enter new password";
            txtConfirmPassword.SelectedText = "";
            txtConfirmPassword.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtConfirmPassword.Size = new Size(435, 39);
            txtConfirmPassword.TabIndex = 9;
            // 
            // chkShowConfirm
            // 
            chkShowConfirm.AutoSize = true;
            chkShowConfirm.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            chkShowConfirm.ForeColor = Color.FromArgb(91, 91, 91);
            chkShowConfirm.Location = new Point(29, 248);
            chkShowConfirm.Name = "chkShowConfirm";
            chkShowConfirm.Size = new Size(138, 20);
            chkShowConfirm.TabIndex = 10;
            chkShowConfirm.Text = "Show Password";
            chkShowConfirm.UseVisualStyleBackColor = true;
            chkShowConfirm.CheckedChanged += chkShowConfirm_CheckedChanged_1;
            // 
            // ConfirmBtn
            // 
            ConfirmBtn.BorderRadius = 15;
            ConfirmBtn.CustomizableEdges = customizableEdges5;
            ConfirmBtn.DisabledState.BorderColor = Color.DarkGray;
            ConfirmBtn.DisabledState.CustomBorderColor = Color.DarkGray;
            ConfirmBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            ConfirmBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            ConfirmBtn.FillColor = Color.FromArgb(55, 91, 231);
            ConfirmBtn.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ConfirmBtn.ForeColor = Color.White;
            ConfirmBtn.HoverState.BorderColor = Color.FromArgb(0, 50, 125);
            ConfirmBtn.HoverState.CustomBorderColor = Color.FromArgb(0, 50, 125);
            ConfirmBtn.HoverState.FillColor = Color.FromArgb(0, 50, 125);
            ConfirmBtn.HoverState.Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ConfirmBtn.HoverState.ForeColor = Color.White;
            ConfirmBtn.Location = new Point(303, 280);
            ConfirmBtn.Name = "ConfirmBtn";
            ConfirmBtn.PressedColor = Color.FromArgb(0, 50, 125);
            ConfirmBtn.ShadowDecoration.CustomizableEdges = customizableEdges6;
            ConfirmBtn.Size = new Size(161, 44);
            ConfirmBtn.TabIndex = 11;
            ConfirmBtn.Text = "Confirm";
            ConfirmBtn.Click += ConfirmBtn_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(29, 292);
            label2.Name = "label2";
            label2.Size = new Size(0, 20);
            label2.TabIndex = 12;
            // 
            // lblError
            // 
            lblError.AutoSize = true;
            lblError.Location = new Point(29, 292);
            lblError.Name = "lblError";
            lblError.Size = new Size(0, 20);
            lblError.TabIndex = 13;
            // 
            // SetPasswordModal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(493, 341);
            Controls.Add(lblError);
            Controls.Add(label2);
            Controls.Add(ConfirmBtn);
            Controls.Add(chkShowConfirm);
            Controls.Add(txtConfirmPassword);
            Controls.Add(chkShowNew);
            Controls.Add(label1);
            Controls.Add(txtNewPassword);
            Controls.Add(usernamelb);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SetPasswordModal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TriQUE - Set Up Password";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label usernamelb;
        private Guna.UI2.WinForms.Guna2TextBox txtNewPassword;
        private Label label1;
        private CheckBox chkShowNew;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmPassword;
        private CheckBox chkShowConfirm;
        private Guna.UI2.WinForms.Guna2Button ConfirmBtn;
        private Label label2;
        private Label lblError;
    }
}