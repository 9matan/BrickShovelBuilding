using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace VVOSS.Json
{

	public class VOSJsonTemplates
	{

		//
		// < Dictionary >
		//

		public static void ToJsonObjectFromJobjDictionary<TKey, TValue>(IVOSJsonObject jobj, IDictionary<TKey, TValue> dct)
			where TValue : IVOSJsonSirializable
		{
			IVOSJsonObject jobjitem = null;

			foreach (var item in dct)
			{
				jobjitem = jobj.AddObjectField(item.Key.ToString());
				item.Value.SerializeToJson(jobjitem);
			}
		}

		public static void ToJsonObjectFromDictionary<TKey>(IVOSJsonObject jobj, IDictionary<TKey, string> dct)
		{
			foreach (var item in dct)
			{
				jobj.AddField(item.Key.ToString(), item.Value);
			}
		}

		public static void ToJsonObjectFromDictionary<TKey>(IVOSJsonObject jobj, IDictionary<TKey, int> dct)
		{
			foreach (var item in dct)
			{
				jobj.AddField(item.Key.ToString(), item.Value);
			}
		}

		public static void ToJsonObjectFromDictionary<TKey>(IVOSJsonObject jobj, IDictionary<TKey, float> dct)
		{
			foreach (var item in dct)
			{
				jobj.AddField(item.Key.ToString(), item.Value);
			}
		}

		public static void ToJsonObjectFromDictionary<TKey>(IVOSJsonObject jobj, IDictionary<TKey, bool> dct)
		{
			foreach (var item in dct)
			{
				jobj.AddField(item.Key.ToString(), item.Value);
			}
		}

		public static void ToJsonObjectFromDictionary<TKey>(IVOSJsonObject jobj, IDictionary<TKey, object> dct)
		{
			foreach (var item in dct)
			{
				jobj.AddField(item.Key.ToString(), item.Value.ToString());
			}
		}



		public static void FromJsonObjectToJobjDictionary<TValue>(IVOSJsonObject jobj, IDictionary<string, TValue> dct)
			where TValue : IVOSJsonSirializable, new()
		{
			TValue val = new TValue();
			string key = string.Empty;

			for (int i = 0; i < jobj.count; ++i)
			{
				key = jobj.GetKey(i);

				val = new TValue();
				val.DeserializeFromJson(
					jobj.GetObjectField(key));

				dct.Add(key, val);
			}
		}
				
		public static void FromJsonObjectToDictionary(IVOSJsonObject jobj, IDictionary<string, string> dct)
		{
			string key = string.Empty;

			for (int i = 0; i < jobj.count; ++i)
			{
				key = jobj.GetKey(i);

				dct.Add(key, jobj.GetStringField(key));
			}
		}

		public static void FromJsonObjectToDictionary(IVOSJsonObject jobj, IDictionary<string, int> dct)
		{
			string key = string.Empty;

			for (int i = 0; i < jobj.count; ++i)
			{
				key = jobj.GetKey(i);

				dct.Add(key, jobj.GetIntField(key));
			}
		}

		public static void FromJsonObjectToDictionary(IVOSJsonObject jobj, IDictionary<string, float> dct)
		{
			string key = string.Empty;

			for (int i = 0; i < jobj.count; ++i)
			{
				key = jobj.GetKey(i);

				dct.Add(key, jobj.GetFloatField(key));
			}
		}

		public static void FromJsonObjectToDictionary(IVOSJsonObject jobj, IDictionary<string, bool> dct)
		{
			string key = string.Empty;

			for (int i = 0; i < jobj.count; ++i)
			{
				key = jobj.GetKey(i);

				dct.Add(key, jobj.GetBoolField(key));
			}
		}

		public static void FromJsonObjectToDictionary(IVOSJsonObject jobj, IDictionary<string, object> dct)
		{
			string key = string.Empty;

			for (int i = 0; i < jobj.count; ++i)
			{
				key = jobj.GetKey(i);

				dct.Add(key, jobj.GetStringField(key));
			}
		}


		public static void FromJsonObjectToEnumDictionary<TEnum, TValue>(IVOSJsonObject jobj, IDictionary<TEnum, TValue> dct)
			where TValue : IVOSJsonSirializable, new()
		{
			TValue val = new TValue();
			string key = string.Empty;

			for (int i = 0; i < jobj.count; ++i)
			{
				key = jobj.GetKey(i);

				val = new TValue();
				val.DeserializeFromJson(
					jobj.GetObjectField(key));

				dct.Add(
					(TEnum)System.Enum.Parse(typeof(TEnum), key),
					val);
			}
		}

		//
		// </ Dictionary >
		//

		//
		// < Collection >
		//

		public static void ToJsonArrayFromJobjCollection<TValue>(IVOSJsonArray jarr, ICollection<TValue> coll)
			where TValue : IVOSJsonSirializable
		{
			IVOSJsonObject jobj = null;

			foreach (var item in coll)
			{
				jobj = jarr.AddObjectItem();
				item.SerializeToJson(jobj);
			}
		}

		public static void ToJsonArrayFromCollection<TValue>(IVOSJsonArray jarr, ICollection<TValue> coll)
		{
			foreach (var item in coll)
			{
				jarr.AddItem(item.ToString());
			}
		}



		public static void FromJsonArrayToEnumCollection<TEnum>(IVOSJsonArray jarr, ICollection<TEnum> coll)
		{
			for (int i = 0; i < jarr.count; ++i)
			{
				coll.Add((TEnum)System.Enum.Parse(
					typeof(TEnum),
					jarr.GetStringItem(i)));
			}
		}

		public static void FromJsonArrayToJobjCollection<TValue>(IVOSJsonArray jarr, ICollection<TValue> coll)
			where TValue : IVOSJsonSirializable, new()
		{
			TValue val = new TValue();

			for(int i = 0; i < jarr.count; ++i)
			{
				val = new TValue();
				val.DeserializeFromJson(jarr.GetObjectItem(i));
				coll.Add(val);
			}
		}

		//
		// </ Collection >
		//

		//
		// < Array >
		//

		public static void ToJsonArrayFromJobjArray<TValue>(IVOSJsonArray jarr, TValue[,] arr)
			where TValue : IVOSJsonSirializable
		{
			IVOSJsonArray rw = null;
			IVOSJsonObject jobj = null;

			for (int i = 0; i < arr.GetLength(0); ++i)
			{
				rw = jarr.AddArrayItem();
				for (int j = 0; j < arr.GetLength(1); ++j)
				{
					jobj = rw.AddObjectItem();
					arr[i, j].SerializeToJson(jobj);
				}
			}
		}

		public static void ToJsonArrayFromJobjArray<TValue>(IVOSJsonArray jarr, TValue[] arr)
			where TValue : IVOSJsonSirializable
		{
			IVOSJsonObject jobj = null;

			for (int i = 0; i < arr.Length; ++i)
			{
				jobj = jarr.AddObjectItem();
				arr[i].SerializeToJson(jobj);
			}
		}



		public static void ToJsonArrayFromArray(IVOSJsonArray jarr, string[] arr)
		{
			for (int i = 0; i < arr.Length; ++i)
			{
				jarr.AddItem(arr[i]);
			}
		}

		public static void ToJsonArrayFromArray(IVOSJsonArray jarr, int[] arr)
		{
			for (int i = 0; i < arr.Length; ++i)
			{
				jarr.AddItem(arr[i]);
			}
		}

		public static void ToJsonArrayFromArray(IVOSJsonArray jarr, float[] arr)
		{
			for (int i = 0; i < arr.Length; ++i)
			{
				jarr.AddItem(arr[i]);
			}
		}

		public static void ToJsonArrayFromArray(IVOSJsonArray jarr, bool[] arr)
		{
			for (int i = 0; i < arr.Length; ++i)
			{
				jarr.AddItem(arr[i]);
			}
		}

		public static void ToJsonArrayFromEnumArray<TEnum>(IVOSJsonArray jarr, TEnum[] arr)
		{
			for (int i = 0; i < arr.Length; ++i)
			{
				jarr.AddItem(arr[i].ToString());
			}
		}



		public static void FromJsonArrayToJobjArray<TValue>(IVOSJsonArray jarr, TValue[] arr)
			where TValue : IVOSJsonSirializable, new()
		{
			TValue val = new TValue();
			IVOSJsonObject jobj = null;

			for (int i = 0; i < jarr.count; ++i)
			{
				jobj = jarr.GetObjectItem(i);

				val = new TValue();
				val.DeserializeFromJson(jobj);

				arr[i] = val;
			}

		}

		public static void FromJsonArrayToJobjArray<TValue>(IVOSJsonArray jarr, TValue[,] arr)
			where TValue : IVOSJsonSirializable, new()
		{
			TValue val = new TValue();
			IVOSJsonArray rw = null;
			IVOSJsonObject jobj = null;

			for (int i = 0; i < jarr.count; ++i)
			{
				rw = jarr.GetArrayItem(i);
				for (int j = 0; j < rw.count; ++j)
				{
					jobj = rw.GetObjectItem(j);

					val = new TValue();
					val.DeserializeFromJson(jobj);

					arr[i, j] = val;
				}
			}

		}



		public static void FromJsonArrayToArray(IVOSJsonArray jarr, string[] arr)
		{
			string val = string.Empty;

			for (int i = 0; i < jarr.count; ++i)
			{
				val = jarr.GetStringItem(i);
				arr[i] = val;
			}
		}

		public static void FromJsonArrayToArray(IVOSJsonArray jarr, int[] arr)
		{
			int val = 0;

			for (int i = 0; i < jarr.count; ++i)
			{
				val = jarr.GetIntItem(i);
				arr[i] = val;
			}
		}

		public static void FromJsonArrayToArray(IVOSJsonArray jarr, bool[] arr)
		{
			bool val = false;

			for (int i = 0; i < jarr.count; ++i)
			{
				val = jarr.GetBoolItem(i);
				arr[i] = val;
			}
		}

		public static void FromJsonArrayToArray(IVOSJsonArray jarr, float[] arr)
		{
			float val = 0.0f;

			for (int i = 0; i < jarr.count; ++i)
			{
				val = jarr.GetFloatItem(i);
				arr[i] = val;
			}
		}

		public static void FromJsonArrayToEnumArray<TEnum>(IVOSJsonArray jarr, TEnum[] arr)
		{
			for (int i = 0; i < jarr.count; ++i)
			{
				arr[i] = (TEnum)System.Enum.Parse(
					typeof(TEnum),
					jarr.GetStringItem(i));
			}
		}

		//
		// </ Array >
		//

		public static TValue FromJsonObjectToJobj<TValue>(IVOSJsonObject jobj)
			where TValue : IVOSJsonSirializable, new()
		{
			var val = new TValue();
			val.DeserializeFromJson(jobj);
			return val;
		}

		public static TJson ToJsonObjectFromJobj<TValue, TJson>(TValue obj)
			where TValue : IVOSJsonSirializable
			where TJson : IVOSJsonObject, new()
		{
			var jobj = (new TJson());
			obj.SerializeToJson(jobj);
			return jobj;
		}

	}

}
