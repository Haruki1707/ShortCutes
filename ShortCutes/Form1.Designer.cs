﻿namespace ShortCutes
{
    partial class ShortCutes
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShortCutes));
            this.label5 = new System.Windows.Forms.Label();
            this.OpenShortFolderCheck = new System.Windows.Forms.CheckBox();
            this.DesktopCheck = new System.Windows.Forms.CheckBox();
            this.gameBrow = new System.Windows.Forms.Button();
            this.emuBrow = new System.Windows.Forms.Button();
            this.Shortcutbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.emulatorcb = new System.Windows.Forms.ComboBox();
            this.Edirbox = new System.Windows.Forms.TextBox();
            this.Gdirbox = new System.Windows.Forms.TextBox();
            this.createshortbtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panelBorderStyle = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.miniBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.ICOpic = new System.Windows.Forms.PictureBox();
            this.panelBorderStyle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ICOpic)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(495, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(263, 16);
            this.label5.TabIndex = 27;
            this.label5.Text = "Select a PNG or JPG Image to set as ICON Shotcut:";
            // 
            // OpenShortFolderCheck
            // 
            this.OpenShortFolderCheck.AutoSize = true;
            this.OpenShortFolderCheck.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OpenShortFolderCheck.ForeColor = System.Drawing.Color.White;
            this.OpenShortFolderCheck.Location = new System.Drawing.Point(37, 345);
            this.OpenShortFolderCheck.Name = "OpenShortFolderCheck";
            this.OpenShortFolderCheck.Size = new System.Drawing.Size(147, 20);
            this.OpenShortFolderCheck.TabIndex = 22;
            this.OpenShortFolderCheck.TabStop = false;
            this.OpenShortFolderCheck.Text = "Open ShortCutes Folder";
            this.OpenShortFolderCheck.UseVisualStyleBackColor = true;
            // 
            // DesktopCheck
            // 
            this.DesktopCheck.AutoSize = true;
            this.DesktopCheck.Checked = true;
            this.DesktopCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DesktopCheck.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DesktopCheck.ForeColor = System.Drawing.Color.White;
            this.DesktopCheck.Location = new System.Drawing.Point(333, 170);
            this.DesktopCheck.Name = "DesktopCheck";
            this.DesktopCheck.Size = new System.Drawing.Size(115, 20);
            this.DesktopCheck.TabIndex = 17;
            this.DesktopCheck.Text = "Desktop Shortcut";
            this.DesktopCheck.UseVisualStyleBackColor = true;
            // 
            // gameBrow
            // 
            this.gameBrow.FlatAppearance.BorderSize = 0;
            this.gameBrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gameBrow.Font = new System.Drawing.Font("Bahnschrift Condensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gameBrow.ForeColor = System.Drawing.Color.White;
            this.gameBrow.Location = new System.Drawing.Point(333, 295);
            this.gameBrow.Name = "gameBrow";
            this.gameBrow.Size = new System.Drawing.Size(119, 35);
            this.gameBrow.TabIndex = 19;
            this.gameBrow.Text = "Browse Game";
            this.gameBrow.UseVisualStyleBackColor = true;
            this.gameBrow.Click += new System.EventHandler(this.GameBrow_Click);
            // 
            // emuBrow
            // 
            this.emuBrow.FlatAppearance.BorderSize = 0;
            this.emuBrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emuBrow.Font = new System.Drawing.Font("Bahnschrift Condensed", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emuBrow.ForeColor = System.Drawing.Color.White;
            this.emuBrow.Location = new System.Drawing.Point(333, 226);
            this.emuBrow.Name = "emuBrow";
            this.emuBrow.Size = new System.Drawing.Size(119, 35);
            this.emuBrow.TabIndex = 18;
            this.emuBrow.Text = "Browse Dir";
            this.emuBrow.UseVisualStyleBackColor = true;
            this.emuBrow.Click += new System.EventHandler(this.EmuBrow_Click);
            // 
            // Shortcutbox
            // 
            this.Shortcutbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(57)))), ((int)(((byte)(77)))));
            this.Shortcutbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Shortcutbox.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 12F);
            this.Shortcutbox.ForeColor = System.Drawing.Color.White;
            this.Shortcutbox.Location = new System.Drawing.Point(37, 170);
            this.Shortcutbox.Name = "Shortcutbox";
            this.Shortcutbox.Size = new System.Drawing.Size(250, 20);
            this.Shortcutbox.TabIndex = 16;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(34, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Shortcut name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(37, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Select an emulator:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(37, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 16);
            this.label2.TabIndex = 23;
            this.label2.Text = "Game File Directory:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(37, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "Emulator Directory:";
            // 
            // emulatorcb
            // 
            this.emulatorcb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(57)))), ((int)(((byte)(77)))));
            this.emulatorcb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emulatorcb.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emulatorcb.ForeColor = System.Drawing.Color.White;
            this.emulatorcb.FormattingEnabled = true;
            this.emulatorcb.Location = new System.Drawing.Point(37, 113);
            this.emulatorcb.MaxDropDownItems = 10;
            this.emulatorcb.Name = "emulatorcb";
            this.emulatorcb.Size = new System.Drawing.Size(176, 26);
            this.emulatorcb.TabIndex = 13;
            this.emulatorcb.SelectedIndexChanged += new System.EventHandler(this.Emulatorcb_SelectedIndexChanged);
            // 
            // Edirbox
            // 
            this.Edirbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(57)))), ((int)(((byte)(77)))));
            this.Edirbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Edirbox.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 9F);
            this.Edirbox.ForeColor = System.Drawing.Color.White;
            this.Edirbox.Location = new System.Drawing.Point(37, 226);
            this.Edirbox.Multiline = true;
            this.Edirbox.Name = "Edirbox";
            this.Edirbox.ReadOnly = true;
            this.Edirbox.Size = new System.Drawing.Size(253, 35);
            this.Edirbox.TabIndex = 14;
            this.Edirbox.TabStop = false;
            this.Edirbox.Click += new System.EventHandler(this.EmuBrow_Click);
            this.Edirbox.Enter += new System.EventHandler(this.notfocus_Enter);
            // 
            // Gdirbox
            // 
            this.Gdirbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(57)))), ((int)(((byte)(77)))));
            this.Gdirbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Gdirbox.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 9F);
            this.Gdirbox.ForeColor = System.Drawing.Color.White;
            this.Gdirbox.Location = new System.Drawing.Point(37, 295);
            this.Gdirbox.Multiline = true;
            this.Gdirbox.Name = "Gdirbox";
            this.Gdirbox.ReadOnly = true;
            this.Gdirbox.Size = new System.Drawing.Size(254, 35);
            this.Gdirbox.TabIndex = 15;
            this.Gdirbox.TabStop = false;
            this.Gdirbox.Click += new System.EventHandler(this.GameBrow_Click);
            this.Gdirbox.Enter += new System.EventHandler(this.notfocus_Enter);
            // 
            // createshortbtn
            // 
            this.createshortbtn.FlatAppearance.BorderSize = 0;
            this.createshortbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createshortbtn.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createshortbtn.ForeColor = System.Drawing.Color.White;
            this.createshortbtn.Location = new System.Drawing.Point(37, 382);
            this.createshortbtn.Name = "createshortbtn";
            this.createshortbtn.Size = new System.Drawing.Size(404, 65);
            this.createshortbtn.TabIndex = 21;
            this.createshortbtn.TabStop = false;
            this.createshortbtn.Text = "Create ShortCute";
            this.createshortbtn.UseVisualStyleBackColor = true;
            this.createshortbtn.Click += new System.EventHandler(this.CreateShortCute_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Bahnschrift SemiLight SemiConde", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(258, 90);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(208, 61);
            this.label6.TabIndex = 28;
            this.label6.Text = "(You can select emulator directory directly, if its supported it will be selected" +
    " automatically)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelBorderStyle
            // 
            this.panelBorderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(17)))), ((int)(((byte)(23)))));
            this.panelBorderStyle.Controls.Add(this.label7);
            this.panelBorderStyle.Controls.Add(this.miniBtn);
            this.panelBorderStyle.Controls.Add(this.closeBtn);
            this.panelBorderStyle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBorderStyle.Location = new System.Drawing.Point(0, 0);
            this.panelBorderStyle.Name = "panelBorderStyle";
            this.panelBorderStyle.Size = new System.Drawing.Size(825, 42);
            this.panelBorderStyle.TabIndex = 29;
            this.panelBorderStyle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelBorderStyle_MouseDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(12, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(151, 31);
            this.label7.TabIndex = 2;
            this.label7.Text = "ShortCutes";
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelBorderStyle_MouseDown);
            // 
            // miniBtn
            // 
            this.miniBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(22)))), ((int)(((byte)(27)))));
            this.miniBtn.BackgroundImage = global::ShortCutes.Properties.Resources.minimizebtn;
            this.miniBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.miniBtn.FlatAppearance.BorderSize = 0;
            this.miniBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(41)))), ((int)(((byte)(51)))));
            this.miniBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.miniBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.miniBtn.Location = new System.Drawing.Point(749, 0);
            this.miniBtn.Name = "miniBtn";
            this.miniBtn.Size = new System.Drawing.Size(35, 35);
            this.miniBtn.TabIndex = 1;
            this.miniBtn.UseVisualStyleBackColor = false;
            this.miniBtn.Click += new System.EventHandler(this.miniBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(22)))), ((int)(((byte)(27)))));
            this.closeBtn.BackgroundImage = global::ShortCutes.Properties.Resources.closebtn;
            this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Location = new System.Drawing.Point(787, 0);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(35, 35);
            this.closeBtn.TabIndex = 0;
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // ICOpic
            // 
            this.ICOpic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ICOpic.BackgroundImage")));
            this.ICOpic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ICOpic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ICOpic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ICOpic.Location = new System.Drawing.Point(484, 150);
            this.ICOpic.Name = "ICOpic";
            this.ICOpic.Size = new System.Drawing.Size(300, 300);
            this.ICOpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ICOpic.TabIndex = 26;
            this.ICOpic.TabStop = false;
            this.ICOpic.Click += new System.EventHandler(this.ICOpic_Click);
            // 
            // ShortCutes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(825, 475);
            this.Controls.Add(this.panelBorderStyle);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.OpenShortFolderCheck);
            this.Controls.Add(this.DesktopCheck);
            this.Controls.Add(this.ICOpic);
            this.Controls.Add(this.gameBrow);
            this.Controls.Add(this.emuBrow);
            this.Controls.Add(this.Shortcutbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.emulatorcb);
            this.Controls.Add(this.Edirbox);
            this.Controls.Add(this.Gdirbox);
            this.Controls.Add(this.createshortbtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ShortCutes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShortCutes";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ShortCutes_Paint);
            this.panelBorderStyle.ResumeLayout(false);
            this.panelBorderStyle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ICOpic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox OpenShortFolderCheck;
        private System.Windows.Forms.CheckBox DesktopCheck;
        private System.Windows.Forms.PictureBox ICOpic;
        private System.Windows.Forms.Button gameBrow;
        private System.Windows.Forms.Button emuBrow;
        private System.Windows.Forms.TextBox Shortcutbox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox emulatorcb;
        private System.Windows.Forms.TextBox Edirbox;
        private System.Windows.Forms.TextBox Gdirbox;
        private System.Windows.Forms.Button createshortbtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panelBorderStyle;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button miniBtn;
        private System.Windows.Forms.Label label7;
    }
}

