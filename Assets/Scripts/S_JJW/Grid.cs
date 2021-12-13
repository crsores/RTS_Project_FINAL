using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid gridinstance = null;

    public Node startnode;
    public Node endnode;
    public int width;
    public int height;
    public float cellsize;

    Node[,] grid;   //Node�� index��ȣ�� ������ grid ����

    private void Awake()
    {
        if (null == gridinstance) gridinstance = this;
        else Destroy(this.gameObject);
        CreateGrid();
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //for (int x = 0; x < (int)width; x++)
        //{
        //    Debug.DrawLine(new Vector3(x * cellsize, 0, 0), new Vector3(x * cellsize, 0, width * cellsize), Color.red);
        //}

        //for (int y = 0; y < (int)height; y++)
        //{
        //    Debug.DrawLine(new Vector3(0, 0, y * cellsize), new Vector3(height * cellsize, 0, y * cellsize), Color.red);
        //}
    }

    float LU = 0;
    float LB = 0;
    float RU = 0;
    float RB = 0;

    Vector2 Center;
    public void CreateGrid()
    {
        grid = new Node[(int)width, (int)height];   // grid�� ���� ���� ��ŭ �迭 ����


        for (int x = 0; x < (int)width; x++)
        {
            for (int y = 0; y < (int)height; y++)
            {
                RaycastHit hit;

                int layerMask = (1 << LayerMask.NameToLayer("ground")) + (1 << LayerMask.NameToLayer("Sea"));


                //  Vector3 worldPoint = worldBottomLeft + Vector3.right * (x + 0.5f) + Vector3.forward * (y + 0.5f);

                if (Physics.Raycast(new Vector3(x * cellsize, 10, y * cellsize), Vector3.down, out hit, 100f, layerMask))
                {
                    LB = hit.point.y;
                }
                if (Physics.Raycast(new Vector3(x * cellsize, 10, y * cellsize + cellsize), Vector3.down, out hit, 100f, layerMask))
                {
                    LU = hit.point.y;
                }
                if (Physics.Raycast(new Vector3(x * cellsize + cellsize, 10, y * cellsize), Vector3.down, out hit, 100f, layerMask))
                {
                    RB = hit.point.y;
                }
                if (Physics.Raycast(new Vector3(x * cellsize + cellsize, 10, y * cellsize + cellsize), Vector3.down, out hit, 100f, layerMask))
                {
                    RU = hit.point.y;
                }


                // Debug.Log(LB +" : "+ LU + " : " + RB + " : " + RU);

                //                grid[x, y] = new Node(true, x, y, LB, LU, RB, RU, Center);

                if (JudgeObstacle(LB, LU, RB, RU)) grid[x, y] = new Node(true, x, y, LB, LU, RB, RU);
                else grid[x, y] = new Node(false, x, y, LB, LU, RB, RU);

                if (grid[x, y].YDepthLB <= 1) grid[x, y].walkable = false;



                //if (LB == RU&&LB==LU&&LB==RB&&LU==RB&&LU==RU&&RB==RU)
                //{
                //    grid[x, y] = new Node(true, x, y, LB, LU, RB, RU,Center);
                //   // Debug.Log("1");
                //}
                //else
                //{
                //    grid[x, y] = new Node(false, x, y, LB, LU, RB, RU, Center);
                //  //  Debug.Log("2");
                ////    Debug.Log(x + " : " + y);
                //}
                //  //  Debug.Log((LB == LU && LU == RU && RU == RB));

                // Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                // Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                //Debug.Log(GetWorldPosition(x,y));

            }
        }
        //Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width , height), Color.white, 100f);
        //Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
    }

 

    private Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellsize;
    }

    public Node NodePoint(Vector3 rayPosition, float cellsize)
    {

        int x = (int)(rayPosition.x / cellsize);
        int z = (int)(rayPosition.z / cellsize);

        return grid[x, z];
    }


    public Vector3[] ReturnPosArr(List<Node> path, float cellsize)
    {
        List<Vector3> waypoints = new List<Vector3>();
        for (int i = 0; i < path.Count; i++)
        {
            float X = path[i].gridX * cellsize + cellsize / 2;
            float Y = path[i].YDepthLB;
            float Z = path[i].gridY * cellsize + cellsize / 2;

            Debug.Log("hihi");

            waypoints.Add(new Vector3(X, Y, Z));
        }
        return waypoints.ToArray();
    }

    public Vector3 ReturnPos(Node node, float Y, float cellsize)
    {
        float X = node.gridX * cellsize;
        float Z = node.gridY * cellsize;




        return new Vector3(X, 0, Z);
    }

    public void ObstacleNode(int x, int y)
    {
        //int x = (int)(Obstacle.x + width / 2);
        //int y = (int)(Obstacle.z + height / 2);
        //grid[x, y].ChangeNode = false;
    }

    public void ExitObstacleNode(Vector3 Obstacle)
    {
        int x = (int)(Obstacle.x + width / 2);
        int y = (int)(Obstacle.z + height / 2);
        grid[x, y].ChangeNode = true;
    }

    public List<Node> GetNeighbours(Node node)  //������Ƽ�� �������� ������ ���� �� �̵� ������ ��带 ��ȯ
    {
        List<Node> neighbours = new List<Node>();
        int[,] temp = { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } }; //���� ��带 �������� ���� �¿�
                                                                    // 0    1       1
                                                                    // 1    0   1   0   1
                                                                    // 0   -1       1
                                                                    //-1    0

        bool[] walkableUDLR = new bool[4];  // �̿� ����� �̵����� ���θ� ���� bool�� �迭 �غ��Ѵ�

        //�����¿��� ��� ���� ���
        for (int i = 0; i < 4; i++)
        {

            int checkX = node.gridX + temp[i, 0];
            //0,1,0,-1

            int checkY = node.gridY + temp[i, 1];
            //1,0,-1,0

            //0,1
            //1,0
            //0,-1
            //-1,0

            // if node.gridX = 1,1

            //1,2
            //2,1
            //1,0
            //0,1

            if (checkX >= 0 && checkX < (int)width && checkY >= 0 && checkY < (int)height)
            {  //�˻� ��尡 ��ü ���ȿ� ��ġ�� �ִ��� �˻�



                if (grid[checkX, checkY].walkable) walkableUDLR[i] = true; //�ش� ��忡 ��ֹ��� ���� �� �� �ִٸ� �ش� bool ���� true�� ����
                                                                           //��ֹ��� �ִ� ����� false�� �߰�



                if (Mathf.Abs(node.YDepthLB - grid[checkX, checkY].YDepthLB) > 0.5f)
                {
                    // node.walkable = false;
                    grid[checkX, checkY].walkable = false;
                }





                neighbours.Add(grid[checkX, checkY]);   //�̿� ��忡 �߰�
            }
        }

        //�밢���� ��带 ���
        for (int i = 0; i < 4; i++)
        {
            if (walkableUDLR[i] || walkableUDLR[(i + 1) % 4])
            {
                int checkX = node.gridX + temp[i, 0] + temp[(i + 1) % 4, 0];
                int checkY = node.gridY + temp[i, 1] + temp[(i + 1) % 4, 1];
                if (checkX >= 0 && checkX < (int)width && checkY >= 0 && checkY < (int)height)
                {


                    if (Mathf.Abs(node.YDepthLB - grid[checkX, checkY].YDepthLB) > 1.0f)
                    {
                        // node.walkable = false;
                        grid[checkX, checkY].walkable = false;


                    }

                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
        //�̿����� ���� List�� ��ȯ
    }

    bool JudgeObstacle(float LB, float LU, float RB, float RL)
    {
        return Mathf.Abs(LB - LU) < 0.3f && Mathf.Abs(RU - LU) < 0.3f && Mathf.Abs(RU - RB) < 0.3f && Mathf.Abs(RB - LB) < 0.3f;
    }


    public void SetObstacle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node node = RayCast();
            if (node != null) //���콺 ����Ʈ�� ���� ���� ���� �ƴ϶�� return
            {
                if (node.start || node.end)      // Ray�� ���� ��尡 ����, ������ �Ǵ� ���������̰ų� �� �����̸� ����/���� ��ġ�� ������ �� �ִ�.
                    StartCoroutine("SwitchStartEnd", node);
                else
                    StartCoroutine("ChangeWalkable", node); //�����̳� ���� �ƴ϶�� ��ֹ��� ����
            }
            return;
        }

    }

    public Node RayCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //ī�޶� �������� ���콺 ��ġ�� Ray�߻�
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            GameObject obj = hit.collider.gameObject; //���� hit�� ������ ��ȯ
                                                      //  Debug.Log(obj.name);
                                                      //  Debug.Log(obj.transform.position);
            return Grid.gridinstance.NodePoint(obj.transform.position, cellsize);  // ������ ����� x,y ������ grid[x,y]�� ã��
        }
        return null; // ���� collider�� ������ null ��ȯ
    }


    IEnumerator SwitchStartEnd(Node node)   // �巡�׷� ��ŸƮ ���� ��ġ ����
    {
        //node = ���콺 ����Ʈ�� ���� node
        bool start = node.start;    // �� ��尡 start���� 
        Node nodeOld = node;    //noldOld�� ���콺 ����Ʈ�� ���� node�� ����

        while (Input.GetMouseButton(0))
        {
            node = RayCast();
            if (node != null && node != nodeOld)
            {
                if (start && !node.end)
                {

                    startnode = node;

                    nodeOld = node;
                }
                else if (!start && !node.start)
                {
                    endnode = node;
                    nodeOld = node;
                }
            }
            yield return null;
        }
    }

    IEnumerator ChangeWalkable(Node node)
    {
        bool walkable = !node.walkable; // ���� �Ұ��� �ݴ�� ��ȯ

        while (Input.GetMouseButton(0)) //���콺 ��ư�� ������ ���� ��� ����
        {
            node = RayCast();
            if (node != null && !node.start && !node.end) //�ش� ��尡 �־�� �ϰ�, ���۰� ������ �ƴ� �� ����
            {
                node.ChangeNode = walkable;
            }
            yield return null;
        }
    }

    public float Getcellsize
    {
        get
        {
            return cellsize;
        }
    }

}
