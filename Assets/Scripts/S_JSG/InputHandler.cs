using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Units.Player;
using UnityEngine.EventSystems;
namespace InputManager
{
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler instance = null;

        private RaycastHit hit;

        public List<Transform> selectedUnits = new List<Transform>();

        public Transform selectedBuilding = null;

        public LayerMask interactableLayer = new LayerMask();
        public LayerMask interactableLayer2 = new LayerMask();

        private bool isDragging = false;

        private Vector3 mousePos;
        private void Awake()
        {

            if (instance == null)
                instance = this;

        }
        void Start()
        {
            //instance = this;
        }

        // Update is called once per frame
        private void OnGUI()
        {
            if (isDragging)
            {
                Rect rect = MultiSelect.GetScreenRect(mousePos, Input.mousePosition);
                MultiSelect.DrawScreenRect(rect, new Color(0f, 0f, 0f, 0.25f));
                MultiSelect.DrawScreenRectBorder(rect, 3, Color.blue);
            }
        }
        public void HandleUnitMovement() // ����
        {


            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;

                }
                mousePos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100, interactableLayer))
                {
                    if (addeUnit(hit.transform, Input.GetKey(KeyCode.LeftShift)))
                    {
                        // be able to do stuff with units
                    }

                }
                else if (Physics.Raycast(ray, out hit, 100, interactableLayer2))
                {
                    if (addeBuilding(hit.transform))
                    {
                        //be able td do stuff with building
                    }

                }




                else
                {
                    isDragging = true;
                    DeselectUnits();
                }
            }
            if (Input.GetMouseButtonUp(0))
            {


                foreach (Transform child in RTS.Player.playerManager.instance.playerUnits)
                {
                    // foreach (Transform unit in child)
                    {
                        if (iswithinSelectionBounds(child))//�����ȿ� ������ �������
                        {
                            addeUnit(child, true); ;
                        }
                    }
                }
                isDragging = false;
            }
            if (Input.GetMouseButtonDown(1) && HaveSleletedUnits())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                int layerMask = 1 << LayerMask.NameToLayer("Sea");

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    LayerMask layerHit = hit.transform.gameObject.layer;

                    switch (layerHit.value)
                    {
                        case 6: //Units Layer

                            for (int i = 0; selectedUnits[i]; i++)
                            {
                                //selectedUnits[i].GetComponent<>().baseStats.    

                            }

                            break;
                        case 9: //enemy untis layer

                            break;
                        default:
                            foreach (Transform unit in selectedUnits)
                            {
                                PlayerUnit pU = unit.gameObject.GetComponent<PlayerUnit>();
                                Vector3 HitPos = hit.point;
                                Debug.Log("22");
                                // HitPos.y += 10f;
                                // pU.SetDestinatin(HitPos); // ��ǥ���� << �ٲٸ��
                                //selectedUnits
                                //  transform.LookAt(HitPos);

                            }
                            break;
                    }

                }

            }
            else if (Input.GetMouseButtonDown(1) && selectedBuilding != null)
            {

                if (selectedBuilding.GetComponent<Building.Player.PlayerBuilding>().baseStats.spawn == true)
                    selectedBuilding.gameObject.GetComponent<Building.BuildingSpawn>().SetSpawnMarkerLocation();//���콺 Ŭ���Ѱ� ���� ��ġ ����
            }
            else if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
            {




            }
        }
        //private void SelectUnit(Transform unit, bool canMultiselect = false) //���� ����
        //{
        //    if (!canMultiselect) //�����Ͻ�
        //    {
        //        DeselectUnits();

        //    }
        //    if (selectedUnits.Count < 12)
        //    {
        //        selectedUnits.Add(unit);
        //        unit.Find("Highlight").gameObject.SetActive(true);
        //    }

        //}
        private void DeselectUnits() //���� ����
        {
            if (selectedBuilding)
            {
                selectedBuilding.gameObject.GetComponent<Interactables.IBuilding>().OnInteractExit();
                selectedBuilding = null;
            }
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].gameObject.GetComponent<Interactables.IUnit>().OnInteractExit();
            }
            selectedUnits.Clear();
        }
        private bool iswithinSelectionBounds(Transform tf) //�巡�� ������ üũ
        {
            if (!isDragging)
            {
                return false;
            }
            Camera cam = Camera.main;
            Bounds vpBounds = MultiSelect.GetVPBounds(cam, mousePos, Input.mousePosition);
            return vpBounds.Contains(cam.WorldToViewportPoint(tf.position));
        }
        private bool HaveSleletedUnits() //������ ���� �ߴ���
        {
            if (selectedUnits.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private Interactables.IUnit addeUnit(Transform tf, bool canMultiselect = false)
        {
            Interactables.IUnit iUnit = tf.GetComponent<Interactables.IUnit>();
            if (iUnit)
            {
                if (!canMultiselect)
                {
                    DeselectUnits();
                }

                selectedUnits.Add(iUnit.gameObject.transform);

                iUnit.OnInteractEnter();

                return iUnit;

            }
            else
            {
                return null;
            }



        }


        private Interactables.IBuilding addeBuilding(Transform tf)
        {
            Interactables.IBuilding iBuilding = tf.GetComponent<Interactables.IBuilding>();

            if (iBuilding)
            {
                DeselectUnits();

                selectedBuilding = iBuilding.gameObject.transform;

                iBuilding.OnInteractEnter();

                return iBuilding;
            }
            else
            {
                return null;
            }

        }
        public void UnitSpawn(string objectToSpwan)
        {


            selectedBuilding.GetComponent<Building.BuildingSpawn>().StartSpawnTimer(objectToSpwan);


        }



    }



}
