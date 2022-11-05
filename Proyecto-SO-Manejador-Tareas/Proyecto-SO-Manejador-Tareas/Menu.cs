using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_SO_Manejador_Tareas
{

    struct proceso
    {
        public int id;
        public string nombre;
        public int cpu;
        public int prioridad;
        public string estado;
        public int tiempo;
    }

   


    public partial class Menu : Form
    {

        private Thread hiloProcesoRoundRobin = null;

        Queue<proceso> colaProcesos = new Queue<proceso>();
        Queue<proceso> colaProcesosTerminados = new Queue<proceso>();
        int quantum;
        int tiempoms;
        public Menu()
        {
            InitializeComponent();
        }
        public void agregarProcesoCola(int codigo, string nombre, int cpu, int prioridad)
        {
            proceso itemColaProceso = new proceso();
            itemColaProceso.id = codigo;
            itemColaProceso.nombre = nombre;
            itemColaProceso.cpu = cpu;
            itemColaProceso.prioridad = prioridad;
            itemColaProceso.estado = "Listo";

            colaProcesos.Enqueue(itemColaProceso);
        }

        private void btCargar_Click(object sender, EventArgs e)
        {

            if (cmbAlgoritmo.Text.Equals("Round Robin"))
            {
                ProcesosRoundRobin agregarProcesos = new ProcesosRoundRobin();
                AddOwnedForm(agregarProcesos);
                agregarProcesos.Show();
            }
            
        }

        private void btIniciar_Click(object sender, EventArgs e)
        {
            hiloProcesoRoundRobin = new Thread(new ThreadStart(algoritmoRoundRobin));

            if (txtQuantumGeneral.Text.Equals("") || txtTiempo.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar el quantum y tiempo en milisegundos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                quantum = int.Parse(txtQuantumGeneral.Text);
                tiempoms = int.Parse(txtTiempo.Text);

                if (cmbAlgoritmo.Text.Equals("Round Robin"))
                {
                    hiloProcesoRoundRobin.Start(); 
                }
            }
            
        }

        private void algoritmoRoundRobin()
        {           

            imprimirListos(colaProcesos);

            do
            {
                proceso itemProceso = colaProcesos.Peek();
                txtID.Invoke((MethodInvoker) (() => txtID.Text = itemProceso.id.ToString()));
                txtNombreProceso.Invoke((MethodInvoker) (() => txtNombreProceso.Text = itemProceso.nombre));
                txtCPU.Invoke((MethodInvoker) (() => txtCPU.Text = itemProceso.cpu.ToString()));

                for (int i = quantum; i >= 1; i--)
                {
                    txtQuantum.Invoke((MethodInvoker) (() => txtQuantum.Text = i.ToString()));
                    Thread.Sleep(tiempoms);
                }

                proceso itemProcesoLeido = colaProcesos.Dequeue();
                if (itemProcesoLeido.cpu <= quantum)
                {
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";
                    colaProcesosTerminados.Enqueue(itemProcesoLeido);
                    imprimirTerminados(colaProcesosTerminados);
                    imprimirListos(colaProcesos);
                }
                else
                {
                    itemProcesoLeido.cpu -= quantum;
                    colaProcesos.Enqueue(itemProcesoLeido);
                    imprimirListos(colaProcesos);
                }

            } while (colaProcesos.Count > 0);

        }

        private void imprimirTerminados(Queue<proceso> procesos)
        {
            grdTerminados.Invoke((MethodInvoker)(() => grdTerminados.Rows.Clear()));
            foreach (proceso pro in procesos)
            {
                grdTerminados.Invoke((MethodInvoker)(() => grdTerminados.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
            }
        }

        private void imprimirListos(Queue<proceso> procesos)
        {
            grdProcesosListos.Invoke((MethodInvoker) (() => grdProcesosListos.Rows.Clear()));
            foreach (proceso pro in procesos)
            {
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
            }
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }
    }
}
