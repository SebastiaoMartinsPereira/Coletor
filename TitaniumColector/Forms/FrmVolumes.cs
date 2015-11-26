using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TitaniumColector.Classes.Model;
using TitaniumColector.Classes.Procedimentos;
using System.Globalization;

namespace TitaniumColector.Forms
{
    public partial class FrmVolumes : Form
    {
        public FrmVolumes()
        {
            InitializeComponent();
            configurarForm();
        }

        private void FrmVolumes_Load(object sender, EventArgs e)
        {
            carregarListEmbalagens();

            if (ProcedimentosLiberacao.TotalVolumes == 0) 
            {
                ProcedimentosLiberacao.setTotalVolumes();
            }
            
            if (temItens())
                this.listVolumes.Items[0].Selected = true;

            atualizarDadosForm();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            addVolume();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            removeVolume();
        }

        /// <summary>
        /// Carrega a listView de embalagens para visualização.
        /// </summary>
        private void carregarListEmbalagens()
        {
            this.listVolumes.Items.Clear();

            //carrega o listview com as informações carregadas do banco de dados.
            foreach (var item in ProcedimentosLiberacao.ListEmbalagensSeparacao)
            {
                CultureInfo ptBr = CultureInfo.CreateSpecificCulture("pt-BR");
                string peso = string.Format(item.Peso.ToString("00.000", ptBr));

                this.listItem = new System.Windows.Forms.ListViewItem();
                this.listItem.Tag = item.Codigo.ToString();
                this.listItem.Text = item.Codigo.ToString();
                this.listItem.SubItems.Add(item.Nome);
                this.listItem.SubItems.Add(item.Quantidade.ToString());
                this.listItem.SubItems.Add(peso);
                this.listVolumes.Items.Add(this.listItem);
            }
        }

        /// <summary>
        /// Carrega a listView de embalagens para visualização.
        /// </summary>
        private void carregarListEmbalagens(int index)
        {
            this.listVolumes.Items.Clear();

            //carrega o listview com as informações carregadas do banco de dados.
            foreach (var item in ProcedimentosLiberacao.ListEmbalagensSeparacao)
            {
                CultureInfo ptBr = CultureInfo.CreateSpecificCulture("pt-BR");
                string peso = string.Format(item.Peso.ToString("00.000", ptBr));

                this.listItem = new System.Windows.Forms.ListViewItem();
                this.listItem.Tag = item.Codigo.ToString();
                this.listItem.Text = item.Codigo.ToString();
                this.listItem.SubItems.Add(item.Nome);
                this.listItem.SubItems.Add(item.Quantidade.ToString());
                this.listItem.SubItems.Add(peso);
                this.listVolumes.Items.Add(this.listItem);
            }

            this.listVolumes.Items[index].Selected = true;
        }

        private void addVolume() 
        {
            if (temItemSelecionado())
            {
                int index = indexEmbalagemSelecionada();
                ProcedimentosLiberacao.incrementaQtdEmbalagem(embalagemSelecionada());
                carregarListEmbalagens(indexEmbalagemSelecionada());
                atualizarDadosForm();
                itemFocado(index);
            }
        }

        private void removeVolume() 
        {
            try
            {
                if (temItemSelecionado())
                {
                    int index = indexEmbalagemSelecionada();
                    ProcedimentosLiberacao.decrementaQtdEmbalagem(embalagemSelecionada());
                    carregarListEmbalagens(index);
                    atualizarDadosForm();
                    itemFocado(index);
                }
            }
            catch (Exception ex)
            {
                MainConfig.errorMessage(ex.Message, "Gerenciar Volumes");
            }
           
        }

        private bool temItemSelecionado() 
        {
            return this.listVolumes.SelectedIndices.Count > 0;
        }

        private bool temItens() 
        {
            return this.listVolumes.Items.Count > 0;
        }

        private int embalagemSelecionada() 
        {
            return Convert.ToInt32(this.listVolumes.Items[this.listVolumes.SelectedIndices[0]].SubItems[0].Text);
        }

        private int indexEmbalagemSelecionada() 
        {
            return Convert.ToInt32(this.listVolumes.Items[this.listVolumes.SelectedIndices[0]].Index);
        }

        private void itemFocado(int index)
        {
            this.listVolumes.Items[index].Focused = true;
        }

        private void atualizarDadosForm() 
        {
            CultureInfo ptBr = CultureInfo.CreateSpecificCulture("pt-BR");
            string volumes = string.Format(ProcedimentosLiberacao.TotalVolumes.ToString("000", ptBr));
            string pesoEmbalagem = string.Format(ProcedimentosLiberacao.PesoTotalEmbalagens.ToString("00.000", ptBr));
            string pesoProduto = string.Format(ProcedimentosLiberacao.PesoTotalEmbalagens.ToString("00.000", ptBr));

            this.lblValVolumes.Text = volumes;
            this.lblValPesoEmb.Text = pesoEmbalagem + " kg";
            this.lblValTotal.Text = pesoProduto + " kg";
        }

    }
}