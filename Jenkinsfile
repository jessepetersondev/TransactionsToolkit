// Restore dependencies and build the solution
stage('Restore & Build') {
    bat 'dotnet restore'
    bat 'dotnet build --no-restore --configuration Release'
}

// Run tests and publish results
stage('Test') {
    // Execute xUnit tests, output TRX
    bat '''
        dotnet test "tests\\TransactionsToolkit.Tests\\TransactionsToolkit.Tests.csproj" ^
            --no-build --configuration Release ^
            --logger "trx;LogFileName=test_results.trx"
    '''

    // Publish the TRX to Jenkins via MSTest publisher (plugin required)
    step([$class: 'MSTestPublisher', testResultsFile: '**\\test_results.trx'])
}