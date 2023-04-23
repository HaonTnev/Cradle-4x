using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Update_CurrentFunc = Update_DetectModeStart;
    }
    // Generic bookkeeping variables
    Vector3 lastMousePos;//from Input.mousePosition


    // CameraDraging bookkeeping variables
  
    Vector3 lastMouseGroundPlanePosition;
    Vector3 cameraTargetOffset;

    delegate void UpdateFunc();
    UpdateFunc Update_CurrentFunc;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelUpdateFunc();
        }
        Update_CurrentFunc();
        lastMousePos = Input.mousePosition;
        Update_ScrollZoom();
    }

     void Update_DetectModeStart()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //right mouse button went down.
            //this dosen't do anything by itself

        }
        else if (Input.GetMouseButton(1) && Input.mousePosition != lastMouseGroundPlanePosition)
        {
            // left button is being held down &  the mouse moved? thats a camera Drag!
            Update_CurrentFunc = Update_CameraDrag;
            lastMouseGroundPlanePosition = MouseToGroundPlane(Input.mousePosition);

            Update_CurrentFunc();
        }

    }
    // Update is called once per frame

    void CancelUpdateFunc()
    {
        Update_CurrentFunc = Update_DetectModeStart;

        // also cleanup UI shit 

    }

    Vector3 MouseToGroundPlane(Vector3 mousePos)
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
        //At which point intersects the mouse ray Y=0
        if (mouseRay.direction.y >= 0)
        {
            //Debug.Log("Why is mouse ray pionting up?");
            return Vector3.zero;
        }
        float rayLength = mouseRay.origin.y / mouseRay.direction.y;
        return mouseRay.origin - (mouseRay.direction * rayLength);
    }
    void Update_CameraDrag()
    {
        if (Input.GetMouseButtonUp(1))
        {
            CancelUpdateFunc();
            return;
        }


        Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);

       // lastMouseGroundPlanePosition = hitPos;

            Vector3 diff = lastMouseGroundPlanePosition - hitPos;
            Camera.main.transform.Translate(diff, Space.World);

            lastMouseGroundPlanePosition = hitPos = MouseToGroundPlane(Input.mousePosition);

        
    }

     void Update_ScrollZoom()
    {
        //Zoom mit mousewheel
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        float minHeight = 2;
        float maxHeight = 20;

        if (Mathf.Abs(scrollAmount) > 0.01f)
        {

            Vector3 hitPos = MouseToGroundPlane(Input.mousePosition);

            //Move Camera towoard hitPos
            Vector3 direction = hitPos - Camera.main.transform.position;

            Vector3 p = Camera.main.transform.position;

            //Stop Zoom at max height 
            if (scrollAmount > 0 || p.y < maxHeight - 0.1f)
            {
                Camera.main.transform.Translate(direction * scrollAmount, Space.World);
            }


            p = Camera.main.transform.position;
            if (p.y < minHeight)
            {
                p.y = minHeight;
            }
            if (p.y > maxHeight)
            {
                p.y = maxHeight;
            }
            Camera.main.transform.position = p;

            // Change camera angle with up/down schrolling

            float lowzoom = minHeight + 3;
            float highZoom = maxHeight - 7;

            Camera.main.transform.rotation = Quaternion.Euler(
                Mathf.Lerp(35, 90, p.y / (maxHeight / 1.5f)),
                Camera.main.transform.rotation.eulerAngles.y,
                Camera.main.transform.rotation.eulerAngles.z
                );
        }
    }
}
