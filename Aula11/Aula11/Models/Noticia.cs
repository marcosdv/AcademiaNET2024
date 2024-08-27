namespace Aula11.Models
{
    public class Noticia
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public DateTime Data { get; set; }
        public int CatId { get; set; }
        public Categoria Categoria { get; set; }

        public Noticia() { }

        public Noticia(NoticiaRequest request)
        {
            Titulo = request.Titulo;
            Texto = request.Texto;
            CatId = request.Categoria;
            Data = DateTime.Now;
        }
    }
}