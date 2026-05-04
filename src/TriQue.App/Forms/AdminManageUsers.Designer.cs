using Guna.UI2.WinForms;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminManageUsers));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            UserListPanel = new Guna2Panel();
            AddUserBtn = new Guna2ImageButton();
            SearchBar = new Guna2TextBox();
            lblManageUsersTitle = new Label();
            NavbarPanel = new Panel();
            ManageUsersBtn = new Guna2ImageButton();
            LogoutBtn = new Guna2ImageButton();
            ViewQueueBtn = new Guna2ImageButton();
            DashboardBtn = new Guna2ImageButton();
            SettingsBtn = new Guna2ImageButton();
            Logo = new PictureBox();
            GenerateReportBtn = new Guna2ImageButton();
            UserListDataGrid = new Guna2DataGridView();
            UserListPanel.SuspendLayout();
            NavbarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Logo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)UserListDataGrid).BeginInit();
            SuspendLayout();
            // 
            // UserListPanel
            // 
            UserListPanel.BorderColor = Color.FromArgb(84, 84, 84);
            UserListPanel.BorderThickness = 1;
            UserListPanel.Controls.Add(UserListDataGrid);
            UserListPanel.Controls.Add(AddUserBtn);
            UserListPanel.Controls.Add(SearchBar);
            UserListPanel.CustomizableEdges = customizableEdges15;
            UserListPanel.Location = new Point(131, 141);
            UserListPanel.Margin = new Padding(4);
            UserListPanel.Name = "UserListPanel";
            UserListPanel.ShadowDecoration.CustomizableEdges = customizableEdges16;
            UserListPanel.Size = new Size(1017, 481);
            UserListPanel.TabIndex = 3;
            // 
            // AddUserBtn
            // 
            AddUserBtn.BackColor = Color.FromArgb(55, 91, 231);
            AddUserBtn.CheckedState.ImageSize = new Size(30, 30);
            AddUserBtn.HoverState.ImageSize = new Size(31, 31);
            AddUserBtn.Image = (Image)resources.GetObject("AddUserBtn.Image");
            AddUserBtn.ImageOffset = new Point(0, 0);
            AddUserBtn.ImageRotate = 0F;
            AddUserBtn.ImageSize = new Size(30, 30);
            AddUserBtn.Location = new Point(965, 21);
            AddUserBtn.Name = "AddUserBtn";
            AddUserBtn.PressedState.ImageSize = new Size(30, 30);
            AddUserBtn.ShadowDecoration.CustomizableEdges = customizableEdges12;
            AddUserBtn.Size = new Size(39, 33);
            AddUserBtn.TabIndex = 6;
            // 
            // SearchBar
            // 
            SearchBar.BorderColor = Color.FromArgb(91, 91, 91);
            SearchBar.CustomizableEdges = customizableEdges13;
            SearchBar.DefaultText = "";
            SearchBar.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            SearchBar.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            SearchBar.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            SearchBar.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            SearchBar.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            SearchBar.Font = new Font("Segoe UI", 9F);
            SearchBar.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            SearchBar.Location = new Point(17, 21);
            SearchBar.Margin = new Padding(3, 4, 3, 4);
            SearchBar.Name = "SearchBar";
            SearchBar.PlaceholderText = "Search a driver";
            SearchBar.SelectedText = "";
            SearchBar.ShadowDecoration.CustomizableEdges = customizableEdges14;
            SearchBar.Size = new Size(942, 33);
            SearchBar.TabIndex = 4;
            // 
            // lblManageUsersTitle
            // 
            lblManageUsersTitle.AutoSize = true;
            lblManageUsersTitle.ForeColor = Color.FromArgb(55, 91, 231);
            lblManageUsersTitle.Location = new Point(121, 71);
            lblManageUsersTitle.Margin = new Padding(4, 0, 4, 0);
            lblManageUsersTitle.Name = "lblManageUsersTitle";
            lblManageUsersTitle.Size = new Size(181, 29);
            lblManageUsersTitle.TabIndex = 4;
            lblManageUsersTitle.Text = "Manage Users";
            lblManageUsersTitle.TextAlign = ContentAlignment.MiddleLeft;
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
            ManageUsersBtn.ShadowDecoration.CustomizableEdges = customizableEdges17;
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
            LogoutBtn.ShadowDecoration.CustomizableEdges = customizableEdges18;
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
            ViewQueueBtn.ShadowDecoration.CustomizableEdges = customizableEdges19;
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
            DashboardBtn.ShadowDecoration.CustomizableEdges = customizableEdges20;
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
            SettingsBtn.ShadowDecoration.CustomizableEdges = customizableEdges21;
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
            GenerateReportBtn.ShadowDecoration.CustomizableEdges = customizableEdges22;
            GenerateReportBtn.Size = new Size(82, 82);
            GenerateReportBtn.TabIndex = 4;
            GenerateReportBtn.Click += GenerateReportBtn_Click_1;
            // 
            // UserListDataGrid
            // 
            dataGridViewCellStyle4.BackColor = Color.White;
            UserListDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.White;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            UserListDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            UserListDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.White;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            UserListDataGrid.DefaultCellStyle = dataGridViewCellStyle6;
            UserListDataGrid.GridColor = Color.FromArgb(231, 229, 255);
            UserListDataGrid.Location = new Point(17, 76);
            UserListDataGrid.Name = "UserListDataGrid";
            UserListDataGrid.RowHeadersVisible = false;
            UserListDataGrid.RowHeadersWidth = 51;
            UserListDataGrid.Size = new Size(987, 388);
            UserListDataGrid.TabIndex = 7;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.Font = null;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            UserListDataGrid.ThemeStyle.BackColor = Color.White;
            UserListDataGrid.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            UserListDataGrid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            UserListDataGrid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            UserListDataGrid.ThemeStyle.HeaderStyle.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            UserListDataGrid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            UserListDataGrid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            UserListDataGrid.ThemeStyle.HeaderStyle.Height = 4;
            UserListDataGrid.ThemeStyle.ReadOnly = false;
            UserListDataGrid.ThemeStyle.RowsStyle.BackColor = Color.White;
            UserListDataGrid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            UserListDataGrid.ThemeStyle.RowsStyle.Font = new Font("Microsoft Sans Serif", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            UserListDataGrid.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            UserListDataGrid.ThemeStyle.RowsStyle.Height = 29;
            UserListDataGrid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            UserListDataGrid.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // AdminManageUsers
            // 
            AutoScaleDimensions = new SizeF(15F, 29F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1182, 653);
            Controls.Add(NavbarPanel);
            Controls.Add(lblManageUsersTitle);
            Controls.Add(UserListPanel);
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
            UserListPanel.ResumeLayout(false);
            NavbarPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Logo).EndInit();
            ((System.ComponentModel.ISupportInitialize)UserListDataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel UserListPanel;
        private Label lblManageUsersTitle;
        private Panel NavbarPanel;
        private Guna.UI2.WinForms.Guna2ImageButton ManageUsersBtn;
        private Guna.UI2.WinForms.Guna2ImageButton LogoutBtn;
        private Guna.UI2.WinForms.Guna2ImageButton ViewQueueBtn;
        private Guna.UI2.WinForms.Guna2ImageButton DashboardBtn;
        private Guna.UI2.WinForms.Guna2ImageButton SettingsBtn;
        private PictureBox Logo;
        private Guna.UI2.WinForms.Guna2ImageButton GenerateReportBtn;
        private Guna.UI2.WinForms.Guna2ImageButton SearchBtn;
        private Guna.UI2.WinForms.Guna2TextBox SearchBar;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2ImageButton AddUserBtn;
        private Guna2DataGridView UserListDataGrid;
    }
}