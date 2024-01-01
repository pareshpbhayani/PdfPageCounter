namespace PdfPageCounter
{
    partial class FrmHome
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmHome));
            btnBrowse = new Button();
            txtFolderPath = new TextBox();
            txtTotalCount = new TextBox();
            progressBar1 = new ProgressBar();
            lblProcessStatus = new Label();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnBrowse
            // 
            btnBrowse.Location = new Point(213, 45);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 37);
            btnBrowse.TabIndex = 0;
            btnBrowse.Text = "Browse";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // txtFolderPath
            // 
            txtFolderPath.Enabled = false;
            txtFolderPath.Location = new Point(12, 14);
            txtFolderPath.Name = "txtFolderPath";
            txtFolderPath.Size = new Size(476, 25);
            txtFolderPath.TabIndex = 1;
            // 
            // txtTotalCount
            // 
            txtTotalCount.Enabled = false;
            txtTotalCount.Location = new Point(149, 88);
            txtTotalCount.Name = "txtTotalCount";
            txtTotalCount.Size = new Size(203, 25);
            txtTotalCount.TabIndex = 1;
            txtTotalCount.TextAlign = HorizontalAlignment.Center;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 121);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(476, 23);
            progressBar1.TabIndex = 2;
            // 
            // lblProcessStatus
            // 
            lblProcessStatus.AutoSize = true;
            lblProcessStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            lblProcessStatus.ForeColor = Color.Green;
            lblProcessStatus.Location = new Point(180, 148);
            lblProcessStatus.Name = "lblProcessStatus";
            lblProcessStatus.Size = new Size(141, 19);
            lblProcessStatus.TabIndex = 3;
            lblProcessStatus.Text = "Process completed!";
            lblProcessStatus.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Maroon;
            label1.Location = new Point(358, 91);
            label1.Name = "label1";
            label1.Size = new Size(123, 19);
            label1.TabIndex = 3;
            label1.Text = "Counts ony PDFs.";
            label1.Visible = false;
            // 
            // FrmHome
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 171);
            Controls.Add(label1);
            Controls.Add(lblProcessStatus);
            Controls.Add(progressBar1);
            Controls.Add(txtTotalCount);
            Controls.Add(txtFolderPath);
            Controls.Add(btnBrowse);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmHome";
            Text = "PDF Page Counter";
            Load += FrmHome_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBrowse;
        private TextBox txtFolderPath;
        private TextBox txtTotalCount;
        private ProgressBar progressBar1;
        private Label lblProcessStatus;
        private Label label1;
    }
}