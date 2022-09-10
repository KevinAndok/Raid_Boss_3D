using System.Collections.Generic;
using UnityEngine;

public class PlayerCommander : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask selectionLayers;
    [SerializeField] private BoxSelection selection;

    Vector3 mouseClickBegin;
    Vector3 mousePointCurrent;

    public static Vector3 pointingAtGround = Vector3.zero;
    public static Entity pointingAtEntity = null;
    public static bool isPointingAtGround;
    public static bool isPointingAtEntity;

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

        isPointingAtGround = Physics.Raycast(ray, out hitOne, 100, groundLayer, QueryTriggerInteraction.Ignore);
        mousePointCurrent = hitOne.point;

        isPointingAtEntity = Physics.Raycast(ray, out hitTwo, 100, selectionLayers, QueryTriggerInteraction.Ignore);
        if (hitTwo.transform) hitTwo.transform.TryGetComponent(out pointingAtEntity);

        selection.SetMousePositions(mouseClickBegin, mousePointCurrent);
    }

    public void MoveAndAttackCommand()
    {
        if (pointingAtEntity)
        {
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.Instance.shiftDown) e.StopAllCommands();

                if (pointingAtEntity.team == Team.player)     //ally
                {
                    e.commands.Add(new FollowCommand(e, pointingAtEntity));
                }
                else if (pointingAtEntity.team == Team.boss)  //enemy
                {
                    e.commands.Add(new AttackCommand(e, pointingAtEntity));
                }
            }
        }
        else if (isPointingAtGround)
        {
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.Instance.shiftDown) e.StopAllCommands();
                e.commands.Add(new MoveCommand(e, mousePointCurrent));
            }
        }
    }
    public void StartUnitSelection()
    {
        if (isPointingAtGround)
            mouseClickBegin = mousePointCurrent;
        else return;
    }
    public void EndUnitSelection()
    {
        List<PlayerUnit> entities = selection.Select();

        if (entities.Count == 0 &&
            pointingAtEntity &&
            pointingAtEntity.transform != null &&
            pointingAtEntity.transform.TryGetComponent<PlayerUnit>(out var entity))
            entities.Add(entity);

        if (!CustomInput.Instance.shiftDown)
        {
            PlayerController.UnselectAllUnits();
        }

        ToggleSelection(entities);
    }

    private void ToggleSelection(List<PlayerUnit> entities)
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

    //Work in Progress
    public void CastSpell(int spellIndex)
    {
        var cmd = (object)PlayerController.selectedUnits[0].spells[spellIndex].command.GetClass();
        if (cmd is CastSpell spell)
        {
            PlayerController.selectedUnits[0].StopAllCommands();
            PlayerController.selectedUnits[0].commands.Add(spell.GetCommand(PlayerController.selectedUnits[0]));
        }
    }
}
