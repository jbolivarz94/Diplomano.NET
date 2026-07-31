namespace market_place
{
    /// <summary>
    /// Unidad de medida para la venta de productos (kg, lb, atado, unidad, caja).
    /// Mapea a la tabla "units_of_measure".
    /// </summary>
    public class UnitOfMeasure
    {
        /// <summary>Identificador único de la unidad de medida (autoincremental).</summary>
        public int id { get; set; }

        /// <summary>Nombre completo de la unidad (ej. "Kilogramo").</summary>
        public string name { get; set; } = string.Empty;

        /// <summary>Abreviatura de la unidad (ej. "kg").</summary>
        public string abbreviation { get; set; } = string.Empty;
    }
}
