namespace Aula11.Models
{
    public class Noticia
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }
        public int Categoria { get; set; }

        public Noticia() { }

        public Noticia(NoticiaRequest request)
        {
            Titulo = request.Titulo;
            Texto = request.Texto;
            Categoria = request.Categoria;
            Data = DateTime.Now;
        }
    }
}