namespace Endjin.Templify.MSBuild
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;

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

        public CreatePackage(IPackageCreatorTasks packageCreatorTasks)
        {
            this.PackageCreatorTasks = packageCreatorTasks;
            this.PackageCreatorTasks.Progress += this.OnPackageCreatorTasksProgress;
        }
        
        protected string Author { get; set; }

        protected string Name { get; set; }

        protected string PackageName { get; set; }

        protected string PackageRepositoryPath { get; set; }

        protected string Path { get; set; }

        protected ITaskItem[] Tokens { get; set; }

        protected string Version { get; set; }

        [Import]
        private IPackageCreatorTasks PackageCreatorTasks { get; set; }

        public override bool Execute()
        {
            try
            {
                var options = this.BuildOptions();
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
                    Tokens = BuildTokenDictionaryFrom(this.Tokens),
                    Version = this.Version,
                };
        }

        private static Dictionary<string, string> BuildTokenDictionaryFrom(IEnumerable<ITaskItem> taskItems)
        {
            return taskItems.Select(BuildTokenFrom).ToDictionary(x => x.Key, x => x.Value);
        }

        private static KeyValuePair<string, string> BuildTokenFrom(ITaskItem taskItem)
        {
            var itemSpec = taskItem.ItemSpec;
            var key = taskItem.GetMetadata("value");
            var val = taskItem.GetMetadata("token");

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception(string.Format("Token {0} has no value element.", itemSpec));
            }

            if (string.IsNullOrEmpty(val))
            {
                throw new Exception(string.Format("Token {0} has no token element.", itemSpec));
            }

            return new KeyValuePair<string, string>(key, val);
        }

        private void OnPackageCreatorTasksProgress(object sender, PackageProgressEventArgs e)
        {
            this.Log.LogMessage(MessageImportance.Normal, e.ProgressStage.GetDescription());
        }
    }
}