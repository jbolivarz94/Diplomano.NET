namespace market_place
{
    /// <summary>
    /// Categoría de productos agrícolas (Frutas, Hortalizas, Tubérculos, Lácteos...).
    /// Mapea a la tabla "categories".
    /// </summary>
    public class Categorie
    {
        /// <summary>Identificador único de la categoría (autoincremental).</summary>
        public int id { get; set; }

        /// <summary>Nombre de la categoría (único).</summary>
        public string name { get; set; } = string.Empty;

        /// <summary>Descripción de la categoría.</summary>
        public string description { get; set; } = string.Empty;
    }
}
