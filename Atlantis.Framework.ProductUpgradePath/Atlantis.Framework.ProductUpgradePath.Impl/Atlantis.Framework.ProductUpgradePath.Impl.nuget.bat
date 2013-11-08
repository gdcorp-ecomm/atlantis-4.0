@ECHO off
nuget pack Atlantis.Framework.ProductUpgradePath.Impl.csproj -IncludeReferencedProjects -Build -Prop Configuration=Release -OutputDirectory \\g1dwdevmgmt001\webcontent\nuget.packages\
