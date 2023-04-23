using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexComponent : MonoBehaviour
{

    public Hex Hex;
    public MapScript MapScript;

    public void UpdatePosition()
    {
        this.transform.position = Hex.PositionFromCamera(
            Camera.main.transform.position,
            MapScript.NumberOfRows,
            MapScript.NumberOfColumns);
    }
}
