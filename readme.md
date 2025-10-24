# BancoWs – API de Ahorros y Clientes

API REST en .NET 8 para gestionar clientes y sus cuentas de ahorro. Implementa autenticación JWT, catálogo de estados de México y persistencia en MongoDB (por defecto). Incluye Swagger en entorno Development.

## Contenidos
- Descripción general
- Arquitectura del proyecto
- Requisitos previos
- Configuración (appsettings)
- Ejecución local
- Autenticación (JWT)
- Endpoints y ejemplos
- Modelos de datos
- Notas y solución de problemas

## Descripción general
La API permite:
- Registrar clientes y generar su CURP a través de un servicio externo.
- Iniciar sesión para obtener un token JWT con el rol Cliente.
- Crear y consultar cuentas de ahorro asociadas a un cliente.
- Consultar el catálogo de entidades federativas (estados).

Persistencia por defecto: MongoDB. El repositorio SQL/Dapper está disponible pero no habilitado en la configuración actual.

## Arquitectura del proyecto
- `Banco.Presentacion.Api` (API ASP.NET Core, net8.0)
- `Banco.ReglasDeNegocio` (servicios de dominio y Unit of Work)
- `Banco.Core` (interfaces y DTOs)
- `Banco.Core.Repositorio` (entidades de persistencia)
- `Banco.Repsositorio.MongoDb` (repositorios MongoDB)
- `Banco.Repositorio.Sql` y `Banco.Repositorio.Dapper` (repositorios alternativos)
- `AutorizacionJwtServicio` (configuración y emisión de tokens JWT)
- `Banco.Servicios` (servicios externos; p.ej. generación de CURP)

Puntos de entrada y controladores principales:
- `Banco.Presentacion.Api/Program.cs`
- `Banco.Presentacion.Api/Controllers/ClientesController.cs`
- `Banco.Presentacion.Api/Controllers/AhorrosController.cs`
- `Banco.Presentacion.Api/Controllers/EstadosController.cs`

## Requisitos previos
- .NET SDK 8.0+
- MongoDB en ejecución y accesible desde la máquina local
- Opcional: SQL Server si se desea activar el repositorio SQL (no habilitado por defecto)

## Configuración
Archivo: `Banco.Presentacion.Api/appsettings.json`

Claves relevantes:
- `ConnectionStrings:MongoDb`: cadena de conexión a MongoDB. Ejemplo:
  `mongodb://root:123456@localhost:27017/Banco?data=null&authSource=admin`
- `LlaveJwt`: clave simétrica usada para firmar el token JWT.

Notas:
- Por defecto, la API usa el repositorio de MongoDB habilitado en `Banco.ReglasDeNegocio/Helpers/ReglasDeNegocioExtensor.cs`.
- Las claves de configuración no son sensibles a mayúsculas/minúsculas.

## Ejecución local
1) Restaurar y compilar
   - `dotnet restore`
   - `dotnet build`
2) Ejecutar la API
   - `dotnet run --project Banco.Presentacion.Api`
3) Swagger (en Development)
   - Navega a `http://localhost:5097/swagger` o al puerto configurado en `launchSettings.json`.

## Autenticación (JWT)
- Inicio de sesión: `POST /api/Clientes/InicioDeSesiones` devuelve un `Token` (Bearer).
- Rutas protegidas requieren encabezado: `Authorization: Bearer {token}`.
- La política `Cliente` valida que el token contenga el claim `Role=Cliente`.

Claims emitidos por el token:
- `Nombre`, `Role`, `ClienteId`, `email`.

Importante: agrega el token a cada solicitud protegida. Si la autorización no funciona, revisa las notas al final.

## Endpoints y ejemplos

1) Estados
- GET `/api/Estados`
  - Descripción: Lista las entidades federativas de México.
  - Respuesta 200 (JSON):
    `[{ "id": 1, "nombre": "Aguascalientes" }, ...]`

