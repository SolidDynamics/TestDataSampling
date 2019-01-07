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

		public string EntityName { get; set; }

		public Dictionary<RecordGroup, List<Guid>> RecordGroups { get; set; }
	}
}
