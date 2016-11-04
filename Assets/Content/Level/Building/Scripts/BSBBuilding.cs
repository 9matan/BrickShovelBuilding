using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	using Events;

	namespace Events
	{
		public delegate void OnBuildingAction(IBSBBuilding building);
		public delegate void OnActionUpdate(IBSBMonoAction action);
	}

	public interface IBSBBuilding :
		IBSBActionTimeable,
		IBSBBuildingEvents
	{
		int					id { get; }
		EBSBBuildingType	type { get; }
		int					level { get; }
		EBSBBuildingState	state { get; }
	}	

	public interface IBSBBuildingEvents
	{
		event OnBuildingAction onBuildingBuilt;
		event OnBuildingAction onBuildingUpgraded;
	}

	public enum EBSBBuildingType
	{
		NONE,
		BARRACKS,
		HOUSE,
		SHOP
	}

	public enum EBSBBuildingState
	{
		NONE,
		CONSTRUCTION,
		UPGRADE,
		IDLE
	}

	public class BSBBuilding : MonoBehaviour,
		IBSBBuilding
	{

		protected static int _ID = 0;

		public int					id
		{
			get { return _id; }
		}
		public EBSBBuildingType		type
		{
			get { return _type; }
		}
		public int					level
		{
			get { return _level; }
		}
		public EBSBBuildingState	state
		{
			get { return _state; }
		}

	//	[SerializeField]
		protected EBSBBuildingType _type;
		[SerializeField]
		protected int _level;
		[SerializeField]
		protected EBSBBuildingState _state;

		protected int _id;

		//
		// < Initialize >
		//

		public void Initialize()
		{
			_id = ++_ID;
		}

		//
		// </ Initialize >
		//

		public float actionTime
		{
			get
			{

				return _currentAction.actionTime;
			}
		}
		public float currentActionTime
		{
			get
			{
				return _currentAction.currentActionTime;
			}
		}

		protected BSBMonoAction _currentAction;

		public void Build(float time)
		{
			_state = EBSBBuildingState.CONSTRUCTION;
			_currentAction = BSBMonoAction.StartAction(
				this, _UpdateConstruction, _OnBuilt, time);
		}

		public void Upgrade(float time)
		{
			_state = EBSBBuildingState.UPGRADE;
			_currentAction = BSBMonoAction.StartAction(
				this, _UpdateUpgrade, _OnUpgraded, time);
		}



		

		protected void _UpdateConstruction(IBSBMonoAction action)
		{

		}

		protected void _UpdateUpgrade(IBSBMonoAction action)
		{

		}

		//
		// < Events >
		//

		public event OnBuildingAction onBuildingBuilt = delegate { };
		public event OnBuildingAction onBuildingUpgraded = delegate { };

		protected void _OnBuilt()
		{
			_level = 1;
			_state = EBSBBuildingState.IDLE;
			onBuildingBuilt(this);
		}

		protected void _OnUpgraded()
		{
			++_level;
			_state = EBSBBuildingState.IDLE;
			onBuildingUpgraded(this);
		}

		//
		// </ Events >
		//

		//
		// < Clear >
		//

		protected void _ClearEvents()
		{
			onBuildingBuilt = delegate { };
			onBuildingUpgraded = delegate { };
		}

		public void Clear()
		{
			_ClearEvents();

			_level = 0;
			_state = EBSBBuildingState.NONE;
		}

		//
		// </ Clear >
		//

		//
		// < Log >
		//

		public bool debug = false;

		public void Log(object msg)
		{
			if (debug)
				Debug.Log(msg);
		}

		//
		// </ Log >
		//

	}



	//
	// < Action >
	//

	public interface IBSBMonoAction :
		IBSBActionTimeable
	{
		bool stop { get; }
		bool isRunning { get; }

		IEnumerator Action(OnActionUpdate update, System.Action onStopped, float time);
	}

	public interface IBSBActionTimeable
	{
		float actionTime { get; }
		float currentActionTime { get; }
	}

	public class BSBMonoAction : IBSBMonoAction
	{

		public float	actionTime
		{
			get; protected set;
		}
		public float	currentActionTime
		{
			get; protected set;
		}
		public bool		stop
		{
			get; set;
		}
		public bool		isRunning
		{
			get; protected set;
		}
		public bool		pause
		{
			get; protected set;
		}

		public BSBMonoAction()
		{
			_Reset();
		}

		protected void _Reset()
		{
			pause = false;
			isRunning = false;
			stop = false;
		}

		public IEnumerator Action(OnActionUpdate update, System.Action onStopped, float time)
		{
			actionTime = time;
			currentActionTime = 0.0f;
			isRunning = true;			

			while (currentActionTime < actionTime && !stop)
			{
				if (!pause)
				{
					update(this);
					currentActionTime += Time.deltaTime;
				}
				yield return null;
			}

			onStopped();
			_Reset();
		}

		public static BSBMonoAction StartAction(MonoBehaviour mono, OnActionUpdate update, System.Action onStopped, float time)
		{
			var action = new BSBMonoAction();
			mono.StartCoroutine(action.Action(update, onStopped, time));
			return action;
		}

	}

	//
	// </ Action >
	//

}
