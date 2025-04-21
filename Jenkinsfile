pipeline {
  agent any    // or: agent { label 'windows' } if you have a Windows‑only node

  stages {
    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Restore & Build') {
      steps {
        bat 'dotnet restore'
        bat 'dotnet build --no-restore --configuration Release'
      }
    }

    stage('Test') {
      steps {
        // Run tests, output a TRX file
        bat '''
          dotnet test tests\\TransactionsToolkit.Tests\\ `
            --no-build --configuration Release `
            --logger "trx;LogFileName=test_results.trx"
        '''
      }
      post {
        always {
          // Publish the TRX test report to the MSTest plugin
          mstest testResultsFile: '**\\test_results.trx'
        }
      }
    }
  }

  post {
    success {
      echo '🟢 All builds & tests passed!'
    }
    failure {
      echo '🔴 Build or tests failed — check the console output.'
    }
  }
}
