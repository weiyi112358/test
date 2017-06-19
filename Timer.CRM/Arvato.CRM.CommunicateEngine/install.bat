
sc create "Arvato.CRM.CommunicateService" binpath= "%~dp0Arvato.CRM.CommunicateEngine.exe"
sc failure "Arvato.CRM.CommunicateService" reset= 3600 actions= restart/500/restart/5000/restart/30000
sc start "Arvato.CRM.CommunicateService"
