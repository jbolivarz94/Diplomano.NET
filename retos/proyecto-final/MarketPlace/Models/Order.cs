namespace market_place
{
    /// <summary>
    /// Orden de compra generada por un consumidor con dirección de envío y logística de entrega.
    /// Mapea a la tabla "orders".
    /// </summary>
    public class Order
    {
        /// <summary>Identificador único de la orden (autoincremental).</summary>
        public int id { get; set; }

        /// <summary>Número de orden legible y único (ej. "ORD-20260731120434-C546").</summary>
        public string orderNumber { get; set; } = string.Empty;

        /// <summary>ID del perfil de agricultor que atiende la orden (FK a farmer_profiles).</summary>
        public int farmerProfileId { get; set; }

        /// <summary>Estado actual de la orden (Pending, Confirmed, Preparing, InTransit, Delivered, Cancelled).</summary>
        public StatusOrder status { get; set; }

        /// <summary>Monto total de la orden (suma del total de sus artículos).</summary>
        public float totalAmount { get; set; }

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

        /// <summary>Tipo de entrega de la orden (FarmPickup, DirectHomeDelivery, LocalMarketPoint).</summary>
        public DeliveryType deliveryType { get; set; }

        /// <summary>Fecha estimada de entrega en formato "yyyy-MM-dd" (opcional).</summary>
        public string estimatedDeliveryDate { get; set; } = string.Empty;

        /// <summary>Fecha real de entrega en formato "yyyy-MM-dd HH:mm:ss" (opcional).</summary>
        public string deliveredAt { get; set; } = string.Empty;

        /// <summary>Fecha de creación de la orden en formato "yyyy-MM-dd HH:mm:ss".</summary>
        public string createdAt { get; set; } = string.Empty;
    }
}
