using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDB : Singleton<PropDB>
{
    public List<Prop> db;

    public Prop FindItem(PropType _type)
    {
        return db.Find(item => item.type == _type);
    }

}
