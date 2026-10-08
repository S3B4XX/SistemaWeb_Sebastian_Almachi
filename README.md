# Proyecto_WEB: Inventario de productos con Login (ASP.NET Core MVC)

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-512BD4)
![SQLite](https://img.shields.io/badge/SQLite-desarrollo-003B57?logo=sqlite&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-producci%C3%B3n-4169E1?logo=postgresql&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5-7952B3?logo=bootstrap&logoColor=white)

Aplicación web académica hecha con **ASP.NET Core MVC (.NET 8)** que combina un **sistema de autenticación (login)** con un **CRUD de productos** protegido. Solo los usuarios que inician sesión pueden entrar a la sección de productos: cualquier URL protegida redirige al login si no hay una sesión activa.



## Tabla de contenidos

- [Funcionalidades](#funcionalidades)
- [Tecnologías](#tecnologías)
- [Arquitectura MVC](#arquitectura-mvc)
- [Requisitos previos](#requisitos-previos)
- [Instalación y ejecución](#instalación-y-ejecución)
- [Cómo usar la aplicación](#cómo-usar-la-aplicación)
- [Autenticación y cifrado de contraseñas](#autenticación-y-cifrado-de-contraseñas)
- [Base de datos](#base-de-datos)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Autor](#autor)

## Funcionalidades

- **Registro e inicio de sesión** con usuario (correo electrónico) y contraseña.
- **Cierre de sesión** desde la barra de navegación.
- **Rutas protegidas:** sin sesión iniciada, `/`, `/Productos` y todas sus acciones redirigen al login.
- **CRUD completo de productos:** crear, listar, ver detalle, editar y eliminar.
- **Validaciones** en el navegador y en el servidor, con mensajes en español.
- **Contraseñas cifradas:** nunca se guardan en texto plano.
- **Interfaz en español** y con HTML semántico, pensado para la accesibilidad.

## Tecnologías

| Capa | Tecnología |
|---|---|
| Lenguaje y framework | C# con ASP.NET Core MVC (.NET 8) |
| Acceso a datos | Entity Framework Core 8 |
| Base de datos (desarrollo) | SQLite |
| Base de datos (producción) | PostgreSQL con Npgsql |
| Autenticación | ASP.NET Core Identity (cookies) |
| Vistas | Razor + Bootstrap 5 |

## Arquitectura MVC

El proyecto separa las responsabilidades según el patrón **Modelo-Vista-Controlador**:

| Componente | Responsabilidad | Archivos |
|---|---|---|
| **Modelo** | Estructura y validación de los datos | `Models/Producto.cs`, `Data/ApplicationDbContext.cs` |
| **Vista** | Solo presentación (Razor), sin lógica de negocio ni consultas | `Views/Productos/*`, `Views/Home/*`, `Views/Shared/*` |
| **Controlador** | Recibe las peticiones, usa el modelo y elige la vista | `Controllers/ProductosController.cs`, `Controllers/HomeController.cs` |

La pantalla de login es una página de Identity personalizada: `Areas/Identity/Pages/Account/Login.cshtml` (vista) y `Login.cshtml.cs` (lógica).

### Rutas del CRUD

| Operación | Método y ruta |
|---|---|
| Listar | `GET /Productos` |
| Ver detalle | `GET /Productos/Details/{id}` |
| Crear | `GET` y `POST /Productos/Create` |
| Editar | `GET` y `POST /Productos/Edit/{id}` |
| Eliminar | `GET` (confirmación) y `POST /Productos/Delete/{id}` |

Todas están bajo `[Authorize]`.

## Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- Un editor como Visual Studio 2022 o Visual Studio Code (opcional)

Para verificar la instalación:

```bash
dotnet --version
```

## Instalación y ejecución

1. Clona el repositorio y entra a la carpeta:

   ```bash
   git clone <URL-DEL-REPOSITORIO>
   cd <CARPETA-DEL-REPOSITORIO>
   ```

2. Restaura las dependencias y ejecuta:

   ```bash
   dotnet restore
   dotnet run --launch-profile https
   ```

3. Abre en el navegador la dirección que aparece en la consola. Con el perfil `https` es `https://localhost:7282`.

No hace falta instalar ninguna base de datos: en desarrollo se usa **SQLite** y el archivo `proyecto_web.db` se crea automáticamente la primera vez.

Si el navegador muestra una advertencia de certificado, ejecuta una vez `dotnet dev-certs https --trust`.

## Cómo usar la aplicación

1. Al abrir la aplicación sin sesión, te redirige a la pantalla de **Iniciar sesión**.
2. Pulsa **Regístrese** y crea un usuario con un correo y una contraseña. La contraseña debe tener al menos 6 caracteres, una mayúscula, una minúscula, un número y un símbolo.
3. Inicia sesión. Verás la página de inicio y el enlace **Productos** en el menú.
4. En **Productos** puedes crear, ver, editar y eliminar registros.
5. Pulsa **Cerrar sesión** y prueba abrir `/Productos` directamente: volverás al login.

La aplicación **no trae usuarios precargados**: cada persona se registra con su propio correo.

## Autenticación y cifrado de contraseñas

- Se usa **ASP.NET Core Identity** con autenticación por **cookie**.
- Las vistas y acciones protegidas llevan el atributo `[Authorize]`. Si no hay sesión, ASP.NET redirige a `/Identity/Account/Login?ReturnUrl=...`.
- Las contraseñas **se guardan cifradas (hash con sal)**, nunca en texto plano. Identity usa **PBKDF2** (HMAC-SHA512) con una sal aleatoria por usuario.
- Se eligió PBKDF2 en lugar de **MD5** porque MD5 está obsoleto para contraseñas: es rápido de atacar y no usa sal. PBKDF2 cumple el mismo objetivo (que la contraseña no sea legible en la base de datos) con un nivel de seguridad adecuado.

**Cómo comprobarlo:** abre `proyecto_web.db` con [DB Browser for SQLite](https://sqlitebrowser.org/) y revisa la tabla `AspNetUsers`. La columna `PasswordHash` contiene el hash, no la contraseña.

## Base de datos

| Entorno | Motor | Configuración |
|---|---|---|
| Desarrollo | SQLite | `appsettings.Development.json` → `ConnectionStrings:DefaultConnection` |
| Producción | PostgreSQL (Npgsql) | Variable de entorno `ConnectionStrings__DefaultConnection` |

`Program.cs` elige el motor según el entorno (`IsDevelopment()`). Ejemplo de cadena de conexión para PostgreSQL:

```text
Host=mi-servidor;Port=5432;Database=mi_base;Username=mi_usuario;Password=mi_clave
```

Las tablas se crean al arrancar con `EnsureCreated()`, que funciona igual en SQLite y en PostgreSQL. **Si cambias un modelo**, borra los archivos `proyecto_web.db*` para que la base se cree de nuevo (se pierden los datos de prueba).

## Estructura del proyecto

```text
Proyecto_WEB/
├── Areas/Identity/Pages/Account/   # Login personalizado (Login.cshtml y Login.cshtml.cs)
├── Controllers/                    # HomeController y ProductosController
├── Data/                           # ApplicationDbContext (EF Core + Identity)
├── Models/                         # Producto y ErrorViewModel
├── Views/
│   ├── Home/                       # Inicio y privacidad
│   ├── Productos/                  # Index, Details, Create, Edit, Delete y _Formulario
│   └── Shared/                     # Layout, menú de sesión y página de error
├── wwwroot/                        # Archivos estáticos (Bootstrap, jQuery, CSS, JS)
├── Program.cs                      # Configuración de servicios y pipeline
├── appsettings.json                # Configuración general
├── appsettings.Development.json    # Cadena de conexión de desarrollo (SQLite)
└── Proyecto_WEB.csproj             # Proyecto .NET 8 y paquetes NuGet
```

## Autor

**Sebastián Almachi**, estudiante de Ingeniería de Software, Universidad de las Américas (UDLA), Ecuador.

Proyecto realizado con fines académicos para la asignatura Ingeniería Web.
