using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    #region singleton

    public static Door instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion
    public Animator animator;
    public bool isLock = false;
    public bool isOpen = false;
    public bool temp = false;
    public AudioSource As;
    public AudioClip[] doorSounds;
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", isOpen);
        As = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    
}
