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
        private readonly IGroqService _groq;

        private readonly AppDbContext _db;

        /// <summary>
        /// Inicializa el controlador con el contexto de base de datos.
        /// </summary>
        /// <param name="db">Contexto de Entity Framework Core (AppDbContext).</param>
        /// <param name="groq">Servicio de conexión a la API de Groq.</param>
        public AiController(AppDbContext db, IGroqService groq)
        {
            _db = db;
            _groq = groq;
        }

        /// <summary>
        /// Genera recomendaciones de productos a partir de un prompt del usuario.
        /// </summary>
        /// <param name="request">Solicitud con el prompt del usuario para generar recomendaciones.</param>
        /// <returns>200 OK con la respuesta de la IA y los tokens utilizados.</returns>
        [HttpPost("recommendations")]
        public async Task<ActionResult<object>> GetRecommendations(AiRequest request)
        {

            if (request == null || string.IsNullOrWhiteSpace(request.prompt))
            {
                return BadRequest("Debe enviar un prompt.");
            }

            var products = await (
                from p in _db.Products
                join c in _db.Categories
                    on p.categoryId equals c.id
                join u in _db.UnitsOfMeasure
                    on p.unitOfMeasureId equals u.id
                join f in _db.FarmerProfiles
                    on p.farmerProfileId equals f.id
                where p.isActive == 1
                select new
                {
                    p.id,
                    p.farmerProfileId,
                    farmer = f.farmName,

                    p.categoryId,
                    category = c.name,

                    p.unitOfMeasureId,
                    unitOfMeasure = u.name,

                    p.name,
                    p.description,
                    p.unitPrice,
                    p.stockQuantity,
                    p.isOrganic,
                    p.harvestDate,
                    p.isActive,
                    p.createdAt,

                    AverageRating = _db.Reviews
                        .Where(r => r.productId == p.id)
                        .Average(r => (double?)r.rating) ?? 0,

                    Reviews = _db.Reviews
                        .Count(r => r.productId == p.id)
                })
                .OrderByDescending(x => x.AverageRating)
                .Take(20)
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound("No hay productos disponibles.");
            }

            var context = string.Join("\n\n",
                products.Select(p =>
            $"""
            ID: {p.id}
            Producto: {p.name}
            Descripción: {p.description}
            Categoría: {p.category}
            Productor: {p.farmer}
            Unidad de medida: {p.unitOfMeasure}
            Precio: {p.unitPrice}
            Stock disponible: {p.stockQuantity}
            Orgánico: {(p.isOrganic == 1 ? "Sí" : "No")}
            Fecha de cosecha: {p.harvestDate}
            Fecha de publicación: {p.createdAt}
            Calificación promedio: {p.AverageRating:F1}
            Cantidad de reseñas: {p.Reviews}
            """));

            var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

            var prompt = $"""
                        Eres un experto en productos agrícolas.

                        Estos son los productos disponibles en AgroMarket.

                        {context}

                        Pregunta del cliente:

                        {request.prompt}

                        Reglas:

                        - Recomienda únicamente productos existentes.
                        - Prioriza productos con mejor calificación.
                        - Considera si el producto es orgánico.
                        - Considera el stock.
                        - Explica por qué recomiendas cada producto.
                        - Si ningún producto aplica, indícalo claramente.
                        """;

            var answer = await _groq.AskAsync(prompt);

            var conversations = new List<AiConversation>
            {
                new AiConversation { promptRole = PromptRole.system, message = prompt, tokensUsed = 0, createdAt = now },
                new AiConversation { promptRole = PromptRole.user, message = request.prompt, tokensUsed = 0, createdAt = now },
                new AiConversation { promptRole = PromptRole.assistant, message = answer, tokensUsed = 42, createdAt = now }
            };

            _db.AiConversations.AddRange(conversations);
            await _db.SaveChangesAsync();

            return Ok(new { response = conversations[2].message });
        }
    }
}
