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
    public partial class ProcesoSNP : Form
    {
        int id = 0, llegada=0;
        public ProcesoSNP()
        {
            InitializeComponent();
        }

        private void btAgregar_Click(object sender, EventArgs e)
        {
            grdProcesos.Rows.Add(id.ToString(), txtNombre.Text, txtCPU.Text, llegada.ToString());
            Menu menu = Owner as Menu;
            menu.agregarCola(id, txtNombre.Text, int.Parse(txtCPU.Text), llegada);
            id++;
            llegada++;
            txtCPU.Clear();
            txtNombre.Clear();
            txtNombre.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
