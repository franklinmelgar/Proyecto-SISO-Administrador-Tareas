using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_SO_Manejador_Tareas
{
    public partial class ProcesoPrioridad : Form
    {
        int id = 1;
        public ProcesoPrioridad()
        {
            InitializeComponent();
        }

        private void btAgregar_Click(object sender, EventArgs e)
        {
            grdProcesos.Rows.Add(id.ToString(), txtNombre.Text, txtCPU.Text, txtPrioridad.Text);
            Menu menu = Owner as Menu;
            menu.agregarProcesoCola(id, txtNombre.Text, int.Parse(txtCPU.Text), int.Parse(txtPrioridad.Text));
            id++;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
