using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	using IBSBObjectOperationCollection = ICollection<IBSBObjectOperation>;

	namespace Events
	{
		public delegate BSBPrice ReactivePrice();
		public delegate void OnOperationAction(IBSBObjectOperation operation);
		public delegate bool OperationValidy(IBSBObjectOperation operation);
	}
	
	public interface IBSBObjectOperationCollectionable
	{
		IBSBObjectOperationCollection GetOptionsList();
	}

	public interface IBSBObjectOperation :
		IBSBObjectOperationEvents
	{
		EBSBObjectOperation		type { get; }
		IBSBObjectOperationInfo info { get; }

		bool IsValid();
		void Activate();
	}

	public interface IBSBObjectOperationEvents
	{
		event Events.OnOperationAction onOperationActivated;
	}

	public interface IBSBObjectOperationInfo
	{
		Sprite operationSprite { get; }
		BSBPrice operationPrice { get; }
	}

	public class BSBObjectOperationInfo :
		 IBSBObjectOperationInfo
	{

		public Sprite operationSprite
		{
			get; set;
		}
		public BSBPrice operationPrice
		{
			get { return _price(); }
		}

		protected Events.ReactivePrice _price;

		public static BSBObjectOperationInfo Create(
			Sprite __operationSprite,
			Events.ReactivePrice __operationPrice)
		{
			var info = new BSBObjectOperationInfo();
			info.operationSprite = __operationSprite;
			info._price = __operationPrice;
			return info;
		}

	}

	public enum EBSBObjectOperation
	{
		DEFAULT
	}

	public class BSBObjectOperation :
		IBSBObjectOperation
	{

		public static BSBObjectOperation Create(
				Events.OnOperationAction callBack,
				IBSBObjectOperationInfo info = null,
				Events.OperationValidy isValid = null
			)
		{
			var oper = new BSBObjectOperation();

			oper.onOperationActivated += callBack;
			oper.isValid = isValid;
			oper.info = info;
			oper.type = EBSBObjectOperation.DEFAULT;

			return oper;
		}

		public event Events.OnOperationAction onOperationActivated = delegate { };

		public EBSBObjectOperation		type
		{
			get; set;
		}
		public Events.OperationValidy	isValid
		{
			get; set;
		}
		public IBSBObjectOperationInfo	info
		{
			get; set;
		}

		public bool IsValid()
		{
			if (isValid == null)
				return true;
			else
				return isValid(this);
		}
		
		public void Activate()
		{
			onOperationActivated(this);
		}	

	}

}
