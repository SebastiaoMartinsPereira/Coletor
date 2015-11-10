using System;
using System.Drawing;
using System.Net;
using System.Collections.Generic;
using System.Windows.Forms;
using TitaniumColector.Classes;
using TitaniumColector.Classes.Utility;
using TitaniumColector.Classes.Dao;

namespace TitaniumColector
{
    public static class MainConfig
    {

        private static string strVersaoSO;
        public static string HostName { get; set; }
        public static string DeviceIp { get; set; }
        private static string strUsuarioLogado;
        private static int intUsuarioLogado;
        private static Size screenSize;
        private static Size clienteSize;
        private static Int64 intCodigoAcesso;
        private static Usuario userOn;

        //Fontes Utiizadas no sistema.
        private static Font fontPequenaRegular;
        private static Font fontPequenaBold;
        private static Font fontPequenaItalic;
        private static Font fontPequenaItalicBold;

        private static Font fontPadraoRegular;
        private static Font fontPadraoBold;
        private static Font fontPadraoItalic;
        private static Font fontPadraoItalicBold;

        private static Font fontMediaBold;
        private static Font fontMuitoGrandeBold;

        private static Font fontGrandeRegular;
        private static Font fontGrandeBold;
        private static Font fontGrandeItalic;
        private static Single tamanho;
        private static FontStyle FontStyle;
        private static Permissoes permissoes_TB1210;

        //Contantes
        public const int intPositionX = 0;
        public const int intPositionY = 0;
        public const Char PasswordChar = '*';

        //Permissões do sistema.
        private static Boolean validarSequenciaEtiqueta;


        #region "GET & SETS"
        

        public static string VersaoSO
        {
            get {  return strVersaoSO != null ? strVersaoSO : "ND"; }

            set
            {
                if (value != null)
                {
                    strVersaoSO = value.Trim();
                }
            }
        }

        public static string UsuarioLogado 
        { 
            get {  return strUsuarioLogado; }
            set 
            {
                if (!(String.IsNullOrEmpty(value)))
                {
                    strUsuarioLogado = (string)value;
                }
            }
        }

        public static int CodigoUsuarioLogado
        {
            get { return intUsuarioLogado;}
            set { MainConfig.intUsuarioLogado = value;}
        }

        public static Font FontPequenaRegular
        {
            get { return MainConfig.fontPequenaRegular; }
            set { MainConfig.fontPequenaRegular = value; }
        }


        public static Font FontPequenaBold
        {
            get { return MainConfig.fontPequenaBold; }
            set { MainConfig.fontPequenaBold = value; }
        }


        public static Font FontPequenaItalic
        {
            get { return MainConfig.fontPequenaItalic; }
            set { MainConfig.fontPequenaItalic = value; }
        }

        public static Font FontPequenaItalicBold
        {
            get { return MainConfig.fontPequenaItalicBold; }
            set { MainConfig.fontPequenaItalicBold = value; }
        }

        public static Font FontMediaBold
        {
            get { return MainConfig.fontMediaBold; }
            set { MainConfig.fontMediaBold = value; }
        }

        public static Font FontPadraoRegular
        {
            get  {  return fontPadraoRegular; }
            set  { fontPadraoRegular = value; }
        }

        public static Font FontPadraoBold
        {
            get {  return fontPadraoBold; }
            set { fontPadraoBold = value; }
        }

        public static Font FontPadraoItalic
        {
            get { return MainConfig.fontPadraoItalic; }
            set { MainConfig.fontPadraoItalic = value; }
        }

        public static Font FontPadraoItalicBold
        {
            get { return MainConfig.fontPadraoItalicBold; }
            set { MainConfig.fontPadraoItalicBold = value; }
        }

        public static Font FontGrandeRegular
        {
            get { return fontGrandeRegular; }
            set { fontGrandeRegular = value; }
        }

        public static Font FontGrandeBold
        {
            get { return MainConfig.fontGrandeBold; }
            set { MainConfig.fontGrandeBold = value; }
        }

        public static Font FontGrandeItalic
        {
            get { return MainConfig.fontGrandeItalic; }
            set { MainConfig.fontGrandeItalic = value; }
        }

        public static Font FontMuitoGrandeBold
        {
            get { return MainConfig.fontMuitoGrandeBold; }
            set { MainConfig.fontMuitoGrandeBold = value; }
        }


        public static Int64 CodigoAcesso
        {
            get  {  return intCodigoAcesso; }
            set  {  intCodigoAcesso = value; }
        }

        public static Size ScreenSize
        {
            get { return MainConfig.screenSize; }
            set { MainConfig.screenSize = value; }
        }

        public static Size ClienteSize
        {
            get { return MainConfig.clienteSize; }
            set { MainConfig.clienteSize = value; }
        }

        internal static Usuario UserOn
        {
            get { return MainConfig.userOn; }
            set { MainConfig.userOn = value; }
        }

        internal static Permissoes Permissoes_TB1210
        {
            get { return MainConfig.permissoes_TB1210; }
            set { MainConfig.permissoes_TB1210 = value; }
        }

        #endregion

        #region "MÉTODOS DE CONFIGURAÇÃO"

        public static void setMainConfigurations()
        {
            defineScreenSize();
            capturaVersãoSo();
            capturaHostName();
            capturaIp();
            defineFontPequena();
            defineFontMedia();
            defineFontPadrao();
            defineFontGrande();
            defineFontMuitoGrande();
        }

        private static void defineScreenSize() 
        {
            Size size = new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height);
            ScreenSize = (size);
        }

        public static void defineClienteSize(Size size)
        {
            ClienteSize = (size);
        }

        private static void capturaVersãoSo()
        {
            VersaoSO  = Environment.OSVersion.ToString();
        }

