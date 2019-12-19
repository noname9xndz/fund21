@ECHO OFF
ECHO 'Stopping IIS'
C:\Windows\System32\iisreset.exe /stop

DEL  /F /Q C:\inetpub\wwwroot\com.orbitteam.api
RMDIR /s /q C:\inetpub\wwwroot\com.orbitteam.api

ECHO 'Starting IIS'
C:\Windows\System32\iisreset.exe /start