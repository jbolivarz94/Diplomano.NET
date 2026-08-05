namespace market_place
{
    /// <summary>
    /// Solicitud para actualizar los datos de entrega de una orden.
    /// </summary>
    public class DeliveryUpdateRequest
    {
        /// <summary>Nueva fecha estimada de entrega en formato "yyyy-MM-dd" (opcional).</summary>
        public DateOnly? estimatedDeliveryDate { get; set; }

        /// <summary>Fecha real de entrega en formato "yyyy-MM-dd HH:mm:ss" (opcional).</summary>
        public DateTime? deliveredAt { get; set; }

        /// <summary>Notas de la entrega (opcional).</summary>
        public string notes { get; set; } = string.Empty;
    }
}
