using System.Collections.Generic;
using UnityEngine;

public class PlayerCommander : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask selectionLayers;
    [SerializeField] private Selection selection;

    Vector3 mouseClickBegin;
    Vector3 mousePointCurrent;
    bool pointingAtGround;

    Entity pointingAt = null;
    bool pointingAtEntity;

    public PlayerController PlayerController;

    private void Awake() => selection.layers = selectionLayers;
    private void Start()
    {
        CustomInput.Instance.OnLeftMouseDown += StartUnitSelection;
        CustomInput.Instance.OnLeftMouseUp += EndUnitSelection;

        CustomInput.Instance.OnRightMouseDown += MoveAndAttackCommand;
    }
    private void Update()
    {
        GetMousePoint();
    }
    private void LateUpdate()
    {
        selection.gameObject.SetActive(CustomInput.Instance.leftMouseDown);
    }

    private void GetMousePoint()
    {
        RaycastHit hitOne, hitTwo;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        pointingAtGround = Physics.Raycast(ray, out hitOne, 100, groundLayer, QueryTriggerInteraction.Ignore);
        mousePointCurrent = hitOne.point;

        pointingAtEntity = Physics.Raycast(ray, out hitTwo, 100, selectionLayers, QueryTriggerInteraction.Ignore);
        if (hitTwo.transform) hitTwo.transform.TryGetComponent(out pointingAt);

        selection.SetMousePositions(mouseClickBegin, mousePointCurrent);
    }

    public void MoveAndAttackCommand()
    {
        if (pointingAtEntity)
        {
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.Instance.shiftDown) e.commands.Clear();

                if (pointingAt.team == Team.player)     //ally
                {
                    e.commands.Add(new FollowCommand(e, pointingAt));
                }
                else if (pointingAt.team == Team.boss)  //enemy
                {
                    e.commands.Add(new AttackCommand(e, pointingAt));
                }
            }
        }
        else if (pointingAtGround)
        {
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.Instance.shiftDown) e.commands.Clear();
                e.commands.Add(new MoveCommand(e, mousePointCurrent));
            }
        }
    }
    public void StartUnitSelection()
    {
        if (pointingAtGround)
            mouseClickBegin = mousePointCurrent;
        else return;
    }
    public void EndUnitSelection()
    {
        List<Entity> entities = selection.Select();

        if (entities.Count == 0 &&
            pointingAtEntity &&
            pointingAt.transform != null &&
            pointingAt.transform.TryGetComponent<Entity>(out var entity))
            entities.Add(entity);

        if (!CustomInput.Instance.shiftDown)
        {
            PlayerController.UnselectAllUnits();
        }

        ToggleSelection(entities);
    }

    private void ToggleSelection(List<Entity> entities)
    {
        foreach (var item in entities)
        {
            if (item.team != Team.player) continue;

            if (CustomInput.Instance.shiftDown)
            {
                switch (PlayerController.selectedUnits.Contains(item))
                {
                    case true:
                        PlayerController.UnselectUnit(item);
                        break;
                    case false:
                        PlayerController.SelectUnit(item);
                        break;
                }
                continue;
            }

            if (!PlayerController.selectedUnits.Contains(item))
                PlayerController.SelectUnit(item);
        }
    }
}
