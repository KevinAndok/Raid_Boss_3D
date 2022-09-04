using UnityEngine;
using UnityEngine.UI;

public class CustomContentFitter : MonoBehaviour
{
    public RectTransform rtTransform;
    public float insidePadding;
    public int outsidePadding;
    public HorizontalLayoutGroup horizontalLayoutGroup;

    public RectTransform[] children;

    private void OnValidate()
    {
        SetSize();
        horizontalLayoutGroup.spacing = insidePadding;
        horizontalLayoutGroup.padding = new RectOffset(outsidePadding, outsidePadding, 0, 0);
    }

    public void SetSize() //called whenever the number of abilities changes
    {
        int activeChildren = 0;
        float childWidth = 0;
        foreach (var trans in children)
        {
            if (!trans.gameObject.activeInHierarchy) continue;
            childWidth += trans.sizeDelta.x;
            activeChildren++;
        }
        rtTransform.sizeDelta = new Vector2(childWidth + (activeChildren - 1) * insidePadding + 2 * outsidePadding, rtTransform.sizeDelta.y);
    }
}
