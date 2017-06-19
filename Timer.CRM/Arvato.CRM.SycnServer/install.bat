sc create "Arvato.CRM.SycnServer" binpath= "%~dp0Arvato.CRM.SycnServer.exe"
sc failure "Arvato.CRM.SycnServer" reset= 3600 actions= restart/500/restart/5000/restart/30000
sc start "Arvato.CRM.SycnServer"
pause 