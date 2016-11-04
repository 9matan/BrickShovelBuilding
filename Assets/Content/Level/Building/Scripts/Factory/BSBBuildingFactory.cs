using UnityEngine;
using System.Collections;
using BSB;

namespace BSB
{

	public interface IBSBBuildingFactory :
		IVOSComponentPoolFactory<BSBBuilding>,
		IVOSFactoryTypable<EBSBBuildingType>
	{

	}

	public class BSBBuildingFactory : VOSComponentPoolFactory<BSBBuilding>,
		IBSBBuildingFactory
	{

		public EBSBBuildingType factoryType
		{
			get { return _factoryType; }
		}

		protected EBSBBuildingType _factoryType;

		public override void Initialize()
		{
			_factoryType = _creatorImplementer.prefab.GetComponent<BSBBuilding>().type;

			base.Initialize();
		}
		
		//
		// < Log >
		//

		public bool debug = false;
				
		public void Log(object msg)
		{
			if(debug)
				Debug.Log(msg);
		}		

		//
		// </ Log >
		//
		
	}

}
