namespace Endjin.Templify.MSBuild.Contracts.Mappers
{
	using System.Collections.Generic;

	using Microsoft.Build.Framework;

	public interface ITokenDictionaryMapper
	{
		Dictionary<string, string> MapFrom(IEnumerable<ITaskItem> taskItems);
	}
}