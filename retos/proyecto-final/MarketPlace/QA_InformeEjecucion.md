# Informe de Ejecución de Pruebas — AgroMarket Local API

**Proyecto:** MarketPlace (AgroMarket Local API)
**Base URL:** `http://localhost:5008`
**Fecha de ejecución:** 2026-08-03
**Realizado por:** QA - Lizeth Bedoya

## Resumen

| Concepto | Valor |
|----------|-------|
| Casos de prueba | 44 |
| Correctos | 43 |
| Documentado | 1 |
| Capturas tomadas | 44 |

Se ejecutaron peticiones reales contra la API y se capturaron los resultados desde Swagger. Cada caso cuenta con su evidencia visual correspondiente en la carpeta `QA_Capturas/`.

---

## 1. Catálogo — Categorías y Unidades de Medida

### TC-01 — Listar categorías
- **Servicio:** `GET /api/categories`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-01](QA_Capturas/TC-01.png)

### TC-02 — Listar unidades de medida
- **Servicio:** `GET /api/units-of-measure`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-02](QA_Capturas/TC-02.png)

---

## 2. Perfiles de Agricultor

### TC-03 — Crear un perfil correcto
- **Servicio:** `POST /api/farmer-profiles`
- **Esperado:** 201 — **Obtenido:** 201 — **Resultado:** Correcto

![TC-03](QA_Capturas/TC-03.png)

### TC-04 — Crear un perfil con estado no permitido
- **Servicio:** `POST /api/farmer-profiles`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-04](QA_Capturas/TC-04.png)

### TC-05 — Ver los perfiles creados
- **Servicio:** `GET /api/farmer-profiles`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-05](QA_Capturas/TC-05.png)

### TC-06 — Consultar un perfil que existe
- **Servicio:** `GET /api/farmer-profiles/{id}`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-06](QA_Capturas/TC-06.png)

### TC-07 — Consultar un perfil que no existe
- **Servicio:** `GET /api/farmer-profiles/{id}`
- **Esperado:** 404 — **Obtenido:** 404 — **Resultado:** Correcto

![TC-07](QA_Capturas/TC-07.png)

### TC-08 — Consultar perfil con ID no numérico
- **Servicio:** `GET /api/farmer-profiles/{id}`
- **Esperado:** 400 — **Obtenido:** validación de valor no numérico — **Resultado:** Correcto

![TC-08](QA_Capturas/TC-08.png)

---

## 3. Productos

### TC-09 — Crear un producto correcto
- **Servicio:** `POST /api/products`
- **Esperado:** 201 — **Obtenido:** 201 — **Resultado:** Correcto

![TC-09](QA_Capturas/TC-09.png)

### TC-10 — Crear producto con agricultor inexistente
- **Servicio:** `POST /api/products`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-10](QA_Capturas/TC-10.png)

### TC-11 — Crear producto con categoría inexistente
- **Servicio:** `POST /api/products`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-11](QA_Capturas/TC-11.png)

### TC-12 — Crear producto con unidad de medida inexistente
- **Servicio:** `POST /api/products`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-12](QA_Capturas/TC-12.png)

### TC-13 — Crear producto con precio negativo
- **Servicio:** `POST /api/products`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-13](QA_Capturas/TC-13.png)

### TC-14 — Crear producto con existencias negativas
- **Servicio:** `POST /api/products`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-14](QA_Capturas/TC-14.png)

### TC-15 — Ver los productos activos
- **Servicio:** `GET /api/products`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-15](QA_Capturas/TC-15.png)

### TC-16 — Filtrar productos por categoría
- **Servicio:** `GET /api/products?categoryId=1`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-16](QA_Capturas/TC-16.png)

### TC-17 — Filtrar productos orgánicos
- **Servicio:** `GET /api/products?isOrganic=1`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-17](QA_Capturas/TC-17.png)

### TC-18 — Filtrar productos por agricultor
- **Servicio:** `GET /api/products?farmerProfileId={id}`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-18](QA_Capturas/TC-18.png)

### TC-19 — Consultar un producto que existe
- **Servicio:** `GET /api/products/{id}`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-19](QA_Capturas/TC-19.png)

### TC-20 — Consultar un producto que no existe
- **Servicio:** `GET /api/products/{id}`
- **Esperado:** 404 — **Obtenido:** 404 — **Resultado:** Correcto

![TC-20](QA_Capturas/TC-20.png)

---

## 4. Reseñas

