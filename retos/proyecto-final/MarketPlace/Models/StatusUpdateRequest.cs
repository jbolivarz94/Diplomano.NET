namespace market_place
{
    /// <summary>
    /// Solicitud para cambiar el estado de una orden.
    /// </summary>
    public class StatusUpdateRequest
    {
        /// <summary>Nuevo estado de la orden (Pending, Confirmed, Preparing, InTransit, Delivered, Cancelled).</summary>
        public StatusOrder status { get; set; }
    }
}
