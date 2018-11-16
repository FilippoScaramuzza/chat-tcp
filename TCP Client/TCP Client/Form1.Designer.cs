namespace TCP_Client
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbChat = new System.Windows.Forms.ListBox();
            this.tbDestIp = new System.Windows.Forms.TextBox();
            this.tbMsg = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelIntro = new System.Windows.Forms.Label();
            this.lbDebug = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbChat
            // 
            this.lbChat.FormattingEnabled = true;
            this.lbChat.Location = new System.Drawing.Point(12, 41);
            this.lbChat.Name = "lbChat";
            this.lbChat.Size = new System.Drawing.Size(364, 121);
            this.lbChat.TabIndex = 0;
            // 
            // tbDestIp
            // 
            this.tbDestIp.Location = new System.Drawing.Point(116, 173);
            this.tbDestIp.Name = "tbDestIp";
            this.tbDestIp.Size = new System.Drawing.Size(260, 20);
            this.tbDestIp.TabIndex = 1;
            // 
            // tbMsg
            // 
            this.tbMsg.Location = new System.Drawing.Point(116, 199);
            this.tbMsg.Name = "tbMsg";
            this.tbMsg.Size = new System.Drawing.Size(260, 20);
            this.tbMsg.TabIndex = 2;
            this.tbMsg.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbMsg_KeyPress);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(263, 226);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(113, 23);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Invia";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 176);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP Destinatario";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Messaggio";
            // 
            // labelIntro
            // 
            this.labelIntro.AutoSize = true;
            this.labelIntro.Location = new System.Drawing.Point(12, 13);
            this.labelIntro.Name = "labelIntro";
            this.labelIntro.Size = new System.Drawing.Size(112, 13);
            this.labelIntro.TabIndex = 6;
            this.labelIntro.Text = "Nessuna connessione";
            // 
            // lbDebug
            // 
            this.lbDebug.FormattingEnabled = true;
            this.lbDebug.Location = new System.Drawing.Point(393, 41);
            this.lbDebug.Name = "lbDebug";
            this.lbDebug.Size = new System.Drawing.Size(231, 212);
            this.lbDebug.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(393, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Finestra di conferma";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 267);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbDebug);
            this.Controls.Add(this.labelIntro);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.tbMsg);
            this.Controls.Add(this.tbDestIp);
            this.Controls.Add(this.lbChat);
            this.Name = "Form1";
            this.Text = "Chat TCP";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbChat;
        private System.Windows.Forms.TextBox tbDestIp;
        private System.Windows.Forms.TextBox tbMsg;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelIntro;
        private System.Windows.Forms.ListBox lbDebug;
        private System.Windows.Forms.Label label3;
    }
}

