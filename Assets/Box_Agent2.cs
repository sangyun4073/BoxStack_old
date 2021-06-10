using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.Assertions.Must;
using TMPro;

public class Box_Agent2 : Agent
{
    public GameObject Box1;
    public GameObject Box2;
    public Rigidbody r_Box;
    public Rigidbody r_Box2;

    public Transform Target;
    public Transform Target2;


    public int CountAction;
    void Start()
    {
    }


    public override void OnEpisodeBegin()
    {
        r_Box.angularVelocity = Vector3.zero;
        r_Box.velocity = Vector3.zero;
        r_Box.mass = 1;

        r_Box2.angularVelocity = Vector3.zero;
        r_Box2.velocity = Vector3.zero;
        r_Box2.mass = 1;

        Box1.transform.position = new Vector3(2f, 2f, 2f);
        Box1.transform.rotation = new Quaternion(0, 0, 0, 0);
        Box2.transform.position = new Vector3(-2f, 4f, -2f);
        Box2.transform.rotation = new Quaternion(0, 0, 0, 0);

        Target.position = new Vector3(Random.value * 4 - 2, 0.4f, Random.value * 4 - 2);
        Target2.position = new Vector3(Target.position.x + 1, 0.4f, Target.position.z + 1);

        r_Box.useGravity = false;
        r_Box2.useGravity = false;
        Physics.gravity = new Vector3(0, -100f, 0);
        CountAction = 0;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
 
        sensor.AddObservation(Box1.transform.position);
        sensor.AddObservation(Box2.transform.position);
        sensor.AddObservation(Target.position - Box1.transform.position);
        sensor.AddObservation(Target2.position - Box2.transform.position);

    }

    
    public float Speed = 1;
    [System.Obsolete]
    public override void OnActionReceived(float[] vectorAction)
    {


        float distanceToTarget_x = (Box1.transform.position.x - Target.position.x) * (Box1.transform.position.x - Target.position.x);
        float distanceToTarget_z = (Box1.transform.position.z - Target.position.z) * (Box1.transform.position.z - Target.position.z);
        float distanceToTarget_y = (Box1.transform.position.y - Target.position.y) * (Box1.transform.position.y - Target.position.y);
        float distanceToTarget_x2 = (Box2.transform.position.x - Target2.position.x) * (Box2.transform.position.x - Target2.position.x);
        float distanceToTarget_z2 = (Box2.transform.position.z - Target2.position.z) * (Box2.transform.position.z - Target2.position.z);
        float distanceToTarget_y2 = (Box2.transform.position.y - Target2.position.y) * (Box2.transform.position.y - Target2.position.y);

        Vector3 moveXZ = new Vector3(vectorAction[0], 0, vectorAction[1]);
        Vector3 moveXZ_2 = new Vector3(vectorAction[2], 0, vectorAction[3]);
        Vector3 moveY = new Vector3(0, -100, 0);




        if ((!(distanceToTarget_x < 0.01 && distanceToTarget_z < 0.01)) && CountAction < 100)
        {
            AddReward(-1 / 2000f);
            Box1.transform.Translate(moveXZ * Speed);
            //Box1.transform.position = Vector3.MoveTowards(Box1.transform.position, Target.position, Speed);
            CountAction++;
        }

        else if ((distanceToTarget_x < 0.01 && distanceToTarget_z < 0.01)  &&  CountAction <200)
        {
            Stop(r_Box);
            Resume(r_Box);
            AddReward(0.25f);
            Debug.Log("MoveY Box1, Reward : 0.25");
            Box1.transform.Translate(moveY * Speed);
            r_Box2.useGravity = true;

            CountAction++;
        }
        if ((!(distanceToTarget_x2 < 0.01 && distanceToTarget_z2 < 0.01)) && CountAction < 300)
        {
            AddReward(-1 / 2000f);
            Box2.transform.Translate(moveXZ_2 * Speed);
            //Box2.transform.position = Vector3.MoveTowards(Box2.transform.position, Target2.position, Speed);

            CountAction++;
        }

        else if ((distanceToTarget_x2 < 0.01 && distanceToTarget_z2 < 0.01) && CountAction < 400)
        {
            Stop(r_Box2);
            Resume(r_Box2);
            AddReward(0.25f);
            Debug.Log("MoveY Box2, Reward : 0.5");
            Box1.transform.Translate(moveY * Speed);
            r_Box.useGravity = true;
            CountAction++;
        }
        else if (CountAction < 500)
        {
            CountAction++;
            if (CountAction == 500)
            {
                Debug.Log("Count Action  > 1000 : EndEpisode");
                EndEpisode();
            }
        }

        //if (Done(distanceToTarget_y, distanceToTarget_y2))
        //{
        //    SetReward(1f);
        //    Debug.Log("Success!!! Reward : 1");
        //    EndEpisode();
        //}

        if(Done2(Box1.transform, Target) && Done2(Box2.transform, Target2))
        {
            SetReward(1f);
            Debug.Log("Success!!! Reward : 1");
            EndEpisode();
        }
        if (Box1.transform.position.y > 4 )
        {
            Debug.Log("Out of Bound : y, Set Reward : -0.5");
            SetReward(-0.5f);
            EndEpisode();
        }
        if (!((Box1.transform.position.x < 5f && Box1.transform.position.x > -5f)
    || (Box1.transform.position.z < 5f && Box1.transform.position.z > -5f)))
        {
            Debug.Log("Out of Bound : x, z, Set Reward : -0.5");
            SetReward(-0.5f);
            EndEpisode();
        }
        if (Box2.transform.position.y > 4)
        {
            Debug.Log("Out of Bound : y, Set Reward : -0.5");
            SetReward(-0.5f);
            EndEpisode();
        }
        if (!((Box2.transform.position.x < 5f && Box2.transform.position.x > -5f)
    || (Box2.transform.position.z < 5f && Box2.transform.position.z > -5f)))
        {
            Debug.Log("Out of Bound : x, z, Set Reward : -0.5");
            SetReward(-0.5f);
            EndEpisode();
        }
    }
    public void Stop(Rigidbody r)
    {
        Debug.Log("Stop Box");
        r.mass = 99999;
        r.velocity = Vector3.zero;
        r.sleepVelocity = 1;
        r.sleepAngularVelocity = 1;
        r.angularVelocity = Vector3.zero;
    }
    public void Resume(Rigidbody r)
    {
        Debug.Log("Resume");
        r.mass = 1;
        r.sleepVelocity = 0;
        r.sleepAngularVelocity = 0;
    }
    bool Done(float a, float b)
    {
        if (a < 0.001 && b < 0.001)
            return true;
        else
            return false;
    }

    bool Done2(Transform a, Transform b)
    {
        float distanceToTarget = Vector3.Distance(a.position, b.position);
        if (distanceToTarget < 0.1)
        {
            return true;
        }
        else
            return false;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
