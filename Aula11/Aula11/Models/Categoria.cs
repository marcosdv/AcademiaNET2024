namespace Aula11.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        public IList<Noticia> Noticias { get; set; }
    }
}