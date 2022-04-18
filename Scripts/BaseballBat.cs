using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    private bool isAttacking = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Zombie" && !isAttacking)
        {
            if(other.transform.gameObject.GetComponent<ZombieAI>() != null)
            {
                other.transform.gameObject.GetComponent<ZombieAI>().Stumbling();
            }
            if (other.transform.gameObject.GetComponent<ZombieAI2>() != null)
            {
                other.transform.gameObject.GetComponent<ZombieAI2>().Stumbling();
            }
        }
    }
}
