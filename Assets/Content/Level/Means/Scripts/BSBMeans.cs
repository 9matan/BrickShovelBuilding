using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public enum EBSBMeansType
	{
		FUNDS,
		MATERIALS,
		WORKERS
	}

	public enum EBSBMeansModel
	{
		RECOVERY,
		CONSUMABLE
	}

	//
	// < Means >
	//

	public interface IBSBMeans
	{
		EBSBMeansType type { get; }
		EBSBMeansModel model { get; }
		int amount { get; }

		void Use(int amount);
		void Add(int amount);
		bool Contains(int amount);
	}

	[System.Serializable]
	public abstract class BSBMeans : 
		IBSBMeans
	{
	
		public abstract int amount
		{
			get;
		}

		public EBSBMeansType	type
		{
			get { return _type; }
		}
		public EBSBMeansModel	model
		{
			get { return _model; }
		}

		public BSBMeans (EBSBMeansType __type, EBSBMeansModel __model)
		{
			_type = __type;
			_model = __model;
		}

		[SerializeField]
		protected EBSBMeansType _type;
		[SerializeField]
		protected EBSBMeansModel _model;

		public abstract void Use(int __amount);
		public abstract void Add(int __amount);
		public abstract bool Contains(int __amount);
	}

	//
	// </ Means >
	//

	//
	// < Consumable >
	//

	public interface IBSBConsumableMeans : IBSBMeans
	{
		
	}

	[System.Serializable]
	public class BSBConsumableMeans : BSBMeans,
		IBSBConsumableMeans
	{
		public override int amount
		{
			get { return _amount; }
		}

		[SerializeField]
		protected int _amount;

		public BSBConsumableMeans(int __amount, EBSBMeansType __type) : 
			base(__type, EBSBMeansModel.CONSUMABLE)
		{
		}

		public override void Use(int __amount)
		{
			if (!Contains(__amount))
				throw new UnityException("No enough means!");
			_amount -= __amount;
		}

		public override void Add(int __amount)
		{
			_amount += __amount;
		}

		public override bool Contains(int __amount)
		{
			return _amount >= __amount;
		}
	}

	//
	// </ Consumable >
	//

	//
	// < Recovery >
	//

	public interface IBSBRecoveryMeans : IBSBMeans
	{
		int free { get; }
		int total { get; }

		void Restore(int amount);
		bool ContainsTotal(int totalAmount);
	}

	[System.Serializable]
	public class BSBRecoveryMeans <TType> : BSBMeans,
		IBSBRecoveryMeans
	{
		public override int amount
		{
			get { return _free; }
		}
		public int			free
		{
			get { return _free; }
		}
		public int			total
		{
			get { return _total; }
		}

		[SerializeField]
		protected int _free;
		[SerializeField]
		protected int _total;

		public BSBRecoveryMeans(int __amount, EBSBMeansType __type) : 
			base(__type, EBSBMeansModel.RECOVERY)
		{
			_free = _total = __amount;
		}

		public override void Use(int __amount)
		{
			if (!Contains(__amount))
				throw new UnityException("No enough means!");
			_free -= __amount;
		}

		public override void Add(int __amount)
		{
			_total += __amount;
			_free += __amount;
		}

		public override bool Contains(int __amount)
		{
			return _free >= __amount;
		}

		public virtual bool ContainsTotal(int __totalAmount)
		{
			return _total >= __totalAmount;
		}

		public virtual void Restore(int __amount)
		{
			_free = Mathf.Min(_total, _free + __amount);
		}

	}

	//
	// </ Recovery >
	//



	//
	// < Game means >
	//

	[System.Serializable]
	public class BSBFunds : BSBConsumableMeans
	{
		public BSBFunds(int __amount) : 
			base(__amount, EBSBMeansType.FUNDS)
		{
		}
	}

	[System.Serializable]
	public class BSBMaterials : BSBConsumableMeans
	{
		public BSBMaterials(int __amount) :
			base(__amount, EBSBMeansType.MATERIALS)
		{
		}
	}

	[System.Serializable]
	public class BSBWorkers : BSBRecoveryMeans
	{
		public BSBWorkers(int __amount) :
			base(__amount, EBSBMeansType.WORKERS)
		{
		}
	}

	//
	// </ Game means >
	//

	//
	// < Means list >
	//

	public interface IBSBMeansList :
		IEnumerable<KeyValuePair<EBSBMeansType, IBSBMeans>>
	{
		void Add(IBSBMeans means);
		bool Contains(EBSBMeansType type);

		void AddMeans(IBSBMeansList meansList);
		void UseMeans(IBSBMeansList meansList);
		void RestoreMeans(IBSBMeansList meansList);
		bool ContainsMeans(IBSBMeansList meansList);
		bool ContainsMeans(IBSBMeans means);

		IBSBRecoveryMeans GetRecovery(EBSBMeansType type);
		IBSBConsumableMeans GetConsumable(EBSBMeansType type);
	}

	[System.Serializable]
	public class BSBMeansList<TType, TContainer> :
		IBSBMeansList
	{

		public IBSBMeans this [EBSBMeansType type]
		{
			get { return _meansContainer[type]; }
		}

		[SerializeField]
		protected VOSSerializableDictionary<EBSBMeansType, IBSBMeans> _meansContainer =
			new VOSSerializableDictionary<EBSBMeansType, IBSBMeans>();

		public void Add(IBSBMeans means)
		{
			_meansContainer.Add(means.type, means);
		}

		public bool Contains(EBSBMeansType type)
		{
			return _meansContainer.ContainsKey(type);
		}



		public void AddMeans(IBSBMeansList meansList)
		{
			foreach (var kvp in meansList)
			{
				_meansContainer[kvp.Key].Add(kvp.Value.amount);
			}
		}

		public void UseMeans(IBSBMeansList meansList)
		{
			foreach (var kvp in meansList)
			{
				_meansContainer[kvp.Key].Use(kvp.Value.amount);
			}
		}
		
		public void RestoreMeans(IBSBMeansList meansList)
		{
			foreach (var kvp in meansList)
			{
				if (kvp.Value.model == EBSBMeansModel.RECOVERY)
				{
					GetRecovery(kvp.Key).Restore(kvp.Value.amount);
				}
			}
		}

		public bool ContainsMeans(IBSBMeansList meansList)
		{
			bool contains = true;

			foreach (var kvp in meansList)
			{
				if (!(contains = ContainsMeans(kvp.Value)))
					break;
			}

			return contains;
		}

		public bool ContainsMeans(IBSBMeans means)
		{
			if (!Contains(means.type))
				return false;

			return _meansContainer[means.type].Contains(means.amount);
		}





		public IBSBRecoveryMeans GetRecovery(EBSBMeansType type)
		{
			return (IBSBRecoveryMeans)_meansContainer[type];
		}

		public IBSBConsumableMeans GetConsumable(EBSBMeansType type)
		{
			return (IBSBConsumableMeans)_meansContainer[type];
		}



		public IEnumerator<KeyValuePair<EBSBMeansType, IBSBMeans>> GetEnumerator()
		{
			return _meansContainer.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_meansContainer).GetEnumerator();
		}
	}

	//
	// </ Means list >
	//

	//
	// < Game list >
	//

	[System.Serializable]
	public class BSBReserves : BSBMeansList
	{

	}

	//
	// < Game list >
	//

}
