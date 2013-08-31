@ECHO off
nuget pack Atlantis.Framework.AppSettings.Interface.csproj -IncludeReferencedProjects -Build -Prop Configuration=Release -OutputDirectory \\g1dwdevmgmt001\webcontent\nuget.packages\
PAUSE