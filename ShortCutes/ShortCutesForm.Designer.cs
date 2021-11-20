namespace ShortCutes
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
            this.components = new System.ComponentModel.Container();
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
            this.HistoryBtn = new System.Windows.Forms.Button();
            this.InfoButton = new System.Windows.Forms.Button();
            this.ConfigBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.miniBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.ICOurl = new System.Windows.Forms.TextBox();
            this.ICOpic = new System.Windows.Forms.PictureBox();
            this.ClearSCSelected = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelBorderStyle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ICOpic)).BeginInit();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(443, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(303, 35);
            this.label5.TabIndex = 27;
            this.label5.Text = "Select a PNG or JPG image to set as the shortcut ICON:\r\nDouble click to crop sele" +
    "cted image";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OpenShortFolderCheck
            // 
            this.OpenShortFolderCheck.AutoSize = true;
            this.OpenShortFolderCheck.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F);
            this.OpenShortFolderCheck.ForeColor = System.Drawing.Color.White;
            this.OpenShortFolderCheck.Location = new System.Drawing.Point(147, 311);
            this.OpenShortFolderCheck.Name = "OpenShortFolderCheck";
            this.OpenShortFolderCheck.Size = new System.Drawing.Size(146, 20);
            this.OpenShortFolderCheck.TabIndex = 7;
            this.OpenShortFolderCheck.TabStop = false;
            this.OpenShortFolderCheck.Text = "Open ShortCutes Folder";
            this.OpenShortFolderCheck.UseVisualStyleBackColor = true;
            this.OpenShortFolderCheck.CheckedChanged += new System.EventHandler(this.Shortcutbox_Focus);
            // 
            // DesktopCheck
            // 
            this.DesktopCheck.AutoSize = true;
            this.DesktopCheck.Checked = true;
            this.DesktopCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DesktopCheck.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 9.75F);
            this.DesktopCheck.ForeColor = System.Drawing.Color.White;
            this.DesktopCheck.Location = new System.Drawing.Point(15, 311);
            this.DesktopCheck.Name = "DesktopCheck";
            this.DesktopCheck.Size = new System.Drawing.Size(114, 20);
            this.DesktopCheck.TabIndex = 5;
            this.DesktopCheck.Text = "Desktop Shortcut";
            this.DesktopCheck.UseVisualStyleBackColor = true;
            this.DesktopCheck.CheckedChanged += new System.EventHandler(this.Shortcutbox_Focus);
            // 
            // gameBrow
            // 
            this.gameBrow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.gameBrow.FlatAppearance.BorderSize = 0;
            this.gameBrow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(116)))), ((int)(((byte)(128)))));
            this.gameBrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gameBrow.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold);
            this.gameBrow.ForeColor = System.Drawing.Color.White;
            this.gameBrow.Location = new System.Drawing.Point(302, 261);
            this.gameBrow.Name = "gameBrow";
            this.gameBrow.Size = new System.Drawing.Size(119, 35);
            this.gameBrow.TabIndex = 3;
            this.gameBrow.Text = "Select Game";
            this.gameBrow.UseVisualStyleBackColor = false;
            this.gameBrow.Click += new System.EventHandler(this.GameBrow_Click);
            // 
            // emuBrow
            // 
            this.emuBrow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.emuBrow.FlatAppearance.BorderSize = 0;
            this.emuBrow.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(116)))), ((int)(((byte)(128)))));
            this.emuBrow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emuBrow.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold);
            this.emuBrow.ForeColor = System.Drawing.Color.White;
            this.emuBrow.Location = new System.Drawing.Point(302, 192);
            this.emuBrow.Name = "emuBrow";
            this.emuBrow.Size = new System.Drawing.Size(119, 35);
            this.emuBrow.TabIndex = 2;
            this.emuBrow.Text = "Select Emulator";
            this.emuBrow.UseVisualStyleBackColor = false;
            this.emuBrow.Click += new System.EventHandler(this.EmuBrow_Click);
            // 
            // Shortcutbox
            // 
            this.Shortcutbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.Shortcutbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Shortcutbox.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 12F);
            this.Shortcutbox.ForeColor = System.Drawing.Color.White;
            this.Shortcutbox.Location = new System.Drawing.Point(14, 137);
            this.Shortcutbox.MaxLength = 50;
            this.Shortcutbox.Multiline = true;
            this.Shortcutbox.Name = "Shortcutbox";
            this.Shortcutbox.Size = new System.Drawing.Size(254, 22);
            this.Shortcutbox.TabIndex = 1;
            this.Shortcutbox.TextChanged += new System.EventHandler(this.Shortcutbox_TextChanged);
            this.Shortcutbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ICOurl_KeyDown);
            this.Shortcutbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Shortcutbox_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(14, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 16);
            this.label4.TabIndex = 25;
            this.label4.Text = "Shortcut name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(14, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 16);
            this.label3.TabIndex = 24;
            this.label3.Text = "Select an emulator:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(14, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 16);
            this.label2.TabIndex = 23;
            this.label2.Text = "Game File Directory:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift SemiBold SemiConden", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(14, 172);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "Emulator Directory:";
            // 
            // emulatorcb
            // 
            this.emulatorcb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.emulatorcb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emulatorcb.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 11F);
            this.emulatorcb.ForeColor = System.Drawing.Color.White;
            this.emulatorcb.FormattingEnabled = true;
            this.emulatorcb.Location = new System.Drawing.Point(14, 73);
            this.emulatorcb.MaxDropDownItems = 10;
            this.emulatorcb.Name = "emulatorcb";
            this.emulatorcb.Size = new System.Drawing.Size(176, 26);
            this.emulatorcb.TabIndex = 6;
            this.emulatorcb.SelectedIndexChanged += new System.EventHandler(this.Emulatorcb_SelectedIndexChanged);
            // 
            // Edirbox
            // 
            this.Edirbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.Edirbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Edirbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.Edirbox.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 9F);
            this.Edirbox.ForeColor = System.Drawing.Color.White;
            this.Edirbox.Location = new System.Drawing.Point(14, 192);
            this.Edirbox.Multiline = true;
            this.Edirbox.Name = "Edirbox";
            this.Edirbox.ReadOnly = true;
            this.Edirbox.Size = new System.Drawing.Size(254, 35);
            this.Edirbox.TabIndex = 14;
            this.Edirbox.TabStop = false;
            this.Edirbox.Click += new System.EventHandler(this.EmuBrow_Click);
            // 
            // Gdirbox
            // 
            this.Gdirbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.Gdirbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Gdirbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.Gdirbox.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 9F);
            this.Gdirbox.ForeColor = System.Drawing.Color.White;
            this.Gdirbox.Location = new System.Drawing.Point(14, 261);
            this.Gdirbox.Multiline = true;
            this.Gdirbox.Name = "Gdirbox";
            this.Gdirbox.ReadOnly = true;
            this.Gdirbox.Size = new System.Drawing.Size(254, 35);
            this.Gdirbox.TabIndex = 15;
            this.Gdirbox.TabStop = false;
            this.Gdirbox.Click += new System.EventHandler(this.GameBrow_Click);
            // 
            // createshortbtn
            // 
            this.createshortbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.createshortbtn.FlatAppearance.BorderSize = 0;
            this.createshortbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(116)))), ((int)(((byte)(128)))));
            this.createshortbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createshortbtn.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 14.25F, System.Drawing.FontStyle.Bold);
            this.createshortbtn.ForeColor = System.Drawing.Color.White;
            this.createshortbtn.Location = new System.Drawing.Point(12, 337);
            this.createshortbtn.Name = "createshortbtn";
            this.createshortbtn.Size = new System.Drawing.Size(409, 65);
            this.createshortbtn.TabIndex = 4;
            this.createshortbtn.Text = "Create ShortCute";
            this.createshortbtn.UseVisualStyleBackColor = false;
            this.createshortbtn.Click += new System.EventHandler(this.CreateShortCute_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Bahnschrift SemiLight SemiConde", 9.75F);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(199, 61);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(222, 44);
            this.label6.TabIndex = 28;
            this.label6.Text = "Opening \'.exe\' emulator will auto-select it";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelBorderStyle
            // 
            this.panelBorderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(13)))), ((int)(((byte)(17)))), ((int)(((byte)(23)))));
            this.panelBorderStyle.Controls.Add(this.HistoryBtn);
            this.panelBorderStyle.Controls.Add(this.InfoButton);
            this.panelBorderStyle.Controls.Add(this.ConfigBtn);
            this.panelBorderStyle.Controls.Add(this.label7);
            this.panelBorderStyle.Controls.Add(this.miniBtn);
            this.panelBorderStyle.Controls.Add(this.closeBtn);
            this.panelBorderStyle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelBorderStyle.Location = new System.Drawing.Point(0, 0);
            this.panelBorderStyle.Name = "panelBorderStyle";
            this.panelBorderStyle.Size = new System.Drawing.Size(760, 42);
            this.panelBorderStyle.TabIndex = 29;
            this.panelBorderStyle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDisp_MouseDown);
            // 
            // HistoryBtn
            // 
            this.HistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(22)))), ((int)(((byte)(27)))));
            this.HistoryBtn.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("HistoryBtn.BackgroundImage")));
            this.HistoryBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.HistoryBtn.FlatAppearance.BorderSize = 0;
            this.HistoryBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.HistoryBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HistoryBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.HistoryBtn.Location = new System.Drawing.Point(602, 4);
            this.HistoryBtn.Name = "HistoryBtn";
            this.HistoryBtn.Size = new System.Drawing.Size(35, 35);
            this.HistoryBtn.TabIndex = 5;
            this.HistoryBtn.TabStop = false;
            this.toolTip.SetToolTip(this.HistoryBtn, "History");
            this.HistoryBtn.UseVisualStyleBackColor = false;
            this.HistoryBtn.Click += new System.EventHandler(this.HistoryBtn_Click);
            // 
            // InfoButton
            // 
            this.InfoButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(22)))), ((int)(((byte)(27)))));
            this.InfoButton.BackgroundImage = global::ShortCutes.Properties.Resources.infobtn;
            this.InfoButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.InfoButton.FlatAppearance.BorderSize = 0;
            this.InfoButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.InfoButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.InfoButton.ForeColor = System.Drawing.SystemColors.Control;
            this.InfoButton.Location = new System.Drawing.Point(564, 4);
            this.InfoButton.Name = "InfoButton";
            this.InfoButton.Size = new System.Drawing.Size(35, 35);
            this.InfoButton.TabIndex = 4;
            this.InfoButton.TabStop = false;
            this.toolTip.SetToolTip(this.InfoButton, "ShortCutes Information");
            this.InfoButton.UseVisualStyleBackColor = false;
            this.InfoButton.Click += new System.EventHandler(this.InfoButton_Click);
            // 
            // ConfigBtn
            // 
            this.ConfigBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(22)))), ((int)(((byte)(27)))));
            this.ConfigBtn.BackgroundImage = global::ShortCutes.Properties.Resources.config;
            this.ConfigBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ConfigBtn.FlatAppearance.BorderSize = 0;
            this.ConfigBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.ConfigBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConfigBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.ConfigBtn.Location = new System.Drawing.Point(640, 4);
            this.ConfigBtn.Name = "ConfigBtn";
            this.ConfigBtn.Size = new System.Drawing.Size(35, 35);
            this.ConfigBtn.TabIndex = 3;
            this.ConfigBtn.TabStop = false;
            this.toolTip.SetToolTip(this.ConfigBtn, "Configuration");
            this.ConfigBtn.UseVisualStyleBackColor = false;
            this.ConfigBtn.Click += new System.EventHandler(this.ConfigBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Bahnschrift SemiCondensed", 20.25F);
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(4, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 33);
            this.label7.TabIndex = 2;
            this.label7.Text = "ShortCutes";
            this.label7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormDisp_MouseDown);
            // 
            // miniBtn
            // 
            this.miniBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(22)))), ((int)(((byte)(27)))));
            this.miniBtn.BackgroundImage = global::ShortCutes.Properties.Resources.minimizebtn;
            this.miniBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.miniBtn.FlatAppearance.BorderSize = 0;
            this.miniBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.miniBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.miniBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.miniBtn.Location = new System.Drawing.Point(682, 4);
            this.miniBtn.Name = "miniBtn";
            this.miniBtn.Size = new System.Drawing.Size(35, 35);
            this.miniBtn.TabIndex = 0;
            this.miniBtn.TabStop = false;
            this.miniBtn.UseVisualStyleBackColor = false;
            this.miniBtn.Click += new System.EventHandler(this.MiniBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(22)))), ((int)(((byte)(27)))));
            this.closeBtn.BackgroundImage = global::ShortCutes.Properties.Resources.closebtn;
            this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Location = new System.Drawing.Point(719, 4);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(35, 35);
            this.closeBtn.TabIndex = 0;
            this.closeBtn.TabStop = false;
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // ICOurl
            // 
            this.ICOurl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ICOurl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ICOurl.Font = new System.Drawing.Font("Bahnschrift Light SemiCondensed", 11F);
            this.ICOurl.ForeColor = System.Drawing.Color.White;
            this.ICOurl.Location = new System.Drawing.Point(446, 382);
            this.ICOurl.Multiline = true;
            this.ICOurl.Name = "ICOurl";
            this.ICOurl.Size = new System.Drawing.Size(300, 28);
            this.ICOurl.TabIndex = 30;
            this.ICOurl.Text = "or paste an Image URL here...";
            this.ICOurl.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ICOurl.Click += new System.EventHandler(this.ICOurl_Click);
            this.ICOurl.TextChanged += new System.EventHandler(this.ICOurl_TextChanged);
            this.ICOurl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ICOurl_KeyDown);
            this.ICOurl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ICOurl_KeyPress);
            this.ICOurl.Leave += new System.EventHandler(this.ICOurl_Leave);
            // 
            // ICOpic
            // 
            this.ICOpic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ICOpic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ICOpic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ICOpic.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ICOpic.Location = new System.Drawing.Point(446, 76);
            this.ICOpic.Name = "ICOpic";
            this.ICOpic.Size = new System.Drawing.Size(300, 300);
            this.ICOpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ICOpic.TabIndex = 26;
            this.ICOpic.TabStop = false;
            this.ICOpic.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ICOpic_MouseClick);
            this.ICOpic.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ICOpic_MouseDoubleClick);
            // 
            // ClearSCSelected
            // 
            this.ClearSCSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(56)))), ((int)(((byte)(74)))));
            this.ClearSCSelected.FlatAppearance.BorderSize = 0;
            this.ClearSCSelected.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(116)))), ((int)(((byte)(128)))));
            this.ClearSCSelected.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ClearSCSelected.Font = new System.Drawing.Font("Bahnschrift Condensed", 11.25F, System.Drawing.FontStyle.Bold);
            this.ClearSCSelected.ForeColor = System.Drawing.Color.White;
            this.ClearSCSelected.Location = new System.Drawing.Point(302, 124);
            this.ClearSCSelected.Name = "ClearSCSelected";
            this.ClearSCSelected.Size = new System.Drawing.Size(119, 35);
            this.ClearSCSelected.TabIndex = 31;
            this.ClearSCSelected.Text = "Clean Data";
            this.toolTip.SetToolTip(this.ClearSCSelected, "Cleans UI from selected history data");
            this.ClearSCSelected.UseVisualStyleBackColor = false;
            this.ClearSCSelected.Visible = false;
            this.ClearSCSelected.Click += new System.EventHandler(this.ClearSCSelected_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 250;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 50;
            // 
            // ShortCutes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(28)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(760, 415);
            this.ControlBox = false;
            this.Controls.Add(this.ClearSCSelected);
            this.Controls.Add(this.ICOurl);
            this.Controls.Add(this.panelBorderStyle);
            this.Controls.Add(this.label6);
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
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShortCutes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShortCutes";
            this.Shown += new System.EventHandler(this.ShortCutes_Shown);
            this.Click += new System.EventHandler(this.Shortcutbox_Focus);
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
        private System.Windows.Forms.TextBox ICOurl;
        private System.Windows.Forms.Button ConfigBtn;
        private System.Windows.Forms.Button InfoButton;
        private System.Windows.Forms.Button HistoryBtn;
        private System.Windows.Forms.Button ClearSCSelected;
        private System.Windows.Forms.ToolTip toolTip;
    }
}

