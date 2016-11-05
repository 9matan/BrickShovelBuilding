using UnityEngine;
using System.Collections;

public static class VOSRectTransformExtention
{

	//
	// < Stretch >
	//

	public static void FullStretch(this RectTransform tr)
	{
		VerticalStretch (tr);
		HorizontalStretch (tr);
	}

	public static void VerticalStretch(this RectTransform tr)
	{
		var parent = (RectTransform)tr.parent;

		tr.SetInsetAndSizeFromParentEdge (
			RectTransform.Edge.Bottom, 0.0f, parent.sizeDelta.y);
		tr.SetInsetAndSizeFromParentEdge (
			RectTransform.Edge.Top, 0.0f, parent.sizeDelta.y);

		SetDefaultScale (tr);
		SetDefaultZ (tr);
	}

	public static void HorizontalStretch(this RectTransform tr)
	{
		var parent = (RectTransform)tr.parent;
		
		tr.SetInsetAndSizeFromParentEdge (
			RectTransform.Edge.Left, 0.0f, parent.sizeDelta.x);
		tr.SetInsetAndSizeFromParentEdge (
			RectTransform.Edge.Right, 0.0f, parent.sizeDelta.x);
		
		SetDefaultScale (tr);
		SetDefaultZ (tr);
	}

	//
	// </ Stretch >
	//

	public static void SetDefault(this RectTransform trans)
	{
		SetDefaultScale (trans);
		SetDefaultZ (trans);
	}

	public static void SetDefaultZ(this RectTransform trans)
	{
		trans.localPosition = new Vector3 (
			trans.localPosition.x,
			trans.localPosition.y,
			0.0f
		);
	}

	public static void SetDefaultScale(this RectTransform trans) 
	{
		trans.localScale = new Vector3(1, 1, 1);
	}

	public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec) 
	{
		trans.pivot = aVec;
		trans.anchorMin = aVec;
		trans.anchorMax = aVec;
	}
	
	public static Vector2 GetSize(this RectTransform trans) {
		return trans.rect.size;
	}
	public static float GetWidth(this RectTransform trans) 
	{
		return trans.rect.width;
	}

	public static float GetHeight(this RectTransform trans) 
	{
		return trans.rect.height;
	}
	
	public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos) 
	{
		trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
	}
	
	public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos) 
	{
		trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
	}

	public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos) 
	{
		trans.localPosition = new Vector3(newPos.x + (trans.pivot.x * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
	}

	public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos) 
	{
		trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y + (trans.pivot.y * trans.rect.height), trans.localPosition.z);
	}

	public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos) 
	{
		trans.localPosition = new Vector3(newPos.x - ((1f - trans.pivot.x) * trans.rect.width), newPos.y - ((1f - trans.pivot.y) * trans.rect.height), trans.localPosition.z);
	}
	
	public static void SetSize(this RectTransform trans, Vector2 newSize) 
	{
		Vector2 oldSize = trans.rect.size;
		Vector2 deltaSize = newSize - oldSize;
		trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
		trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
	}

	public static void SetWidth(this RectTransform trans, float newSize) 
	{
		SetSize(trans, new Vector2(newSize, trans.rect.size.y));
	}

	public static void SetHeight(this RectTransform trans, float newSize) 
	{
		SetSize(trans, new Vector2(trans.rect.size.x, newSize));
	}
}
