using UnityEngine;

public class itemDestroyer : MonoBehaviour
{
    public bool destroy = false;
    public GameObject slotItem;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroy)
        {


            Destroy(this.gameObject);

            
        }

        
        

    }

}
