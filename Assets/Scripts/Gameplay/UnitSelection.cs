using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public PlayerController controller;

    [SerializeField] private LayerMask _unitLayers;
    [SerializeField] private Color _selectionBoxColorInner;
    [SerializeField] private Color _selectionBoxColorOuter;

    private bool _isDraggingMouseBox = false;
    private Vector3 _dragStartPosition;
    private bool _isPointingAtEntity;
    private Entity _mouseOverEntity;

    private void OnEnable()
    {
        CustomInput.OnLeftMouseDown += LeftMouseDown;
        CustomInput.OnLeftMouseUp += LeftMouseUp;
    }
    private void OnDisable()
    {
        CustomInput.OnLeftMouseDown -= LeftMouseDown;
        CustomInput.OnLeftMouseUp -= LeftMouseUp;
    }
    void OnGUI()
    {
        if (_isDraggingMouseBox)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(_dragStartPosition, Input.mousePosition);
            Utils.DrawScreenRect(rect, _selectionBoxColorInner);
            Utils.DrawScreenRectBorder(rect, 1, _selectionBoxColorOuter);
        }
    }

    private void LeftMouseDown()
    {
        _isDraggingMouseBox = true;
        _dragStartPosition = Input.mousePosition;
    }
    private void LeftMouseUp()
    {
        CheckIfPointingAtEntity();

        if (!CustomInput.shiftDown) controller.DeselectAllUnits();

        if (_isDraggingMouseBox && _dragStartPosition != Input.mousePosition)
        {
            _SelectUnitsInDraggingBox();
        }

        else if (_isPointingAtEntity)
        {
            if (_mouseOverEntity.TryGetComponent<PlayerUnit>(out var playerUnit))
            {
                controller.SelectUnit(playerUnit);
            }
        }

        _isDraggingMouseBox = false;
    }

    void CheckIfPointingAtEntity()
    {
        if (_isPointingAtEntity =
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
            out var hit, 100, _unitLayers, QueryTriggerInteraction.Collide))
            hit.transform.TryGetComponent(out _mouseOverEntity);
    }

    private void _SelectUnitsInDraggingBox()
    {
        Bounds selectionBounds = Utils.GetViewportBounds(
            Camera.main,
            _dragStartPosition,
            Input.mousePosition
        );
        
        bool inBounds;
        foreach (Entity unit in EntityManager.AllEntities)
        {
            if (unit.TryGetComponent<PlayerUnit>(out var playerUnit))
            {
                inBounds = selectionBounds.Contains(
                    Camera.main.WorldToViewportPoint(unit.transform.position)
                );
                if (inBounds) controller.SelectUnit(playerUnit);
                //else
                //    controller.DeselectUnit(playerUnit);
            }
            
        }
    }
}