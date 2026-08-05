namespace market_place
{
    /// <summary>
    /// Producto agrícola publicado por un agricultor en el catálogo.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identificador numérico del producto.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Identificador numérico del agricultor que publica el producto.
        /// </summary>
        public int farmerProfileId { get; set; }

        /// <summary>
        /// Identificador numérico de la categoría del producto.
        /// </summary>
        public int categoryId { get; set; }

        /// <summary>
        /// Identificador numérico de la unidad de medida del producto.
        /// </summary>
        public int unitOfMeasureId { get; set; }

        /// <summary>
        /// Nombre del producto (ej. "Tomate cherry orgánico").
        /// </summary>
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del producto (opcional).
        /// </summary>
        public string? description { get; set; }

        /// <summary>
        /// Precio unitario del producto.
        /// </summary>
        public decimal unitPrice { get; set; }

        /// <summary>
        /// Cantidad en stock del producto.
        /// </summary>
        public decimal stockQuantity { get; set; }

        /// <summary>
        /// Indica si el producto es orgánico.
        /// </summary>
        public bool isOrganic { get; set; }

        /// <summary>
        /// Fecha de cosecha del producto (opcional).
        /// </summary>
        public DateOnly? harvestDate { get; set; }

        /// <summary>
        /// Indica si el producto está activo.
        /// </summary>
        public bool isActive { get; set; }

        /// <summary>
        /// Fecha de creación del producto.
        /// </summary>
        public DateTime createdAt { get; set; }
    }
}