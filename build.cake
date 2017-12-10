#tool "GitVersion.CommandLine"
#tool "WiX.Toolset"
#load "./build/version.cake"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory("build") + Directory(configuration);
var msiDir = "./build/Release.msi";

BuildVersion version;

Setup(context =>
{
    version = BuildVersion.Calculate(Context);
    Information("   .-='''''''''''=-.");
    Information("   | . . . . . . . |");
    Information("   | .'.'.'.'.'.'. |");
    Information("  ()_______________()");
    Information("  ||_____COUCH_____||");
    Information("  ||____EXPLORER___||");
    Information("   W               W");
    Information("Version:      " + version.Version);
    Information("SemVersion:   " + version.SemVersion);
    Information("Assembly:     " + version.AssemblyVersion);
    Information("Cake Version: " + version.CakeVersion);
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore("./src/CouchExplorer.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild("./src/CouchExplorer.sln", settings =>
    settings.SetConfiguration(configuration));
});

Task("Msi")
    .IsDependentOn("Build")
    .Does(() =>
{
    WiXCandle(GetFiles("./*.wxs"), new CandleSettings
    {
        Defines = new Dictionary<string, string>
        {
            { "Version", version.AssemblyVersion },
            { "Configuration", Context.Argument("configuration", "Release") }
        },
        Extensions = new List<string>
        {
            "WixUtilExtension"
        },
        OutputDirectory = msiDir
    });

    var objectFiles = GetFiles(msiDir + "/*.wixobj");

    foreach (var objectFile in objectFiles)
    {
        WiXLight(msiDir + "/" + objectFile.GetFilename(), new LightSettings
        {   
            Extensions = new List<string>
            {
                "WixUtilExtension"
            },
            OutputFile = msiDir + "/" +
                objectFile.GetFilenameWithoutExtension() +  version.SemVersion + ".msi"
        });
    }
});

Task("Arrange-Artifacts")
    .Does(() =>
{
    var artifactsDir = "./build/pkg/CouchExplorer-" + version.SemVersion;

    Information("Copying build artifacts for " + version.SemVersion + " to " + artifactsDir);

    EnsureDirectoryExists(artifactsDir);
    CopyFiles(msiDir + "/*.msi", artifactsDir);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Build");

Task("Full")
    .IsDependentOn("Msi")
    .IsDependentOn("Arrange-Artifacts");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
