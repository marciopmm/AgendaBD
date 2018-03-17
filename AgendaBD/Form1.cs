using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgendaBD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Criar conexão com o banco de dados SQL Server
            SqlConnection conexao = new SqlConnection();
            
            // Passar para a conexão qual é a string de conexão
            conexao.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Agenda.mdf;Integrated Security=True";

            // Criar comando para ler os dados da tabela
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexao;
            comando.CommandType = CommandType.Text;
            comando.CommandText = "SELECT ContatoID, Nome, DataNascimento, Celular, Email FROM Contato";

            // Abrir conexão com o Banco de Dados
            conexao.Open();

            // Cria um Leitor de Dados que recebe os dados da execução do comando
            SqlDataReader leitor = comando.ExecuteReader();

            // Para cada linha do leitor (tabela), criar uma linha na lista de contatos
            while (leitor.Read())
            {
                ListViewItem item = new ListViewItem();
                item.Text = leitor["ContatoID"].ToString();
                item.SubItems.Add(leitor["Nome"].ToString());
                item.SubItems.Add(leitor["Celular"].ToString());
                item.SubItems.Add(leitor["Email"].ToString());

                lstContatos.Items.Add(item);
            }

            // Fechar o leitor
            leitor.Close();

            // Fechar a conexão
            conexao.Close();
        }

        private void lstContatos_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Se houverem linhas selecionadas
            if (lstContatos.SelectedIndices.Count > 0)
            {
                // Desabilitar as caixas de texto
                TrocarHabilitacaoCampos(false);

                // Obter o texto da coluna 0 (ID)
                string contatoId = lstContatos.SelectedItems[0].Text;

                // Transformar para número
                int id = int.Parse(contatoId);

                // Criar conexão com o banco de dados SQL Server
                SqlConnection conexao = new SqlConnection();

                // Passar para a conexão qual é a string de conexão
                conexao.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|Agenda.mdf;Integrated Security=True";

                // Criar comando para ler os dados da tabela
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexao;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "SELECT ContatoID, Nome, DataNascimento, Celular, Email FROM Contato WHERE ContatoID = @ContatoID";

                // Adicionar o parâmetro do campo ContatoID
                comando.Parameters.AddWithValue("ContatoID", id);

                // Abrir conexão com o Banco de Dados
                conexao.Open();

                // Cria um Leitor de Dados que recebe os dados da execução do comando
                SqlDataReader leitor = comando.ExecuteReader();

                // Para a linha do leitor (tabela), preencher os detalhes na tela
                if (leitor.Read())
                {
                    lblID.Text = leitor["ContatoID"].ToString();
                    txtNome.Text = leitor["Nome"].ToString();

                    DateTime dtNasc = (DateTime)leitor["DataNascimento"];
                    txtDataNasc.Text = dtNasc.ToShortDateString();

                    txtCelular.Text = leitor["Celular"].ToString();
                    txtEmail.Text = leitor["Email"].ToString();
                }

                // Fechar o leitor
                leitor.Close();

                // Fechar a conexão
                conexao.Close();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            TrocarHabilitacaoCampos(true);
        }

        private void TrocarHabilitacaoCampos(bool habilitado)
        {
            txtNome.Enabled = habilitado;
            txtDataNasc.Enabled = habilitado;
            txtCelular.Enabled = habilitado;
            txtEmail.Enabled = habilitado;
        }
    }
}
