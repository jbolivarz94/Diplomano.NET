namespace market_place
{
    /// <summary>
    /// Solicitud para crear una orden de compra: dirección de envío, tipo de entrega y artículos.
    /// </summary>
    public class OrderRequest
    {
        /// <summary>ID del perfil de agricultor que atiende la orden.</summary>
        public int farmerProfileId { get; set; }

        /// <summary>Notas generales de la orden (opcional).</summary>
        public string notes { get; set; } = string.Empty;

        /// <summary>Calle y número de la dirección de envío (obligatoria).</summary>
        public string streetAddress { get; set; } = string.Empty;

        /// <summary>Municipio de la dirección de envío (obligatorio).</summary>
        public string municipality { get; set; } = string.Empty;

        /// <summary>Departamento de la dirección de envío (obligatorio).</summary>
        public string department { get; set; } = string.Empty;

        /// <summary>Detalles adicionales de la dirección (opcional).</summary>
        public string additionalDetails { get; set; } = string.Empty;

        /// <summary>Tipo de entrega (por defecto: DirectHomeDelivery).</summary>
        public DeliveryType? deliveryType { get; set; }

        /// <summary>Fecha estimada de entrega en formato "yyyy-MM-dd" (opcional).</summary>
        public DateOnly? estimatedDeliveryDate { get; set; }

        /// <summary>Lista de artículos solicitados (al menos uno).</summary>
        public List<OrderItemRequest> items { get; set; } = new();
    }

    /// <summary>
    /// Artículo solicitado dentro de una orden: producto y cantidad.
    /// </summary>
    public class OrderItemRequest
    {
        /// <summary>ID del producto a comprar.</summary>
        public int productId { get; set; }

        /// <summary>Cantidad a comprar del producto (mayor a 0).</summary>
        public decimal quantity { get; set; }
    }
}
