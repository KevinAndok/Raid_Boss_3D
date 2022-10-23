using System;
using UnityEngine;

public sealed class PlayerCommander : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask entityLayer;

    public static Vector3 mouseGroundPoint = Vector3.zero;
    public static Entity mouseEntityPoint = null;
    public static bool isPointingAtGround;
    public static bool isPointingAtEntity;

    public PlayerController PlayerController;

    #region InputEventsRegister
    private void OnEnable()
    {
        //CustomInput.OnLeftMouseDown += StartUnitSelection;
        //CustomInput.OnLeftMouseUp += EndUnitSelection;

        CustomInput.OnRightMouseDown += MoveAndAttackCommand;

        CustomInput.OnQDown += () => CastSpell(0);
        CustomInput.OnWDown += () => CastSpell(1);
        CustomInput.OnEDown += () => CastSpell(2);
        CustomInput.OnRDown += () => CastSpell(3);

        CustomInput.OnADown += () => CastSpell(4);
        CustomInput.OnSDown += () => CastSpell(5);
        CustomInput.OnDDown += () => CastSpell(6);
        CustomInput.OnFDown += () => CastSpell(7);

        CustomInput.OnZDown += () => CastSpell(8);
        CustomInput.OnXDown += () => CastSpell(9);
        CustomInput.OnCDown += () => CastSpell(10);
        CustomInput.OnVDown += () => CastSpell(11);
    }
    private void OnDisable()
    {
        //CustomInput.OnLeftMouseDown -= StartUnitSelection;
        //CustomInput.OnLeftMouseUp -= EndUnitSelection;

        CustomInput.OnRightMouseDown -= MoveAndAttackCommand;

        CustomInput.OnQDown -= () => CastSpell(0);
        CustomInput.OnWDown -= () => CastSpell(1);
        CustomInput.OnEDown -= () => CastSpell(2);
        CustomInput.OnRDown -= () => CastSpell(3);

        CustomInput.OnADown -= () => CastSpell(4);
        CustomInput.OnSDown -= () => CastSpell(5);
        CustomInput.OnDDown -= () => CastSpell(6);
        CustomInput.OnFDown -= () => CastSpell(7);

        CustomInput.OnZDown -= () => CastSpell(8);
        CustomInput.OnXDown -= () => CastSpell(9);
        CustomInput.OnCDown -= () => CastSpell(10);
        CustomInput.OnVDown -= () => CastSpell(11);
    }
    #endregion

    private void Update()
    {
        GetMousePoint();
    }
    private void LateUpdate()
    {

    }

    private void GetMousePoint()
    {
        RaycastHit hitOne;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        isPointingAtGround = Physics.Raycast(ray, out hitOne, 100, groundLayer, QueryTriggerInteraction.Collide);
        mouseGroundPoint = isPointingAtGround ? hitOne.point : Vector3.zero;

        isPointingAtEntity = Physics.Raycast(ray, out hitOne, 100, entityLayer, QueryTriggerInteraction.Collide);
        if (hitOne.collider) hitOne.collider.gameObject.TryGetComponent<Entity>(out mouseEntityPoint);
    }

    public void MoveAndAttackCommand()
    {
        if (isPointingAtEntity)
        {
            if (mouseEntityPoint.team == Team.player)     //ally
            {
                CommandUnits(typeof(FollowCommand));
            }
            else if (mouseEntityPoint.team == Team.boss)  //enemy
            {
                CommandUnits(typeof(AttackCommand));
            }
        }
        else if (isPointingAtGround)
        {
            //CommandUnits(object command)
            CommandUnits(typeof(MoveCommand));
        }
    }
    public void CastSpell(int spellIndex)
    {
        var selectedUnit = PlayerController.selectedUnits[0];
        var unitCommands = selectedUnit.commands;
        if (spellIndex >= selectedUnit.spells.Count) return;
        var spell = selectedUnit.spells[spellIndex];

        if (spell.lastCastTime > Time.time + spell.cooldown) return; //spell on cooldown, we cannot cast it

        if (!CustomInput.shiftDown || unitCommands[0].GetType() == typeof(WaitCommand))
            selectedUnit.StopAllCommands();

        //todo: start mouse indicator
        spell.lastCastTime = Time.time;
        unitCommands.Add(selectedUnit.spells[spellIndex].GetCommand(selectedUnit));
    }

    public void CommandUnits(Type command)
    {
        if (PlayerController.selectedUnits.Count == 0) return;

        if (CustomInput.altDown)
        {
            //all selected
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.shiftDown) e.StopAllCommands();
                IssueCommand(e, command);
            }
        }
        else
        {
            //only selected[0]
            var e = PlayerController.selectedUnits[0];
            if (!CustomInput.shiftDown) e.StopAllCommands();
            IssueCommand(e, command);
        }
    }
    public void IssueCommand(Entity e, Type command)
    {
        if (command == typeof(MoveCommand))
        {
            e.commands.Add(new MoveCommand(e, mouseGroundPoint));
        }
        else if (command == typeof(AttackCommand))
        {
            e.commands.Add(new AttackCommand(e, mouseEntityPoint));
        }
        else if (command == typeof(FollowCommand))
        {
            e.commands.Add(new FollowCommand(e, mouseEntityPoint));
        }
    }
}