        private static void capturaHostName()
        {
           string hostName = System.Net.Dns.GetHostName();
           HostName = hostName;
        }

        private static void capturaIp()
        {
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(HostName);
            IPAddress addr = ipEntry.AddressList[ipEntry.AddressList.Length-1];
            DeviceIp = addr.ToString() ;
        }

        public static Boolean ValidarSequenciaEtiqueta
        {
            get { return MainConfig.validarSequenciaEtiqueta; }
            set { MainConfig.validarSequenciaEtiqueta = value; }
        }

        public static void recuperaPermissoes() 
        {
            DaoPermissoes daoPermissoes = new DaoPermissoes();
            Permissoes_TB1210 = new Permissoes();
            Permissoes_TB1210 = daoPermissoes.recuperarPermissoes(Permissoes_TB1210.ListCodigoParametro);
        }

        private static void defineFontPadrao()
        {
            //REGULAR
            tamanho = new Single();
            tamanho = 10F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Regular;
            FontPadraoRegular = new System.Drawing.Font("Arial", tamanho, FontStyle);

            //BOLD
            tamanho = new Single();
            tamanho = 10F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontPadraoBold = new System.Drawing.Font("Arial", tamanho, FontStyle);

            //ITALIC
            tamanho = new Single();
            tamanho = 10F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Italic;
            FontPadraoItalic = new System.Drawing.Font("Arial", tamanho, FontStyle);

            //ITALIC & BOLD
            tamanho = new Single();
            tamanho = 10F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold | FontStyle.Italic;
            FontPadraoItalicBold = new System.Drawing.Font("Arial", tamanho, FontStyle);

        }

        private static void defineFontMedia()
        {
            //BOLD
            tamanho = new Single();
            tamanho = 12F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontMediaBold = new System.Drawing.Font("Arial", tamanho, FontStyle);

        }

        private static void defineFontMuitoGrande()
        {
            //BOLD
            tamanho = new Single();
            tamanho = 31F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontMuitoGrandeBold = new System.Drawing.Font("Arial", tamanho, FontStyle);

        }

        private static void defineFontPequena()
        {
            //REGULAR
            tamanho= new Single();
            tamanho = 8F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Regular;
            FontPequenaRegular = new System.Drawing.Font("Arial", tamanho, FontStyle);

            //ITALIC
            tamanho= new Single();
            tamanho = 8F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Italic;
            FontPequenaItalic = new System.Drawing.Font("Arial", tamanho, FontStyle);

            //BOLD
            tamanho= new Single();
            tamanho = 8F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontPequenaBold = new System.Drawing.Font("Arial", tamanho, FontStyle);

            //ITALIC & BOLD
            tamanho = new Single();
            tamanho = 8F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold | FontStyle.Italic ;
            FontPequenaItalicBold = new System.Drawing.Font("Arial", tamanho, FontStyle);

        }

        private static void defineFontGrande()
        {
            //REGULAR 
            tamanho= new Single();
            tamanho = 20F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Regular;
            FontGrandeRegular = new System.Drawing.Font("Arial", tamanho, FontStyle);

            //BOLD
            tamanho= new Single();
            tamanho = 20F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Bold;
            FontGrandeBold = new System.Drawing.Font("Arial", tamanho, FontStyle);

            //Italic
            tamanho= new Single();
            tamanho = 20F;
            FontStyle = new FontStyle();
            FontStyle = FontStyle.Italic;
            FontGrandeItalic = new System.Drawing.Font("Arial", tamanho, FontStyle);
        }

        #endregion

        public static SizeF sizeStringEmPixel(string text,Font font)
        {
            SizeF size = new SizeF();
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(new Bitmap(1, 1)))
            {
               size = graphics.MeasureString(text ,font);
            }
            return size;
        }

        /// <summary>
        /// Carregar Combox a partir de um List carregado com um tipo de objeto.
        /// </summary>
        /// <param name="cb">ComboBox a ser preenchida</param>
        /// <param name="objectList">List preenchida com um tipo de Objeto</param>
        /// <param name="displayName">parâmetro do objeto a ser mostrado na ComboBox</param>
        /// <param name="columnName">parâmetro que terá o seu Valor utilizado na ComboBox. </param>
        public static void carregarComboBox(System.Windows.Forms.ComboBox cb, List<object> objectList, string displayName, string columnName)
        {
            cb.Items.Clear();
            cb.DataSource = objectList;
            cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            cb.DisplayMember = displayName;
            cb.ValueMember = columnName;
            cb.SelectedItem = null;
        }

        /// <summary>
        /// Recupera o controle ao qual está com o focus ativo.
        /// </summary>
        /// <param name="parent">Formulário ao qual pertence o controle</param>
        /// <returns>Controle no qual o foco está ativo.</returns>
        public static Control GetFocusedControl(Control parent)
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

        /// <summary>
        /// Exibe uma mensagem padronizada ao usuário.
        /// </summary>
        /// <param name="msg">Informação a ser passada</param>
        /// <param name="head">Cabeçalho da caixa de texto.</param>
        public static void errorMessage(String msg,String head) 
        {
            System.Windows.Forms.MessageBox.Show(msg, head, System.Windows.Forms.MessageBoxButtons.OK,
                               System.Windows.Forms.MessageBoxIcon.Exclamation, System.Windows.Forms.MessageBoxDefaultButton.Button1);
        }

        // Create a bitmap object and fill it with the specified color.   
        // To make it look like a custom image, draw an ellipse in it.
        public static Bitmap MakeBitmap(Color color, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(color), 0, 0, bmp.Width, bmp.Height);
            g.DrawEllipse(new Pen(Color.DarkGray), 3, 3, width - 6, height - 6);
            g.Dispose();

            return bmp;
        }

    }
}
