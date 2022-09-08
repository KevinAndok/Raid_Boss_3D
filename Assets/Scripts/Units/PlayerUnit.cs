using Shapes;
using UnityEngine;

public class PlayerUnit : Entity
{
    public Sprite unitIcon;

    public GameObject selectionCircle;
    public Disc selectionCircleGFX;

    private void Awake()
    {
        //selectionCircle = Instantiate(selectionCircle);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        RotateSelectionCircle();
    }

    void RotateSelectionCircle() 
    {
        if (selectionCircle.activeInHierarchy) selectionCircleGFX.DashOffset += Time.fixedDeltaTime * .5f;
    }
}
