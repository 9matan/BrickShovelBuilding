using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using VVOSS.Json;

public static class VOSRandom
{

	[System.Serializable]
	public struct Probability :
		IVOSJsonSirializable
	{
		public int numerator;
		public int denominator;

		public Probability (int __numerator, int __denominator)
		{
			numerator = __numerator;
			denominator = __denominator;
		}

		public bool DoesEventOccur()
		{
			return (numerator >= UnityEngine.Random.Range(1, denominator + 1));
		}



		public void SerializeToJson(IVOSJsonObject jobj)
		{
			jobj.AddField("Numerator", numerator);
			jobj.AddField("Denominator", denominator);
		}

		public void DeserializeFromJson(IVOSJsonObject jobj)
		{
			denominator = jobj.GetIntField("Denominator");
			numerator = jobj.GetIntField("Numerator");
		}
	}

	public interface IWeightProbabilityItem<T>
	{
		T item { get; }
		int weight { get; }
	}

	static public T AbsoluteProbability<T>(IWeightProbabilityItem<T>[] probabilities) where T: new()
	{
		int sum = 0, cur = 0, rnd;
		T res = new T();

		for(int i = 0; i < probabilities.Length; ++i)
		{
			sum += probabilities[i].weight;
		}

		rnd = Random.Range(1, sum + 1);

		for (int i = 0; i < probabilities.Length; ++i)
		{
			cur += probabilities[i].weight;
			if (cur >= rnd)
			{
				res = probabilities[i].item;
				break;
			}
		}

		return res;
	}

/*	static public T AbsProb<T>(Dictionary<T, float> prob)
	{
		float sum = 0.0f;

		foreach (var item in prob)
			sum += item.Value;

		Dictionary<T, float> nprob = new Dictionary<T, float>();

		foreach (var item in prob)
			nprob.Add(item.Key, item.Value / sum);

		return RelatProb(nprob);
	}

	static public T RelatProb<T>(Dictionary<T, float> prob)
	{
		float[] nprob = new float[prob.Count];
		T[] number = new T[prob.Count];
		int iter = 0;

		foreach (var item in prob)
		{
			nprob[iter] = item.Value;
			if (iter != 0)
				nprob[iter] += nprob[iter - 1];
			number[iter++] = item.Key;
		}

		float R = Random.Range(0.0f, 1.0f);

		for (int i = 0; i < nprob.Length; ++i)
		{
			if (nprob[i] >= R)
				return number[i];
		}

		return number[0];
	}
	*/
}
