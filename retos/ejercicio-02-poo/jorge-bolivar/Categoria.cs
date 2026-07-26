namespace jorge_bolivar
{
    public class Categoria
    {
        public string Nombre { get; set; }
        public string Color { get; set; }
        public string Descripcion { get; set; }

        public Categoria()
        {
            Nombre = "Default";
            Color = "Blanco";
            Descripcion = "";
        }

        public Categoria(string nombre, string color, string descripcion)
        {
            Nombre = nombre;
            Color = color;
            Descripcion = descripcion;
        }
    }
}