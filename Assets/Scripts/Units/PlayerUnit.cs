using Shapes;
using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerUnit : Entity
{
    public Sprite unitIcon;

    public GameObject selectionCircle;
    [HideInInspector] public Disc selectionCircleGFX;
    [HideInInspector] public Transform selectionCircleTrans;

    public List<Spell> spells;

    protected override void Awake()
    {
        base.Awake();

        var parent = new GameObject(name);
        parent.transform.parent = transform.parent;
        transform.parent = parent.transform;
        selectionCircle = Instantiate(selectionCircle);
        selectionCircle.transform.parent = transform.parent;
        selectionCircleGFX = selectionCircle.GetComponent<Disc>();
        selectionCircleTrans = selectionCircle.transform;
    }

    protected override void Update()
    {
        MoveSelectionCircle();
        RotateSelectionCircle();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
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
