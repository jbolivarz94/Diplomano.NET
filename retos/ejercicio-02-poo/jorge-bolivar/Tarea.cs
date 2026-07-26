namespace jorge_bolivar
{
    public class Tarea : IExportable
    {
        private static int _contador = 0;
        public int Id { get; private set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public Prioridad Prioridad { get; set; }
        public bool Completada { get; set; }
        public Categoria Categoria { get; set; }

        public DateTime FechaCreacion { get; set; }

        public Tarea(string titulo, string descripcion, Prioridad prioridad, Categoria categoria)
        {
            Id = ++_contador;
            Titulo = titulo;
            Descripcion = descripcion;
            Prioridad = prioridad;
            Completada = false;
            Categoria = categoria;
            FechaCreacion = DateTime.Now;
        }

        public virtual void MostrarInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Título: {Titulo}");
            Console.WriteLine($"Descripción: {Descripcion}");
            Console.WriteLine($"Prioridad: {Prioridad}");
            Console.WriteLine($"Completada: {Completada}");
            Console.WriteLine($"Categoría: {Categoria.Nombre} (Color: {Categoria.Color}, Descripción: {Categoria.Descripcion})");
            Console.WriteLine($"Fecha de Creación: {FechaCreacion}");
        }

        public virtual string Exportar()
        {
            return $"ID: {Id}| Título: {Titulo}| Descripción: {Descripcion}| Prioridad: {Prioridad}| Completada: {Completada}| Categoría: {Categoria.Nombre} (Color: {Categoria.Color}, Descripción: {Categoria.Descripcion})| Fecha de Creación: {FechaCreacion}";
        }
    }
}