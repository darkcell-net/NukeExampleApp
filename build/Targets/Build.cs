using Build.Settings;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;
using System.IO;
using System.IO.Compression;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

namespace Build.Targets
{
    public partial class Build : NukeBuild
    {
        private string _buildVersion;
        private GlobalSettings _globalSettings;

        public Target Clean => _ => _
            .Description("Remove previous build output")
            .Executes(() => EnsureCleanDirectory(Settings.BuildOutputDirectory));

        public Target Compile => _ => _
            .DependsOn(Clean)
            .Description("Build all projects in the solution")
            .Executes(() => DotNetBuild(settings => settings.SetProjectFile(Solution.Path)));

        public Target Package => _ => _
            .Description("Package the application")
            .DependsOn(Test)
            .Executes(() =>
            {
                DotNetPublish(
                    settings => settings
                        .SetConfiguration("Release")
                        .SetRuntime("linux-x64")
                        .SetProperty("Version", GetBuildVersion())
                        .SetOutput(Settings.PublishDirectory)
                        .SetProject(Settings.SourceDirectory / "NukeExampleApp"));

                string packagePath = Settings.PackageDirectory / $"NukeExampleApp_{GetBuildVersion()}.zip";

                Directory.CreateDirectory(Settings.PackageDirectory);
                ZipFile.CreateFromDirectory(Settings.PublishDirectory, packagePath);

                using (FileStream zipStream = File.Open(packagePath, FileMode.Open))
                {
                    using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Update))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            entry.ExternalAttributes = (entry.Name == "NukeExampleApp" ? 0x81e4 : 0x81a4) << 16;
                        }
                    }
                }
            });

        public Target Test => _ => _
            .Description("Perform all unit tests")
            .DependsOn(Compile)
            .Executes(() => DotNetTest(
                settings => settings
                .SetProjectFile(Settings.TestDirectory / "Tests")
                .EnableNoBuild()));

        private GlobalSettings Settings => _globalSettings = _globalSettings ?? new GlobalSettings(RootDirectory);

        [Solution("NukeExampleApp.sln")]
        private Solution Solution { get; }

        public static int Main() => Execute<Build>(x => x.Package);

        private string GetBuildVersion()
        {
            // Normally we would check with GIT to get a commit count, but hardcode for now.
            if (_buildVersion != null) { return _buildVersion; }

            return _buildVersion = $"0.0.1";
        }
    }
}