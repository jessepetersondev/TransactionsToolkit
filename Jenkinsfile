pipeline {
  agent any

  stages {
    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Restore & Build') {
      steps {
        sh 'dotnet restore'
        sh 'dotnet build --no-restore --configuration Release'
      }
    }

    stage('Test') {
      steps {
        sh '''
          dotnet test tests/TransactionsToolkit.Tests/ \
            --no-build --configuration Release \
            --logger "trx;LogFileName=test_results.trx"
        '''
      }
      post {
        always {
          // Publish test report in Jenkins
          mstest testResultsFile: '**/test_results.trx'
        }
      }
    }
  }

  post {
    success {
      echo '🟢 Build & tests passed!'
    }
    failure {
      echo '🔴 Build or tests failed—see console output.'
    }
  }
}
