using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerCommander : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    //[SerializeField] private LayerMask selectionLayers;
    //[SerializeField] private BoxSelection selection;

    public static Vector3 mouseGroundPoint = Vector3.zero;
    public static Entity mouseEntityPoint = null;
    public static bool isPointingAtGround;
    public static bool isPointingAtEntity;

    public PlayerController PlayerController;

    //private void Awake() => selection.layers = selectionLayers;

    #region InputEventsRegister
    private void OnEnable()
    {
        ////CustomInput.OnLeftMouseDown += StartUnitSelection;
        ////CustomInput.OnLeftMouseUp += EndUnitSelection;

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
        //selection.gameObject.SetActive(CustomInput.leftMouseDown);
    }

    private void GetMousePoint()
    {
        RaycastHit hitOne;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        isPointingAtGround = Physics.Raycast(ray, out hitOne, 100, groundLayer, QueryTriggerInteraction.Collide);
        mouseGroundPoint = hitOne.point;

        //selection.SetMousePositions(mouseClickBegin, mouseGroundPoint);
    }

    public void MoveAndAttackCommand()
    {
        if (isPointingAtEntity)
        {
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.shiftDown) e.StopAllCommands();

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
            //var dir = (PlayerController.selectedUnits[0].transform.position - mouseGroundPoint).normalized;

            //for (int i = 0; i < PlayerController.selectedUnits.Count; i++)
            //{
            //    var unit = PlayerController.selectedUnits[i];

            //    if (!CustomInput.shiftDown) unit.StopAllCommands();

            //    var offsetX = Mathf.RoundToInt(i / 2f); //ceil?
            //    var offsetY = Mathf.RoundToInt(i / 5f); //ceil?

            //    Vector3 left = Vector3.Cross(dir, Vector3.up).normalized;

            //    unit.commands.Add(new MoveCommand(unit, mouseGroundPoint + (i % 2 == 1 ? -1 : 1) * offsetX * left + dir * offsetY));


            //}
            foreach (Entity e in PlayerController.selectedUnits)
            {
                if (!CustomInput.shiftDown) e.StopAllCommands();
                e.commands.Add(new MoveCommand(e, mouseGroundPoint));
            }
        }
    }
    public void CastSpell(int spellIndex)
    {
        var selectedUnit = PlayerController.selectedUnits[0];
        var unitCommands = selectedUnit.commands;
        var spell = selectedUnit.spells[spellIndex];

        if (spell.lastCastTime > Time.time + spell.cooldown) return; //spell on cooldown, we cannot cast it

        if (!CustomInput.shiftDown || unitCommands[0].GetType() == typeof(WaitCommand))
            selectedUnit.StopAllCommands();

        //todo: start mouse indicator
        //todo: start cooldown
        unitCommands.Add(selectedUnit.spells[spellIndex].GetCommand(selectedUnit));
    }
}
