namespace Aula04.Models
{
    public class Pokemon
    {
        public int id { get => int.Parse(numero); }
        public string numero { get => url.Replace("https://pokeapi.co/api/v2/pokemon/", "").Replace("/", ""); }
        public string name { get; set; }
        public string url { get; set; }
    }
}