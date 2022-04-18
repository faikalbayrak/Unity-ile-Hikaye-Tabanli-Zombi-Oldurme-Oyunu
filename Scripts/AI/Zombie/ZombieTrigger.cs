using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieTrigger : MonoBehaviour
{
    #region singleton

    public static ZombieTrigger instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    public bool canBeDamaged = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canBeDamaged = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            canBeDamaged = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canBeDamaged = false;
        }
    }
}
