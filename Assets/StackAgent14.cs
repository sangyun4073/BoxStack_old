using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.Assertions.Must;
using UnityEditor.Build;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.Barracuda;

public class StackAgent14 : Agent
{

    // Define Objects of Box, Rigidbody of Box
    public GameObject Boxes;
    public GameObject Box0, Box1, Box2, Box3, Box4, Box5, Box6, Box7, Box8, Box9;
    public GameObject Box10, Box11, Box12, Box13, Box14, Box15, Box16, Box17, Box18, Box19;
    public GameObject Box20, Box21, Box22, Box23, Box24, Box25, Box26, Box27, Box28, Box29;
    public GameObject Box30, Box31, Box32, Box33, Box34, Box35, Box36, Box37, Box38, Box39;
    public GameObject Box40, Box41, Box42, Box43, Box44, Box45, Box46, Box47, Box48, Box49;
    public GameObject Box50, Box51, Box52, Box53, Box54, Box55, Box56, Box57, Box58, Box59;
    public GameObject Box60, Box61, Box62, Box63;

    public Rigidbody rBox0, rBox1, rBox2, rBox3, rBox4, rBox5, rBox6, rBox7, rBox8, rBox9;
    public Rigidbody rBox10, rBox11, rBox12, rBox13, rBox14, rBox15, rBox16, rBox17, rBox18, rBox19;
    public Rigidbody rBox20, rBox21, rBox22, rBox23, rBox24, rBox25, rBox26, rBox27, rBox28, rBox29;
    public Rigidbody rBox30, rBox31, rBox32, rBox33, rBox34, rBox35, rBox36, rBox37, rBox38, rBox39;
    public Rigidbody rBox40, rBox41, rBox42, rBox43, rBox44, rBox45, rBox46, rBox47, rBox48, rBox49;
    public Rigidbody rBox50, rBox51, rBox52, rBox53, rBox54, rBox55, rBox56, rBox57, rBox58, rBox59;
    public Rigidbody rBox60, rBox61, rBox62, rBox63;

    // Define number of boxes for cube size 2x2x2, 3x3x3, 4x4x4
    public int boxesOfFloor; // 4 or 9 or 16
    public int numOfBoxes; // 8 or 27 or 64
    public int maxFloor; // 2, 3, 4
    public int floor; // present floor

    // Define Agent
    public GameObject BoxPointer;
    public Rigidbody r_BoxPointer;
    public bool CanStack;

    // Define Plane
    public GameObject Pallet;
    public Rigidbody r_Pallet;

    // Define List for sequential stacking
    public List<GameObject> Box_list;
    public List<Rigidbody> Stacked_Box_list;

    public List<Rigidbody> rb_list;
    public List<bool> Box_Stacked_list;
    public List<bool> MoveBoxTry_list;

    public int BoxIndex;
    public int pre_BoxIndex;
    public int CallAction;
    public float gravity;
    public float speed;
    public float Variance;

    public void AgentSpawn()
    {
        float PosX = 0.0f;
        float PosZ = 0.0f;
        BoxPointer.transform.position = new Vector3(r_Pallet.worldCenterOfMass.x + PosX,
                                            r_Pallet.worldCenterOfMass.y + 0.0625f,
                                            r_Pallet.worldCenterOfMass.z + PosZ);
    }

    public void ListInitialize()
    {
        Box_Stacked_list = new List<bool>()
        {
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false
        };

        MoveBoxTry_list = new List<bool>()
        {
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false, false, false, false, false, false, false,
        false, false, false, false
        };


        Box_list = new List<GameObject>()
        {
        Box0, Box1, Box2, Box3, Box4, Box5, Box6, Box7, Box8, Box9,
        Box10, Box11, Box12, Box13, Box14, Box15, Box16, Box17, Box18, Box19,
        Box20, Box21, Box22, Box23, Box24, Box25, Box26, Box27, Box28, Box29,
        Box30, Box31, Box32, Box33, Box34, Box35, Box36, Box37, Box38, Box39,
        Box40, Box41, Box42, Box43, Box44, Box45, Box46, Box47, Box48, Box49,
        Box50, Box51, Box52, Box53, Box54, Box55, Box56, Box57, Box58, Box59,
        Box60, Box61, Box62, Box63
        };

        rb_list = new List<Rigidbody>()
        {
        rBox0, rBox1, rBox2, rBox3, rBox4, rBox5, rBox6, rBox7, rBox8, rBox9,
        rBox10, rBox11, rBox12, rBox13, rBox14, rBox15, rBox16, rBox17, rBox18, rBox19,
        rBox20, rBox21, rBox22, rBox23, rBox24, rBox25, rBox26, rBox27, rBox28, rBox29,
        rBox30, rBox31, rBox32, rBox33, rBox34, rBox35, rBox36, rBox37, rBox38, rBox39,
        rBox40, rBox41, rBox42, rBox43, rBox44, rBox45, rBox46, rBox47, rBox48, rBox49,
        rBox50, rBox51, rBox52, rBox53, rBox54, rBox55, rBox56, rBox57, rBox58, rBox59,
        rBox60, rBox61, rBox62, rBox63
        };
        Stacked_Box_list = new List<Rigidbody> { };
    }

