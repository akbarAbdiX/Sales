pipeline {
    agent { label 'dotnet' }

    stages {
        stage('Build') {
            steps {
                echo 'Building...'
                
				sh 'pwsh ./build.ps1 -PreventMigration'
				
				echo 'Built'
            }
        }
        stage('Deploy') {
            when {
                expression {
                    return env.BRANCH_NAME ==~ /^(release\/.)|(test)|(stage)|(master)*$/
                }
            }
            steps {
                echo 'Deploying...'
				
                withCredentials([string(credentialsId: 'ERP_NUGET_API_KEY', variable: 'ERP_NUGET_API_KEY')]) {
                    sh 'pwsh ./publish-nuget.ps1 -NugetSource "https://nexus3.dotin.ir/repository/ERP-NuGet/" -NugetApiKey $ERP_NUGET_API_KEY'
                }
                
				echo 'Deployed'
            }
        }
    }
}