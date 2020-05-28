// Serialization.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Digicrafts.Serialization
{	
	// List<T>
	[Serializable]
	public class Serializer<T>
	{
		[SerializeField]
		List<T> target;
		public List<T> ToList() { return target; }

		public Serializer(List<T> target)
		{
			this.target = target;
		}
	}

	// Dictionary<TKey, TValue>
	[Serializable]
	public class Serializer<TKey, TValue> : ISerializationCallbackReceiver
	{
		[SerializeField]
		List<TKey> keys;
		[SerializeField]
		List<TValue> values;

		Dictionary<TKey, TValue> target;
		public Dictionary<TKey, TValue> ToDictionary() { return target; }

		public Serializer(Dictionary<TKey, TValue> target)
		{
			this.target = target;
		}

		public void OnBeforeSerialize()
		{
			keys = new List<TKey>(target.Keys);
	//		if(target.Values is IList && target.Values.GetType().IsGenericType) {
	//
	//		} else
				values = new List<TValue>(target.Values);
		}

		public void OnAfterDeserialize()
		{
			var count = Math.Min(keys.Count, values.Count);
			target = new Dictionary<TKey, TValue>(count);
			for (var i = 0; i < count; ++i)
			{
				target.Add(keys[i], values[i]);
			}
		}
	}

	/// <summary>
	/// Json helper.
	/// </summary>
	public static class JsonHelper
	{
		public static T[] FromJson<T>(string json)
		{
			Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
			return wrapper.Items;
		}

		public static string ToJson<T>(T[] array)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson(wrapper);
		}

		public static string ToJson<T>(T[] array, bool prettyPrint)
		{
			Wrapper<T> wrapper = new Wrapper<T>();
			wrapper.Items = array;
			return JsonUtility.ToJson(wrapper, prettyPrint);
		}

		[Serializable]
		private class Wrapper<T>
		{
			public T[] Items;
		}
	}

}