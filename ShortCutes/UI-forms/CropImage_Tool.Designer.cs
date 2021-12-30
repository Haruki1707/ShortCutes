
namespace ShortCutes
{
    partial class CropImage_Tool
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.OriginalImage = new System.Windows.Forms.Button();
            this.CropBTN = new System.Windows.Forms.Button();
            this.SaveBTN = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.SaveOriBTN = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(73, 66);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 400);
            this.panel1.TabIndex = 4;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // OriginalImage
            // 
            this.OriginalImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.OriginalImage.FlatAppearance.BorderSize = 0;
            this.OriginalImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(116)))), ((int)(((byte)(128)))));
            this.OriginalImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OriginalImage.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold);
            this.OriginalImage.ForeColor = System.Drawing.Color.White;
            this.OriginalImage.Location = new System.Drawing.Point(410, 105);
            this.OriginalImage.Name = "OriginalImage";
            this.OriginalImage.Size = new System.Drawing.Size(119, 35);
            this.OriginalImage.TabIndex = 5;
            this.OriginalImage.Text = "Original Image";
            this.OriginalImage.UseVisualStyleBackColor = false;
            this.OriginalImage.Click += new System.EventHandler(this.OriginalImage_Click);
            // 
            // CropBTN
            // 
            this.CropBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.CropBTN.FlatAppearance.BorderSize = 0;
            this.CropBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(116)))), ((int)(((byte)(128)))));
            this.CropBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CropBTN.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold);
            this.CropBTN.ForeColor = System.Drawing.Color.White;
            this.CropBTN.Location = new System.Drawing.Point(410, 190);
            this.CropBTN.Name = "CropBTN";
            this.CropBTN.Size = new System.Drawing.Size(119, 35);
            this.CropBTN.TabIndex = 6;
            this.CropBTN.Text = "Crop";
            this.CropBTN.UseVisualStyleBackColor = false;
            this.CropBTN.Click += new System.EventHandler(this.CropBTN_Click);
            // 
            // SaveBTN
            // 
            this.SaveBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.SaveBTN.Enabled = false;
            this.SaveBTN.FlatAppearance.BorderSize = 0;
            this.SaveBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(116)))), ((int)(((byte)(128)))));
            this.SaveBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveBTN.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold);
            this.SaveBTN.ForeColor = System.Drawing.Color.White;
            this.SaveBTN.Location = new System.Drawing.Point(410, 275);
            this.SaveBTN.Name = "SaveBTN";
            this.SaveBTN.Size = new System.Drawing.Size(119, 35);
            this.SaveBTN.TabIndex = 7;
            this.SaveBTN.Text = "Save Croped Image";
            this.SaveBTN.UseVisualStyleBackColor = false;
            this.SaveBTN.Click += new System.EventHandler(this.SaveBTN_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(22)))), ((int)(((byte)(27)))));
            this.closeBtn.BackgroundImage = global::ShortCutes.Properties.Resources.closebtn;
            this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Location = new System.Drawing.Point(495, 7);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(35, 35);
            this.closeBtn.TabIndex = 8;
            this.closeBtn.TabStop = false;
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // SaveOriBTN
            // 
            this.SaveOriBTN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.SaveOriBTN.FlatAppearance.BorderSize = 0;
            this.SaveOriBTN.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(116)))), ((int)(((byte)(128)))));
            this.SaveOriBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveOriBTN.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold);
            this.SaveOriBTN.ForeColor = System.Drawing.Color.White;
            this.SaveOriBTN.Location = new System.Drawing.Point(410, 360);
            this.SaveOriBTN.Name = "SaveOriBTN";
            this.SaveOriBTN.Size = new System.Drawing.Size(119, 35);
            this.SaveOriBTN.TabIndex = 9;
            this.SaveOriBTN.Text = "Save Original Image";
            this.SaveOriBTN.UseVisualStyleBackColor = false;
            this.SaveOriBTN.Click += new System.EventHandler(this.SaveOriBTN_Click);
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(413, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 47);
            this.label5.TabIndex = 28;
            this.label5.Text = "Recommended crop from top to bottom";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CropImage_Tool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.ClientSize = new System.Drawing.Size(535, 415);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SaveOriBTN);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.SaveBTN);
            this.Controls.Add(this.CropBTN);
            this.Controls.Add(this.OriginalImage);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "CropImage_Tool";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Crop_Image";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button OriginalImage;
        private System.Windows.Forms.Button CropBTN;
        private System.Windows.Forms.Button SaveBTN;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button SaveOriBTN;
        private System.Windows.Forms.Label label5;
    }
}