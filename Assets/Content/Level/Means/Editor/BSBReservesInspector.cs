using UnityEngine;
using UnityEditor;
using System.Collections;

namespace BSB
{

	[CustomPropertyDrawer(typeof(BSBReserves))]
	public class BSBReservesInspector : PropertyDrawer
	{


		protected bool _foldout = false;
		protected float _itemHeight = 15.0f;

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

			var rect = position;
			rect.height = _itemHeight;

			EditorGUI.BeginChangeCheck();
			_foldout = EditorGUI.Foldout(rect, _foldout, label, true);
			if (EditorGUI.EndChangeCheck())
				EditorPrefs.SetBool(label.text, _foldout);

			if (!_foldout)
				return;

			var reserves = fieldInfo.GetValue(property.serializedObject.targetObject) as IBSBReserves;

			Undo.RecordObject(property.serializedObject.targetObject, "Change");

			rect.x += 10.0f;
			rect.y += rect.height;
			_MeansField(rect, reserves.funds, "Funds");
			rect.y += rect.height;
			_MeansField(rect, reserves.materials, "Materials");
			rect.y += rect.height;
			_MeansField(rect, reserves.workers, "Workers");

			property.serializedObject.ApplyModifiedProperties();
		}

		protected void _MeansField(Rect rect, IBSBReserveMeans means, string label)
		{
			means.Set(
				EditorGUI.IntField(rect, new GUIContent(label), means.amount));
		}

	}

}