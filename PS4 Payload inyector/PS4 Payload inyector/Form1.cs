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
            DevExpress.XtraEditors.XtraMessageBox.Show("App v2 Desarrollada por CYB3R");
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

        private void pictureBox3_Click(object sender, EventArgs e)//traduccion al inglés
        {
            groupBox1.Text = "Connection";
            btconectar.Text = "Connect";
            mButton4.Text = "info ports";
            mButton5.Text = "Save IP and Port";
            groupBox2.Text = "Select and Send Payload";
            mButton2.Text = "Search Payload";
            mButton3.Text = "Send Payload";
            label1.Text = "Status:";
            lblestado.Text = "Not connected";
            lblenviado.Text = "Not sent";
            groupBox3.Text = "Language Selector";
        }

        private void pictureBox4_Click(object sender, EventArgs e)//traduccion en español
        {
            groupBox1.Text = "Conexión";
            btconectar.Text = "Conectar";
            mButton4.Text = "Info Puertos";
            mButton5.Text = "Guardar IP y Puerto";
            groupBox2.Text = "Seleccionar y Enviar Payload";
            mButton2.Text = "Buscar Payload";
            mButton3.Text = "Enviar Payload";
            label1.Text = "Estado:";
            lblestado.Text = "No Conectado";
            lblenviado.Text = "No Enviado";
            groupBox3.Text = "Selector de Idioma";
        }

        private void pictureBox5_Click(object sender, EventArgs e)//traduccion en frances
        {
            groupBox1.Text = "Connexion";
            btconectar.Text = "Connecter";
            mButton4.Text = "Info sur le port";
            mButton5.Text = "Salvar IP e Porta";
            groupBox2.Text = "Sélectionner et envoyer la charge utile";
            mButton2.Text = "Trouver Payload";
            mButton3.Text = "Envoyer Payload";
            label1.Text = "statut:";
            lblestado.Text = "Non connecté";
            lblenviado.Text = "Non soumis";
            groupBox3.Text = "Sélecteur de langue";
        }

        private void pictureBox6_Click(object sender, EventArgs e)//traduccion portuges
        {
            groupBox1.Text = "Conexão";
            btconectar.Text = "Conectar";
            mButton4.Text = "Info portas";
            mButton5.Text = "sauvegarder IP et le port";
            groupBox2.Text = "Selecione e envie o payload";
            mButton2.Text = "Buscar Payload";
            mButton3.Text = "Enviar Payload";
            label1.Text = "status:";
            lblestado.Text = "não conectado";
            lblenviado.Text = "não enviado";
            groupBox3.Text = "Seletor de Idiomas";
        }

        private void pictureBox7_Click(object sender, EventArgs e)//traduccion aleman
        {
            groupBox1.Text = "Verbindung";
            btconectar.Text = "verbinden";
            mButton4.Text = "Info-Ports";
            mButton5.Text = "Speichern Sie IP und Port";
            groupBox2.Text = "Nutzlast auswählen und senden";
            mButton2.Text = "finden Payload";
            mButton3.Text = "senden Payload";
            label1.Text = "Status:";
            lblestado.Text = "nicht verbunden";
            lblenviado.Text = "nicht gesendet";
            groupBox3.Text = "Sprachauswahl";
        }
    }
}
