# CT_Login

Proyecto de microservicio de autenticación en .NET (C#) que maneja los endpoints de **login** y **refresh-token** usando JWT.

## Funcionalidades

- **Login:** Autenticación de usuarios con validación de credenciales y generación de token JWT.
- **Refresh Token:** Renovación del token JWT a partir del token expirado.
- **Integración con API externa:** Validación especial para conductores a través de un servicio externo.

## Estructura del Proyecto

El proyecto se organiza en las siguientes carpetas:

- **Controllers:** Contiene los controladores de la API (por ejemplo, `AuthController.cs`).
- **Models:** Define los modelos y DTOs usados en la aplicación (por ejemplo, `LoginRequest.cs`, `LoginResponse.cs`, `RefreshTokenRequest.cs`).
- **Repositories:** Encapsula el acceso a la base de datos (por ejemplo, `UserRepository.cs`).
- **Services:** Contiene la lógica de negocio y la integración con servicios externos (por ejemplo, `AuthService.cs` y `ConductoresBukService.cs`).
- **Helpers:** Clases de utilidad, como `JwtHelper.cs` para la generación y validación de tokens.

## Configuración

El proyecto utiliza un archivo **.env** para almacenar datos sensibles y de configuración. Asegúrate de crear el archivo **.env** en la raíz del proyecto con el siguiente contenido (ajusta los valores según tu entorno):

```
JWT_SECRET_KEY=EstaEsUnaClaveSuperSegura1234567890
DB_CONNECTION_STRING=Server=tu_servidor;Database=tu_basededatos;User Id=tu_usuario;Password=tu_contraseña;
EXTERNAL_API_TOKEN=
```

## Requisitos

- [.NET 6 SDK](https://dotnet.microsoft.com/download) o superior.
- SQL Server para la base de datos.
- Git para el control de versiones.

## Procedimiento Almacenado

Se utiliza un procedimiento almacenado llamado `sp_GetUserRolesAndSubroles` para optimizar la consulta de roles y subroles del usuario. Puedes crearlo en tu base de datos con el siguiente script:

```sql
CREATE PROCEDURE sp_GetUserRolesAndSubroles
    @UsuarioID INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Consulta de Roles
    SELECT r.*
    FROM Roles r
    INNER JOIN UsuarioRoles ur ON r.RolID = ur.RolID
    WHERE ur.UsuarioID = @UsuarioID;

    -- Consulta de Subroles
    SELECT sr.*
    FROM Subroles sr
    INNER JOIN UsuarioSubroles usr ON sr.SubrolID = usr.SubrolID
    WHERE usr.UsuarioID = @UsuarioID;
END
GO
```

## Instalación y Ejecución

1. **Restaura los paquetes:**
    
    ```bash
    dotnet restore
    ```
    
2. **Compila el proyecto:**
    
    ```bash
    dotnet build
    
    ```
    
3. **Ejecuta la aplicación:**
    
    ```bash
    dotnet run
    ```
    

La aplicación se ejecutará y podrás acceder a los endpoints, por ejemplo:

- `POST /api/auth/login`
- `POST /api/auth/refresh-token`

## Uso de la API

### Endpoint de Login

**Request:**

```json
{
  "username": "tu_usuario",
  "password": "tu_contraseña"
}
```

**Response:**

```json
{
  "state": "successful",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Endpoint de Refresh Token

**Request:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Response:**

```json
{
  "state": "successful",
  "token": "nuevo_token_JWT..."
}
```

## Notas y Consideraciones

- **Seguridad:**
    
    Se recomienda mejorar el manejo de contraseñas usando hashing (por ejemplo, BCrypt) en lugar de comparaciones directas.
    
- **Validaciones y Errores:**
    
    Asegúrate de manejar adecuadamente los casos de error y validar la entrada de datos.
    
- **Actualizaciones:**
    
    Cada vez que realices cambios, recuerda actualizar tu repositorio:
    
    ```bash
    git add .
    git commit -m "Descripción de los cambios"
    git push
    ```
    

## Contribuciones

Si deseas contribuir a este proyecto, por favor abre un *issue* o envía un *pull request*.