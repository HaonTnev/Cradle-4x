using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraKeyboardController : MonoBehaviour
{
    public float Movespeed =1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //WASD Movement. Tunable through input manager in the project settings!!
        Vector3 translate = new Vector3
            (
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
            );
        this.transform.Translate(translate * Movespeed*Time.deltaTime, Space.World);
    }
}
