namespace TitaniumColector.Forms
{
    partial class FrmAcao
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.MainMenu menuFrmAcao;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnVenda = new System.Windows.Forms.Button();
            this.btnSaida = new System.Windows.Forms.Button();
            this.painelFrmAcao = new System.Windows.Forms.Panel();
            this.painelFrmAcao.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnVenda
            // 
            this.btnVenda.Location = new System.Drawing.Point(41, 39);
            this.btnVenda.Name = "btnVenda";
            this.btnVenda.Size = new System.Drawing.Size(265, 174);
            this.btnVenda.TabIndex = 0;
            this.btnVenda.Text = "Próxima Venda";
            this.btnVenda.Click += new System.EventHandler(this.btnVenda_Click);
            // 
            // btnSaida
            // 
            this.btnSaida.Location = new System.Drawing.Point(41, 241);
            this.btnSaida.Name = "btnSaida";
            this.btnSaida.Size = new System.Drawing.Size(265, 174);
            this.btnSaida.TabIndex = 1;
            this.btnSaida.Text = "Saída";
            this.btnSaida.Click += new System.EventHandler(this.btnSaida_Click);
            // 
            // painelFrmAcao
            // 
            this.painelFrmAcao.Controls.Add(this.btnVenda);
            this.painelFrmAcao.Controls.Add(this.btnSaida);
            this.painelFrmAcao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.painelFrmAcao.Location = new System.Drawing.Point(0, 0);
            this.painelFrmAcao.Name = "painelFrmAcao";
            this.painelFrmAcao.Size = new System.Drawing.Size(346, 455);
            // 
            // FrmAcao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(346, 455);
            this.ControlBox = false;
            this.Controls.Add(this.painelFrmAcao);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FrmAcao";
            this.Text = "FrmAcao";
            this.painelFrmAcao.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Configura todos os controles do formulário
        /// </summary>
        private void controlsConfig()
        {

            //
            //FrmAcao
            //
            this.Size = new System.Drawing.Size(MainConfig.ScreenSize.Width, MainConfig.ScreenSize.Height);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Text = "Próxima ação";

            //Menus 

            ////menuItem Opções
            this.mnuAcao_Opcoes = new System.Windows.Forms.MenuItem();
            this.mnuAcao_Opcoes.Text = "Opção";
            this.mnuAcao_Opcoes.Enabled = true;

            ////menuItem Exit
            this.mnuAcao_Exit = new System.Windows.Forms.MenuItem();
            this.mnuAcao_Exit.Text = "Exit";
            this.mnuAcao_Exit.Enabled = true;
            this.mnuAcao_Exit.Click += new System.EventHandler(mnuAcao_Exit_Click);
           

            ////MenuItem Logout
            this.mnuAcao_Logout = new System.Windows.Forms.MenuItem();
            this.mnuAcao_Logout.Text = "Logout";
            this.mnuAcao_Logout.Enabled = true;
            this.mnuAcao_Logout.Click += new System.EventHandler(mnuAcao_Logout_Click);
            //
            ////Adiciona os menus ao MenuPrincipal.
            //
            this.menuFrmAcao = new System.Windows.Forms.MainMenu();
            this.menuFrmAcao.MenuItems.Add(mnuAcao_Opcoes);
            this.mnuAcao_Opcoes.MenuItems.Add(this.mnuAcao_Exit);
            this.mnuAcao_Opcoes.MenuItems.Add(this.mnuAcao_Logout);
            this.Menu = this.menuFrmAcao;

            //
            //btnVenda
            //
            this.btnVenda.Location = new System.Drawing.Point(MainConfig.intPositionX + 20, MainConfig.intPositionY + 50);
            this.btnVenda.Size = new System.Drawing.Size(MainConfig.ScreenSize.Width - 40, MainConfig.ScreenSize.Height / 3);
            this.btnVenda.Text = "Próxima Venda";
            this.btnVenda.Font = MainConfig.FontGrandeBold;
            //
            //btnSaida
            //
            this.btnSaida.Location = new System.Drawing.Point(MainConfig.intPositionX + 20, btnVenda.Location.Y + btnVenda.Size.Height + 10);
            this.btnSaida.Size = new System.Drawing.Size(MainConfig.ScreenSize.Width - 40, MainConfig.ScreenSize.Height / 3);
            this.btnSaida.Text = "Próxima Compra";
            this.btnSaida.BackColor = System.Drawing.SystemColors.Control;
            this.btnSaida.Font = MainConfig.FontGrandeBold;

        }


        #endregion

        private System.Windows.Forms.MenuItem mnuAcao_Opcoes;
        private System.Windows.Forms.MenuItem mnuAcao_Exit;
        private System.Windows.Forms.MenuItem mnuAcao_Logout;
        private System.Windows.Forms.Button btnVenda;
        private System.Windows.Forms.Button btnSaida;
        private System.Windows.Forms.Panel painelFrmAcao;
    }
}