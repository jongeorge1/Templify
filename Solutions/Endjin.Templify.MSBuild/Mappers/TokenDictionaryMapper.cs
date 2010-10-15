namespace Endjin.Templify.MSBuild.Mappers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Endjin.Templify.MSBuild.Contracts.Mappers;

	using Microsoft.Build.Framework;

	[Exports(typeof(ITokenDictionaryMapper))]
	public class TokenDictionaryMapper : ITokenDictionaryMapper
	{
		public Dictionary<string, string> MapFrom(IEnumerable<ITaskItem> taskItems)
		{
			return taskItems.Select(MapFrom).ToDictionary(x => x.Key, x => x.Value);
		}

		private static KeyValuePair<string, string> MapFrom(ITaskItem taskItem)
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

	}
}