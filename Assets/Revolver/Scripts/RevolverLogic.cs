using System;
using System.Collections.Generic;
using UnityEngine;

public class RevolverLogic
{
    public Action OnFired;
    public Action OnFullLoaded;
    private List<PatronItem> Baraban;
    private int LoadPositionId;
    public int CurrentLoadPosition {
        get { return LoadPositionId; }
    }

    public RevolverLogic(int barabanCount,int loadPositionId)
    {
        LoadPositionId = loadPositionId;
        Baraban = new List<PatronItem>(barabanCount);
        for (int index = 0; index < barabanCount; index++)
        {
            Baraban.Add(new PatronItem());
        }
        Baraban[0].isBarret=true;
    }

  /*  public void Spine()
    {
        Spine(10);//todo Random
    }*/

    public int GetBarabanCount()
    {
        return Baraban.Count;
    }

    public bool IsFullLoaded()
    {
        return GetLoadedPatrons() == Baraban.Count - 1;// >1 is empty all time
    }

    public List<int> GetLoadedPatronsWithId()
    {
        var result = new List<int>();
        var load = 0;
        for (int index = 0; index < Baraban.Count; index++)
        {
            var patronItem = Baraban[index];
            if (patronItem.isLoad)
            {
                result.Add(index);
            }
                
        }
        return result;
    }

    public int GetLoadedPatrons()
    {
        var load = 0;
        for (int index = 0; index < Baraban.Count; index++)
        {
            var patronItem = Baraban[index];
            if (patronItem.isLoad)
                load++;
        }
        return load;
    }

    public int GetCurrentBarrelId()
    {
        for (int index = 0; index < Baraban.Count; index++)
        {
            var patronItem = Baraban[index];
            if (patronItem.isBarret)
                return index;
        }

        return -1;
    }

    public int Spine(int count)
    {
        var indexStart = GetCurrentBarrelId();
        Baraban[indexStart].isBarret = false;
        var shift = count%Baraban.Count;

        indexStart = (indexStart + shift) % Baraban.Count;
        LoadPositionId = (LoadPositionId + shift)%Baraban.Count;

        Baraban[indexStart].isBarret = true;
        return indexStart;
    }

    private bool LoadPatronTo(int id)
    {
        if (Baraban[id].isLoad) return false;

        Baraban[id].isLoad = true;

        return true;
    }


   public bool LoadPatronToPosition() // false - -- not loaded
    {
        if (IsFullLoaded()) return false;
        return LoadPatronTo(LoadPositionId);
    }

   
    public bool Fire()
    {
        var indexStart = GetCurrentBarrelId();
        var result = Baraban[indexStart].isLoad;
        Baraban[indexStart].isLoad = false;
        return result;
    }
}

public class PatronItem
{
    public bool isLoad;
    public bool isBarret;
}