using System.Collections.Generic;
using UnityEngine;

public sealed class EntityManager : MonoBehaviour
{
    public static List<Entity> AllEntities = new List<Entity>();

    public StatusBar StatusBar;

    private void Start()
    {
        var camRotation = Camera.main.transform.rotation.eulerAngles.x;

        foreach (var entity in AllEntities)
        {
            entity.statusBar = Instantiate(StatusBar, entity.transform.parent);
            entity.statusBar.barParent.rotation = Quaternion.Euler(new Vector3(camRotation, 0, 0));
        }
    }

    private void Update()
    {
        DoHealthBars();
    }

    private void DoHealthBars()
    {
        var camTrans = Camera.main.transform;

        foreach (var entity in AllEntities)
        {
            SetEntityStatusBar(entity, camTrans);
        }
    }
    private void SetEntityStatusBar(Entity e, Transform camTrans)
    {
        var status = e.statusBar;

        if (e.IsDead)
        {
            status.gameObject.SetActive(false);
            return;
        }

        status.gameObject.SetActive(true);

        status.transform.position = e.transform.position;
        //status.barParent.LookAt(camTrans);

        status.SetHealth(e.stats.HealthPercentage);
        status.SetMana(e.stats.ManaPercentage);
        status.SetCast(e.commands[0].castTime); 
    }
}
