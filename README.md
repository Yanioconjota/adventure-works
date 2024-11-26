
# Adventure Works API 🚀

Este proyecto es una API basada en **.NET 8** y **SQL Server** diseñada para administrar datos de la base de datos AdventureWorks. La aplicación está Dockerizada para facilitar su despliegue y ejecución en cualquier entorno.

---

## **Requisitos previos 🛠**

Antes de comenzar, asegúrate de tener instalados los siguientes componentes en tu máquina:

- **Docker** y **Docker Compose**:
  - Verifica la instalación con:
    ```bash
    docker --version
    docker-compose --version
    ```
- **Git** (opcional, para clonar el repositorio):
  ```bash
  git --version
  ```

---

## **Instrucciones de configuración y despliegue 🐳**

### **1. Clonar el repositorio**

Clona este repositorio en tu máquina local:

```bash
git clone https://github.com/tu-usuario/adventure-works.git
cd adventure-works
```

---

### **2. Configurar variables de entorno**

El proyecto utiliza variables de entorno para configurar tanto la API como el contenedor de SQL Server. Asegúrate de que los archivos `.env` están configurados correctamente:

- Archivo principal: `.env` en la carpeta raíz.
- Archivo específico de la API: `adventureworksapi/Presentation/.env`.

#### Ejemplo del archivo `.env`:
```env
SA_PASSWORD=MyStrongPassWordHere
ACCEPT_EULA=Y

# Puertos
SQLSERVER_PORT=1433
API_PORT=8000

#Connection String
DB_CONNECTION_STRING=Server=sqlserver;Database=AdventureWorks2017;User Id=sa;Password=MyStrongPassWordHere;Encrypt=False;TrustServerCertificate=True;
```

---

### **3. Construir y levantar los contenedores**

Usa Docker Compose para construir y levantar los servicios:

```bash
docker-compose up --build
```

Esto hará lo siguiente:
- Construirá la imagen de la base de datos SQL Server y restaurará la base de datos AdventureWorks.
- Construirá la API y la conectará al contenedor de SQL Server.

---

### **4. Verificar los servicios**

- **SQL Server**:
  - Disponible en el puerto `1433`.
  - Puedes conectarte usando herramientas como SQL Server Management Studio (SSMS).
  - Configuración de conexión:
    - Servidor: `localhost`
    - Usuario: `sa`
    - Contraseña: `MyStrongPassWordHere`

- **API**:
  - Disponible en el puerto `8000`.
  - Endpoint principal: [http://localhost:8000/swagger/index.html](http://localhost:8000/swagger/index.html).

---

## **Estructura del proyecto 📁**

```plaintext
adventure-works/
├── adventureworksapi/
│   ├── Application/            # Lógica de negocio
│   ├── Domain/                 # Entidades y modelos de datos
│   ├── Infrastructure/         # Configuración de acceso a datos
│   ├── Presentation/           # Controladores y capa web
│   │   ├── .env                # Variables de entorno para la API
│   │   ├── appsettings.json    # Configuración de la API
│   └── Dockerfile              # Dockerfile para la API
├── scripts/
│   ├── sqlserver.Dockerfile    # Dockerfile para SQL Server
│   └── init-db.sh              # Script de inicialización de la base de datos
├── backups/
│   └── AdventureWorks2017.bak  # Backup de la base de datos
├── .env                        # Variables de entorno globales
├── docker-compose.yml          # Archivo de configuración de Docker Compose
└── README.md                   # Este archivo
```

---

## **Comandos útiles 🧑‍💻**

### **Levantar servicios en segundo plano**
```bash
docker-compose up -d
```

### **Detener los servicios**
```bash
docker-compose down
```

### **Reconstruir servicios**
Si realizas cambios en el código o en los Dockerfiles:
```bash
docker-compose up --build
```

---

## **Solución de problemas 🛠**

### **1. Error de conexión a la base de datos**
Si la API no puede conectarse a SQL Server:
- Verifica que los contenedores están corriendo:
  ```bash
  docker ps
  ```
- Revisa los logs del contenedor de SQL Server:
  ```bash
  docker logs adventure-works-sqlserver-1
  ```

### **2. Variables de entorno faltantes**
Asegúrate de que los archivos `.env` estén correctamente configurados y que las rutas sean accesibles desde el archivo `docker-compose.yml`.

### **3. Puertos en uso**
Si los puertos `1433` o `8000` están ocupados:
- Cambia los puertos en `docker-compose.yml`:
  ```yaml
  ports:
    - "nuevo_puerto:8000" # Cambiar el puerto de la API
    - "nuevo_puerto:1433" # Cambiar el puerto de SQL Server
  ```

### **4. Error de formato en archivos sh**
Convertir el archivo a formato `UNIX`
1. Convertir desde VS Code
2. Abre init-db.sh en Visual Studio Code.
3. Mira en la parte inferior derecha de la ventana. Si ves CRLF, haz clic en ello.
4. Cambia CRLF a LF.
5. Guarda el archivo.
6. Reconstruir y ejecutar
```bash
docker-compose down
docker-compose up --build
```

Esto eliminará los caracteres `\r` y convertirá el archivo al formato `UNIX`.

---
