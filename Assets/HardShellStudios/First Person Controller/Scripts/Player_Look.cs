using UnityEngine;
using System.Collections;

public class Player_Look : MonoBehaviour
{

    hardInput hInput = null;

    public bool inverted;
    public float speedX;
    public float speedY;
    float xrot;
    float yrot;
    // Use this for initialization
    void Start () 
	{
        hInput = GameObject.FindObjectOfType<hardInput>();	
	}
	
	// Update is called once per frame
	void Update () 
	{
        //transform.parent.eulerAngles += (new Vector3(0, Input.GetAxis("Mouse X"), 0) * speedX);
        transform.parent.eulerAngles += (new Vector3(0, hInput.GetAxis("MouseX", "MouseX", 1), 0) * speedX);
        xrot = transform.eulerAngles.y;

        if (inverted)
        {
            //yrot = Mathf.Clamp(yrot + Input.GetAxis("Mouse Y") * speedY, -70, 40);
            yrot = Mathf.Clamp(yrot + hInput.GetAxis("MouseY", "MouseY", 1) * speedY, -80, 60);
            //playerBodyNeck.eulerAngles += (new Vector3(Input.GetAxis("Mouse Y"), 0, 0) * playerLookSpeed);
        }
        else
        {
            //yrot = Mathf.Clamp(yrot + -Input.GetAxis("Mouse Y") * speedY, -70, 40);
            yrot = Mathf.Clamp(yrot + -hInput.GetAxis("MouseY", "MouseY", 1) * speedY, -80, 60);
            //playerBodyNeck.eulerAngles += (new Vector3(-Input.GetAxis("Mouse Y"), 0, 0) * playerLookSpeed);
        }

        //print(Input.mousePosition.z);
        transform.rotation = Quaternion.Euler(yrot, xrot, 0);
    }
}
