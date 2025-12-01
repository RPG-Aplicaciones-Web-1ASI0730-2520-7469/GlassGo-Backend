# 🔌 Guía Completa de API Backend - GlassGo

## 📋 Índice
1. [IAM (Identity & Access Management)](#iam-identity--access-management)
2. [Profiles (Gestión de Perfiles)](#profiles-gestión-de-perfiles)
3. [Estructura de Datos](#estructura-de-datos)
4. [Códigos de Estado HTTP](#códigos-de-estado-http)
5. [Mapeo Frontend-Backend](#mapeo-frontend-backend)

---

## 🔐 IAM (Identity & Access Management)

### 📍 Ubicación en Frontend
```
src/iam/
├── application/
│   └── auth.store.js          # Pinia store para autenticación
├── domain/
│   └── model/
│       ├── login-credentials.entity.js
│       └── user.entity.js
├── infrastructure/
│   ├── auth-api.js           # Llamadas HTTP al backend
│   ├── auth.guard.js         # Protección de rutas
│   └── user.assembler.js     # Transformación de datos
└── presentation/
    ├── components/
    │   └── auth-layout.vue
    └── views/
        ├── login.vue
        ├── register.vue
        └── forgot-password.vue
```

### Base URL
```
http://localhost:3000/api
```

---

### 1. **Obtener Lista de Usuarios (Para Login)**
**Endpoint:** `GET /users`

**Descripción:** Obtiene todos los usuarios registrados. El frontend usa esto para validar credenciales de login.

**Usado en:** `src/iam/infrastructure/auth-api.js` → método `getUsers()`

**Request:**
```http
GET /api/users
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "username": "distributor.main",
    "email": "distributor@glassgo.com",
    "password": "dist123",
    "role": "distributor",
    "firstName": "Main",
    "lastName": "Distributor",
    "company": "Distribuidora Central SAC",
    "taxId": "20123456789",
    "address": "Av. Brasil 1234, Lima",
    "phone": "+51 987654321",
    "preferredCurrency": "PEN",
    "notifications": {
      "email": true,
      "sms": false,
      "push": true
    },
    "paymentMethods": [
      {
        "type": "bank_transfer",
        "bank": "BCP",
        "account": "123-456-789"
      }
    ],
    "isActive": true
  },
  {
    "id": 2,
    "username": "owner.main",
    "email": "owner.main@glassgo.com",
    "password": "owner123",
    "role": "business-owner",
    "firstName": "Main",
    "lastName": "Owner",
    "isActive": true,
    "createdAt": "2025-01-15T00:00:00Z",
    "businessName": "Restobar El Buen Sabor SAC",
    "taxId": "20456789231",
    "address": "Av. Primavera 456, Lima",
    "phone": "+51 999888777",
    "preferredCurrency": "PEN",
    "notifications": {
      "email": true,
      "sms": true,
      "push": false
    },
    "loyaltyPoints": 72
  },
  {
    "id": 3,
    "username": "carrier.main",
    "email": "carrier@glassgo.com",
    "password": "car123",
    "role": "carrier",
    "firstName": "Main",
    "lastName": "Driver",
    "isActive": true,
    "createdAt": "2025-01-10T00:00:00Z",
    "phone": "+51 999888777"
  },
  {
    "id": 4,
    "username": "admin.main",
    "email": "admin@glassgo.com",
    "password": "admin123",
    "role": "admin",
    "firstName": "Main",
    "lastName": "Admin",
    "isActive": true,
    "phone": "+51 999888777"
  }
]
```

---

### 2. **Login de Usuario**
**Endpoint:** `POST /auth/login` (No implementado actualmente en JSON Server)

**Descripción:** Autentica un usuario con email/username y contraseña

**Usado en:** `src/iam/application/auth.store.js` → método `login(credentials)`

**Cómo funciona actualmente:**
- El frontend hace `GET /users`
- Busca un usuario que coincida con email/username Y password
- Si encuentra match, guarda el usuario en localStorage

**Request:**
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "distributor@glassgo.com",
  "password": "dist123"
}
```

**Response Exitoso (200 OK):**
```json
{
  "success": true,
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "username": "distributor.main",
    "email": "distributor@glassgo.com",
    "role": "distributor",
    "firstName": "Main",
    "lastName": "Distributor",
    "company": "Distribuidora Central SAC",
    "phone": "+51 987654321",
    "isActive": true
  }
}
```

**Response Error (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Invalid credentials"
}
```

**⚠️ IMPORTANTE:**
- NUNCA retornar el campo `password` en la respuesta
- El token debe ser un JWT válido
- El token debe guardarse en localStorage del frontend
- El token debe incluirse en futuras peticiones autenticadas

---

### 3. **Validar Token**
**Endpoint:** `POST /auth/validate`

**Descripción:** Valida si un token de sesión es válido

**Usado en:** `src/iam/infrastructure/auth-api.js` → método `validateToken(token)`

**Request:**
```http
POST /api/auth/validate
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response Válido (200 OK):**
```json
{
  "valid": true,
  "userId": 1,
  "expiresAt": "2025-12-02T10:30:00Z"
}
```

**Response Inválido (401 Unauthorized):**
```json
{
  "valid": false,
  "message": "Invalid or expired token"
}
```

---

### 4. **Logout**
**Endpoint:** `POST /auth/logout`

**Descripción:** Cierra la sesión del usuario (invalida el token)

**Usado en:** `src/iam/application/auth.store.js` → método `logout()`

**Request:**
```http
POST /api/auth/logout
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Logout successful"
}
```

**Nota:** El frontend también borra el token de localStorage

---

### 5. **Recuperar Contraseña**
**Endpoint:** `POST /auth/forgot-password`

**Descripción:** Envía email de recuperación de contraseña

**Usado en:** `src/iam/presentation/views/forgot-password.vue`

**Request:**
```http
POST /api/auth/forgot-password
Content-Type: application/json

{
  "email": "distributor@glassgo.com"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Password reset email sent (demo mode)"
}
```

**Response Error (404 Not Found):**
```json
{
  "success": false,
  "message": "Email not found"
}
```

---

### 6. **Registrar Nuevo Usuario**
**Endpoint:** `POST /auth/register`

**Descripción:** Crea una nueva cuenta de usuario

**Usado en:** `src/iam/presentation/views/register.vue`

**Request:**
```http
POST /api/auth/register
Content-Type: application/json

{
  "username": "newuser",
  "email": "newuser@example.com",
  "password": "securePass123",
  "firstName": "John",
  "lastName": "Doe",
  "role": "business-owner",
  "company": "Doe's Business",
  "phone": "+51 999777666",
  "address": "Av. Example 123, Lima",
  "taxId": "20999888777",
  "preferredCurrency": "PEN"
}
```

**Response Exitoso (201 Created):**
```json
{
  "success": true,
  "user": {
    "id": 5,
    "username": "newuser",
    "email": "newuser@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "role": "business-owner",
    "company": "Doe's Business",
    "phone": "+51 999777666",
    "isActive": true,
    "createdAt": "2025-12-01T10:30:00Z"
  },
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response Error (409 Conflict):**
```json
{
  "success": false,
  "message": "Email already registered"
}
```

---

## 👤 Profiles (Gestión de Perfiles)

### 📍 Ubicación en Frontend
```
src/profiles/
├── application/
│   └── profile.store.js       # Pinia store para perfil
├── domain/
│   └── model/
│       ├── user-profile.entity.js
│       └── history-item.entity.js
├── infrastructure/
│   ├── profile-api.js         # Llamadas HTTP al backend
│   └── profile.assembler.js   # Transformación de datos
└── presentation/
    ├── components/
    │   ├── profile-layout.vue
    │   └── simple-profile-layout.vue
    └── views/
        └── profile.vue
```

---

### 1. **Obtener Perfil de Usuario**
**Endpoint:** `GET /users/:userId`

**Descripción:** Obtiene la información completa del perfil de un usuario

**Usado en:** `src/profiles/infrastructure/profile-api.js` → método `getProfile(userId)`

**Request:**
```http
GET /api/users/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response Distributor (200 OK):**
```json
{
  "id": 1,
  "username": "distributor.main",
  "email": "distributor@glassgo.com",
  "role": "distributor",
  "firstName": "Main",
  "lastName": "Distributor",
  "company": "Distribuidora Central SAC",
  "taxId": "20123456789",
  "address": "Av. Brasil 1234, Lima",
  "phone": "+51 987654321",
  "preferredCurrency": "PEN",
  "notifications": {
    "email": true,
    "sms": false,
    "push": true
  },
  "paymentMethods": [
    {
      "type": "bank_transfer",
      "bank": "BCP",
      "account": "123-456-789"
    }
  ],
  "isActive": true
}
```

**Response Business Owner (200 OK):**
```json
{
  "id": 2,
  "username": "owner.main",
  "email": "owner.main@glassgo.com",
  "role": "business-owner",
  "firstName": "Main",
  "lastName": "Owner",
  "isActive": true,
  "createdAt": "2025-01-15T00:00:00Z",
  "businessName": "Restobar El Buen Sabor SAC",
  "taxId": "20456789231",
  "address": "Av. Primavera 456, Lima",
  "phone": "+51 999888777",
  "preferredCurrency": "PEN",
  "notifications": {
    "email": true,
    "sms": true,
    "push": false
  },
  "loyaltyPoints": 72
}
```

**Response Carrier (200 OK):**
```json
{
  "id": 3,
  "username": "carrier.main",
  "email": "carrier@glassgo.com",
  "role": "carrier",
  "firstName": "Main",
  "lastName": "Driver",
  "isActive": true,
  "createdAt": "2025-01-10T00:00:00Z",
  "phone": "+51 999888777"
}
```

**Response Error (404 Not Found):**
```json
{
  "success": false,
  "message": "User not found"
}
```

---

### 2. **Actualizar Perfil de Usuario**
**Endpoint:** `PUT /users/:userId` o `PATCH /users/:userId`

**Descripción:** Actualiza la información del perfil del usuario

**Usado en:** `src/profiles/infrastructure/profile-api.js` → método `updateProfile(profileData)`

**Request (Distributor):**
```http
PUT /api/users/1
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

{
  "firstName": "Main",
  "lastName": "Distributor Updated",
  "company": "Distribuidora Central SAC",
  "phone": "+51 987654322",
  "address": "Av. Brasil 1235, Lima",
  "preferredCurrency": "USD",
  "notifications": {
    "email": true,
    "sms": true,
    "push": true
  }
}
```

**Request (Business Owner):**
```http
PUT /api/users/2
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

{
  "firstName": "Main",
  "lastName": "Owner",
  "businessName": "Restobar El Mejor Sabor SAC",
  "phone": "+51 999888778",
  "address": "Av. Primavera 457, Lima",
  "notifications": {
    "email": true,
    "sms": true,
    "push": true
  }
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Profile updated successfully",
  "user": {
    "id": 1,
    "username": "distributor.main",
    "email": "distributor@glassgo.com",
    "firstName": "Main",
    "lastName": "Distributor Updated",
    "role": "distributor",
    "company": "Distribuidora Central SAC",
    "phone": "+51 987654322",
    "address": "Av. Brasil 1235, Lima",
    "preferredCurrency": "USD",
    "notifications": {
      "email": true,
      "sms": true,
      "push": true
    },
    "isActive": true
  }
}
```

**Response Error (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid data provided",
  "errors": {
    "phone": "Invalid phone format",
    "email": "Email cannot be changed"
  }
}
```

---

### 3. **Obtener Historial de Usuario**
**Endpoint:** `GET /history?userId=:userId`

**Descripción:** Obtiene el historial de actividades del usuario (usado en perfil)

**Usado en:** `src/profiles/infrastructure/profile-api.js` → método `getHistory(userId)`

**Request:**
```http
GET /api/history?userId=1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "userId": 1,
    "action": "Order Created",
    "description": "Created order #GG2025-01",
    "timestamp": "2025-11-30T14:20:00Z",
    "type": "order"
  },
  {
    "id": 2,
    "userId": 1,
    "action": "Profile Updated",
    "description": "Updated profile information",
    "timestamp": "2025-12-01T08:15:00Z",
    "type": "profile"
  },
  {
    "id": 3,
    "userId": 1,
    "action": "Payment Received",
    "description": "Payment processed for order #GG2025-02",
    "timestamp": "2025-11-29T16:45:00Z",
    "type": "payment"
  }
]
```

**Nota:** Actualmente el frontend filtra los resultados por `userId` en el cliente. El backend debería filtrar directamente.

---

### 4. **Obtener Configuraciones de Usuario**
**Endpoint:** `GET /users/:userId/settings`

**Descripción:** Obtiene las preferencias y configuraciones del usuario

**Usado en:** `src/profiles/infrastructure/profile-api.js` → método `getUserSettings(userId)`

**Request:**
```http
GET /api/users/1/settings
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response (200 OK):**
```json
{
  "userId": 1,
  "notifications": {
    "email": true,
    "sms": false,
    "push": true
  },
  "twoFactorAuth": {
    "enabled": false,
    "method": null
  },
  "language": "es",
  "timezone": "America/Lima",
  "theme": "light",
  "preferredCurrency": "PEN"
}
```

---

### 5. **Actualizar Configuraciones de Usuario**
**Endpoint:** `PUT /users/:userId/settings` or `PATCH /users/:userId/settings`

**Descripción:** Actualiza las preferencias del usuario

**Usado en:** `src/profiles/infrastructure/profile-api.js` → método `updateUserSettings(userId, settings)`

**Request:**
```http
PUT /api/users/1/settings
Content-Type: application/json
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

{
  "notifications": {
    "email": true,
    "sms": true,
    "push": true
  },
  "twoFactorAuth": {
    "enabled": true,
    "method": "email"
  },
  "language": "en",
  "theme": "dark",
  "preferredCurrency": "USD"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Settings updated successfully",
  "settings": {
    "userId": 1,
    "notifications": {
      "email": true,
      "sms": true,
      "push": true
    },
    "twoFactorAuth": {
      "enabled": true,
      "method": "email"
    },
    "language": "en",
    "timezone": "America/Lima",
    "theme": "dark",
    "preferredCurrency": "USD"
  }
}
```

---

### 6. **Actualizar Avatar/Foto de Perfil**
**Endpoint:** `POST /users/:userId/avatar`

**Descripción:** Sube una nueva foto de perfil

**Usado en:** `src/profiles/presentation/views/profile.vue` (botón "Change Photo")

**Request:**
```http
POST /api/users/1/avatar
Content-Type: multipart/form-data
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

Form Data:
- avatar: [archivo de imagen JPG/PNG]
```

**Response (200 OK):**
```json
{
  "success": true,
  "avatarUrl": "https://storage.glassgo.com/avatars/user-1-avatar.jpg",
  "message": "Avatar updated successfully"
}
```

**Response Error (400 Bad Request):**
```json
{
  "success": false,
  "message": "Invalid file format. Only JPG and PNG allowed"
}
```

---

### 7. **Obtener Estadísticas del Dashboard**
**Endpoint:** `GET /users/:userId/stats`

**Descripción:** Obtiene métricas y estadísticas del usuario para mostrar en el perfil/dashboard

**Usado en:** `src/profiles/infrastructure/profile-api.js` → método `getDashboardStats(userId)`

**Request:**
```http
GET /api/users/1/stats
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Response Distributor (200 OK):**
```json
{
  "userId": 1,
  "role": "distributor",
  "totalOrders": 156,
  "activeOrders": 12,
  "deliveredOrders": 140,
  "cancelledOrders": 4,
  "stockProducts": 133,
  "totalRevenue": 45670.50,
  "monthlyRevenue": 5430.00,
  "period": "last_30_days"
}
```

**Response Business Owner (200 OK):**
```json
{
  "userId": 2,
  "role": "business-owner",
  "monthlyConsumption": 1250,
  "activeSubscriptions": 2,
  "pendingOrders": 3,
  "totalSpent": 8450.00,
  "loyaltyPoints": 72,
  "period": "last_30_days"
}
```

**Response Carrier (200 OK):**
```json
{
  "userId": 3,
  "role": "carrier",
  "deliveriesToday": 8,
  "completedDeliveries": 5,
  "pendingDeliveries": 2,
  "failedDeliveries": 1,
  "activeRoutes": 3,
  "period": "today"
}
```

---

## 📊 Estructura de Datos

### 🔹 User Entity (Completo según db.json actual)

**Campos Comunes para todos los roles:**
```typescript
{
  id: number;                    // ID único del usuario
  username: string;              // Nombre de usuario único
  email: string;                 // Email único
  password: string;              // ⚠️ NUNCA enviar en respuestas API
  role: 'distributor' | 'business-owner' | 'carrier' | 'admin';
  firstName: string;             // Nombre
  lastName: string;              // Apellido
  phone: string;                 // Teléfono (formato: +51 999888777)
  isActive: boolean;             // Usuario activo o desactivado
  createdAt?: string;            // Fecha de creación (ISO 8601)
}
```

**Campos específicos por rol:**

**Distributor:**
```typescript
{
  ...camposComunes,
  company: string;               // Nombre de la empresa
  taxId: string;                 // RUC (Perú: 20123456789)
  address: string;               // Dirección física
  preferredCurrency: string;     // "PEN" | "USD"
  notifications: {
    email: boolean;
    sms: boolean;
    push: boolean;
  },
  paymentMethods: Array<{
    type: string;                // "bank_transfer" | "card" | "cash"
    bank?: string;               // Nombre del banco
    account?: string;            // Número de cuenta
  }>
}
```

**Business Owner:**
```typescript
{
  ...camposComunes,
  businessName: string;          // Nombre del negocio
  taxId: string;                 // RUC
  address: string;               // Dirección
  preferredCurrency: string;     // "PEN" | "USD"
  notifications: {
    email: boolean;
    sms: boolean;
    push: boolean;
  },
  loyaltyPoints: number;         // Puntos de fidelización
}
```

**Carrier:**
```typescript
{
  ...camposComunes
  // Solo campos comunes
}
```

**Admin:**
```typescript
{
  ...camposComunes
  // Solo campos comunes
}
```

---

### 🔹 Login Credentials Entity

```typescript
{
  email: string;                 // O username
  password: string;
}
```

**Usado en:** `src/iam/domain/model/login-credentials.entity.js`

---

### 🔹 Auth Response

```typescript
{
  success: boolean;
  user?: {
    id: number;
    username: string;
    email: string;
    firstName: string;
    lastName: string;
    role: string;
    company?: string;
    phone: string;
    isActive: boolean;
    // ... otros campos según rol (SIN password)
  };
  token?: string;                // JWT token
  message?: string;              // Mensaje de error si success = false
}
```

---

### 🔹 History Item Entity

```typescript
{
  id: number;
  userId: number;                // ID del usuario dueño del historial
  action: string;                // Título de la acción
  description: string;           // Descripción detallada
  timestamp: string;             // ISO 8601 (ej: "2025-11-30T14:20:00Z")
  type: 'order' | 'profile' | 'payment' | 'system';
}
```

**Usado en:** `src/profiles/domain/model/history-item.entity.js`

**Ejemplo en db.json:**
```json
{
  "id": 1,
  "userId": 1,
  "action": "Order Created",
  "description": "Created order #GG2025-01",
  "timestamp": "2025-11-30T14:20:00Z",
  "type": "order"
}
```

---

### 🔹 User Settings

```typescript
{
  userId: number;
  notifications: {
    email: boolean;
    sms: boolean;
    push: boolean;
  };
  twoFactorAuth: {
    enabled: boolean;
    method: 'email' | 'sms' | 'app' | null;
  };
  language: 'en' | 'es';
  timezone: string;              // ej: "America/Lima"
  theme: 'light' | 'dark';
  preferredCurrency: 'PEN' | 'USD';
}
```

---

### 🔹 Order Entity (Referencia)

```typescript
{
  id: string;                    // ej: "GG2025-01"
  client: string;                // Nombre del cliente
  status: 'active' | 'delivered' | 'cancelled' | 'pending' | 'failed';
  date: string;                  // Formato: "2025-11-01"
  total?: number;                // Monto total
  liters?: number;               // Cantidad de litros
  userId: number;                // ID del usuario dueño
}
```

---

## 🎯 Códigos de Estado HTTP

| Código | Significado | Cuándo usarlo |
|--------|-------------|---------------|
| **200** | OK | Solicitud GET/PUT/PATCH exitosa |
| **201** | Created | Usuario/recurso creado exitosamente (POST /auth/register) |
| **204** | No Content | DELETE exitoso sin contenido en respuesta |
| **400** | Bad Request | Datos inválidos, malformados o falta información requerida |
| **401** | Unauthorized | Credenciales inválidas, token expirado o falta token |
| **403** | Forbidden | Usuario autenticado pero sin permisos para la acción |
| **404** | Not Found | Usuario/recurso no encontrado |
| **409** | Conflict | Email/username duplicado, conflicto de estado |
| **422** | Unprocessable Entity | Validación de negocio fallida (ej: password muy corto) |
| **500** | Internal Server Error | Error inesperado del servidor |

---

## 🔒 Headers Requeridos

### Para todas las peticiones:
```http
Content-Type: application/json
Accept: application/json
```

### Para peticiones autenticadas (después del login):
```http
Authorization: Bearer {token}
Content-Type: application/json
Accept: application/json
```

**Ejemplo:**
```http
GET /api/users/1
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOjEsInJvbGUiOiJkaXN0cmlidXRvciIsImlhdCI6MTcwMTQzMjAwMH0.abc123...
```

---

## 🔄 Mapeo Frontend-Backend

### IAM Module

| Archivo Frontend | Método | Endpoint Backend | Descripción |
|-----------------|--------|------------------|-------------|
| `auth-api.js` | `getUsers()` | `GET /users` | Obtiene todos los usuarios |
| `auth-api.js` | `validateToken(token)` | `POST /auth/validate` | Valida token |
| `auth-api.js` | `logout()` | `POST /auth/logout` | Cierra sesión |
| `auth-api.js` | `forgotPassword(email)` | `POST /auth/forgot-password` | Recuperar contraseña |
| `auth.store.js` | `login(credentials)` | `POST /auth/login` | Login de usuario |
| `register.vue` | (submit) | `POST /auth/register` | Registro de usuario |

### Profiles Module

| Archivo Frontend | Método | Endpoint Backend | Descripción |
|-----------------|--------|------------------|-------------|
| `profile-api.js` | `getProfile(userId)` | `GET /users/:userId` | Obtener perfil |
| `profile-api.js` | `updateProfile(data)` | `PUT /users/:userId` | Actualizar perfil |
| `profile-api.js` | `getHistory(userId)` | `GET /history?userId=:userId` | Obtener historial |
| `profile-api.js` | `getUserSettings(userId)` | `GET /users/:userId/settings` | Obtener configuraciones |
| `profile-api.js` | `updateUserSettings(userId, settings)` | `PUT /users/:userId/settings` | Actualizar configuraciones |
| `profile-api.js` | `getDashboardStats(userId)` | `GET /users/:userId/stats` | Obtener estadísticas |
| `profile.vue` | (upload avatar) | `POST /users/:userId/avatar` | Actualizar foto |

---

## 🧪 Ejemplo Completo de db.json

```json
{
  "users": [
    {
      "id": 1,
      "username": "distributor.main",
      "email": "distributor@glassgo.com",
      "password": "dist123",
      "role": "distributor",
      "firstName": "Main",
      "lastName": "Distributor",
      "company": "Distribuidora Central SAC",
      "taxId": "20123456789",
      "address": "Av. Brasil 1234, Lima",
      "phone": "+51 987654321",
      "preferredCurrency": "PEN",
      "notifications": {
        "email": true,
        "sms": false,
        "push": true
      },
      "paymentMethods": [
        {
          "type": "bank_transfer",
          "bank": "BCP",
          "account": "123-456-789"
        }
      ],
      "isActive": true
    },
    {
      "id": 2,
      "username": "owner.main",
      "email": "owner.main@glassgo.com",
      "password": "owner123",
      "role": "business-owner",
      "firstName": "Main",
      "lastName": "Owner",
      "isActive": true,
      "createdAt": "2025-01-15T00:00:00Z",
      "businessName": "Restobar El Buen Sabor SAC",
      "taxId": "20456789231",
      "address": "Av. Primavera 456, Lima",
      "phone": "+51 999888777",
      "preferredCurrency": "PEN",
      "notifications": {
        "email": true,
        "sms": true,
        "push": false
      },
      "loyaltyPoints": 72
    },
    {
      "id": 3,
      "username": "carrier.main",
      "email": "carrier@glassgo.com",
      "password": "car123",
      "role": "carrier",
      "firstName": "Main",
      "lastName": "Driver",
      "isActive": true,
      "createdAt": "2025-01-10T00:00:00Z",
      "phone": "+51 999888777"
    },
    {
      "id": 4,
      "username": "admin.main",
      "email": "admin@glassgo.com",
      "password": "admin123",
      "role": "admin",
      "firstName": "Main",
      "lastName": "Admin",
      "isActive": true,
      "phone": "+51 999888777"
    }
  ],
  "history": [
    {
      "id": 1,
      "userId": 1,
      "action": "Order Created",
      "description": "Created order #GG2025-01",
      "timestamp": "2025-11-30T14:20:00Z",
      "type": "order"
    },
    {
      "id": 2,
      "userId": 1,
      "action": "Profile Updated",
      "description": "Updated profile information",
      "timestamp": "2025-12-01T08:15:00Z",
      "type": "profile"
    },
    {
      "id": 3,
      "userId": 2,
      "action": "Order Created",
      "description": "Created order #ORD-1002",
      "timestamp": "2025-10-29T10:30:00Z",
      "type": "order"
    }
  ],
  "userSettings": [
    {
      "id": 1,
      "userId": 1,
      "notifications": {
        "email": true,
        "sms": false,
        "push": true
      },
      "twoFactorAuth": {
        "enabled": false,
        "method": null
      },
      "language": "es",
      "timezone": "America/Lima",
      "theme": "light",
      "preferredCurrency": "PEN"
    },
    {
      "id": 2,
      "userId": 2,
      "notifications": {
        "email": true,
        "sms": true,
        "push": false
      },
      "twoFactorAuth": {
        "enabled": false,
        "method": null
      },
      "language": "es",
      "timezone": "America/Lima",
      "theme": "light",
      "preferredCurrency": "PEN"
    }
  ]
}
```

---

## 📝 Notas Importantes para el Desarrollo Backend

### 1. **Seguridad Crítica:**
- ❌ **NUNCA** enviar el campo `password` en las respuestas API
- ✅ Usar **bcrypt** o **argon2** para hashear passwords
- ✅ Implementar **HTTPS** en producción
- ✅ Validar y sanitizar **todos** los inputs
- ✅ Implementar **rate limiting** para login (máx 5 intentos/minuto)
- ✅ Usar **JWT** con expiración (recomendado: 24 horas)

### 2. **Validaciones Requeridas:**
- **Email:** Formato válido, único, máx 255 caracteres
- **Password:** Mínimo 8 caracteres, 1 mayúscula, 1 minúscula, 1 número
- **Phone:** Formato internacional (+51 999888777)
- **Role:** Solo 'admin', 'distributor', 'carrier', 'business-owner'
- **TaxId:** 11 dígitos numéricos (RUC en Perú)

### 3. **Roles y Permisos:**
- **admin**: Ver/editar todos los usuarios, desactivar usuarios
- **distributor**: Ver inventario, crear órdenes
- **carrier**: Ver rutas, actualizar entregas
- **business-owner**: Ver puntos lealtad, hacer pedidos

### 4. **Formato de Fechas:**
- Usar **ISO 8601**: `"2025-12-01T10:30:00Z"`
- Zona horaria: UTC o `America/Lima`

---

## 🚀 Implementación Recomendada (Backend)

### Tecnologías Sugeridas:
- **Node.js** v18+ con **TypeScript**
- **NestJS** (framework modular) o **Express.js**
- **PostgreSQL** o **MongoDB**
- **Prisma** o **TypeORM** (ORM)
- **JWT** (jsonwebtoken)
- **bcrypt** para passwords
- **class-validator** para validación

### Estructura Recomendada:
```
backend/
├── src/
│   ├── modules/
│   │   ├── iam/                    # Identity & Access
│   │   │   ├── controllers/
│   │   │   ├── services/
│   │   │   ├── entities/
│   │   │   └── dto/
│   │   └── profiles/               # Perfiles
│   │       ├── controllers/
│   │       ├── services/
│   │       └── dto/
│   ├── common/                     # Código compartido
│   └── config/
└── .env
```

---

**Creado por:** RPG Startup - GlassGo Team  
**Fecha:** Diciembre 2025  
**Versión:** 2.0.0  
**Frontend:** Vue 3 + Vite  
**Backend Recomendado:** NestJS + PostgreSQL
