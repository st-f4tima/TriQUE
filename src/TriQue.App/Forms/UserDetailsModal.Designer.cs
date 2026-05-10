namespace TriQue
{
    partial class UserDetailsModal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserDetailsModal));
            UserInformationPanel = new Guna.UI2.WinForms.Guna2Panel();
            lblGroupNameValue = new Label();
            label1 = new Label();
            GroupIcon = new PictureBox();
            lblDriverStatus = new Label();
            lblPhoneValue = new Label();
            lblBodyValue = new Label();
            lblRoleValue = new Label();
            lblRouteValue = new Label();
            StatusIcon = new PictureBox();
            MapIcon = new PictureBox();
            RoleIcon = new PictureBox();
            NumberIcon = new PictureBox();
            PhoneIcon = new PictureBox();
            lblCurrentStatus = new Label();
            lblAssignedRoute = new Label();
            lblRole = new Label();
            lblBodyNumber = new Label();
            lblPhoneNumber = new Label();
            UserIcon = new PictureBox();
            lblUserRole = new Label();
            lblFullName = new Label();
            UserInformationPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)GroupIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)StatusIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MapIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RoleIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NumberIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PhoneIcon).BeginInit();
            ((System.ComponentModel.ISupportInitialize)UserIcon).BeginInit();
            SuspendLayout();
            // 
            // UserInformationPanel
            // 
            UserInformationPanel.BorderColor = Color.DarkGray;
            UserInformationPanel.BorderRadius = 15;
            UserInformationPanel.BorderThickness = 1;
            UserInformationPanel.Controls.Add(lblGroupNameValue);
            UserInformationPanel.Controls.Add(label1);
            UserInformationPanel.Controls.Add(GroupIcon);
            UserInformationPanel.Controls.Add(lblDriverStatus);
            UserInformationPanel.Controls.Add(lblPhoneValue);
            UserInformationPanel.Controls.Add(lblBodyValue);
            UserInformationPanel.Controls.Add(lblRoleValue);
            UserInformationPanel.Controls.Add(lblRouteValue);
            UserInformationPanel.Controls.Add(StatusIcon);
            UserInformationPanel.Controls.Add(MapIcon);
            UserInformationPanel.Controls.Add(RoleIcon);
            UserInformationPanel.Controls.Add(NumberIcon);
            UserInformationPanel.Controls.Add(PhoneIcon);
            UserInformationPanel.Controls.Add(lblCurrentStatus);
            UserInformationPanel.Controls.Add(lblAssignedRoute);
            UserInformationPanel.Controls.Add(lblRole);
            UserInformationPanel.Controls.Add(lblBodyNumber);
            UserInformationPanel.Controls.Add(lblPhoneNumber);
            UserInformationPanel.CustomizableEdges = customizableEdges1;
            UserInformationPanel.ForeColor = Color.FromArgb(91, 91, 91);
            UserInformationPanel.Location = new Point(24, 126);
            UserInformationPanel.Name = "UserInformationPanel";
            UserInformationPanel.ShadowDecoration.CustomizableEdges = customizableEdges2;
            UserInformationPanel.Size = new Size(624, 303);
            UserInformationPanel.TabIndex = 0;
            // 
            // lblGroupNameValue
            // 
            lblGroupNameValue.AutoSize = true;
            lblGroupNameValue.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblGroupNameValue.Location = new Point(390, 162);
            lblGroupNameValue.Name = "lblGroupNameValue";
            lblGroupNameValue.Size = new Size(84, 22);
            lblGroupNameValue.TabIndex = 17;
            lblGroupNameValue.Text = "Group A";
            lblGroupNameValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(91, 91, 91);
            label1.Location = new Point(53, 165);
            label1.Name = "label1";
            label1.Size = new Size(122, 22);
            label1.TabIndex = 16;
            label1.Text = "Group Name";
            // 
            // GroupIcon
            // 
            GroupIcon.Image = (Image)resources.GetObject("GroupIcon.Image");
            GroupIcon.Location = new Point(18, 162);
            GroupIcon.Name = "GroupIcon";
            GroupIcon.Size = new Size(25, 25);
            GroupIcon.SizeMode = PictureBoxSizeMode.Zoom;
            GroupIcon.TabIndex = 15;
            GroupIcon.TabStop = false;
            GroupIcon.Click += GroupIcon_Click;
            // 
            // lblDriverStatus
            // 
            lblDriverStatus.AutoSize = true;
            lblDriverStatus.BackColor = Color.Transparent;
            lblDriverStatus.ForeColor = Color.FromArgb(91, 91, 91);
            lblDriverStatus.Location = new Point(394, 252);
            lblDriverStatus.Name = "lblDriverStatus";
            lblDriverStatus.Size = new Size(72, 20);
            lblDriverStatus.TabIndex = 10;
            lblDriverStatus.Text = "Waiting";
            lblDriverStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblPhoneValue
            // 
            lblPhoneValue.AutoSize = true;
            lblPhoneValue.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPhoneValue.ForeColor = Color.FromArgb(91, 91, 91);
            lblPhoneValue.Location = new Point(390, 30);
            lblPhoneValue.Name = "lblPhoneValue";
            lblPhoneValue.Size = new Size(131, 22);
            lblPhoneValue.TabIndex = 14;
            lblPhoneValue.Text = "09192544631";
            lblPhoneValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblBodyValue
            // 
            lblBodyValue.AutoSize = true;
            lblBodyValue.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBodyValue.Location = new Point(390, 77);
            lblBodyValue.Name = "lblBodyValue";
            lblBodyValue.Size = new Size(54, 22);
            lblBodyValue.TabIndex = 13;
            lblBodyValue.Text = "1722";
            lblBodyValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRoleValue
            // 
            lblRoleValue.AutoSize = true;
            lblRoleValue.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRoleValue.Location = new Point(390, 124);
            lblRoleValue.Name = "lblRoleValue";
            lblRoleValue.Size = new Size(64, 22);
            lblRoleValue.TabIndex = 12;
            lblRoleValue.Text = "Driver";
            lblRoleValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRouteValue
            // 
            lblRouteValue.AutoSize = true;
            lblRouteValue.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRouteValue.Location = new Point(394, 206);
            lblRouteValue.Name = "lblRouteValue";
            lblRouteValue.Size = new Size(50, 22);
            lblRouteValue.TabIndex = 11;
            lblRouteValue.Text = "BSU";
            lblRouteValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // StatusIcon
            // 
            StatusIcon.Image = (Image)resources.GetObject("StatusIcon.Image");
            StatusIcon.Location = new Point(12, 252);
            StatusIcon.Name = "StatusIcon";
            StatusIcon.Size = new Size(35, 22);
            StatusIcon.SizeMode = PictureBoxSizeMode.Zoom;
            StatusIcon.TabIndex = 9;
            StatusIcon.TabStop = false;
            // 
            // MapIcon
            // 
            MapIcon.Image = (Image)resources.GetObject("MapIcon.Image");
            MapIcon.Location = new Point(12, 206);
            MapIcon.Name = "MapIcon";
            MapIcon.Size = new Size(35, 22);
            MapIcon.SizeMode = PictureBoxSizeMode.Zoom;
            MapIcon.TabIndex = 8;
            MapIcon.TabStop = false;
            // 
            // RoleIcon
            // 
            RoleIcon.Image = (Image)resources.GetObject("RoleIcon.Image");
            RoleIcon.Location = new Point(12, 124);
            RoleIcon.Name = "RoleIcon";
            RoleIcon.Size = new Size(35, 22);
            RoleIcon.SizeMode = PictureBoxSizeMode.Zoom;
            RoleIcon.TabIndex = 7;
            RoleIcon.TabStop = false;
            // 
            // NumberIcon
            // 
            NumberIcon.Image = (Image)resources.GetObject("NumberIcon.Image");
            NumberIcon.Location = new Point(12, 77);
            NumberIcon.Name = "NumberIcon";
            NumberIcon.Size = new Size(35, 22);
            NumberIcon.SizeMode = PictureBoxSizeMode.Zoom;
            NumberIcon.TabIndex = 6;
            NumberIcon.TabStop = false;
            // 
            // PhoneIcon
            // 
            PhoneIcon.Image = (Image)resources.GetObject("PhoneIcon.Image");
            PhoneIcon.Location = new Point(12, 30);
            PhoneIcon.Name = "PhoneIcon";
            PhoneIcon.Size = new Size(35, 22);
            PhoneIcon.SizeMode = PictureBoxSizeMode.Zoom;
            PhoneIcon.TabIndex = 5;
            PhoneIcon.TabStop = false;
            // 
            // lblCurrentStatus
            // 
            lblCurrentStatus.AutoSize = true;
            lblCurrentStatus.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCurrentStatus.ForeColor = Color.FromArgb(91, 91, 91);
            lblCurrentStatus.Location = new Point(53, 252);
            lblCurrentStatus.Name = "lblCurrentStatus";
            lblCurrentStatus.Size = new Size(67, 22);
            lblCurrentStatus.TabIndex = 4;
            lblCurrentStatus.Text = "Status";
            // 
            // lblAssignedRoute
            // 
            lblAssignedRoute.AutoSize = true;
            lblAssignedRoute.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAssignedRoute.ForeColor = Color.FromArgb(91, 91, 91);
            lblAssignedRoute.Location = new Point(53, 206);
            lblAssignedRoute.Name = "lblAssignedRoute";
            lblAssignedRoute.Size = new Size(151, 22);
            lblAssignedRoute.TabIndex = 3;
            lblAssignedRoute.Text = "Assigned Route";
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblRole.ForeColor = Color.FromArgb(91, 91, 91);
            lblRole.Location = new Point(53, 124);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(51, 22);
            lblRole.TabIndex = 2;
            lblRole.Text = "Role";
            // 
            // lblBodyNumber
            // 
            lblBodyNumber.AutoSize = true;
            lblBodyNumber.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblBodyNumber.ForeColor = Color.FromArgb(91, 91, 91);
            lblBodyNumber.Location = new Point(53, 77);
            lblBodyNumber.Name = "lblBodyNumber";
            lblBodyNumber.Size = new Size(130, 22);
            lblBodyNumber.TabIndex = 1;
            lblBodyNumber.Text = "Body Number";
            // 
            // lblPhoneNumber
            // 
            lblPhoneNumber.AutoSize = true;
            lblPhoneNumber.Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPhoneNumber.ForeColor = Color.FromArgb(91, 91, 91);
            lblPhoneNumber.Location = new Point(53, 30);
            lblPhoneNumber.Name = "lblPhoneNumber";
            lblPhoneNumber.Size = new Size(142, 22);
            lblPhoneNumber.TabIndex = 0;
            lblPhoneNumber.Text = "Phone Number";
            // 
            // UserIcon
            // 
            UserIcon.Image = (Image)resources.GetObject("UserIcon.Image");
            UserIcon.Location = new Point(24, 30);
            UserIcon.Name = "UserIcon";
            UserIcon.Size = new Size(100, 75);
            UserIcon.SizeMode = PictureBoxSizeMode.Zoom;
            UserIcon.TabIndex = 1;
            UserIcon.TabStop = false;
            // 
            // lblUserRole
            // 
            lblUserRole.AutoSize = true;
            lblUserRole.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblUserRole.ForeColor = Color.FromArgb(91, 91, 91);
            lblUserRole.Location = new Point(146, 80);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Size = new Size(69, 25);
            lblUserRole.TabIndex = 2;
            lblUserRole.Text = "Driver";
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.Font = new Font("Microsoft Sans Serif", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblFullName.ForeColor = Color.FromArgb(91, 91, 91);
            lblFullName.Location = new Point(146, 38);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(266, 39);
            lblFullName.TabIndex = 3;
            lblFullName.Text = "Juan Dela Cruz";
            // 
            // UserDetailsModal
            // 
            AutoScaleDimensions = new SizeF(11F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(674, 455);
            Controls.Add(lblFullName);
            Controls.Add(lblUserRole);
            Controls.Add(UserIcon);
            Controls.Add(UserInformationPanel);
            Font = new Font("Microsoft Sans Serif", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            Name = "UserDetailsModal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "TriQUE - View User Detail";
            UserInformationPanel.ResumeLayout(false);
            UserInformationPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GroupIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)StatusIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)MapIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)RoleIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)NumberIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)PhoneIcon).EndInit();
            ((System.ComponentModel.ISupportInitialize)UserIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel UserInformationPanel;
        private PictureBox UserIcon;
        private Label lblUserRole;
        private Label lblFullName;
        private Label lblCurrentStatus;
        private Label lblAssignedRoute;
        private Label lblRole;
        private Label lblBodyNumber;
        private Label lblPhoneNumber;
        private PictureBox PhoneIcon;
        private PictureBox StatusIcon;
        private PictureBox MapIcon;
        private PictureBox RoleIcon;
        private PictureBox NumberIcon;
        private Label lblBodyValue;
        private Label lblRoleValue;
        private Label lblRouteValue;
        private Label lblDriverStatus;
        private Label lblPhoneValue;
        private PictureBox GroupIcon;
        private Label lblGroupNameValue;
        private Label label1;
    }
}