2) Clientes
- POST `/api/Clientes`
  - Descripción: Registra un cliente nuevo. Si ya existe (mismo `EncodedKey` o misma CURP estimada) devuelve 208.
  - Body (JSON):
    {
      "primerApellido": "Pérez",
      "segundoApellido": "López",
      "correo": "usuario@correo.com",
      "contrasenia": "MiClave123",
      "nombre": "Juan",
      "telefono": "5512345678",
      "fechaDeNacimiento": "1990-05-20",
      "estadoDeNacimiento": 9,
      "sexo": "H"
    }
  - Respuestas:
    - 201 Created (sin cuerpo)
    - 208 Already Reported: `{ "id": 1, "encodedkey": "...", "mensaje": "Registro previo", "fechaDeRegistro": "..." }`

- POST `/api/Clientes/InicioDeSesiones`
  - Descripción: Genera un token JWT. Usuario = correo del cliente; contraseña debe coincidir exactamente.
  - Body (JSON):
    { "usuario": "usuario@correo.com", "contraseña": "MiClave123" }
  - Respuestas:
    - 200 OK: `{ "token": "...", "fecha": "...", "fechaDeExpiracion": "...", "expiracionEnMinutos": 20 }`
    - 400 Bad Request: `{ "mensaje": "Credenciales no validas" }`

3) Ahorros (requiere JWT)
- POST `/api/Ahorros`
  - Descripción: Crea una cuenta de ahorro para un cliente (si no existe). Por defecto, al registrar un cliente ya se crea una cuenta "Ahorro".
  - Encabezados: `Authorization: Bearer {token}`
  - Body (JSON):
    {
      "clienteId": "1"  
    }
  - Respuestas:
    - 201 Created: `{ "id": 1, "encodedkey": "...", "mensaje": "Ahorro registrado", "fechaDeRegistro": "..." }`
    - 208 Already Reported si ya existe: `{ "id": 1, "encodedkey": "...", "mensaje": "Ahorro registrado previamente", ... }`

- GET `/api/Ahorros`
  - Descripción: Devuelve el ahorro asociado al `ClienteId` del token.
  - Encabezados: `Authorization: Bearer {token}`
  - Respuesta 200 (JSON):
    { "id": 1, "clienteId": 1, "encodedkey": "...", "total": 0, "fechaDeRegistro": "..." }

- POST `/api/Ahorros/Depositos`
  - Estado: pendiente de implementación en reglas de negocio.
  - Body esperado (referencia): `{ "monto": 100.0, "concepto": "Depósito", "referencia": "ABC123", "encodedkey": "..." }`

## Modelos de datos (resumen)
- ClienteDtoIn
  - `primerApellido` (requerido, máx. 255)
  - `segundoApellido` (opcional, máx. 255)
  - `correo` (requerido, email)
  - `contrasenia` (requerido, máx. 20)
  - `nombre` (requerido, máx. 64)
  - `telefono` (requerido, 10 dígitos)
  - `fechaDeNacimiento` (requerido, fecha)
  - `estadoDeNacimiento` (requerido, 1–32)
  - `sexo` (requerido, "H" o "M")

- InicioDeSesionDto
  - `usuario` (correo)
  - `contraseña` (texto)

- AhorroDtoIn
  - `clienteId` (string; acepta id numérico o `encodedkey` del cliente)

## Notas y solución de problemas
- Swagger solo se habilita en `ASPNETCORE_ENVIRONMENT=Development`.
- Dependencia externa CURP: durante el registro de cliente se invoca `https://utilidades.vmartinez84.xyz/api/Curp`. Si el servicio externo no responde, el registro continúa pero la CURP podría quedar vacía.
- Autorización JWT:
  - Asegúrate de enviar `Authorization: Bearer {token}` en rutas protegidas.
  - Si las rutas con `[Authorize]` no reconocen el token, verifica que en el pipeline esté configurado `UseAuthentication()` antes de `UseAuthorization()`.
- Persistencia:
  - Por defecto usa MongoDB (`ConnectionStrings:MongoDb`). Si deseas SQL/Dapper, ajusta la configuración en `Banco.ReglasDeNegocio/Helpers/ReglasDeNegocioExtensor.cs` para registrar el repositorio correspondiente.

## Licencia
Uso académico/demostrativo. Ajusta según tus necesidades.
