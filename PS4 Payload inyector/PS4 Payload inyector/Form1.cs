using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Diagnostics;
using PS4Lib;

namespace PS4_Payload_inyector
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        PS4API PS4 = new PS4API();
        public static string IP = "192.168.178.30";//IP Temporal
        public static string Puerto = "9020";//Puerto Temporal
        IniFile ini = new IniFile(Application.StartupPath + @"\config.ini");//archivo de configuracion
        public Form1()
        {
            InitializeComponent();
            DevExpress.Skins.SkinManager.EnableFormSkins();//habilitar skins devexpress
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("App Desarrollada por CYB3R");
            iptxt.Text = ini.IniReadValue("ps4", "ip");//lee y deja la IP del .ini
            puertotxt.Text = ini.IniReadValue("ps4", "puerto");//lee y deja puesto el puerto del .ini
        }
        string version;
        private void mButton1_Click(object sender, EventArgs e)
        {
            if (iptxt.Text == "")//Aqui se puede poner la IP de public static string IP "192.168.178.30"
            {
                MessageBox.Show("introduce la ip de tu PS4");
            }
            else
            {
                PS4.Notify(222, "PS4 Inyector Conectado :)");
                bool result = Connect2PS4(iptxt.Text, puertotxt.Text);
                lblestado.Text = "Conectado";
                lblestado.ForeColor = Color.LimeGreen;
                btconectar.ForeColor = Color.LimeGreen;
                if (!result)
                {
                    lblestado.Text = "Fallo!";
                    lblestado.ForeColor = Color.Red;
                    MessageBox.Show("Error\n" + Exception);
                }
            }
        }

        public static Socket _psocket;
        public static bool pDConnected;
        public static string Exception;
        public static bool Connect2PS4(string ip, string port)
        {
            try
            {
                _psocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _psocket.ReceiveTimeout = 3000;
                _psocket.SendTimeout = 3000;
                _psocket.Connect(new IPEndPoint(IPAddress.Parse(ip), Int32.Parse(port)));
                pDConnected = true;
                return true;
            }
            catch (Exception ex)
            {
                pDConnected = false;
                Exception = ex.ToString();
                return false;
            }
        }

        public static void SendPayload(string filename)
        {
            _psocket.SendFile(filename);
        }

        public static void DisconnectPayload()
        {
            pDConnected = false;
            _psocket.Close();
        }

        public static string path;
        private void mButton2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            path = openFileDialog1.FileName;
            mButton2.Text = path;
        }

        private void mButton3_Click(object sender, EventArgs e)
        {
            try
            {
                SendPayload(path);              
            }
            catch (Exception ex)
            {
                lblenviado.Text = "Error";
                lblenviado.ForeColor = Color.Red;
                MessageBox.Show("Error al enviar payload!\n" + ex);
            }
            try
            {
                DisconnectPayload();
                lblenviado.Text = "Enviado";
                lblenviado.ForeColor = Color.LimeGreen;
                MessageBox.Show("Payload enviado!");
            }
            catch (Exception ex)
            {
                lblenviado.Text = "Error";
                lblenviado.ForeColor = Color.Red;
                MessageBox.Show("Error se desconectara!\n" + ex);
            }
        }

        private void mButton4_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("Puertos según el Firmware\n\nPuerto para 1.76 es 9023\nPuerto para 4.05 es 9020\nPuerto para 4.55 es 9020\nPuerto para 5.05 es 9020");
        }

        private void mButton5_Click(object sender, EventArgs e)
        {
            try
            {
                if (iptxt.Text == "")
                {
                    MessageBox.Show("Introduce una ip valida");
                }
                else
                {
                    ini.IniWriteValue("ps4", "ip", iptxt.Text);
                    ini.IniWriteValue("ps4", "puerto", puertotxt.Text);
                    MessageBox.Show("IP cambiada a: " + iptxt.Text);
                    MessageBox.Show("Puerto cambiado a: " + puertotxt.Text);
                    //this.Close();
                }
            }
            catch
            {
                MessageBox.Show("Error al cambiar la ip");
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)//donaciones paypal
        {
            string str = "";
            string str2 = "alcaponefst@gmail.com";
            string str3 = "Donacion";
            string str4 = "EU";
            string str5 = "EUR";
            string str6 = str;
            Process.Start(str6 + "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=" + str2 + "&lc=" + str4 + "&item_name=" + str3 + "&currency_code=" + str5 + "&bn=PP%2dDonationsBF");
        }

        private void rb455_CheckedChanged(object sender, EventArgs e)//puerto 4.55
        {
            puertotxt.Text = "9020";
        }

        private void rb405_CheckedChanged(object sender, EventArgs e)//puerto 4.05
        {
            puertotxt.Text = "9020";
        }

        private void rb176_CheckedChanged(object sender, EventArgs e)//puerto 1.76
        {
            puertotxt.Text = "9023";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)//puerto 5.05
        {
            puertotxt.Text = "9020";
        }
    }
}
