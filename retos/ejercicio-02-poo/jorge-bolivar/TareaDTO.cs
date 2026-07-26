namespace jorge_bolivar
{
    public class TareaDto
    {
        public int Id {get; set;}
        public string Titulo {get; set;} = "";
        public string Descripcion {get; set;} = "";
        public Prioridad Prioridad {get; set;} 
        public Categoria Categoria {get; set;} = new Categoria();
        public bool Completada {get; set;} 
        public DateTime FechaCreacion {get; set;}
        public DateTime? FechaVencimiento {get; set;}
    }
}