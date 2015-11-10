
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace TitaniumColector.Utility
{

    public class FileUtility
    {
        private string _pathMobile; //path diretório onde está salva a aplicação.
        private string _fileName; //Nome do arquivo a ser utilizado (ex : exemplo.txt)
        private string _fullPath;  //caminho completo para o arquivo a ser utilizado

        #region "Contrutures Gets e Sets"

        /// <summary>
        /// 'Ao ser instanciada configura um caminho padrão e um arquivo padrão onde será buscado o arquivo .txt contendo a string de conexão
        /// </summary>
        /// <remarks></remarks>
        //contrutor define caminho default de arquivo
        public FileUtility()
        {
            PathMobile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            FileName = "strConn.txt";
            setFullPath();
        }

        /// <summary>
        /// Ao ser instanciada configura um caminho e um Nome de arquivo 
        /// de acordo com os parâmetros informados. 
        /// </summary>
        /// <param name="strPathMobile">Path no dispositivo mobile</param>
        /// <param name="strFileName">Nome de um arquivo de text existente no Path informado</param>
        /// <remarks></remarks>
        public FileUtility  (string strPathMobile, string strFileName)
        {
            PathMobile = strPathMobile;
            FileName = strFileName;
            setFullPath();
        }

        public string PathMobile {
            get { return _pathMobile; }

            set {
                if ((!string.IsNullOrEmpty(value))) {
                    if ((value.Substring(value.Length - 1) != "\\")) {
                        value = value + "\\";
                    }
                    _pathMobile = value;
                } else {
                    _pathMobile = null;
                }
            }
        }

        public string FileName {
            get { return _fileName; }

            set {
                if ((!string.IsNullOrEmpty(value))) {
                    if ((value.Substring(0, 1) == "\\")) {
                        value = value.Substring(1).Replace("/", "_").Replace("\\","_");
                    }
                    _fileName = value;
                }
            }
        }


        public string getFullPath()
        {
            return _fullPath;
        }


        public void setFullPath()
        {
            if ((!string.IsNullOrEmpty(PathMobile) & !string.IsNullOrEmpty(FileName))) {
                _fullPath = PathMobile + FileName;
            } else if ((!string.IsNullOrEmpty(PathMobile) & string.IsNullOrEmpty(FileName))) {
                _fullPath = PathMobile + "text" + System.DateTime.Now.DayOfYear + ".txt";
            } else {
                _fullPath = null;
            }

        }


        public void setFullPath(string pathMobile, string fileName)
        {
            if ((!string.IsNullOrEmpty(pathMobile) & !string.IsNullOrEmpty(fileName))) {
                _fullPath = pathMobile + fileName;
            } else if ((!string.IsNullOrEmpty(pathMobile) & string.IsNullOrEmpty(fileName))) {
                _fullPath = pathMobile + "text" + System.DateTime.Now.DayOfYear + ".txt";
            } else {
                _fullPath = null;
            }

        }

        #endregion

        /// <summary>
        /// Criar um arquivo de texto
        /// </summary>
        /// <param name="strTexto">Texto a ser salvo no arquivo</param>
        /// <remarks></remarks>
        public void salvarArquivoTexto(string strTexto)
        {
            //caminho onde será salvo o arquivo
            string fullPath = Path.GetFullPath(PathMobile);

            //Deleta o arquivo caso ele já exista 
            if (File.Exists(fullPath)) {
                File.Delete(fullPath);
            }

            //Criar o novo arquivo e adiciona o texto.
            FileStream fs = File.Create(fullPath);
            AddText(fs, strTexto);
            fs.Close();

            //Open the stream and read it back.
            fs = File.OpenRead(fullPath);
            byte[] b = new byte[1025];
            UTF8Encoding temp = new UTF8Encoding(true);

            while (fs.Read(b, 0, b.Length) > 0) {
                //Console.WriteLine(temp.GetByteCount(b.ToString));
            }

            fs.Close();

        }

        /// <summary>
        /// Lê um arquivo de texto disponível na rede
        /// </summary>
        /// <returns>Um array list contendo os dados do arquivo de texto</returns>
        /// <remarks></remarks>
        public List <string> readTextFile()
        {

            string fullPath = Path.GetFullPath(getFullPath());
            List<string> alTextFile = new List<string>();

            try {
                //instancia um streamReader que irá ler o arquivo informado
                StreamReader sr = new StreamReader(fullPath);
                string line = null;

                do {
                    line = sr.ReadLine();
                    alTextFile.Add(line);
                } while (!(line == null));
                sr.Close();
            } catch (Exception E) {
               MessageBox.Show("O arquivo não pode ser lido.");
                Debug.WriteLine(E.Message);
            }

            return alTextFile;

        }

        /// <summary>
        /// Lê um arquivo de texto disponível na rede
        /// </summary>
        /// <returns>Um array list contendo os dados do arquivo de texto</returns>
        /// <remarks></remarks>
        public ArrayList readTextFile(string strPathMobile, string strNomeArquivo)
        {

            //'Seta o novo caminho para o parametro _fullPath
            setFullPath(strPathMobile, strNomeArquivo);

            string fullPath = Path.GetFullPath(getFullPath());
            ArrayList alTextFile = new ArrayList();

            try {
                //instancia um streamReader que irá ler o arquivo informado
                StreamReader sr = new StreamReader(fullPath);
                string line = null;

                do {
                    line = sr.ReadLine();
                    alTextFile.Add(line);
                } while (!(line == null));
                sr.Close();
            } catch (Exception E) {
                MessageBox.Show ("O arquivo não pode ser lido.");
                Debug.WriteLine(E.Message);
            }

            return alTextFile;

        }

        /// <summary>
        /// Retorna os diretórios existentes no caminho informado
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ArrayList getDiretorio()
        {

            ArrayList alDiretorio = new ArrayList();

            foreach (string entry in System.IO.Directory.GetDirectories(Path.GetDirectoryName(getFullPath()))) {
                //alDiretorio.Add(new System.IO.FileInfo(entry).ToString);
            }

            return alDiretorio;

        }

        /// <summary>
        /// Gera um array a partir de uma string sendo nescessário indicar apenas o 
        /// tipo de caracter que irá servir como split e a string a ser trabalhada no array.
        /// </summary>
        /// <param name="strFile">String da qual será gerado a Array de string</param>
        /// <param name="splitCaracter">Tipo de caracter a ser usado como separador no split</param>
        /// <returns>Array preenchido com valores do tipo string de acordo com a Quantidade de separadores encontrados na string </returns>
        /// <remarks></remarks>
        /// <exemplo>
        /// 
        /// string = "text1/text2/text3/text4/text5" onde o separador e o caracter "/"
        /// 
        /// o retorno será um array com 5 posições
        /// string(0)= text1
        /// string(1)= text2
        /// string(2)= text3
        /// string(3)= text4
        /// string(4)= text5
        /// </exemplo>
        public static String[] arrayOfTextFile(string strFile, splitType splitCaracter)
        {
            try
            {
                string[] arrayFile = null;
                char  splitCarac = (char)defineSplitType(splitCaracter);
                arrayFile = strFile.Split(splitCarac);
                return arrayFile;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Utilizado para montar um arquivo de texto
        /// </summary>
        /// <param name="fs">FileStream</param>
        /// <param name="value">Texto</param>
        /// <remarks></remarks>
        private static void AddText(System.IO.FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        /// <summary>
        /// Define um caracter para ser usado como separador na função Split()
        /// </summary>
        /// <param name="splitType">Tipo de caracter a ser usado com separador (ENUM)</param>
        /// <returns>o caracter a ser usado como separador</returns>
        /// <remarks></remarks>
        public static char defineSplitType(splitType st)
        {
            char splitCaracter;

            switch (st) {
                case splitType.BARRA:
                    splitCaracter = '/';
                    break;
                case splitType.BARRA_INVERTIDA :
                    splitCaracter = '\\';
                    break;
                case splitType.DOIS_PONTOS :
                    splitCaracter = ':';
                    break;
                case splitType.PIPE :
                    splitCaracter = '|';
                    break;
                case splitType.PONTO_VIRGULA:
                    splitCaracter = ';';
                    break;
                case splitType.VIRGULA:
                    splitCaracter = ',';
                    break;
                case splitType.E_COMERCIAL :
                    splitCaracter = '&';
                    break;
                case splitType.EXCLAMACAO :
                    splitCaracter = '!';
                    break;
                case splitType.CIFRAO :
                    splitCaracter = '$';
                    break;
                default:
                    splitCaracter = '|';
                    break;
            }

            return splitCaracter;

        }

        /// <summary>
        /// Tipos de caracteres que devem ser usados como separador
        /// </summary>
        /// <remarks></remarks>
        public enum splitType
        {
            BARRA = 1,
            BARRA_INVERTIDA = 2,
            DOIS_PONTOS = 3,
            PIPE = 4,
            PONTO_VIRGULA = 5,
            VIRGULA = 6,
            E_COMERCIAL = 7,
            EXCLAMACAO = 8,
            CIFRAO = 9
        }

    }
}

