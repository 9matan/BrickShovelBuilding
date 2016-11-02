using UnityEngine;
using System.Collections;
using System;

public static class VOSPlayerPrefs
{

	public interface IVOSPlayerPrefSaver
	{
		void SavePlayerPref();
		void LoadPlayerPref();
	}

	public class Item<TValue>
	{

		public virtual string prefKey { get; set; }
		public virtual TValue prefValue { get; set; }

		public static Item<TValue> Create(string __prefKey, TValue __prefValue, bool load)
		{
			var item = new Item<TValue>(__prefKey, __prefValue);
			if (load)
				item.LoadPref();
			return item;
		}

		public Item(string __prefKey, TValue __prefValue)
		{
			prefKey = __prefKey;
			prefValue = __prefValue;
		}

		public virtual void SavePref()
		{

		}

		public virtual TValue LoadPref()
		{
			return prefValue;
		}

	}

	[System.Serializable]
	public abstract class ItemInfo<TPref, TValue>
		where TPref : Item<TValue>
	{

		public abstract TPref pref { get; }

		public virtual TValue prefValue
		{
			get { return pref.prefValue; }
			set { pref.prefValue = value; }
		}

		public bool usePlayerPref = false;
		[Range(0, 3)]
		public int playerPrefKeyParentDepth;



		public void Initialize(GameObject go, TValue __value, bool load = false)
		{
			CalculateKey(go);
			pref.prefValue = __value;

			if (load)
				pref.LoadPref();
		}

		public void CalculateKey(GameObject go)
		{
			pref.prefKey = go.ParentName(playerPrefKeyParentDepth, true);
		}

		public virtual void SavePref()
		{
			pref.SavePref();
		}

		public virtual TValue LoadPref()
		{
			return pref.LoadPref();
		}

	}

	//
	// < Integer >
	//

	public static void SetInt(string prefKey, int prefVal)
	{
		if (string.IsNullOrEmpty(prefKey))
			throw new UnityException("Invalid player pref key!");

		PlayerPrefs.SetInt(prefKey, prefVal);
	}

	public static int GetInt(string prefKey, int defaultValue = 0)
	{
		if (!PlayerPrefs.HasKey(prefKey))
			return defaultValue;
		return PlayerPrefs.GetInt(prefKey);
	}
	
	public class IntItem : Item<int>
	{

		public static new IntItem Create(string __prefKey, int __prefValue, bool load)
		{
			return (IntItem)Item<int>.Create(__prefKey, __prefValue, load);
		}


		public IntItem(string __prefKey, int __prefValue) :
			base(__prefKey, __prefValue)
		{ }

		public override void SavePref()
		{
			SetInt(prefKey, prefValue);
		}

		public override int LoadPref()
		{
			return (prefValue = GetInt(prefKey, prefValue));
		}

	}

	[System.Serializable]
	public class IntItemInfo : ItemInfo<IntItem, int>
	{

		public override IntItem pref
		{
			get { return _pref; }
		}

		protected IntItem _pref = new IntItem(string.Empty, 0);

	}

	//
	// </ Integer >
	//



	//
	// < Float >
	//

	public static void SetFloat(string prefKey, float prefVal)
	{
		if (string.IsNullOrEmpty(prefKey))
			throw new UnityException("Invalid player pref key!");

		PlayerPrefs.SetFloat(prefKey, prefVal);
	}

	public static float GetFloat(string prefKey, float defaultValue = 0.0f)
	{
		if (!PlayerPrefs.HasKey(prefKey))
			return defaultValue;
		return PlayerPrefs.GetFloat(prefKey);
	}

	public class FloatItem : Item<float>
	{

		public static new FloatItem Create(string __prefKey, float __prefValue, bool load)
		{
			return (FloatItem)Item<float>.Create(__prefKey, __prefValue, load);
		}


		public FloatItem(string __prefKey, float __prefValue) :
			base(__prefKey, __prefValue)
		{ }

		public override void SavePref()
		{
			SetFloat(prefKey, prefValue);
		}

		public override float LoadPref()
		{
			return (prefValue = GetFloat(prefKey, prefValue));
		}

	}

	[System.Serializable]
	public class FloatItemInfo : ItemInfo<FloatItem, float>
	{

		public override FloatItem pref
		{
			get { return _pref; }
		}

		protected FloatItem _pref = new FloatItem(string.Empty, 0.0f);

	}

	//
	// </ Float >
	//


}
