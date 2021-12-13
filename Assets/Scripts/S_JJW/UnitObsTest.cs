using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObsTest : MonoBehaviour
{

    Node Obs;   //���� ��ġ�� ������ ���
    Node OldObs;    //�����̱� ���� ��ġ�� ������ ���

    private float sizeX = 0;
    private float sizeZ = 0;

    private float cellsize = 0f;




    void Start()
    {
        OldObs = Grid.gridinstance.NodePoint(Vector3.zero, 1);
        cellsize = Grid.gridinstance.cellsize;
    }

    void Update()
    {

    }

    List<Node> falseNode = new List<Node>();
    public void UnitObstacle()
    {
        Vector3 thisPos = this.transform.position;
        int Xpos = (int)(sizeX / cellsize);
        int Zpos = (int)(sizeZ / cellsize);
        for (int i = 0; i < Xpos; i++)
        {
            for (int j = 0; j < Zpos; j++)
            {
                Vector3 ObsPos = new Vector3((thisPos.x + i * cellsize), 0, (thisPos.z + j * cellsize));
                Obs = Grid.gridinstance.NodePoint(ObsPos, cellsize);
                falseNode.Add(Obs);
                Obs.walkable = false; //���� ��带 false�� �ٲ�

                //if (Obs != OldObs)  //������ �������� ���� ���� �����̱� ���� ��尡 �ٸ� ��
                //{
                //    OldObs.walkable = true; //������ ���� true�� �ٲ�
                //    OldObs = Obs;   //���� ���� ���� ��带 ���� ����
                //}
            }
        }
    }

    public void ReSetUnitObstacle()
    {

        int Xpos = (int)(sizeX / cellsize);
        int Zpos = (int)(sizeZ / cellsize);
        for (int i = 0; i < Xpos; i++)
        {
            for (int j = 0; j < Zpos; j++)
            {
                falseNode[j].walkable = true;
            }
        }
        falseNode.Clear();
    }

}
