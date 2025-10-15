using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrgrid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        pos.x = (pos.x - pos.x % 1) + 0.5f;
        pos.y = (pos.y - pos.y % 1);
        pos.z = (pos.z - pos.z % 1);
        gameObject.transform.position = pos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

