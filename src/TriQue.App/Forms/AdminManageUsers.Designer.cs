namespace Trique.Forms
{
    partial class AdminManageUsers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminManageUsers));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            UsersPanel = new Guna.UI2.WinForms.Guna2Panel();
            lblManageUsersTitle = new Label();
            NavbarPanel = new Panel();
            ManageUsersBtn = new Guna.UI2.WinForms.Guna2ImageButton();
            LogoutBtn = new Guna.UI2.WinForms.Guna2ImageButton();
            ViewQueueBtn = new Guna.UI2.WinForms.Guna2ImageButton();
            DashboardBtn = new Guna.UI2.WinForms.Guna2ImageButton();
            SettingsBtn = new Guna.UI2.WinForms.Guna2ImageButton();
            Logo = new PictureBox();
            GenerateReportBtn = new Guna.UI2.WinForms.Guna2ImageButton();
            NavbarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Logo).BeginInit();
            SuspendLayout();
            // 
            // UsersPanel
            // 
            UsersPanel.BorderColor = Color.FromArgb(84, 84, 84);
            UsersPanel.BorderThickness = 1;
            UsersPanel.CustomizableEdges = customizableEdges1;
            UsersPanel.Location = new Point(131, 114);
            UsersPanel.Margin = new Padding(4);
            UsersPanel.Name = "UsersPanel";
            UsersPanel.ShadowDecoration.CustomizableEdges = customizableEdges2;
            UsersPanel.Size = new Size(1020, 508);
            UsersPanel.TabIndex = 3;
            // 
            // lblManageUsersTitle
            // 
            lblManageUsersTitle.AutoSize = true;
            lblManageUsersTitle.ForeColor = Color.FromArgb(55, 91, 231);
            lblManageUsersTitle.Location = new Point(131, 72);
            lblManageUsersTitle.Margin = new Padding(4, 0, 4, 0);
            lblManageUsersTitle.Name = "lblManageUsersTitle";
            lblManageUsersTitle.Size = new Size(181, 29);
            lblManageUsersTitle.TabIndex = 4;
            lblManageUsersTitle.Text = "Manage Users";
            // 
            // NavbarPanel
            // 
            NavbarPanel.BackColor = Color.FromArgb(215, 215, 215);
            NavbarPanel.Controls.Add(ManageUsersBtn);
            NavbarPanel.Controls.Add(LogoutBtn);
            NavbarPanel.Controls.Add(ViewQueueBtn);
            NavbarPanel.Controls.Add(DashboardBtn);
            NavbarPanel.Controls.Add(SettingsBtn);
            NavbarPanel.Controls.Add(Logo);
            NavbarPanel.Controls.Add(GenerateReportBtn);
            NavbarPanel.Location = new Point(0, -9);
            NavbarPanel.Margin = new Padding(4, 3, 4, 3);
            NavbarPanel.Name = "NavbarPanel";
            NavbarPanel.Size = new Size(96, 660);
            NavbarPanel.TabIndex = 5;
            // 
            // ManageUsersBtn
            // 
            ManageUsersBtn.CheckedState.ImageSize = new Size(64, 64);
            ManageUsersBtn.HoverState.ImageSize = new Size(37, 37);
            ManageUsersBtn.Image = (Image)resources.GetObject("ManageUsersBtn.Image");
            ManageUsersBtn.ImageOffset = new Point(0, 0);
            ManageUsersBtn.ImageRotate = 0F;
            ManageUsersBtn.ImageSize = new Size(42, 42);
            ManageUsersBtn.Location = new Point(7, 244);
            ManageUsersBtn.Name = "ManageUsersBtn";
            ManageUsersBtn.PressedState.ImageSize = new Size(34, 34);
            ManageUsersBtn.ShadowDecoration.CustomizableEdges = customizableEdges3;
            ManageUsersBtn.Size = new Size(82, 82);
            ManageUsersBtn.TabIndex = 7;
            ManageUsersBtn.Click += ManageUsersBtn_Click;
            // 
            // LogoutBtn
            // 
            LogoutBtn.CheckedState.ImageSize = new Size(64, 64);
            LogoutBtn.HoverState.ImageSize = new Size(37, 37);
            LogoutBtn.Image = (Image)resources.GetObject("LogoutBtn.Image");
            LogoutBtn.ImageOffset = new Point(0, 0);
            LogoutBtn.ImageRotate = 0F;
            LogoutBtn.ImageSize = new Size(36, 36);
            LogoutBtn.Location = new Point(10, 549);
            LogoutBtn.Name = "LogoutBtn";
            LogoutBtn.PressedState.ImageSize = new Size(34, 34);
            LogoutBtn.ShadowDecoration.CustomizableEdges = customizableEdges4;
            LogoutBtn.Size = new Size(82, 82);
            LogoutBtn.TabIndex = 6;
            LogoutBtn.Click += LogoutBtn_Click;
            // 
            // ViewQueueBtn
            // 
            ViewQueueBtn.CheckedState.ImageSize = new Size(64, 64);
            ViewQueueBtn.HoverState.ImageSize = new Size(37, 37);
            ViewQueueBtn.Image = (Image)resources.GetObject("ViewQueueBtn.Image");
            ViewQueueBtn.ImageOffset = new Point(0, 0);
            ViewQueueBtn.ImageRotate = 0F;
            ViewQueueBtn.ImageSize = new Size(32, 32);
            ViewQueueBtn.Location = new Point(7, 184);
            ViewQueueBtn.Name = "ViewQueueBtn";
            ViewQueueBtn.PressedState.ImageSize = new Size(34, 34);
            ViewQueueBtn.ShadowDecoration.CustomizableEdges = customizableEdges5;
            ViewQueueBtn.Size = new Size(82, 82);
            ViewQueueBtn.TabIndex = 2;
            ViewQueueBtn.Click += ViewQueueBtn_Click_1;
            // 
            // DashboardBtn
            // 
            DashboardBtn.CheckedState.ImageSize = new Size(64, 64);
            DashboardBtn.HoverState.ImageSize = new Size(37, 37);
            DashboardBtn.Image = (Image)resources.GetObject("DashboardBtn.Image");
            DashboardBtn.ImageOffset = new Point(0, 0);
            DashboardBtn.ImageRotate = 0F;
            DashboardBtn.ImageSize = new Size(32, 32);
            DashboardBtn.Location = new Point(7, 123);
            DashboardBtn.Name = "DashboardBtn";
            DashboardBtn.PressedState.ImageSize = new Size(33, 33);
            DashboardBtn.ShadowDecoration.CustomizableEdges = customizableEdges6;
            DashboardBtn.Size = new Size(82, 82);
            DashboardBtn.TabIndex = 1;
            DashboardBtn.Click += DashboardBtn_Click;
            // 
            // SettingsBtn
            // 
            SettingsBtn.CheckedState.ImageSize = new Size(64, 64);
            SettingsBtn.HoverState.ImageSize = new Size(37, 37);
            SettingsBtn.Image = (Image)resources.GetObject("SettingsBtn.Image");
            SettingsBtn.ImageOffset = new Point(0, 0);
            SettingsBtn.ImageRotate = 0F;
            SettingsBtn.ImageSize = new Size(36, 36);
            SettingsBtn.Location = new Point(7, 370);
            SettingsBtn.Name = "SettingsBtn";
            SettingsBtn.PressedState.ImageSize = new Size(34, 34);
            SettingsBtn.ShadowDecoration.CustomizableEdges = customizableEdges7;
            SettingsBtn.Size = new Size(82, 82);
            SettingsBtn.TabIndex = 5;
            SettingsBtn.Click += SettingsBtn_Click;
            // 
            // Logo
            // 
            Logo.Image = (Image)resources.GetObject("Logo.Image");
            Logo.Location = new Point(7, 47);
            Logo.Name = "Logo";
            Logo.Size = new Size(82, 62);
            Logo.SizeMode = PictureBoxSizeMode.Zoom;
            Logo.TabIndex = 0;
            Logo.TabStop = false;
            // 
            // GenerateReportBtn
            // 
            GenerateReportBtn.CheckedState.ImageSize = new Size(64, 64);
            GenerateReportBtn.HoverState.ImageSize = new Size(37, 37);
            GenerateReportBtn.Image = (Image)resources.GetObject("GenerateReportBtn.Image");
            GenerateReportBtn.ImageOffset = new Point(0, 0);
            GenerateReportBtn.ImageRotate = 0F;
            GenerateReportBtn.ImageSize = new Size(36, 36);
            GenerateReportBtn.Location = new Point(8, 307);
            GenerateReportBtn.Name = "GenerateReportBtn";
            GenerateReportBtn.PressedState.ImageSize = new Size(34, 34);
            GenerateReportBtn.ShadowDecoration.CustomizableEdges = customizableEdges8;
            GenerateReportBtn.Size = new Size(82, 82);
            GenerateReportBtn.TabIndex = 4;
            GenerateReportBtn.Click += GenerateReportBtn_Click_1;
            // 
            // AdminManageUsers
            // 
            AutoScaleDimensions = new SizeF(15F, 29F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1182, 653);
            Controls.Add(NavbarPanel);
            Controls.Add(lblManageUsersTitle);
            Controls.Add(UsersPanel);
            Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(5, 4, 5, 4);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "AdminManageUsers";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TriQue";
            NavbarPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Logo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel UsersPanel;
        private Label lblManageUsersTitle;
        private Panel NavbarPanel;
        private Guna.UI2.WinForms.Guna2ImageButton ManageUsersBtn;
        private Guna.UI2.WinForms.Guna2ImageButton LogoutBtn;
        private Guna.UI2.WinForms.Guna2ImageButton ViewQueueBtn;
        private Guna.UI2.WinForms.Guna2ImageButton DashboardBtn;
        private Guna.UI2.WinForms.Guna2ImageButton SettingsBtn;
        private PictureBox Logo;
        private Guna.UI2.WinForms.Guna2ImageButton GenerateReportBtn;
    }
}