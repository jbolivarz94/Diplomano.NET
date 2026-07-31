# Agromarket Local

**Autores:**
- **Jorge Bolivar (Backend)**
- **Bryan Pacheco (Backend IA)**
- **Lizeth Anay Bedoya Bolívar (QA)**


**AgroMarket Local** es una plataforma integral de comercio electrónico diseñada para conectar directamente a los agricultores con los consumidores urbanos, eliminando intermediarios y garantizando productos frescos con trazabilidad completa.

**Características:**
- **Catálogo de Productos:** Gestión completa de productos con categorías y unidades de medida.
- **Gestión de Pedidos:** Flujo completo de pedidos con estados.
- **Logística:** Gestión de entregas y despachos.
- **Reseñas:** Sistema de reseñas para productos.
- **IA:** Integración con GROQ API para análisis inteligente de datos.

## Esquema de Base de Datos

A continuación se muestra el diagrama de entidad-relación (ERD) de la base de datos generada a partir de `schema.sql`:

```mermaid
erDiagram
    farmer_profiles ||--o{ products : "offers"
    farmer_profiles ||--o{ orders : "receives"
    categories ||--o{ products : "categorizes"
    units_of_measure ||--o{ products : "measures"
    orders ||--o{ order_items : "contains"
    products ||--o{ order_items : "included_in"
    products ||--o{ reviews : "receives"
    
    farmer_profiles {
        INTEGER id PK
        TEXT farm_name
        TEXT description
        TEXT verification_status
        TEXT bank_account_info
        TEXT created_at
    }
    
    categories {
        INTEGER id PK
        TEXT name
        TEXT description
    }
    
    units_of_measure {
        INTEGER id PK
        TEXT name
        TEXT abbreviation
    }
    
    products {
        INTEGER id PK
        INTEGER farmer_profile_id FK
        INTEGER category_id FK
        INTEGER unit_of_measure_id FK
        TEXT name
        TEXT description
        REAL unit_price
        REAL stock_quantity
        INTEGER is_organic
        TEXT harvest_date
        INTEGER is_active
        TEXT created_at
    }
    
    orders {
        INTEGER id PK
        TEXT order_number
        INTEGER farmer_profile_id FK
        TEXT status
        REAL total_amount
        TEXT notes
        TEXT street_address
        TEXT municipality
        TEXT department
        TEXT additional_details
        TEXT delivery_type
        TEXT estimated_delivery_date
        TEXT delivered_at
        TEXT created_at
    }
    
    order_items {
        INTEGER id PK
        INTEGER order_id FK
        INTEGER product_id FK
        REAL quantity
        REAL unit_price
        REAL total_price
    }
    
    reviews {
        INTEGER id PK
        INTEGER product_id FK
        INTEGER rating
        TEXT comment
        TEXT created_at
    }
    
    ai_conversations {
        INTEGER id PK
        TEXT prompt_role
        TEXT message
        INTEGER tokens_used
        TEXT created_at
    }
```

## Estado Actual del Proyecto

El backend está construido con **ASP.NET Core 8 (.NET 8)** con la estructura base creada en `MarketPlace/`.

**Implementado:**
- **Capa de modelos** (`Models/`) con clases para las 8 tablas del esquema: `FarmerProfile`, `Categorie`, `UnitOfMeasure`, `Product`, `Order`, `OrderItem`, `Review` y `AiConversation`.
- **Enums** para las columnas con restricciones `CHECK`: `VerificationStatus`, `StatusOrder`, `DeliveryType` y `PromptRole`.
- **Controladores** (`Controllers/`) agrupados por dominio: perfiles de agricultores, catálogo (categorías, unidades, productos y reseñas), pedidos (incluye dirección de envío y entrega) e IA.
- **Capa de datos**: EF Core + SQLite (`Data/AppDbContext.cs`); la base de datos se crea automáticamente desde `schema.sql` al iniciar la aplicación por primera vez.
- **Swagger / OpenAPI** con documentación XML de todos los endpoints.

**Pendiente:**
- Autenticación y autorización por rol (Agricultor / Consumidor / Administrador).
- Integración real con la API de Groq para recomendaciones con IA (el endpoint `POST /api/ai/recommendations` responde actualmente con un stub).
- Módulo de pagos (fuera de alcance; la entrega se programa al crear la orden).

## Flujos del Sistema (Diagramas Mermaid)

En esta sección se presentan los flujos de interacción entre los clientes HTTP (Swagger, Postman o `MarketPlace.http`) y la API.

### 1. Registrar Productos (Agricultor)
Este flujo representa cómo un agricultor agrega un nuevo producto a su catálogo disponible en la plataforma.

```mermaid
sequenceDiagram
    autonumber
    actor Agricultor
    participant API as "Backend (API)"
    participant DB as "Base de Datos"

    Agricultor->>API: POST /api/products (datos del producto)
    API->>DB: Validar existencia de Farmer, Category y Unit of Measure
    DB-->>API: OK (Referencias válidas)
    API->>DB: INSERT INTO products
    DB-->>API: OK (Producto Creado)
    API-->>Agricultor: 201 Created (Detalles del producto)
```

---

### 2. Registrar Órdenes de Compra (Consumidor)
Este flujo detalla cómo un consumidor genera un pedido de compra mediante una solicitud HTTP.

