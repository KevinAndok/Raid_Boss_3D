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

        CustomInput.Instance.OnRightMouseDown += StartMoveCommand;
    }

    public void StartMoveCommand()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, groundLayer, QueryTriggerInteraction.Ignore))
            foreach (Entity e in PlayerController.Instance.selectedUnits)
                e.commands.Add(new MoveCommand(e, hit.point));
    }
    public void StartUnitSelection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, selectionLayers, QueryTriggerInteraction.Ignore)) 
            PlayerController.Instance.selectedUnits.Add(hit.transform.GetComponent<Entity>());

        //else if (Physics.Raycast(ray, out hit, 100, groundLayer, QueryTriggerInteraction.Ignore)) 
        //    selectionBegin = hit.point;
    }
    public void EndUnitSelection()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100, groundLayer, QueryTriggerInteraction.Ignore))
        {
            Vector3 selectionEnd = hit.point;

        }
    }
}
