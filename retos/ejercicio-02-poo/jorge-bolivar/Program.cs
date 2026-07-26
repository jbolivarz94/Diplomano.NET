using System.Globalization;

namespace jorge_bolivar
{
    class Program
    {
        static GestorTarea _gestor = new();
        const string ArchivoJson = "tareas.json";
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            _gestor.CargarDesdeJSON(ArchivoJson);
            bool salir = false;
            string opcion;
            do
            {
                Menu();
                opcion = (Console.ReadLine() ?? "").Trim();
                try
                {
                    switch (opcion)
                    {
                        case "1": AgregarTarea(); break;
                        case "2": ListarTodas(); break;
                        case "3": ListarPorCategoria(); break;
                        case "4": ListarPorPrioridad(); break;
                        case "5": CompletarTarea(); break;
                        case "6": ListarVencidas(); break;
                        case "7": EliminarTarea(); break;
                        case "8": ExportarJson(); break;
                        case "9": 
                            _gestor.GuardarEnJSON(AppDomain.CurrentDomain.BaseDirectory,ArchivoJson);
                            Console.WriteLine("Datos guardados");
                            salir = true;
                            break;
                        default:
                            Console.WriteLine("Opcion invalida");
                            break;
                    }
                    
                } catch(Exception ex)
                {
                    Console.WriteLine($"\nOcurrió un error: {ex.Message}");
                }
                
            } while (!salir);
        }

        static void Menu()
        {
            Console.WriteLine("=== GESTOR DE TAREAS ===");
            Console.WriteLine("1. Agregar tarea");
            Console.WriteLine("2. Listar todas");
            Console.WriteLine("3. Listar por categoría");
            Console.WriteLine("4. Listar por prioridad");
            Console.WriteLine("5. Marcar como completada");
            Console.WriteLine("6. Mostrar tareas vencidas");
            Console.WriteLine("7. Eliminar tarea");
            Console.WriteLine("8. Exportar a JSON");
            Console.WriteLine("9. Salir");
            Console.WriteLine("Selecione una opcion: ");
        }

        //Metodo que agrega una tarea a la lista
        static void AgregarTarea()
        {
            Console.WriteLine("=== AGREGAR TAREA ===");
            Console.Write("Titulo: ");
            string titulo = (Console.ReadLine() ?? "").Trim();

            Console.Write("Descripcion: ");
            string descripcion = (Console.ReadLine() ?? "").Trim();

            Console.Write("Categoria: ");
            Categoria categoria = setCategoria();

            Prioridad prioridad = SetPrioridad();

            Console.WriteLine("Tiene fecha de vencimiento? (s/n)");
            string op =  (Console.ReadLine() ?? "").Trim();
            if(op == "s")
            {
                Console.WriteLine("Fecha de vencimiento (dd/mm/aaaa): ");
                string fechaTx = (Console.ReadLine() ?? "").Trim();

                if(!DateTime.TryParseExact(fechaTx,
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                 out DateTime fechaVencimiento))
                {
                    Console.WriteLine("Fecha invalida. Tarea se crea sin fecha de vencimiento");
                    _gestor.Agregar(new Tarea(titulo, descripcion, prioridad, categoria));
                }
                else
                {
                    _gestor.Agregar(new TareaConVencimiento(titulo,descripcion,prioridad,categoria,fechaVencimiento));
                }
            }
            else
            {
                _gestor.Agregar(new Tarea(titulo, descripcion, prioridad, categoria));
            }
            Console.WriteLine("Tarea agregada");
        }

        //Metodo que lista todas las tareas
        static void ListarTodas()
        {
            List<Tarea> tareas = _gestor.ListarTodas();
            Console.WriteLine();
            if(tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas registradas");
                return;
            }

            Console.WriteLine("=== LISTADO DE TAREAS === ");
            foreach(Tarea tarea in tareas)
            {
                tarea.MostrarInfo();
                Console.WriteLine();
            }
        }