```mermaid
sequenceDiagram
    autonumber
    actor Consumidor
    participant API as "Backend (API)"
    participant DB as "Base de Datos"

    Consumidor->>API: POST /api/orders (Dirección, Tipo de entrega, Items y Cantidades)
    API->>DB: Validar agricultor y consultar stock de los productos
    DB-->>API: Retorna stock actual y precios
    Note over API: Valida stock suficiente y calcula el Total<br/>de la orden (suma de items)
    API->>DB: INSERT INTO orders (Status: 'Pending', dirección y entrega)
    API->>DB: INSERT INTO order_items (por cada item)
    API->>DB: UPDATE products SET stock_quantity = stock_quantity - Qty
    DB-->>API: Transacción Exitosa
    API-->>Consumidor: 201 Created (Detalles del Pedido e ID)
```

---

### 3. Entrega de la Orden (Agricultor / Repartidor)
Lógica para actualizar el estado de la orden y registrar los datos de entrega.

```mermaid
sequenceDiagram
    autonumber
    actor Repartidor
    participant API as "Backend (API)"
    participant DB as "Base de Datos"

    Repartidor->>API: PATCH /api/orders/{id}/status (Preparing, InTransit, Delivered)
    API->>DB: UPDATE orders SET status
    DB-->>API: OK (Actualizado)
    API-->>Repartidor: 204 NoContent
    Note over Repartidor: Al despachar/entregar se registra la fecha real
    Repartidor->>API: PATCH /api/orders/{id}/delivery (fecha estimada, fecha real, notas)
    API->>DB: UPDATE orders SET estimated_delivery_date, delivered_at, notes
    DB-->>API: OK (Actualizado)
    API-->>Repartidor: 204 NoContent
```

---

### 4. Registro de Reseña (Consumidor)
Lógica para que un consumidor califique un producto adquirido.

```mermaid
sequenceDiagram
    autonumber
    actor Consumidor
    participant API as "Backend (API)"
    participant DB as "Base de Datos"

    Consumidor->>API: POST /api/products/{id}/reviews (Rating, Comment)
    Note over API: Valida rango de Rating (1 a 5)
    API->>DB: INSERT INTO reviews
    DB-->>API: OK (Guardado)
    API-->>Consumidor: 201 Created (Reseña Registrada)
```

---

### 5. Recomendaciones con IA (Consumidor & Backend IA)
Interacción con la API de Groq para sugerir productos o analizar datos usando modelos de lenguaje (diseño objetivo; el endpoint responde actualmente con un stub).

```mermaid
sequenceDiagram
    autonumber
    actor Consumidor
    participant API as "Backend (API)"
    participant DB as "Base de Datos"
    participant Groq as "Groq API (LLM)"

    Consumidor->>API: POST /api/ai/recommendations (User Prompt)
    API->>DB: Consultar catálogo de productos, compras del usuario y reviews
    DB-->>API: Retorna datos agregados de contexto
    Note over API: Prepara System Prompt con el contexto del<br/>negocio y datos del catálogo
    API->>Groq: Enviar Prompt de Sistema + Prompt de Usuario + Contexto de Productos
    Groq-->>API: Retorna respuesta inteligente (Recomendación)
    API->>DB: INSERT INTO ai_conversations (Guardar log de tokens e interacción)
    DB-->>API: OK (Guardado)
    API-->>Consumidor: 200 OK (Respuesta generada por la IA)
```

## Endpoints de la API

La siguiente tabla detalla los posibles endpoints que componen la API del sistema:

| Módulo | Método | Endpoint | Descripción | Acceso / Rol |
| :--- | :---: | :--- | :--- | :--- |
| **Identidad / Perfiles** | `POST` | `/api/farmer-profiles` | Registrar un nuevo perfil de agricultor | Público |
| | `GET` | `/api/farmer-profiles/{id}` | Obtener información detallada de un agricultor | Público |
| **Catálogo** | `GET` | `/api/categories` | Listar todas las categorías de productos | Público |
| | `GET` | `/api/units-of-measure` | Listar todas las unidades de medida | Público |
| | `GET` | `/api/products` | Buscar y listar productos (filtros por categoría, orgánico y agricultor) | Público |
| | `GET` | `/api/products/{id}` | Obtener el detalle de un producto específico | Público |
| | `POST` | `/api/products` | Publicar un nuevo producto en el catálogo | Agricultor |
| | `GET` | `/api/products/{id}/reviews` | Obtener las reseñas y valoración promedio de un producto | Público |
| | `POST` | `/api/products/{id}/reviews` | Registrar calificación y comentario para un producto | Público |
| **Pedidos** | `POST` | `/api/orders` | Crear una orden de compra con dirección de envío, tipo de entrega y artículos | Público |
| | `GET` | `/api/orders` | Listar todas las órdenes | Público |
| | `GET` | `/api/orders/{id}` | Obtener detalles y artículos (`order_items`) de un pedido | Público |
| | `PATCH` | `/api/orders/{id}/status` | Cambiar el estado del pedido (`Pending`, `Confirmed`, `Preparing`, `InTransit`, `Delivered`, `Cancelled`) | Público |
| | `PATCH` | `/api/orders/{id}/delivery` | Actualizar datos de entrega (fecha estimada, fecha real, notas) | Público |
| **IA (Groq)** | `POST` | `/api/ai/recommendations` | Obtener sugerencias de productos personalizadas o análisis de precios | Público |
