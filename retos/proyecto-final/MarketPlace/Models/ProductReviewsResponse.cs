namespace market_place
{
    /// <summary>
    /// Respuesta con las reseñas de un producto y su valoración promedio.
    /// </summary>
    public class ProductReviewsResponse
    {
        /// <summary>Lista de reseñas del producto.</summary>
        public List<Review> reviews { get; set; } = new();

        /// <summary>Valoración promedio de las reseñas (0 si no hay reseñas).</summary>
        public double averageRating { get; set; }
    }
}
