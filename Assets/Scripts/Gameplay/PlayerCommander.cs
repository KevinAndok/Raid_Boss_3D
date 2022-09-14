using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCommander : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask selectionLayers;
    [SerializeField] private BoxSelection selection;

    Vector3 mouseClickBegin;

    public static Vector3 mouseGroundPoint = Vector3.zero;
    public static Entity mouseEntityPoint = null;
    public static bool isPointingAtGround;
    public static bool isPointingAtEntity;

    public PlayerController PlayerController;

    private void Awake() => selection.layers = selectionLayers;
    private void Start()
    {
        CustomInput.Instance.OnLeftMouseDown += StartUnitSelection;
        CustomInput.Instance.OnLeftMouseUp += EndUnitSelection;

        CustomInput.Instance.OnRightMouseDown += MoveAndAttackCommand;

        CustomInput.Instance.OnQDown += () => CastSpell(0);
        CustomInput.Instance.OnWDown += () => CastSpell(1);
        CustomInput.Instance.OnEDown += () => CastSpell(2);
        CustomInput.Instance.OnRDown += () => CastSpell(3);

        CustomInput.Instance.OnADown += () => CastSpell(4);
        CustomInput.Instance.OnSDown += () => CastSpell(5);
        CustomInput.Instance.OnDDown += () => CastSpell(6);
        CustomInput.Instance.OnFDown += () => CastSpell(7);

        CustomInput.Instance.OnZDown += () => CastSpell(8);
        CustomInput.Instance.OnXDown += () => CastSpell(9);
        CustomInput.Instance.OnCDown += () => CastSpell(10);
        CustomInput.Instance.OnVDown += () => CastSpell(11);
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
        mouseGroundPoint = hitOne.point;

        isPointingAtEntity = Physics.Raycast(ray, out hitTwo, 100, selectionLayers, QueryTriggerInteraction.Ignore);
        if (hitTwo.transform) hitTwo.transform.TryGetComponent(out mouseEntityPoint);

        selection.SetMousePositions(mouseClickBegin, mouseGroundPoint);
    }

    public void MoveAndAttackCommand()
    {
        if (isPointingAtEntity)
        {
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.Instance.shiftDown) e.StopAllCommands();

                if (mouseEntityPoint.team == Team.player)     //ally
                {
                    e.commands.Add(new FollowCommand(e, mouseEntityPoint));
                }
                else if (mouseEntityPoint.team == Team.boss)  //enemy
                {
                    e.commands.Add(new AttackCommand(e, mouseEntityPoint));
                }
            }
        }
        else if (isPointingAtGround)
        {
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.Instance.shiftDown) e.StopAllCommands();
                e.commands.Add(new MoveCommand(e, mouseGroundPoint));
            }
        }
    }
    public void StartUnitSelection()
    {
        if (isPointingAtGround)
            mouseClickBegin = mouseGroundPoint;
        else return;
    }
    public void EndUnitSelection()
    {
        List<PlayerUnit> entities = selection.Select();

        if (entities.Count == 0 &&
            isPointingAtEntity &&
            mouseEntityPoint.transform != null &&
            mouseEntityPoint.transform.TryGetComponent<PlayerUnit>(out var entity))
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

    public void CastSpell(int spellIndex)
    {
        var selectedUnit = PlayerController.selectedUnits[0];
        var unitCommands = selectedUnit.commands;
        var spell = selectedUnit.spells[spellIndex];

        if (spell.lastCastTime > Time.time + spell.cooldown) return; //spell on cooldown, we cannot cast it

        if (!CustomInput.Instance.shiftDown || unitCommands[0].GetType() == typeof(WaitCommand))
            selectedUnit.StopAllCommands();

        //todo: start mouse indicator
        //todo: start cooldown
        unitCommands.Add(selectedUnit.spells[spellIndex].GetCommand(selectedUnit));
    }
}
