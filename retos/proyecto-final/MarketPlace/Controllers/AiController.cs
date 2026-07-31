using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace market_place
{
    /// <summary>
    /// Recomendaciones con IA (Groq).
    /// </summary>
    [ApiController]
    [Route("api/ai")]
    public class AiController : ControllerBase
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Inicializa el controlador con el contexto de base de datos.
        /// </summary>
        /// <param name="db">Contexto de Entity Framework Core (AppDbContext).</param>
        public AiController(AppDbContext db) => _db = db;

        /// <summary>
        /// Genera recomendaciones de productos a partir de un prompt del usuario.
        /// </summary>
        /// <param name="request">Solicitud con el prompt del usuario para generar recomendaciones.</param>
        /// <returns>200 OK con la respuesta de la IA y los tokens utilizados.</returns>
        [HttpPost("recommendations")]
        public async Task<ActionResult<object>> GetRecommendations(AiRequest request)
        {
            // TODO: integrar la API de Groq para generación real de recomendaciones.
            var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            var systemPrompt = "Eres un asistente experto de AgroMarket Local que recomienda productos agrícolas frescos de agricultores locales.";

            var conversations = new List<AiConversation>
            {
                new AiConversation { promptRole = PromptRole.system, message = systemPrompt, tokensUsed = 0, createdAt = now },
                new AiConversation { promptRole = PromptRole.user, message = request.prompt, tokensUsed = 0, createdAt = now },
                new AiConversation { promptRole = PromptRole.assistant, message = $"Recomendación simulada para: {request.prompt}", tokensUsed = 42, createdAt = now }
            };

            _db.AiConversations.AddRange(conversations);
            await _db.SaveChangesAsync();

            return Ok(new { response = conversations[2].message, tokensUsed = conversations[2].tokensUsed });
        }
    }
}
