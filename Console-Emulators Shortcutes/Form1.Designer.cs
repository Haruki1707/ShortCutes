namespace Console_Emulators_Shortcutes
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.Gdirbox = new System.Windows.Forms.TextBox();
            this.Edirbox = new System.Windows.Forms.TextBox();
            this.emulatorcb = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Shortcutbox = new System.Windows.Forms.TextBox();
            this.emuBrow = new System.Windows.Forms.Button();
            this.gameBrow = new System.Windows.Forms.Button();
            this.ICOpic = new System.Windows.Forms.PictureBox();
            this.DesktopCheck = new System.Windows.Forms.CheckBox();
            this.OpenShortFolderCheck = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ICOpic)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(27, 277);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 91);
            this.button1.TabIndex = 5;
            this.button1.TabStop = false;
            this.button1.Text = "Create ShortCute";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Gdirbox
            // 
            this.Gdirbox.Location = new System.Drawing.Point(27, 198);
            this.Gdirbox.Name = "Gdirbox";
            this.Gdirbox.ReadOnly = true;
            this.Gdirbox.Size = new System.Drawing.Size(254, 20);
            this.Gdirbox.TabIndex = 0;
            this.Gdirbox.TabStop = false;
            this.Gdirbox.Click += new System.EventHandler(this.Gdirbox_Click);
            // 
            // Edirbox
            // 
            this.Edirbox.Location = new System.Drawing.Point(27, 140);
            this.Edirbox.Name = "Edirbox";
            this.Edirbox.ReadOnly = true;
            this.Edirbox.Size = new System.Drawing.Size(253, 20);
            this.Edirbox.TabIndex = 0;
            this.Edirbox.TabStop = false;
            this.Edirbox.Click += new System.EventHandler(this.Edirbox_Click);
            // 
            // emulatorcb
            // 
            this.emulatorcb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emulatorcb.FormattingEnabled = true;
            this.emulatorcb.Items.AddRange(new object[] {
            "CEMU",
            "PJ64",
            "SNES9X"});
            this.emulatorcb.Location = new System.Drawing.Point(27, 42);
            this.emulatorcb.Name = "emulatorcb";
            this.emulatorcb.Size = new System.Drawing.Size(187, 21);
            this.emulatorcb.TabIndex = 0;
            this.emulatorcb.SelectedIndexChanged += new System.EventHandler(this.emulatorcb_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 115);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Emulator Directory:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 173);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Game File Directory:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Select an emulator:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Shortcut name:";
            // 
            // Shortcutbox
            // 
            this.Shortcutbox.Location = new System.Drawing.Point(27, 86);
            this.Shortcutbox.Name = "Shortcutbox";
            this.Shortcutbox.Size = new System.Drawing.Size(253, 20);
            this.Shortcutbox.TabIndex = 1;
            // 
            // emuBrow
            // 
            this.emuBrow.Location = new System.Drawing.Point(300, 132);
            this.emuBrow.Name = "emuBrow";
            this.emuBrow.Size = new System.Drawing.Size(107, 35);
            this.emuBrow.TabIndex = 3;
            this.emuBrow.Text = "Browse Dir";
            this.emuBrow.UseVisualStyleBackColor = true;
            this.emuBrow.Click += new System.EventHandler(this.emuBrow_Click);
            // 
            // gameBrow
            // 
            this.gameBrow.Location = new System.Drawing.Point(300, 190);
            this.gameBrow.Name = "gameBrow";
            this.gameBrow.Size = new System.Drawing.Size(107, 35);
            this.gameBrow.TabIndex = 4;
            this.gameBrow.Text = "Browse Game";
            this.gameBrow.UseVisualStyleBackColor = true;
            this.gameBrow.Click += new System.EventHandler(this.gameBrow_Click);
            // 
            // ICOpic
            // 
            this.ICOpic.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ICOpic.BackgroundImage")));
            this.ICOpic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ICOpic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ICOpic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ICOpic.Location = new System.Drawing.Point(431, 79);
            this.ICOpic.Name = "ICOpic";
            this.ICOpic.Size = new System.Drawing.Size(340, 340);
            this.ICOpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ICOpic.TabIndex = 11;
            this.ICOpic.TabStop = false;
            this.ICOpic.Click += new System.EventHandler(this.ICOpic_Click);
            // 
            // DesktopCheck
            // 
            this.DesktopCheck.AutoSize = true;
            this.DesktopCheck.Checked = true;
            this.DesktopCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DesktopCheck.Location = new System.Drawing.Point(300, 87);
            this.DesktopCheck.Name = "DesktopCheck";
            this.DesktopCheck.Size = new System.Drawing.Size(109, 17);
            this.DesktopCheck.TabIndex = 2;
            this.DesktopCheck.Text = "Desktop Shortcut";
            this.DesktopCheck.UseVisualStyleBackColor = true;
            // 
            // OpenShortFolderCheck
            // 
            this.OpenShortFolderCheck.AutoSize = true;
            this.OpenShortFolderCheck.Location = new System.Drawing.Point(231, 315);
            this.OpenShortFolderCheck.Name = "OpenShortFolderCheck";
            this.OpenShortFolderCheck.Size = new System.Drawing.Size(139, 17);
            this.OpenShortFolderCheck.TabIndex = 6;
            this.OpenShortFolderCheck.TabStop = false;
            this.OpenShortFolderCheck.Text = "Open ShortCutes Folder";
            this.OpenShortFolderCheck.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(475, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(254, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Select a PNG or JPG Image to set as ICON Shotcut:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShortCutes";
            ((System.ComponentModel.ISupportInitialize)(this.ICOpic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Gdirbox;
        private System.Windows.Forms.TextBox Edirbox;
        private System.Windows.Forms.ComboBox emulatorcb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Shortcutbox;
        private System.Windows.Forms.Button emuBrow;
        private System.Windows.Forms.Button gameBrow;
        private System.Windows.Forms.PictureBox ICOpic;
        private System.Windows.Forms.CheckBox DesktopCheck;
        private System.Windows.Forms.CheckBox OpenShortFolderCheck;
        private System.Windows.Forms.Label label5;
    }
}

