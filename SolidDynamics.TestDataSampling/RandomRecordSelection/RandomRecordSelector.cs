using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidDynamics.TestDataSampling.RandomRecordSelection
{
	public class RandomRecordSelector
	{
		public decimal ProportionToSelect { get; set; }

		public Dictionary<Type, Func<object, string>> CustomStringConversions { get; set; } = new Dictionary<Type, Func<object, string>>();

		public RandomRecordSelector(decimal proportionToSelect)
		{
			if (proportionToSelect <= 0 || proportionToSelect > 1)
				throw new ArgumentOutOfRangeException("proportionToSelect","Must be a decimal greater than 0 and less than 1");
			ProportionToSelect = proportionToSelect;
		}

		public IEnumerable<RandomRecordSelectionResponse> Execute(params RandomRecordSelectionRequest[] randomRecordSelectionRequests)
		{
			foreach(var randomRecordSelectionRequest in randomRecordSelectionRequests)
			{
				yield return SelectRandomRecords(randomRecordSelectionRequest);
			}
		}

		private RandomRecordSelectionResponse SelectRandomRecords(RandomRecordSelectionRequest randomRecordSelectionRequest)
		{
			var groupedRecords = GroupRecords(randomRecordSelectionRequest);

			var selectedRecordGroups = new Dictionary<RecordGroup, List<Guid>>();

			foreach (var group in groupedRecords)
			{
				var groupCount = group.Value.Count;
				var selectedRecordCount = (int)Math.Floor(groupCount * (double)ProportionToSelect);
				var unselectedRecordCount = groupCount - selectedRecordCount;

				var selectedRecords = group.Value.OrderBy(x => Guid.NewGuid()).Take(selectedRecordCount).ToList();

				selectedRecordGroups.Add(group.Key, selectedRecords);
			}

			return new RandomRecordSelectionResponse(randomRecordSelectionRequest.EntityName)
			{
				RecordGroups = selectedRecordGroups
			};
		}

		private Dictionary<RecordGroup, List<Guid>> GroupRecords(RandomRecordSelectionRequest randomRecordSelectionRequest)
		{
			var groupedRecords = new Dictionary<RecordGroup, List<Guid>>();

			foreach (var record in randomRecordSelectionRequest.Records)
			{
				var groupIdentifier = new Dictionary<string, object>();
				foreach (var field in randomRecordSelectionRequest.FieldsToGroupBy)
				{
					groupIdentifier.Add(field, GetAttributeValueAsString(record, field));
				}
				var recordGroup = new RecordGroup(groupIdentifier);

				if (!groupedRecords.ContainsKey(recordGroup))
				{
					groupedRecords.Add(recordGroup, new List<Guid>());
				}

				groupedRecords[recordGroup].Add((Guid)record[randomRecordSelectionRequest.PrimaryKey]);
			}

			return groupedRecords;
		}

		private string GetAttributeValueAsString(IDictionary<string, object> attributes, string fieldName)
		{
			if (!attributes.ContainsKey(fieldName))
				return string.Empty;

			object fieldValue = attributes[fieldName];
			Type fieldType = fieldValue.GetType();

			if (CustomStringConversions.ContainsKey(fieldType))
			{
				var customFunction = CustomStringConversions[fieldType];
				return customFunction(fieldValue);
			}

			return fieldValue.ToString();
		}
	}
}

