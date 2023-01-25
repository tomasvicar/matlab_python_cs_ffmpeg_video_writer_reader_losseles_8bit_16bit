
namespace test_ffmpeg_16bit
{
    partial class Form1
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
            this.button_save = new System.Windows.Forms.Button();
            this.button_load = new System.Windows.Forms.Button();
            this.button_show = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.textBox_pathSave = new System.Windows.Forms.TextBox();
            this.textBox_pathLoad = new System.Windows.Forms.TextBox();
            this.numericUpDown_frameNum = new System.Windows.Forms.NumericUpDown();
            this.button_writeTiff = new System.Windows.Forms.Button();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_frameNum)).BeginInit();
            this.SuspendLayout();
            // 
            // button_save
            // 
            this.button_save.Location = new System.Drawing.Point(115, 634);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(75, 23);
            this.button_save.TabIndex = 0;
            this.button_save.Text = "save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_load
            // 
            this.button_load.Location = new System.Drawing.Point(1256, 605);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(75, 23);
            this.button_load.TabIndex = 1;
            this.button_load.Text = "load";
            this.button_load.UseVisualStyleBackColor = true;
            this.button_load.Click += new System.EventHandler(this.button_load_Click);
            // 
            // button_show
            // 
            this.button_show.Location = new System.Drawing.Point(12, 44);
            this.button_show.Name = "button_show";
            this.button_show.Size = new System.Drawing.Size(75, 23);
            this.button_show.TabIndex = 2;
            this.button_show.Text = "show";
            this.button_show.UseVisualStyleBackColor = true;
            this.button_show.Click += new System.EventHandler(this.button_show_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(379, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1054, 470);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // textBox_pathSave
            // 
            this.textBox_pathSave.Location = new System.Drawing.Point(93, 607);
            this.textBox_pathSave.Name = "textBox_pathSave";
            this.textBox_pathSave.Size = new System.Drawing.Size(982, 20);
            this.textBox_pathSave.TabIndex = 4;
            this.textBox_pathSave.Text = "..\\..\\..\\..\\retina_gray8_cs.avi";
            this.textBox_pathSave.TextChanged += new System.EventHandler(this.textBox_pathSave_TextChanged);
            // 
            // textBox_pathLoad
            // 
            this.textBox_pathLoad.Location = new System.Drawing.Point(392, 636);
            this.textBox_pathLoad.Name = "textBox_pathLoad";
            this.textBox_pathLoad.Size = new System.Drawing.Size(1013, 20);
            this.textBox_pathLoad.TabIndex = 5;
            this.textBox_pathLoad.Text = "..\\..\\..\\..\\retina_gray8.avi";
            // 
            // numericUpDown_frameNum
            // 
            this.numericUpDown_frameNum.Location = new System.Drawing.Point(12, 94);
            this.numericUpDown_frameNum.Name = "numericUpDown_frameNum";
            this.numericUpDown_frameNum.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown_frameNum.TabIndex = 6;
            this.numericUpDown_frameNum.ValueChanged += new System.EventHandler(this.numericUpDown_frameNum_ValueChanged);
            // 
            // button_writeTiff
            // 
            this.button_writeTiff.Location = new System.Drawing.Point(13, 140);
            this.button_writeTiff.Name = "button_writeTiff";
            this.button_writeTiff.Size = new System.Drawing.Size(75, 23);
            this.button_writeTiff.TabIndex = 7;
            this.button_writeTiff.Text = "write tiff";
            this.button_writeTiff.UseVisualStyleBackColor = true;
            this.button_writeTiff.Click += new System.EventHandler(this.button_writeTiff_Click);
            // 
            // comboBox_type
            // 
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Items.AddRange(new object[] {
            "rgb24",
            "gray16",
            "gray8"});
            this.comboBox_type.Location = new System.Drawing.Point(379, 524);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(121, 21);
            this.comboBox_type.TabIndex = 8;
            this.comboBox_type.Text = "gray8";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1445, 669);
            this.Controls.Add(this.comboBox_type);
            this.Controls.Add(this.button_writeTiff);
            this.Controls.Add(this.numericUpDown_frameNum);
            this.Controls.Add(this.textBox_pathLoad);
            this.Controls.Add(this.textBox_pathSave);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_show);
            this.Controls.Add(this.button_load);
            this.Controls.Add(this.button_save);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_frameNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.Button button_show;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox textBox_pathSave;
        private System.Windows.Forms.TextBox textBox_pathLoad;
        private System.Windows.Forms.NumericUpDown numericUpDown_frameNum;
        private System.Windows.Forms.Button button_writeTiff;
        private System.Windows.Forms.ComboBox comboBox_type;
    }
}

