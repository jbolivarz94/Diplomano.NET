namespace jorge_bolivar
{
    public class TareaConVencimiento : Tarea
    {
        public DateTime FechaVencimiento { get; set; }

        public TareaConVencimiento(string titulo, string descripcion, Prioridad prioridad, Categoria categoria, DateTime fechaVencimiento)
            : base(titulo, descripcion, prioridad, categoria)
        {
            FechaVencimiento = fechaVencimiento;
        }

        public TareaConVencimiento(int id, string titulo, string descripcion, Prioridad prioridad, Categoria categoria, DateTime fechaCreacion, DateTime fechaVencimiento)
            : base(id, titulo, descripcion, prioridad, categoria, fechaCreacion)
        {
            FechaVencimiento = fechaVencimiento;
        }

        public int DiasRestantes
        {
            get
            {
                int diasRestantes = (FechaVencimiento - DateTime.Now).Days;
                return diasRestantes;
            }
        }

        public override void MostrarInfo()
        {
            base.MostrarInfo();
            Console.WriteLine($"Fecha de Vencimiento: {FechaVencimiento}");
            Console.WriteLine($"Días Restantes: {DiasRestantes}");
        }

        public override string Exportar()
        {
            return base.Exportar() + $"| Fecha de Vencimiento: {FechaVencimiento}| Dias Restantes: {DiasRestantes}";
        }
    }
}