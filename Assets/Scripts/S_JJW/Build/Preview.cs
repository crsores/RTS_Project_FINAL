using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preview : MonoBehaviour
{
    private List<Collider> colliderList = new List<Collider>(); //�浹�˻�

    [SerializeField]
    private int layerGround; //�׶��� ���̾�
    private const int IGNORE_LAYER = 2; //������ ���̾�

    [SerializeField] private Material green;    // �浹�� ���� �� ������ �ʷϻ� ������
    [SerializeField] private Material red;  // �浹 ��ü�� ���� �� ������ ������ ������

    private float LB;
    private float LU;
    private float RB;
    private float RU;

    private float sizeX = 0;
    private float sizeZ = 0;

    private float cellsize = 00;

    void Start()
    {
        sizeX = this.transform.localScale.x;
        sizeZ = this.transform.localScale.z;

        cellsize = Grid.gridinstance.cellsize;
    }

    void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (colliderList.Count > 0 && !CanBuildable()) //�浹 ��ü�� �ϳ� �̻��� �� 
        {
        //Debug.Log("������Ʈ");
            SetColor(red);
        }

        else { SetColor(green); /*Debug.Log("������Ʈ22222")*/;
        }
    }

    private bool CanBuildable()
    {
        Vector3 LBpos = new Vector3(this.transform.position.x - sizeX / 2, this.transform.position.y, this.transform.position.z-sizeZ/2);
        Vector3 LUpos = new Vector3(this.transform.position.x - sizeX / 2, this.transform.position.y, this.transform.position.z+sizeZ/2);
        Vector3 RBpos = new Vector3(this.transform.position.x + sizeX / 2, this.transform.position.y, this.transform.position.z-sizeZ/2);
        Vector3 RUpos = new Vector3(this.transform.position.x + sizeX / 2, this.transform.position.y, this.transform.position.z+sizeZ/2);

        LB = Grid.gridinstance.NodePoint(LBpos, cellsize).YDepthLB;
        LU = Grid.gridinstance.NodePoint(LUpos, cellsize).YDepthLU;
        RB = Grid.gridinstance.NodePoint(RBpos, cellsize).YDepthRB;
        RU = Grid.gridinstance.NodePoint(RUpos, cellsize).YDepthRU;

        float X1 = Mathf.Abs(LB - LU);
        float X2 = Mathf.Abs(LU - RU);
        float X3 = Mathf.Abs(RU - RB);
        float X4 = Mathf.Abs(RB - LB);

        return X1 < 0.2f && X2 < 0.2f && X3 < 0.2f && X4 < 0.2f;

    }

    private void SetColor(Material mat)
    {
        Debug.Log("0000");
        foreach (Transform thistransform in transform)
        {
            Debug.Log("1111");
            var newMaterials = new Material[thistransform.GetComponent<Renderer>().materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                Debug.Log("2222");
                newMaterials[i] = mat;
            }
            thistransform.GetComponent<Renderer>().materials = newMaterials;
        }

    
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_LAYER)
        {
            colliderList.Add(other);
            Debug.Log("�浹");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != layerGround && other.gameObject.layer != IGNORE_LAYER)
        {
            colliderList.Remove(other);
            Debug.Log("���浹");
        }
    }

    public bool isBuildable()
    {
        return colliderList.Count == 0;
    }
}