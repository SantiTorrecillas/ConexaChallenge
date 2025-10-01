# Conexa Challenge

Este repositorio contiene la solución al **challenge técnico para Conexa**, desarrollado en **.NET 9** con **Entity Framework Core** y **SQL Server**.

---

## Configuración del proyecto

### 1. Clonar el repositorio
```bash
git clone https://github.com/SantiTorrecillas/ConexaChallenge.git
cd ConexaChallenge
```

### 2. Configurar la base de datos
Editar el archivo `appsettings.json` y actualizar la cadena de conexión con tus credenciales de SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=ConexaDb;User Id=sa;Password=your_password;TrustServerCertificate=True;"
}
```

### 3. Aplicar migraciones
Desde la **Package Manager Console** de Visual Studio o tu IDE preferido:

```powershell
update-database
```

> ⚠️ Si el comando no se reconoce, probablemente falte instalar los paquetes de Entity Framework Core Tools.

### 4. Ejecutar la API
Iniciar el proyecto desde Visual Studio o con:

```bash
dotnet run --project ConexaChallenge
```

Por defecto, la API estará disponible en:
```
http://localhost:5128
```

---

## Documentación de la API

Este proyecto utiliza **Scalar** en lugar de Swagger, una alternativa moderna y compatible con .NET 9.

La documentación interactiva estará disponible en:
```
http://localhost:5128/scalar/v1
```

---

## Tests

Para validar el correcto funcionamiento:

- Desde **Visual Studio Test Explorer**, o  
- Con CLI:

```bash
dotnet test
```

---

##  Tecnologías principales

- [.NET 9](https://dotnet.microsoft.com/)  
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)  
- [SQL Server](https://www.microsoft.com/sql-server)  
- [Scalar](https://scalar.com/) para documentación de APIs  

---

## Estado

El proyecto se encuentra en una versión estable para levantar en local y correr con base de datos SQL Server.
