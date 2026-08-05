namespace market_place
{
    /// <summary>
    /// Artículo de una orden: detalle de producto, cantidad y precios al momento de la compra.
    /// Mapea a la tabla "order_items".
    /// </summary>
    public class OrderItem
    {
        /// <summary>Identificador único del artículo (autoincremental).</summary>
        public int id { get; set; }

        /// <summary>ID de la orden a la que pertenece el artículo (FK a orders).</summary>
        public int orderId { get; set; }

        /// <summary>ID del producto comprado (FK a products).</summary>
        public int productId { get; set; }

        /// <summary>Cantidad comprada del producto (mayor a 0).</summary>
        public decimal quantity { get; set; }

        /// <summary>Precio unitario del producto al momento de la compra.</summary>
        public decimal unitPrice { get; set; }

        /// <summary>Precio total del artículo (cantidad x precio unitario).</summary>
        public decimal totalPrice { get; set; }
    }
}
