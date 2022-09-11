using Shapes;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Entity
{
    public Sprite unitIcon;

    public GameObject selectionCircle;
    [HideInInspector] public Disc selectionCircleGFX;
    [HideInInspector] public Transform selectionCircleTrans;

    public List<Spell> spells;

    private void Awake()
    {
        var parent = new GameObject(name);
        parent.transform.parent = transform.parent;
        transform.parent = parent.transform;
        selectionCircle = Instantiate(selectionCircle);
        selectionCircle.transform.parent = transform.parent;
        selectionCircleGFX = selectionCircle.GetComponent<Disc>();
        selectionCircleTrans = selectionCircle.transform;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        MoveSelectionCircle();
        RotateSelectionCircle();
    }

    private void MoveSelectionCircle()
    {
        selectionCircleTrans.position = transform.position;
    }

    void RotateSelectionCircle() 
    {
        if (selectionCircle.activeInHierarchy) selectionCircleGFX.DashOffset += Time.fixedDeltaTime * 1;
    }
}
