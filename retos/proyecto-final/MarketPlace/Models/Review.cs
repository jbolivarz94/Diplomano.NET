namespace market_place
{
    /// <summary>
    /// Reseña de un producto: calificación de 1 a 5 y comentario de un consumidor.
    /// Mapea a la tabla "reviews".
    /// </summary>
    public class Review
    {
        /// <summary>Identificador único de la reseña (autoincremental).</summary>
        public int id { get; set; }

        /// <summary>ID del producto calificado (FK a products).</summary>
        public int productId { get; set; }

        /// <summary>Calificación del producto (debe estar entre 1 y 5).</summary>
        public int rating { get; set; }

        /// <summary>Comentario del consumidor sobre el producto (opcional).</summary>
        public string comment { get; set; } = string.Empty;

        /// <summary>Fecha de creación de la reseña en formato "yyyy-MM-dd HH:mm:ss".</summary>
        public string createdAt { get; set; } = string.Empty;
    }
}
