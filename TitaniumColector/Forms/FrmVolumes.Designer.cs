using System.Drawing;
using System;
namespace TitaniumColector.Forms
{
    partial class FrmVolumes
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        //private System.Windows.Forms.MainMenu mainMenu1;
        

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
            this.SuspendLayout();
             
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(290, 222);
            this.Location = new System.Drawing.Point(MainConfig.intPositionX + 18, MainConfig.intPositionY + 40);

            this.Name = "FrmVolumes";
            this.Text = "Gerenciar Volumes";
            this.ResumeLayout(false);   
        }


        private void configurarForm() 
        {

            this.listVolumes = new System.Windows.Forms.ListView();
            this.ColunaNome = new System.Windows.Forms.ColumnHeader();
            this.ColunaQuantidade = new System.Windows.Forms.ColumnHeader();
            this.ColunaAdd = new System.Windows.Forms.ColumnHeader();
            this.ColunaRemove = new System.Windows.Forms.ColumnHeader();
            this.listItem = new System.Windows.Forms.ListViewItem();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.panelInform = new System.Windows.Forms.Panel();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.lblPesoEmb = new System.Windows.Forms.Label();
            this.lblValPesoEmb = new System.Windows.Forms.Label();
            this.lblVolumes = new System.Windows.Forms.Label();
            this.lblValVolumes = new System.Windows.Forms.Label();
            this.lblPesoProd = new System.Windows.Forms.Label();
            this.lblValPesoProd = new System.Windows.Forms.Label();
            this.lblPesoTot = new System.Windows.Forms.Label();
            this.lblValPesoTot = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblValTotal = new System.Windows.Forms.Label();

            this.SuspendLayout();

            //
            // ListVolumes
            //
            this.listVolumes.FullRowSelect = true;
            this.listVolumes.Size = new System.Drawing.Size(210, 130);
            //this.listVolumes.Dock = System.Windows.Forms.DockStyle.Top;
            this.listVolumes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listVolumes.BackColor = System.Drawing.Color.Beige;
            this.listVolumes.Font = MainConfig.FontPadraoBold;
            this.listVolumes.Location = new System.Drawing.Point(MainConfig.intPositionX + 2, MainConfig.intPositionY + 2);
            this.listVolumes.View = System.Windows.Forms.View.Details;



            //
            //Colunas ListVolumes
            //
            this.listVolumes.Columns.Add("Codigo", 0, System.Windows.Forms.HorizontalAlignment.Center);
            this.listVolumes.Columns.Add("Embalagem", 90, System.Windows.Forms.HorizontalAlignment.Center);
            this.listVolumes.Columns.Add("Qtd", 40, System.Windows.Forms.HorizontalAlignment.Center);
            this.listVolumes.Columns.Add("Peso", 60, System.Windows.Forms.HorizontalAlignment.Center);

            //apenas para visualização no form (TESTE)

            //for (int i = 0; i < 10; i++)
            //{
            //    this.listItem = new System.Windows.Forms.ListViewItem();
            //    this.listItem.Text = " 12345";
            //    this.listItem.SubItems.Add("Caixa 10");
            //    this.listItem.SubItems.Add("99");
            //    this.listItem.SubItems.Add("99,999");
            //    this.listVolumes.Items.Add(this.listItem);
            //}

            //
            //painel de Botões
            //
            this.panelButtons.Location = new System.Drawing.Point(this.listVolumes.Location.X + this.listVolumes.Width + 1, this.listVolumes.Location.Y);
            this.panelButtons.Size = new System.Drawing.Size(this.Width - this.listVolumes.Width-6,this.listVolumes.Height);
            this.panelButtons.BackColor = System.Drawing.SystemColors.ControlLight;

            //
            //painel de Informações
            //
            this.panelInform.Location = new System.Drawing.Point(this.listVolumes.Location.X , this.listVolumes.Location.Y + this.listVolumes.Height-2);
            this.panelInform.Size = new System.Drawing.Size(this.Width, (int)(this.listVolumes.Height * ((double)68.5 / 100)));
            this.panelInform.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelInform.Dock = System.Windows.Forms.DockStyle.Bottom;
            //
            // Botões
            //             
            this.btnUp.Name = "btnUp";
            this.btnUp.Text = "+";
            this.btnUp.Font = MainConfig.FontGrandeBold;
            this.btnUp.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnUp.Size = new System.Drawing.Size(50, 50);
            this.btnUp.Location = new System.Drawing.Point((panelButtons.Width / 2) - (btnUp.Width/2),15);

            this.btnDown.Name = "btnDown";
            this.btnDown.Text = "-";
            this.btnDown.Font = MainConfig.FontGrandeBold;
            this.btnDown.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDown.Size = new System.Drawing.Size(50, 50);
            this.btnDown.Location = new System.Drawing.Point(this.btnUp.Location.X, this.btnUp.Location.Y + this.btnUp.Height + 5);

            //
            // labels
            //
            this.lblVolumes.Text = "Volumes_:";
            this.lblVolumes.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lblVolumes.Text, MainConfig.FontPequenaBold);
            this.lblVolumes.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lblVolumes.Location = new Point(12, 6);
            this.lblVolumes.Text = "Volumes :";

            this.lblValVolumes.Text = "_000";
            this.lblValVolumes.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lblValVolumes.Text, MainConfig.FontPequenaBold);
            this.lblValVolumes.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lblValVolumes.Location = new Point(lblVolumes.Location.X + lblVolumes.Width + 15, lblVolumes.Location.Y);
            this.lblValVolumes.Text = "000";

            this.lblPesoProd.Text = "Produtos_:";
            this.lblPesoProd.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lblPesoProd.Text, MainConfig.FontPequenaBold);
            this.lblPesoProd.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lblPesoProd.Location = new Point(12, this.lblVolumes.Location.Y + this.lblVolumes.Height + 1);
            this.lblPesoProd.Text = String.Format("Produtos :");


            this.lblValPesoProd.Text = "1000,000_Kg";
            this.lblValPesoProd.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lblValPesoProd.Text, MainConfig.FontPequenaBold);
            this.lblValPesoProd.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            //this.lblValPesoProd.Location = new Point(lblVolumes.Location.X + lblPesoProd.Width + 12, lblPesoProd.Location.Y);
            this.lblValPesoProd.Location = new Point(this.lblValVolumes.Location.X, this.lblPesoProd.Location.Y);
            this.lblValPesoProd.Text = String.Format("1000,000 Kg");

            this.lblPesoEmb.Text = "Embalagem_:";
            this.lblPesoEmb.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lblPesoEmb.Text, MainConfig.FontPequenaBold);
            this.lblPesoEmb.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lblPesoEmb.Location = new Point(12, this.lblPesoProd.Location.Y + this.lblPesoProd.Height + 1);
            this.lblPesoEmb.Text = "Embalagens :";

            this.lblValPesoEmb.Text = "100,000_Kg";
            this.lblValPesoEmb.Font = MainConfig.FontPequenaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lblValPesoEmb.Text, MainConfig.FontPequenaBold);
            this.lblValPesoEmb.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            //this.lblValPesoEmb.Location = new Point(lblPesoEmb.Location.X + lblPesoEmb.Width + 2, lblPesoEmb.Location.Y);
            this.lblValPesoEmb.Location = new Point(this.lblValVolumes.Location.X, this.lblPesoEmb.Location.Y);
            this.lblValPesoEmb.Text = String.Format("100,000 Kg");

            this.lblTotal.Text = "Peso_Total_:";
            this.lblTotal.Font = MainConfig.FontMediaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lblTotal.Text, MainConfig.FontMediaBold);
            this.lblTotal.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lblTotal.Location = new Point(22, this.lblPesoEmb.Location.Y + this.lblPesoEmb.Height + 15);
            this.lblTotal.Text = "Peso Total :";

            this.lblValTotal.Text = "100,000_Kg";
            this.lblValTotal.Font = MainConfig.FontMediaBold;
            this.fontStringSize = MainConfig.sizeStringEmPixel(this.lblValTotal.Text,this.lblValTotal.Font);
            this.lblValTotal.Size = new Size((int)this.fontStringSize.Width, (int)this.fontStringSize.Height);
            this.lblValTotal.Location = new Point(lblTotal.Location.X + lblTotal.Width + 2, lblTotal.Location.Y);
            this.lblValTotal.Text = String.Format("100,000 Kg");


            //this.Menu = this.mainMenu1;
            this.panelButtons.Controls.Add(this.btnUp);
            this.panelButtons.Controls.Add(this.btnDown);
            this.panelInform.Controls.Add(this.lblVolumes);
            this.panelInform.Controls.Add(this.lblPesoEmb);
            this.panelInform.Controls.Add(this.lblPesoProd);
            this.panelInform.Controls.Add(this.lblValVolumes);
            this.panelInform.Controls.Add(this.lblValPesoEmb);
            this.panelInform.Controls.Add(this.lblValPesoProd);
            this.panelInform.Controls.Add(this.lblTotal);
            this.panelInform.Controls.Add(this.lblValTotal);
            this.Controls.Add(this.panelButtons);
            this.Controls.Add(this.panelInform);
            this.Controls.Add(this.listVolumes);

            this.ResumeLayout(false);

            //
            //Eventos
            //
            //Form
            this.Load += new EventHandler(FrmVolumes_Load);

            //Botões
            this.btnDown.Click += new EventHandler(btnDown_Click);
            this.btnUp.Click += new EventHandler(btnUp_Click);
        }


        private System.Windows.Forms.ListView listVolumes;
        private System.Windows.Forms.ColumnHeader ColunaNome;
        private System.Windows.Forms.ColumnHeader ColunaQuantidade;
        private System.Windows.Forms.ColumnHeader ColunaAdd;
        private System.Windows.Forms.ColumnHeader ColunaRemove;
        private System.Windows.Forms.ListViewItem listItem;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Panel panelInform;
        //private System.Windows.Forms.Button btnManejar;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Label lblVolumes;
        private System.Windows.Forms.Label lblValVolumes;
        private System.Windows.Forms.Label lblPesoProd;
        private System.Windows.Forms.Label lblValPesoProd;
        private System.Windows.Forms.Label lblPesoEmb;
        private System.Windows.Forms.Label lblValPesoEmb;
        private System.Windows.Forms.Label lblPesoTot;
        private System.Windows.Forms.Label lblValPesoTot;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblValTotal;
        private System.Drawing.SizeF fontStringSize;

        #endregion
    }
}