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
        public int tikects;
        public int probabilidad;
        public int ticketMinimo;
        public int ticketMaximo;
    }

   


    public partial class Menu : Form
    {

        private Thread hiloProcesoRoundRobin = null;
        private Thread hiloProcesoPorSorteo = null;

        Queue<proceso> colaProcesos = new Queue<proceso>();
        Queue<proceso> colaProcesosTerminados = new Queue<proceso>();
        Queue<proceso> colaProcesosBloqueados = new Queue<proceso>();
        int quantum;
        int tiempoms;
        int cantidadTickets;
        int totalPrioridad = 0;


        public Menu()
        {
            InitializeComponent();
        }

        public void asignarCantidadTickets(int cantidad)
        {
            cantidadTickets = cantidad;
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
            grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Clear()));
            grdBloqueados.Invoke((MethodInvoker)(() => grdBloqueados.Rows.Clear()));
            grdTerminados.Invoke((MethodInvoker)(() => grdTerminados.Rows.Clear()));
            colaProcesos.Clear();
            colaProcesosTerminados.Clear();
            colaProcesosBloqueados.Clear();


            if (cmbAlgoritmo.Text.Equals("Round Robin"))
            {
                ProcesosRoundRobin agregarProcesos = new ProcesosRoundRobin();
                AddOwnedForm(agregarProcesos);
                agregarProcesos.Show();
            }else if (cmbAlgoritmo.Text.Equals("Por sorteo"))
            {
                ProcesoPorSorteo agregarProcesosSorteo = new ProcesoPorSorteo();
                AddOwnedForm(agregarProcesosSorteo);
                agregarProcesosSorteo.Show();
            
            }else if (cmbAlgoritmo.Text.Equals(""))
            {
                MessageBox.Show("Debe seleccionar un algoritmo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btIniciar_Click(object sender, EventArgs e)
        {

            generarColumnasGrid(cmbAlgoritmo.Text);

            hiloProcesoRoundRobin = new Thread(new ThreadStart(algoritmoRoundRobin));
            hiloProcesoPorSorteo = new Thread(new ThreadStart(algoritmoPorSorteo));

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
                }else if (cmbAlgoritmo.Text.Equals("Por sorteo"))
                {
                    hiloProcesoPorSorteo.Start();
                }
            }
            
        }

        private void algoritmoRoundRobin()
        {           

            imprimirListos(colaProcesos, "Round Robin");

            do
            {
                proceso itemProceso = colaProcesos.Peek();
                txtID.Invoke((MethodInvoker) (() => txtID.Text = itemProceso.id.ToString()));
                txtNombreProceso.Invoke((MethodInvoker) (() => txtNombreProceso.Text = itemProceso.nombre));
                txtCPU.Invoke((MethodInvoker) (() => txtCPU.Text = itemProceso.cpu.ToString()));

                for (int i = quantum; i >= 1; i--)
                {
                    txtQuantum.Invoke((MethodInvoker) (() => txtQuantum.Text = i.ToString()));
                    txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text =  (int.Parse(txtCPU.Text) - 1).ToString() ));
                    Thread.Sleep(tiempoms);
                }

                proceso itemProcesoLeido = colaProcesos.Dequeue();
                if (itemProcesoLeido.cpu <= quantum)
                {
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";
                    colaProcesosTerminados.Enqueue(itemProcesoLeido);
                    imprimirTerminados(colaProcesosTerminados);
                    imprimirListos(colaProcesos, "Round Robin");
                }
                else
                {
                    itemProcesoLeido.cpu -= quantum;
                    colaProcesos.Enqueue(itemProcesoLeido);
                    imprimirListos(colaProcesos, "Round Robin");
                }

            } while (colaProcesos.Count > 0);

            MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void algoritmoPorSorteo()
        {
            Boolean encontrado;

            //asignar probabilidad
            asignarProbabilidad();

            imprimirListos(colaProcesos, "Por Sorteo");
            int ultimonumero = 0;

            do
            {
                encontrado = false;
                int ticket = 0;
                Random generador = new Random();

                do
                {
                    ticket = generador.Next(1, cantidadTickets);
                } while (ultimonumero == ticket);

                ultimonumero = ticket;
                lblTicket.Invoke((MethodInvoker)(() => lblTicket.Text = ticket.ToString() ));


                do
                {
                    //proceso itemProceso = colaProcesos.Peek();
                    proceso itemProcesoLeido = colaProcesos.Dequeue();

                    if (ticket >= itemProcesoLeido.ticketMinimo && ticket <= itemProcesoLeido.ticketMaximo)
                    {
                        encontrado = true;
                        txtID.Invoke((MethodInvoker)(() => txtID.Text = itemProcesoLeido.id.ToString()));
                        txtNombreProceso.Invoke((MethodInvoker)(() => txtNombreProceso.Text = itemProcesoLeido.nombre));
                        txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = itemProcesoLeido.cpu.ToString()));

                        for (int i = quantum; i >= 1; i--)
                        {
                            txtQuantum.Invoke((MethodInvoker)(() => txtQuantum.Text = i.ToString()));
                            txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = (int.Parse(txtCPU.Text) - 1).ToString()));
                            Thread.Sleep(tiempoms);
                        }
                        
                        if (itemProcesoLeido.cpu <= quantum)
                        {
                            itemProcesoLeido.cpu = 0;
                            itemProcesoLeido.estado = "Terminado";
                            colaProcesosTerminados.Enqueue(itemProcesoLeido);
                            imprimirTerminados(colaProcesosTerminados);
                            asignarProbabilidad();
                            imprimirListos(colaProcesos, "Por Sorteo");
                        }
                        else
                        {
                            itemProcesoLeido.cpu -= quantum;
                            colaProcesos.Enqueue(itemProcesoLeido);
                            imprimirListos(colaProcesos, "Por Sorteo");
                        }
                    }

                    if (encontrado.Equals(false))
                    {
                        colaProcesos.Enqueue(itemProcesoLeido);
                    }

                } while (encontrado == false);                

            } while (colaProcesos.Count > 0);

            //MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void asignarProbabilidad()
        {
            totalPrioridad = 0;
            int cantidad = colaProcesos.Count();

            foreach (proceso pro in colaProcesos)
            {
                totalPrioridad += pro.prioridad;
            }

            decimal porcentaje;
            decimal prioridad;
            int ticketAsignado = 1;
            int cantidadTicketsAsignado;
            for (int x = 0; x < cantidad; x++)
            {
                proceso itemProceso = colaProcesos.Dequeue();
                prioridad = decimal.Parse(itemProceso.prioridad.ToString());
                porcentaje = prioridad / decimal.Parse(totalPrioridad.ToString());
                cantidadTicketsAsignado = Decimal.ToInt32(cantidadTickets * porcentaje);
                itemProceso.probabilidad = Decimal.ToInt32(porcentaje * 100);
                itemProceso.tikects = cantidadTicketsAsignado;
                itemProceso.ticketMinimo = ticketAsignado;
                itemProceso.ticketMaximo = (ticketAsignado + cantidadTicketsAsignado) - 1;
                ticketAsignado = (ticketAsignado + cantidadTicketsAsignado);
                colaProcesos.Enqueue(itemProceso);
            }
        }



        private void generarColumnasGrid(string algoritmoNombre)
        {
            grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Clear()));

            if (algoritmoNombre.Equals("Round Robin"))
            {
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("Id", "Id")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("nombre", "Nombre Proceso")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("rafaga", "Rafaga CPU")));

            }
            else if (algoritmoNombre.Equals("Por sorteo"))
            {
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("Id", "Id")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("nombre", "Nombre Proceso")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("rafaga", "Rafaga CPU")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("tickets", "Tickets")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("probabilidad", "Probabilidad")));
            }

        }


        private void imprimirTerminados(Queue<proceso> procesos)
        {
            grdTerminados.Invoke((MethodInvoker)(() => grdTerminados.Rows.Clear()));
            foreach (proceso pro in procesos)
            {
                grdTerminados.Invoke((MethodInvoker)(() => grdTerminados.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
            }
        }

        private void imprimirListos(Queue<proceso> procesos, string algoritmoNombre)
        {
            grdProcesosListos.Invoke((MethodInvoker) (() => grdProcesosListos.Rows.Clear()));
            foreach (proceso pro in procesos)
            {

                if (algoritmoNombre.Equals("Round Robin"))
                {
                    grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
                }else if (algoritmoNombre.Equals("Por Sorteo"))
                {
                    grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString(), pro.tikects, pro.probabilidad.ToString() + "%" )));
                }

            }
        }

        private void imprimirBloqueados(Queue<proceso> procesos)
        {
            grdBloqueados.Invoke((MethodInvoker)(() => grdBloqueados.Rows.Clear()));
            foreach (proceso pro in procesos)
            {
                grdBloqueados.Invoke((MethodInvoker)(() => grdBloqueados.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
            }
        }

        private void Menu_Load(object sender, EventArgs e)
        {

        }
    }
}
