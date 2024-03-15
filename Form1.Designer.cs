namespace ImageDistorsionUI
{
    partial class Form1
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
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            loadBtn = new Button();
            rstBtn = new Button();
            logBtn = new Button();
            correctBtn = new Button();
            saveBtn = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            flowLayoutPanel2 = new FlowLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            txtBoxWidthInput = new TextBox();
            txtBoxHeightInput = new TextBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.AutoSize = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(634, 354);
            panel1.TabIndex = 2;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(634, 354);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.MouseClick += pictureBox1_MouseClick;
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(loadBtn);
            flowLayoutPanel1.Controls.Add(rstBtn);
            flowLayoutPanel1.Controls.Add(txtBoxWidthInput);
            flowLayoutPanel1.Controls.Add(txtBoxHeightInput);
            flowLayoutPanel1.Controls.Add(correctBtn);
            flowLayoutPanel1.Controls.Add(saveBtn);
            flowLayoutPanel1.Controls.Add(logBtn);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(643, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(154, 354);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // loadBtn
            // 
            loadBtn.Location = new Point(3, 3);
            loadBtn.Name = "loadBtn";
            loadBtn.Size = new Size(151, 23);
            loadBtn.TabIndex = 0;
            loadBtn.Text = "Load image";
            loadBtn.UseVisualStyleBackColor = true;
            loadBtn.Click += loadBtn_Click;
            // 
            // rstBtn
            // 
            rstBtn.AutoSize = true;
            rstBtn.Location = new Point(3, 32);
            rstBtn.Name = "rstBtn";
            rstBtn.Size = new Size(151, 25);
            rstBtn.TabIndex = 1;
            rstBtn.Text = "Reset markers";
            rstBtn.UseVisualStyleBackColor = true;
            rstBtn.MouseClick += rstBtn_MouseClick;
            // 
            // logBtn
            // 
            logBtn.Location = new Point(3, 179);
            logBtn.Name = "logBtn";
            logBtn.Size = new Size(151, 23);
            logBtn.TabIndex = 2;
            logBtn.Text = "Log locations";
            logBtn.UseVisualStyleBackColor = true;
            logBtn.Click += logBtn_Click;
            // 
            // correctBtn
            // 
            correctBtn.Location = new Point(3, 121);
            correctBtn.Name = "correctBtn";
            correctBtn.Size = new Size(151, 23);
            correctBtn.TabIndex = 1;
            correctBtn.Text = "Correct distorsion";
            correctBtn.UseVisualStyleBackColor = true;
            correctBtn.Click += correctBtn_Click;
            // 
            // saveBtn
            // 
            saveBtn.Location = new Point(3, 150);
            saveBtn.Name = "saveBtn";
            saveBtn.Size = new Size(151, 23);
            saveBtn.TabIndex = 3;
            saveBtn.Text = "Save image";
            saveBtn.UseVisualStyleBackColor = true;
            saveBtn.Click += saveBtn_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoScroll = true;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 1, 0);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Controls.Add(label1);
            flowLayoutPanel2.Controls.Add(label2);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.Location = new Point(3, 363);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(634, 84);
            flowLayoutPanel2.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(47, 0);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 1;
            label2.Text = "label2";
            // 
            // txtBoxWidthInput
            // 
            txtBoxWidthInput.Location = new Point(3, 63);
            txtBoxWidthInput.Name = "txtBoxWidthInput";
            txtBoxWidthInput.Size = new Size(142, 23);
            txtBoxWidthInput.TabIndex = 4;
            txtBoxWidthInput.Text = "Target width";
            // 
            // txtBoxHeightInput
            // 
            txtBoxHeightInput.Location = new Point(3, 92);
            txtBoxHeightInput.Name = "txtBoxHeightInput";
            txtBoxHeightInput.Size = new Size(142, 23);
            txtBoxHeightInput.TabIndex = 5;
            txtBoxHeightInput.Text = "Target height";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private Panel panel1;
        private PictureBox pictureBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button loadBtn;
        private Button rstBtn;
        private Button logBtn;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label1;
        private Label label2;
        private Button correctBtn;
        private Button saveBtn;
        private TextBox txtBoxWidthInput;
        private TextBox txtBoxHeightInput;
    }
}
