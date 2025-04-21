stage('Restore') {
  steps {
    bat 'dotnet restore'  // restores NuGet packages ([learn.microsoft.com](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-test?utm_source=chatgpt.com))
  }
}

stage('Build') {
  steps {
    bat 'dotnet build --configuration Release --no-restore'  // builds solution ([medium.com](https://medium.com/southworks/creating-a-jenkins-pipeline-for-a-net-core-application-937a2165b073?utm_source=chatgpt.com), [c-sharpcorner.com](https://www.c-sharpcorner.com/article/cicd-pipeline-using-jenkins-and-github-for-net-core-web-application/?utm_source=chatgpt.com))
  }
}

stage('Test') {
  steps {
    // Run xUnit tests and generate TRX results
    bat 'dotnet test "tests\\TransactionsToolkit.Tests\\TransactionsToolkit.Tests.csproj" --configuration Release --no-build --logger "trx;LogFileName=test_results.trx"'  
    // runs tests yielding a TRX report ([stackoverflow.com](https://stackoverflow.com/questions/43284881/jenkins-integration-for-dotnet-test?utm_source=chatgpt.com))
  }
  post {
    always {
      // Publish test results via xUnit plugin (MSTest format) ([plugins.jenkins.io](https://plugins.jenkins.io/xunit?utm_source=chatgpt.com), [plugins.jenkins.io](https://plugins.jenkins.io/mstest?utm_source=chatgpt.com))
      xunit(
        tools: [
          MSTest(pattern: '**/test_results.trx')
        ]
      )
    }
  }
}