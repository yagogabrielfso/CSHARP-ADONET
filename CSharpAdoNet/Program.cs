using System;
using System.Data.SqlClient;
using static System.Console;

namespace CSharpAdoNet
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("============= CONTROLE DE CLIENTES =============\n");
            WriteLine("Selecione um operação:");
            WriteLine("1 - Listar ");
            WriteLine("2 - Cadastrar ");
            WriteLine("3 - Editar ");
            WriteLine("4 - Deletar ");
            WriteLine("5 - Visualizar ");
            Write("Opção: ");
            int opc = Convert.ToInt32(ReadLine());
            int idOp;
            int _id;
            string _nome, _email;
            string nome;
            string email;
            string answer;


            switch (opc)
            {
                case 1:
                    Title = "Listagem de clientes";
                    WriteLine("============= LISTAGEM DE CLIENTES ============= ");
                    ListarClientes();
                    break;

                case 2:
                    Title = "Cadastro de clientes";
                    WriteLine("============= CADASTRO DE CLIENTES ============= ");

                    Write("Informe um nome: ");
                    nome = ReadLine();

                    Write("Informe um e-mail: ");
                    email = ReadLine();

                    SalvarCliente(nome,email);
                    break;

                case 3:
                    Title = "Edição de clientes";
                    WriteLine("============= EDIÇÃO DE CLIENTES ============= ");

                    ListarClientes();
                    Write("Selecione um cliente pela ID: ");
                    idOp =  Convert.ToInt32(ReadLine());
                    (_id,  _nome,  _email) = SelecionarCliente(idOp);

                    Clear();

                    Title = $"Edição de clientes - {_id} - {_nome}";
                    WriteLine($"============= EDIÇÃO DE CLIENTES - Cód.{_id} - {_nome} ============= ");
                    Write("Informe um nome: ");
                    nome = ReadLine();

                    Write("Informe um email: ");
                    email = ReadLine();

                    nome = nome.Equals("") ? _nome : nome;
                    email = email.Equals("") ? _email : email;
                    SalvarCliente(nome, email, idOp);

                    break;

                case 4:
                    Title = "Exclusão de clientes";
                    WriteLine("============= EXCLUSÃO DE CLIENTES ============= ");
                    ListarClientes();

                    Write("Selecione um cliente pela ID: ");
                    idOp = Convert.ToInt32(ReadLine());

                    (_id, _nome, _email) = SelecionarCliente(idOp);

                    WriteLine("\n\n**************** ATENÇÃO!!! ****************");
                    WriteLine($"\nConfirmar exclusão d" +
                              $"o cliente {_nome}? S/N");
                    answer = ReadLine().ToUpper();

                    switch (answer)
                    {
                        case "S":
                            DeletarCliente(idOp);
                            Clear();
                            ListarClientes();
                            break;
                        case "N":
                            Clear();
                            WriteLine($"O cliente {_nome} não foi deletado.");
                            break;
                        default:
                            Clear();
                            WriteLine("Opção inválida.");
                            break;

                    }

                    break;

                case 5:
                    Title = "Visualização de clientes";
                    WriteLine("============= VER DETALHES DE CLIENTE ============= ");
                    ListarClientes();

                    Write("Selecione um cliente pela ID: ");
                    idOp = Convert.ToInt32(ReadLine());
                    (_id, _nome, _email) = SelecionarCliente(idOp);

                    Clear();
                    Title = $"Visualização de clientes {_nome}";
                    WriteLine($"============= VER DETALHES DE CLIENTE - {_nome} ============= "); 

                    WriteLine($"ID: {_id}");
                    WriteLine($"Nome: {_nome}");
                    WriteLine($"Email: {_email}");

                    break;

                default:
                    Title = "Opção inválida";
                    WriteLine("============= SELECIONE UMA OPÇÃO VÁLIDA! ============= ");
                    break;



            }

            ReadLine();
        }

        static (int, string, string) SelecionarCliente(int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    dr.Read();
                    return (Convert.ToInt32(dr["id"].ToString()), dr["nome"].ToString(), dr["email"].ToString());
                }
            }
        }
        static string getStringConn()
        {
            // string para conexão com o banco de dados
            string   connString = "Server =DESKTOP-MHUELB2\\SQLEXPRESS;Database=CSharpAdoNET;User Id=sa;Password=dev123"; 
            return connString;
        }

        static void ListarClientes()
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            { 
                conn.Open();

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from clientes order by id";

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        WriteLine("ID: {0}", dr["id"]);
                        WriteLine("Nome: {0}", dr["nome"]);
                        WriteLine("E-mail: {0}", dr["email"]);
                        WriteLine("--------------------------------");
                    }
                    ReadLine();


                }
            }
        }

        static void SalvarCliente(string nome, string email)
        {
            //Busca string de conexão
            string connString = getStringConn();

            // efetua a conexão com o BD, utiliz o using para fechar a conexão após o bloco
            using (SqlConnection conn = new SqlConnection(connString))
            {

                // Abre a conexão
                conn.Open();

                //Cria um comando dentro da conexão
                SqlCommand cmd = conn.CreateCommand();

                //Determina qual comando de SQL vai ser
                cmd.CommandText = "Insert into clientes (nome, email) values (@nome, @email)";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);

                // Como não é uma QUERY de consulta, executa esse comando para realizar uma
                // ação no banco.
                cmd.ExecuteNonQuery();
            }
        }

        static void SalvarCliente(string nome, string email, int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update clientes set nome = @nome, email = @email where id = @id";
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        static void DeletarCliente(int id)
        {
            string connString = getStringConn();
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "delete clientes where id = @id";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
