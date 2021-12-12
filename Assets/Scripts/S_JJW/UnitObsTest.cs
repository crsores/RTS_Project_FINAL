using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitObsTest : MonoBehaviour
{

    Node Obs;   //현재 위치를 저장할 노드
    Node OldObs;    //움직이기 전의 위치를 저장할 노드

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

    IEnumerator SetOld()
    {
        yield return null;
        OldObs = Grid.gridinstance.NodePoint(this.transform.position, 1);
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
                Obs.walkable = false; //현재 노드를 false로 바꿈

                //if (Obs != OldObs)  //유닛이 움직여서 현재 노드와 움직이기 전의 노드가 다를 때
                //{
                //    OldObs.walkable = true; //이전의 노드는 true로 바꿈
                //    OldObs = Obs;   //이전 노드와 현재 노드를 같게 만듬
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
