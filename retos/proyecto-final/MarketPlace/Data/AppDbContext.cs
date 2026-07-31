using Microsoft.EntityFrameworkCore;

namespace market_place
{
    /// <summary>
    /// Contexto de Entity Framework Core: expone las entidades de AgroMarket Local
    /// y mapea cada una a su tabla física del esquema SQLite (snake_case).
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Inicializa el contexto con las opciones de configuración.
        /// </summary>
        /// <param name="options">Opciones del contexto (proveedor SQLite y cadena de conexión).</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>Perfiles de agricultores (tabla farmer_profiles).</summary>
        public DbSet<FarmerProfile> FarmerProfiles => Set<FarmerProfile>();

        /// <summary>Categorías de productos (tabla categories).</summary>
        public DbSet<Categorie> Categories => Set<Categorie>();

        /// <summary>Unidades de medida (tabla units_of_measure).</summary>
        public DbSet<UnitOfMeasure> UnitsOfMeasure => Set<UnitOfMeasure>();

        /// <summary>Productos del catálogo (tabla products).</summary>
        public DbSet<Product> Products => Set<Product>();

        /// <summary>Órdenes de compra (tabla orders).</summary>
        public DbSet<Order> Orders => Set<Order>();

        /// <summary>Artículos de las órdenes (tabla order_items).</summary>
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        /// <summary>Reseñas de productos (tabla reviews).</summary>
        public DbSet<Review> Reviews => Set<Review>();

        /// <summary>Conversaciones con la IA (tabla ai_conversations).</summary>
        public DbSet<AiConversation> AiConversations => Set<AiConversation>();

        /// <summary>
        /// Configura el modelo: nombres de tablas y columnas (snake_case),
        /// llaves primarias, llaves foráneas, índices y conversiones de enums a texto.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo de EF Core.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FarmerProfile>(e =>
            {
                e.ToTable("farmer_profiles");
                e.HasKey(f => f.id);
                e.Property(f => f.farmName).HasColumnName("farm_name");
                e.Property(f => f.verificationStatus).HasColumnName("verification_status").HasConversion<string>();
                e.Property(f => f.bankAccountInfo).HasColumnName("bank_account_info");
                e.Property(f => f.createdAt).HasColumnName("created_at");
            });

            modelBuilder.Entity<Categorie>(e =>
            {
                e.ToTable("categories");
                e.HasKey(c => c.id);
            });

            modelBuilder.Entity<UnitOfMeasure>(e =>
            {
                e.ToTable("units_of_measure");
                e.HasKey(u => u.id);
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("products");
                e.HasKey(p => p.id);
                e.Property(p => p.farmerProfileId).HasColumnName("farmer_profile_id");
                e.Property(p => p.categoryId).HasColumnName("category_id");
                e.Property(p => p.unitOfMeasureId).HasColumnName("unit_of_measure_id");
                e.Property(p => p.unitPrice).HasColumnName("unit_price");
                e.Property(p => p.stockQuantity).HasColumnName("stock_quantity");
                e.Property(p => p.isOrganic).HasColumnName("is_organic");
                e.Property(p => p.harvestDate).HasColumnName("harvest_date");
                e.Property(p => p.isActive).HasColumnName("is_active");
                e.Property(p => p.createdAt).HasColumnName("created_at");
                e.HasOne<FarmerProfile>()
                    .WithMany()
                    .HasForeignKey(p => p.farmerProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne<Categorie>()
                    .WithMany()
                    .HasForeignKey(p => p.categoryId);
                e.HasOne<UnitOfMeasure>()
                    .WithMany()
                    .HasForeignKey(p => p.unitOfMeasureId);
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("orders");
                e.HasKey(o => o.id);
                e.Property(o => o.orderNumber).HasColumnName("order_number");
                e.Property(o => o.farmerProfileId).HasColumnName("farmer_profile_id");
                e.Property(o => o.status).HasColumnName("status").HasConversion<string>();
                e.Property(o => o.totalAmount).HasColumnName("total_amount");
                e.Property(o => o.streetAddress).HasColumnName("street_address");
                e.Property(o => o.additionalDetails).HasColumnName("additional_details");
                e.Property(o => o.deliveryType).HasColumnName("delivery_type").HasConversion<string>();
                e.Property(o => o.estimatedDeliveryDate).HasColumnName("estimated_delivery_date");
                e.Property(o => o.deliveredAt).HasColumnName("delivered_at");
                e.Property(o => o.createdAt).HasColumnName("created_at");
                e.HasIndex(o => o.orderNumber).IsUnique();
                e.HasOne<FarmerProfile>()
                    .WithMany()
                    .HasForeignKey(o => o.farmerProfileId);
            });

            modelBuilder.Entity<OrderItem>(e =>
            {
                e.ToTable("order_items");
                e.HasKey(i => i.id);
                e.Property(i => i.orderId).HasColumnName("order_id");
                e.Property(i => i.productId).HasColumnName("product_id");
                e.Property(i => i.unitPrice).HasColumnName("unit_price");
                e.Property(i => i.totalPrice).HasColumnName("total_price");
                e.HasOne<Order>()
                    .WithMany()
                    .HasForeignKey(i => i.orderId)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(i => i.productId);
            });

            modelBuilder.Entity<Review>(e =>
            {
                e.ToTable("reviews");
                e.HasKey(r => r.id);
                e.Property(r => r.productId).HasColumnName("product_id");
                e.Property(r => r.createdAt).HasColumnName("created_at");
                e.HasOne<Product>()
                    .WithMany()
                    .HasForeignKey(r => r.productId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AiConversation>(e =>
            {
                e.ToTable("ai_conversations");
                e.HasKey(a => a.id);
                e.Property(a => a.promptRole).HasColumnName("prompt_role").HasConversion<string>();
                e.Property(a => a.tokensUsed).HasColumnName("tokens_used");
                e.Property(a => a.createdAt).HasColumnName("created_at");
            });
        }
    }
}
