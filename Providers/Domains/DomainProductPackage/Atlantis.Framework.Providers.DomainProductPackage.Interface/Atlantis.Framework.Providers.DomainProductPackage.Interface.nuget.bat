﻿@ECHO off
nuget pack Atlantis.Framework.Providers.DomainProductPackage.Interface.csproj -IncludeReferencedProjects -Build -Prop Configuration=Release -OutputDirectory \\g1dwdevmgmt001\webcontent\nuget.packages\
