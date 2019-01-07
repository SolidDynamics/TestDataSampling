using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace SolidDynamics.TestDataSampling.RandomRecordSelection
{
	[TypeConverter(typeof(RecordGroupTypeConverter))]
	public class RecordGroup : IEquatable<RecordGroup>
	{
		public RecordGroup(Dictionary<string, object> fieldValues)
		{
			FieldValues = fieldValues;
		}

		public Dictionary<string, object> FieldValues { get; set; }

		public override string ToString()
		{
			return JsonConvert.SerializeObject(FieldValues);
		}

		public bool Equals(RecordGroup other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return FieldValues.SequenceEqual(other.FieldValues);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return FieldValues.SequenceEqual(((RecordGroup)obj).FieldValues);
		}

		public override int GetHashCode()
		{
			unchecked // Overflow is fine, just wrap
			{
				var hash = 17;
				foreach (var fieldValue in FieldValues)
				{
					hash = hash * 23 + fieldValue.GetHashCode();
				}

				return hash;
			}
		}

		public static bool operator ==(RecordGroup left, RecordGroup right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(RecordGroup left, RecordGroup right)
		{
			return !Equals(left, right);
		}
	}

	public class RecordGroupTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			if (value is string)
			{
				var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(value.ToString());
				return new RecordGroup(dictionary);
			}
			return base.ConvertFrom(context, culture, value);
		}
	}
}
