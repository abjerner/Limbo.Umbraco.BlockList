@echo off
dotnet build src/Limbo.Umbraco.BlockList --configuration Debug /t:rebuild /t:pack -p:BuildTools=1 -p:PackageOutputPath=c:/nuget