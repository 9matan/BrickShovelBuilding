using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using BSB;

namespace BSB
{

	public class BSBObjectOperetionView : MonoBehaviour 
	{

		public int funds
		{
			set { _fundsui.text = value.ToString(); }
		}
		public int materials
		{
			set { _materialsui.text = value.ToString(); }
		}
		public int workers
		{
			set { _workersui.text = value.ToString(); }
		}

		public BSBPrice price
		{
			set
			{
				funds = value.funds;
				materials = value.materials;
				workers = value.workers;
			}
		}

		[SerializeField]
		protected Button	_button;
		[SerializeField]
		protected Image		_operationImg;
		[SerializeField]
		protected Text		_fundsui;
		[SerializeField]
		protected Text		_materialsui;
		[SerializeField]
		protected Text		_workersui;

		public IBSBObjectOperation oper
		{
			get; protected set;
		}

		public void SetOperation(IBSBObjectOperation __oper)
		{
			oper = __oper;
			_UpdateData();
		}

		public void Reset()
		{
			oper = null;
		}

		public void OnClick()
		{
			if (oper.IsValid())
				oper.Activate();
		}

		protected void Update()
		{
			if (oper == null) return;
			_UpdateData();
			var c = _operationImg.color;

			if (oper.IsValid())
				_operationImg.color = new Color(c.a, c.g, c.b, 1.0f);
			else
				_operationImg.color = new Color(c.a, c.g, c.b, 0.5f);
		}

		protected void _UpdateData()
		{
			price = oper.info.operationPrice;
			_operationImg.sprite = oper.info.operationSprite;
		}

	}

}
