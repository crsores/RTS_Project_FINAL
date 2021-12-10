using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
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

    public int gCost; //���� ������ ���� �������� ����ġ�� ��
    public int hCost;   // ���� ������ ���� �������� ���� ����ġ�� ��

    public Node parent;

    public Node(bool _walkable, int _gridX, int _gridY, float YPosLB_, float YPosLU_, float YPosRB_, float YPosRU_)
    {


        walkable = _walkable;

        gridX = _gridX;
        gridY = _gridY;

        YDepthLB = YPosLB_;
        YDepthRB = YPosRB_;
        YDepthLU = YPosLU_;
        YDepthRU = YPosRB_;


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
