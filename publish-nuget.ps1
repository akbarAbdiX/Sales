param(
    [string]$NugetSource,
    [string]$NugetApiKey,
    [switch]$IgnoreGit=$false
)
Import-Module -Name ./publish-global.psm1

if (-not $NugetSource) {
    $NugetSource = $env:NUGET_SOURCE
}
if (-not $NugetApiKey) {
    $NugetApiKey = $env:NUGET_API_KEY
}

#Discover publish version 
$version = Discover-Version

#Check Git commit status
if(-Not $IgnoreGit){
    Check-Git
}


#build
dotnet restore
Check-Command-Result "restore nuget packages"

#pack
    #top level package is a package with the name of BigBang.Sales and is dependent to all packable solution Project and host dependent packages
$projectName = "BigBang.Applications.Sales"
$tempDirectory = Join-Path $pwd  "`$temp"
$nugetDirectory = Join-Path $tempDirectory  ".nuget"
function createProjectFile {
    param(
        $projectFileName
    )
    $projectData = @'
    <Project Sdk="Microsoft.NET.Sdk">
    <PropertyGroup>
      <TargetFramework>net8.0</TargetFramework>
      <NoBuild>true</NoBuild>
      <IncludeBuildOutput>false</IncludeBuildOutput>
      <IncludeSource>false</IncludeSource>
      <IncludeSymbols>false</IncludeSymbols>
      <NoDefaultExcludes>true</NoDefaultExcludes>
      <NuspecFile>$(MSBuildThisFileDirectory)$(MSBuildProjectName).nuspec</NuspecFile>
      <NuspecBasePath>./</NuspecBasePath>
    </PropertyGroup>
  </Project>
'@
    $xcsProject =[System.Xml.Linq.XDocument]::Parse($projectData)
    $xcsProject.Save($projectFileName)
}
function getHostTopLevelPackages(){
   $json = dotnet list .\src\Host\ package --format json | ConvertFrom-Json
   return $json.projects[0].frameworks[0].topLevelPackages
}
function createNuspecFile {
    param (
        $nuspecFileName,
        $nupackagesDirectory,
        [psobject]$config
    )
    $packageId = $config.name
    $packageTitle = $config.title
    $nuspecHead = @"
    <package>
    <metadata>
      <id>$packageId</id>
      <title>$packageTitle</title>
      <authors>ERP SME Team</authors>
      <owners>ERP SME Team</owners>
      <description>Enterprise sales web based application</description>
      <tags>BigBang Sales</tags>
      <version>
"@
$nuspecHead = $nuspecHead + $version
$nuspecHead = $nuspecHead+ @'
</version>
      <copyright>Copyright ©2025</copyright>
      <dependencies>
'@

        $nuspecTail = @'
        </dependencies>
        </metadata>
        <files>
        <dependency src="*.csproj" exclude="*.csproj" />
        </files>
        </package>
'@
    $dependecies = ''
    $filter = Join-Path $nupackagesDirectory '*.nupkg'
    $nupackageFiles = Get-ChildItem -Path $filter -Name
    foreach ($nupackageFile in $nupackageFiles) {
        $packageId = [System.IO.Path]::GetFileNameWithoutExtension($nupackageFile)
        $packageId = $packageId.Replace(".${version}",'')
        if($config.subPackages.type -eq "reject"){
            $rejected = $false;
            foreach ($regex in $config.subPackages.regexList) {
                if([regex]::IsMatch($packageId, $regex)){
                    $rejected = $true
                    break
                }
            }
            if($rejected){
                continue
            }
        }else{
            $accepted = $false;
            foreach ($regex in $config.subPackages.regexList) {
                if([regex]::IsMatch($packageId, $regex)){
                    $accepted = $true
                    break
                }
            }
            if(-Not $accepted){
                continue
            }
        }
        $dependecies += '<dependency id="'+$packageId+'" version="'+$version+'"/>'
    }
    $xnuspec =[System.Xml.Linq.XDocument]::Parse($nuspecHead+$dependecies+$nuspecTail)
    $xnuspec.Save($nuspecFileName)
}

function createTopLevelPackage {

    $configFilePath = Join-Path $pwd  "publish.nuget.config.json"
        $config = Get-Content $configFilePath -Raw | ConvertFrom-Json
        foreach ($packageConfig in $config) {
            $projectName = $packageConfig.name
            $directory = Join-Path $nugetDirectory ".topLevelPackage"
            $projectFileName = Join-Path $directory "${projectName}.csproj"
            $nuspecFileName = Join-Path $directory "${projectName}.nuspec"
            if(Test-Path -Path $directory){
                Remove-Item -Path $directory -Force -Recurse
            }
            New-Item -Path $directory -ItemType Directory
    
            createNuspecFile -nuspecFileName $nuspecFileName -nupackagesDirectory $nugetDirectory -config $packageConfig
            createProjectFile -projectFileName $projectFileName
            dotnet pack $projectFileName /p:Version=$version --output $nugetDirectory
        }
}
function createPackages {
    if(Test-Path -Path $nugetDirectory){
        Remove-Item -Path $nugetDirectory -Force -Recurse
    }
    New-Item -Path $nugetDirectory -ItemType Directory
    
    dotnet pack /p:Version=$version --configuration Release
    Check-Command-Result 'packing nuget packages' 
    createTopLevelPackage    
}
function pushPackage {
    $filter = Join-Path $nugetDirectory '*.nupkg'
    $nupackageFiles = Get-ChildItem -Path $filter -Name
    foreach ($nupackageFile in $nupackageFiles) {
        $nugetPackageFullPath = Join-Path $nugetDirectory $nupackageFile
        $packageId = [System.IO.Path]::GetFileNameWithoutExtension($nupackageFile)
        $packageId = $packageId.Replace(".${version}",'')
        if([string]::IsNullOrEmpty($NugetApiKey)){
            dotnet nuget push $nugetPackageFullPath --source $NugetSource
        }else{
            dotnet nuget push $nugetPackageFullPath --source $NugetSource --api-key $NugetApiKey
        }

        Check-Command-Result "push package with id ${packageId} and version ${version}"
    }
}
createPackages

#push package
if(-Not [string]::IsNullOrEmpty($NugetSource)){
    pushPackage
}

#Create git tag and push it to remote repository
if(-Not $IgnoreGit){
    Create-Tag
    Push-Tag
}