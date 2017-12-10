public class BuildVersion
{
    public string Version { get; private set; }
    public string SemVersion { get; private set; }
    public string AssemblyVersion { get; private set; }
    public string CakeVersion { get; private set; }

    public static BuildVersion Calculate(ICakeContext context)
    {
        if (context.BuildSystem().AppVeyor.IsRunningOnAppVeyor)
        {
            return new BuildVersion
            {
                Version = context.EnvironmentVariable("GitVersion_MajorMinorPatch"),
                SemVersion = context.EnvironmentVariable("GitVersion_SemVer"),
                AssemblyVersion = context.EnvironmentVariable("GitVersion_AssemblySemVer"),
                CakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString()
            };
        }

        var gitVersion = context.GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json,
            UpdateAssemblyInfo = false
        });

        return new BuildVersion
        {
            Version = gitVersion.MajorMinorPatch,
            SemVersion = gitVersion.SemVer,
            AssemblyVersion = gitVersion.AssemblySemVer,
            CakeVersion = typeof(ICakeContext).Assembly.GetName().Version.ToString()
        };
    }
}