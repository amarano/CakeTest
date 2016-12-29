#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=SpecFlow"
#addin "Cake.Npm"

var target = Argument("target", "Default");
var solution = File("./CakeTest.sln");
var configuration = Argument("configuration", "Debug");
var outputDirectory = string.Format("./CakeTest/bin/{0}/", configuration);
var program = File(System.IO.Path.Combine(outputDirectory, "CakeTest.exe").ToString());

Func<IFileSystemInfo, bool> exclude_node_modules =
    fileSystemInfo => {
        return !fileSystemInfo.Path.FullPath.Contains("node_modules");
        };

Task("Default")
    .IsDependentOn("Test");

Task("CompileTs")
    .Does(() => {
        Npm.FromPath("./CakeTest/wwwroot/caketest").Install().RunScript("build");
    });

Task("CopyStaticFiles")
    .Does(() => {
        var wwwroot = System.IO.Path.Combine(outputDirectory, "wwwroot");
        if (!System.IO.Directory.Exists(wwwroot)){
            System.IO.Directory.CreateDirectory(wwwroot);
        }
        CopyFiles(GetFiles("./CakeTest/wwwroot/caketest/dist/*.*"), wwwroot, true);
    });

Task("CopyNodeModules")
    .Does(() => {
        var nodeModulesPath = System.IO.Path.Combine(outputDirectory, "wwwroot", "node_modules");
        if (!System.IO.Directory.Exists(nodeModulesPath)){
            System.IO.Directory.CreateDirectory(nodeModulesPath);
        }
        CopyFiles(GetFiles("./CakeTest/wwwroot/node_modules/**/*.*"), nodeModulesPath, true);
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

    CleanDirectory(string.Format("./CakeTest/bin/{0}/wwwroot", configuration), exclude_node_modules);
    CleanDirectory(string.Format("./CakeTest/bin/{0}", configuration), exclude_node_modules);
  });

RunTarget(target);