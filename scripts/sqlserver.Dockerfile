FROM mcr.microsoft.com/mssql/server:2022-latest

# Cambiar a root para permisos de instalación
USER root

# Actualizar e instalar dependencias necesarias
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        curl \
        apt-transport-https \
        gnupg && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/22.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y --no-install-recommends \
        mssql-tools unixodbc-dev && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*

# Añadir herramientas de SQL al PATH
ENV PATH=$PATH:/opt/mssql-tools/bin

# Cambiar de nuevo al usuario mssql
USER mssql

# Comando por defecto para iniciar SQL Server
CMD ["/opt/mssql/bin/sqlservr"]
