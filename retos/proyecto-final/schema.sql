-- ========================================================
-- AGROMARKET LOCAL - SCRIPT BASE DE DATOS SQLITE
-- ========================================================

PRAGMA foreign_keys = ON;

-- 1. IDENTIDAD
CREATE TABLE IF NOT EXISTS addresses (
    id TEXT PRIMARY KEY,
    farmer_profile_id TEXT NOT NULL,
    street_address TEXT NOT NULL,
    municipality TEXT NOT NULL,
    department TEXT NOT NULL,
    additional_details TEXT,
    latitude REAL,
    longitude REAL,
    is_default INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (farmer_profile_id) REFERENCES farmer_profiles(id) ON DELETE CASCADE
);

-- 2. AGRICULTORES
CREATE TABLE IF NOT EXISTS farmer_profiles (
    id TEXT PRIMARY KEY,
    farm_name TEXT NOT NULL,
    description TEXT,
    verification_status TEXT NOT NULL DEFAULT 'Pending' CHECK (verification_status IN ('Pending', 'Approved', 'Rejected')),
    bank_account_info TEXT,
    profile_image_url TEXT,
    created_at TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP)
);

-- 3. CATÁLOGO
CREATE TABLE IF NOT EXISTS categories (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL UNIQUE,
    description TEXT,
    image_url TEXT
);

CREATE TABLE IF NOT EXISTS units_of_measure (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    name TEXT NOT NULL,
    abbreviation TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS products (
    id TEXT PRIMARY KEY,
    farmer_profile_id TEXT NOT NULL,
    category_id INTEGER NOT NULL,
    unit_of_measure_id INTEGER NOT NULL,
    name TEXT NOT NULL,
    description TEXT,
    unit_price REAL NOT NULL CHECK (unit_price >= 0),
    stock_quantity REAL NOT NULL DEFAULT 0 CHECK (stock_quantity >= 0),
    is_organic INTEGER NOT NULL DEFAULT 0,
    harvest_date TEXT,
    image_url TEXT,
    is_active INTEGER NOT NULL DEFAULT 1,
    created_at TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    FOREIGN KEY (farmer_profile_id) REFERENCES farmer_profiles(id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES categories(id),
    FOREIGN KEY (unit_of_measure_id) REFERENCES units_of_measure(id)
);

-- 4. PEDIDOS
CREATE TABLE IF NOT EXISTS orders (
    id TEXT PRIMARY KEY,
    order_number TEXT NOT NULL UNIQUE,
    farmer_profile_id TEXT NOT NULL,
    shipping_address_id TEXT NOT NULL,
    status TEXT NOT NULL DEFAULT 'Pending' CHECK (status IN ('Pending', 'Confirmed', 'Preparing', 'InTransit', 'Delivered', 'Cancelled')),
    subtotal REAL NOT NULL CHECK (subtotal >= 0),
    shipping_fee REAL NOT NULL DEFAULT 0 CHECK (shipping_fee >= 0),
    total_amount REAL NOT NULL CHECK (total_amount >= 0),
    notes TEXT,
    created_at TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    FOREIGN KEY (farmer_profile_id) REFERENCES farmer_profiles(id),
    FOREIGN KEY (shipping_address_id) REFERENCES addresses(id)
);

CREATE TABLE IF NOT EXISTS order_items (
    id TEXT PRIMARY KEY,
    order_id TEXT NOT NULL,
    product_id TEXT NOT NULL,
    quantity REAL NOT NULL CHECK (quantity > 0),
    unit_price REAL NOT NULL CHECK (unit_price >= 0),
    total_price REAL NOT NULL CHECK (total_price >= 0),
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(id)
);

-- 5. PAGOS Y LOGÍSTICA
CREATE TABLE IF NOT EXISTS payments (
    id TEXT PRIMARY KEY,
    order_id TEXT NOT NULL UNIQUE,
    method TEXT NOT NULL CHECK (method IN ('CashOnDelivery', 'BankTransfer', 'DigitalWallet')),
    status TEXT NOT NULL DEFAULT 'Pending' CHECK (status IN ('Pending', 'Approved', 'Rejected')),
    transaction_reference TEXT,
    paid_at TEXT,
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS deliveries (
    id TEXT PRIMARY KEY,
    order_id TEXT NOT NULL UNIQUE,
    delivery_type TEXT NOT NULL CHECK (delivery_type IN ('FarmPickup', 'DirectHomeDelivery', 'LocalMarketPoint')),
    estimated_delivery_date TEXT,
    delivered_at TEXT,
    notes TEXT,
    FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE
);

-- 6. RESEÑAS E IA (GROQ API)
CREATE TABLE IF NOT EXISTS reviews (
    id TEXT PRIMARY KEY,
    product_id TEXT NOT NULL,
    rating INTEGER NOT NULL CHECK (rating BETWEEN 1 AND 5),
    comment TEXT,
    created_at TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP),
    FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS ai_conversations (
    id TEXT PRIMARY KEY,
    prompt_role TEXT NOT NULL CHECK (prompt_role IN ('system', 'user', 'assistant')),
    message TEXT NOT NULL,
    tokens_used INTEGER DEFAULT 0,
    created_at TEXT NOT NULL DEFAULT (CURRENT_TIMESTAMP)
);

-- ÍNDICES DE RENDIMIENTO
CREATE INDEX IF NOT EXISTS idx_products_farmer ON products(farmer_profile_id);
CREATE INDEX IF NOT EXISTS idx_products_category ON products(category_id);
CREATE INDEX IF NOT EXISTS idx_orders_farmer ON orders(farmer_profile_id);
CREATE INDEX IF NOT EXISTS idx_order_items_order ON order_items(order_id);
CREATE INDEX IF NOT EXISTS idx_reviews_product ON reviews(product_id);

-- SEED DATA INICIAL
INSERT OR IGNORE INTO categories (id, name, description) VALUES
(1, 'Frutas', 'Frutas frescas cosechadas en fincas locales'),
(2, 'Hortalizas y Verduras', 'Verduras, verduras de hoja y legumbres frescas'),
(3, 'Tubérculos y Raíces', 'Papa, yuca, plátano, camote y tubérculos autóctonos'),
(4, 'Lácteos y Derivados', 'Queso campesino, leche fresca, yogur artesanal');

INSERT OR IGNORE INTO units_of_measure (id, name, abbreviation) VALUES
(1, 'Kilogramo', 'kg'),
(2, 'Libra', 'lb'),
(3, 'Atado / Manojo', 'atd'),
(4, 'Unidad', 'ud'),
(5, 'Caja (10 kg)', 'cj');
