@echo Off
set config=%1
if "%config%" == "" (
   set config=Release
)

set nuget=
if "%nuget%" == "" (
	set nuget=nuget
)

echo Restoring NuGet packages
%nuget% restore
 
echo Building Solution
 
%WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Liath.ViewRanger.sln /p:Configuration="%config%" /m /v:M /fl /flp:LogFile=msbuild.log;Verbosity=diag /nr:false
 
echo Build Complete
 
mkdir Build
mkdir Build\lib
mkdir Build\lib\net40

echo Packaging 

%nuget% pack "ViewRanger.nuspec" -NoPackageAnalysis -verbosity detailed -o Build -p Configuration="%config%"

echo Done!
