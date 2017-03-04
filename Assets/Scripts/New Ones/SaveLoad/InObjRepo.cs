using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InObjRepo : MonoBehaviour
{
    public List<ObjectUpdate> ObjInactive = new List<ObjectUpdate>();
    public List<ButtonUpdateState> ButInactive = new List<ButtonUpdateState>();
    public List<SuperObjectUpdate> SObjInactive = new List<SuperObjectUpdate>();


    private void Awake()
    {
        foreach (var obj in this.ObjInactive)
        {
            obj.CustomAwake();
        }

        foreach (var obj in this.ButInactive)
        {
            obj.CustomAwake();
        }

        foreach (var obj in this.SObjInactive)
        {
            obj.CustomAwake();
        }
    }
}
