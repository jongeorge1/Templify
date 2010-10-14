namespace Endjin.Templify.MSBuild
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    using Endjin.Templify.Domain.Contracts.Tasks;
    using Endjin.Templify.Domain.Domain.Packages;
    using Endjin.Templify.Domain.Framework;
    using Endjin.Templify.Domain.Framework.Container;
    using Endjin.Templify.Domain.Infrastructure;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class CreatePackage : Task
    {
        public CreatePackage()
        {
            MefContainer.Compose(this);

            this.PackageCreatorTasks.Progress += this.OnPackageCreatorTasksProgress;
        }

        protected string Author { get; set; }

        protected string Name { get; set; }

        protected string PackageName { get; set; }

        protected string PackageRepositoryPath { get; set; }

        protected string Path { get; set; }

        protected string RawMode { get; set; }

        protected string[] RawTokens { get; set; }

        protected Dictionary<string, string> Tokens { get; set; }

        protected string Version { get; set; }

        [Import]
        private IPackageCreatorTasks PackageCreatorTasks { get; set; }

        public override bool Execute()
        {
            var options = this.BuildOptions();

            try
            {
                this.PackageCreatorTasks.CreatePackage(options);
            }
            catch (Exception exception)
            {
                this.Log.LogErrorFromException(exception, true, true, null);
                return false;
            }

            return true;
        }

        private CommandOptions BuildOptions()
        {
            return new CommandOptions
                {
                    Author = this.Author,
                    Mode = Mode.Create,
                    Name = this.Name,
                    PackageName = this.PackageName,
                    PackageRepositoryPath = this.PackageRepositoryPath,
                    Path = this.Path,
                    Tokens = this.Tokens,
                    RawMode = this.RawMode,
                    RawTokens = this.RawTokens,
                    Version = this.Version,
                };
        }

        private void OnPackageCreatorTasksProgress(object sender, PackageProgressEventArgs e)
        {
            this.Log.LogMessage(MessageImportance.Normal, e.ProgressStage.GetDescription());
        }
    }
}