### TC-21 — Crear reseña con calificación 5
- **Servicio:** `POST /api/products/{id}/reviews`
- **Esperado:** 201 — **Obtenido:** 201 — **Resultado:** Correcto

![TC-21](QA_Capturas/TC-21.png)

### TC-22 — Crear reseña con calificación 0
- **Servicio:** `POST /api/products/{id}/reviews`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-22](QA_Capturas/TC-22.png)

### TC-23 — Crear reseña con calificación 6
- **Servicio:** `POST /api/products/{id}/reviews`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-23](QA_Capturas/TC-23.png)

### TC-24 — Crear reseña para producto inexistente
- **Servicio:** `POST /api/products/{id}/reviews`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-24](QA_Capturas/TC-24.png)

### TC-25 — Ver reseñas de producto sin reseñas
- **Servicio:** `GET /api/products/{id}/reviews`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-25](QA_Capturas/TC-25.png)

### TC-26 — Ver reseñas y promedio de un producto
- **Servicio:** `GET /api/products/{id}/reviews`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-26](QA_Capturas/TC-26.png)

---

## 5. Órdenes

### TC-27 — Crear una orden correcta
- **Servicio:** `POST /api/orders`
- **Esperado:** 201 — **Obtenido:** 201 — **Resultado:** Correcto

![TC-27](QA_Capturas/TC-27.png)

### TC-28 — Crear orden sin productos
- **Servicio:** `POST /api/orders`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-28](QA_Capturas/TC-28.png)

### TC-29 — Crear orden con dirección incompleta
- **Servicio:** `POST /api/orders`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-29](QA_Capturas/TC-29.png)

### TC-30 — Crear orden con agricultor inexistente
- **Servicio:** `POST /api/orders`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-30](QA_Capturas/TC-30.png)

### TC-31 — Crear orden con producto inexistente
- **Servicio:** `POST /api/orders`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-31](QA_Capturas/TC-31.png)

### TC-32 — Crear orden con cantidad mayor a las existencias
- **Servicio:** `POST /api/orders`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-32](QA_Capturas/TC-32.png)

### TC-33 — Crear orden sin tipo de entrega
- **Servicio:** `POST /api/orders`
- **Esperado:** 201 — **Obtenido:** 201 — **Resultado:** Correcto

![TC-33](QA_Capturas/TC-33.png)

### TC-34 — Listar órdenes
- **Servicio:** `GET /api/orders`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-34](QA_Capturas/TC-34.png)

### TC-35 — Consultar una orden con sus productos
- **Servicio:** `GET /api/orders/{id}`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

![TC-35](QA_Capturas/TC-35.png)

### TC-36 — Consultar una orden inexistente
- **Servicio:** `GET /api/orders/{id}`
- **Esperado:** 404 — **Obtenido:** 404 — **Resultado:** Correcto

![TC-36](QA_Capturas/TC-36.png)

### TC-37 — Cambiar el estado de una orden
- **Servicio:** `PATCH /api/orders/{id}/status`
- **Esperado:** 204 — **Obtenido:** 204 — **Resultado:** Correcto

![TC-37](QA_Capturas/TC-37.png)

### TC-38 — Cambiar a un estado no permitido
- **Servicio:** `PATCH /api/orders/{id}/status`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-38](QA_Capturas/TC-38.png)

### TC-39 — Cambiar estado de una orden inexistente
- **Servicio:** `PATCH /api/orders/{id}/status`
- **Esperado:** 404 — **Obtenido:** 404 — **Resultado:** Correcto

![TC-39](QA_Capturas/TC-39.png)

### TC-40 — Actualizar la información de entrega
- **Servicio:** `PATCH /api/orders/{id}/delivery`
- **Esperado:** 204 — **Obtenido:** 204 — **Resultado:** Correcto

![TC-40](QA_Capturas/TC-40.png)

### TC-41 — Actualizar entrega de una orden inexistente
- **Servicio:** `PATCH /api/orders/{id}/delivery`
- **Esperado:** 404 — **Obtenido:** 404 — **Resultado:** Correcto

![TC-41](QA_Capturas/TC-41.png)

---

## 6. IA / Groq

### TC-42 — Pedir recomendaciones con una pregunta válida
- **Servicio:** `POST /api/ai/recommendations`
- **Esperado:** 200 — **Obtenido:** 200 — **Resultado:** Correcto

> Para mantener bajo el consumo de tokens, la IA solo recibe los **20 productos mejor calificados** del catálogo.

![TC-42](QA_Capturas/TC-42.png)

