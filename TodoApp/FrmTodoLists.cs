using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Pqc.Crypto.Falcon;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using TodoApp;

namespace TodoApp
{
    public partial class FrmTodoLists : Form
    {

        public FrmTodoLists()
        {
            InitializeComponent();
            dgvListView();
        }

        private int _selectedTaskID = 0;

        private void FrmTodoLists_Load(object sender, EventArgs e)
        {
            lblWelcome.Text = "Welcome, " + UserSession.FullName;

            debounce = new System.Windows.Forms.Timer();
            debounce.Interval = 500; //by ms
            debounce.Tick += debounceTimer_Tick;

            LoadTask();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            DialogResult logout = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (logout == DialogResult.Yes)
            {
                frmLogin frm = new frmLogin();
                frm.Show();
                this.Close();
            }
        }

        public void LoadTask()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string data = @"SELECT taskID, Done, TaskName, DueDate, Priority
                                FROM todolists
                                WHERE usersID = @usersID
                                ORDER BY DueDate ASC";

                    using (var cmd = new MySqlCommand(data, conn))
                    {
                        cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);

                        using (var da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            dgvLists.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Tasks:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLists.CurrentRow == null) return;

            var idCell = dgvLists.CurrentRow.Cells["TaskID"].Value;

            if (idCell != null && int.TryParse(idCell.ToString(), out int id))
            {
                _selectedTaskID = id;
            }
        }

        private void dgvListView()
        {
            dgvLists.AutoGenerateColumns = false;
            dgvLists.Columns["Done"].DataPropertyName = "Done";
            dgvLists.Columns["TaskID"].DataPropertyName = "taskID";
            dgvLists.Columns["TaskName"].DataPropertyName = "TaskName";
            dgvLists.Columns["DueDate"].DataPropertyName = "DueDate";
            dgvLists.Columns["Priority"].DataPropertyName = "Priority";
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (_selectedTaskID == 0)
            {
                MessageBox.Show("Please select a task first.",
                                "No Selection",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            int taskID = Convert.ToInt32(dgvLists.CurrentRow.Cells["TaskID"].Value);

            ToggleTask(taskID);
        }

        private void dgvLists_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvLists.Columns["Done"].Index &&
                e.RowIndex >= 0)
            {
                int taskID = Convert.ToInt32(
                    dgvLists.Rows[e.RowIndex].Cells["TaskID"].Value);

                ToggleTask(taskID);
            }
        }

