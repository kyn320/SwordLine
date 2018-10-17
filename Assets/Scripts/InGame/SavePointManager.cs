using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointManager : Singleton<SavePointManager>
{
    public SavePoint currentSavePoint;

    public List<SavePoint> savePointList;

    public override void Awake()
    {
        base.Awake();
        SavePoint.savePointManager = this;
    }

    public SavePoint FindSavePointToID(int _id)
    {
        return savePointList.Find(item => item.pointID == _id);
    }

    public void SetSavePoint(SavePoint _savePoint)
    {
        currentSavePoint = _savePoint;
    }

    public SavePoint GetSavePoint()
    {
        return currentSavePoint;
    }
        
}
