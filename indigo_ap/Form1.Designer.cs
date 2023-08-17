namespace indigo_ap
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
            this.tbNombreArchivo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btn_eliminar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.dgvData = new System.Windows.Forms.DataGridView();
            this.btnAbrirArchivo = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_openPRN = new System.Windows.Forms.Button();
            this.btn_buscar = new System.Windows.Forms.Button();
            this.tb_buscar = new System.Windows.Forms.TextBox();
            this.cmb_campos = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dgvImpresion = new System.Windows.Forms.DataGridView();
            this.cmb_prn = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnPrint = new System.Windows.Forms.Button();
            this.lbInfo = new System.Windows.Forms.Label();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.pgbEstado = new System.Windows.Forms.ProgressBar();
            this.lbl_info = new System.Windows.Forms.Label();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImpresion)).BeginInit();
            this.SuspendLayout();
            // 
            // tbNombreArchivo
            // 
            resources.ApplyResources(this.tbNombreArchivo, "tbNombreArchivo");
            this.tbNombreArchivo.Name = "tbNombreArchivo";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Name = "label1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btn_eliminar);
            this.tabPage1.Controls.Add(this.btnGuardar);
            this.tabPage1.Controls.Add(this.dgvData);
            this.tabPage1.Controls.Add(this.btnAbrirArchivo);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.tbNombreArchivo);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btn_eliminar
            // 
            this.btn_eliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.btn_eliminar, "btn_eliminar");
            this.btn_eliminar.ForeColor = System.Drawing.Color.White;
            this.btn_eliminar.Name = "btn_eliminar";
            this.btn_eliminar.UseVisualStyleBackColor = false;
            this.btn_eliminar.Click += new System.EventHandler(this.btn_eliminar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.btnGuardar, "btnGuardar");
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // dgvData
            // 
            this.dgvData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvData, "dgvData");
            this.dgvData.Name = "dgvData";
            // 
            // btnAbrirArchivo
            // 
            this.btnAbrirArchivo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.btnAbrirArchivo, "btnAbrirArchivo");
            this.btnAbrirArchivo.ForeColor = System.Drawing.Color.White;
            this.btnAbrirArchivo.Name = "btnAbrirArchivo";
            this.btnAbrirArchivo.UseVisualStyleBackColor = false;
            this.btnAbrirArchivo.Click += new System.EventHandler(this.btnAbrirArchivo_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_openPRN);
            this.tabPage2.Controls.Add(this.btn_buscar);
            this.tabPage2.Controls.Add(this.tb_buscar);
            this.tabPage2.Controls.Add(this.cmb_campos);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.dgvImpresion);
            this.tabPage2.Controls.Add(this.cmb_prn);
            this.tabPage2.Controls.Add(this.label2);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_openPRN
            // 
            this.btn_openPRN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.btn_openPRN, "btn_openPRN");
            this.btn_openPRN.ForeColor = System.Drawing.Color.White;
            this.btn_openPRN.Name = "btn_openPRN";
            this.btn_openPRN.UseVisualStyleBackColor = false;
            this.btn_openPRN.Click += new System.EventHandler(this.btn_openPRN_Click);
            // 
            // btn_buscar
            // 
            this.btn_buscar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.btn_buscar, "btn_buscar");
            this.btn_buscar.ForeColor = System.Drawing.Color.White;
            this.btn_buscar.Name = "btn_buscar";
            this.btn_buscar.UseVisualStyleBackColor = false;
            this.btn_buscar.Click += new System.EventHandler(this.btn_buscar_Click);
            // 
            // tb_buscar
            // 
            resources.ApplyResources(this.tb_buscar, "tb_buscar");
            this.tb_buscar.Name = "tb_buscar";
            // 
            // cmb_campos
            // 
            this.cmb_campos.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_campos, "cmb_campos");
            this.cmb_campos.Name = "cmb_campos";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Name = "label3";
            // 
            // dgvImpresion
            // 
            this.dgvImpresion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resources.ApplyResources(this.dgvImpresion, "dgvImpresion");
            this.dgvImpresion.Name = "dgvImpresion";
            this.dgvImpresion.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvImpresion_CellClick);
            // 
            // cmb_prn
            // 
            this.cmb_prn.FormattingEnabled = true;
            resources.ApplyResources(this.cmb_prn, "cmb_prn");
            this.cmb_prn.Name = "cmb_prn";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Name = "label2";
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Image = global::indigo_ap.Properties.Resources.icons8_print_96;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // lbInfo
            // 
            resources.ApplyResources(this.lbInfo, "lbInfo");
            this.lbInfo.ForeColor = System.Drawing.Color.White;
            this.lbInfo.Name = "lbInfo";
            // 
            // btnCerrar
            // 
            this.btnCerrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            resources.ApplyResources(this.btnCerrar, "btnCerrar");
            this.btnCerrar.ForeColor = System.Drawing.Color.White;
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.UseVisualStyleBackColor = false;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // pgbEstado
            // 
            resources.ApplyResources(this.pgbEstado, "pgbEstado");
            this.pgbEstado.Name = "pgbEstado";
            this.pgbEstado.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // lbl_info
            // 
            this.lbl_info.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.lbl_info, "lbl_info");
            this.lbl_info.ForeColor = System.Drawing.Color.White;
            this.lbl_info.Name = "lbl_info";
            this.lbl_info.Click += new System.EventHandler(this.lbl_info_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ControlBox = false;
            this.Controls.Add(this.lbl_info);
            this.Controls.Add(this.pgbEstado);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.lbInfo);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvData)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvImpresion)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbNombreArchivo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvData;
        private System.Windows.Forms.Button btnAbrirArchivo;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.ProgressBar pgbEstado;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.ComboBox cmb_prn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_buscar;
        private System.Windows.Forms.TextBox tb_buscar;
        private System.Windows.Forms.ComboBox cmb_campos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView dgvImpresion;
        private System.Windows.Forms.Button btn_openPRN;
        private System.Windows.Forms.Label lbl_info;
        private System.Windows.Forms.Button btn_eliminar;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}

