using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightOnOff : MonoBehaviour
{
    public bool isOpened = false;
    void Start()
    {
        transform.GetChild(0).gameObject.GetComponent<Light>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpened)
        {
            transform.GetChild(0).gameObject.GetComponent<Light>().enabled = true;
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Light>().enabled = false;
        }
    }
}
