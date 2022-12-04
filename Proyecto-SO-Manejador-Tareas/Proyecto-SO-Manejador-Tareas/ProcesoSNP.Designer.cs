namespace Proyecto_SO_Manejador_Tareas
{
    partial class ProcesoSNP
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
            this.Llegada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.btAgregar = new System.Windows.Forms.Button();
            this.txtCPU = new System.Windows.Forms.TextBox();
            this.txtNombre = new System.Windows.Forms.TextBox();
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
            this.Llegada});
            this.grdProcesos.Location = new System.Drawing.Point(15, 76);
            this.grdProcesos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.grdProcesos.Name = "grdProcesos";
            this.grdProcesos.RowHeadersWidth = 51;
            this.grdProcesos.RowTemplate.Height = 24;
            this.grdProcesos.Size = new System.Drawing.Size(721, 202);
            this.grdProcesos.TabIndex = 38;
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
            // Llegada
            // 
            this.Llegada.HeaderText = "Tiempo de Llegada";
            this.Llegada.MinimumWidth = 6;
            this.Llegada.Name = "Llegada";
            this.Llegada.ReadOnly = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(571, 297);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(165, 32);
            this.button2.TabIndex = 34;
            this.button2.Text = "Cargar Procesos";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btAgregar
            // 
            this.btAgregar.Location = new System.Drawing.Point(645, 25);
            this.btAgregar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btAgregar.Name = "btAgregar";
            this.btAgregar.Size = new System.Drawing.Size(91, 32);
            this.btAgregar.TabIndex = 33;
            this.btAgregar.Text = "Agregar";
            this.btAgregar.UseVisualStyleBackColor = true;
            this.btAgregar.Click += new System.EventHandler(this.btAgregar_Click);
            // 
            // txtCPU
            // 
            this.txtCPU.Location = new System.Drawing.Point(248, 30);
            this.txtCPU.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCPU.Name = "txtCPU";
            this.txtCPU.Size = new System.Drawing.Size(173, 22);
            this.txtCPU.TabIndex = 31;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(15, 30);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(213, 22);
            this.txtNombre.TabIndex = 30;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(249, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 36;
            this.label2.Text = "Rafaga de CPU";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 35;
            this.label1.Text = "Proceso";
            // 
            // ProcesoSNP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 341);
            this.Controls.Add(this.grdProcesos);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btAgregar);
            this.Controls.Add(this.txtCPU);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ProcesoSNP";
            this.Text = "ProcesoSNP";
            ((System.ComponentModel.ISupportInitialize)(this.grdProcesos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdProcesos;
        private System.Windows.Forms.DataGridViewTextBoxColumn codigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPU;
        private System.Windows.Forms.DataGridViewTextBoxColumn Llegada;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btAgregar;
        private System.Windows.Forms.TextBox txtCPU;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}