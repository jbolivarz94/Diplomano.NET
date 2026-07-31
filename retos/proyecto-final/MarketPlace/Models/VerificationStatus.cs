namespace market_place
{
    /// <summary>
    /// Estado de verificación de un perfil de agricultor.
    /// </summary>
    public enum VerificationStatus
    {
        /// <summary>El perfil está pendiente de revisión.</summary>
        Pending,

        /// <summary>El perfil fue aprobado.</summary>
        Approved,

        /// <summary>El perfil fue rechazado.</summary>
        Rejected
    }

    /// <summary>
    /// Estado del ciclo de vida de una orden de compra.
    /// </summary>
    public enum StatusOrder
    {
        /// <summary>La orden fue creada y está pendiente de confirmación.</summary>
        Pending,

        /// <summary>La orden fue confirmada.</summary>
        Confirmed,

        /// <summary>La orden está en preparación.</summary>
        Preparing,

        /// <summary>La orden está en tránsito hacia el destino.</summary>
        InTransit,

        /// <summary>La orden fue entregada.</summary>
        Delivered,

        /// <summary>La orden fue cancelada.</summary>
        Cancelled
    }
}
