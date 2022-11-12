using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellIconUI : MonoBehaviour
{
    public Image image;
    public TMP_Text cooldown;
    public TMP_Text shortcut;

    Spell currentSpell;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetSlot(Spell spell)
    {
        currentSpell = spell;
        image.sprite = currentSpell.spellIcon;
        image.enabled = true;

        cooldown.gameObject.SetActive(true);
        shortcut.gameObject.SetActive(true);
    }
    public void ClearSlot()
    {
        currentSpell = null;
        image.sprite = null;
        image.enabled = false;

        cooldown.gameObject.SetActive(false);
        shortcut.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentSpell == null) return;
        //cooldown
        cooldown.gameObject.SetActive(currentSpell.lastCastTime + currentSpell.cooldown > Time.time);
        if (cooldown.gameObject.activeInHierarchy)
            cooldown.text = $"{Mathf.CeilToInt(currentSpell.lastCastTime + currentSpell.cooldown - Time.time)}";
    }
    private void OnMouseEnter()
    {
        //todo mouse hover tooltip start
    }
    private void OnMouseExit()
    {
        //todo mouse hover tooltip stop
    }
}
