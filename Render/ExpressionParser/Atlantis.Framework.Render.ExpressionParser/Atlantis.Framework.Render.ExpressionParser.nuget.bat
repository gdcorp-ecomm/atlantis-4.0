@ECHO off
nuget pack Atlantis.Framework.Render.ExpressionParser.csproj -IncludeReferencedProjects -Build -Prop Configuration=Release -OutputDirectory \\g1dwdevmgmt001\webcontent\nuget.packages\
PAUSE
