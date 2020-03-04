using Nuke.Common.IO;

namespace Build.Settings
{
    public class GlobalSettings
    {
        public GlobalSettings(AbsolutePath rootDirectory)
        {
            SourceDirectory = rootDirectory / "src";
            TestDirectory = rootDirectory / "test";
            BuildOutputDirectory = rootDirectory / "buildOutput";

            PackageDirectory = BuildOutputDirectory / "package";
            PublishDirectory = BuildOutputDirectory / "publish";
        }

        public AbsolutePath BuildOutputDirectory { get; }

        public AbsolutePath PackageDirectory { get; }

        public AbsolutePath PublishDirectory { get; }

        public AbsolutePath SourceDirectory { get; }

        public AbsolutePath TestDirectory { get; }
    }
}
