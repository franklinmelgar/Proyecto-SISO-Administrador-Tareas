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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        public int llegada;
        public Color color;
        public int antiguedad;
        public bool bitReferencia;
    }


    public partial class Menu : Form
    {

        private Thread hiloProcesoRoundRobin = null;
        private Thread hiloProcesoPorSorteo = null;
        private Thread hiloProcesoMultipleColas = null;
        private Thread hiloProcesoSNP = null;
        private Thread hiloProcesoCPU = null;
        private Thread hiloProcesoPrioridad = null;
        private Thread hiloFIFOMemoria = null;
        private Thread hiloSegundaOportunidad = null;


        Queue<proceso> colaProcesosIniciales = new Queue<proceso>();
        Queue<proceso> colaProcesos = new Queue<proceso>();
        Queue<proceso> colaProcesosNivel2 = new Queue<proceso>();
        Queue<proceso> colaProcesosNivel3 = new Queue<proceso>();
        Queue<proceso> colaProcesosTerminados = new Queue<proceso>();
        Queue<proceso> colaProcesosBloqueados = new Queue<proceso>();

        private List<Color> listaColores = new List<Color>()
        {
            Color.FromArgb(239, 154, 154),
            Color.FromArgb(186, 104, 200),
            Color.FromArgb(149, 117, 205),
            Color.FromArgb(100, 181, 246),
            Color.FromArgb(77, 208, 225),
            Color.FromArgb(129, 199, 132),
            Color.FromArgb(220, 231, 117),
            Color.FromArgb(255, 241, 118),
            Color.FromArgb(255, 183, 77),
            Color.FromArgb(255, 87, 34),
            Color.FromArgb(121, 85, 72),
            Color.FromArgb(158, 158, 158)
        };

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

            colaProcesosIniciales.Enqueue(itemColaProceso);

            imprimirIniciales(colaProcesosIniciales);

        }

        private void imprimirIniciales(Queue<proceso> procesos)
        {
            grdProcesosIniciales.Invoke((MethodInvoker)(() => grdProcesosIniciales.Rows.Clear()));
            foreach (proceso pro in procesos)
            {
                grdProcesosIniciales.Invoke((MethodInvoker)(() => grdProcesosIniciales.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString(), pro.prioridad.ToString())));
            }
        }

        public void agregarCola(int codigo, string nombre, int cpu, int llegada)
        {
            proceso itemColaProceso = new proceso();
            itemColaProceso.id = codigo;
            itemColaProceso.nombre = nombre;
            itemColaProceso.cpu = cpu;
            itemColaProceso.estado = "Listo";
            itemColaProceso.llegada = llegada;

            colaProcesos.Enqueue(itemColaProceso);
        }

        private void btCargar_Click(object sender, EventArgs e)
        {
            if (cmbAlgoritmo.Text.Equals(""))
            {
                MessageBox.Show("Debe seleccionar un algoritmo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Clear()));
                grdBloqueados.Invoke((MethodInvoker)(() => grdBloqueados.Rows.Clear()));
                grdTerminados.Invoke((MethodInvoker)(() => grdTerminados.Rows.Clear()));
                colaProcesos.Clear();
                colaProcesosTerminados.Clear();
                colaProcesosBloqueados.Clear();
                int conteo = grdProcesosIniciales.RowCount - 1;

                proceso itemColaProceso = new proceso();

                //Seleccionar los procesos 
                for (int x = 0; x < conteo; x++)
                {
                    if (grdProcesosIniciales.Rows[x].Selected)
                    {

                        itemColaProceso.id = int.Parse(grdProcesosIniciales.Rows[x].Cells[0].Value.ToString());
                        itemColaProceso.nombre = grdProcesosIniciales.Rows[x].Cells[1].Value.ToString();
                        itemColaProceso.cpu = int.Parse(grdProcesosIniciales.Rows[x].Cells[2].Value.ToString());
                        itemColaProceso.prioridad = int.Parse(grdProcesosIniciales.Rows[x].Cells[3].Value.ToString());
                        itemColaProceso.estado = "Listo";

                        colaProcesos.Enqueue(itemColaProceso);

                    }
                }
            }

            if (cmbAlgoritmo.Text.Equals("Round Robin"))
            {
                generarColumnasGrid("Round Robin");
                imprimirListos(colaProcesos, "Round Robin");
            }
            else if (cmbAlgoritmo.Text.Equals("Por sorteo"))
            {
                generarColumnasGrid("Por Sorteo");
                imprimirListos(colaProcesos, "Por Sorteo");
            }
            else if (cmbAlgoritmo.Text.Equals("Multiple Colas"))
            {
                generarColumnasGrid("Multiple Colas");
                imprimirListos(colaProcesos, "Multiple Colas");
            }
            else if (cmbAlgoritmo.Text.Equals("Prioridad"))
            {
                generarColumnasGrid("Prioridad");
                imprimirListos(colaProcesos, "Prioridad");
            }
            else if (cmbAlgoritmo.Text.Equals("CPU"))
            {
                generarColumnasGrid("CPU");
                imprimirListos(colaProcesos, "CPU");
            }

        }

        private void btIniciar_Click(object sender, EventArgs e)
        {

            //generarColumnasGrid(cmbAlgoritmo.Text);

            hiloProcesoRoundRobin = new Thread(new ThreadStart(algoritmoRoundRobin));
            hiloProcesoPorSorteo = new Thread(new ThreadStart(algoritmoPorSorteo));
            hiloProcesoMultipleColas = new Thread(new ThreadStart(algoritmoMultipleCola));
            hiloProcesoCPU = new Thread(new ThreadStart(algoritmoCPU));
            hiloProcesoPrioridad = new Thread(new ThreadStart(algoritmoPrioridad));
            hiloProcesoSNP = new Thread(new ThreadStart(algoritmoSNP));

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
                } else if (cmbAlgoritmo.Text.Equals("Por sorteo"))
                {
                    hiloProcesoPorSorteo.Start();
                } else if (cmbAlgoritmo.Text.Equals("Multiple Colas"))
                {
                    hiloProcesoMultipleColas.Start();
                }
                else if (cmbAlgoritmo.Text.Equals("SNP"))
                {
                    algoritmoSNP();
                }
                else if (cmbAlgoritmo.Text.Equals("Prioridad"))
                {
                    hiloProcesoPrioridad.Start();
                }
                else if (cmbAlgoritmo.Text.Equals("CPU"))
                {
                    hiloProcesoCPU.Start();
                }
            }
        }

        private void algoritmoPrioridad()
        {
            //imprimirListos(colaProcesos, "Prioridad");
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

            //MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void algoritmoCPU()
        {
            //imprimirListos(colaProcesos, "CPU");
            int menor = 10000000;
            string nombreProceso = "";
            int id = 0;

            do
            {

                foreach (proceso pro in colaProcesos)
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

            //MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void algoritmoRoundRobin()
        {

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
                    colaProcesos.Enqueue(itemProcesoLeido);
                    imprimirListos(colaProcesos, "Round Robin");
                    colaProcesosBloqueados.Enqueue(itemProcesoLeido);
                    imprimirBloqueados(colaProcesosBloqueados);
                }

            } while (colaProcesos.Count > 0);

            //MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void algoritmoPorSorteo()
        {
            Boolean encontrado;
            int cantidadProcesos;

            //asignar probabilidad
            asignarProbabilidad();
            cantidadTickets = int.Parse(txtCantidadTickets.Text);
            cantidadProcesos = colaProcesos.Count;

            if (cantidadTickets < cantidadProcesos)
            {
                MessageBox.Show("Cantidad de ticket es menor a la cantidad de procesos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
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
                    lblTicket.Invoke((MethodInvoker)(() => lblTicket.Text = ticket.ToString()));


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
                                colaProcesosBloqueados.Enqueue(itemProcesoLeido);
                                imprimirBloqueados(colaProcesosBloqueados);
                            }
                        }

                        if (encontrado.Equals(false))
                        {
                            colaProcesos.Enqueue(itemProcesoLeido);
                        }

                    } while (encontrado == false);

                } while (colaProcesos.Count > 0);

            }
        }

        private void algoritmoMultipleCola()
        {
            //imprimirListos(colaProcesos, "Round Robin");

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

            //MessageBox.Show("Procesos completos", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void algoritmoSNP()
        {
            imprimirListos(colaProcesos, "SNP");

            proceso itemProceso3 = colaProcesos.Peek();
            txtID.Invoke((MethodInvoker)(() => txtID.Text = itemProceso3.id.ToString()));
            txtNombreProceso.Invoke((MethodInvoker)(() => txtNombreProceso.Text = itemProceso3.nombre));
            txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = itemProceso3.cpu.ToString()));

            int primeroCPU = Convert.ToInt32(txtCPU.Text);

            for (int i = 0; i < primeroCPU; i--)
            {
                primeroCPU = primeroCPU - 1;
                //MessageBox.Show(primeroCPU.ToString());
                txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = primeroCPU.ToString()));
                if (txtCPU.Text == "0")
                {
                    proceso itemProcesoLeido = colaProcesos.Dequeue();
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";
                    colaProcesosTerminados.Enqueue(itemProcesoLeido);
                    imprimirTerminados(colaProcesosTerminados);
                    imprimirListos(colaProcesos, "SNP");
                    break;

                }
            }

            List<Proceso> procesos = new List<Proceso>();

            for (int i = 0; i < grdProcesosListos.Rows.Count - 1; i++)
            {
                Proceso prueba = new Proceso(grdProcesosListos.Rows[i].Cells[0].Value.ToString(),
                    grdProcesosListos.Rows[i].Cells[1].Value.ToString(),
                    Convert.ToInt32(grdProcesosListos.Rows[i].Cells[2].Value), i);

                procesos.Add(prueba);
            }

            procesos.Sort(delegate (Proceso a, Proceso b)
            {
                return a.rafaga.CompareTo(b.rafaga);
            });

            foreach (var li in procesos)
            {
                //MessageBox.Show(li.rafaga.ToString());
                txtID.Invoke((MethodInvoker)(() => txtID.Text = li.codigo));
                txtNombreProceso.Invoke((MethodInvoker)(() => txtNombreProceso.Text = li.nombre));
                txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = li.rafaga.ToString()));

                int primerCPU = Convert.ToInt32(txtCPU.Text);

                for (int i = 0; i < primerCPU; i--)
                {
                    primerCPU = primerCPU - 1;
                    //MessageBox.Show(primerCPU.ToString()+""+i.ToString());
                    txtCPU.Invoke((MethodInvoker)(() => txtCPU.Text = primerCPU.ToString()));

                    if (txtCPU.Text == "0")
                    {

                        grdTerminados.Invoke((MethodInvoker)(() => grdTerminados.Rows.Add(li.codigo, li.nombre, li.rafaga.ToString())));
                        break;
                    }
                    primerCPU = Convert.ToInt32(txtCPU.Text);
                }
            }
            grdProcesosListos.Rows.Clear();
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
            }
            else if (algoritmoNombre.Equals("SNP"))
            {
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("Id", "Id")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("nombre", "Nombre Proceso")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("rafaga", "Rafaga CPU")));
                grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Columns.Add("llegada", "Llegada")));
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
            if (algoritmoNombre.Equals("CPU"))
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
            grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Clear()));
            foreach (proceso pro in procesos)
            {

                if (algoritmoNombre.Equals("Round Robin"))
                {
                    grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
                } else if (algoritmoNombre.Equals("Por Sorteo"))
                {
                    grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString(), pro.tikects, pro.probabilidad.ToString() + "%")));
                }
                else if (algoritmoNombre.Equals("SNP"))
                {
                    grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString(), pro.llegada.ToString())));
                }
                else if (algoritmoNombre.Equals("Prioridad"))
                {
                    grdProcesosListos.Invoke((MethodInvoker)(() => grdProcesosListos.Rows.Add(pro.id.ToString(), pro.nombre, pro.cpu.ToString())));
                }
                else if (algoritmoNombre.Equals("CPU"))
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

        //Funciones para la memoria

        private void imprimirProcesosMemoria(Queue<proceso> procesos)
        {
            int ultimaLinea;
            grdColoresProcesos.Invoke((MethodInvoker)(() => grdColoresProcesos.Rows.Clear()));

            foreach (proceso pro in procesos)
            {
                grdColoresProcesos.Invoke((MethodInvoker)(() => grdColoresProcesos.Rows.Add(pro.nombre)));
                ultimaLinea = grdColoresProcesos.RowCount - 1;
                //grdColoresProcesos.Invoke((MethodInvoker)(() => grdColoresProcesos.Rows[ultimaLinea - 1].Cells[0].Style.BackColor = pro.color));
                //grdColoresProcesos.Invoke((MethodInvoker)(() => grdColoresProcesos.Rows[ultimaLinea - 1].Cells[0].Style.ForeColor = Color.WhiteSmoke));

            }
        }

        private void generarCantidadMarcosMemoria()
        {
            int cantidadMarcos = int.Parse(txtCantidadMarcos.Text);
            grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows.Clear()));

            for (int x = 1; x <= cantidadMarcos; x++)
            {
                grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows.Add("Marco " + x.ToString())));
            }

            grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows.Add("Fallos ")));
        }


        private void algortimoFIFOMemoria()
        {

            //generar lista con la cantidad de marcos
            List<proceso> listaMarcos = new List<proceso>();
            proceso procesoEnBlanco = new proceso();
            int posicionInicial = 0;
            int posicionSalida = 0;
            int cantidadMarcos = int.Parse(txtCantidadMarcos.Text);
            int posicionColumna = 1;
            bool fallo = false;
            bool vacioEncontrado = false;
            int conteoAntiguedad = 1;
            int ultimaAntiguedad = 0;

            for (int x = 1; x <= cantidadMarcos; x++)
            {
                procesoEnBlanco.nombre = "";
                listaMarcos.Add(procesoEnBlanco);
            }


            //Round Robbin para consumir los procesos
            do
            {
                fallo = false;
                vacioEncontrado = false;



                proceso itemProceso = colaProcesos.Peek();

                for (int i = quantum; i >= 1; i--)
                {
                    Thread.Sleep(tiempoms);
                }

                //insertar columna en el grid
                grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Columns.Add(itemProceso.nombre, itemProceso.nombre)));

                //agregar proceso al marco iniciales
                if (posicionInicial < cantidadMarcos)
                {
                    procesoEnBlanco.nombre = itemProceso.nombre;
                    procesoEnBlanco.antiguedad = conteoAntiguedad;
                    listaMarcos[posicionInicial] = procesoEnBlanco;
                    posicionInicial++;
                    conteoAntiguedad++;
                    fallo = true;
                }
                else
                {
                    //verificar si ya existe
                    bool encontrado = false;

                    for(int x = 0; x < listaMarcos.Count; x++)
                    {
                        if (listaMarcos[x].nombre.Equals(itemProceso.nombre))
                        {
                            posicionSalida = x;
                            encontrado = true;
                            break;
                        }
                    }

                    //sino se encontrado el proceso se elimina el primero en ingresar
                    if (encontrado.Equals(false))
                    {
                        fallo = true;
                        //buscar si hay vacios
                        for(int x = 0; x < listaMarcos.Count; x++)
                        {
                            if (listaMarcos[x].nombre.Equals(""))
                            {
                                vacioEncontrado = true;
                                posicionSalida = x;
                                break;
                            }
                        }

                        if (!vacioEncontrado)
                        {
                            //detectar cual es el mas antiguo
                            for (int x = 0; x < listaMarcos.Count; x++)
                            {
                                if (listaMarcos[x].antiguedad.Equals(ultimaAntiguedad + 1))
                                {
                                    posicionSalida = x;
                                    ultimaAntiguedad = listaMarcos[x].antiguedad;
                                    break;
                                }
                            }
                        }

                        //sustituir el mas antiguo
                        procesoEnBlanco.nombre = itemProceso.nombre;
                        procesoEnBlanco.antiguedad = conteoAntiguedad;
                        listaMarcos[posicionSalida] = procesoEnBlanco;
                    }

                }

                //imprimir los marcos
                for (int x = 0; x < listaMarcos.Count; x++)
                {
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[x].Cells[posicionColumna].Value = listaMarcos[x].nombre));
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[x].Cells[posicionColumna].Style.Alignment = DataGridViewContentAlignment.MiddleCenter));
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[x].Cells[posicionColumna].Style.BackColor = listaMarcos[x].color));
                    //grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[x].Cells[posicionColumna].Style.ForeColor = Color.WhiteSmoke));
                }

                if (fallo)
                { 
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[cantidadMarcos].Cells[posicionColumna].Value = "X"));
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[cantidadMarcos].Cells[posicionColumna].Style.Alignment = DataGridViewContentAlignment.MiddleCenter));
                }
                else
                {
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[cantidadMarcos].Cells[posicionColumna].Value = "-"));
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[cantidadMarcos].Cells[posicionColumna].Style.Alignment = DataGridViewContentAlignment.MiddleCenter));
                }        


                proceso itemProcesoLeido = colaProcesos.Dequeue();
                if (itemProcesoLeido.cpu <= quantum)
                {
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";

                    procesoEnBlanco.nombre = "";
                    listaMarcos[posicionSalida] = procesoEnBlanco;

                }
                else
                {
                    itemProcesoLeido.cpu -= quantum;
                    colaProcesos.Enqueue(itemProcesoLeido);
                    colaProcesosBloqueados.Enqueue(itemProcesoLeido);
                }

                posicionColumna++;

            } while (colaProcesos.Count > 0);
        }

        private void algortimoSegundaOportunidadMemoria()
        {

            //generar lista con la cantidad de marcos
            List<proceso> listaMarcos = new List<proceso>();
            proceso procesoEnBlanco = new proceso();
            int posicionInicial = 0;
            int posicionSalida = 0;
            int cantidadMarcos = int.Parse(txtCantidadMarcos.Text);
            int posicionColumna = 1;
            bool fallo = false;
            bool vacioEncontrado = false;
            int conteoAntiguedad = 1;
            int ultimaAntiguedad = 0;

            for (int x = 1; x <= cantidadMarcos; x++)
            {
                procesoEnBlanco.nombre = "";
                procesoEnBlanco.bitReferencia = true;
                listaMarcos.Add(procesoEnBlanco);
            }

            //Round Robbin para consumir los procesos
            do
            {
                fallo = false;
                vacioEncontrado = false;

                proceso itemProceso = colaProcesos.Peek();

                for (int i = quantum; i >= 1; i--)
                {
                    Thread.Sleep(tiempoms);
                }

                //insertar columna en el grid
                grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Columns.Add(itemProceso.nombre, itemProceso.nombre)));

                //agregar proceso al marco iniciales
                if (posicionInicial < cantidadMarcos)
                {
                    procesoEnBlanco.nombre = itemProceso.nombre;
                    procesoEnBlanco.antiguedad = conteoAntiguedad;
                    procesoEnBlanco.bitReferencia = true;
                    listaMarcos[posicionInicial] = procesoEnBlanco;
                    posicionInicial++;
                    conteoAntiguedad++;
                    fallo = true;
                }
                else
                {
                    //verificar si ya existe
                    bool encontrado = false;

                    for (int x = 0; x < listaMarcos.Count; x++)
                    {
                        if (listaMarcos[x].nombre.Equals(itemProceso.nombre))
                        {
                            procesoEnBlanco.nombre = listaMarcos[x].nombre;
                            procesoEnBlanco.bitReferencia = true;
                            listaMarcos[x] = procesoEnBlanco;
                            posicionSalida = x;
                            encontrado = true;
                            break;
                        }
                    }

                    //sino se encontrado el proceso se elimina el primero en ingresar
                    if (encontrado.Equals(false))
                    {
                        fallo = true;
                        //buscar si hay vacios
                        for (int x = 0; x < listaMarcos.Count; x++)
                        {
                            if (listaMarcos[x].nombre.Equals(""))
                            {
                                vacioEncontrado = true;
                                posicionSalida = x;
                                break;
                            }
                        }

                        if (!vacioEncontrado)
                        {
                            //detectar cual es el mas antiguo
                            for (int x = 0; x < listaMarcos.Count; x++)
                            {
                                if (listaMarcos[x].antiguedad.Equals(ultimaAntiguedad + 1))
                                {
                                    posicionSalida = x;
                                    ultimaAntiguedad = listaMarcos[x].antiguedad;
                                    break;
                                }
                            }

                            //apagar todos los demas 
                            for (int x = 0; x < listaMarcos.Count; x++)
                            {
                                if (posicionSalida != x)
                                {
                                    procesoEnBlanco.nombre = listaMarcos[x].nombre;
                                    procesoEnBlanco.bitReferencia = false;
                                    listaMarcos[x] = procesoEnBlanco;
                                }
                            }
                        }

                        //sustituir el mas antiguo
                        procesoEnBlanco.nombre = itemProceso.nombre;
                        procesoEnBlanco.antiguedad = conteoAntiguedad;
                        procesoEnBlanco.bitReferencia = true;
                        listaMarcos[posicionSalida] = procesoEnBlanco;
                    }

                }

                //imprimir los marcos
                for (int x = 0; x < listaMarcos.Count; x++)
                {
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[x].Cells[posicionColumna].Value = listaMarcos[x].nombre + "(" + listaMarcos[x].bitReferencia.ToString() + ")"));
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[x].Cells[posicionColumna].Style.Alignment = DataGridViewContentAlignment.MiddleCenter));
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[x].Cells[posicionColumna].Style.BackColor = listaMarcos[x].color));
                    //grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[x].Cells[posicionColumna].Style.ForeColor = Color.WhiteSmoke));
                }

                if (fallo)
                {
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[cantidadMarcos].Cells[posicionColumna].Value = "X"));
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[cantidadMarcos].Cells[posicionColumna].Style.Alignment = DataGridViewContentAlignment.MiddleCenter));
                }
                else
                {
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[cantidadMarcos].Cells[posicionColumna].Value = "-"));
                    grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows[cantidadMarcos].Cells[posicionColumna].Style.Alignment = DataGridViewContentAlignment.MiddleCenter));
                }


                proceso itemProcesoLeido = colaProcesos.Dequeue();
                if (itemProcesoLeido.cpu <= quantum)
                {
                    itemProcesoLeido.cpu = 0;
                    itemProcesoLeido.estado = "Terminado";

                    procesoEnBlanco.nombre = "";
                    listaMarcos[posicionSalida] = procesoEnBlanco;

                }
                else
                {
                    itemProcesoLeido.cpu -= quantum;
                    colaProcesos.Enqueue(itemProcesoLeido);
                    colaProcesosBloqueados.Enqueue(itemProcesoLeido);
                }

                posicionColumna++;

            } while (colaProcesos.Count > 0);
        }


        private void Menu_Load(object sender, EventArgs e)
        {           

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CargarProcesosIniciales pi = new CargarProcesosIniciales();
            AddOwnedForm(pi);
            pi.Show();
        }

        private void btCargarProcesosMemoria_Click(object sender, EventArgs e)
        {
            grdColoresProcesos.Invoke((MethodInvoker)(() => grdColoresProcesos.Rows.Clear()));
            grdMemoria.Invoke((MethodInvoker)(() => grdMemoria.Rows.Clear()));

            colaProcesos.Clear();
            int conteo = grdProcesosIniciales.RowCount - 1;
            int posicionColor = 0;

            proceso itemColaProceso = new proceso();

            //Seleccionar los procesos 
            for (int x = 0; x < conteo; x++)
            {
                if (grdProcesosIniciales.Rows[x].Selected)
                {

                    itemColaProceso.id = int.Parse(grdProcesosIniciales.Rows[x].Cells[0].Value.ToString());
                    itemColaProceso.nombre = grdProcesosIniciales.Rows[x].Cells[1].Value.ToString();
                    itemColaProceso.cpu = int.Parse(grdProcesosIniciales.Rows[x].Cells[2].Value.ToString());
                    itemColaProceso.prioridad = int.Parse(grdProcesosIniciales.Rows[x].Cells[3].Value.ToString());
                    itemColaProceso.estado = "Listo";
                    itemColaProceso.color = listaColores[posicionColor];
                    posicionColor++;
                    colaProcesos.Enqueue(itemColaProceso);
                }
            }

            imprimirProcesosMemoria(colaProcesos);

        }

        private void btIniciarSimuladorMemoria_Click(object sender, EventArgs e)
        {
            generarCantidadMarcosMemoria();

            hiloFIFOMemoria = new Thread(new ThreadStart(algortimoFIFOMemoria));
            hiloSegundaOportunidad = new Thread(new ThreadStart(algortimoSegundaOportunidadMemoria));


            if (txtQuantumGeneral.Text.Equals("") || txtTiempo.Text.Equals(""))
            {
                MessageBox.Show("Debe ingresar el quantum y tiempo en milisegundos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                quantum = int.Parse(txtQuantumGeneral.Text);
                tiempoms = int.Parse(txtTiempo.Text);

                if (cmbAlgoritmoMemoria.Text.Equals("FIFO"))
                {
                    hiloFIFOMemoria.Start();
                }
                else if (cmbAlgoritmoMemoria.Text.Equals("Segunda Oportunidad"))
                {
                    hiloSegundaOportunidad.Start();
                }
            }

        }
    }
}
