namespace market_place
{
    /// <summary>
    /// Solicitud para generar recomendaciones de productos con IA.
    /// </summary>
    public class AiRequest
    {
        /// <summary>Prompt del usuario con el cual se generan las recomendaciones.</summary>
        public string prompt { get; set; } = string.Empty;
    }
}
