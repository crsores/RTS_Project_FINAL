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

    private bool isActivatePreview = false; //Preview�� ����������� Ȯ���� bool�� ����

    //
    private RaycastHit hit;
    private Vector3 _location;

    private Vector3 buildPos;

    private void Awake()
    {


    }
    private void Update()
    {

        if (!isActivatePreview)
        {
            if (Input.GetKeyDown(KeyCode.R)) SlotClick(0);
        }

        if (isActivatePreview)
        {
            int layerMask = (1 << LayerMask.NameToLayer("ground")) + (1 << LayerMask.NameToLayer("Sea"));   //�浹�˻縦 �� ���̾�

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, layerMask))
            {
                Debug.Log(hit.point);


                _location = hit.point;
                _location.y += 0.1f;

                buildPos = new Vector3((int)_location.x, hit.point.y, (int)_location.z);
                PreviewPrefab.transform.position = buildPos;

            }
        }

        if (PreviewPrefab != null && PreviewPrefab.GetComponentInChildren<Preview>().GetBuildable())
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


    public void SlotClick(int _SlotNumber)
    {
        PreviewPrefab = Instantiate(craft[_SlotNumber].previewCraft);
        InsPrefab = craft[_SlotNumber].BuildCraft;
        isActivatePreview = true;
    }









}