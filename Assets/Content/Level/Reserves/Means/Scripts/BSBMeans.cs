using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	using Means;
	using IBSBReserveMeans = Means.IBSBMeans<EBSBMeansType>;

	namespace Means
	{

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

			[SerializeField, HideInInspector]
			protected TType _type;
			[SerializeField, HideInInspector]
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
			where TMeans : IBSBMeans<TType>
			where TContainer : IDictionary<TType, TMeans>
		{

			public IBSBMeans<TType> this[TType type]
			{
				get { return _meansContainer[type]; }
			}

			[SerializeField, HideInInspector]
			protected TContainer _meansContainer;

			public BSBMeansList(TContainer __container)
			{
				_meansContainer = __container;
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
		// < Storage >
		//

		public interface IBSBMeansStorage<TType> :
			IBSBRecoveryMeans<TType>,
			IBSBConsumableMeans<TType>
		{
			IBSBMeans<TType>			means { get; }
			IBSBRecoveryMeans<TType>	recoveryMeans { get; }

			int maxAmount { get; }

			bool isRecovery { get; }
			int capacity { get; }
			int freeCapacity { get; }

			void Extend(int capacity);
			void Resize(int __capacity);
		}

		[System.Serializable]
		public class BSBMeansStorage<TType> :
			IBSBMeansStorage<TType>
		{

			public IBSBMeans<TType>			means
			{
				get { return _means; }
			}
			public IBSBRecoveryMeans<TType>	recoveryMeans
			{
				get
				{
					if(isRecovery)
						return (IBSBRecoveryMeans<TType>)_means;
					return null;
				}
			}
			public bool						isRecovery
			{
				get { return _means.model == EBSBMeansModel.RECOVERY; }
			}

			public int capacity
			{
				get { return _capacity; }
			}
			public int freeCapacity
			{
				get { return Mathf.Max(capacity - maxAmount, 0); }
			}
			public int maxAmount
			{
				get
				{
					if (isRecovery)
						return recoveryMeans.total;
					else
						return _means.amount;
				}
			}
			public int amount
			{
				get { return _means.amount; }
			}

			public EBSBMeansModel	model
			{
				get { return _means.model; }
			}
			public TType			type
			{
				get { return _means.type; }
			}
			
			int IBSBRecoveryMeans<TType>.free
			{
				get
				{
					if (isRecovery)
						return recoveryMeans.free;
					return 0;
				}
			}
			int IBSBRecoveryMeans<TType>.total
			{
				get
				{
					if (isRecovery)
						return recoveryMeans.total;
					return 0;
				}
			}

			[SerializeField]
			protected int _capacity = 0;

			
			protected IBSBMeans<TType>			_means;

			//
			// < Constructors >
			//

			public BSBMeansStorage(IBSBMeans<TType> __means, int __capacity = 0)
			{
				_means = __means;
				_capacity = 0;
				Extend(__capacity);
			}

			//
			// </ Constructors >
			//

			public void Add(int __amount)
			{
				__amount = Mathf.Min(freeCapacity, __amount);
				_means.Add(__amount);
			}
			public void Set(int __amount)
			{
				__amount = Mathf.Min(_capacity, __amount);
				_means.Set(__amount);
			}
			public void Use(int __amount)
			{
				_means.Use(__amount);
			}
			public bool Contains(int __amount)
			{
				return _means.Contains(__amount);
			}

			bool IBSBRecoveryMeans<TType>.ContainsTotal(int __totalAmount)
			{
				if (isRecovery)
					return recoveryMeans.ContainsTotal(__totalAmount);
				return false;
			}
			void IBSBRecoveryMeans<TType>.Restore(int __amount)
			{
				if (isRecovery)
					recoveryMeans.Restore(__amount);
			}

			public void Extend(int __capacity)
			{
				_capacity += __capacity;
			}
			public void Resize(int __capacity)
			{
				if (__capacity < _capacity)
					Add(_capacity - __capacity);
				_capacity = __capacity;
			}
		}

		//
		// </ Storage >
		//

		[System.Serializable]
		public class BSBReservesInternalContainer : Dictionary<EBSBMeansType, IBSBReserveMeans>
		{
		}

	}

	//
	// < Game reserves >
	//	

	public enum EBSBMeansType
	{
		FUNDS,
		MATERIALS,
		WORKERS
	}	

	public interface IBSBReserves : IBSBMeansList<EBSBMeansType, IBSBReserveMeans>
	{
		IBSBFundsMeans funds { get; }
		IBSBWorkersMeans workers { get; }
		IBSBMaterialsMeans materials { get; }
	}

	[System.Serializable]
	public class BSBReserves : BSBMeansList<EBSBMeansType, IBSBReserveMeans, BSBReservesInternalContainer>,
		IBSBReserves
	{

		public IBSBFundsMeans funds
		{
			get
			{
				if (!Contains(EBSBMeansType.FUNDS))
					Add(new BSBFundsMeans(0));
				return (IBSBFundsMeans)this[EBSBMeansType.FUNDS];
			}
		}
		public IBSBWorkersMeans workers
		{
			get
			{
				if (!Contains(EBSBMeansType.WORKERS))
					Add(new BSBWorkersMeans(0));
				return (IBSBWorkersMeans)this[EBSBMeansType.WORKERS];
			}
		}
		public IBSBMaterialsMeans materials
		{
			get
			{
				if (!Contains(EBSBMeansType.MATERIALS))
					Add(new BSBMaterialsMeans(0));
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
			Set(__funds, __workers, __materials);
		}

		public BSBReserves(BSBPrice __price) :
			base(new BSBReservesInternalContainer())
		{
			Set(__price.funds, __price.workers, __price.materials);
		}

		public void Set(int __funds, int __workers, int __materials)
		{
			funds.Set(__funds);
			workers.Set(__workers);
			materials.Set(__materials);
		}

	}

	// < storage >

	public interface IBSBReserveStorage : IBSBMeansStorage<EBSBMeansType>
	{
	}

	[System.Serializable]
	public class BSBReserveStorage : BSBMeansStorage<EBSBMeansType>,
		IBSBReserveStorage
	{
		public BSBReserveStorage(IBSBMeans<EBSBMeansType> __means, int __capacity = 0) : base(__means, __capacity)
		{
		}
	}

	[System.Serializable]
	public class BSBFundsStorage : BSBReserveStorage
	{
		public BSBFundsStorage(int __capacity = 0) : base(new BSBFundsMeans(0), __capacity)
		{
		}
	}

	[System.Serializable]
	public class BSBMaterialsStorage : BSBReserveStorage
	{
		public BSBMaterialsStorage(int __capacity = 0) : base(new BSBMaterialsMeans(0), __capacity)
		{
		}
	}

	[System.Serializable]
	public class BSBWorkersStorage : BSBReserveStorage
	{
		public BSBWorkersStorage(int __capacity = 0) : base(new BSBWorkersMeans(0), __capacity)
		{
		}
	}

	public interface IBSBReservesStorage : IBSBReserves
	{

	}

	[System.Serializable]
	public class BSBReservesStorage : BSBMeansList<EBSBMeansType, IBSBReserveMeans, BSBReservesInternalContainer>,
		IBSBReservesStorage
	{
		IBSBFundsMeans IBSBReserves.funds
		{
			get { return (IBSBFundsMeans)this.funds.means; }
		}
		IBSBWorkersMeans IBSBReserves.workers
		{
			get { return (IBSBWorkersMeans)this.workers.means; }
		}
		IBSBMaterialsMeans IBSBReserves.materials
		{
			get { return (IBSBMaterialsMeans)this.materials.means; }
		}

		public IBSBReserveStorage funds
		{
			get
			{
				if (!Contains(EBSBMeansType.FUNDS))
					Add(new BSBFundsStorage(0));
				return (IBSBReserveStorage)this[EBSBMeansType.FUNDS];
			}
		}
		public IBSBReserveStorage workers
		{
			get
			{
				if (!Contains(EBSBMeansType.WORKERS))
					Add(new BSBWorkersStorage(0));
				return (IBSBReserveStorage)this[EBSBMeansType.WORKERS];
			}
		}
		public IBSBReserveStorage materials
		{
			get
			{
				if (!Contains(EBSBMeansType.MATERIALS))
					Add(new BSBMaterialsStorage(0));
				return (IBSBReserveStorage)this[EBSBMeansType.MATERIALS];
			}
		}

		public BSBReservesStorage() :
			base(new BSBReservesInternalContainer())
		{
			_Set(0, 0, 0);
		}

		public BSBReservesStorage(int __funds, int __workers, int __materials) :
			base(new BSBReservesInternalContainer())
		{
			_Set(__funds, __workers, __materials);
		}

		public BSBReservesStorage(BSBPrice __price) :
			base(new BSBReservesInternalContainer())
		{
			_Set(__price.funds, __price.workers, __price.materials);
		}

		protected void _Set(int __funds, int __workers, int __materials)
		{
			funds.Extend(__funds);
			workers.Extend(__workers);
			materials.Extend(__materials);

			funds.Set(__funds);
			workers.Set(__workers);
			materials.Set(__materials);
		}
	}
	
	// </ storage >

	// < means >

	public interface IBSBFundsMeans : IBSBConsumableMeans<EBSBMeansType>
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

	public interface IBSBMaterialsMeans : IBSBConsumableMeans<EBSBMeansType>
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

	public interface IBSBWorkersMeans : IBSBRecoveryMeans<EBSBMeansType>
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

	// </ means >

	//
	// </ Game reserves >
	//

}
