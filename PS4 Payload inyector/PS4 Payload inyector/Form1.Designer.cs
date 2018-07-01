namespace PS4_Payload_inyector
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.rb176 = new System.Windows.Forms.RadioButton();
            this.rb405 = new System.Windows.Forms.RadioButton();
            this.rb455 = new System.Windows.Forms.RadioButton();
            this.mButton5 = new MButton();
            this.mButton4 = new MButton();
            this.btconectar = new MButton();
            this.puertotxt = new System.Windows.Forms.TextBox();
            this.iptxt = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.mButton3 = new MButton();
            this.mButton2 = new MButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblestado = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblenviado = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.rb176);
            this.groupBox1.Controls.Add(this.rb405);
            this.groupBox1.Controls.Add(this.rb455);
            this.groupBox1.Controls.Add(this.mButton5);
            this.groupBox1.Controls.Add(this.mButton4);
            this.groupBox1.Controls.Add(this.btconectar);
            this.groupBox1.Controls.Add(this.puertotxt);
            this.groupBox1.Controls.Add(this.iptxt);
            this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.groupBox1.Location = new System.Drawing.Point(22, 162);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(350, 179);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conexión";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Font = new System.Drawing.Font("Tahoma", 7F);
            this.radioButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.radioButton1.Location = new System.Drawing.Point(243, 26);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(61, 21);
            this.radioButton1.TabIndex = 27;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "5.05";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // rb176
            // 
            this.rb176.AutoSize = true;
            this.rb176.Font = new System.Drawing.Font("Tahoma", 7F);
            this.rb176.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb176.Location = new System.Drawing.Point(42, 26);
            this.rb176.Name = "rb176";
            this.rb176.Size = new System.Drawing.Size(61, 21);
            this.rb176.TabIndex = 26;
            this.rb176.TabStop = true;
            this.rb176.Text = "1.76";
            this.rb176.UseVisualStyleBackColor = true;
            this.rb176.CheckedChanged += new System.EventHandler(this.rb176_CheckedChanged);
            // 
            // rb405
            // 
            this.rb405.AutoSize = true;
            this.rb405.Font = new System.Drawing.Font("Tahoma", 7F);
            this.rb405.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb405.Location = new System.Drawing.Point(109, 26);
            this.rb405.Name = "rb405";
            this.rb405.Size = new System.Drawing.Size(61, 21);
            this.rb405.TabIndex = 25;
            this.rb405.TabStop = true;
            this.rb405.Text = "4.05";
            this.rb405.UseVisualStyleBackColor = true;
            this.rb405.CheckedChanged += new System.EventHandler(this.rb405_CheckedChanged);
            // 
            // rb455
            // 
            this.rb455.AutoSize = true;
            this.rb455.Font = new System.Drawing.Font("Tahoma", 7F);
            this.rb455.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.rb455.Location = new System.Drawing.Point(176, 26);
            this.rb455.Name = "rb455";
            this.rb455.Size = new System.Drawing.Size(61, 21);
            this.rb455.TabIndex = 24;
            this.rb455.TabStop = true;
            this.rb455.Text = "4.55";
            this.rb455.UseVisualStyleBackColor = true;
            this.rb455.CheckedChanged += new System.EventHandler(this.rb455_CheckedChanged);
            // 
            // mButton5
            // 
            this.mButton5.ForeColor = System.Drawing.Color.White;
            this.mButton5.Location = new System.Drawing.Point(18, 129);
            this.mButton5.Name = "mButton5";
            this.mButton5.Size = new System.Drawing.Size(310, 32);
            this.mButton5.TabIndex = 7;
            this.mButton5.Text = "Guardar IP y Puerto";
            this.mButton5.Click += new System.EventHandler(this.mButton5_Click);
            // 
            // mButton4
            // 
            this.mButton4.ForeColor = System.Drawing.Color.Gray;
            this.mButton4.Location = new System.Drawing.Point(204, 93);
            this.mButton4.Name = "mButton4";
            this.mButton4.Size = new System.Drawing.Size(124, 27);
            this.mButton4.TabIndex = 6;
            this.mButton4.Text = "Info Puertos";
            this.mButton4.Click += new System.EventHandler(this.mButton4_Click);
            // 
            // btconectar
            // 
            this.btconectar.ForeColor = System.Drawing.Color.DarkRed;
            this.btconectar.Location = new System.Drawing.Point(204, 60);
            this.btconectar.Name = "btconectar";
            this.btconectar.Size = new System.Drawing.Size(124, 27);
            this.btconectar.TabIndex = 5;
            this.btconectar.Text = "Conectar";
            this.btconectar.Click += new System.EventHandler(this.mButton1_Click);
            // 
            // puertotxt
            // 
            this.puertotxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.puertotxt.ForeColor = System.Drawing.Color.White;
            this.puertotxt.Location = new System.Drawing.Point(19, 93);
            this.puertotxt.Name = "puertotxt";
            this.puertotxt.Size = new System.Drawing.Size(179, 27);
            this.puertotxt.TabIndex = 2;
            // 
            // iptxt
            // 
            this.iptxt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.iptxt.ForeColor = System.Drawing.Color.White;
            this.iptxt.Location = new System.Drawing.Point(19, 60);
            this.iptxt.Name = "iptxt";
            this.iptxt.Size = new System.Drawing.Size(179, 27);
            this.iptxt.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.mButton3);
            this.groupBox2.Controls.Add(this.mButton2);
            this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.groupBox2.Location = new System.Drawing.Point(22, 356);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(350, 120);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Seleccionar y Enviar Payload";
            // 
            // mButton3
            // 
            this.mButton3.ForeColor = System.Drawing.Color.White;
            this.mButton3.Location = new System.Drawing.Point(20, 68);
            this.mButton3.Name = "mButton3";
            this.mButton3.Size = new System.Drawing.Size(308, 29);
            this.mButton3.TabIndex = 6;
            this.mButton3.Text = "Enviar Payload";
            this.mButton3.Click += new System.EventHandler(this.mButton3_Click);
            // 
            // mButton2
            // 
            this.mButton2.ForeColor = System.Drawing.Color.White;
            this.mButton2.Location = new System.Drawing.Point(20, 33);
            this.mButton2.Name = "mButton2";
            this.mButton2.Size = new System.Drawing.Size(308, 29);
            this.mButton2.TabIndex = 6;
            this.mButton2.Text = "Buscar Payload";
            this.mButton2.Click += new System.EventHandler(this.mButton2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(22, 17);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(350, 129);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 489);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 19);
            this.label1.TabIndex = 19;
            this.label1.Text = "Estado:";
            // 
            // lblestado
            // 
            this.lblestado.AutoSize = true;
            this.lblestado.ForeColor = System.Drawing.Color.Red;
            this.lblestado.Location = new System.Drawing.Point(92, 490);
            this.lblestado.Name = "lblestado";
            this.lblestado.Size = new System.Drawing.Size(108, 19);
            this.lblestado.TabIndex = 20;
            this.lblestado.Text = "No Conectado";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 490);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 19);
            this.label3.TabIndex = 21;
            this.label3.Text = "Payload:";
            // 
            // lblenviado
            // 
            this.lblenviado.AutoSize = true;
            this.lblenviado.ForeColor = System.Drawing.Color.Red;
            this.lblenviado.Location = new System.Drawing.Point(269, 490);
            this.lblenviado.Name = "lblenviado";
            this.lblenviado.Size = new System.Drawing.Size(90, 19);
            this.lblenviado.TabIndex = 22;
            this.lblenviado.Text = "No Enviado";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Bin file (*.BIN)|*.BIN";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(131, 522);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(130, 34);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 23;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // Form1
            // 
            this.ActiveGlowColor = System.Drawing.Color.DodgerBlue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 584);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblenviado);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblestado);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Glow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Visual Studio 2013 Dark";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PS4 Inyector de Payloads";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private MButton btconectar;
        private System.Windows.Forms.TextBox puertotxt;
        private System.Windows.Forms.TextBox iptxt;
        private System.Windows.Forms.GroupBox groupBox2;
        private MButton mButton2;
        private MButton mButton3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblestado;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblenviado;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private MButton mButton4;
        private MButton mButton5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.RadioButton rb455;
        private System.Windows.Forms.RadioButton rb176;
        private System.Windows.Forms.RadioButton rb405;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}

