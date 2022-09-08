using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class BoxSelection : MonoBehaviour
{
    Vector3 mouseClickBegin;
    Vector3 mouseClickCurrent;

    [HideInInspector] public LayerMask layers;
    [HideInInspector] public Vector3 middle;
    [HideInInspector] public Vector3 size;

    [SerializeField] private Polygon selectionPolygon;
    [SerializeField] private Rectangle selectionRect;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider colliderBox;

    private void Update()
    {
        transform.position = middle;

        DrawSelectionBox();
    }

    public List<PlayerUnit> Select()
    {
        List<PlayerUnit> entities = new List<PlayerUnit>();

        foreach (var point in rb.SweepTestAll(Vector3.up))
        {
            if (point.transform.gameObject.TryGetComponent<PlayerUnit>(out var entity))
                entities.Add(entity);
        }

        return entities;
    }

    public void SetMousePositions(Vector3 begin, Vector3 current)
    {
        if (CustomInput.Instance.leftMouseDown) mouseClickBegin = begin;
        else mouseClickBegin = current;
        mouseClickCurrent = current;
    }

    void DrawSelectionBox()
    {
        selectionRect.gameObject.SetActive(true);

        size = (mouseClickBegin - mouseClickCurrent);
        size = new Vector3(Mathf.Abs(size.x), 10, Mathf.Abs(size.z));
        middle = (mouseClickBegin - mouseClickCurrent) / 2 + mouseClickCurrent + Vector3.up * .1f;

        selectionRect.transform.position = middle;
        selectionRect.Width = size.x;
        selectionRect.Height = size.z;

        colliderBox.size = new Vector3(size.x, size.z, colliderBox.size.z);
    }

}
