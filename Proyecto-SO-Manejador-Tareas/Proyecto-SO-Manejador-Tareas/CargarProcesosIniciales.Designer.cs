namespace Proyecto_SO_Manejador_Tareas
{
    partial class CargarProcesosIniciales
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
            this.codigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CPU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prioridad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.btAgregar = new System.Windows.Forms.Button();
            this.txtPrioridad = new System.Windows.Forms.TextBox();
            this.txtCPU = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
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
            this.CPU,
            this.Prioridad});
            this.grdProcesos.Location = new System.Drawing.Point(35, 90);
            this.grdProcesos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grdProcesos.Name = "grdProcesos";
            this.grdProcesos.RowHeadersWidth = 51;
            this.grdProcesos.RowTemplate.Height = 24;
            this.grdProcesos.Size = new System.Drawing.Size(627, 202);
            this.grdProcesos.TabIndex = 18;
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
            // CPU
            // 
            this.CPU.HeaderText = "Rafaga CPU";
            this.CPU.MinimumWidth = 6;
            this.CPU.Name = "CPU";
            this.CPU.ReadOnly = true;
            // 
            // Prioridad
            // 
            this.Prioridad.HeaderText = "Prioridad";
            this.Prioridad.MinimumWidth = 6;
            this.Prioridad.Name = "Prioridad";
            this.Prioridad.ReadOnly = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(496, 303);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(165, 32);
            this.button2.TabIndex = 17;
            this.button2.Text = "Cargar Procesos";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btAgregar
            // 
            this.btAgregar.Location = new System.Drawing.Point(571, 32);
            this.btAgregar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btAgregar.Name = "btAgregar";
            this.btAgregar.Size = new System.Drawing.Size(91, 32);
            this.btAgregar.TabIndex = 16;
            this.btAgregar.Text = "Agregar";
            this.btAgregar.UseVisualStyleBackColor = true;
            this.btAgregar.Click += new System.EventHandler(this.btAgregar_Click);
            // 
            // txtPrioridad
            // 
            this.txtPrioridad.Location = new System.Drawing.Point(404, 42);
            this.txtPrioridad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPrioridad.Name = "txtPrioridad";
            this.txtPrioridad.Size = new System.Drawing.Size(155, 22);
            this.txtPrioridad.TabIndex = 15;
            // 
            // txtCPU
            // 
            this.txtCPU.Location = new System.Drawing.Point(239, 42);
            this.txtCPU.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCPU.Name = "txtCPU";
            this.txtCPU.Size = new System.Drawing.Size(155, 22);
            this.txtCPU.TabIndex = 14;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(35, 42);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(195, 22);
            this.txtNombre.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(402, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Prioridad";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Rafaga de CPU";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Proceso";
            // 
            // CargarProcesosIniciales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 359);
            this.Controls.Add(this.grdProcesos);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btAgregar);
            this.Controls.Add(this.txtPrioridad);
            this.Controls.Add(this.txtCPU);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "CargarProcesosIniciales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cargar Procesos Iniciales";
            this.Load += new System.EventHandler(this.CargarProcesosIniciales_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdProcesos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdProcesos;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPU;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prioridad;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btAgregar;
        private System.Windows.Forms.TextBox txtPrioridad;
        private System.Windows.Forms.TextBox txtCPU;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}