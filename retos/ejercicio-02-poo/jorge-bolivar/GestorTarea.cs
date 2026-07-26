using System.Data.SqlTypes;
using System.Text;
using System.Text.Json;

namespace jorge_bolivar
{
    public class GestorTarea
    {
        private List<Tarea> _tareas = new();

        public void Agregar(Tarea tarea)
        {
            _tareas.Add(tarea);
        }

        public void Completar(int  id)
        {
            var tarea = _tareas.FirstOrDefault(t => t.Id == id);
            if (tarea != null)
            {
                tarea.Completada = true;
            }
        }

        public List<Tarea> ListarTodas()
        {
            return _tareas;
        }

        public List<Tarea> ListarPorCategoria(string categoria)
        {
            return _tareas.Where(t => t.Categoria.Nombre.Equals(categoria, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Tarea> ListarPorPrioridad(Prioridad prioridad)
        {
            return _tareas.Where(t => t.Prioridad == prioridad).ToList();
        }

        public List<Tarea> ObtenerVencidas()
        {
            return _tareas.OfType<TareaConVencimiento>().Where(t => !t.Completada &&
             DateTime.Compare(t.FechaVencimiento, DateTime.Now) < 0).Cast<Tarea>().ToList();
        }

        public void Eliminar(int id)
        {
            var tarea = _tareas.FirstOrDefault(t => t.Id == id);
            if (tarea != null)
            {
                _tareas.Remove(tarea);
            }
        }

        public void GuardarEnJSON(string ruta, string nombreArchivo)
        {
            if(Directory.Exists(ruta) || string.IsNullOrEmpty(Path.GetExtension(ruta)))
            {
                ruta = Path.Combine(ruta, nombreArchivo);
            }

            string? directorio = Path.GetDirectoryName(ruta);

            if(!string.IsNullOrEmpty(directorio) && !Directory.Exists(directorio))
            {
                Directory.CreateDirectory(directorio);
            }

            var tareasExportables = _tareas.Select(t => t.Exportar()).ToList();
            string json = JsonSerializer.Serialize(tareasExportables, new JsonSerializerOptions{WriteIndented = true});
            File.WriteAllText(ruta, json);
        }

        public void CargarDesdeJSON(string archivo)
        {
            if (File.Exists(archivo))
            {
                var lineas = File.ReadAllLines(archivo);
                foreach (var linea in lineas)
                {
                    
                }
            }
        }

        public List<Categoria> CategoriasDisponible()
        {
            return _tareas.Select(t=> t.Categoria)
            .GroupBy(c => c.Nombre, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList();
        }
    }
}