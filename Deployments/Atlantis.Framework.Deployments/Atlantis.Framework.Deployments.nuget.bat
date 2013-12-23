@ECHO off
nuget pack Atlantis.Framework.Deployments.csproj -IncludeReferencedProjects -Build -Prop Configuration=Release -OutputDirectory \\g1dwdevmgmt001\webcontent\nuget.packages\