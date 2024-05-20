using System;
using System.Globalization;
using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private int tiempoMuestreo = 100;
        private bool graficaPausada = false;

        public Form1()
        {
            InitializeComponent();
            // Inicializacion evento Load
            this.Load += Form1_Load;

            grafica1.ChartAreas[0].AxisX.LabelStyle.Format = "mm.ss";
            grafica1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            grafica1.ChartAreas[0].AxisX.Interval = 2;

            btnPausar.Click += new EventHandler(btnPausar_Click);
            btnReanudar.Click += new EventHandler(btnReanudar_Click);
            btnLimpiar.Click += new EventHandler(btnLimpiar_Click);

            timer = new Timer();
            timer.Interval = tiempoMuestreo;
            timer.Tick += Timer_Tick;

            System.Management.ManagementEventWatcher watcher = new System.Management.ManagementEventWatcher();
            watcher.EventArrived += new System.Management.EventArrivedEventHandler(DeviceChangeEventHandler);
            watcher.Query = new System.Management.WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 or EventType = 3");
            watcher.Start();
        }
        private void DeviceChangeEventHandler(object sender, System.Management.EventArrivedEventArgs e)
        {

            this.Invoke((MethodInvoker)delegate
            {
                ActualizarPuertosCOM();
            });
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ActualizarPuertosCOM();
            combobox_baudrate.SelectedIndex = 3;
            btn_desconectar.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                serialPort1.Close();
            }
            catch { }
        }
        private void ActualizarPuertosCOM()
        {
            combobox_puertos.Items.Clear();
            string[] ports = SerialPort.GetPortNames();

            for (int i = 0; i < ports.Length; i++)
            {
                string port = ports[i];
                combobox_puertos.Items.Add(port);
            }

            if (combobox_puertos.Items.Count > 0)
            {
                combobox_puertos.SelectedIndex = 0;
            }
        }
        private void btn_conectar_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = combobox_puertos.Text;
                serialPort1.BaudRate = Convert.ToInt32(combobox_baudrate.Text);
                serialPort1.Open();
                text_estado.Text = "Estado: Conectado";
                img_estado.Image = Properties.Resources.marca_de_verificacion;
                btn_conectar.Enabled = false;
                btn_desconectar.Enabled = true;
                combobox_baudrate.Enabled = false;
                combobox_puertos.Enabled = false;
                timer.Start();

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void btn_desconectar_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();
                    text_estado.Text = "Estado: Desconectado";
                    img_estado.Image = Properties.Resources.eliminar;
                    btn_desconectar.Enabled = false;
                    btn_conectar.Enabled = true;
                    combobox_baudrate.Enabled = true;
                    combobox_puertos.Enabled = true;
                    timer.Stop();

                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_minimizer_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            while (serialPort1.IsOpen && serialPort1.BytesToRead > 0)
            {
                try
                {
                    string serialData = serialPort1.ReadLine();
                    string[] datosSeparados = serialData.Split(',');
                    int.TryParse(datosSeparados[0], NumberStyles.Any, CultureInfo.InvariantCulture, out int num1);
                    int.TryParse(datosSeparados[1], NumberStyles.Any, CultureInfo.InvariantCulture, out int num2);
                    int.TryParse(datosSeparados[2], NumberStyles.Any, CultureInfo.InvariantCulture, out int num3);
                    int.TryParse(datosSeparados[3], NumberStyles.Any, CultureInfo.InvariantCulture, out int num4);
                    int.TryParse(datosSeparados[4], NumberStyles.Any, CultureInfo.InvariantCulture, out int num5);
                    int.TryParse(datosSeparados[5], NumberStyles.Any, CultureInfo.InvariantCulture, out int num6);

                    progressBarPotencia.Invoke((MethodInvoker)(() =>
                    {
                        progressBarPotencia.Value = num1;
                    }));
                    progressBarVoltaje.Invoke((MethodInvoker)(() =>
                    {
                        progressBarVoltaje.Value = num2;
                    }));
                    progressBarFrecuencia.Invoke((MethodInvoker)(() =>
                    {
                        progressBarFrecuencia.Value = num3;
                    }));
                    progressBarEnergia.Invoke((MethodInvoker)(() =>
                    {
                        progressBarEnergia.Value = num4;
                    }));

                    grafica1.Invoke((MethodInvoker)(() =>
                    {
                        DateTime tiempoActual = DateTime.Now;
                        if (!graficaPausada)
                        {
                            grafica1.Series["F. Potencia"].Points.AddXY(tiempoActual, num5);
                            grafica1.Series["Voltaje"].Points.AddXY(tiempoActual, num6);
                        }

                    }));
                }
                catch { }
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                DateTime tiempoActual = DateTime.Now;
                grafica1.ChartAreas[0].AxisX.Minimum = tiempoActual.AddSeconds(-10).ToOADate();
                grafica1.ChartAreas[0].AxisX.Maximum = tiempoActual.ToOADate();
                grafica1.ChartAreas[0].RecalculateAxesScale();
                grafica1.Invalidate();
            }
            catch { }
        }

        private void btnPausar_Click(object sender, EventArgs e)
        {
            timer.Stop();
            graficaPausada = true;
        }

        private void btnReanudar_Click(object sender, EventArgs e)
        {
            timer.Start();
            graficaPausada = false;
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            grafica1.Series["Voltaje"].Points.Clear();
            grafica1.Series["F. Potencia"].Points.Clear();
        }

        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/EdgarM2237");
        }
    }
}
