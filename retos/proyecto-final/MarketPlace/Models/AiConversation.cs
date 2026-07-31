namespace market_place
{
    /// <summary>
    /// Conversación con la IA (Groq): mensaje de system, user o assistant con su rol y tokens utilizados.
    /// Mapea a la tabla "ai_conversations".
    /// </summary>
    public class AiConversation
    {
        /// <summary>Identificador único de la conversación (autoincremental).</summary>
        public int id { get; set; }

        /// <summary>Rol del mensaje en la conversación (system, user, assistant).</summary>
        public PromptRole promptRole { get; set; }

        /// <summary>Contenido del mensaje.</summary>
        public string message { get; set; } = string.Empty;

        /// <summary>Cantidad de tokens utilizados por el mensaje.</summary>
        public int tokensUsed { get; set; }

        /// <summary>Fecha de creación del mensaje en formato "yyyy-MM-dd HH:mm:ss".</summary>
        public string createdAt { get; set; } = string.Empty;
    }
}
