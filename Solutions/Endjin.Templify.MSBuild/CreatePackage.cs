namespace Endjin.Templify.MSBuild
{
    using System;
    using System.ComponentModel.Composition;

    using Endjin.Templify.Domain.Contracts.Tasks;
    using Endjin.Templify.Domain.Domain.Packages;
    using Endjin.Templify.Domain.Framework;
    using Endjin.Templify.Domain.Framework.Container;
    using Endjin.Templify.MSBuild.Contracts.Mappers;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    public class CreatePackage : Task
    {
        public CreatePackage()
        {
            MefContainer.Compose(this);

            this.PackageCreatorTasks.Progress += this.OnPackageCreatorTasksProgress;
        }

        public CreatePackage(
            IPackageCreatorTasks packageCreatorTasks,
            ICommandOptionsMapper commandOptionsMapper)
        {
            this.PackageCreatorTasks = packageCreatorTasks;
            this.CommandOptionsMapper = commandOptionsMapper;
            this.PackageCreatorTasks.Progress += this.OnPackageCreatorTasksProgress;
        }

        public string Author { get; set; }

        public string Name { get; set; }

        public string PackageName { get; set; }

        public string PackageRepositoryPath { get; set; }

        public string Path { get; set; }

        public ITaskItem[] Tokens { get; set; }

        public string Version { get; set; }

        [Import]
        private IPackageCreatorTasks PackageCreatorTasks { get; set; }

        [Import]
        private ICommandOptionsMapper CommandOptionsMapper { get; set; }

        public override bool Execute()
        {
            try
            {
                var options = this.CommandOptionsMapper.MapFrom(this);
                this.PackageCreatorTasks.CreatePackage(options);
            }
            catch (Exception exception)
            {
                this.Log.LogErrorFromException(exception, true, true, null);
                return false;
            }

            return true;
        }

        private void OnPackageCreatorTasksProgress(object sender, PackageProgressEventArgs e)
        {
            const string ProgressMessageFormat = "Package creation progress: {0} {1}/{2}";

            this.Log.LogMessage(
                MessageImportance.Normal, 
                ProgressMessageFormat, 
                e.ProgressStage.GetDescription(),
                e.CurrentValue,
                e.MaxValue);
        }
    }
}