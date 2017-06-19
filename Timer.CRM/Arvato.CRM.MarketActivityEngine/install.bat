
sc create "Arvato.CRM.MarketActivityEngine" binpath= "%~dp0Arvato.CRM.MarketActivityEngine.exe"
sc failure "Arvato.CRM.MarketActivityEngine" reset= 3600 actions= restart/500/restart/5000/restart/30000
sc start "Arvato.CRM.MarketActivityEngine"
