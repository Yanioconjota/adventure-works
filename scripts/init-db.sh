#!/bin/bash

# Iniciar SQL Server y ponerlo en segundo plano
/opt/mssql/bin/sqlservr &

# Esperar a que SQL Server esté completamente operativo
sleep 30

# Ejecutar script SQL para restaurar la base de datos
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P "$SA_PASSWORD" -Q "RESTORE DATABASE AdventureWorks2017 FROM DISK = '/scripts/backups/AdventureWorks2017.bak' WITH MOVE 'AdventureWorks2017' TO '/var/opt/mssql/data/AdventureWorks2017.mdf', MOVE 'AdventureWorks2017_log' TO '/var/opt/mssql/data/AdventureWorks2017_Log.ldf';"

# Mantener el proceso de SQL Server en primer plano
wait