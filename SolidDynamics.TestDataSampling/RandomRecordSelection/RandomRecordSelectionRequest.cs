using System.Collections.Generic;

namespace SolidDynamics.TestDataSampling.RandomRecordSelection
{
	public class RandomRecordSelectionRequest
	{

		public RandomRecordSelectionRequest(string entityName, string primaryKey) 
			: this(entityName, primaryKey, new List<string>())
		{ }

		public RandomRecordSelectionRequest(string entityName, string primaryKey, List<string> fieldsToGroupBy)
		{
			EntityName = entityName;
			FieldsToGroupBy = fieldsToGroupBy;
			PrimaryKey = primaryKey;
		}

		public string EntityName { get; set; }

		public string PrimaryKey { get; set; }

		public IList<string> FieldsToGroupBy { get; set; }

		public IList<Dictionary<string, object>> Records { get; set; }
	}
}
