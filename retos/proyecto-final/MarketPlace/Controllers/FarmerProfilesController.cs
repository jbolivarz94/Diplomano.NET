using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace market_place
{
    /// <summary>
    /// Gestión de perfiles de agricultores.
    /// </summary>
    [ApiController]
    [Route("api/farmer-profiles")]
    public class FarmerProfilesController : ControllerBase
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Inicializa el controlador con el contexto de base de datos.
        /// </summary>
        /// <param name="db">Contexto de Entity Framework Core (AppDbContext).</param>
        public FarmerProfilesController(AppDbContext db) => _db = db;

        /// <summary>
        /// Obtiene todos los perfiles de agricultores registrados.
        /// </summary>
        /// <returns>Lista de perfiles de agricultores, o 404 si no hay registros.</returns>
        [HttpGet]
        public async Task<ActionResult<FarmerProfile>> GetAll()
        {
            var farmers = await _db.FarmerProfiles.ToListAsync();
            if (farmers is null) return NotFound();
            return Ok(farmers);
        }

        /// <summary>
        /// Obtiene el perfil de un agricultor por su ID.
        /// </summary>
        /// <param name="id">ID numérico del perfil de agricultor.</param>
        /// <returns>El perfil solicitado, o 404 si no existe.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<FarmerProfile>> GetById(int id)
        {
            var farmer = await _db.FarmerProfiles.FindAsync(id);
            if (farmer is null) return NotFound();
            return Ok(farmer);
        }

        /// <summary>
        /// Registra un nuevo perfil de agricultor.
        /// </summary>
        /// <param name="farmer">Datos del perfil: nombre de la finca, descripción, estado de verificación e información bancaria.</param>
        /// <returns>201 Created con el perfil registrado (el ID se genera automáticamente).</returns>
        [HttpPost]
        public async Task<ActionResult<FarmerProfile>> Create(FarmerProfile farmer)
        {
            farmer.createdAt = DateTime.UtcNow;
            _db.FarmerProfiles.Add(farmer);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = farmer.id }, farmer);
        }
    }
}
