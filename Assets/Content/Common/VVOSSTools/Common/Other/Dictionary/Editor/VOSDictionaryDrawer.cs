using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;


public abstract class VOSDictionaryDrawer<TK, TV> : PropertyDrawer
{

	public virtual float itemHeight
	{
		get { return 17f; }
	}

	protected VOSSerializableDictionary<TK, TV> _dictionary;
	protected bool _foldout;
	protected const float _kButtonWidth = 18f;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		_CheckInitialize(property, label);
		if (_foldout)
			return (_dictionary.Count + 1) * itemHeight;
		return itemHeight;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorUtility.SetDirty(property.serializedObject.targetObject);
		_CheckInitialize(property, label);

		position.height = itemHeight;

		var foldoutRect = position;
		foldoutRect.width -= 2 * _kButtonWidth;
		EditorGUI.BeginChangeCheck();
		_foldout = EditorGUI.Foldout(foldoutRect, _foldout, label, true);
		if (EditorGUI.EndChangeCheck())
			EditorPrefs.SetBool(label.text, _foldout);

		var buttonRect = position;
		buttonRect.height = _kButtonWidth;
		buttonRect.x = position.width - _kButtonWidth + position.x;
		buttonRect.width = _kButtonWidth + 2;

		if (GUI.Button(buttonRect, new GUIContent("+", "Add item"), EditorStyles.miniButton))
		{
			_RecordProperty(property);
			_AddNewItem();
		}

		buttonRect.x -= _kButtonWidth;

/*		if (GUI.Button(buttonRect, new GUIContent("X", "Clear dictionary"), EditorStyles.miniButtonRight))
		{
			_ClearDictionary();
		}
		*/
		if (!_foldout)
			return;

		foreach (var item in _dictionary)
		{
			var key = item.Key;
			var value = item.Value;

			position.y += itemHeight;

			var keyRect = position;
			keyRect.width /= 2;
			keyRect.width -= 4;
			EditorGUI.BeginChangeCheck();
			var newKey = _KeyField(keyRect, typeof(TK), key);
			if (EditorGUI.EndChangeCheck())
			{
				_RecordProperty(property);
				try
				{
					_dictionary.Remove(key);
					_dictionary.Add(newKey, value);
				}
				catch (Exception e)
				{
					Debug.Log(e.Message);
				}
				break;
			}

			var valueRect = position;
			valueRect.x = position.width / 2 + 15;
			valueRect.width = keyRect.width - _kButtonWidth;
			EditorGUI.BeginChangeCheck();
			value = _ValueField(valueRect, typeof(TV), value);
			if (EditorGUI.EndChangeCheck())
			{
				_RecordProperty(property);
				_dictionary[key] = value;
				break;
			}

			var removeRect = valueRect;
			removeRect.height = _kButtonWidth;	
			removeRect.x = valueRect.xMax + 2;
			removeRect.width = _kButtonWidth;
			if (GUI.Button(removeRect, new GUIContent("x", "Remove item"), EditorStyles.miniButtonRight))
			{
				_RecordProperty(property);
				_RemoveItem(key);
				break;
			}
		}

		property.serializedObject.ApplyModifiedProperties();
	}

	protected void _RecordProperty(SerializedProperty property)
	{
		Undo.RecordObject(property.serializedObject.targetObject, string.Format("Changed {0}", property.name));
	}

	protected void _RemoveItem(TK key)
	{
		_dictionary.Remove(key);
	}

	protected void _CheckInitialize(SerializedProperty property, GUIContent label)
	{
		if (_dictionary == null)
		{
			var target = property.serializedObject.targetObject;
			_dictionary = fieldInfo.GetValue(target) as VOSSerializableDictionary<TK, TV>;
			if (_dictionary == null)
			{
				_dictionary = new VOSSerializableDictionary<TK, TV>();
				fieldInfo.SetValue(target, _dictionary);
			}

			_foldout = EditorPrefs.GetBool(label.text);
		}
	}

	protected static readonly Dictionary<Type, Func<Rect, object, object>> _fields =
		new Dictionary<Type, Func<Rect, object, object>>()
		{
		{ typeof(int), (rect, value) => EditorGUI.IntField(rect, (int)value) },
		{ typeof(float), (rect, value) => EditorGUI.FloatField(rect, (float)value) },
		{ typeof(string), (rect, value) => EditorGUI.TextField(rect, (string)value) },
		{ typeof(bool), (rect, value) => EditorGUI.Toggle(rect, (bool)value) },
		{ typeof(Vector2), (rect, value) => EditorGUI.Vector2Field(rect, GUIContent.none, (Vector2)value) },
		{ typeof(Vector3), (rect, value) => EditorGUI.Vector3Field(rect, GUIContent.none, (Vector3)value) },
		{ typeof(Bounds), (rect, value) => EditorGUI.BoundsField(rect, (Bounds)value) },
		{ typeof(Rect), (rect, value) => EditorGUI.RectField(rect, (Rect)value) }
		};

	protected static T _ItemField<T>(Rect rect, Type type, T value)
	{
		Func<Rect, object, object> field;
		if (_fields.TryGetValue(type, out field))
			return (T)field(rect, value);

		if (type.IsEnum)
			return (T)(object)EditorGUI.EnumPopup(rect, (Enum)(object)value);

		if (typeof(UnityObject).IsAssignableFrom(type))
			return (T)(object)EditorGUI.ObjectField(rect, (UnityObject)(object)value, type, true);

		Debug.Log("Type is not supported: " + type);
		return value;
	}

	protected virtual TV _ValueField(Rect rect, Type type, TV value)
	{
		return _ItemField(rect, type, value);
	}

	protected virtual TK _KeyField(Rect rect, Type type, TK value)
	{
		return _ItemField(rect, type, value);
	}

	protected void _ClearDictionary()
	{
		_dictionary.Clear();
	}

	protected void _AddNewItem()
	{
		TK key;
		if (typeof(TK) == typeof(string))
			key = (TK)(object)"";
		else key = default(TK);

		var value = default(TV);
		try
		{
			_dictionary.Add(key, value);
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
		}
	}
}

//	[CustomPropertyDrawer(typeof(Monstones.LevelEditor.MSLevelEditor.TestD))]
//	public class VOSDictionaryIntIntDrawer : VOSDictionaryDrawer<int, int> { }