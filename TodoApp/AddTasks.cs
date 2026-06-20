using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TodoApp
{
    public partial class AddTasks : Form
    {
        public AddTasks()
        {
            InitializeComponent();
        }

        private int _taskID = 0;

        public AddTasks(int taskID)
        {
            InitializeComponent();

            _taskID = taskID;
        }

        private void AddTasks_Load(object sender, EventArgs e)
        {
            cmbPriority.SelectedIndex = 0;

            dtpDueDate.MinDate = DateTime.Today;

            if (_taskID > 0)
            {
                lblTitle.Text = "Edit Task";
                this.Text = "Edit Task";
                btnSave.Text = "Update";

                LoadTaskDetails();
            }
            else
            {
                this.Text = "Add Task";
                btnSave.Text = "Save";
            }
        }

        private bool Validation()
        {
            if (string.IsNullOrWhiteSpace(txtTaskName.Text))
            {
                MessageBox.Show("Task name is required.");
                txtTaskName.Focus();
                return false;
            }

            if (cmbPriority.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a priority.");
                cmbPriority.Focus();
                return false;
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!Validation())
            {
                return;
            }

            if (_taskID == 0)
            {
                AddTask();
            }
            else
            {
                UpdateTask();
            }

        }

        private void AddTask()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"INSERT INTO todolists
                           (usersID, TaskName, DueDate, Priority, Done)
                           VALUES
                           (@usersID, @TaskName, @DueDate, @Priority, 0)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);
                        cmd.Parameters.AddWithValue("@TaskName", txtTaskName.Text.Trim());
                        cmd.Parameters.AddWithValue("@DueDate", dtpDueDate.Value.Date);
                        cmd.Parameters.AddWithValue("@Priority", cmbPriority.Text);

                        cmd.ExecuteNonQuery();

                        conn.Close();
                    }
                }

                MessageBox.Show("Task added successfully.", "Task Added Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving task:\n" + ex.Message);
            }
        }

        private void UpdateTask()
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string sql = @"UPDATE todolists
                       SET TaskName = @TaskName,
                           DueDate = @DueDate,
                           Priority = @Priority
                       WHERE taskID = @taskID
                       AND usersID = @usersID";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@TaskName", txtTaskName.Text.Trim());
                    cmd.Parameters.AddWithValue("@DueDate", dtpDueDate.Value.Date);
                    cmd.Parameters.AddWithValue("@Priority", cmbPriority.Text);

                    cmd.Parameters.AddWithValue("@taskID", _taskID);
                    cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }

            MessageBox.Show("Task updated successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void LoadTaskDetails()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT TaskName,
                                  DueDate,
                                  Priority
                           FROM todolists
                           WHERE taskID = @taskID
                           AND usersID = @usersID";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskID", _taskID);
                        cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtTaskName.Text = reader["TaskName"].ToString();

                                dtpDueDate.Value =
                                    Convert.ToDateTime(reader["DueDate"]);

                                cmbPriority.Text =
                                    reader["Priority"].ToString();

                                conn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Task cannot be edited if it is past the current date", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
