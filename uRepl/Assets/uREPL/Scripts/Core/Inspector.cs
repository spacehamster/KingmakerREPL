using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace uREPL
{

public class FieldItemInfo
{
	public string name;
	public Type type;
	public object value;
}

public class ComponentInfo
{
	public Component instance;
	public System.Type type;
	public string componentName;
	public string gameObjectPath;
	public List<FieldItemInfo> fields = new List<FieldItemInfo>();
}

public static class Inspector
{
	private const string outputPrefabDirPath = "uREPL/Prefabs/Output/";
	private const string fieldPrefabDirPath  = "uREPL/Prefabs/Field Items/";

	static private bool isInitialized = false;

	static private GameObject gameObjectViewPrefab;
	static private GameObject componentViewPrefab;

	static private GameObject boolItemPrefab;
	static private GameObject intItemPrefab;
	static private GameObject floatItemPrefab;
	static private GameObject stringItemPrefab;
	static private GameObject vector2ItemPrefab;
	static private GameObject vector3ItemPrefab;
	static private GameObject vector4ItemPrefab;
	static private GameObject enumItemPrefab;
	static private GameObject readonlyItemPrefab;

	static public void Initialize()
	{
		if (isInitialized) return;
		isInitialized = true;

		gameObjectViewPrefab = AssetHelper.Load<GameObject>(outputPrefabDirPath + "GameObject Item.prefab");
		componentViewPrefab  = AssetHelper.Load<GameObject>(outputPrefabDirPath + "Component Item.prefab");
		boolItemPrefab       = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "Bool Item.prefab");
		intItemPrefab        = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "Int Item.prefab");
		floatItemPrefab      = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "Float Item.prefab");
		stringItemPrefab     = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "String Item.prefab");
		vector2ItemPrefab    = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "Vector2 Item.prefab");
		vector3ItemPrefab    = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "Vector3 Item.prefab");
		vector4ItemPrefab    = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "Vector4 Item.prefab");
		enumItemPrefab       = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "Enum Item.prefab");
		readonlyItemPrefab   = AssetHelper.Load<GameObject>(fieldPrefabDirPath + "Readonly Item.prefab");
	}

	[Command(name = "inspect", description = "inspect GameObject.")]
	static public void Inspect(GameObject gameObject)
	{
		if (gameObject == null) {
			Log.Warn("given GameObject is null.");
			return;
		}

		Utility.RunOnNextFrame(() => {
			var item = Window.InstantiateInOutputView(gameObjectViewPrefab).GetComponent<GameObjectItem>();;
			item.targetGameObject = gameObject;
			item.title = gameObject.name;
		});
	}

	[Command(name = "inspect", description = "inspect Component.")]
	static public void Inspect<T>(T component) where T : Component
	{
		if (component == null) {
			Log.Warn("given component is null.");
			return;
		}
		Inspect(component, typeof(T));
	}

	static public void Inspect(Component component, Type componentType)
	{
		var componentInfo = new ComponentInfo();
		componentInfo.instance       = component;
		componentInfo.type           = componentType;
		componentInfo.componentName  = componentType.FullName;
		componentInfo.gameObjectPath = component.transform.GetPath();

		var fields = componentType.GetFields(BindingFlags.Public | BindingFlags.Instance);
		foreach (var field in fields) {
			var type = field.FieldType;
			var info = new FieldItemInfo();
			info.type = type;
			info.name = field.Name;
			info.value = field.GetValue(component);
			componentInfo.fields.Add(info);
		}

		Utility.RunOnNextFrame(() => {
			Output(componentInfo);
		});
	}

	static public void Output(ComponentInfo component)
	{
		var item= Window.InstantiateInOutputView(componentViewPrefab).GetComponent<ComponentItem>();
		item.component = component.instance;
		item.title = string.Format("<b><i>{0}</i></b> ({1})",
			component.componentName, component.gameObjectPath);
		foreach (var field in component.fields) {
			InstantiateFieldItem(component, item, field);
		}
	}

	static private void InstantiateFieldItem(
			ComponentInfo component, ComponentItem view, FieldItemInfo field)
	{
		GameObject obj = null;

		if (field.type == typeof(bool)) {
			obj = UnityEngine.Object.Instantiate(boolItemPrefab);
		} else if (field.type == typeof(int)) {
			obj = UnityEngine.Object.Instantiate(intItemPrefab);
		} else if (field.type == typeof(float)) {
			obj = UnityEngine.Object.Instantiate(floatItemPrefab);
		} else if (field.type == typeof(string)) {
			obj = UnityEngine.Object.Instantiate(stringItemPrefab);
		} else if (field.type == typeof(Vector2)) {
			obj = UnityEngine.Object.Instantiate(vector2ItemPrefab);
		} else if (field.type == typeof(Vector3)) {
			obj = UnityEngine.Object.Instantiate(vector3ItemPrefab);
		} else if (field.type == typeof(Vector4)) {
			obj = UnityEngine.Object.Instantiate(vector4ItemPrefab);
		} else if (field.type == typeof(Quaternion)) {
			obj = UnityEngine.Object.Instantiate(vector4ItemPrefab);
		} else if (field.type.IsEnum) {
			obj = UnityEngine.Object.Instantiate(enumItemPrefab);
		} else {
			obj = UnityEngine.Object.Instantiate(readonlyItemPrefab);
		}

		var item = obj.GetComponent<FieldItem>();
		item.label         = field.name;
		item.component     = component.instance;
		item.componentType = component.type;
		item.fieldName     = field.name;
		item.fieldType     = field.type;

		view.Add(obj);
	}
}

}
