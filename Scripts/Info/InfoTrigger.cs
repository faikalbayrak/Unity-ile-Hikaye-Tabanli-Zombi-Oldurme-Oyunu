using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTrigger : MonoBehaviour
{
    public string Message;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Info.instance.ShowMessage(Message,7f);
            this.gameObject.SetActive(false);
        }
    }
}
