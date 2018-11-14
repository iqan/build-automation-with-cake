///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

//////////////////////////////////////////////////////////////////////
// CONFIGURATION
//////////////////////////////////////////////////////////////////////
 
var projectName = "CakeDemo.Lib";
var solutionFile = "./src/CakeDemo.sln";
 
// Publish projects
var publishProjects = new List<string>
{
	 { @"src\" + projectName + @"\" + projectName + ".csproj" },
};

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////


///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("./publish");
    CleanDirectories("./src/**/bin");
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/CakeDemo.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
	DotNetCoreBuild(solutionFile, new DotNetCoreBuildSettings { Configuration = configuration });
});
 
Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .DoesForEach(() => GetFiles("./src/**/*.*Tests.csproj"), (unitTestProject) =>
{ 
    DotNetCoreTest(
        unitTestProject.FullPath,
        new DotNetCoreTestSettings
        {
            Configuration = configuration,
            NoBuild = true
        });
});
 
Task("Publish")
    .IsDependentOn("Run-Unit-Tests")
    .Does(() =>
{
    var settings = new DotNetCorePublishSettings {
        Configuration = configuration,
        OutputDirectory = "./publish/"
    };
	 
    foreach(var project in publishProjects)
    {
		DotNetCorePublish(project, settings);
    }
});

Task("Default")
    .IsDependentOn("Publish");


RunTarget(target);