using System.Windows.Forms;
using System.Drawing;
namespace TitaniumColector
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        public virtual Control ActiveControl
        {
            get
            {
                return GetFocusedControl(this);
            }
            set
            {
                if (!value.Focused)
                {
                    value.Focus();
                }
            }

        }

        private Control GetFocusedControl(Control parent)
        {
            if (parent.Focused)
            {
                return parent;
            }
            foreach (Control ctrl in parent.Controls)
            {
                Control temp = GetFocusedControl(ctrl);
                if (temp != null)
                {
                    return temp;
                }
            }
            return null;
        } 

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.panelFrmLogin = new System.Windows.Forms.Panel();
            this.lbDescricao = new System.Windows.Forms.Label();
            this.txtSenha = new System.Windows.Forms.TextBox();
            this.cbUsuario = new System.Windows.Forms.ComboBox();
            this.lbSenha = new System.Windows.Forms.Label();
            this.lbUsuario = new System.Windows.Forms.Label();
            this.btSair = new System.Windows.Forms.Button();
            this.btLogin = new System.Windows.Forms.Button();
            this.pboxFrmLogin = new System.Windows.Forms.PictureBox();
            this.ImgLogin = new System.Windows.Forms.ImageList();
            this.panelFrmLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelFrmLogin
            // 
            this.panelFrmLogin.Controls.Add(this.lbDescricao);
            this.panelFrmLogin.Controls.Add(this.txtSenha);
            this.panelFrmLogin.Controls.Add(this.cbUsuario);
            this.panelFrmLogin.Controls.Add(this.lbSenha);
            this.panelFrmLogin.Controls.Add(this.lbUsuario);
            this.panelFrmLogin.Controls.Add(this.btSair);
            this.panelFrmLogin.Controls.Add(this.btLogin);
            this.panelFrmLogin.Controls.Add(this.pboxFrmLogin);
            this.panelFrmLogin.Location = new System.Drawing.Point(0, 0);
            this.panelFrmLogin.Name = "panelFrmLogin";
            this.panelFrmLogin.Size = new System.Drawing.Size(345, 455);
            // 
            // lbDescricao
            // 
            this.lbDescricao.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.lbDescricao.Location = new System.Drawing.Point(30, 20);
            this.lbDescricao.Name = "lbDescricao";
            this.lbDescricao.Size = new System.Drawing.Size(100, 20);
            // 
            // txtSenha
            // 
            this.txtSenha.Location = new System.Drawing.Point(85, 296);
            this.txtSenha.Name = "txtSenha";
            this.txtSenha.Size = new System.Drawing.Size(100, 23);
            this.txtSenha.TabIndex = 7;
            this.txtSenha.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSenha_KeyPress);
            // 
            // cbUsuario
            // 
            this.cbUsuario.Location = new System.Drawing.Point(85, 245);
            this.cbUsuario.Name = "cbUsuario";
            this.cbUsuario.Size = new System.Drawing.Size(100, 23);
            this.cbUsuario.TabIndex = 6;
            this.cbUsuario.LostFocus += new System.EventHandler(this.cbUsuario_LostFocus);
            this.cbUsuario.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbUsuario_KeyPress);
            this.cbUsuario.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbUsuario_KeyUp);
            this.cbUsuario.GotFocus += new System.EventHandler(this.cbUsuario_GotFocus);
            // 
            // lbSenha
            // 
            this.lbSenha.Location = new System.Drawing.Point(25, 299);
            this.lbSenha.Name = "lbSenha";
            this.lbSenha.Size = new System.Drawing.Size(52, 20);
            this.lbSenha.Text = "Senha :";
            // 
            // lbUsuario
            // 
            this.lbUsuario.Location = new System.Drawing.Point(19, 247);
            this.lbUsuario.Name = "lbUsuario";
            this.lbUsuario.Size = new System.Drawing.Size(62, 20);
            this.lbUsuario.Text = "Usuário :";
            // 
            // btSair
            // 
            this.btSair.Location = new System.Drawing.Point(182, 354);
            this.btSair.Name = "btSair";
            this.btSair.Size = new System.Drawing.Size(72, 20);
            this.btSair.TabIndex = 2;
            this.btSair.Text = "Sair";
            this.btSair.Click += new System.EventHandler(this.btSair_Click);
            // 
            // btLogin
            // 
            this.btLogin.Location = new System.Drawing.Point(85, 354);
            this.btLogin.Name = "btLogin";
            this.btLogin.Size = new System.Drawing.Size(72, 20);
            this.btLogin.TabIndex = 1;
            this.btLogin.Text = "Login";
            this.btLogin.Click += new System.EventHandler(this.btLogin_Click);
            // 
            // pboxFrmLogin
            // 
            this.pboxFrmLogin.Location = new System.Drawing.Point(0, 0);
            this.pboxFrmLogin.Name = "pboxFrmLogin";
            this.pboxFrmLogin.Size = new System.Drawing.Size(345, 77);
            this.ImgLogin.Images.Clear();
            this.ImgLogin.Images.Add(((System.Drawing.Image)(resources.GetObject("resource"))));
            this.ImgLogin.Images.Add(((System.Drawing.Image)(resources.GetObject("resource1"))));
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(346, 455);
            this.ControlBox = false;
            this.Controls.Add(this.panelFrmLogin);
            this.KeyPreview = true;
            this.Name = "frmLogin";
            this.Text = "Login";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmLogin_KeyDown);
            this.panelFrmLogin.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void configControls()
        {

            this.SuspendLayout();
            this.Size = new System.Drawing.Size(MainConfig.ScreenSize.Width, MainConfig.ScreenSize.Height);

            //
            //pboxFrmLogin
            //
            this.pboxFrmLogin.Location = new System.Drawing.Point(0, 0);
            this.pboxFrmLogin.Size = new System.Drawing.Size(this.Size.Width, 90);
            //Tamanho da Imagem a ser mostrada no Picture Box
            this.ImgLogin.ImageSize = new Size((int)(this.Size.Width), 89);
            this.pboxFrmLogin.BackColor = Color.Black;
            this.pboxFrmLogin.Image = ImgLogin.Images[1];
            this.FormBorderStyle = FormBorderStyle.None;
            //
            //pnFrmLogin  
            //
            this.panelFrmLogin.Size = new System.Drawing.Size(this.Size.Width, this.Size.Height - 20);
            this.panelFrmLogin.BackColor = System.Drawing.SystemColors.AppWorkspace;
            // 
            // Label Descrição
            // 
            this.lbDescricao.Font = MainConfig.FontMuitoGrandeBold;
            this.lbDescricao.Text = "Login";
            sizeString = MainConfig.sizeStringEmPixel(this.lbDescricao.Text, MainConfig.FontMuitoGrandeBold);
            this.lbDescricao.Size = new System.Drawing.Size((int)sizeString.Width,(int)sizeString.Height);
            this.lbDescricao.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            sizeString = MainConfig.sizeStringEmPixel(this.lbDescricao.Text, MainConfig.FontGrandeRegular);
            this.lbDescricao.Location = new System.Drawing.Point((int)(MainConfig.ScreenSize.Width / 2 - lbDescricao.Width / 2),
                                                                  this.panelFrmLogin.Location.Y + pboxFrmLogin.Size.Height);
            //
            //Label Usuário
            //
            this.lbUsuario.Font = MainConfig.FontMediaBold;
            this.lbUsuario.Size = new System.Drawing.Size(90, 35);
            this.lbUsuario.Text = "Usuário :";
            this.lbUsuario.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            sizeString = MainConfig.sizeStringEmPixel(this.lbUsuario.Text, MainConfig.FontGrandeRegular);
            this.lbUsuario.Location = new System.Drawing.Point((int)(MainConfig.intPositionX + 20),
                                                                  this.lbDescricao.Location.Y + 80);
            //
            //Label Senha
            //
            this.lbSenha.Font = MainConfig.FontMediaBold;
            this.lbSenha.Text = "Senha :";
            this.lbSenha.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            sizeString = MainConfig.sizeStringEmPixel(this.lbSenha.Text, MainConfig.FontGrandeRegular);
            this.lbSenha.Size = new System.Drawing.Size((int)sizeString.Width, (int)sizeString.Height);
            this.lbSenha.Location = new System.Drawing.Point((int)(this.lbUsuario.Location.X + 3),
                                                                   this.lbUsuario.Location.Y + 30);

            //
            //ComboBox Usuário
            //
            this.cbUsuario.Font = MainConfig.FontPequenaBold;
            sizeString = MainConfig.sizeStringEmPixel(this.lbSenha.Text, MainConfig.FontGrandeRegular);
            this.cbUsuario.Visible = true;
            this.cbUsuario.Size = new System.Drawing.Size(120, 27);
            this.cbUsuario.Location = new System.Drawing.Point((int)(this.lbUsuario.Location.X + this.lbUsuario.Size.Width),
                                                                   this.lbUsuario.Location.Y - 3);
            this.cbUsuario.DropDownStyle = ComboBoxStyle.DropDown;

            //
            // TextBox Senha
            //
            this.txtSenha.Font = MainConfig.FontMediaBold;
            this.txtSenha.Text = "";
            this.txtSenha.MaxLength = 12;
            this.txtSenha.PasswordChar = MainConfig.PasswordChar;
            this.txtSenha.Visible = true;
            this.txtSenha.Size = new System.Drawing.Size(cbUsuario.Size.Width, 23);
            this.txtSenha.Location = new System.Drawing.Point((int)(this.cbUsuario.Location.X),
                                                                    this.lbSenha.Location.Y - 3);
            this.txtSenha.Enabled = false;

            //
            //Button Login 
            //
            this.btLogin.Font = MainConfig.FontMediaBold;
            this.btLogin.Visible = true;
            this.btLogin.Size = new System.Drawing.Size(72, 30);
            this.btLogin.Location = new System.Drawing.Point((int)(MainConfig.ScreenSize.Width / 2 - btLogin.Size.Width - 3),
                                                                    this.lbSenha.Location.Y + 40);

            //
            //Button Sair
            //
            this.btSair.Font = MainConfig.FontMediaBold;
            this.btSair.Visible = true;
            this.btSair.Size = new System.Drawing.Size(72, 30);
            this.btSair.Location = new System.Drawing.Point((int)(MainConfig.ScreenSize.Width / 2 + 3),
                                                                    this.lbSenha.Location.Y + 40);

            //
            //FrmLogin
            //
            Size size = new Size(this.ClientSize.Width, this.ClientSize.Height);
            MainConfig.defineClienteSize(size);
            this.cbUsuario.Focus();
            this.ResumeLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelFrmLogin;
        private System.Windows.Forms.PictureBox pboxFrmLogin;
        private System.Windows.Forms.ImageList ImgLogin;
        private System.Windows.Forms.Label lbSenha;
        private System.Windows.Forms.Label lbUsuario;
        private System.Windows.Forms.Button btSair;
        private System.Windows.Forms.Button btLogin;
        private System.Windows.Forms.TextBox txtSenha;
        private System.Windows.Forms.ComboBox cbUsuario;
        private System.Windows.Forms.Label lbDescricao;
    }
}

