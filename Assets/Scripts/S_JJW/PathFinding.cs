using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public GameObject Path_ = null;


    //==============================================

    public Node start; //��ã�⿡�� ������ �� ���
    public Node end;    //��ã�⿡�� ���� �� ���

    private float cellsize; //cell ũ�⸦ ��Ÿ�� ����

    [SerializeField] GameObject TargetPos;
    //���콺 ��Ŭ�� �� ������ ������Ʈ
    //�� ������Ʈ�� ��ġ���� Node�� index�� ��ȯ �� ��ã�⸦ ��

    //�� ��ũ��Ʈ���� start�� end�� �� �������� �Ѵ�.
    // ���� start�� end�� pathFinding�� �̷����� �Ѵ�.

    //Grid�� Node���� �޾ƿ� ���� node�� index�� �����;� �Ѵ�.

    Vector3[] Path = null; //��ã�⿡�� ã���� ����� index�� ������ ��ġ������ ������ �迭

    int targetIndex = 0; // wayPoint�� �ε��� ��

    float speed = 10f; //�̵� �ӵ�

    public bool finding;
    public bool success = false;    //��ã�Ⱑ �������� Ȯ��

    private Rigidbody Rb = null;
    Vector3 thisPos = Vector3.zero;

    //======================================================================
    //���� ���� �̵��� ���� ����

    Interactables.IUnit iUnit = null;

    public Units.BasicUnit unitType;

    public Units.UnitStatType.Base baseStats;

    private bool isMove = false;

    Vector3 Movedis = Vector3.zero;
    Vector3 LookDir = Vector3.zero;
    //=======================================================================

    //������ ��ֹ� ������ ���� ����
    UnitObsTest UO = null;


    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
        iUnit = GetComponentInChildren<Interactables.IUnit>();
        cellsize = Grid.gridinstance.Getcellsize;   //Grid���� ������ cellsize����
        baseStats = unitType.baseStats;

        UO = GetComponent<UnitObsTest>();
        UO.UnitObstacle();
    }

    private void Update()
    {

        if (iUnit.isSelected()) //������ ������ �Ǹ�
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (!baseStats.air) //������������ �Ǵ�
                {
                    SetTarget();
                }
                else
                {
                    FlyingTarget();
                }

            }
        }

        FloyingMoveUnit(Movedis);




        //thisPos = this.transform.position;

        //    thisPos.y = Grid.gridinstance.NodePoint(this.transform.position, cellsize).YDepthLB + 1.0f;
        //    this.transform.position = thisPos;


        //====================================================

        //    Vector3 YPOs = this.transform.position;
        //RaycastHit hit;
        //if(Physics.Raycast(this.transform.position,Vector3.forward*0.5f,out hit, 1.0f))
        //{
        //    YPOs.y = hit.point.y;
        //    Debug.DrawLine(this.transform.position, Vector3.forward, Color.red);
        //}
        //    this.transform.position = YPOs;


    }

    //public void OnPathFound()
    //{
    //    StopCoroutine("FollowPath");
    //    StartCoroutine("FollowPath");
    //}

    //IEnumerator FollowPath()
    //{
    //    Vector3 currentWaypoint = Path[0];  //currenWaypoint�� path�迭�� ����
    //    while (true)
    //    {
    //        if (transform.position == currentWaypoint)  //���� ��ġ�� currentWaypoint�� ���� ��
    //        {
    //            targetIndex++;  
    //            if (targetIndex >= Path.Length) yield break;
    //        }
    //        currentWaypoint = Path[targetIndex];    // path�迭�� index��ȣ ����
    //    }
    //    this.transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
    //    yield return null;
    //}


    // ==========================================================
    //������ ���� ��ġ�� ��ã���� ���� ��ġ�� ����
    public void SetStartPos()
    {
        if (start != null)
        {
            //��ŸƮ ��尡 �̹� �ִ� ��� ������ ��ŸƮ�� �ʱ�ȭ ��Ų��.

            start = null;
        }
        start = CurrentPos(); //���� ���� ������Ʈ�� �ִ� ��ġ�� ���

        //��ŸƮ ��带 ���� ������ ��ġ�Ʒ� ���� ����
    }
    public Node CurrentPos()
    {
        //Debug.Log(this.transform.position);
        return Grid.gridinstance.NodePoint(this.transform.position, cellsize);

    }
    // ================================================================






    // ���콺 ����Ʈ�� ���� ��带 ��ã���� ��ǥ�������� ����
    public void SetTarget()
    {
        if (Input.GetMouseButtonDown(1))
        {
            openSet.Clear();
            closedSet.Clear();
            SetStartPos();
            //���콺 ��Ŭ�� �� ���ο� ��ã�⸦ ���� Open�� close�� ����, ���� ��ġ�� ���� ���� ������Ʈ�� ��ġ�� �ʱ�ȭ

            //===========================================================

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Vector3 hitPos = hit.point;
                GameObject item = Instantiate(TargetPos, hit.point, Quaternion.identity);
                //���콺���� Ray�� ���� ���� ���� ������Ʈ�� ����
                //�������� �ð�ȭ �ϱ� ���� ������Ʈ

                if (end != null) end = null;
                //end���� �̹� � ���� ���ִٸ� �ʱ�ȭ

                end = Grid.gridinstance.NodePoint(hitPos, cellsize);
                //end�� hitPos�� �����ϴ� NodeIndex�� ����
            }

            //if (end.walkable == true)
            //{
            StopCoroutine("FindPath");
            StopCoroutine("MoveUnit");
            StartCoroutine("FindPath");
            //}
            //start�� �������� ���������� FindPath�Լ� ����
        }
    }

    List<Node> openSet = new List<Node>();      // �̿���带 ������ List
    HashSet<Node> closedSet = new HashSet<Node>(); // �̹� �˻��� ��带 ������ Hash

    // =================================================================



    public IEnumerator FindPath()
    {

        //��ã�� �Լ�


        openSet.Add(start); //start�� ù ���� ��Ͽ� �߰�



        //================================================================================


        if (openSet.Count > 0)
        {
            while (openSet.Count > 0)   //���� ��Ͽ� ���ڰ� ���� �� ���� �ݺ�
            {
                Node currentNode = openSet[0]; // CurrentNode�� ó�� ������_ ������ ��ġ

                //Open�� fCost�� ���� ���� ��带 ã��
                for (int i = 1; i < openSet.Count; i++)
                { //0�� ���� ����̱� ������ i�� 1���� ����
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                    {
                        currentNode = openSet[i];
                    }
                }

                if (currentNode != end && openSet.Count == 0)
                {
                    success = false;
                    Debug.Log("����");
                    break;
                }

                //���콺 ��ġ�� �̵� ������ �������� Ȯ��
                //�̵� �Ұ����ϴٸ� ���� �Ұ�
                //�̵� ������ �����̸� ����ó��


                openSet.Remove(currentNode);
                closedSet.Add(currentNode);



                // ���� ��尡 �������� while�� Ż��
                if (currentNode == end)
                {
                    //pathSuccess = true;
                    success = true;
                    targetIndex = 0;
                    StopCoroutine("MoveUnit");
                    StartCoroutine("MoveUnit");

                    UO.ReSetUnitObstacle();

                    Debug.Log("���� Node Count : " + closedSet.Count);
                    break;
                }


                //int X = Grid.gridinstance.TrueNodeCount();

                // Debug.Log(X);

                //�̿� ��带 �˻�
                foreach (Node neighbour in Grid.gridinstance.GetNeighbours(currentNode))
                {
                    //�̵��Ұ� ��� �̰ų� �̹� �˻��� ���� ����
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }


                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    // ���� ����� gCost�� ���� ���� �̿� ����� �Ÿ��� ���

                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        //������ ����� ������ �̿� ����� gCost�� ũ�ų� 
                        neighbour.gCost = newMovementCostToNeighbour;

                        neighbour.hCost = GetDistance(neighbour, end);

                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                        {
                            openSet.Add(neighbour);
                        }
                    }
                }
            }


        }
        //���� ã�� ������ �� ����ó�� �ʿ�

        //���� �ٴٴ� �̹� walkable�� false�̱� ������ ���õ��� ����
        //���� ����ó���� �Ǹ� �ٲ� ����

        yield return null;

    }

    float closeDistance = 1.0f;



    //==============================================================================

    //Vector3[] RetracePath(Node startNode, Node endNode)
    //{
    //    //Debug.Log("ggggg");
    //    List<Node> path = new List<Node>();
    //    Node currentNode = endNode;

    //    while (currentNode != startNode)
    //    {
    //        path.Add(currentNode);
    //        currentNode = currentNode.parent;
    //    }
    //    Vector3[] waypoints = SimplifyPath(path);
    //    Array.Reverse(waypoints);
    //    //Debug.Log(waypoints);
    //    return waypoints;
    //}


    ////�ݺ��Ǵ� �̵��� �������ָ� ��������Ʈ�� �����ϰ� �����.
    //Vector3[] SimplifyPath(List<Node> path) // �Ű������� Node�� List�� �޴´�.
    //{
    //    List<Vector3> waypoints = new List<Vector3>();
    //    Vector2 directionOld = Vector2.zero;

    //    for (int i = 1; i < path.Count; i++)
    //    {
    //        Vector2 directionNew = new Vector2((path[i - 1].gridX - path[i].gridX) / 0.25f, (path[i - 1].gridY - path[i].gridY) / 0.25f);
    //        if (directionNew != directionOld)
    //        {
    //            waypoints.Add(path[i - 1].GetPos(this.transform.position.y, cellsize));
    //        }
    //        directionOld = directionNew;
    //    }
    //    waypoints.Add(start.GetPos(this.transform.position.y, cellsize));
    //    //  Debug.Log(waypoints[1]);
    //    return waypoints.ToArray();
    //}

    //=========================================================================================


    Vector3[] RetracePath2(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = Grid.gridinstance.ReturnPosArr(path, cellsize);
        Array.Reverse(waypoints);
        return waypoints;
    }




    int GetDistance(Node nodeA, Node nodeB)
    {
        //��尣 �Ÿ����
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        //���� ���� �̿���� ������ �Ÿ� ���
        //Debug.Log(i);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }


    IEnumerator MoveUnit()
    {
        // Debug.Log("sss");
        if (success)
        {
            Vector3[] Path = RetracePath2(start, end);   //start�� end������ �̵� ����Ʈ�� �����ϴ� �迭 path

            //================================================================================
            //������ �̵���� ǥ��

            //for (int i = 0; i < Path.Length; i++)
            //{
            //    Vector3 NodePos = Path[i];
            //    NodePos.y += 0.5f;


            //    GameObject DD = Instantiate(Path_, NodePos, Quaternion.Euler(90, 0, 0));

            //}

            //================================================================================


            Vector3 currentWaypoint = this.transform.position;

            while (true)
            {
                if (targetIndex < Path.Length)
                {
                    //Debug.Log("TargetIndex : " + targetIndex);
                    currentWaypoint = Path[targetIndex];

                    float AxisX = currentWaypoint.x - transform.position.x;
                    float AxisY = currentWaypoint.z - transform.position.z;
                    Vector3 Movedis = new Vector3(AxisX, 0, AxisY);

                    float SqrLen = Movedis.sqrMagnitude;
                    float SqrLen2 = Movedis.magnitude;

                    if (/*transform.position.x == currentWaypoint.x && transform.position.z == currentWaypoint.z)*/
                        SqrLen < 1.0f)
                    {
                        targetIndex++;
                        //Debug.Log(targetIndex);
                    }

                    this.transform.LookAt(new Vector3(currentWaypoint.x, this.transform.position.y, currentWaypoint.z));
                    this.transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

                    thisPos = this.transform.position;
                    thisPos.y = Grid.gridinstance.NodePoint(currentWaypoint, cellsize).YDepthLB + 0.6f;
                    this.transform.position = thisPos;

                    yield return null;

                }

                else yield break;

            }
            UO.UnitObstacle();

        }

    }

    private void FlyingTarget()
    {
        isMove = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            TargetPos_ = hit.point;
        }

        Movedis = new Vector3(TargetPos_.x, 0, TargetPos_.z);
        LookDir = new Vector3(TargetPos_.x, this.transform.position.y, TargetPos_.z);
        this.transform.LookAt(LookDir);
    }


    Vector3 TargetPos_ = Vector3.zero;
    public void FloyingMoveUnit(Vector3 Dir_)
    {



        Vector3 CurrentPos = new Vector3(this.transform.position.x, 0, this.transform.position.z);

        if (isMove)
        {
            var dir = Dir_ - CurrentPos;
            transform.position += dir.normalized * Time.deltaTime * baseStats.speed;
        }
        if (Vector3.Distance(CurrentPos, Dir_) <= 0.1f)
        {
            isMove = false;
        }

    }


}
