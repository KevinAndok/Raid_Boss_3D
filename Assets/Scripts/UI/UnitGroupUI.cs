using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitGroupUI : MonoBehaviour
{
    public TMP_Text unitCount;
    public Image unitIcon;

    public void SetGroup(Sprite icon, int count)
    {
        if (count == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);

        unitCount.text = count.ToString();
        unitIcon.sprite = icon;
    }

    private void OnDisable()
    {
        unitCount.text = "0";
        unitIcon.sprite = null;
    }
}
