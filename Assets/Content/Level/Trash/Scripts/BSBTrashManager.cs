using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public interface IBSBTrashManager
	{
		List<IBSBObjectOperation> GetOperations(IBSBTrash trash);

		BSBPrice CleaningTrashPrice(IBSBTrash trash);
		bool TryCleaningTrash(IBSBTrash trash);
		void CleaningTrash(IBSBTrash itrash);
		IBSBTrash CreateTrash();
	}

	public class BSBTrashManager : MonoBehaviour,
		IBSBTrashManager,
		IVOSInitializable
	{

		public IBSBPriceManager		priceManager
		{
			get { return BSBDirector.priceManager; }
		}
		public IBSBPlayerResources	playerResources
		{
			get { return BSBDirector.playerResources; }
		}

		[SerializeField]
		protected BSBTrashInfo _info;
		[SerializeField]
		protected BSBTrashFactory _factory;

		protected Dictionary<int, BSBTrash> _trashContainer = new Dictionary<int, BSBTrash>();

		//
		// < Intialize >
		//

		public void Initialize()
		{
			
		}

		//
		// < Intialize >
		//

		public BSBPrice CleaningTrashPrice(IBSBTrash trash)
		{
			return priceManager.Inflation(
				_info.GetCleaningPrice());
		}
		
		public bool TryCleaningTrash(IBSBTrash trash)
		{
			return playerResources.Contains(
				CleaningTrashPrice(trash));
		}

		public void CleaningTrash(IBSBTrash itrash)
		{
			if (!TryCleaningTrash(itrash))
				return;

			var price = CleaningTrashPrice(itrash);
			playerResources.Use(price);
			playerResources.Restore(price);

			var trash = _GetTrashById(itrash.id);
			BSBDirector.map.ClearPlacement(trash.mapPlacement);
			_RemoveTrash(trash);
			_FreeTrash(trash);
		}
		
		public IBSBTrash CreateTrash()
		{
			var trash = _CreateTrash();

			trash.Initialize(_info);
			_AddTrash(trash);

			return trash;
		}



		protected void _AddTrash(BSBTrash trash)
		{
			_trashContainer.Add(trash.id, trash);
		}

		protected void _RemoveTrash(BSBTrash trash)
		{
			_trashContainer.Remove(trash.id);
		}

		protected BSBTrash _GetTrashById(int id)
		{
			return _trashContainer[id];
		}




		protected BSBTrash _CreateTrash()
		{
			return _factory.Allocate();
		}

		protected void _FreeTrash(BSBTrash trash)
		{
			_factory.Free(trash);
		}

		public List<IBSBObjectOperation> GetOperations(IBSBTrash itrash)
		{
			var list = new List<IBSBObjectOperation>();
			var trash = _GetTrashById(itrash.id);

			list.Add(
				_GetCleaningOperation(trash));

			return list;
		}

		protected BSBObjectOperation _GetCleaningOperation(BSBTrash trash)
		{
			return BSBObjectOperation.Create(
				(IBSBObjectOperation oper) =>
				{
					if (oper.IsValid())
					{
						CleaningTrash(trash);
					}
				},
				BSBObjectOperationInfo.Create(
					_info.cleaningOperationSprite,
					() => { return CleaningTrashPrice(trash); }),
				(IBSBObjectOperation oper) =>
				{
					return TryCleaningTrash(trash);
				}
				);
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
