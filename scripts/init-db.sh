#!/bin/bash

# Iniciar SQL Server y ponerlo en segundo plano
/opt/mssql/bin/sqlservr &

# Esperar a que SQL Server esté completamente operativo
sleep 15

# Ejecutar script SQL para restaurar la base de datos
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P "Elasticp!" -d master -i /scripts/restore-database.sql

# Mantener el proceso de SQL Server en primer plano
wait