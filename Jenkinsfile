    stage('Test') {
      steps {
        bat """
          dotnet test "tests\\TransactionsToolkit.Tests\\TransactionsToolkit.Tests.csproj" ^
            --no-build --configuration Release ^
            --logger "trx;LogFileName=test_results.trx"
        """
      }
      post {
        always {
          // Publish the TRX via MSTest publisher
          step([$class: 'MSTestPublisher', testResultsFile: '**\\test_results.trx'])
        }
      }
    }
