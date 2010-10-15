namespace Endjin.Templify.MSBuild.Mappers
{
    using System.Collections.Generic;
    using System.Linq;

    using Endjin.Templify.Domain.Infrastructure;
    using Endjin.Templify.MSBuild.Contracts.Mappers;

    using Microsoft.Build.Framework;

    public class CommandOptionsMapper : ICommandOptionsMapper
    {
        public CommandOptions MapFrom(CreatePackage createPackage)
        {
            return new CommandOptions
            {
                Author = createPackage.Author,
                Mode = Mode.Create,
                Name = createPackage.Name,
                PackageName = createPackage.PackageName,
                PackageRepositoryPath = createPackage.PackageRepositoryPath,
                Path = createPackage.Path,
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