stage('Restore & Build') {
  steps {
    bat 'dotnet restore'
    bat 'dotnet build --no-restore --configuration Release'
  }
}

stage('Test') {
  steps {
    // Run tests and emit TRX
    bat '''
      dotnet test "tests\\TransactionsToolkit.Tests\\TransactionsToolkit.Tests.csproj" ^
        --no-build --configuration Release ^
        --logger "trx;LogFileName=test_results.trx"
    '''
  }
  post {
    always {
      // Publish TRX via MSTest plugin
      step([$class: 'MSTestPublisher', testResultsFile: '**\\test_results.trx'])
    }
  }
}