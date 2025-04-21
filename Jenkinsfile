pipeline {
  agent any   // or agent { label 'windows' }

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
        // Run exactly one project and no stray backticks
        bat """
          dotnet test "tests\\TransactionsToolkit.Tests\\TransactionsToolkit.Tests.csproj" ^
            --no-build --configuration Release ^
            --logger "trx;LogFileName=test_results.trx"
        """
      }
    }
  }

  post {
    success {
      echo '🟢 Build & tests passed!'
    }
    failure {
      echo '🔴 Build or tests failed—please check the console output.'
    }
  }
}