        private void ToggleTask(int taskID)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"UPDATE todolists
                           SET Done = NOT Done
                           WHERE taskID = @taskID
                           AND usersID = @usersID";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskID", taskID);
                        cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);

                        cmd.ExecuteNonQuery();
                    }
                }

                LoadTask();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating task:\n" + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private System.Windows.Forms.Timer debounce;

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounce.Stop();
            debounce.Start();
        }

        private void debounceTimer_Tick(object? sender, EventArgs e)
        {
            debounce.Stop();

            string search = txtSearch.Text.Trim();

            //same logic with the search button but now it will be automatically will be triggered
            if (string.IsNullOrEmpty(search))
            {
                LoadTask();
            }
            else
            {
                Search(search);
            }
        }

        private void Search(string search)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT taskID, Done, TaskName, DueDate, Priority
                                   FROM todolists
                                   WHERE usersID = @usersID
                                   AND (
                                        TaskName LIKE @search
                                        OR Priority LIKE @search
                                        OR DATE_FORMAT(DueDate, '%Y-%m-%d') LIKE @search
                                   )
                                   ORDER BY DueDate ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);
                        cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                        using (var ad = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            ad.Fill(dt);

                            dgvLists.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error Searching Tasks:\n{ex.Message}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvLists_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            int taskID = Convert.ToInt32(dgvLists.CurrentRow.Cells["TaskID"].Value);

            using (AddTasks frm = new AddTasks(taskID))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadTask();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvLists.CurrentRow == null)
            {
                MessageBox.Show("Please select a task first.");
                return;
            }

            int taskID = Convert.ToInt32(
                dgvLists.CurrentRow.Cells["TaskID"].Value);

            DeleteTask(taskID);
        }

        private void DeleteTask(int taskID)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this task?",
                "Confirm Delete",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"DELETE FROM todolists
                           WHERE taskID = @taskID
                           AND usersID = @usersID";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@taskID", taskID);
                        cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Task deleted successfully.",
                                            "Deleted",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Information);

                            LoadTask();

                            // Reset selected task
                            _selectedTaskID = 0;
                        }
                        else
                        {
                            MessageBox.Show("Task not found or you don't have permission to delete it.",
                                            "Delete Failed",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting task:\n" + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (AddTasks frm = new AddTasks())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadTask();
                }
            }
        }

        private void btnMyAnalytics_Click(object sender, EventArgs e)
        {
            MyAnalytics analyticsForm = new MyAnalytics();
            analyticsForm.ShowDialog(this);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            string statusFilter = cboStatus.SelectedItem?.ToString() ?? "All";

            DataTable dt = new DataTable();

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT taskID, Done, TaskName, DueDate, Priority
                            FROM todolists
                            WHERE usersID = @usersID";

                    if (statusFilter == "Done")
                        sql += " AND Done = 1";
                    else if (statusFilter == "Uncomplete")
                        sql += " AND Done = 0";

                    sql += " ORDER BY DueDate ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);

                        using (var da = new MySqlDataAdapter(cmd))

                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks for export:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("There are no records to export for the selected filter.",
                    "Export to Excel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "Excel Workbook (*.xlsx)|*.xlsx";
                saveDialog.FileName = $"TaskList_{statusFilter}.xlsx";

                if (saveDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Tasks");

                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            worksheet.Cell(1, col + 1).Value = dt.Columns[col].ColumnName;
                            worksheet.Cell(1, col + 1).Style.Font.Bold = true;
                        }

                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            for (int col = 0; col < dt.Columns.Count; col++)
                            {
                                object value = dt.Rows[row][col];

                                if (dt.Columns[col].ColumnName == "Done")
                                {
                                    bool isDone = Convert.ToInt32(value) == 1;

                                    var cell = worksheet.Cell(row + 2, col + 1);
                                    cell.Value = isDone ? "✓" : "✗";

                                    cell.Style.Font.FontColor = isDone
                                        ? XLColor.Green
                                        : XLColor.Red;

                                    cell.Style.Font.Bold = true;
                                }
                                else
                                {
                                    worksheet.Cell(row + 2, col + 1).Value = value?.ToString() ?? "";
                                }
                            }
                        }

                        worksheet.Columns().AdjustToContents();

                        workbook.SaveAs(saveDialog.FileName);
                    }

                    MessageBox.Show("Task/s list exported successfully to Excel.",
                        "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting to Excel:\n{ex.Message}",
                        "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            string statusFilter = cboStatus.SelectedItem?.ToString() ?? "All";

            DataTable dt = new DataTable();

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    string sql = @"SELECT taskID, Done, TaskName, DueDate, Priority
                           FROM todolists
                           WHERE usersID = @usersID";

                    if (statusFilter == "Done")
                        sql += " AND Done = 1";
                    else if (statusFilter == "Uncomplete")
                        sql += " AND Done = 0";

                    sql += " ORDER BY DueDate ASC";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@usersID", UserSession.UserID);

                        using (var da = new MySqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks for export:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("There are no records to export for the selected filter.",
                    "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Document (*.pdf)|*.pdf";
                saveDialog.FileName = $"TaskList_{statusFilter}.pdf";

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (PdfDocument document = new PdfDocument())
                    {
                        PdfPage page = document.AddPage();
                        page.Orientation = PdfSharpCore.PageOrientation.Landscape;

                        XGraphics gfx = XGraphics.FromPdfPage(page);

                        XFont titleFont = new XFont("Arial", 16, XFontStyle.Bold);
                        XFont headerFont = new XFont("Arial", 10, XFontStyle.Bold);
                        XFont cellFont = new XFont("Arial", 9, XFontStyle.Regular);

                        double margin = 30;
                        double rowHeight = 22;

                        int columnCount = dt.Columns.Count;
                        double tableWidth = page.Width - (margin * 2);
                        double colWidth = tableWidth / columnCount;

                        double y = 30;

                        void DrawHeader()
                        {
                            gfx.DrawString(
                                $"Task List Report ({statusFilter})",
                                titleFont,
                                XBrushes.Black,
                                new XRect(0, 10, page.Width, 20),
                                XStringFormats.TopCenter);

                            double x = margin;

                            for (int col = 0; col < columnCount; col++)
                            {
                                gfx.DrawRectangle(XPens.Black, x, y, colWidth, rowHeight);

                                gfx.DrawString(
                                    dt.Columns[col].ColumnName,
                                    headerFont,
                                    XBrushes.Black,
                                    new XRect(x + 2, y + 3, colWidth, rowHeight),
                                    XStringFormats.Center);

                                x += colWidth;
                            }
                        }

                        DrawHeader();
                        y += rowHeight;

                        foreach (DataRow row in dt.Rows)
                        {
                            if (y + rowHeight > page.Height - margin)
                            {
                                page = document.AddPage();
                                page.Orientation = PdfSharpCore.PageOrientation.Landscape;

                                gfx.Dispose();
                                gfx = XGraphics.FromPdfPage(page);

                                y = 30;
                                DrawHeader();
                                y += rowHeight;
                            }

                            double x = margin;

                            for (int col = 0; col < columnCount; col++)
                            {
                                string text;

                                if (dt.Columns[col].ColumnName == "Done")
                                {
                                    bool isDone = Convert.ToInt32(row[col]) == 1;
                                    text = isDone ? "Done" : "Uncomplete";
                                }
                                else if (dt.Columns[col].ColumnName == "DueDate")
                                {
                                    text = Convert.ToDateTime(row[col])
                                        .ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    text = row[col]?.ToString() ?? "";
                                }

                                gfx.DrawRectangle(XPens.Black, x, y, colWidth, rowHeight);

                                gfx.DrawString(
                                    text,
                                    cellFont,
                                    XBrushes.Black,
                                    new XRect(x + 2, y + 3, colWidth, rowHeight),
                                    XStringFormats.Center);

                                x += colWidth;
                            }

                            y += rowHeight;
                        }

                        gfx.Dispose();
                        document.Save(saveDialog.FileName);
                    }

                    MessageBox.Show(
                        "Task list exported successfully to PDF.",
                        "Export Complete",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error exporting to PDF:\n{ex.Message}",
                        "Export Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}