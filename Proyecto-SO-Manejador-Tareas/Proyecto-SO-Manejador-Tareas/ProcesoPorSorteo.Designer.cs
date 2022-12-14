namespace Proyecto_SO_Manejador_Tareas
{
    partial class ProcesoPorSorteo
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
            this.txtCantidadTickets = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
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
            this.grdProcesos.Location = new System.Drawing.Point(23, 163);
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
            this.button2.Location = new System.Drawing.Point(484, 376);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 32);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cargar Procesos";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btAgregar
            // 
            this.btAgregar.Location = new System.Drawing.Point(559, 106);
            this.btAgregar.Name = "btAgregar";
            this.btAgregar.Size = new System.Drawing.Size(91, 32);
            this.btAgregar.TabIndex = 4;
            this.btAgregar.Text = "Agregar";
            this.btAgregar.UseVisualStyleBackColor = true;
            this.btAgregar.Click += new System.EventHandler(this.btAgregar_Click);
            // 
            // txtPrioridad
            // 
            this.txtPrioridad.Location = new System.Drawing.Point(392, 116);
            this.txtPrioridad.Name = "txtPrioridad";
            this.txtPrioridad.Size = new System.Drawing.Size(155, 22);
            this.txtPrioridad.TabIndex = 3;
            // 
            // txtCPU
            // 
            this.txtCPU.Location = new System.Drawing.Point(227, 116);
            this.txtCPU.Name = "txtCPU";
            this.txtCPU.Size = new System.Drawing.Size(155, 22);
            this.txtCPU.TabIndex = 2;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(23, 116);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(195, 22);
            this.txtNombre.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(389, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 16);
            this.label3.TabIndex = 12;
            this.label3.Text = "Prioridad";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(230, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Rafaga de CPU";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Proceso";
            // 
            // txtCantidadTickets
            // 
            this.txtCantidadTickets.Location = new System.Drawing.Point(23, 43);
            this.txtCantidadTickets.Name = "txtCantidadTickets";
            this.txtCantidadTickets.Size = new System.Drawing.Size(627, 22);
            this.txtCantidadTickets.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(121, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "Cantidad de tickets";
            // 
            // ProcesoPorSorteo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 425);
            this.Controls.Add(this.txtCantidadTickets);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.grdProcesos);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btAgregar);
            this.Controls.Add(this.txtPrioridad);
            this.Controls.Add(this.txtCPU);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ProcesoPorSorteo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Proceso Por Sorteo";
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
        private System.Windows.Forms.TextBox txtCantidadTickets;
        private System.Windows.Forms.Label label4;
    }
}