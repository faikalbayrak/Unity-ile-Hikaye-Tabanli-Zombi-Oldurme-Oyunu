using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public SlotManager slotManager;
    Transform chest;
    public Transform pistolRayStart;
    public Vector3 offset;
    public Vector3 offset2;
    Vector3 rot;
    public Camera main;
    public GameObject hitEffect,bloodEffect;
    Door door;
    public float gunCoolDown = 0.5f;
    public GameObject[] screenIcons;
    float gunTimer = 0;
    public float speed = 10;
    float headRot = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        #region GunRay
        
            RaycastHit hitFire;
            Ray rayFire = main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if(gunTimer < 2) { gunTimer += Time.deltaTime; }

            if (Physics.Raycast(rayFire, out hitFire,1000f))
            {

                if (Player.instance.animator.GetBool("Aiming") && Player.instance.animator.GetBool("isPistol"))
                {
                    
                    chest = Player.instance.animator.GetBoneTransform(HumanBodyBones.Chest);
                    chest.LookAt(hitFire.point);
                    chest.rotation *= Quaternion.Euler(offset);
                    
                    
                    if (Input.GetKey(KeyCode.Mouse0) && !Player.instance.animator.GetBool("ReloadPistol") && gunTimer > gunCoolDown)
                    {
                        Debug.Log(hitFire.transform.gameObject.name);
                    
                        Player.instance.pistolAimingTransform.GetChild(0).GetComponent<AudioSource>().clip = Player.instance.pistolSounds[1];
                        Player.instance.pistolAimingTransform.GetChild(0).GetComponent<AudioSource>().Play();
                        Player.instance.pistolAimingTransform.GetChild(0).GetComponent<MuzzleEffect>().Effect();
                        Player.instance.pistolAimingTransform.GetChild(0).GetComponent<Animator>().SetTrigger("fire");

                        Recoil();

                        if(hitFire.transform.gameObject.tag != "Zombie")
                        {
                            GameObject _hitEffect = Instantiate(hitEffect, hitFire.point, Quaternion.LookRotation(hitFire.normal));
                            Destroy(_hitEffect, 1f);
                        
                        }
                        else if(hitFire.transform.gameObject.tag == "Zombie")
                        {
                            if(hitFire.transform.gameObject.GetComponent<ZombieAI>() != null)
                                hitFire.transform.gameObject.GetComponent<ZombieAI>().GetDamage(20);
                            if (hitFire.transform.gameObject.GetComponent<ZombieAI2>() != null)
                                hitFire.transform.gameObject.GetComponent<ZombieAI2>().GetDamage(20);
                            GameObject _bloodEffect = Instantiate(bloodEffect, hitFire.point, Quaternion.LookRotation(hitFire.normal));
                            Destroy(_bloodEffect, 1f);
                        
                        }
                    
                        gunTimer = 0;
                    

                    }
                }
                

            }

        #endregion

        #region DoorRay
        RaycastHit hitDoor;
        Ray rayDoor = main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(rayDoor.origin, rayDoor.direction * 2.6f, Color.red);
        if (Physics.Raycast(rayDoor, out hitDoor, 2.6f))
        {
            if(hitDoor.transform.gameObject.CompareTag("sewageDoor"))
            {
                
                screenIcons[1].SetActive(true);
                screenIcons[0].SetActive(false);
                door = hitDoor.transform.gameObject.GetComponent<Door>();
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Player.instance.animator.SetTrigger("Open");
                    if (!door.isLock && !door.isOpen && !door.temp)
                    {
                        
                        door.isOpen = true;
                        door.animator.SetBool("isOpen", door.isOpen);
                        door.As.clip = door.doorSounds[1];
                        door.As.Play();
                        door.temp = true;
                    }

                    if (!door.isLock && door.isOpen && !door.temp)
                    {
                        
                        door.isOpen = false;
                        door.animator.SetBool("isOpen", door.isOpen);
                        door.As.clip = door.doorSounds[2];
                        door.As.Play();
                        door.temp = true;
                    }

                    if (door.isLock && !door.temp && !SlotManager.instance.isThereItem("Slot_Item_sewageKey(Clone)"))
                    {
                        door.As.clip = door.doorSounds[0];
                        door.As.Play();
                        Info.instance.ShowMessage("it's locked", 3f);
                    }
                    else if (door.isLock && !door.temp && SlotManager.instance.isThereItem("Slot_Item_sewageKey(Clone)"))
                    {
                        door.As.clip = door.doorSounds[3];
                        door.As.Play();
                        Info.instance.ShowMessage("unlocked", 3f);
                        SlotManager.instance.DeleteKey("Slot_Item_sewageKey(Clone)");
                        door.isLock = false;
                    }

                    door.temp = false;
                }
                
            }

            



        }
        else
        {
            screenIcons[1].SetActive(false);
        }
        #endregion

        #region ItemTakeRay
        RaycastHit hitItem;
        Ray rayItem = main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(rayItem.origin, rayItem.direction * 4f, Color.red);

        if (Physics.Raycast(rayItem, out hitItem, 4f))
        {
            if (hitItem.transform.gameObject.CompareTag("pickUpTrigger"))
            {
                
                screenIcons[0].SetActive(true);
                screenIcons[1].SetActive(false);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SlotManager.instance.AddItemToSlot(hitItem.transform.gameObject.GetComponentInParent<itemDestroyer>().slotItem);
                    hitItem.transform.gameObject.GetComponentInParent<itemDestroyer>().destroy = true;
                }
                
            }

            

            
        }
        else
        {
            screenIcons[0].SetActive(false);
        }
        #endregion

        
        
    }

    void Recoil()
    {
        rot = chest.localRotation.eulerAngles;
        chest.localRotation = Quaternion.Euler(rot.x - 2, rot.y, rot.z);
        chest.localRotation = Quaternion.Slerp(chest.localRotation, Quaternion.Euler(rot.x,rot.y,rot.z), Time.deltaTime * 6);
    }

    
    
}
