using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace market_place
{
    /// <summary>
    /// Catálogo: categorías, unidades de medida, productos y reseñas.
    /// </summary>
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Inicializa el controlador con el contexto de base de datos.
        /// </summary>
        /// <param name="db">Contexto de Entity Framework Core (AppDbContext).</param>
        public ProductsController(AppDbContext db) => _db = db;

        /// <summary>
        /// Lista todas las categorías de productos.
        /// </summary>
        /// <returns>Lista de categorías registradas en el catálogo.</returns>
        [HttpGet("/api/categories")]
        public async Task<ActionResult<List<Categorie>>> ListCategories()
            => await _db.Categories.ToListAsync();

        /// <summary>
        /// Lista todas las unidades de medida.
        /// </summary>
        /// <returns>Lista de unidades de medida disponibles (kg, lb, atado, unidad, caja).</returns>
        [HttpGet("/api/units-of-measure")]
        public async Task<ActionResult<List<UnitOfMeasure>>> ListUnitsOfMeasure()
            => await _db.UnitsOfMeasure.ToListAsync();

        /// <summary>
        /// Lista productos activos con filtros opcionales por categoría, orgánico y agricultor.
        /// </summary>
        /// <param name="categoryId">Filtro opcional: ID de la categoría del producto.</param>
        /// <param name="isOrganic">Filtro opcional: 1 para solo orgánicos, 0 para solo no orgánicos.</param>
        /// <param name="farmerProfileId">Filtro opcional: ID del perfil de agricultor que publica el producto.</param>
        /// <returns>Lista de productos activos (isActive = 1) que cumplen los filtros.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll(
            [FromQuery] int? categoryId,
            [FromQuery] int? isOrganic,
            [FromQuery] int? farmerProfileId)
        {
            var query = _db.Products.Where(p => p.isActive == 1);
            if (categoryId.HasValue)
                query = query.Where(p => p.categoryId == categoryId);
            if (isOrganic.HasValue)
                query = query.Where(p => p.isOrganic == isOrganic);
            if (farmerProfileId.HasValue)
                query = query.Where(p => p.farmerProfileId == farmerProfileId);
            return await query.ToListAsync();
        }

        /// <summary>
        /// Obtiene el detalle de un producto por su ID.
        /// </summary>
        /// <param name="id">ID numérico del producto.</param>
        /// <returns>El producto solicitado, o 404 si no existe.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product is null) return NotFound();
            return Ok(product);
        }

        /// <summary>
        /// Publica un nuevo producto en el catálogo.
        /// </summary>
        /// <param name="product">Datos del producto: agricultor, categoría, unidad de medida, precio y stock.</param>
        /// <returns>201 Created con el producto registrado (el ID se genera automáticamente), o 400 si las referencias no existen o los valores son inválidos.</returns>
        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            var farmer = await _db.FarmerProfiles.FindAsync(product.farmerProfileId);
            if (farmer is null) return BadRequest("El agricultor no existe");

            var category = await _db.Categories.FindAsync(product.categoryId);
            if (category is null) return BadRequest("La categoría no existe");

            var unit = await _db.UnitsOfMeasure.FindAsync(product.unitOfMeasureId);
            if (unit is null) return BadRequest("La unidad de medida no existe");

            if (product.unitPrice < 0) return BadRequest("El precio no puede ser negativo");
            if (product.stockQuantity < 0) return BadRequest("El stock no puede ser negativo");

            product.createdAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            product.isActive = 1;
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.id }, product);
        }

        /// <summary>
        /// Obtiene las reseñas y la valoración promedio de un producto.
        /// </summary>
        /// <param name="productId">ID numérico del producto.</param>
        /// <returns>Objeto con la lista de reseñas y el promedio de calificaciones (0 si no hay reseñas).</returns>
        [HttpGet("{productId}/reviews")]
        public async Task<ActionResult<ProductReviewsResponse>> GetReviews(int productId)
        {
            var reviews = await _db.Reviews.Where(r => r.productId == productId).ToListAsync();
            var promedio = reviews.Count == 0 ? 0 : reviews.Average(r => r.rating);
            return Ok(new ProductReviewsResponse { reviews = reviews, averageRating = promedio });
        }

        /// <summary>
        /// Registra una calificación (1-5) y comentario para un producto.
        /// </summary>
        /// <param name="productId">ID numérico del producto a calificar.</param>
        /// <param name="review">Datos de la reseña: rating (1 a 5) y comentario.</param>
        /// <returns>201 Created con la reseña registrada, o 400 si el producto no existe o el rating está fuera del rango.</returns>
        [HttpPost("{productId}/reviews")]
        public async Task<ActionResult<Review>> CreateReview(int productId, Review review)
        {
            var product = await _db.Products.FindAsync(productId);
            if (product is null) return BadRequest("El producto no existe");
            if (review.rating < 1 || review.rating > 5) return BadRequest("El rating debe estar entre 1 y 5");

            review.productId = productId;
            review.createdAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReviews), new { productId = review.productId }, review);
        }
    }
}
