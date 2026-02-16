¡Exacto, Deko! Para que no te falte ni un solo punto de la rúbrica y asegures ese 100/100, vamos a pulir el README.md integrando específicamente esos 5 puntos que mencionaste.

Aquí tienes la versión final "Master" que incluye: pasos de instalación, variables, el archivo .http, las credenciales para la demo y una estructura impecable.

🎓 Sistema de Gestión Académica - UNIANDES 2026
Este proyecto es una aplicación Full-Stack para la administración de estudiantes, con un enfoque en seguridad End-to-End y arquitectura desacoplada.

🚀 1. Pasos para la Instalación y Configuración
Requisitos Previos:
XAMPP (MySQL activo).

.NET 8 SDK.

Node.js & Angular CLI 19.

Paso 1: Base de Datos (MySQL)
Abre phpMyAdmin.

Crea una base de datos llamada db_sistema_escolar.

Importa el archivo db_sistema_escolar.sql que se encuentra en la carpeta /Base_de_Datos de este repositorio.

Paso 2: Backend (.NET 8 Web API)
Abre la carpeta Backend_NET en Visual Studio.

Revisa el archivo appsettings.json (ver sección de Variables de Entorno).

Presiona F5 o el botón "Play" para iniciar el servidor en https://localhost:7299.

Paso 3: Frontend (Angular 19)
Abre la carpeta Frontend_Angular en VS Code.

Ejecuta npm install para instalar dependencias.

Inicia la aplicación con ng serve.

Abre tu navegador en http://localhost:4200.

⚙️ 2. Variables de Entorno y Configuración
El sistema utiliza las siguientes configuraciones clave:

Connection String (Backend): Ubicada en appsettings.json.

"DefaultConnection": "Server=localhost;Database=db_sistema_escolar;User=root;Password=;"

API Endpoint (Frontend): Configurado en src/app/services/estudiante.ts.

private apiUrl = 'https://localhost:7299/api/ApiEstudiantes';

CORS Policy: Habilitada en Program.cs para permitir peticiones desde el puerto 4200.

🔐 3. Credenciales para Demo (Acceso Total)
Para realizar una prueba rápida del sistema sin crear nuevos usuarios, utiliza:

Usuario: darwinma03@uniandes.edu.ec

Contraseña: Admin123

Nota: La sesión expira automáticamente después de 20 minutos de inactividad por seguridad.

🧪 4. Pruebas de Integración (Postman / .http)
Se ha incluido un archivo de pruebas integrado en Visual Studio para validar los Endpoints sin necesidad de usar el navegador:

Archivo: PruebasSistema.http (Ubicado en la raíz del proyecto Backend).

Contenido: Pruebas de Login, Listado, Registro y Eliminación.

Uso: Abrir en Visual Studio y hacer clic en el botón de "Play" sobre cada petición.

📁 5. Estructura del Proyecto
Plaintext
/
├── Base_de_Datos/          # Script .sql para MySQL
├── Backend_NET/            # Código fuente ASP.NET Core 8 Web API
│   └── PruebasSistema.http # Pruebas de integración
└── Frontend_Angular/       # Código fuente Angular 19 (Standalone)
    └── src/app/            # Componentes y Servicios
Desarrollado por: Dario Moyano Alvarez 

 UNIANDES 2026