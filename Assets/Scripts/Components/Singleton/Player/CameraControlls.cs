using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraControlls : MonoBehaviour
{
    public Camera Camera;
    public SpriteRenderer Nebula;
    public Vector3 NebulaDefaultScale;

    public CameraControlls()
    {
        Singles.CameraControlls = this;
    }

    // Use this for initialization
    void Start () {
        NebulaDefaultScale = Nebula.transform.localScale;
        UpdateNebulaSize();
    }

    private float GetMoveSpeed()
    {
        return Camera.orthographicSize / 12;
    }
	
	// Update is called once per frame
	void Update () {
        if (Singles.UiController != null && Singles.UiController.Tutorial != null && Singles.UiController.Tutorial.IsOpen) return;

        var screenWidth = Camera.pixelWidth;
        var screenHeight = Camera.pixelHeight;

        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y > screenHeight - 30)
        {
            transform.position = new Vector3(transform.position.x, (transform.position.y + GetMoveSpeed()).GetMinMax(-180f, 50f), 
                transform.position.z);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y < 30)
        {
            transform.position = new Vector3(transform.position.x, (transform.position.y - GetMoveSpeed()).GetMinMax(-180f, 50f), 
                transform.position.z);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x < 30)
        {
            transform.position = new Vector3((transform.position.x - GetMoveSpeed()).GetMinMax(-50f, 200f), 
                transform.position.y, transform.position.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x > screenWidth - 30)
        {
            transform.position = new Vector3((transform.position.x + GetMoveSpeed()).GetMinMax(-50f, 200f), 
                transform.position.y, transform.position.z);
        }

        var mouseWheelAxis = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheelAxis != 0)
        {
            if(mouseWheelAxis < 0) Camera.orthographicSize *= mouseWheelAxis * -12;
            else Camera.orthographicSize /= mouseWheelAxis * 12;

            Camera.orthographicSize = Camera.orthographicSize.GetMinMax(2, 100);

            UpdateNebulaSize();
        }
    }

    private void UpdateNebulaSize()
    {
        Nebula.transform.localScale = new Vector3(
                NebulaDefaultScale.x * Camera.orthographicSize,
                NebulaDefaultScale.y * Camera.orthographicSize,
                NebulaDefaultScale.z
            );
    }
}
