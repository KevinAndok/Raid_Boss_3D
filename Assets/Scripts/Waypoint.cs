using UnityEngine;
using Shapes;

public class Waypoint : MonoBehaviour
{
    public Disc disc;

    [HideInInspector] public Vector3? position;
    [HideInInspector] public Transform followPosition;

    public void Set (Vector3 pos, float size)
    {
        disc.Radius = size;
        disc.Thickness = size/2;

        followPosition = null;
        transform.position = pos;
    }
    public void Set (Transform trans, float size)
    {
        disc.Radius = size;
        disc.Thickness = size / 2;

        followPosition = trans;
        position = null;
    }

    private void Update()
    {
        if (followPosition != null) transform.position = followPosition.position;
    }
}
