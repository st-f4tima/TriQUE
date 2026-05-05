using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TriQue.Data.Repositories;
using TriQue.DTOs;

namespace TriQue
{
    public partial class EditUserInformationModal : Form
    {
        private readonly int _userID;
        private readonly UserRepository _repo = new();
        public EditUserInformationModal(int userID)
        {
            InitializeComponent();
            _userID = userID;
            LoadRoutes();
            cboRole.SelectedIndexChanged += CboRole_Changed;
            LoadData();
        }

        private void LoadRoutes()
        {
            cboRole.Items.Clear();
            cboRole.Items.AddRange(new[] { "Driver", "Admin" });

            cboAdminLevel.Items.Clear();
            cboAdminLevel.Items.AddRange(new[] { "SuperAdmin", "TodaOfficer", "Staff" });

            var routes = _repo.GetAllRoutes();
            cboAssignedRoute.DataSource = routes;
            cboAssignedRoute.DisplayMember = "RouteName";
            cboAssignedRoute.ValueMember = "RouteID";
        }

        private void LoadData()
        {
            var d = _repo.GetUserDetail(_userID);
            if (d == null) return;

            txtFullName.Text = d.FullName;
            txtPhoneNumber.Text = d.PhoneNumber;
            cboRole.SelectedIndex = d.RoleID == 2 ? 1 : 0;

            foreach (RouteItem item in cboAssignedRoute.Items)
                if (item.RouteID == d.RouteID)
                { cboAssignedRoute.SelectedItem = item; break; }

            CboRole_Changed(null, EventArgs.Empty);
        }

        private void CboRole_Changed(object? sender, EventArgs e)
        {
            bool isDriver = cboRole.SelectedIndex == 0;
            lblAssignedRoute.Visible = isDriver;
            cboAssignedRoute.Visible = isDriver;
            lblAdminLevel.Visible = !isDriver;
            cboAdminLevel.Visible = !isDriver;
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            string name = txtFullName.Text.Trim();
            string phone = txtPhoneNumber.Text.Trim();
            int roleID = cboRole.SelectedIndex == 0 ? 1 : 2;
            int routeID = cboAssignedRoute.SelectedItem is RouteItem ri ? ri.RouteID : 0;
            int levelID = cboAdminLevel.SelectedIndex + 1;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Please fill in all fields.", "Validation",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _repo.UpdateUser(_userID, name, phone, roleID, routeID);
                MessageBox.Show("User updated successfully.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Are you sure you want to delete this user?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes) return;

            try
            {
                _repo.DeleteUser(_userID);
                MessageBox.Show("User deleted.", "Deleted",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
