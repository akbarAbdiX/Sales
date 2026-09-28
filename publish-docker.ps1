param(
    [string]$DockerRegistry,
	[string]$ServerAddress,
    [switch]$IgnoreGit=$false
)
Import-Module -Name ./publish-global.psm1

#Discover publish version 
$version = Discover-Version

#Check Git commit status
if(-Not $IgnoreGit){
    Check-Git
}

$tempDirectory = Join-Path $pwd  "`$temp"

$applicationName = 'app.sales'
$applicationName = $applicationName.ToLower()
$imageName = "bigbang/"+$applicationName+':'+$version
$latestImageName = "bigbang/"+$applicationName+':latest'

$env:DOCKER_BUILDKIT = "0"
docker build --network=host --label version=$version -t $imageName -f ./.docker/Dockerfile ./
Remove-Item Env:\DOCKER_BUILDKIT

Check-Command-Result 'docker build'
docker tag $imageName $latestImageName
Check-Command-Result 'docker tag'

if(-Not [string]::IsNullOrEmpty($DockerRegistry)){
    $registryImageName = $DockerRegistry+'/'+$imageName
    $registryLatestImageName = $DockerRegistry+'/'+$latestImageName
    docker tag $imageName $registryImageName
    Check-Command-Result 'docker tag'
    docker tag $latestImageName $registryLatestImageName
    Check-Command-Result 'docker tag'
    docker push $registryImageName
    Check-Command-Result 'docker push'
    docker push $registryLatestImageName
    Check-Command-Result 'docker push'

}

#Create git tag and push it to remote repository
if(-Not $IgnoreGit){
    Create-Tag
    Push-Tag
}

$environmentName = ""
if ((git branch --show-current) -eq "test") {
    $environmentName = "alpha"
}
if ((git branch --show-current) -eq "stage") {
    $environmentName = "beta"
}

if ((-Not [string]::IsNullOrEmpty($environmentName))) {
    Write-Host "Deploying on $environmentName server"
    $version = Discover-Version
    $imageName ="registry.bigbang.dotin.ir/bigbang/app.sales:"+$version
    $applicationPath = "/app/$environmentName/app.sales"

    Write-Host "Updating .env files"
    ssh $ServerAddress "echo -e APP_SALES_IMAGE=$imageName >  $applicationPath/.env" 
    Check-Command-Result "Updating .env files"

    Write-Host "Pull docker"
    ssh $ServerAddress "docker pull $imageName"
    Check-Command-Result "Pull docker image"

    Write-Host "docker compose down"
    ssh $ServerAddress "cd $applicationPath && docker-compose down"
    Check-Command-Result "docker compose down"

    Write-Host "docker compose up"
    ssh $ServerAddress "cd $applicationPath && docker-compose up -d"
    Check-Command-Result "docker compose up"
}