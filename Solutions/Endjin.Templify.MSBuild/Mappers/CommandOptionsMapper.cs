namespace Endjin.Templify.MSBuild.Mappers
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Linq;

    using Endjin.Templify.Domain.Infrastructure;
    using Endjin.Templify.MSBuild.Contracts.Mappers;

    using Microsoft.Build.Framework;

    [Export(typeof(ICommandOptionsMapper))]
    public class CommandOptionsMapper : ICommandOptionsMapper
    {
        public CommandOptions MapFrom(CreatePackage createPackage)
        {
            return new CommandOptions
            {
                Author = createPackage.Author,
                Mode = Mode.Create,
                Name = createPackage.Name,
                PackageRepositoryPath = new DirectoryInfo(createPackage.PackageRepositoryPath).FullName,
                PackageRepositoryWorkingPath = new DirectoryInfo(createPackage.PackageRepositoryWorkingPath).FullName,
                Path = new DirectoryInfo(createPackage.Path).FullName,
                RawTokens = BuildRawTokenDictionaryFrom(createPackage.Tokens),
                Version = createPackage.Version,
            };
        }

        private static string[] BuildRawTokenDictionaryFrom(IEnumerable<ITaskItem> taskItems)
        {
            return taskItems.Select(x => x.ItemSpec).ToArray();
        }
    }
}