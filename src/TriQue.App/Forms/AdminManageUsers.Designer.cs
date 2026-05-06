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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminManageUsers));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            UserListPanel = new Guna2Panel();
            UserListDataGrid = new Guna2DataGridView();
            AddUserBtn = new Guna2ImageButton();
            SearchBar = new Guna2TextBox();
            lblManageUsersTitle = new Label();
            NavbarPanel = new Panel();
            ManageUserBtn = new Guna2ImageButton();
            LogoutBtn = new Guna2ImageButton();
            ViewQueueBtn = new Guna2ImageButton();
            DashboardBtn = new Guna2ImageButton();
            SettingsBtn = new Guna2ImageButton();
            Logo = new PictureBox();
            GenerateReportBtn = new Guna2ImageButton();
            UserListPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)UserListDataGrid).BeginInit();
            NavbarPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Logo).BeginInit();
            SuspendLayout();
            // 
            // UserListPanel
            // 
            UserListPanel.BorderColor = Color.DarkGray;
            UserListPanel.BorderRadius = 15;
            UserListPanel.BorderThickness = 1;
            UserListPanel.Controls.Add(UserListDataGrid);
            UserListPanel.Controls.Add(AddUserBtn);
            UserListPanel.Controls.Add(SearchBar);
            UserListPanel.CustomizableEdges = customizableEdges4;
            UserListPanel.Location = new Point(131, 83);
            UserListPanel.Margin = new Padding(4);
            UserListPanel.Name = "UserListPanel";
            UserListPanel.ShadowDecoration.CustomizableEdges = customizableEdges5;
            UserListPanel.Size = new Size(1017, 522);
            UserListPanel.TabIndex = 3;
            // 
            // UserListDataGrid
            // 
            UserListDataGrid.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(231, 229, 255);
            UserListDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(50, 100, 230);
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            UserListDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            UserListDataGrid.ColumnHeadersHeight = 40;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.White;
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            UserListDataGrid.DefaultCellStyle = dataGridViewCellStyle3;
            UserListDataGrid.GridColor = Color.White;
            UserListDataGrid.Location = new Point(17, 74);
            UserListDataGrid.Name = "UserListDataGrid";
            UserListDataGrid.RowHeadersVisible = false;
            UserListDataGrid.RowHeadersWidth = 51;
            UserListDataGrid.RowTemplate.Height = 40;
            UserListDataGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            UserListDataGrid.Size = new Size(987, 432);
            UserListDataGrid.TabIndex = 0;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.Font = null;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            UserListDataGrid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            UserListDataGrid.ThemeStyle.BackColor = Color.White;
            UserListDataGrid.ThemeStyle.GridColor = Color.White;
            UserListDataGrid.ThemeStyle.HeaderStyle.BackColor = Color.White;
            UserListDataGrid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            UserListDataGrid.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            UserListDataGrid.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(50, 100, 230);
            UserListDataGrid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            UserListDataGrid.ThemeStyle.HeaderStyle.Height = 40;
            UserListDataGrid.ThemeStyle.ReadOnly = false;
            UserListDataGrid.ThemeStyle.RowsStyle.BackColor = Color.White;
            UserListDataGrid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            UserListDataGrid.ThemeStyle.RowsStyle.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            UserListDataGrid.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            UserListDataGrid.ThemeStyle.RowsStyle.Height = 40;
            UserListDataGrid.ThemeStyle.RowsStyle.SelectionBackColor = Color.White;
            UserListDataGrid.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(64, 64, 64);
            UserListDataGrid.CellClick += UserListDataGrid_CellContentClick;
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
            AddUserBtn.ShadowDecoration.CustomizableEdges = customizableEdges1;
            AddUserBtn.Size = new Size(39, 33);
            AddUserBtn.TabIndex = 6;
            AddUserBtn.Click += AddUserBtn_Click_1;
            // 
            // SearchBar
            // 
            SearchBar.BorderColor = Color.DarkGray;
            SearchBar.BorderRadius = 5;
            SearchBar.CustomizableEdges = customizableEdges2;
            SearchBar.DefaultText = "";
            SearchBar.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            SearchBar.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            SearchBar.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            SearchBar.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            SearchBar.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            SearchBar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SearchBar.ForeColor = Color.FromArgb(91, 91, 91);
            SearchBar.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            SearchBar.Location = new Point(21, 21);
            SearchBar.Margin = new Padding(4);
            SearchBar.Name = "SearchBar";
            SearchBar.PlaceholderText = "Search a driver";
            SearchBar.SelectedText = "";
            SearchBar.ShadowDecoration.CustomizableEdges = customizableEdges3;
            SearchBar.Size = new Size(937, 35);
            SearchBar.TabIndex = 4;
            SearchBar.TextChanged += SearchBar_TextChanged;
            // 
            // lblManageUsersTitle
            // 
            lblManageUsersTitle.AutoSize = true;
            lblManageUsersTitle.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblManageUsersTitle.ForeColor = Color.FromArgb(55, 91, 231);
            lblManageUsersTitle.Location = new Point(131, 38);
            lblManageUsersTitle.Margin = new Padding(4, 0, 4, 0);
            lblManageUsersTitle.Name = "lblManageUsersTitle";
            lblManageUsersTitle.Size = new Size(152, 25);
            lblManageUsersTitle.TabIndex = 4;
            lblManageUsersTitle.Text = "Manage Users";
            lblManageUsersTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // NavbarPanel
            // 
            NavbarPanel.BackColor = Color.FromArgb(224, 224, 224);
            NavbarPanel.Controls.Add(ManageUserBtn);
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
            // ManageUserBtn
            // 
            ManageUserBtn.CheckedState.ImageSize = new Size(64, 64);
            ManageUserBtn.HoverState.ImageSize = new Size(44, 44);
            ManageUserBtn.Image = (Image)resources.GetObject("ManageUserBtn.Image");
            ManageUserBtn.ImageOffset = new Point(0, 0);
            ManageUserBtn.ImageRotate = 0F;
            ManageUserBtn.ImageSize = new Size(42, 42);
            ManageUserBtn.Location = new Point(7, 242);
            ManageUserBtn.Name = "ManageUserBtn";
            ManageUserBtn.PressedState.ImageSize = new Size(44, 44);
            ManageUserBtn.ShadowDecoration.CustomizableEdges = customizableEdges6;
            ManageUserBtn.Size = new Size(82, 82);
            ManageUserBtn.TabIndex = 7;
            ManageUserBtn.Click += ManageUsersBtn_Click;
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
            LogoutBtn.ShadowDecoration.CustomizableEdges = customizableEdges7;
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
            ViewQueueBtn.ShadowDecoration.CustomizableEdges = customizableEdges8;
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
            DashboardBtn.ShadowDecoration.CustomizableEdges = customizableEdges9;
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
            SettingsBtn.ShadowDecoration.CustomizableEdges = customizableEdges10;
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
            GenerateReportBtn.ShadowDecoration.CustomizableEdges = customizableEdges11;
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
            Text = "TriQUE";
            UserListPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)UserListDataGrid).EndInit();
            NavbarPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Logo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Guna.UI2.WinForms.Guna2Panel UserListPanel;
        private Label lblManageUsersTitle;
        private Panel NavbarPanel;
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
        private Guna2ImageButton ManageUserBtn;
    }
}