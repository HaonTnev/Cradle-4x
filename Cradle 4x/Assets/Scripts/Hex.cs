using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// here the values related to a particular hex are stored for reading 
public class Hex {

    public Hex(MapScript mapScript, int q, int r)
    {
        this.mapScript = mapScript;

        this.Q = q;
        this.R = r;
        this.S = -(q + r);
    }

    // Q + R + S = 0 
    // S = -(Q + R) 

    public readonly int Q; // Column
    public readonly int R; // Row
    public readonly int S; // some sum factor

    //Data for Map generation
    public float Elevation;
    public float Moisture;

    static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    public readonly MapScript mapScript;
    float radius = 1f;

    HashSet<SacredArtist> units;

    //public bool allowWrapEastWest = true;
    //public bool allowWrapNorthSouth = false;

    public Vector3 Position()
    {

        return new Vector3
            (
            HorizontalSpacing () * (this.Q + this.R * 0.5f),    //x
            0,                                                  //y
            VerticalSpacing () * this.R                         //z
            );
    }

    public float HexHeight()
    {
        return radius * 2;
    }

    public float HexWidth()
    {
        return WIDTH_MULTIPLIER * HexHeight();
    }

    public float HorizontalSpacing()
    {
        return HexWidth();
    }

    public float VerticalSpacing()
    {
        return HexHeight() * 0.75f;
    }
    public Vector3 PositionFromCamera()
    {
        return mapScript.GetHexPosition(this);
    }
    public Vector3 PositionFromCamera(Vector3 cameraPosition, float numberOfRows, float numberOfColumns) 
    {
       // float mapHeight = numberOfRows * VerticalSpacing();
        float mapWidth = numberOfColumns * HorizontalSpacing();

        Vector3 position = Position();

        float howManyWidthsFromCamera = (position.x - cameraPosition.x) / mapWidth;

        //Camera zwischen -0,5 und 0,5 halten ????
        if (mapScript.AllowWrapEastWest)
        {

            if (howManyWidthsFromCamera > 0)
                  howManyWidthsFromCamera += 0.5f;
            else
                  howManyWidthsFromCamera -= 0.5f;


            int howmanywidthsToFix = (int)howManyWidthsFromCamera;

            position.x -= howmanywidthsToFix * mapWidth;
        }

        return position;
    }
    public static float Distance(Hex a, Hex b)
    {
        //FIXME: Wrapping
        int dQ = Mathf.Abs(a.Q - b.Q);
        if (a.mapScript.AllowWrapEastWest)
        {
            if (dQ > a.mapScript.NumberOfColumns / 2)
                dQ = a.mapScript.NumberOfColumns - dQ;
        }

        int dR = Mathf.Abs(a.R - b.R);
        if (a.mapScript.AllowWrapNorthSouth)
        {   
            if (dR > a.mapScript.NumberOfColumns / 2)
                dR = a.mapScript.NumberOfColumns - dR;
        }
      

        return
            Mathf.Max(
                dQ,
                dR,
                Mathf.Abs(a.S - b.S)
                );
    }

    public void AddUnit(SacredArtist unit)
    {
        if (units == null)
        {
            units = new HashSet<SacredArtist>();
        }

        units.Add(unit);
    }
    public void RemoveUnit (SacredArtist unit)
    {
        if (units!= null)
        {
            units.Remove(unit);
        }
        
    }
    public SacredArtist[] SacredArtists()
    {
        return units.ToArray();
    }
}
