pipeline {
  agent any

  environment {
    // Inject your NuGet API key from Jenkins credentials
    NUGET_API_KEY = credentials('NUGET_API_KEY')
    // The feed URL you want to push to (nuget.org or your private feed)
    NUGET_FEED    = 'https://api.nuget.org/v3/index.json'
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Restore & Build') {
      steps {
        sh 'dotnet --info'
        sh 'dotnet restore'
        sh 'dotnet build --no-restore --configuration Release'
      }
    }

    stage('Test') {
      steps {
        // Run tests and output a TRX file
        sh '''
          dotnet test tests/TransactionsToolkit.Tests/ \
            --no-build --configuration Release \
            --logger "trx;LogFileName=test_results.trx"
        '''
      }
      post {
        always {
          // Publish the TRX test report
          mstest testResultsFile: '**/test_results.trx'
        }
      }
    }

    stage('Pack') {
      steps {
        sh 'dotnet pack src/TransactionsToolkit/TransactionsToolkit.csproj --no-build -c Release'
      }
    }

    stage('Publish Package') {
      steps {
        sh """
          dotnet nuget push \
            src/TransactionsToolkit/bin/Release/net8.0/TransactionsToolkit.*.nupkg \
            --api-key $NUGET_API_KEY \
            --source $NUGET_FEED \
            --skip-duplicate
        """
      }
    }
  }

  post {
    success {
      echo '🟢 Build, test, and deploy succeeded!'
    }
    failure {
      echo '🔴 Something failed. Check the console output.'
    }
  }
}
