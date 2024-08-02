using Aula01;
using System.Data.SqlClient;

internal class Program
{
    private static Noticias noticias;

    private static void Main(string[] args)
    {
        noticias = new Noticias();
        int opcao = 0;

        while (opcao != 9)
        {
            Console.WriteLine("Selecione a opção de Filtro:");
            Console.WriteLine("1 -> Filtrar por Número de dias");
            Console.WriteLine("2 -> Filtrar por Título");
            Console.WriteLine("3 -> Inserir Notícia");
            Console.WriteLine("4 -> Alterar Notícia");
            Console.WriteLine("5 -> Excluir Notícia");
            Console.WriteLine("9 -> Encerrar Aplicação");
            opcao = int.Parse(Console.ReadLine());
            
            Console.Clear();

            switch (opcao)
            {
                case 1: BuscaPorData(); break;
                case 2: BuscaPorTitulo(); break;
                case 3: InserirNoticia(); break;
                case 4: AlterarNoticia(); break;
                case 5: ExcluirNoticia(); break;
                case 9: break;
                default: Console.WriteLine("Opção inválida!"); break;
            }

            Console.ReadLine();
        }
    }

    private static void BuscaPorData()
    {
        Console.Write("Digite até quantos dias busca a notícia: ");
        int numDias = int.Parse(Console.ReadLine()) * -1;
        DateTime filtroData = DateTime.Now.Date.AddDays(numDias);

        noticias.BuscarPorData(filtroData);
    }

    private static void BuscaPorTitulo()
    {
        Console.Write("Filtrar por título da notícia: ");
        string titulo = Console.ReadLine();

        noticias.BuscarPorTitulo(titulo);
    }

    private static void InserirNoticia()
    {
        Console.Write("Título da notícia: ");
        string titulo = Console.ReadLine();

        Console.Write("Texto da notícia: ");
        string texto = Console.ReadLine();

        Console.Write("Categoria da Notícia: ");
        int categoria = int.Parse(Console.ReadLine());

        noticias.Inserir(titulo, texto, categoria);
    }

    private static void AlterarNoticia()
    {
        Console.Write("Notícia que deseja alterar: ");
        int codigo = int.Parse(Console.ReadLine());

        Console.Write("Título da notícia: ");
        string titulo = Console.ReadLine();

        Console.Write("Texto da notícia: ");
        string texto = Console.ReadLine();

        Console.Write("Categoria da Notícia: ");
        int categoria = int.Parse(Console.ReadLine());

        noticias.Alterar(codigo, titulo, texto, categoria);
    }

    private static void ExcluirNoticia()
    {
        Console.Write("Notícia que deseja excluir: ");
        int codigo = int.Parse(Console.ReadLine());

        noticias.Excluir(codigo);
    }
}