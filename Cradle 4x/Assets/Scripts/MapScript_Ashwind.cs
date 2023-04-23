using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript_Ashwind : MapScript
{
    override public void CreateMap()
    {
        // call the CreateMap Function (baseMap)

        base.CreateMap();

        int numberOfContinents = 3;
        int continetSpacing = NumberOfColumns/numberOfContinents;

        //Random.InitState(0);
        
        // Instanciate landmass
        for (int c = 0; c < numberOfContinents; c++)
        {
            int numberOfTerrainTiles = Random.Range(4, 8);
            for (int i = 0; i < numberOfTerrainTiles; i++)
            {
                int range = Random.Range(5, 8);
                int y = Random.Range(range, NumberOfRows - range);
                int x = Random.Range(0, 10) - y / 2 + (c*continetSpacing);

                ElevateArea(x, y, range);
            }
        }
        //Map Noise 
        float noiseRes = 0.1f;
        Vector2 noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
        float noiseScale = 2.5f;

        for (int column = 0; column < NumberOfColumns; column++)
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                Hex h   = GetHexAt(column, row);
                    float n = Mathf.PerlinNoise(    ((float)column / Mathf.Max(NumberOfColumns, NumberOfRows) / noiseRes)+noiseOffset.x, 
                                                    ((float)row /Mathf.Max(NumberOfColumns, NumberOfRows) / noiseRes)+noiseOffset.y)
                    -0.5f;
               h.Elevation += n * noiseScale;
            }

        }

        
        //Set Madra types
         noiseRes = 0.05f;
         noiseOffset = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
         noiseScale = 2.5f;

        for (int column = 0; column < NumberOfColumns; column++)
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                Hex h = GetHexAt(column, row);
                float n = Mathf.PerlinNoise(((float)column / Mathf.Max(NumberOfColumns, NumberOfRows) / noiseRes) + noiseOffset.x,
                                                ((float)row / Mathf.Max(NumberOfColumns, NumberOfRows) / noiseRes) + noiseOffset.y)
                - 0.5f;
                h.Moisture += n * noiseScale;
            }

        }


        //set landmass to mountains, forrest, plaines, dessert
        UpdateHexVisuals();

        SacredArtist unit = new SacredArtist();
        SpawnUnitAt(unit, SacredArtist, 50, 16);
    }
    void ElevateArea(int q, int r, int range, float centerHeight = 0.5f)
    {
        Hex centerHex = GetHexAt(q, r);

        //centerHex.elevation = 0.5f; 

        Hex[] areaHexes = GetHexesWithinRangeOf(centerHex, range);


        foreach(Hex h in areaHexes)
        {
            if (h.Elevation < 0)
                h.Elevation = 0;
            h.Elevation += centerHeight* Mathf.Lerp(1f,0.25f,Hex.Distance(centerHex,h)/range);
        }

    }

}
