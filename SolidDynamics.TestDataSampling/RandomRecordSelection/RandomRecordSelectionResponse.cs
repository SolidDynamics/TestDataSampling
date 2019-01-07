using System;
using System.Collections.Generic;

namespace SolidDynamics.TestDataSampling.RandomRecordSelection
{

	public class RandomRecordSelectionResponse
	{
		public RandomRecordSelectionResponse()
		{
		}

		public RandomRecordSelectionResponse(string entityName)
		{
			EntityName = entityName;
		}

		public string EntityName { get; }

		public Dictionary<RecordGroup, List<Guid>> RecordGroups { get; internal set; }
	}
}
