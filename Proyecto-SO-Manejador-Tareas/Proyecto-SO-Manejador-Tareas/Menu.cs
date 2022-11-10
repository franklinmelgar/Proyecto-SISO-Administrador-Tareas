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
        private Thread hiloProcesoMultipleColas = null;
        private Thread hiloProcesoCPU = null;
        private Thread hiloProcesoPrioridad = null;

        Queue<proceso> colaProcesos = new Queue<proceso>();
        Queue<proceso> colaProcesosNivel2 = new Queue<proceso>();
        Queue<proceso> colaProcesosNivel3 = new Queue<proceso>();
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
            }
            else if (cmbAlgoritmo.Text.Equals("Por sorteo"))
            {
                ProcesoPorSorteo agregarProcesosSorteo = new ProcesoPorSorteo();
                AddOwnedForm(agregarProcesosSorteo);
                agregarProcesosSorteo.Show();

            }
            else if (cmbAlgoritmo.Text.Equals("Multiple Colas"))
            {
                ProcesoMiltipleCola agregarProcesosMultipleCola = new ProcesoMiltipleCola();
                AddOwnedForm(agregarProcesosMultipleCola);
                agregarProcesosMultipleCola.Show();

            }
            else if (cmbAlgoritmo.Text.Equals("CPU"))
            {
                ProcesoCPU agregarProcesoCPU = new ProcesoCPU();
                AddOwnedForm(agregarProcesoCPU);
                agregarProcesoCPU.Show();

            }
            else if (cmbAlgoritmo.Text.Equals("Prioridad"))
            {
                ProcesoPrioridad agregarProcesoPrioridad = new ProcesoPrioridad();
                AddOwnedForm(agregarProcesoPrioridad);
                agregarProcesoPrioridad.Show();

            }
            else if (cmbAlgoritmo.Text.Equals(""))
            {
                MessageBox.Show("Debe seleccionar un algoritmo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btIniciar_Click(object sender, EventArgs e)
        {

            generarColumnasGrid(cmbAlgoritmo.Text);

            hiloProcesoRoundRobin = new Thread(new ThreadStart(algoritmoRoundRobin));
            hiloProcesoPorSorteo = new Thread(new ThreadStart(algoritmoPorSorteo));
            hiloProcesoMultipleColas = new Thread(new ThreadStart(algoritmoMultipleCola));
            hiloProcesoCPU = new Thread(new ThreadStart(algoritmoCPU));
            hiloProcesoPrioridad = new Thread(new ThreadStart(algoritmoPrioridad));

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
                }else if (cmbAlgoritmo.Text.Equals("Multiple Colas"))
                {
                    hiloProcesoMultipleColas.Start();
                }
                else if (cmbAlgoritmo.Text.Equals("CPU"))
                {
                    hiloProcesoCPU.Start();
                }
                else if (cmbAlgoritmo.Text.Equals("Prioridad"))
                {
                    hiloProcesoPrioridad.Start();
                }
            }
            
        }

        private void algoritmoPrioridad()
        {
            imprimirListos(colaProcesos, "Prioridad");
            //int menor = 0;
            int menor = 10000000;
            string nombreProceso = "";
            int id = 0;

            do
            {

                foreach (proceso pro in colaProcesos)
                {
                    if (pro.prioridad < menor)
                    {
                        menor = pro.prioridad;
                        nombreProceso = pro.nombre;
                        id = pro.id;
                    }
                }

                //segundo ciclo solo para poner el menor en la primera posicion
                proceso itemProceso;
                proceso itemProcesoLeido;

                string nombre2 = "";
                do
                {
                    itemProceso = colaProcesos.Peek();
                    if (itemProceso.id.Equals(id))
                    {
                        nombre2 = itemProceso.nombre;
                    }
                    else
                    {
                        itemProcesoLeido = colaProcesos.Dequeue();
                        colaProcesos.Enqueue(itemProcesoLeido);
                    }

                } while (nombre2 != nombreProceso);

                itemProceso = colaProcesos.Peek();
                txtID.Invoke((MethodInvoker)(() => txtID.Text = itemProceso.id.ToString()));
                txtNombreProceso.Invoke((MethodInvoker)(() => txtNombreProceso.Text = itemProceso.nombre));
                txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = itemProceso.cpu.ToString()));

                for (int i = quantum; i >= 1; i--)
                {
                    txtQuantum.Invoke((MethodInvoker)(() => txtQuantum.Text = i.ToString()));
                    txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = (int.Parse(txtCPU.Text) - 1).ToString()));
                    Thread.Sleep(tiempoms);
                }

                itemProcesoLeido = colaProcesos.Dequeue();
                if (itemProcesoLeido.cpu <= quantum)
                {
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";
                    colaProcesosTerminados.Enqueue(itemProcesoLeido);
                    imprimirTerminados(colaProcesosTerminados);
                    imprimirListos(colaProcesos, "CPU");
                    menor = 10000000;
                    nombreProceso = "";
                    id = 0;
                }
                else
                {
                    itemProcesoLeido.cpu -= quantum;
                    colaProcesos.Enqueue(itemProcesoLeido);
                    imprimirListos(colaProcesos, "CPU");
                    colaProcesosBloqueados.Enqueue(itemProcesoLeido);
                    imprimirBloqueados(colaProcesosBloqueados);
                }

            } while (colaProcesos.Count > 0);

            MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void algoritmoCPU()
        {
            imprimirListos(colaProcesos, "CPU");
            //int menor = 0;
            int menor = 10000000;
            string nombreProceso = "";
            int id = 0;

            do
            {

                foreach(proceso pro in colaProcesos)
                {
                    if (pro.cpu < menor)
                    {
                        menor = pro.cpu;
                        nombreProceso = pro.nombre;
                        id = pro.id;
                    }
                }

                //segundo ciclo solo para poner el menor en la primera posicion
                proceso itemProceso;
                proceso itemProcesoLeido;

                string nombre2 = "";
                do
                {
                    itemProceso = colaProcesos.Peek();
                    if (itemProceso.id.Equals(id)){
                        nombre2 = itemProceso.nombre;
                    }else
                    {
                        itemProcesoLeido = colaProcesos.Dequeue();
                        colaProcesos.Enqueue(itemProcesoLeido);
                    }

                } while (nombre2 != nombreProceso);

                itemProceso = colaProcesos.Peek();
                txtID.Invoke((MethodInvoker)(() => txtID.Text = itemProceso.id.ToString()));
                txtNombreProceso.Invoke((MethodInvoker)(() => txtNombreProceso.Text = itemProceso.nombre));
                txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = itemProceso.cpu.ToString()));

                for (int i = quantum; i >= 1; i--)
                {
                    txtQuantum.Invoke((MethodInvoker)(() => txtQuantum.Text = i.ToString()));
                    txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = (int.Parse(txtCPU.Text) - 1).ToString()));
                    Thread.Sleep(tiempoms);
                }

                itemProcesoLeido = colaProcesos.Dequeue();
                if (itemProcesoLeido.cpu <= quantum)
                {
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";
                    colaProcesosTerminados.Enqueue(itemProcesoLeido);
                    imprimirTerminados(colaProcesosTerminados);
                    imprimirListos(colaProcesos, "CPU");
                    menor = 10000000;
                    nombreProceso = "";
                    id = 0;
                }
                else
                {
                    itemProcesoLeido.cpu -= quantum;
                    colaProcesos.Enqueue(itemProcesoLeido);
                    imprimirListos(colaProcesos, "CPU");
                    colaProcesosBloqueados.Enqueue(itemProcesoLeido);
                    imprimirBloqueados(colaProcesosBloqueados);
                }

            } while (colaProcesos.Count > 0);

            MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    //colaProcesosBloqueados.Enqueue(itemProcesoLeido);
                    //imprimirBloqueados(colaProcesosBloqueados);
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
                            //colaProcesosBloqueados.Enqueue(itemProcesoLeido);
                            //imprimirBloqueados(colaProcesosBloqueados);
                        }
                    }

                    if (encontrado.Equals(false))
                    {
                        colaProcesos.Enqueue(itemProcesoLeido);
                    }

                } while (encontrado == false);                

            } while (colaProcesos.Count > 0);

            MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);


        }

        private void algoritmoMultipleCola()
        {
            imprimirListos(colaProcesos, "Round Robin");

            //Nivel 1
            do
            {
                proceso itemProceso = colaProcesos.Peek();
                txtID.Invoke((MethodInvoker)(() => txtID.Text = itemProceso.id.ToString()));
                txtNombreProceso.Invoke((MethodInvoker)(() => txtNombreProceso.Text = itemProceso.nombre));
                txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = itemProceso.cpu.ToString()));

                for (int i = quantum; i >= 1; i--)
                {
                    txtQuantum.Invoke((MethodInvoker)(() => txtQuantum.Text = i.ToString()));
                    txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = (int.Parse(txtCPU.Text) - 1).ToString()));
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
                    colaProcesosNivel2.Enqueue(itemProcesoLeido);
                    imprimirListos(colaProcesos, "Round Robin");
                    imprimirListosNivel2(colaProcesosNivel2);
                }

            } while (colaProcesos.Count > 0);

            //nivel 2
            do
            {
                proceso itemProceso2 = colaProcesosNivel2.Peek();
                txtID.Invoke((MethodInvoker)(() => txtID.Text = itemProceso2.id.ToString()));
                txtNombreProceso.Invoke((MethodInvoker)(() => txtNombreProceso.Text = itemProceso2.nombre));
                txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = itemProceso2.cpu.ToString()));

                for (int i = quantum; i >= 1; i--)
                {
                    txtQuantum.Invoke((MethodInvoker)(() => txtQuantum.Text = i.ToString()));
                    txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = (int.Parse(txtCPU.Text) - 1).ToString()));
                    Thread.Sleep(tiempoms);
                }

                proceso itemProcesoLeido = colaProcesosNivel2.Dequeue();
                if (itemProcesoLeido.cpu <= quantum)
                {
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";
                    colaProcesosTerminados.Enqueue(itemProcesoLeido);
                    imprimirTerminados(colaProcesosTerminados);
                    imprimirListos(colaProcesos, "Round Robin");
                    imprimirListosNivel2(colaProcesosNivel2);
                }
                else
                {
                    itemProcesoLeido.cpu -= quantum;
                    colaProcesosNivel3.Enqueue(itemProcesoLeido);
                    imprimirListos(colaProcesos, "Round Robin");
                    imprimirListosNivel2(colaProcesosNivel2);
                    imprimirListosNivel3(colaProcesosNivel3);
                }

            } while (colaProcesosNivel2.Count > 0);

            //Nivel 3
            do
            {
                proceso itemProceso3 = colaProcesosNivel3.Peek();
                txtID.Invoke((MethodInvoker)(() => txtID.Text = itemProceso3.id.ToString()));
                txtNombreProceso.Invoke((MethodInvoker)(() => txtNombreProceso.Text = itemProceso3.nombre));
                txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = itemProceso3.cpu.ToString()));

                for (int i = quantum; i >= 1; i--)
                {
                    txtQuantum.Invoke((MethodInvoker)(() => txtQuantum.Text = i.ToString()));
                    txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = (int.Parse(txtCPU.Text) - 1).ToString()));
                    Thread.Sleep(tiempoms);
                }

                proceso itemProcesoLeido = colaProcesosNivel3.Dequeue();
                if (itemProcesoLeido.cpu <= quantum)
                {
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";
                    colaProcesosTerminados.Enqueue(itemProcesoLeido);
                    imprimirTerminados(colaProcesosTerminados);
                    imprimirListos(colaProcesos, "Round Robin");
                    imprimirListosNivel2(colaProcesosNivel2);
                    imprimirListosNivel3(colaProcesosNivel3);
                }
                else
                {
                    itemProcesoLeido.cpu -= quantum;
                    colaProcesosNivel3.Enqueue(itemProcesoLeido);
                    imprimirListos(colaProcesos, "Round Robin");
                    imprimirListosNivel2(colaProcesosNivel2);
                    imprimirListosNivel3(colaProcesosNivel3);
                }

            } while (colaProcesosNivel3.Count > 0);

            MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                groupBox1.Invoke((MethodInvoker)(() => groupBox1.Size = new Size(400, 700)));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("Id", "Id")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("nombre", "Nombre Proceso")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("rafaga", "Rafaga CPU")));
                grupoTicket.Invoke((MethodInvoker)(() => grupoTicket.Visible = false));
                groupBox6.Invoke((MethodInvoker)(() => groupBox6.Visible = false));
                groupBox7.Invoke((MethodInvoker)(() => groupBox7.Visible = false));

            }
            else if (algoritmoNombre.Equals("Por sorteo"))
            {
                grupoTicket.Invoke((MethodInvoker)(() => grupoTicket.Visible = true));
                groupBox1.Invoke((MethodInvoker)(() => groupBox1.Size = new Size(400, 700)));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("Id", "Id")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("nombre", "Nombre Proceso")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("rafaga", "Rafaga CPU")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("tickets", "Tickets")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("probabilidad", "Probabilidad")));
                groupBox6.Invoke((MethodInvoker)(() => groupBox6.Visible = false));
                groupBox7.Invoke((MethodInvoker)(() => groupBox7.Visible = false));
            }
            else if (algoritmoNombre.Equals("Multiple Colas"))
            {
                grupoTicket.Invoke((MethodInvoker)(() => grupoTicket.Visible = false));
                groupBox1.Invoke((MethodInvoker)(() => groupBox1.Size = new Size(400, 200)));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("Id", "Id")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("nombre", "Nombre Proceso")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("rafaga", "Rafaga CPU")));
                groupBox6.Invoke((MethodInvoker)(() => groupBox6.Visible = true));
                groupBox6.Invoke((MethodInvoker)(() => groupBox6.Location = new Point(30, 270)));
                groupBox7.Invoke((MethodInvoker)(() => groupBox7.Visible = true));
                groupBox7.Invoke((MethodInvoker)(() => groupBox7.Location = new Point(30, 480)));
            }else if (algoritmoNombre.Equals("CPU")){
                groupBox1.Invoke((MethodInvoker)(() => groupBox1.Size = new Size(400, 700)));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("Id", "Id")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("nombre", "Nombre Proceso")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("rafaga", "Rafaga CPU")));
                grupoTicket.Invoke((MethodInvoker)(() => grupoTicket.Visible = false));
                groupBox6.Invoke((MethodInvoker)(() => groupBox6.Visible = false));
                groupBox7.Invoke((MethodInvoker)(() => groupBox7.Visible = false));
            }
            else if (algoritmoNombre.Equals("Prioridad"))
            {
                groupBox1.Invoke((MethodInvoker)(() => groupBox1.Size = new Size(400, 700)));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("Id", "Id")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("nombre", "Nombre Proceso")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("rafaga", "Rafaga CPU")));
                grupoTicket.Invoke((MethodInvoker)(() => grupoTicket.Visible = false));
                groupBox6.Invoke((MethodInvoker)(() => groupBox6.Visible = false));
                groupBox7.Invoke((MethodInvoker)(() => groupBox7.Visible = false));
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
                else if (algoritmoNombre.Equals("CPU"))
                {
                    grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
                }
                else if (algoritmoNombre.Equals("Prioridad"))
                {
                    grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
                }

            }
        }

        private void imprimirListosNivel2(Queue<proceso> procesos)
        {
            grdProcesosListosNivel2.Invoke((MethodInvoker)(() => grdProcesosListosNivel2.Rows.Clear()));
            foreach (proceso pro in procesos)
            {                
                    grdProcesosListosNivel2.Invoke((MethodInvoker)(() => grdProcesosListosNivel2.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
            }
        }

        private void imprimirListosNivel3(Queue<proceso> procesos)
        {
            grdProcesosListosNivel3.Invoke((MethodInvoker)(() => grdProcesosListosNivel3.Rows.Clear()));
            foreach (proceso pro in procesos)
            {
                grdProcesosListosNivel3.Invoke((MethodInvoker)(() => grdProcesosListosNivel3.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
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