    // Episode Initialize
    public override void OnEpisodeBegin()
    {
        AgentSpawn();
        ListInitialize();
        CanStack = true;
        floor = 0;
        CallAction = 0;
        BoxIndex = 0;
        pre_BoxIndex = 0;
        Physics.gravity = new Vector3(0, -10, 0);
        Variance = 0;

        int edge = (numOfBoxes) / boxesOfFloor;
        //Debug.Log(edge);
        for (int y = 0; y < edge; y++)
        {
            for (int z = 0; z < edge; z++)
            {
                for (int x = 0; x < edge; x++)
                {
                    int index = y * edge * edge + z * edge + x;
                    Box_list[index].transform.position = new Vector3(r_Pallet.worldCenterOfMass.x - 1.3f + 0.5f * x,
                                                                 r_Pallet.worldCenterOfMass.y - 4 + 0.5f * y,
                                                                 r_Pallet.worldCenterOfMass.z + 0.5f * z);
                }
            }
        }

        for (int BoxIndex = numOfBoxes; BoxIndex < 64; BoxIndex++)
        {
            Box_list[BoxIndex].SetActive(false);
        }

        foreach (GameObject Box in Box_list)
        {
            Box.transform.rotation = new Quaternion(0, 0, 0, 0);
        }


        foreach (Rigidbody r_Box in rb_list)
        {
            r_Box.useGravity = false;
            r_Box.constraints = RigidbodyConstraints.FreezeAll;
        }

        r_BoxPointer.velocity = Vector3.zero;
        r_BoxPointer.angularVelocity = Vector3.zero;


    }

