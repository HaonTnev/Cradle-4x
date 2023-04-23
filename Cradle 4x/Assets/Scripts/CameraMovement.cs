using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        oldPosition = this.transform.position;
    }
    Vector3 oldPosition;
    internal static object main;

    // Update is called once per frame
    void Update()
    {
        //to do Camera movement 
        //wasd
        //zoom 

        CheckIfCameraMoved();
    }
    public void WASDMovement()
    {
        
    }
    public void PanToHex()
    {

    }

    void CheckIfCameraMoved()
    {
        if (oldPosition != this.transform.position)
        {
            oldPosition = this.transform.position;

            HexComponent[] hexes = GameObject.FindObjectsOfType<HexComponent>();
            
            foreach(HexComponent hex in hexes)
            {
                hex.UpdatePosition();
            }
        }
    }


}
