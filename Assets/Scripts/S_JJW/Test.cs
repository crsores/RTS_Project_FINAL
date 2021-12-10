using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    public Camera camera;
    public float cellsize;
    public int X;
    public int Y;

    private Grid_2 grid;
    private void Start()
    {
        grid = new Grid_2(X, Y, cellsize);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.SetValue(GetMouseWorldPosition(), 56);
        }
        //Debug.Log(GetMouseWorldPosition());
       // Debug.Log(camera.ScreenToWorldPoint(Input.mousePosition));
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        //Debug.Log("���콺 ��ġ"+Input.mousePosition);    //����� �� ����
        vec.z = 0f;
        return vec;
    }
    //public Vector3 GetMouseWorldPositionWithZ()
    //{
    //    return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
    //}
    //public Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
    //{
    //    return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
    //}
    public Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition); // ���⼭ ���� ������ ����
        Debug.Log("���콺 ��ġ" + worldPosition);
        
        return worldPosition;
    }

}
