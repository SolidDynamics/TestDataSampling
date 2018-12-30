using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolidDynamics.TestDataSampling.RandomRecordSelection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SolidDynamics.TestDataSampling.Tests
{
	[TestClass]
	public class RandomRecordSelectorTests
	{
		readonly List<Dictionary<string, object>> sampleData = new List<Dictionary<string, object>>()
			{
				{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("c0fb927d-53bf-43fc-83fa-d89eb5b4b193") },
						{ "name", "apple" },
						{ "type", "fruit" },
						{ "price", new PlantPrice() { Amount = 10, Unit = "kg" } }
					}
				},
					{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("dfe1018b-9dac-4ba0-8cad-535fa8b2581d") },
						{ "name", "broccoli" },
						{ "type", "vegetable" },
						{ "price", new PlantPrice() { Amount = 10, Unit = "kg" } }
					}
				},
						{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("d77ca0d1-5c1e-4dc5-b292-58c577a8f78f") },
						{ "name", "carrot" },
						{ "type", "vegetable" },
						{ "price", new PlantPrice() { Amount = 10, Unit = "lb" } }
					}
				},
						{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("5877ee44-663d-4b22-bc77-8bfd93c0cd95") },
						{ "name", "banana" },
						{ "type", "fruit" },
						{ "price", new PlantPrice() { Amount = 10, Unit = "lb" } }
					}
				},
						{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("842e99de-8e18-4090-a5c4-f8816f1d229b") },
						{ "name", "pear" },
						{ "type", "fruit" },
						{ "price", new PlantPrice() { Amount = 15, Unit = "kg" } }
					}
				},
						{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("cecf7973-b1f3-4c54-9fc9-05ab436d65a7") },
						{ "name", "potato" },
						{ "type", "vegetable" },
						{ "price", new PlantPrice() { Amount = 15, Unit = "kg" } }
					}
				},
						{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("706a255f-af3a-4abc-8d11-eb8eca8f70bf") },
						{ "name", "orange" },
						{ "type", "fruit" },
						{ "price", new PlantPrice() { Amount = 25, Unit = "kg" } }
					}
				},
						{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("d0007394-76ee-4e1a-8f49-37f7d5ffb1a3") },
						{ "name", "pineapple" },
						{ "type", "fruit" },
						{ "price", new PlantPrice() { Amount = 25, Unit = "kg" } }
					}
				},
						{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("d28a140f-392b-418f-903f-9e6d4a730513") },
						{ "name", "grape" },
						{ "type", "fruit" },
						{ "price", new PlantPrice() { Amount = 25, Unit = "kg" } }
					}
				},
						{
					new Dictionary<string, object>()
					{
						{ "id",new Guid("ff8b74d0-469d-4084-ab65-389f9e14522f") },
						{ "name", "kale" },
						{ "type", "vegetable" },
						{ "price", new PlantPrice() { Amount = 25, Unit = "kg" } }
					}
				}
			};

		[TestMethod]
		public void NonGroupedReturnsCorrectNumberOfRecords()
		{
			var request = new RandomRecordSelectionRequest("plants", "id");
			request.Records = sampleData;
			RandomRecordSelector randomRecordSelector = new RandomRecordSelector(0.35m);
			var response = randomRecordSelector.Execute(request);

			Assert.AreEqual(1, response.Count());

			var responseEntity = response.First();

			Assert.AreEqual("plants", responseEntity.EntityName);
			Assert.AreEqual(1, responseEntity.SelectedRecords.Count);
			Assert.AreEqual(3, responseEntity.SelectedRecords.First().Value.Count);
		}

		[TestMethod]
		public void GroupedReturnsCorrectNumberOfRecords()
		{
			var request = new RandomRecordSelectionRequest("plants", "id", new List<string>() { "type" });
			request.Records = sampleData;
			RandomRecordSelector randomRecordSelector = new RandomRecordSelector(0.5m);
			var response = randomRecordSelector.Execute(request);

			Assert.AreEqual(1, response.Count());

			var responseEntity = response.First();

			Assert.AreEqual("plants", responseEntity.EntityName);
			Assert.AreEqual(2, responseEntity.SelectedRecords.Count);
			Assert.AreEqual(3, responseEntity.SelectedRecords.Single(x => x.Key.FieldValues.Single().Value == "fruit").Value.Count);
			Assert.AreEqual(2, responseEntity.SelectedRecords.Single(x => x.Key.FieldValues.Single().Value == "vegetable").Value.Count);
		}

		[TestMethod]
		public void GroupByComplexObjectResolvesCorrectly()
		{
			var request = new RandomRecordSelectionRequest("plants", "id", new List<string>() { "price" });
			request.Records = sampleData;
			RandomRecordSelector randomRecordSelector = new RandomRecordSelector(1m);
			var response = randomRecordSelector.Execute(request);

			Assert.AreEqual(1, response.Count());

			var responseEntity = response.First();

			Assert.AreEqual("plants", responseEntity.EntityName);
			Assert.AreEqual(4, responseEntity.SelectedRecords.Count);
			var TwentyFivePerKiloGroup = responseEntity.SelectedRecords.Single(x => ((PlantPrice)x.Key.FieldValues.Single().Value).Amount == 25 && ((PlantPrice)x.Key.FieldValues.Single().Value).Unit == "kg");

			Assert.AreEqual(4, TwentyFivePerKiloGroup.Value.Count);
		}
	}

	internal sealed class PlantPrice
	{
		public decimal Amount { get; set; }
		public string Unit { get; set; }
	}
}
