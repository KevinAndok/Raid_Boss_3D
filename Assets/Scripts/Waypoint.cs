using Shapes;
using UnityEngine;

public sealed class Waypoint : MonoBehaviour
{
    public Color[] orderColors = new Color[4];
    public Disc disc;

    [HideInInspector] public Vector3? position;
    [HideInInspector] public Transform followPosition;

    private Pool pool;

    private void Awake() => pool = PoolingSystem.GetPoolByName("Waypoint");

    public void Set(Vector3 pos, float size, OrderType type)
    {
        if (type == OrderType.none)
        {
            pool.ObjectPool.Release(gameObject);
            return;
        }

        disc.Radius = size;
        disc.Thickness = size / 2;

        followPosition = null;
        transform.position = pos;

        disc.Color = orderColors[(int)type];
    }
    public void Set(Transform trans, float size, OrderType type)
    {
        if (type == OrderType.none) return;

        disc.Radius = size;
        disc.Thickness = size / 2;

        followPosition = trans;
        position = null;

        disc.Color = orderColors[(int)type];
    }

    private void Update()
    {
        if (followPosition != null) transform.position = followPosition.position + Vector3.up * .1f;
    }
}
