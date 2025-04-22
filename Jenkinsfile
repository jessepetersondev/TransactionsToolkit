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
        bat 'dotnet restore'
        bat 'dotnet build --no-restore --configuration Release'
      }
    }

    stage('Test with Coverage') {
      steps {
        bat '''
          dotnet test "tests\\TransactionsToolkit.Tests\\TransactionsToolkit.Tests.csproj" ^
            --no-build --configuration Release ^
            --collect:"XPlat Code Coverage" ^
            --results-directory "TestResults" ^
            --logger "trx;LogFileName=test_results.trx"
        '''
      }
    }

    stage('Generate Coverage Report') {
      steps {
        bat '''
          dotnet tool install --global dotnet-reportgenerator-globaltool || exit 0
          set PATH=%PATH%;%USERPROFILE%\\.dotnet\\tools
          reportgenerator ^
            -reports:"**\\coverage.cobertura.xml" ^
            -targetdir:"coverage-report" ^
            -reporttypes:HtmlSummary;TextSummary
        '''
      }
    }

    stage('Archive Artifacts') {
      steps {
        junit 'TestResults\\*.trx'
        archiveArtifacts artifacts: 'coverage-report\\**', allowEmptyArchive: true
      }
    }

    stage('Print Coverage Summary') {
      steps {
        bat 'type coverage-report\\Summary.txt || echo No coverage summary found.'
      }
    }
  }

  post {
    success {
      echo '🟢 Build, tests, and coverage succeeded!'
    }
    failure {
      echo '🔴 Build or tests failed—please check the console output.'
    }
  }
}
