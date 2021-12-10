using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Craft  // ������ �ǹ����� �з��� Ŭ����
{
    public string craftname; //�ǹ� �̸�
    public GameObject previewCraft; //�̸����� ������
    public GameObject BuildCraft; // ���� ������ ������
    
}
public class BuildManager : MonoBehaviour
{
    [SerializeField] private Craft[] craft = null;  //����ȭ�� ���� �ν�����â���� �����ϱ� ���� ����

    private GameObject PreviewPrefab = null;    //Craft�� ���� ������ �̸����⿡ ����� ���� ����
    private GameObject InsPrefab = null;    //������ �ǹ�

    private bool isActivatePreview = false;
   private Vector3 MousePos;
    //
    private RaycastHit hitinfo;
    private Vector3 _location;

    private Vector3 buildPos;

    private void Awake()
    {
        
        
    }
    private void Update()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //���콺�� ���� ��ġ �ޱ�

        if (Input.GetKeyDown(KeyCode.R)) SlotClick(0,0);
        if (Input.GetKeyDown(KeyCode.W)) SlotClick(1,-1);

        int X;
        int Z;

        if (isActivatePreview)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitinfo))
            {
                if (hitinfo.transform != null)
                {
                    _location = hitinfo.point;
                 


                    if ((int)_location.x % 2 == 0) X = (int)_location.x;
                    else X = (int)(_location.x) - 1;


                    if ((int)_location.z % 2 == 0) Z = (int)_location.z;
                    else Z = (int)(_location.z) - 1;

                    //Debug.Log(X + " : " + Z);
                    buildPos = new Vector3(X, hitinfo.point.y, Z);
                    PreviewPrefab.transform.position = buildPos;
                }
            }
        }

        if (PreviewPrefab != null && PreviewPrefab.GetComponentInChildren<Preview>().isBuildable())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(InsPrefab, buildPos, Quaternion.identity);
                Destroy(PreviewPrefab);
                isActivatePreview = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && PreviewPrefab != null)
        {
            Destroy(PreviewPrefab);
            isActivatePreview = false;
           
        }
    }


    public void SlotClick(int _SlotNumber, float X)
    {
        Vector3 mousePos = MousePos;
        mousePos.y += X;
        PreviewPrefab = Instantiate(craft[_SlotNumber].previewCraft, mousePos, Quaternion.identity);
        InsPrefab = craft[_SlotNumber].BuildCraft;
       
        isActivatePreview = true;
        //BuildNum = _SlotNumber;
    }

  

  

    



}