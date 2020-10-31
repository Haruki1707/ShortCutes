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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.Gdirbox = new System.Windows.Forms.TextBox();
            this.Edirbox = new System.Windows.Forms.TextBox();
            this.emulatorcb = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.emuladorDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emupathBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.emupathDataSet = new Console_Emulators_Shortcutes.emupathDataSet();
            this.emupathTableAdapter = new Console_Emulators_Shortcutes.emupathDataSetTableAdapters.emupathTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emupathBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emupathDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(115, 275);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(187, 91);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Gdirbox
            // 
            this.Gdirbox.Location = new System.Drawing.Point(114, 102);
            this.Gdirbox.Name = "Gdirbox";
            this.Gdirbox.Size = new System.Drawing.Size(254, 20);
            this.Gdirbox.TabIndex = 2;
            // 
            // Edirbox
            // 
            this.Edirbox.Location = new System.Drawing.Point(114, 44);
            this.Edirbox.Name = "Edirbox";
            this.Edirbox.Size = new System.Drawing.Size(253, 20);
            this.Edirbox.TabIndex = 1;
            // 
            // emulatorcb
            // 
            this.emulatorcb.FormattingEnabled = true;
            this.emulatorcb.Items.AddRange(new object[] {
            "CEMU",
            "PJ64",
            "SNES9X"});
            this.emulatorcb.Location = new System.Drawing.Point(115, 128);
            this.emulatorcb.Name = "emulatorcb";
            this.emulatorcb.Size = new System.Drawing.Size(187, 21);
            this.emulatorcb.TabIndex = 3;
            this.emulatorcb.SelectedIndexChanged += new System.EventHandler(this.emulatorcb_SelectedIndexChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(438, 26);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(314, 412);
            this.textBox1.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.emuladorDataGridViewTextBoxColumn,
            this.pathDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.emupathBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(114, 175);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(282, 94);
            this.dataGridView1.TabIndex = 5;
            // 
            // emuladorDataGridViewTextBoxColumn
            // 
            this.emuladorDataGridViewTextBoxColumn.DataPropertyName = "emulador";
            this.emuladorDataGridViewTextBoxColumn.HeaderText = "emulador";
            this.emuladorDataGridViewTextBoxColumn.Name = "emuladorDataGridViewTextBoxColumn";
            // 
            // pathDataGridViewTextBoxColumn
            // 
            this.pathDataGridViewTextBoxColumn.DataPropertyName = "path";
            this.pathDataGridViewTextBoxColumn.HeaderText = "path";
            this.pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            // 
            // emupathBindingSource
            // 
            this.emupathBindingSource.DataMember = "emupath";
            this.emupathBindingSource.DataSource = this.emupathDataSet;
            // 
            // emupathDataSet
            // 
            this.emupathDataSet.DataSetName = "emupathDataSet";
            this.emupathDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // emupathTableAdapter
            // 
            this.emupathTableAdapter.ClearBeforeFill = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.emulatorcb);
            this.Controls.Add(this.Edirbox);
            this.Controls.Add(this.Gdirbox);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emupathBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emupathDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Gdirbox;
        private System.Windows.Forms.TextBox Edirbox;
        private System.Windows.Forms.ComboBox emulatorcb;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private emupathDataSet emupathDataSet;
        private System.Windows.Forms.BindingSource emupathBindingSource;
        private emupathDataSetTableAdapters.emupathTableAdapter emupathTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn emuladorDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
    }
}