### TC-43 — Enviar una pregunta vacía
- **Servicio:** `POST /api/ai/recommendations`
- **Esperado:** 400 — **Obtenido:** 400 — **Resultado:** Correcto

![TC-43](QA_Capturas/TC-43.png)

### TC-44 — Pedir recomendaciones superando el límite de peticiones
- **Servicio:** `POST /api/ai/recommendations`
- **Esperado:** error de la plataforma — **Obtenido:** 500 (el error interno es **429 Too Many Requests** de Groq) — **Resultado:** Documentado

![TC-44](QA_Capturas/TC-44.png)

---

## Resultado general de los 44 casos

| ID | Servicio | Esperado | Obtenido | Resultado |
|----|----------|----------|----------|-----------|
| TC-01 | GET /api/categories | 200 | 200 | Correcto |
| TC-02 | GET /api/units-of-measure | 200 | 200 | Correcto |
| TC-03 | POST /api/farmer-profiles | 201 | 201 | Correcto |
| TC-04 | POST /api/farmer-profiles | 400 | 400 | Correcto |
| TC-05 | GET /api/farmer-profiles | 200 | 200 | Correcto |
| TC-06 | GET /api/farmer-profiles/{id} | 200 | 200 | Correcto |
| TC-07 | GET /api/farmer-profiles/{id} | 404 | 404 | Correcto |
| TC-08 | GET /api/farmer-profiles/{id} | 400 | Validación no numérica | Correcto |
| TC-09 | POST /api/products | 201 | 201 | Correcto |
| TC-10 | POST /api/products | 400 | 400 | Correcto |
| TC-11 | POST /api/products | 400 | 400 | Correcto |
| TC-12 | POST /api/products | 400 | 400 | Correcto |
| TC-13 | POST /api/products | 400 | 400 | Correcto |
| TC-14 | POST /api/products | 400 | 400 | Correcto |
| TC-15 | GET /api/products | 200 | 200 | Correcto |
| TC-16 | GET /api/products?categoryId=1 | 200 | 200 | Correcto |
| TC-17 | GET /api/products?isOrganic=1 | 200 | 200 | Correcto |
| TC-18 | GET /api/products?farmerProfileId={id} | 200 | 200 | Correcto |
| TC-19 | GET /api/products/{id} | 200 | 200 | Correcto |
| TC-20 | GET /api/products/{id} | 404 | 404 | Correcto |
| TC-21 | POST /api/products/{id}/reviews | 201 | 201 | Correcto |
| TC-22 | POST /api/products/{id}/reviews | 400 | 400 | Correcto |
| TC-23 | POST /api/products/{id}/reviews | 400 | 400 | Correcto |
| TC-24 | POST /api/products/{id}/reviews | 400 | 400 | Correcto |
| TC-25 | GET /api/products/{id}/reviews | 200 | 200 | Correcto |
| TC-26 | GET /api/products/{id}/reviews | 200 | 200 | Correcto |
| TC-27 | POST /api/orders | 201 | 201 | Correcto |
| TC-28 | POST /api/orders | 400 | 400 | Correcto |
| TC-29 | POST /api/orders | 400 | 400 | Correcto |
| TC-30 | POST /api/orders | 400 | 400 | Correcto |
| TC-31 | POST /api/orders | 400 | 400 | Correcto |
| TC-32 | POST /api/orders | 400 | 400 | Correcto |
| TC-33 | POST /api/orders | 201 | 201 | Correcto |
| TC-34 | GET /api/orders | 200 | 200 | Correcto |
| TC-35 | GET /api/orders/{id} | 200 | 200 | Correcto |
| TC-36 | GET /api/orders/{id} | 404 | 404 | Correcto |
| TC-37 | PATCH /api/orders/{id}/status | 204 | 204 | Correcto |
| TC-38 | PATCH /api/orders/{id}/status | 400 | 400 | Correcto |
| TC-39 | PATCH /api/orders/{id}/status | 404 | 404 | Correcto |
| TC-40 | PATCH /api/orders/{id}/delivery | 204 | 204 | Correcto |
| TC-41 | PATCH /api/orders/{id}/delivery | 404 | 404 | Correcto |
| TC-42 | POST /api/ai/recommendations | 200 | 200 | Correcto |
| TC-43 | POST /api/ai/recommendations | 400 | 400 | Correcto |
| TC-44 | POST /api/ai/recommendations | error de la plataforma | 500 (429 de Groq) | Documentado |
