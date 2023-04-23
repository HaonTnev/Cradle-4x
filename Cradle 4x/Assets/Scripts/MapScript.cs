using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapScript : MonoBehaviour
{
    void Start()
    {
        CreateMap();
    }

    void Update()
    {
        //hit space to advance to next turn

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (units!= null)
            {
                foreach(SacredArtist u in units)
                {
                    u.DoTurn();
                }
            }
        }
    }
    //public Hex Hex;
    public GameObject SacredArtist;
    public GameObject HexPrefab;

    public Mesh MeshPlains;
    public Mesh MeshWater;
    public Mesh MeshHills;
    public Mesh MeshMountains;

    public GameObject ForestPrefab;
    public GameObject JunglePrefab;

    public Material[] HexMaterials;
    public Material[] MountainMaterials;

    public Material MatMountain1;
    public Material MatMountain2;
    public Material MatJungle;
    public Material MatForest;
    public Material MatGrasslands;
    public Material MatPlains;
    public Material MatDesert;

    public Material MatWater;
    public Material MatBlood;
    public Material MatShadow;
    public Material MatLife;
    public Material MatEarth;
    public Material MatFire;

    public float HeightMountain = 0.85f;
    public float HeightHill = 0.6f;
    public float HeightPlains = 0f;

    // public float MadraShadow=0.6f;
    //public float MadraEarth =1f;
    // public float MadraFire =0.6f;
    // public float MadraLife =0.4f;
    // public float MadraBlood =0.7f;

    public float MoistureJungle = 0.5f;
    public float MoistureForest = 0.33f;
    public float MoistureGraslands = 0f;
    public float MoisturePlains = -0.5f;



    public int NumberOfColumns = 10;
    public int NumberOfRows = 10;

    private Hex[,] hexes;
    private Dictionary<Hex, GameObject> hexToGameObjectMap;

    private HashSet<SacredArtist>units;
    private Dictionary<SacredArtist, GameObject> unitToGameObjectMap;

    //not yet linked to Hex.cs's version of these 
    public bool AllowWrapEastWest = true;
    public bool AllowWrapNorthSouth = false;

    public Hex GetHexAt(int x, int y)
    {
        if (hexes == null)
        {
            Debug.LogError("ArrayList not instanciated yet!!");
            return null;
        }

        if (AllowWrapEastWest)
        {
            x = x % NumberOfColumns;
            if (x < 0)
            {
                x += NumberOfColumns;
            }
        }

        if (AllowWrapNorthSouth)
        {
            y = y % NumberOfRows;
            if (y < 0)
            {
                y += NumberOfRows;
            }
        }
        return hexes[x, y];

    }

    public Vector3 GetHexPosition(int q, int r)
    {
        Hex hex = GetHexAt(q, r);
        return GetHexPosition(hex);
    }
    public Vector3 GetHexPosition(Hex hex)
    {
     
        return hex.PositionFromCamera(Camera.main.transform.position, NumberOfRows, NumberOfColumns);
    }
    virtual public void CreateMap()
    {

        //create 2dArray to store information about the hexes 
        hexes = new Hex[NumberOfColumns, NumberOfRows];

        hexToGameObjectMap = new Dictionary<Hex, GameObject>();


        for (int column = 0; column < NumberOfColumns; column++)
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                //instaniate new Hex
                Hex h = new Hex(this, column, row);
                h.Elevation = -0.5f;


                hexes[column, row] = h;

                Vector3 pos = h.PositionFromCamera(
                    Camera.main.transform.position,
                    NumberOfRows,
                    NumberOfColumns
                    );

                // Get world position of that Hex
                GameObject hexGo = (GameObject)Instantiate(
                     HexPrefab,
                     pos,
                     Quaternion.identity,
                     this.transform
                     );

                hexToGameObjectMap[h] = hexGo;
                hexGo.name = string.Format("Hex:{0}, {1}", column, row);    
                hexGo.GetComponent<HexComponent>().Hex = h;
                hexGo.GetComponent<HexComponent>().MapScript = this;

                hexGo.GetComponentInChildren<TextMeshPro>().text = string.Format("{0},{1}", column, row);



            }
        }
        UpdateHexVisuals();

    }
    public void UpdateHexVisuals()
    {
        for (int column = 0; column < NumberOfColumns; column++)
        {
            for (int row = 0; row < NumberOfRows; row++)
            {
                Hex h = hexes[column, row];

                GameObject hexGO = hexToGameObjectMap[h];

                MeshRenderer mr = hexGO.GetComponentInChildren<MeshRenderer>();
                MeshFilter mf = hexGO.GetComponentInChildren<MeshFilter>();




                if (h.Elevation >= HeightPlains && h.Elevation < HeightMountain)
                {
                    if (h.Moisture >= MoistureJungle)
                    {
                        mr.material = MatJungle;
                        GameObject.Instantiate(ForestPrefab, hexGO.transform.position, Quaternion.identity, hexGO.transform);

                    }
                    else if (h.Moisture >= MoistureForest)
                    {
                        mr.material = MatForest;
                        GameObject.Instantiate(ForestPrefab, hexGO.transform.position, Quaternion.identity, hexGO.transform);
                    }
                    else if (h.Moisture >= MoistureGraslands)
                    {
                        mr.material = MatGrasslands;
                        //spawn forests
                    }
                    else if (h.Moisture >= MoisturePlains)
                    {
                        mr.material = MatPlains;
                        //TODO: spawn forests
                    }
                    else
                    {
                        mr.material = MatDesert;
                    }


                }

                if (h.Elevation >= HeightMountain)
                {
                    Material[] materials = { MatMountain1, MatMountain2 };
                    mr.materials = materials;
                    mf.mesh = MeshMountains;
                    hexGO.transform.position = new Vector3(hexGO.transform.position.x, -0.30f,hexGO.transform.position.z);
                }
                else if (h.Elevation >= HeightHill)
                {
                    mf.mesh = MeshHills;
                }
                else if (h.Elevation >= HeightPlains)
                {
                    mf.mesh = MeshPlains;
                }
                else
                {
                    mr.material = MatWater;
                    mf.mesh = MeshWater;
                }
            }
        }
    }

    public Hex[] GetHexesWithinRangeOf(Hex centerHex, int range)
    {
        List<Hex> results = new List<Hex>();


        for (int dx = -range; dx < range - 1; dx++)
        {
            for (int dy = Mathf.Max(-range + 1, -dx - range); dy < Mathf.Min(range, -dx + range - 1); dy++)
            {
                results.Add(GetHexAt(centerHex.Q + dx, centerHex.R + dy));
            }
        }
        return results.ToArray();
    }

    public void SpawnUnitAt(SacredArtist unit , GameObject prefab, int q, int r)
    {

        if (units == null)
        {
            units = new HashSet<SacredArtist>();
            unitToGameObjectMap = new Dictionary<SacredArtist, GameObject>();
        }

        Hex myHex = GetHexAt(q, r);
        GameObject myHexGO = hexToGameObjectMap[myHex];
        unit.SetHex(myHex);
        GameObject unitGo = (GameObject)Instantiate(prefab, myHexGO.transform.position, Quaternion.identity, myHexGO.transform);
        unit.OnUnitMoved += unitGo.GetComponent<UnitView>().OnUnitMoved;

        units.Add(unit);
        unitToGameObjectMap.Add(unit, unitGo);
    

    }

}