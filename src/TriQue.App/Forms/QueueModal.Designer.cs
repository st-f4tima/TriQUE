namespace Trique.Forms
{
    partial class QueueModal
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueModal));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            SearchBar = new Guna.UI2.WinForms.Guna2TextBox();
            guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            SearchBtn = new Guna.UI2.WinForms.Guna2ImageButton();
            DriverListDataGrid = new Guna.UI2.WinForms.Guna2DataGridView();
            UpdateStatusBtn = new Guna.UI2.WinForms.Guna2Button();
            ResetQueueBtn = new Guna.UI2.WinForms.Guna2Button();
            guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)DriverListDataGrid).BeginInit();
            SuspendLayout();
            // 
            // SearchBar
            // 
            SearchBar.BorderColor = Color.DarkGray;
            SearchBar.BorderRadius = 5;
            SearchBar.CustomizableEdges = customizableEdges1;
            SearchBar.DefaultText = "";
            SearchBar.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            SearchBar.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            SearchBar.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            SearchBar.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            SearchBar.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            SearchBar.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold);
            SearchBar.ForeColor = Color.FromArgb(91, 91, 91);
            SearchBar.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            SearchBar.Location = new Point(75, 12);
            SearchBar.Margin = new Padding(4);
            SearchBar.Name = "SearchBar";
            SearchBar.PlaceholderText = "Search a driver";
            SearchBar.SelectedText = "";
            SearchBar.ShadowDecoration.CustomizableEdges = customizableEdges2;
            SearchBar.Size = new Size(701, 33);
            SearchBar.TabIndex = 0;
            // 
            // guna2Panel1
            // 
            guna2Panel1.BorderColor = Color.DarkGray;
            guna2Panel1.BorderRadius = 5;
            guna2Panel1.BorderThickness = 1;
            guna2Panel1.Controls.Add(SearchBtn);
            guna2Panel1.CustomizableEdges = customizableEdges4;
            guna2Panel1.Location = new Point(27, 12);
            guna2Panel1.Name = "guna2Panel1";
            guna2Panel1.ShadowDecoration.CustomizableEdges = customizableEdges5;
            guna2Panel1.Size = new Size(41, 33);
            guna2Panel1.TabIndex = 1;
            // 
            // SearchBtn
            // 
            SearchBtn.CheckedState.ImageSize = new Size(31, 31);
            SearchBtn.HoverState.ImageSize = new Size(31, 31);
            SearchBtn.Image = (Image)resources.GetObject("SearchBtn.Image");
            SearchBtn.ImageOffset = new Point(0, 0);
            SearchBtn.ImageRotate = 0F;
            SearchBtn.ImageSize = new Size(30, 30);
            SearchBtn.Location = new Point(3, 3);
            SearchBtn.Name = "SearchBtn";
            SearchBtn.PressedState.ImageSize = new Size(31, 31);
            SearchBtn.ShadowDecoration.CustomizableEdges = customizableEdges3;
            SearchBtn.Size = new Size(35, 27);
            SearchBtn.TabIndex = 2;
            // 
            // DriverListDataGrid
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            DriverListDataGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(41, 75, 255);
            dataGridViewCellStyle2.SelectionBackColor = Color.White;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            DriverListDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            DriverListDataGrid.ColumnHeadersHeight = 30;
            DriverListDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(91, 91, 91);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            DriverListDataGrid.DefaultCellStyle = dataGridViewCellStyle3;
            DriverListDataGrid.GridColor = Color.White;
            DriverListDataGrid.Location = new Point(27, 61);
            DriverListDataGrid.Name = "DriverListDataGrid";
            DriverListDataGrid.RowHeadersVisible = false;
            DriverListDataGrid.RowHeadersWidth = 40;
            DriverListDataGrid.RowTemplate.Height = 25;
            DriverListDataGrid.Size = new Size(749, 291);
            DriverListDataGrid.TabIndex = 0;
            DriverListDataGrid.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            DriverListDataGrid.ThemeStyle.AlternatingRowsStyle.Font = null;
            DriverListDataGrid.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            DriverListDataGrid.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            DriverListDataGrid.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            DriverListDataGrid.ThemeStyle.BackColor = Color.White;
            DriverListDataGrid.ThemeStyle.GridColor = Color.White;
            DriverListDataGrid.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            DriverListDataGrid.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            DriverListDataGrid.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 8F);
            DriverListDataGrid.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            DriverListDataGrid.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            DriverListDataGrid.ThemeStyle.HeaderStyle.Height = 30;
            DriverListDataGrid.ThemeStyle.ReadOnly = false;
            DriverListDataGrid.ThemeStyle.RowsStyle.BackColor = Color.White;
            DriverListDataGrid.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            DriverListDataGrid.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            DriverListDataGrid.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(64, 64, 64);
            DriverListDataGrid.ThemeStyle.RowsStyle.Height = 25;
            DriverListDataGrid.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            DriverListDataGrid.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            DriverListDataGrid.CellFormatting += DgvQueue_CellFormatting;
            // 
            // UpdateStatusBtn
            // 
            UpdateStatusBtn.BorderRadius = 15;
            UpdateStatusBtn.CustomizableEdges = customizableEdges6;
            UpdateStatusBtn.DisabledState.BorderColor = Color.DarkGray;
            UpdateStatusBtn.DisabledState.CustomBorderColor = Color.DarkGray;
            UpdateStatusBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            UpdateStatusBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            UpdateStatusBtn.FillColor = Color.FromArgb(55, 91, 231);
            UpdateStatusBtn.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            UpdateStatusBtn.ForeColor = Color.White;
            UpdateStatusBtn.Location = new Point(624, 374);
            UpdateStatusBtn.Name = "UpdateStatusBtn";
            UpdateStatusBtn.ShadowDecoration.CustomizableEdges = customizableEdges7;
            UpdateStatusBtn.Size = new Size(152, 42);
            UpdateStatusBtn.TabIndex = 3;
            UpdateStatusBtn.Text = "Update Status";
            UpdateStatusBtn.Click += UpdateStatusBtn_Click_1;
            // 
            // ResetQueueBtn
            // 
            ResetQueueBtn.BorderRadius = 15;
            ResetQueueBtn.CustomizableEdges = customizableEdges8;
            ResetQueueBtn.DisabledState.BorderColor = Color.DarkGray;
            ResetQueueBtn.DisabledState.CustomBorderColor = Color.DarkGray;
            ResetQueueBtn.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            ResetQueueBtn.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            ResetQueueBtn.FillColor = Color.FromArgb(55, 91, 231);
            ResetQueueBtn.Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ResetQueueBtn.ForeColor = Color.White;
            ResetQueueBtn.Location = new Point(466, 374);
            ResetQueueBtn.Name = "ResetQueueBtn";
            ResetQueueBtn.ShadowDecoration.CustomizableEdges = customizableEdges9;
            ResetQueueBtn.Size = new Size(152, 42);
            ResetQueueBtn.TabIndex = 4;
            ResetQueueBtn.Text = "Reset Queue";
            ResetQueueBtn.Click += ResetQueueBtn_Click;
            // 
            // QueueModal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 450);
            Controls.Add(ResetQueueBtn);
            Controls.Add(UpdateStatusBtn);
            Controls.Add(DriverListDataGrid);
            Controls.Add(guna2Panel1);
            Controls.Add(SearchBar);
            ForeColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "QueueModal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Route";
            guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)DriverListDataGrid).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox SearchBar;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2ImageButton SearchBtn;
        private Guna.UI2.WinForms.Guna2DataGridView DriverListDataGrid;
        private Guna.UI2.WinForms.Guna2Button UpdateStatusBtn;
        private Guna.UI2.WinForms.Guna2Button ResetQueueBtn;
    }
}