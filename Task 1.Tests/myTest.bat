3d-party\opencover.4.6.519\OpenCover.Console.exe -register:user -filter:"+[*]DomainModel*" -target:"..\packages\NUnit.ConsoleRunner.3.7.0\tools\nunit3-console.exe" -targetargs:"\"bin\Debug\Task 1.Tests.dll\" --result:test-results.xml;format=nunit3" -output:"coverage-results.xml"

3d-party\ReportGenerator_2.5.5\ReportGenerator.exe "-reports:coverage-results.xml" "-targetdir:.\coverage"