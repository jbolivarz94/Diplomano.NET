# QA — Plan de Pruebas de los Servicios

**Proyecto:** AgroMarket Local API (MarketPlace)
**Dirección:** `http://localhost:5008`
**Fecha:** 2026-08-03
**Realizado por:** QA - Lizeth Bedoya

## Alcance

Se prueban los 16 servicios que expone la aplicación. La base de datos es SQLite y se crea sola con información de ejemplo la primera vez que se enciende la aplicación.

> **Nota:** las pruebas de creación guardan información de forma permanente. Para repetir todo el proceso desde cero, borrar el archivo `marketplace.db` y volver a encender la aplicación.

## Convención de estados esperados

| Código | Significado |
|--------|-------------|
| 200 | Todo correcto (consultas) |
| 201 | Se creó el registro correctamente |
| 204 | Se actualizó correctamente |
| 400 | La información enviada no es válida |
| 404 | El registro buscado no existe |
| 500 | Error interno |

---

## 1. Catálogo — Categorías y Unidades de Medida

| ID | Servicio | Caso | Resultado esperado | Prioridad |
|----|----------|------|--------------------|-----------|
| TC-01 | `GET /api/categories` | Ver las categorías precargadas | **200** + lista con las categorías de ejemplo (p.ej. Frutas, Verduras) | Alta |
| TC-02 | `GET /api/units-of-measure` | Ver las unidades de medida precargadas | **200** + lista (kg, lb, atado, unidad, caja) | Alta |

---

## 2. Perfiles de Agricultor — `api/farmer-profiles`

| ID | Servicio | Caso | Resultado esperado | Prioridad |
|----|----------|------|--------------------|-----------|
| TC-03 | `POST /api/farmer-profiles` | Crear un perfil correcto (con estado `"Pending"`) | **201** + perfil con número y fecha generados automáticamente | Alta |
| TC-04 | `POST /api/farmer-profiles` | Crear perfil con un estado no permitido (p.ej. `"Confirmed"`) | **400** (ese valor no está entre los estados válidos) | Media |
| TC-05 | `GET /api/farmer-profiles` | Ver los perfiles creados | **200** + lista de perfiles | Alta |
| TC-06 | `GET /api/farmer-profiles/{id}` | Consultar un perfil que existe | **200** + información del perfil | Alta |
| TC-07 | `GET /api/farmer-profiles/{id}` | Consultar un perfil que no existe (`99999`) | **404** | Alta |
| TC-08 | `GET /api/farmer-profiles/{id}` | Enviar un valor que no es número (`abc`) | **400** | Media |

---

## 3. Productos — `api/products`

| ID | Servicio | Caso | Resultado esperado | Prioridad |
|----|----------|------|--------------------|-----------|
| TC-09 | `POST /api/products` | Crear producto con datos válidos (agricultor, categoría y unidad de medida existentes) | **201** + producto marcado como activo y con fecha generada automáticamente | Alta |
| TC-10 | `POST /api/products` | Agricultor que no existe | **400** "El agricultor no existe" | Alta |
| TC-11 | `POST /api/products` | Categoría que no existe | **400** "La categoría no existe" | Alta |
| TC-12 | `POST /api/products` | Unidad de medida que no existe | **400** "La unidad de medida no existe" | Alta |
| TC-13 | `POST /api/products` | Precio negativo | **400** "El precio no puede ser negativo" | Media |
| TC-14 | `POST /api/products` | Cantidad de existencias negativa | **400** "El stock no puede ser negativo" | Media |
| TC-15 | `GET /api/products` | Ver los productos activos | **200** + solo productos activos | Alta |
| TC-16 | `GET /api/products?categoryId=1` | Filtrar por categoría | **200** + solo productos de esa categoría | Media |
| TC-17 | `GET /api/products?isOrganic=true` | Filtrar solo productos orgánicos | **200** + solo productos orgánicos | Media |
| TC-18 | `GET /api/products?farmerProfileId={id}` | Filtrar por agricultor | **200** + solo productos de ese agricultor | Media |
| TC-19 | `GET /api/products/{id}` | Consultar un producto que existe | **200** + información del producto | Alta |
| TC-20 | `GET /api/products/{id}` | Consultar un producto que no existe | **404** | Alta |

---

## 4. Reseñas — `api/products/{id}/reviews`

