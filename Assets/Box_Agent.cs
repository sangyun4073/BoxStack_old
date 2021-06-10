using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.Assertions.Must;

public class Box_Agent : Agent
{
    public GameObject Box1;
    public GameObject Box2;
    public Rigidbody r_Box;
    public Transform Target;

    public int CountAction;
    // Start is called before the first frame update
    void Start()
    {
        //Box1 = GetComponent<GameObject>();
        //Target = GetComponent<Transform>();
        //r_Box = GetComponent<Rigidbody>();
        //Box1 = transform.GetChild(1).gameObject;
        //r_Box = GetComponent<Rigidbody>(); //GetComponent는 현재 오브젝트의 컴포넌트를 가져오는 메소드다.
        //r_Box = GetComponentInChildren<Rigidbody>();
     }


    public override void OnEpisodeBegin()
    {
        r_Box.angularVelocity = Vector3.zero;
        r_Box.velocity = Vector3.zero;
        r_Box.mass = 1;
        Box1.transform.position = new Vector3(0, 2f, 0);
        Box1.transform.rotation = new Quaternion(0, 0, 0, 0);

        Target.position = new Vector3(Random.value * 4 -2, 0.4f, Random.value * 4 - 2);
        r_Box.useGravity = false;
        r_Box.sleepVelocity = 0;
        r_Box.sleepAngularVelocity = 0;
        CountAction = 0;

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Target.position);
        sensor.AddObservation(Box1.transform.position);
        sensor.AddObservation(r_Box.velocity.x);
        sensor.AddObservation(r_Box.velocity.z);
        sensor.AddObservation(r_Box.velocity.y);


    }

    public float forceMultiplier = 10;

    [System.Obsolete]
    public override void OnActionReceived(float[] vectorAction)
    {
        float distanceToTarget_x = (Box1.transform.position.x - Target.position.x) * (Box1.transform.position.x - Target.position.x);
        float distanceToTarget_z = (Box1.transform.position.z - Target.position.z) * (Box1.transform.position.z - Target.position.z);
        float distanceToTarget_y = (Box1.transform.position.y - Target.position.y) * (Box1.transform.position.y - Target.position.y);
        float distanceToTarget = Vector3.Distance(Box1.transform.position, Target.position);
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        controlSignal.y = 0;

        Vector3 controlSignal2 = Vector3.zero;
        controlSignal2.x = 0;
        controlSignal2.z = 0;
        controlSignal2.y = vectorAction[2];

        if ((!(distanceToTarget_x < 0.01 && distanceToTarget_z < 0.01)) && CountAction < 100)
        {
            SetReward(-0.01f);
            Debug.Log("Set Reward : -0.01");

            r_Box.AddForce(controlSignal * forceMultiplier);
            CountAction++;
        }

        else if ((distanceToTarget_x < 0.01 && distanceToTarget_z < 0.01) && CountAction < 200)
        {
            CountAction++;
            //AddReward(-0.01f);
            //Debug.Log("Add Reward : -0.1/Max");

            //r_Box.AddForce(controlSignal2 * forceMultiplier); //동작하지 않는다.
            Stop(controlSignal);
            Resume();
            gravityOn();
            Debug.Log("Gravity On");

            if (distanceToTarget_y < 0.01f)
            {
                SetReward(1.0f);
                Debug.Log("Sucess !, Set Reward 1.0f");
                EndEpisode();
            }

            //else
            //EndEpisode();

        }

        else if (CountAction < 300)
        {
            CountAction++;
            if (CountAction == 300)
            {
                Debug.Log("Count Action  > 300 : EndEpisode");
                EndEpisode();
            }
        }

        if (Box1.transform.position.y > 4 || Box1.transform.position.y < 0) {
            Debug.Log("Out of Bound : y, Set Reward : -0.5 ");
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

    }

    public void gravityOn()
    {
        r_Box.useGravity = true;
        Physics.gravity = new Vector3(0, -100f, 0);
    }

    public void Stop(Vector3 speed)
    {
        Debug.Log("Stop Box");
        r_Box.mass = 99999;
        r_Box.velocity = Vector3.zero;
        r_Box.sleepVelocity = 1;
        r_Box.sleepAngularVelocity = 1;
        r_Box.angularVelocity = Vector3.zero;
        speed.x = 0;
        speed.z = 0;
        speed.y = 0;

    }

    public void Resume()
    {
        Debug.Log("Resume");
        r_Box.mass = 1;
        r_Box.sleepVelocity = 0;
        r_Box.sleepAngularVelocity = 0;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
