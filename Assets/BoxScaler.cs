using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScaler : MonoBehaviour
{
    // Start is called before the first frame update
    public float RanSize_X;
    public float RanSize_Y;
    public float RanSize_Z;

    void Scaler(GameObject obj)
    {
        obj.transform.localScale = new Vector3();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
