
sc create "Arvato.CRM.SubdivisionEngine" binpath= "%~dp0Arvato.CRM.SubdivisionEngine.exe"
sc failure "Arvato.CRM.SubdivisionEngine" reset= 3600 actions= restart/500/restart/5000/restart/30000
sc start "Arvato.CRM.SubdivisionEngine"
