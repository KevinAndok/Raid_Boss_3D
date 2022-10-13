using UnityEngine;

public class StatusBar : MonoBehaviour
{
    public Transform barParent;

    public Transform healthBar;
    public Transform manaBar;
    public Transform castBar;

    public void SetHealth(float value) => healthBar.localScale = new Vector3(Mathf.Clamp01(value), 1, 1);
    public void SetMana(float value) => manaBar.localScale = new Vector3(Mathf.Clamp01(value), 1, 1);
    public void SetCast(float? value)
    {
        castBar.parent.gameObject.SetActive(value!=null);
        if (value == null) return;
        castBar.localScale = new Vector3(Mathf.Clamp01((float)value), 1, 1);
    }
}