| ID | Servicio | Caso | Resultado esperado | Prioridad |
|----|----------|------|--------------------|-----------|
| TC-21 | `POST /api/products/{id}/reviews` | Crear reseña con calificación 5 en un producto existente | **201** + reseña guardada con fecha automática | Alta |
| TC-22 | `POST /api/products/{id}/reviews` | Calificación 0 | **400** "El rating debe estar entre 1 y 5" | Alta |
| TC-23 | `POST /api/products/{id}/reviews` | Calificación 6 | **400** "El rating debe estar entre 1 y 5" | Alta |
| TC-24 | `POST /api/products/{id}/reviews` | Producto que no existe | **400** "El producto no existe" | Media |
| TC-25 | `GET /api/products/{id}/reviews` | Producto sin reseñas | **200** + lista vacía y promedio 0 | Alta |
| TC-26 | `GET /api/products/{id}/reviews` | Producto con reseñas (crear 2 o más con calificaciones distintas) | **200** + promedio correcto (p.ej. 5 y 3 → promedio 4) | Alta |

---

## 5. Órdenes — `api/orders`

| ID | Servicio | Caso | Resultado esperado | Prioridad |
|----|----------|------|--------------------|-----------|
| TC-27 | `POST /api/orders` | Crear orden correcta (1 o más productos, dirección completa, agricultor y producto con existencias) | **201** + número de orden con formato `ORD-…`, estado inicial `Pending`, total = cantidad × precio, y existencias descontadas | Alta |
| TC-28 | `POST /api/orders` | Sin productos (`items` vacío) | **400** "La orden debe contener al menos un artículo" | Alta |
| TC-29 | `POST /api/orders` | Dirección incompleta (falta municipio o departamento) | **400** "La dirección de envío… es obligatoria" | Alta |
| TC-30 | `POST /api/orders` | Agricultor que no existe | **400** "El agricultor no existe" | Alta |
| TC-31 | `POST /api/orders` | Producto que no existe o no está activo | **400** "El producto {id} no existe o está inactivo" | Alta |
| TC-32 | `POST /api/orders` | Cantidad mayor a las existencias | **400** "Stock insuficiente para el producto …" y **no se descuenta inventario** | Alta |
| TC-33 | `POST /api/orders` | Sin tipo de entrega (se omite el campo) | **201** + se asigna entrega a domicilio por defecto | Media |
| TC-34 | `GET /api/orders` | Ver las órdenes creadas | **200** + lista de órdenes | Alta |
| TC-35 | `GET /api/orders/{id}` | Consultar una orden que existe | **200** + orden con sus productos | Alta |
| TC-36 | `GET /api/orders/{id}` | Consultar una orden que no existe | **404** | Alta |
| TC-37 | `PATCH /api/orders/{id}/status` | Cambiar a un estado válido (`"Preparing"`, `"Delivered"`, `"Cancelled"`) | **204** + el cambio queda guardado (verificar consultando la orden) | Alta |
| TC-38 | `PATCH /api/orders/{id}/status` | Estado no válido (`"Shipped"`) | **400** (ese valor no está entre los estados válidos) | Media |
| TC-39 | `PATCH /api/orders/{id}/status` | Orden que no existe | **404** | Alta |
| TC-40 | `PATCH /api/orders/{id}/delivery` | Actualizar fecha estimada, fecha real y notas | **204** + cambios guardados (verificar consultando la orden) | Media |
| TC-41 | `PATCH /api/orders/{id}/delivery` | Orden que no existe | **404** | Media |

---

## 6. IA / Groq — `api/ai/recommendations`

> Para este servicio se necesita una clave de Groq válida y conexión a internet.

| ID | Servicio | Caso | Resultado esperado | Prioridad |
|----|----------|------|--------------------|-----------|
| TC-42 | `POST /api/ai/recommendations` | Enviar una pregunta válida con productos activos | **200** + respuesta con la recomendación de la IA, y se guardan 3 mensajes de la conversación (sistema, usuario y asistente) | Alta |
| TC-43 | `POST /api/ai/recommendations` | Pregunta vacía o sin texto | **400** "Debe enviar un prompt." | Alta |
| TC-44 | `POST /api/ai/recommendations` | Enviar varias peticiones seguidas superando el límite de Groq | **500** con mensaje interno **429 (Too Many Requests)** (error de la plataforma, no de la API) | Baja |

---

## Resumen y recomendaciones

- **Total de casos:** 44 (cubren los casos normales, las validaciones y los errores).
- **Cobertura:** los 16 servicios públicos.
- **Consumo de tokens:** el servicio de IA solo envía los **20 productos mejor calificados** a Groq para mantener bajo el consumo de tokens.
