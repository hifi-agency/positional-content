SET APPVEYOR_REPO_BRANCH=%BUILD_SOURCEBRANCHNAME%
SET APPVEYOR_REPO_TAG=true
SET APPVEYOR_BUILD_NUMBER=%REVISION%
SET APPVEYOR_BUILD_VERSION=%BUILD_BUILDNUMBER%

ECHO APPVEYOR_REPO_BRANCH: %APPVEYOR_REPO_BRANCH%
ECHO APPVEYOR_REPO_TAG: %APPVEYOR_REPO_TAG%
ECHO APPVEYOR_BUILD_NUMBER : %APPVEYOR_BUILD_NUMBER%
ECHO APPVEYOR_BUILD_VERSION : %APPVEYOR_BUILD_VERSION%

Call Tools\nuget.exe restore ..\PositionalContent.sln
CALL "%programfiles(x86)%\Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin\MsBuild.exe" package.proj