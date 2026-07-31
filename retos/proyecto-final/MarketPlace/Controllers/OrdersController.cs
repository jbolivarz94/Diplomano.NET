using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace market_place
{
    /// <summary>
    /// Gestión de órdenes de compra: creación, estados y entrega.
    /// </summary>
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Inicializa el controlador con el contexto de base de datos.
        /// </summary>
        /// <param name="db">Contexto de Entity Framework Core (AppDbContext).</param>
        public OrdersController(AppDbContext db) => _db = db;

        /// <summary>
        /// Lista todas las órdenes de compra.
        /// </summary>
        /// <returns>Lista de órdenes registradas.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAll()
            => await _db.Orders.ToListAsync();

        /// <summary>
        /// Obtiene el detalle de una orden con sus artículos (order_items).
        /// </summary>
        /// <param name="id">ID numérico de la orden.</param>
        /// <returns>Objeto con la orden y su lista de artículos, o 404 si la orden no existe.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetById(int id)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.id == id);
            if (order is null) return NotFound();
            var items = await _db.OrderItems.Where(i => i.orderId == id).ToListAsync();
            return Ok(new { order = order, items = items });
        }

        /// <summary>
        /// Crea una orden de compra: valida stock, calcula el total, descuenta inventario y programa la entrega.
        /// </summary>
        /// <param name="request">Datos de la orden: agricultor, dirección de envío, tipo de entrega y artículos solicitados.</param>
        /// <returns>201 Created con la orden registrada (ID generado automáticamente), o 400 si los datos son inválidos, el agricultor no existe o hay stock insuficiente.</returns>
        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderRequest request)
        {
            if (request.items is null || request.items.Count == 0)
                return BadRequest("La orden debe contener al menos un artículo");

            if (string.IsNullOrWhiteSpace(request.streetAddress) || string.IsNullOrWhiteSpace(request.municipality) || string.IsNullOrWhiteSpace(request.department))
                return BadRequest("La dirección de envío (calle, municipio y departamento) es obligatoria");

            var farmer = await _db.FarmerProfiles.FindAsync(request.farmerProfileId);
            if (farmer is null) return BadRequest("El agricultor no existe");

            var order = new Order
            {
                orderNumber = $"ORD-{DateTime.Now:yyyyMMddHHmmss}-{Guid.NewGuid().ToString()[..4].ToUpper()}",
                farmerProfileId = request.farmerProfileId,
                status = StatusOrder.Pending,
                totalAmount = 0,
                notes = request.notes,
                streetAddress = request.streetAddress,
                municipality = request.municipality,
                department = request.department,
                additionalDetails = request.additionalDetails,
                deliveryType = request.deliveryType ?? DeliveryType.DirectHomeDelivery,
                estimatedDeliveryDate = request.estimatedDeliveryDate,
                createdAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            var items = new List<OrderItem>();
            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();

                foreach (var item in request.items)
                {
                    var product = await _db.Products.FirstOrDefaultAsync(p => p.id == item.productId && p.isActive == 1);
                    if (product is null)
                        return BadRequest($"El producto {item.productId} no existe o está inactivo");
                    if (product.stockQuantity < item.quantity)
                        return BadRequest($"Stock insuficiente para el producto {product.name}");

                    product.stockQuantity -= item.quantity;

                    items.Add(new OrderItem
                    {
                        orderId = order.id,
                        productId = product.id,
                        quantity = item.quantity,
                        unitPrice = product.unitPrice,
                        totalPrice = item.quantity * product.unitPrice
                    });
                    order.totalAmount += items[^1].totalPrice;
                }

                order.totalAmount = items.Sum(i => i.totalPrice);
                _db.OrderItems.AddRange(items);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }

            return CreatedAtAction(nameof(GetById), new { id = order.id }, order);
        }

        /// <summary>
        /// Cambia el estado de una orden (Pending, Confirmed, Preparing, InTransit, Delivered, Cancelled).
        /// </summary>
        /// <param name="id">ID numérico de la orden.</param>
        /// <param name="request">Nuevo estado de la orden.</param>
        /// <returns>204 NoContent si se actualizó correctamente, o 404 si la orden no existe.</returns>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, StatusUpdateRequest request)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order is null) return NotFound();
            order.status = request.status;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Actualiza los datos de entrega de la orden (fecha estimada, fecha real, notas).
        /// </summary>
        /// <param name="id">ID numérico de la orden.</param>
        /// <param name="request">Datos de entrega: fecha estimada, fecha real de entrega y notas.</param>
        /// <returns>204 NoContent si se actualizó correctamente, o 404 si la orden no existe.</returns>
        [HttpPatch("{id}/delivery")]
        public async Task<IActionResult> UpdateDelivery(int id, DeliveryUpdateRequest request)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order is null) return NotFound();
            order.estimatedDeliveryDate = request.estimatedDeliveryDate;
            order.deliveredAt = request.deliveredAt;
            order.notes = request.notes;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
