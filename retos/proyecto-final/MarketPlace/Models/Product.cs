namespace market_place
{
    /// <summary>
    /// Producto agrícola publicado por un agricultor en el catálogo.
    /// Mapea a la tabla "products".
    /// </summary>
    public class Product
    {
        /// <summary>Identificador único del producto (autoincremental).</summary>
        public int id { get; set; }

        /// <summary>ID del perfil de agricultor que publica el producto (FK a farmer_profiles).</summary>
        public int farmerProfileId { get; set; }

        /// <summary>ID de la categoría del producto (FK a categories).</summary>
        public int categoryId { get; set; }

        /// <summary>ID de la unidad de medida del producto (FK a units_of_measure).</summary>
        public int unitOfMeasureId { get; set; }

        /// <summary>Nombre del producto.</summary>
        public string name { get; set; } = string.Empty;

        /// <summary>Descripción del producto.</summary>
        public string description { get; set; } = string.Empty;

        /// <summary>Precio por unidad de medida (no puede ser negativo).</summary>
        public float unitPrice { get; set; }

        /// <summary>Cantidad de existencias disponibles (no puede ser negativa).</summary>
        public float stockQuantity { get; set; }

        /// <summary>Indica si el producto es orgánico: 1 = orgánico, 0 = no orgánico.</summary>
        public int isOrganic { get; set; }

        /// <summary>Fecha de cosecha del producto en formato "yyyy-MM-dd".</summary>
        public string harvestDate { get; set; } = string.Empty;

        /// <summary>Indica si el producto está activo: 1 = activo, 0 = inactivo.</summary>
        public int isActive { get; set; }

        /// <summary>Fecha de publicación del producto en formato "yyyy-MM-dd HH:mm:ss".</summary>
        public string createdAt { get; set; } = string.Empty;
    }
}
