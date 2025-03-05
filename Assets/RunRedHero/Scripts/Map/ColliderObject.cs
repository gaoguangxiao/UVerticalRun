using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //触发器
    private void OnTriggerEnter(Collider other)
    {
        
        //Debug.Log("gameobject" + other.name + other.gameObject);
        if(other.tag == "Food")
        {
            //other.gameObject.SetActive(false);
            Destroy(other.gameObject);
        }
    }
}
