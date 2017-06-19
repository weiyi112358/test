
sc create "Arvato.CRM.WCFService" binpath= "%~dp0Arvato.CRM.WCF.exe"
sc failure "Arvato.CRM.WCFService" reset= 3600 actions= restart/500/restart/5000/restart/30000
sc start "Arvato.CRM.WCFService"
