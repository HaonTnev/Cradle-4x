using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SacredArtist {


    public string Name = "Malice";
    public int LifePoints = 100;
    public int Strength = 10;
    public int MovePoints = 2;
    public int MovepointsRemaining = 2;

    public delegate void UnitMovedDelegate(Hex oldHex, Hex newHex);

    public Hex Hex { get; protected set; }


    public event UnitMovedDelegate OnUnitMoved;
    public void SetHex(Hex newHex)
    {
        Hex oldHex = Hex;
        if (Hex != null)
        {
            Hex.RemoveUnit(this);
        }

        Hex = newHex;
        Hex.AddUnit(this);

        if (OnUnitMoved!= null)
        {
            OnUnitMoved(oldHex, newHex);
        }
    }
    public void DoTurn()
    {
        //do qued move?
  

        // testing: move us one tile to the right 
        Debug.Log("do turn");
        Hex oldHex = Hex;
        Hex newHex = oldHex.mapScript.GetHexAt(oldHex.Q + 1, oldHex.R);

        SetHex(newHex);
    }

}
