using System.Collections.Generic;
using UnityEngine;

public class PlayerCommander : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask selectionLayers;

    Vector3 selectionBegin = Vector3.zero;

    private void Start()
    {
        CustomInput.Instance.OnLeftMouseDown += StartUnitSelection;
        CustomInput.Instance.OnLeftMouseUp += EndUnitSelection;

        CustomInput.Instance.OnRightMouseDown += MoveAndAttackCommand;
    }

    public void MoveAndAttackCommand()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, selectionLayers, QueryTriggerInteraction.Ignore))
        {
            var entity = hit.transform.GetComponent<Entity>();

            foreach (Entity e in PlayerController.Instance.selectedUnits)
            {
                if (!CustomInput.Instance.shiftDown) e.commands.Clear();
                if (entity.team == Team.player) //ally
                {
                    e.commands.Add(new FollowCommand(e, entity));
                    return;
                }
                //TODO: add attack command if enemy
            }
        }
        else if (Physics.Raycast(ray, out hit, 100, groundLayer, QueryTriggerInteraction.Ignore))
        {
            foreach (Entity e in PlayerController.Instance.selectedUnits)
            {
                if (!CustomInput.Instance.shiftDown) e.commands.Clear();
                e.commands.Add(new MoveCommand(e, hit.point));
            }
        }
    }
    public void StartUnitSelection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, groundLayer, QueryTriggerInteraction.Ignore)) 
            selectionBegin = hit.point;
    }
    public void EndUnitSelection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, groundLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 selectionEnd = hit.point;
            Vector3 halfExtends = (selectionBegin - selectionEnd);
            halfExtends = new Vector3(Mathf.Abs(halfExtends.x), 10, Mathf.Abs(halfExtends.z));
            Vector3 middle = (selectionBegin - selectionEnd) / 2 + selectionEnd;

            RaycastHit[] hits = Physics.BoxCastAll(middle, halfExtends, Vector3.up, Quaternion.identity, 1, selectionLayers, QueryTriggerInteraction.Ignore);
            List<Entity> entities = new List<Entity>();

            if (hits.Length == 0)
            {
                hits = new RaycastHit[1];
                if (!Physics.Raycast(ray, out hits[0], 100, selectionLayers, QueryTriggerInteraction.Ignore))
                    hits = new RaycastHit[0];
            }

            foreach (RaycastHit target in hits)
            {
                if (target.transform.TryGetComponent<Entity>(out var entity))
                {
                    entities.Add(entity);
                }
            }

            if (!CustomInput.Instance.shiftDown) 
                PlayerController.Instance.selectedUnits.Clear();

            foreach (var item in entities)
                if (!PlayerController.Instance.selectedUnits.Contains(item))
                    PlayerController.Instance.selectedUnits.Add(item);
        }
    }
}
