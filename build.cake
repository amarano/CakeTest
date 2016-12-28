#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=SpecFlow"
#addin "Cake.Npm"

var target = Argument("target", "Default");
var solution = File("./CakeTest.sln");
var configuration = Argument("configuration", "Debug");
var outputDirectory = string.Format("./CakeTest/bin/{0}/", configuration);
var program = File(System.IO.Path.Combine(outputDirectory, "CakeTest.exe").ToString());

Task("Default")
    .IsDependentOn("Test");

Task("CompileTs")
    .Does(() => {
        Npm.FromPath("./CakeTest/wwwroot").Install().RunScript("tsc");
    });

Task("CopyStaticFiles")
    .Does(() => {
        var wwwroot = System.IO.Path.Combine(outputDirectory, "wwwroot");
        if (!System.IO.Directory.Exists(wwwroot)){
            System.IO.Directory.CreateDirectory(wwwroot);
        }
        CopyFiles(GetFiles("./CakeTest/wwwroot/site/**/*.*"), wwwroot);
    });

Task("Run")
    .IsDependentOn("Test")
    .IsDependentOn("CompileTs")
    .IsDependentOn("CopyStaticFiles")
    .Finally(() => {
        Information("Running process at {0}", program);
        StartProcess(program);
    });

Task("Build")
  .Does(() =>
{
	Information("Building...");
	NuGetRestore(solution);
	DotNetBuild(solution, config => {
	    config.SetConfiguration(configuration);
	});
});

Task("Test")
    .IsDependentOn("Rebuild")
	.Does(() => {
		NUnit3("./**/bin/debug/*.Tests.dll");
		SpecFlowStepDefinitionReport("./CakeTest.Tests/Features");
	});

Task("Rebuild")
    .IsDependentOn("Clean")
    .IsDependentOn("Build");

Task("Clean")
  .Does(() =>
  {
    CleanDirectories(string.Format("./**/obj/{0}",
      configuration));
    CleanDirectories(string.Format("./**/bin/{0}",
      configuration));
  });

RunTarget(target);