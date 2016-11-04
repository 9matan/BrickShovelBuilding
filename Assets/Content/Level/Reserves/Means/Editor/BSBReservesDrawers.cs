using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BSB
{
/*
//	[CustomPropertyDrawer(typeof(BSBReserves))]
	public class BSBReservesDrawer : PropertyDrawer
	{


		protected bool _foldout = false;
		protected float _itemHeight = 15.0f;

		protected BSBReserves _reserves;
		protected GUIContent _label;
		protected SerializedProperty _property;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (_foldout)
				return _itemHeight * 4;
			else
				return _itemHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorUtility.SetDirty(property.serializedObject.targetObject);

			_label = label;
			_property = property;
			_CheckInitialize();			

			var rect = position;
			rect.height = _itemHeight;

			EditorGUI.BeginChangeCheck();
			_foldout = EditorGUI.Foldout(rect, _foldout, label, true);
			if (EditorGUI.EndChangeCheck())
				EditorPrefs.SetBool(label.text, _foldout);

			if (!_foldout)
				return;

			Undo.RecordObject(property.serializedObject.targetObject, "Change");

			rect.x += 10.0f;
			rect.y += rect.height;

		//	BSBEditorGUI.ReservesField(rect, _reserves);
			_property.objectReferenceValue = _reserves;


			_property.serializedObject.ApplyModifiedProperties();
		}

		protected void _CheckInitialize()
		{
			_reserves = _property.objectReferenceValue as BSBReserves;
			if (_reserves == null)
			{
				_reserves = BSBReserves.Create();
				_property.objectReferenceValue = _reserves;
			}
				
		//	_foldout = EditorPrefs.GetBool(_label.text);
		}

	}*/

}