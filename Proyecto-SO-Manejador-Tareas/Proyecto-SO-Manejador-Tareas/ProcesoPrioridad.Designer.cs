namespace Proyecto_SO_Manejador_Tareas
{
    partial class ProcesoCPU
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
            this.grdProcesos = new System.Windows.Forms.DataGridView();
            this.txtCPU = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPrioridad = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.btAgregar = new System.Windows.Forms.Button();
            this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.prioridad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdProcesos)).BeginInit();
            this.SuspendLayout();
            // 
            // grdProcesos
            // 
            this.grdProcesos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdProcesos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdProcesos.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.codigo,
            this.nombre,
            this.dataGridViewTextBoxColumn1,
            this.prioridad});
            this.grdProcesos.Location = new System.Drawing.Point(42, 121);
            this.grdProcesos.Name = "grdProcesos";
            this.grdProcesos.RowHeadersWidth = 51;
            this.grdProcesos.RowTemplate.Height = 24;
            this.grdProcesos.Size = new System.Drawing.Size(619, 202);
            this.grdProcesos.TabIndex = 16;
            // 
            // txtCPU
            // 
            this.txtCPU.Location = new System.Drawing.Point(226, 61);
            this.txtCPU.Name = "txtCPU";
            this.txtCPU.Size = new System.Drawing.Size(128, 22);
            this.txtCPU.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(223, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "Rafaga CPU";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(42, 61);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(128, 22);
            this.txtNombre.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Proceso";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(398, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 16);
            this.label3.TabIndex = 17;
            this.label3.Text = "Prioridad";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // txtPrioridad
            // 
            this.txtPrioridad.Location = new System.Drawing.Point(401, 61);
            this.txtPrioridad.Name = "txtPrioridad";
            this.txtPrioridad.Size = new System.Drawing.Size(128, 22);
            this.txtPrioridad.TabIndex = 18;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(495, 344);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 32);
            this.button2.TabIndex = 19;
            this.button2.Text = "Cargar Procesos";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btAgregar
            // 
            this.btAgregar.Location = new System.Drawing.Point(570, 51);
            this.btAgregar.Name = "btAgregar";
            this.btAgregar.Size = new System.Drawing.Size(91, 32);
            this.btAgregar.TabIndex = 20;
            this.btAgregar.Text = "Agregar";
            this.btAgregar.UseVisualStyleBackColor = true;
            this.btAgregar.Click += new System.EventHandler(this.btAgregar_Click);
            // 
            // codigo
            // 
            this.codigo.HeaderText = "Id";
            this.codigo.MinimumWidth = 6;
            this.codigo.Name = "codigo";
            this.codigo.ReadOnly = true;
            // 
            // nombre
            // 
            this.nombre.HeaderText = "Proceso";
            this.nombre.MinimumWidth = 6;
            this.nombre.Name = "nombre";
            this.nombre.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Rafaga CPU";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // prioridad
            // 
            this.prioridad.HeaderText = "Prioridad";
            this.prioridad.MinimumWidth = 6;
            this.prioridad.Name = "prioridad";
            // 
            // ProcesoCPU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 396);
            this.Controls.Add(this.btAgregar);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.txtPrioridad);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.grdProcesos);
            this.Controls.Add(this.txtCPU);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label1);
            this.Name = "ProcesoCPU";
            this.Text = "ProcesoCPU";
            ((System.ComponentModel.ISupportInitialize)(this.grdProcesos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdProcesos;
        private System.Windows.Forms.TextBox txtCPU;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPrioridad;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btAgregar;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn prioridad;
    }
}