@echo off
dotnet build src/Limbo.Umbraco.BlockList --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget