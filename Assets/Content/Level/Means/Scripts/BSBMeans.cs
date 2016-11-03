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

	public interface IBSBMeans<TType>
	{
		TType type { get; }
		EBSBMeansModel model { get; }
		int amount { get; }

		void Use(int amount);
		void Add(int amount);
		bool Contains(int amount);
		void Set(int __amount);
	}

	[System.Serializable]
	public abstract class BSBMeans<TType> :
		IBSBMeans<TType>
	{

		public abstract int amount
		{
			get;
		}

		public TType type
		{
			get { return _type; }
		}
		public EBSBMeansModel model
		{
			get { return _model; }
		}

		public BSBMeans(TType __type, EBSBMeansModel __model)
		{
			_type = __type;
			_model = __model;
		}

		[SerializeField]
		protected TType _type;
		[SerializeField]
		protected EBSBMeansModel _model;

		public abstract void Use(int __amount);
		public abstract void Add(int __amount);
		public abstract bool Contains(int __amount);
		public abstract void Set(int __amount);
	}

	//
	// </ Means >
	//

	//
	// < Consumable >
	//

	public interface IBSBConsumableMeans<TType> : IBSBMeans<TType>
	{

	}

	[System.Serializable]
	public class BSBConsumableMeans<TType> : BSBMeans<TType>,
		IBSBConsumableMeans<TType>
	{
		public override int amount
		{
			get { return _amount; }
		}

		[SerializeField]
		protected int _amount;

		public BSBConsumableMeans(int __amount, TType __type) :
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

		public override void Set(int __amount)
		{
			_amount = __amount;
		}
	}

	//
	// </ Consumable >
	//

	//
	// < Recovery >
	//

	public interface IBSBRecoveryMeans<TType> : IBSBMeans<TType>
	{
		int free { get; }
		int total { get; }

		void Restore(int amount);
		bool ContainsTotal(int totalAmount);
	}

	[System.Serializable]
	public class BSBRecoveryMeans<TType> : BSBMeans<TType>,
		IBSBRecoveryMeans<TType>
	{
		public override int amount
		{
			get { return _free; }
		}
		public int free
		{
			get { return _free; }
		}
		public int total
		{
			get { return _total; }
		}

		[SerializeField]
		protected int _free;
		[SerializeField]
		protected int _total;

		public BSBRecoveryMeans(int __amount, TType __type) :
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

		public override void Set(int __amount)
		{
			_free = _total = __amount;
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

	public interface IBSBReserveMeans : IBSBConsumableMeans<EBSBMeansType>
	{ }
	
	public interface IBSBFundsMeans : IBSBReserveMeans
	{ }

	[System.Serializable]
	public class BSBFundsMeans : BSBConsumableMeans<EBSBMeansType>,
		IBSBFundsMeans
	{
		public BSBFundsMeans(int __amount) :
			base(__amount, EBSBMeansType.FUNDS)
		{
		}
	}

	public interface IBSBMaterialsMeans : IBSBReserveMeans
	{ }

	[System.Serializable]
	public class BSBMaterialsMeans : BSBConsumableMeans<EBSBMeansType>,
		IBSBMaterialsMeans
	{
		public BSBMaterialsMeans(int __amount) :
			base(__amount, EBSBMeansType.MATERIALS)
		{
		}
	}

	public interface IBSBWorkersMeans : IBSBReserveMeans
	{ }

	[System.Serializable]
	public class BSBWorkersMeans : BSBRecoveryMeans<EBSBMeansType>,
		IBSBWorkersMeans
	{
		public BSBWorkersMeans(int __amount) :
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

	public interface IBSBMeansList<TType, TMeans> :
		IEnumerable<KeyValuePair<TType, TMeans>>
		where TMeans : IBSBMeans<TType>		
	{
		void Add(TMeans means);
		bool Contains(TType type);

		void AddMeans(IBSBMeansList<TType, TMeans> meansList);
		void UseMeans(IBSBMeansList<TType, TMeans> meansList);
		void RestoreMeans(IBSBMeansList<TType, TMeans> meansList);
		bool ContainsMeans(IBSBMeansList<TType, TMeans> meansList);
		bool ContainsMeans(TMeans means);

		IBSBRecoveryMeans<TType> GetRecovery(TType type);
		IBSBConsumableMeans<TType> GetConsumable(TType type);
	}

	[System.Serializable]
	public class BSBMeansList<TType, TMeans, TContainer> :
		IBSBMeansList<TType, TMeans>
		where TMeans: IBSBMeans<TType>
		where TContainer: IDictionary<TType, TMeans>
	{

		public IBSBMeans<TType> this [TType type]
		{
			get { return _meansContainer[type]; }
		}

		[SerializeField]
		protected TContainer _meansContainer;

		public BSBMeansList(TContainer container)
		{
			_meansContainer = container;
		}


		public void Add(TMeans means)
		{
			_meansContainer.Add(means.type, means);
		}

		public bool Contains(TType type)
		{
			return _meansContainer.ContainsKey(type);
		}



		public void AddMeans(IBSBMeansList<TType, TMeans> meansList)
		{
			foreach (var kvp in meansList)
			{
				_meansContainer[kvp.Key].Add(kvp.Value.amount);
			}
		}

		public void UseMeans(IBSBMeansList<TType, TMeans> meansList)
		{
			foreach (var kvp in meansList)
			{
				_meansContainer[kvp.Key].Use(kvp.Value.amount);
			}
		}
		
		public void RestoreMeans(IBSBMeansList<TType, TMeans> meansList)
		{
			foreach (var kvp in meansList)
			{
				if (kvp.Value.model == EBSBMeansModel.RECOVERY)
				{
					GetRecovery(kvp.Key).Restore(kvp.Value.amount);
				}
			}
		}

		public bool ContainsMeans(IBSBMeansList<TType, TMeans> meansList)
		{
			bool contains = true;

			foreach (var kvp in meansList)
			{
				if (!(contains = ContainsMeans(kvp.Value)))
					break;
			}

			return contains;
		}

		public bool ContainsMeans(TMeans means)
		{
			if (!Contains(means.type))
				return false;

			return _meansContainer[means.type].Contains(means.amount);
		}





		public IBSBRecoveryMeans<TType> GetRecovery(TType type)
		{
			if (_meansContainer[type].model != EBSBMeansModel.RECOVERY)
				return null;

			return (IBSBRecoveryMeans<TType>)_meansContainer[type];
		}

		public IBSBConsumableMeans<TType> GetConsumable(TType type)
		{
			if (_meansContainer[type].model != EBSBMeansModel.CONSUMABLE)
				return null;

			return (IBSBConsumableMeans<TType>)_meansContainer[type];
		}



		public IEnumerator<KeyValuePair<TType, TMeans>> GetEnumerator()
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

	public interface IBSBReservesInternalContainer : IDictionary<EBSBMeansType, IBSBReserveMeans>
	{ }

	[System.Serializable]
	public class BSBReservesInternalContainer : Dictionary<EBSBMeansType, IBSBReserveMeans>,
		IBSBReservesInternalContainer
	{
	}

	public interface IBSBReserves : IBSBMeansList< EBSBMeansType, IBSBReserveMeans>
	{
		IBSBFundsMeans funds { get; }
		IBSBWorkersMeans workers { get; }
		IBSBMaterialsMeans materials { get; }
	}

	[System.Serializable]
	public class BSBReserves : BSBMeansList<EBSBMeansType, IBSBReserveMeans, BSBReservesInternalContainer>,
		IBSBReserves
	{

		public IBSBFundsMeans		funds
		{
			get
			{
				if (!Contains(EBSBMeansType.FUNDS))
					Add(_funds);
				return (IBSBFundsMeans)this[EBSBMeansType.FUNDS];
			}
		}
		public IBSBWorkersMeans		workers
		{
			get
			{
				if (!Contains(EBSBMeansType.WORKERS))
					Add(_workers);
				return (IBSBWorkersMeans)this[EBSBMeansType.WORKERS];
			}
		}
		public IBSBMaterialsMeans	materials
		{
			get
			{
				if (!Contains(EBSBMeansType.MATERIALS))
					Add(_materials);
				return (IBSBMaterialsMeans)this[EBSBMeansType.MATERIALS];
			}
		}

		public BSBReserves() : 
			base(new BSBReservesInternalContainer())
		{
		}

		public BSBReserves(int __funds, int __workers, int __materials) :
			base(new BSBReservesInternalContainer())
		{
			funds.Set(__funds);
			workers.Set(__workers);
			materials.Set(__materials);
		}

		[SerializeField]
		protected BSBFundsMeans _funds = new BSBFundsMeans(0);
		[SerializeField]
		protected BSBWorkersMeans _workers = new BSBWorkersMeans(0);
		[SerializeField]
		protected BSBMaterialsMeans _materials = new BSBMaterialsMeans(0);

	}

	//
	// < Game list >
	//

}
