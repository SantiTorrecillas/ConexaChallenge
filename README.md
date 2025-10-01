ConexaChallenge

Este repositorio contiene la solución al challenge técnico para Conexa.  
Luego de clonar el repositorio configurar la conexión a la base de datos:

-Configurar ambiente
	Editar `appsettings.json` con tu cadena de conexión a SQL Server:
   	"ConnectionStrings": {
     	"DefaultConnection": "Server=localhost;Database=ConexaDb;User Id=sa;Password=your_password;TrustServerCertificate=True;"
   	}

-Ejecutar migraciones
	Desde la package manajer console seria suficiente con ejecutar "update-database", de no reconocerse este comando podria estar faltando alguna nugget relacionado a entity framework

-Ejecutar en local
	La api comenzara a ejecutarse en http://localhost:5128 como indica el `appsettings.json`
  
-Este proyecto usa Scalar como documentación interactiva de la API en vez de Swagger, una opcion reciente y mas moderna, aceptada por la comunidad de .NET 9
	La UI de Scalar estará disponible en: http://localhost:5128/scalar/v1
  <img width="347" height="667" alt="image" src="https://github.com/user-attachments/assets/cb4774f9-6cec-4051-b5a9-94a9403e32ce" />
  <img width="1509" height="666" alt="image" src="https://github.com/user-attachments/assets/328e649a-6653-48b7-a4d0-e4254c171c34" />
  <img width="1773" height="776" alt="image" src="https://github.com/user-attachments/assets/60c3a220-0f61-44e6-abf1-a7123d32502a" />


-Se Sugiere correr los test para comprobar su correcto funcionamiento desde el test explorer en Visual Studio o desde su IDE de preferencia
