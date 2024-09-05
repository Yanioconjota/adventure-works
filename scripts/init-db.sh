#!/bin/bash

# Iniciar SQL Server en segundo plano
/opt/mssql/bin/sqlservr &

# Instalar las herramientas de línea de comando de SQL Server
apt-get update && apt-get install -y curl apt-transport-https gnupg2 && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev && \
    echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc && \
    source ~/.bashrc

# Esperar a que SQL Server esté listo
sleep 30

# Ejecutar el script SQL para restaurar la base de datos
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Elasticp!" -d master -i /scripts/restore-database.sql

# Mantener el proceso de SQL Server en primer plano
wait
