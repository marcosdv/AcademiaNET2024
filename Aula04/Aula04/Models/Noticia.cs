namespace Aula04.Models
{
    public class Noticia
    {
        public int NotId { get; set; }
        public string NotTitulo { get; set; }
        public string NotTexto { get; set; }
        public DateTime NotData { get; set; }
        public Categoria Categoria { get; set; }
    }
}