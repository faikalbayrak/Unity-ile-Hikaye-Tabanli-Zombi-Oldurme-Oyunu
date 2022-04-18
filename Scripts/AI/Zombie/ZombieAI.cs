using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    #region singleton

    public static ZombieAI instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion
    public bool isAware = false;
    private float health = 100;
    private bool isDead = false;
    public float fov = 120f;
    public float viewDistance = 10f;
    CapsuleCollider _collider;
    Animator animator;
    Rigidbody physic;
    AudioSource audioSource;
    public AudioClip[] sounds;
    public GameObject player;
    private NavMeshAgent agent;
    bool dontTail = false;

    ZombieTrigger zt;
    private void Start()
    {
        setRigidbodyState(true);
        setColliderState(false);
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        physic = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();
       
        zt = GetComponentInChildren<ZombieTrigger>();
    }

    private void LateUpdate()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < .9f)
            dontTail = true;
        else
            dontTail = false;

        if (Vector3.Distance(transform.position, player.transform.position) < 1.3f)
        {
            if (!Player.instance.isDead)
                animator.SetTrigger("Attack");
            else
            {
                StartCoroutine(YouCanEat());
            }

        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

    }
    public void Update()
    {
        
        if (health <= 0)
        {
            Dead();
        }
        if (isAware)
        {
            if (!isDead)
            {
                if (!dontTail)
                {
                    agent.speed = .3f;
                    agent.destination = player.transform.position;
                    
                    //transform.LookAt(player.transform);
                    //physic.constraints = RigidbodyConstraints.None;
                    //physic.constraints = RigidbodyConstraints.FreezeRotationX;
                    //physic.constraints = RigidbodyConstraints.FreezeRotationZ;
                } 
                else
                {
                    agent.speed = 0;
                    physic.constraints = RigidbodyConstraints.FreezeRotation;
                   
                }
                    
            }
                
        }
        else
        {
            SearchForPlayer();
        }

       

    }

    public void SearchForPlayer()
    {
        if (Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(player.transform.position)) < fov / 2f)
        {
            if (Vector3.Distance(player.transform.position, transform.position) < viewDistance)
            {
                if (!isDead)
                    OnAware();
            }
        }

    }
    
    public void Stumbling()
    {
        animator.SetBool("stumbling",true);
    }

    public void StumblingFalse()
    {
        animator.SetBool("stumbling", false);
    }



    public void OnAware()
    {
        animator.SetBool("isFound", true);
        StartCoroutine(GettingUp());
    }

    public void Dead()
    {
        isDead = true;
        agent.enabled = false;
        animator.SetBool("isDead", true);
        animator.enabled = false;
        setRigidbodyState(false);
        setColliderState(true);
        //StartCoroutine(FreezePosZombie());
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            health = 0;

    }

    public void SetZombieCollider()
    {
        if (isDead)
        {
            _collider.center = new Vector3(0.00389853f, 0.15f, -0.03498267f);
            _collider.direction = 2;
        }
        else
        {
            _collider.center = new Vector3(0.00389853f, 0.84f, -0.03498267f);
            _collider.direction = 1;
        }
    }

    public void ChangeTailingStatus()
    {
        if (!dontTail)
        {
            dontTail = true;
            animator.SetBool("isTailing", !dontTail);
        }
        else
        {
            dontTail = false;
            animator.SetBool("isTailing", !dontTail);
        }
            
    }

    public void TakeDamage(float damage)
    {
        Player.instance.getDamage(damage);
    }
    IEnumerator FreezePosZombie()
    {
        
        yield return new WaitForSeconds(3f);
        physic.constraints = RigidbodyConstraints.FreezeAll;
    }
    IEnumerator GettingUp()
    {
        
        yield return new WaitForSeconds(7.05f);
        isAware = true;
    }


    //IEnumerator Attack()
    //{
    //    if (temp)
    //        animator.SetBool("isAttacking", true);
    //    temp = false;
    //    if (animator.GetBool("isAttacking"))
    //    {
    //        if (Vector3.Distance(transform.position, player.transform.position) < 1)
    //            TakeDamage(1f);
    //        Debug.Log(Player.instance.stats.health);
    //        if (!Player.instance.isDead)
    //            animator.SetTrigger("Attack");
    //        else
    //        {
    //            yield return new WaitForSeconds(2);
    //            audioSource.clip = sounds[3];
    //            animator.SetBool("youCanEat", true);
    //        }
    //    }
    //    animator.SetBool("isAttacking", false);
    //    //attack sound
    //    yield return new WaitForSeconds(2);

    //    animator.SetBool("isAttacking", true);
    //}

    public void Attack()
    {
        animator.SetBool("isAttacking", true);
        if (zt.canBeDamaged)
            TakeDamage(20f);
        Debug.Log(Player.instance.stats.health);
    }

    IEnumerator YouCanEat()
    {
        yield return new WaitForSeconds(2f);
        audioSource.clip = sounds[3];
        animator.SetBool("youCanEat", true);
    }
    public void GettingUpSound()
    {
        audioSource.clip = sounds[0];
        audioSource.Play();
    }

    public void DeadSound()
    {
        audioSource.clip = sounds[2];
        audioSource.Play();
    }

    public void SearchingSound()
    {
        audioSource.clip = sounds[1];
        audioSource.Play();
    }


    void setRigidbodyState(bool state)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }

        GetComponent<Rigidbody>().isKinematic = !state;
    }

    void setColliderState(bool state)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.name != "Trigger")
                collider.enabled = state;
        }

        GetComponent<Collider>().enabled = !state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "car")
        {
            Dead();
        }
    }
}