    // Define Observation
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(BoxPointer.transform.position);
        for (int i = 0; i < numOfBoxes; i++)
        {
            sensor.AddObservation(Box_list[i].transform.position);

        }
    }

    // Define Action
    // Action[0] = Move agent
    // Action[1] = Move Box

    public void AgentAction(ActionSegment<int> act)
    {
        float[] box_size = box_size_scaler(Box_list[BoxIndex]);

        //var dirToX = new Vector3(box_size[0], 0f, 0f);
        //var dirToY = new Vector3(box_size[0], 0f, 0f);
        //var dirToZ = new Vector3(0f, 0f, box_size[2]);

        var action = act[0];
        Vector3 next_pos = Vector3.zero;
        switch (action)
        {
            case 1:
                next_pos = new Vector3(this.transform.position.x + box_size[0], this.transform.position.y, this.transform.position.z);
                BoxPointer.transform.position = new Vector3(next_pos.x, next_pos.y, next_pos.z);
                break;
            case 2:
                next_pos = new Vector3(this.transform.position.x - box_size[0], this.transform.position.y, this.transform.position.z);
                BoxPointer.transform.position = new Vector3(next_pos.x, next_pos.y, next_pos.z);
                break;
            case 3:
                next_pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + box_size[2]);
                BoxPointer.transform.position = new Vector3(next_pos.x, next_pos.y, next_pos.z);
                break;
            case 4:
                next_pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - box_size[2]);
                BoxPointer.transform.position = new Vector3(next_pos.x, next_pos.y, next_pos.z);
                break;
            //case 5:
            //    next_pos = new Vector3(this.transform.position.x, this.transform.position.y + box_size[1], this.transform.position.z);
            //    BoxPointer.transform.position = new Vector3(next_pos.x, next_pos.y, next_pos.z);
            //    break;
            //case 6:
            //    next_pos = new Vector3(this.transform.position.x, this.transform.position.y - box_size[1], this.transform.position.z);
            //    BoxPointer.transform.position = new Vector3(next_pos.x, next_pos.y, next_pos.z);
            //    break;
        }
    }

    public void MoveBox(ActionSegment<int> act)
    {
        if (act[1] == 1)
        {
            float[] BoxSize = box_size_scaler(Box_list[BoxIndex]);
            if (CanStack) // 박스가 에이전트 좌표에 없다면
            {
                //Debug.Log("Teleport Box");
                Vector3 pos_BoxPointer = new Vector3(BoxPointer.transform.position.x, BoxPointer.transform.position.y, BoxPointer.transform.position.z);
                Box_list[BoxIndex].transform.position = new Vector3(pos_BoxPointer.x, pos_BoxPointer.y, pos_BoxPointer.z);
                rb_list[BoxIndex].constraints = RigidbodyConstraints.None;
                rb_list[BoxIndex].useGravity = true;
                Stacked_Box_list.Add(rb_list[BoxIndex]);
                if (BoxIndex > 0)
                {
                    if (CheckGathered())
                    {
                        SetReward((1f / numOfBoxes) * (BoxIndex));
                        //Debug.Log("Gathered SetReward : " + ((1f / numOfBoxes) * (BoxIndex)));
                        //AddReward(0.125f / (Variance + 0.00001f));
                    
                    }
                }

            }

        }
        else
        {
            //Debug.Log("Not move box");
        }
    }

    // ActionReceived
    public override void OnActionReceived(ActionBuffers actions)
    {

        CheckAgentBoundary();

        //if (BoxIndex == numOfBoxes)
        //{
        //    if (Variance < 3.0f)
        //    {
        //        Debug.Log("Success!!");
        //        SetReward(1.0f);
        //        EndEpisode();
        //    }
        //    else
        //    {
        //        AddReward(-0.5f);
        //        EndEpisode();
        //    }
        //}
        if (BoxIndex == (floor + 1) * boxesOfFloor) // 만약 박스를 한층의 개수만큼 쌓았을 때
        {

            bool floorDone = CheckStackedOneFloor(); // 높이와 모여있는 정도를 검사
            if (floorDone) // 만약 잘 쌓았다면 보상을 준다.
            {
                SetReward(1f / maxFloor * (floor + 1));
                Debug.Log("Set reward : " + 1f / maxFloor * (floor + 1));
                //Stacked_Box_list = new List<Rigidbody> { };
                floor += 1;
                //this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + floor * 0.125f, this.transform.position.z);
                BoxPointer.transform.position = new Vector3(r_Pallet.worldCenterOfMass.x, r_Pallet.worldCenterOfMass.y + floor * 0.31f, r_Pallet.worldCenterOfMass.z);
                if (BoxIndex == numOfBoxes)
                {
                    CheckBoxesBoundary();
                    if (Variance < 3.0f)
                    {
                        SetReward(1.0f);
                        Debug.Log("Success!!");
                        EndEpisode();
                    }
                    else
                    {
                        Debug.Log("Failed to stack all box");
                        EndEpisode();
                    }
                }
            }
            else // 
            {
                Debug.Log("Failed to stack one floor");
                //AddReward(-1.0f / numOfBoxes * boxesOfFloor *0.5f);
                EndEpisode();
            }
        }

        AgentAction(actions.DiscreteActions);


        if (!Box_Stacked_list[BoxIndex]) // 현재 박스가 쌓여있지 않을 경우
        {
            MoveBox(actions.DiscreteActions);
        }

        
        if (CheckStacked(BoxIndex))
        {
            pre_BoxIndex = BoxIndex;
            BoxIndex++;
            // Freeze preBox
            //rb_list[pre_BoxIndex].Sleep();
            //rb_list[pre_BoxIndex].sleepThreshold = 0;
            //rb_list[pre_BoxIndex].constraints = RigidbodyConstraints.FreezeAll;
        }

        CallAction++;
        AddReward(-0.5f / MaxStep);
        if (CallAction > 1000)
        {
            Debug.Log("Time Over");
            SetReward(-1.0f);
            EndEpisode();
        }

    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            CanStack = false;//Debug.Log("Box collide with something");
            //Debug.Log("Can't Stack");
        }
    }
    void OnTriggerExit(Collider other)
    {
        CanStack = true;//Debug.Log("Box collide with something");
        //Debug.Log("Can Stack");

    }

    bool CheckGathered()
    {
        bool gathered;
        Variance = 0.0f;
        float boundary = 3.0f;
        for (int i = 0; i < Stacked_Box_list.Count-1; i++)
        {
            for (int j = i+1; j < Stacked_Box_list.Count; j++)
            {
                Variance += Mathf.Pow(Vector3.Distance(Stacked_Box_list[i].worldCenterOfMass, Stacked_Box_list[j].worldCenterOfMass), 2.0f);
            }
        }
        if (Variance <= boundary)
            gathered = true;
        else
            gathered = false;
        return gathered;
    }

    bool CheckStackedOneFloor()
    {
        bool sameY = true;
        bool gathered = CheckGathered();
        for (int BoxIndex = 0 + floor * boxesOfFloor; BoxIndex < (floor + 1) * boxesOfFloor; BoxIndex++)
        {
            if (Mathf.Abs(rb_list[BoxIndex].worldCenterOfMass.y - r_Pallet.worldCenterOfMass.y) > 0.3f * (floor + 1))
            {
                sameY = false;
                Debug.Log("Not same Y");
                //Debug.Log(Mathf.Abs(rb_list[BoxIndex].worldCenterOfMass.y - r_Pallet.worldCenterOfMass.y));

            }
        }

        if (sameY && gathered)
        {
            Debug.Log("Gathered");
            return true;
        }
        else
            return false;

    }
    void CheckAgentBoundary()
    {
        if (Vector3.Distance(rb_list[0].worldCenterOfMass, r_BoxPointer.worldCenterOfMass) < 0.125f * maxFloor)
        {
            AddReward(1f / MaxStep);
        }

        if (Mathf.Abs(r_Pallet.worldCenterOfMass.x - r_BoxPointer.worldCenterOfMass.x) > 0.5f ||
                        r_BoxPointer.worldCenterOfMass.y - r_Pallet.worldCenterOfMass.y > 0.8f ||
                            r_BoxPointer.worldCenterOfMass.y - r_Pallet.worldCenterOfMass.y < 0f ||
                            Mathf.Abs(r_Pallet.worldCenterOfMass.z - r_BoxPointer.worldCenterOfMass.z) > 0.8f)
        {
            SetReward(-1.0f);
            Debug.Log("Agent out of bound");
            EndEpisode();
        }

    }

    void CheckBoxesBoundary()
    {
        foreach (Rigidbody r_Box in rb_list)
        {
            if (Mathf.Abs(r_Box.worldCenterOfMass.x - r_BoxPointer.worldCenterOfMass.x) > 0.5f ||
                        r_Box.worldCenterOfMass.y - r_Pallet.worldCenterOfMass.y > 1.0f ||
                            r_Box.worldCenterOfMass.y - r_Pallet.worldCenterOfMass.y < 0f ||
                            Mathf.Abs(r_Box.worldCenterOfMass.z - r_BoxPointer.worldCenterOfMass.z) > 0.8f)
            {
                //SetReward(-1.0f);
                Debug.Log("Some boxes out of bound. Failed....");
                EndEpisode();
            }
        }

        Debug.Log("All boxes in bound");
    }

    bool CheckStacked(int BoxIndex)
    {
        if (Box_Stacked_list[BoxIndex] == true)
        {
            return true;
        }
        else
            return false;
    }

    float CalcVariance(List<Rigidbody> StackedBoxList)
    {

        for (int i = 0; i < StackedBoxList.Count-1; i++)
        {
            float tempVar = 0.0f;
            for (int j = i+1; j < StackedBoxList.Count; j++)
            {
                tempVar += Mathf.Pow(Vector3.Distance(StackedBoxList[i].worldCenterOfMass, StackedBoxList[j].worldCenterOfMass), 2.0f);
            }
            Variance += tempVar;

        }
        return Variance;
    }


    float[] box_size_scaler(GameObject Box)
    {
        float[] Box_Size = new float[3]; // 가로, 세로, 높이
        //Box_Size[0] = Box.transform.lossyScale.x;
        //Box_Size[1] = Box.transform.lossyScale.y;
        //Box_Size[2] = Box.transform.lossyScale.z;
        Box_Size[0] = 0.25f;
        Box_Size[1] = 0.25f;
        Box_Size[2] = 0.23f;
        return Box_Size;
    }


    public override void Heuristic(in ActionBuffers actionOut)
    {
        var action = actionOut.DiscreteActions;
        action.Clear();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            action[0] = 3;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            action[0] = 4;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            action[0] = 1;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            action[0] = 2;
        }
        if (Input.GetKey(KeyCode.M))
        {
            action[1] = 1;
        }
    }

    IEnumerator WaitTime(float t)
    {
        yield return new WaitForSeconds(t);
    }
    // Start is called before the first frame update
    void Start()
    {
        Box0 = Boxes.transform.GetChild(0).gameObject;
        Box1 = Boxes.transform.GetChild(1).gameObject;
        Box2 = Boxes.transform.GetChild(2).gameObject;
        Box3 = Boxes.transform.GetChild(3).gameObject;
        Box4 = Boxes.transform.GetChild(4).gameObject;
        Box5 = Boxes.transform.GetChild(5).gameObject;
        Box6 = Boxes.transform.GetChild(6).gameObject;
        Box7 = Boxes.transform.GetChild(7).gameObject;
        Box8 = Boxes.transform.GetChild(8).gameObject;
        Box9 = Boxes.transform.GetChild(9).gameObject;
        Box10 = Boxes.transform.GetChild(10).gameObject;
        Box11 = Boxes.transform.GetChild(11).gameObject;
        Box12 = Boxes.transform.GetChild(12).gameObject;
        Box13 = Boxes.transform.GetChild(13).gameObject;
        Box14 = Boxes.transform.GetChild(14).gameObject;
        Box15 = Boxes.transform.GetChild(15).gameObject;
        Box16 = Boxes.transform.GetChild(16).gameObject;
        Box17 = Boxes.transform.GetChild(17).gameObject;
        Box18 = Boxes.transform.GetChild(18).gameObject;
        Box19 = Boxes.transform.GetChild(19).gameObject;
        Box20 = Boxes.transform.GetChild(20).gameObject;
        Box21 = Boxes.transform.GetChild(21).gameObject;
        Box22 = Boxes.transform.GetChild(22).gameObject;
        Box23 = Boxes.transform.GetChild(23).gameObject;
        Box24 = Boxes.transform.GetChild(24).gameObject;
        Box25 = Boxes.transform.GetChild(25).gameObject;
        Box26 = Boxes.transform.GetChild(26).gameObject;
        Box27 = Boxes.transform.GetChild(27).gameObject;
        Box28 = Boxes.transform.GetChild(28).gameObject;
        Box29 = Boxes.transform.GetChild(29).gameObject;
        Box30 = Boxes.transform.GetChild(30).gameObject;
        Box31 = Boxes.transform.GetChild(31).gameObject;
        Box32 = Boxes.transform.GetChild(32).gameObject;
        Box33 = Boxes.transform.GetChild(33).gameObject;
        Box34 = Boxes.transform.GetChild(34).gameObject;
        Box35 = Boxes.transform.GetChild(35).gameObject;
        Box36 = Boxes.transform.GetChild(36).gameObject;
        Box37 = Boxes.transform.GetChild(37).gameObject;
        Box38 = Boxes.transform.GetChild(38).gameObject;
        Box39 = Boxes.transform.GetChild(39).gameObject;
        Box40 = Boxes.transform.GetChild(40).gameObject;
        Box41 = Boxes.transform.GetChild(41).gameObject;
        Box42 = Boxes.transform.GetChild(42).gameObject;
        Box43 = Boxes.transform.GetChild(43).gameObject;
        Box44 = Boxes.transform.GetChild(44).gameObject;
        Box45 = Boxes.transform.GetChild(45).gameObject;
        Box46 = Boxes.transform.GetChild(46).gameObject;
        Box47 = Boxes.transform.GetChild(47).gameObject;
        Box48 = Boxes.transform.GetChild(48).gameObject;
        Box49 = Boxes.transform.GetChild(49).gameObject;
        Box50 = Boxes.transform.GetChild(50).gameObject;
        Box51 = Boxes.transform.GetChild(51).gameObject;
        Box52 = Boxes.transform.GetChild(52).gameObject;
        Box53 = Boxes.transform.GetChild(53).gameObject;
        Box54 = Boxes.transform.GetChild(54).gameObject;
        Box55 = Boxes.transform.GetChild(55).gameObject;
        Box56 = Boxes.transform.GetChild(56).gameObject;
        Box57 = Boxes.transform.GetChild(57).gameObject;
        Box58 = Boxes.transform.GetChild(58).gameObject;
        Box59 = Boxes.transform.GetChild(59).gameObject;
        Box60 = Boxes.transform.GetChild(60).gameObject;
        Box61 = Boxes.transform.GetChild(61).gameObject;
        Box62 = Boxes.transform.GetChild(62).gameObject;
        Box63 = Boxes.transform.GetChild(63).gameObject;

        rBox0 = Box0.GetComponent<Rigidbody>();
        rBox1 = Box1.GetComponent<Rigidbody>();
        rBox2 = Box2.GetComponent<Rigidbody>();
        rBox3 = Box3.GetComponent<Rigidbody>();
        rBox4 = Box4.GetComponent<Rigidbody>();
        rBox5 = Box5.GetComponent<Rigidbody>();
        rBox6 = Box6.GetComponent<Rigidbody>();
        rBox7 = Box7.GetComponent<Rigidbody>();
        rBox8 = Box8.GetComponent<Rigidbody>();
        rBox9 = Box9.GetComponent<Rigidbody>();
        rBox10 = Box10.GetComponent<Rigidbody>();
        rBox11 = Box11.GetComponent<Rigidbody>();
        rBox12 = Box12.GetComponent<Rigidbody>();
        rBox13 = Box13.GetComponent<Rigidbody>();
        rBox14 = Box14.GetComponent<Rigidbody>();
        rBox15 = Box15.GetComponent<Rigidbody>();
        rBox16 = Box16.GetComponent<Rigidbody>();
        rBox17 = Box17.GetComponent<Rigidbody>();
        rBox18 = Box18.GetComponent<Rigidbody>();
        rBox19 = Box19.GetComponent<Rigidbody>();
        rBox20 = Box20.GetComponent<Rigidbody>();
        rBox21 = Box21.GetComponent<Rigidbody>();
        rBox22 = Box22.GetComponent<Rigidbody>();
        rBox23 = Box23.GetComponent<Rigidbody>();
        rBox24 = Box24.GetComponent<Rigidbody>();
        rBox25 = Box25.GetComponent<Rigidbody>();
        rBox26 = Box26.GetComponent<Rigidbody>();
        rBox27 = Box27.GetComponent<Rigidbody>();
        rBox28 = Box28.GetComponent<Rigidbody>();
        rBox29 = Box29.GetComponent<Rigidbody>();
        rBox30 = Box30.GetComponent<Rigidbody>();
        rBox31 = Box31.GetComponent<Rigidbody>();
        rBox32 = Box32.GetComponent<Rigidbody>();
        rBox33 = Box33.GetComponent<Rigidbody>();
        rBox34 = Box34.GetComponent<Rigidbody>();
        rBox35 = Box35.GetComponent<Rigidbody>();
        rBox36 = Box36.GetComponent<Rigidbody>();
        rBox37 = Box37.GetComponent<Rigidbody>();
        rBox38 = Box38.GetComponent<Rigidbody>();
        rBox39 = Box39.GetComponent<Rigidbody>();
        rBox40 = Box40.GetComponent<Rigidbody>();
        rBox41 = Box41.GetComponent<Rigidbody>();
        rBox42 = Box42.GetComponent<Rigidbody>();
        rBox43 = Box43.GetComponent<Rigidbody>();
        rBox44 = Box44.GetComponent<Rigidbody>();
        rBox45 = Box45.GetComponent<Rigidbody>();
        rBox46 = Box46.GetComponent<Rigidbody>();
        rBox47 = Box47.GetComponent<Rigidbody>();
        rBox48 = Box48.GetComponent<Rigidbody>();
        rBox49 = Box49.GetComponent<Rigidbody>();
        rBox50 = Box50.GetComponent<Rigidbody>();
        rBox51 = Box51.GetComponent<Rigidbody>();
        rBox52 = Box52.GetComponent<Rigidbody>();
        rBox53 = Box53.GetComponent<Rigidbody>();
        rBox54 = Box54.GetComponent<Rigidbody>();
        rBox55 = Box55.GetComponent<Rigidbody>();
        rBox56 = Box56.GetComponent<Rigidbody>();
        rBox57 = Box57.GetComponent<Rigidbody>();
        rBox58 = Box58.GetComponent<Rigidbody>();
        rBox59 = Box59.GetComponent<Rigidbody>();
        rBox60 = Box60.GetComponent<Rigidbody>();
        rBox61 = Box61.GetComponent<Rigidbody>();
        rBox62 = Box62.GetComponent<Rigidbody>();
        rBox63 = Box63.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