        static void ListarPorCategoria()
        {
            List<Categoria> categorias = _gestor.CategoriasDisponible();
            if(categorias.Count > 0)
            {
                Console.WriteLine("Categorias que existen: ");
                for(int i = 0; i < categorias.Count; i++)
                {
                    Console.WriteLine($" {i + 1}. {categorias[i]}");
                }
                Console.WriteLine("Seleccione una opcion: ");                
                string x = (Console.ReadLine() ?? "").Trim();
                if(int.TryParse(x, out int seleccion) && seleccion >= 1 && seleccion <= categorias.Count)
                {
                    Listar(_gestor.ListarPorCategoria(categorias[seleccion-1].Nombre), "CATEGORIA");
                }
                else
                {
                    Console.WriteLine("No tareas para esta categoria");
                }
            }
        }

        static void ListarPorPrioridad()
        {
            Console.WriteLine("=== LISTADO POR PRIORIDAD === ");
            Prioridad prioridad = SetPrioridad();
            Listar(_gestor.ListarPorPrioridad(prioridad),"PRIORIDAD");
        }

        static void CompletarTarea()
        {
            ListarTodas();
            Console.WriteLine();
            Console.WriteLine("Ingrese el id de la tarea a completar: ");
            if(int.TryParse(Console.ReadLine(), out int id))
            {
                _gestor.Completar(id);
                Console.WriteLine("Tarea completada");
            }
            else
            {
                Console.WriteLine("Id invalido / Tarea no encontrada");
            }

        }

        static void ListarVencidas()
        {
            List<Tarea> vencidas = _gestor.ObtenerVencidas();
            Listar(vencidas, "VENCIDAS");
        }

        static void EliminarTarea()
        {
            ListarTodas();
            Console.WriteLine();
            Console.WriteLine("Ingrese el id de la tarea a eliminar: ");
            if(int.TryParse(Console.ReadLine(), out int id))
            {
                _gestor.Eliminar(id);
                Console.WriteLine("Tarea eliminada");
            }
            else
            {
                Console.WriteLine("Id invalido / Tarea no encontrada");
            }
        }

        static void ExportarJson()
        {
            Console.WriteLine("Ingrese la ruta donde se guardara el archivo: ");
            string ruta = (Console.ReadLine() ?? "").Trim();
            _gestor.GuardarEnJSON(ruta, ArchivoJson);
            foreach(var tarea in _gestor.ListarTodas())
            {
                Console.WriteLine(tarea.Exportar);
            }

        }

        //Metodo que settea la prioridad de una tarea
        static Prioridad SetPrioridad()
        {
            Console.WriteLine("Seleccione Prioridad: 1) Baja  2) Media  3) Alta  4)Critica");
            string x = (Console.ReadLine() ?? "").Trim();

            return x switch
            {
                "1" => Prioridad.Baja,
                "2" => Prioridad.Media,
                "3" => Prioridad.Alta,
                "4" => Prioridad.Critica,
                _ => Prioridad.Media
            };
        }

        //Metodo que settea una categoria existente o nueva a una tarea
        static Categoria setCategoria()
        {
            List<Categoria> categorias = _gestor.CategoriasDisponible();
            if(categorias.Count > 0)
            {
                Console.WriteLine("Categorias que existen: ");
                for(int i = 0; i < categorias.Count; i++)
                {
                    Console.WriteLine($" {i + 1}. {categorias[i]}");
                }
                Console.WriteLine("0. Crear una categoria nueva");
                Console.WriteLine("Seleccione una opcion: ");
                
                string x = (Console.ReadLine() ?? "").Trim();

                if(int.TryParse(x, out int seleccion) && seleccion >= 1 && seleccion <= categorias.Count)
                {
                    return categorias[seleccion-1];
                }
            }

            return CrearCategoriaNueva();
        }

        //Metodo que crea una categoria nueva
        static Categoria CrearCategoriaNueva()
        {
            Console.Write("Nombre: ");
            string nombre = (Console.ReadLine() ?? "").Trim();

            Console.Write("Color: ");
            string color = (Console.ReadLine() ?? "").Trim();

            Console.Write("Descripcion: ");
            string descripcion = (Console.ReadLine() ?? "").Trim();

            return new Categoria(nombre, color, descripcion);
        }

        static void Listar(List<Tarea> tareas, string tipo)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {tipo.ToUpper()} ===");

            if(tareas.Count == 0)
            {
                Console.WriteLine("No hay tareas registradas");
                return;
            }

            foreach(Tarea tarea in tareas)
            {
                tarea.MostrarInfo();
                Console.WriteLine();
            }
        }

    }
}