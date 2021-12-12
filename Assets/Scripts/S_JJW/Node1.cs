using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node1
{
    public bool walkable;

    public int gridX;
    public int gridY;

    public float YDepthLB;
    public float YDepthLU;
    public float YDepthRB;
    public float YDepthRU;



    public bool start;
    public bool end;

    public int gCost; //시작 노드부터 현재 노드까지의 가중치의 합
    public int hCost;   // 현재 노드부터 도착 노드까지의 예상 가중치의 합

    public List<Node> MiniNode;

    public Node1 parent;

    public Node1(bool _walkable, int _gridX, int _gridY, float YPosLB_, float YPosLU_, float YPosRB_, float YPosRU_,List<Node> MiniNode_)
    {


        walkable = _walkable;

        gridX = _gridX;
        gridY = _gridY;

        YDepthLB = YPosLB_;
        YDepthRB = YPosRB_;
        YDepthLU = YPosLU_;
        YDepthRU = YPosRB_;
        MiniNode = MiniNode_;

    }

    public int GetX
    {
        get
        {
            return gridX;
        }
    }

    public int GetY
    {
        get
        {
            return gridY;
        }
    }

    public Vector3 GetPos(float Y, float cellcise)
    {

        float X = gridX * cellcise;
        float Z = gridY * cellcise;

        return new Vector3(X, Y, Z);
    }




    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public bool ChangeNode
    {
        set
        {
            walkable = value;
        }
    }








}
