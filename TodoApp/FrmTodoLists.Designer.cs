namespace TodoApp
{
    partial class FrmTodoLists
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
            cboStatus = new ComboBox();
            dgvLists = new DataGridView();
            Done = new DataGridViewCheckBoxColumn();
            TaskID = new DataGridViewTextBoxColumn();
            TaskName = new DataGridViewTextBoxColumn();
            DueDate = new DataGridViewTextBoxColumn();
            Priority = new DataGridViewTextBoxColumn();
            txtSearch = new TextBox();
            btnBack = new Button();
            btnAdd = new Button();
            btnDone = new Button();
            btnDelete = new Button();
            lblWelcome = new Label();
            btnMyAnalytics = new Button();
            btnExcel = new Button();
            btnPdf = new Button();
            lblExport = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvLists).BeginInit();
            SuspendLayout();
            // 
            // cboStatus
            // 
            cboStatus.AutoCompleteCustomSource.AddRange(new string[] { "All", "Done", "Uncomplete" });
            cboStatus.AutoCompleteMode = AutoCompleteMode.Suggest;
            cboStatus.AutoCompleteSource = AutoCompleteSource.ListItems;
            cboStatus.FormattingEnabled = true;
            cboStatus.Items.AddRange(new object[] { "All", "Done", "Uncomplete" });
            cboStatus.Location = new Point(472, 118);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(168, 28);
            cboStatus.TabIndex = 10;
            // 
            // dgvLists
            // 
            dgvLists.AllowUserToAddRows = false;
            dgvLists.AllowUserToDeleteRows = false;
            dgvLists.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvLists.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLists.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLists.Columns.AddRange(new DataGridViewColumn[] { Done, TaskID, TaskName, DueDate, Priority });
            dgvLists.Location = new Point(12, 160);
            dgvLists.Name = "dgvLists";
            dgvLists.RowHeadersVisible = false;
            dgvLists.RowHeadersWidth = 51;
            dgvLists.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLists.Size = new Size(704, 341);
            dgvLists.TabIndex = 0;
            dgvLists.CellContentClick += dgvLists_CellContentClick;
            dgvLists.CellDoubleClick += dgvLists_CellDoubleClick;
            dgvLists.SelectionChanged += dgvList_SelectionChanged;
            // 
            // Done
            // 
            Done.FlatStyle = FlatStyle.Flat;
            Done.HeaderText = "Done";
            Done.MinimumWidth = 6;
            Done.Name = "Done";
            Done.Resizable = DataGridViewTriState.True;
            Done.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // TaskID
            // 
            TaskID.HeaderText = "Task ID";
            TaskID.MinimumWidth = 6;
            TaskID.Name = "TaskID";
            // 
            // TaskName
            // 
            TaskName.HeaderText = "Task Name";
            TaskName.MinimumWidth = 6;
            TaskName.Name = "TaskName";
            // 
            // DueDate
            // 
            DueDate.HeaderText = "Due Date";
            DueDate.MinimumWidth = 6;
            DueDate.Name = "DueDate";
            // 
            // Priority
            // 
            Priority.HeaderText = "Priority";
            Priority.MinimumWidth = 6;
            Priority.Name = "Priority";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(12, 69);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Search for a specific task";
            txtSearch.Size = new Size(439, 27);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // btnBack
            // 
            btnBack.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBack.BackColor = Color.Red;
            btnBack.Location = new Point(602, 64);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(114, 36);
            btnBack.TabIndex = 2;
            btnBack.Text = "<- Logout";
            btnBack.UseVisualStyleBackColor = false;
            btnBack.Click += btnBack_Click;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAdd.BackColor = Color.DarkSeaGreen;
            btnAdd.Location = new Point(41, 516);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(120, 45);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "Add Task";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnDone
            // 
            btnDone.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDone.BackColor = Color.YellowGreen;
            btnDone.Location = new Point(207, 516);
            btnDone.Name = "btnDone";
            btnDone.Size = new Size(120, 45);
            btnDone.TabIndex = 4;
            btnDone.Text = "Mark as Done";
            btnDone.UseVisualStyleBackColor = false;
            btnDone.Click += btnDone_Click;
            // 
            // btnDelete
            // 
            btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnDelete.BackColor = Color.Yellow;
            btnDelete.Location = new Point(383, 516);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(120, 45);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete Task";
            btnDelete.UseVisualStyleBackColor = false;
            btnDelete.Click += btnDelete_Click;
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("Verdana", 13.8F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblWelcome.Location = new Point(12, 18);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(0, 28);
            lblWelcome.TabIndex = 6;
            // 
            // btnMyAnalytics
            // 
            btnMyAnalytics.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnMyAnalytics.BackColor = Color.LightPink;
            btnMyAnalytics.Location = new Point(571, 516);
            btnMyAnalytics.Name = "btnMyAnalytics";
            btnMyAnalytics.Size = new Size(117, 45);
            btnMyAnalytics.TabIndex = 7;
            btnMyAnalytics.Text = "My Analytics";
            btnMyAnalytics.UseVisualStyleBackColor = false;
            btnMyAnalytics.Click += btnMyAnalytics_Click;
            // 
            // btnExcel
            // 
            btnExcel.BackColor = Color.LimeGreen;
            btnExcel.Location = new Point(9, 109);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(155, 45);
            btnExcel.TabIndex = 8;
            btnExcel.Text = "Export Data to Excel";
            btnExcel.UseVisualStyleBackColor = false;
            btnExcel.Click += btnExcel_Click;
            // 
            // btnPdf
            // 
            btnPdf.BackColor = Color.IndianRed;
            btnPdf.Location = new Point(188, 109);
            btnPdf.Name = "btnPdf";
            btnPdf.Size = new Size(155, 45);
            btnPdf.TabIndex = 9;
            btnPdf.Text = "Export Data to PDF";
            btnPdf.UseVisualStyleBackColor = false;
            btnPdf.Click += btnPdf_Click;
            // 
            // lblExport
            // 
            lblExport.AutoSize = true;
            lblExport.Location = new Point(360, 121);
            lblExport.Name = "lblExport";
            lblExport.Size = new Size(106, 20);
            lblExport.TabIndex = 11;
            lblExport.Text = "Data to Export";
            // 
            // FrmTodoLists
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.PeachPuff;
            ClientSize = new Size(726, 573);
            Controls.Add(lblExport);
            Controls.Add(cboStatus);
            Controls.Add(btnPdf);
            Controls.Add(btnExcel);
            Controls.Add(btnMyAnalytics);
            Controls.Add(lblWelcome);
            Controls.Add(btnDelete);
            Controls.Add(btnDone);
            Controls.Add(btnAdd);
            Controls.Add(btnBack);
            Controls.Add(txtSearch);
            Controls.Add(dgvLists);
            Name = "FrmTodoLists";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Todo Lists";
            Load += FrmTodoLists_Load;
            ((System.ComponentModel.ISupportInitialize)dgvLists).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvLists;
        private TextBox txtSearch;
        private Button btnBack;
        private Button btnAdd;
        private Button btnDone;
        private Button btnDelete;
        private Label lblWelcome;
        private DataGridViewCheckBoxColumn Done;
        private DataGridViewTextBoxColumn TaskID;
        private DataGridViewTextBoxColumn TaskName;
        private DataGridViewTextBoxColumn DueDate;
        private DataGridViewTextBoxColumn Priority;
        private Button btnMyAnalytics;
        private Button btnExcel;
        private Button btnPdf;
        private ComboBox cboStatus;
        private Label lblExport;
    }
}