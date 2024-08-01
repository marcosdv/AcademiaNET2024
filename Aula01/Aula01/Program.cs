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
            Console.WriteLine("9 -> Encerrar Aplicação");
            opcao = int.Parse(Console.ReadLine());
            
            Console.Clear();

            switch (opcao)
            {
                case 1: BuscaPorData(); break;
                case 2: BuscaPorTitulo(); break;
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
}