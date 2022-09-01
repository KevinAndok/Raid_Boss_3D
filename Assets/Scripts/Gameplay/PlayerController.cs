using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public List<Entity> selectedUnits = new List<Entity>();

    private void Awake()
    {
        if (Instance) Destroy(Instance);
        Instance = this;
    }
}
