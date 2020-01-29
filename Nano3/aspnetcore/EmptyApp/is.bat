sc create MyService binPath= %~EmptyApp.exe
sc failure MyService actions= restart/60000/restart/60000/""/60000 reset= 86400
sc start MyService
sc config MyService start=auto