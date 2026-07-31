# Agromarket Local

**Autores:**
- **Jorge Bolivar**
- **Bryan Pacheco**
- **Lizeth Anay Bedoya Bolívar**


**AgroMarket Local** es una plataforma integral de comercio electrónico diseñada para conectar directamente a los agricultores con los consumidores urbanos, eliminando intermediarios y garantizando productos frescos con trazabilidad completa.

**Características:**
- **Catálogo de Productos:** Gestión completa de productos con categorías y unidades de medida.
- **Gestión de Pedidos:** Flujo completo de pedidos con estados y notificaciones.
- **Pagos y Logística:** Integración de pagos y gestión de entregas.
- **Reseñas:** Sistema de reseñas para productos.
- **IA:** Integración con GROQ API para análisis inteligente de datos.

## Esquema de Base de Datos

A continuación se muestra el diagrama de entidad-relación (ERD) de la base de datos generada a partir de `schema.sql`:

```mermaid
erDiagram
    farmer_profiles ||--o{ addresses : "has"
    farmer_profiles ||--o{ products : "offers"
    farmer_profiles ||--o{ orders : "receives"
    addresses ||--o{ orders : "used_for_shipping"
    categories ||--o{ products : "categorizes"
    units_of_measure ||--o{ products : "measures"
    orders ||--o{ order_items : "contains"
    products ||--o{ order_items : "included_in"
    orders ||--o| payments : "has"
    orders ||--o| deliveries : "has"
    products ||--o{ reviews : "receives"
    
    addresses {
        TEXT id PK
        TEXT farmer_profile_id FK
        TEXT street_address
        TEXT municipality
        TEXT department
        TEXT additional_details
        REAL latitude
        REAL longitude
        INTEGER is_default
    }
    
    farmer_profiles {
        TEXT id PK
        TEXT farm_name
        TEXT description
        TEXT verification_status
        TEXT bank_account_info
        TEXT profile_image_url
        TEXT created_at
    }
    
    categories {
        INTEGER id PK
        TEXT name
        TEXT description
        TEXT image_url
    }
    
    units_of_measure {
        INTEGER id PK
        TEXT name
        TEXT abbreviation
    }
    
    products {
        TEXT id PK
        TEXT farmer_profile_id FK
        INTEGER category_id FK
        INTEGER unit_of_measure_id FK
        TEXT name
        TEXT description
        REAL unit_price
        REAL stock_quantity
        INTEGER is_organic
        TEXT harvest_date
        TEXT image_url
        INTEGER is_active
        TEXT created_at
    }
    
    orders {
        TEXT id PK
        TEXT order_number
        TEXT farmer_profile_id FK
        TEXT shipping_address_id FK
        TEXT status
        REAL subtotal
        REAL shipping_fee
        REAL total_amount
        TEXT notes
        TEXT created_at
    }
    
    order_items {
        TEXT id PK
        TEXT order_id FK
        TEXT product_id FK
        REAL quantity
        REAL unit_price
        REAL total_price
    }
    
    payments {
        TEXT id PK
        TEXT order_id FK
        TEXT method
        TEXT status
        TEXT transaction_reference
        TEXT paid_at
    }
    
    deliveries {
        TEXT id PK
        TEXT order_id FK
        TEXT delivery_type
        TEXT estimated_delivery_date
        TEXT delivered_at
        TEXT notes
    }
    
    reviews {
        TEXT id PK
        TEXT product_id FK
        INTEGER rating
        TEXT comment
        TEXT created_at
    }
    
    ai_conversations {
        TEXT id PK
        TEXT prompt_role
        TEXT message
        INTEGER tokens_used
        TEXT created_at
    }
```
