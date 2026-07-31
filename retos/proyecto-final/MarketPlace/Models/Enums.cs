namespace market_place
{
    /// <summary>
    /// Tipo de entrega de una orden de compra.
    /// </summary>
    public enum DeliveryType
    {
        /// <summary>El consumidor recoge el pedido directamente en la finca.</summary>
        FarmPickup,

        /// <summary>El pedido se entrega directamente en el domicilio del consumidor.</summary>
        DirectHomeDelivery,

        /// <summary>El pedido se entrega en un punto de mercado local.</summary>
        LocalMarketPoint
    }

    /// <summary>
    /// Rol de un mensaje dentro de una conversación con la IA.
    /// </summary>
    public enum PromptRole
    {
        /// <summary>Mensaje de sistema que define el comportamiento de la IA.</summary>
        system,

        /// <summary>Mensaje del usuario.</summary>
        user,

        /// <summary>Respuesta generada por la IA.</summary>
        assistant
    }
}
