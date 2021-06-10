using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class HitVirtualWall : MonoBehaviour
{
    //public StackAgent8 agent_script;
    public StackAgent8_1 agent_script;
    public GameObject Box;
    public int Index;
    // Start is called before the first frame update
    void Start()
    {
        //agent_script = GameObject.Find("BoxAgent").GetComponent<StackAgent8>();
        agent_script = GameObject.Find("BoxAgent").GetComponent<StackAgent8_1>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("Collide with wall, Set Reward -1");
            agent_script.SetReward(-0.1f);
            agent_script.EndEpisode();
        }

        if (collision.gameObject.CompareTag("Box"))
        {
            agent_script.Box_Stacked_list[Index] = true;
            //Debug.Log("Collide with Box" + Index);

        }

        else if (collision.gameObject.name == "StackOnPlane")
        {
            agent_script.Box_Stacked_list[Index] = true;
            //Debug.Log("Collide with Plane" + Index);

        }
        else if (collision.collider == null)
            agent_script.Box_Stacked_list[Index] = false;

    }
}
