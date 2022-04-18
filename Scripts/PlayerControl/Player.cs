using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Player : MonoBehaviour
{
    #region singleton

    public static Player instance;
    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    public PlayerStats stats = new PlayerStats();
    public bool isDead = false;
    CharacterController controller;
    Rigidbody physic;
    public BoxCollider carTrigger;
    public GameObject flashLightTransform;
    public Transform flashLightHoldingTransform;
    public GameObject InventoryPanel;
    public GameObject crosshair;
    public GameObject tpCamera;
    public GameObject tpAimCamera;
    public GameObject mainCam;
    public GameObject carCam;
    public GameObject car;
    public GameObject selectedItem;
    public GameObject Sun;
    public GameObject Rain;
    public GameObject QuestPanel;
    public string selectedItemName;
    public Animator animator;
    Vector3 velocity;
    AudioSource As;
    public AudioSource getHurtSource;
    bool isMoving;
    bool isGrounded;
    bool flashLightOpened = false;
    bool inventoryOpened = false;
    public bool isDriving = false;
    bool drivable = false;
    bool isWalking = false;
    bool isBaseballBatPushing = false;
    bool isRaining = false;
    bool RainingTemp = false;
   

    public Transform ground;
    public Transform leaner;
    public Transform pistolHoldingTransform, pistolAimingTransform;
    public Transform baseballBatHoldingTransform;

    public float zRot;
    bool isRotating;
    public float distance = 0.3f;

    float originalHeight = 1.8f;
    Vector3 originalCenter = new Vector3(0, 0.98f, 0);
    float crouchHeiht = 1.2f;
    Vector3 crouchCenter = new Vector3(0, 0.68f, 0);

    float timer;
    public float timeBetweenSteps;
    public float speed = 0f;
    public float jumpHeight;
    public float gravity;

    public LayerMask mask;
    float mouseX;
    float mouseY;
    float xRotation = 0f;
    float yRotation = 0f;
    float horizontal;
    float vertical;
    float roty;

    float SunTemprature = 4549;
    float SunIntensity = 9427;
    float RainTemprature = 7741;
    float RainIntensity = 500;
    float RainTimeInterval = 0;
    float RainTimeInterval2 = 0;
    float RainXScale = 1;

    public AudioClip[] pistolSounds;
    public AudioClip[] baseballBatSounds;
    public AudioClip[] getHurtSounds;

    AudioSource RainSound;
    Light sunLight;

    void Start()
    {
        //crosshair = GameObject.Find("Crosshair");
        //InventoryPanel = GameObject.Find("InventoryMenu");
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        physic = GetComponent<Rigidbody>();
        As = GetComponent<AudioSource>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        sunLight = Sun.GetComponent<Light>();
        RainSound = Rain.transform.GetChild(0).GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        
        if (animator.GetBool("Aiming"))
        {

            mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 70;
            mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * 70;



            xRotation -= mouseX;
            yRotation -= mouseY;

            

            Quaternion lookrotation = Quaternion.LookRotation(new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z));
            transform.localRotation = lookrotation;

            //transform.localRotation = Quaternion.Euler(transform.localRotation.x, -xRotation, transform.localRotation.z);
            //bone = animator.GetBoneTransform(HumanBodyBones.Chest);
            //bone.rotation = Quaternion.Euler(bone.localRotation.x + yRotation, bone.localRotation.y , bone.localRotation.z);





            
        }

        
        if (Vector3.Distance(transform.position, car.transform.position) <= 3)
        {
            if (!carTrigger.enabled)
                carTrigger.enabled = true;
        }
        else
        {
            if (carTrigger.enabled)
                carTrigger.enabled = false;
        }

        

        
        

    }
    // Update is called once per frame
    void Update()
    {

        
        if (stats.health <= 0)
        {
            isDead = true;
            animator.SetBool("isDead", true);
            physic.constraints = RigidbodyConstraints.FreezeAll;
            getHurtSource.PlayOneShot(getHurtSounds[3]);
        }
        if(horizontal != 0 || vertical != 0)
        {
            Quaternion lookrotation = Quaternion.LookRotation(new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z));
            transform.localRotation = Quaternion.Lerp(transform.localRotation, lookrotation, Time.deltaTime * 5);
            if (!animator.GetBool("Crouching"))
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }
        }
        else
        {
            isWalking = false;
        }
        if (Input.GetKeyDown(KeyCode.I))
            InventoryOpenClose();

        if (inventoryOpened)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                inventoryOpened = false;
            }
            QuestPanel.SetActive(false);
        }

        if (!this.gameObject.activeSelf)
        {
            isDriving = true;
        }

        

        if (inventoryOpened)
        {

            InventoryPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            crosshair.SetActive(false);
        }
        else
        {
            InventoryPanel.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
           
        }

        if (flashLightTransform.transform.childCount == 0)
        {
            animator.SetBool("FlashLight", false);
        }

        if (pistolAimingTransform.childCount == 0 && pistolHoldingTransform.childCount == 0)
        {
            animator.SetBool("Aiming", false);
        }

        if (!InventoryPanel.activeSelf && !isDead)
        {
            mainCam.GetComponent<CinemachineBrain>().enabled = true;
            tpCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;
            tpAimCamera.GetComponent<CinemachineVirtualCamera>().enabled = true;
            Movement();
            Jump();
            ReloadPistol();
            FoodSteps();
            Aiming();
            MeleeAttack();
            Crouch();
            if (Input.GetKeyDown(KeyCode.F) && !drivable)
            {
                int i = 0;
                foreach (var slot in SlotManager.instance.Slots)
                {
                    if (slot.transform.childCount != 0)
                    {
                        if (slot.transform.GetChild(0).name == "Slot_Item_flashlight")
                            if(selectedItemName.Contains("Flashlight"))
                                FlashLight(SlotManager.instance.Slots[i].transform.GetChild(0).GetComponent<SlotItem>());
                    }
                    i++;


                }

            }


            if (Input.GetKey(KeyCode.LeftShift) && !animator.GetBool("Crouching") && !animator.GetBool("Aiming"))
            {
                speed = 5f;
                timeBetweenSteps = 0.3f;
                animator.SetBool("Running", true);


            }
            else
            {
                speed = 2f;
                timeBetweenSteps = 0.5f;
                animator.SetBool("Running", false);
            }


            


            
           
        }
        else
        {
            mainCam.GetComponent<CinemachineBrain>().enabled = false;
            tpCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
            tpAimCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
        }


        Gravity();
        RainingStatus();






    }

    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        animator.SetFloat("Horizontal", horizontal);
        animator.SetFloat("Vertical", vertical);
      
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * speed * Time.deltaTime);
        
        
    }

    void Aiming()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && animator.GetBool("isPistol"))
        {
            int i = 0;
            foreach (var slot in SlotManager.instance.Slots)
            {
                if (slot.transform.childCount != 0)
                {
                    if (slot.transform.GetChild(0).name == "Slot_Item_Pistol")
                    {
                        if (selectedItemName.Contains("Pistol"))
                            PistolAiming(SlotManager.instance.Slots[i].transform.GetChild(0).GetComponent<SlotItem>());
                    }
                    i++;

                }



            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) && animator.GetBool("isPistol"))
        {
            int i = 0;
            foreach (var slot in SlotManager.instance.Slots)
            {
                if (slot.transform.childCount != 0)
                {
                    if (slot.transform.GetChild(0).name == "Slot_Item_Pistol")
                    {
                        if (selectedItemName.Contains("Pistol"))
                            PistolHolding(SlotManager.instance.Slots[i].transform.GetChild(0).GetComponent<SlotItem>());
                    }
                    i++;
                }



            }
        }

        if (animator.GetBool("Aiming"))
        {

            if (Input.GetKeyDown(KeyCode.F) && pistolAimingTransform.childCount != 0)
            {
                pistolAimingTransform.GetChild(0).GetComponentInChildren<FlashlightOnOff>().isOpened = !pistolAimingTransform.GetChild(0).GetComponentInChildren<FlashlightOnOff>().isOpened;
                pistolAimingTransform.GetChild(0).GetComponent<AudioSource>().clip = pistolSounds[0];
                pistolAimingTransform.GetChild(0).GetComponent<AudioSource>().Play();
            }


        }
    }

    void MeleeAttack()
    {
        if (animator.GetBool("isBaseballBat"))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("BaseballBatAttack");
            }

            if (Input.GetKeyDown(KeyCode.Space) && !isDriving && !animator.GetBool("Running") && !animator.GetBool("Crouching"))
            {
                animator.SetTrigger("BaseballBatPush");
                isBaseballBatPushing = true;
                StartCoroutine(BaseballBatPushingFalse());
            }
        }
    }

    IEnumerator BaseballBatPushingFalse()
    {
        yield return new WaitForSeconds(2f);
        isBaseballBatPushing = false;
    }
    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetBool("Crouching", true);

            
            speed = 1f;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetBool("Crouching", false);

            
            speed = 2f;
        }
    }

    public void setControllerHeight1()
    {
        controller.height = crouchHeiht;
        controller.center = crouchCenter;
    }

    public void setControllerHeight2()
    {
        controller.height = originalHeight;
        controller.center = originalCenter;
    }
    void Gravity()
    {
        isGrounded = Physics.CheckSphere(ground.position, distance, mask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    IEnumerator _Jump()
    {
        yield return new WaitForSeconds(0.5f);
        velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            animator.SetTrigger("Jump");
            StartCoroutine(_Jump());
        }
    }

    void PeekLook()
    {
        if (Input.GetKey(KeyCode.E))
        {
            zRot = Mathf.Lerp(zRot, -20.0f, 5f * Time.deltaTime);
            isRotating = true;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            zRot = Mathf.Lerp(zRot, 20.0f, 5f * Time.deltaTime);
            isRotating = true;
        }

        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q))
        {
            isRotating = false;
        }

        if (!isRotating)
        {
            zRot = Mathf.Lerp(zRot, 0.0f, 5f * Time.deltaTime);
        }

        leaner.localRotation = Quaternion.Euler(0, 0, zRot);
    }

    void ReloadPistol()
    {
        if(pistolHoldingTransform.childCount != 0 || pistolAimingTransform.childCount != 0)
        {
            if (Input.GetKey(KeyCode.R))
            {
                if(pistolHoldingTransform.childCount != 0)
                {
                    pistolHoldingTransform.GetChild(0).GetComponent<AudioSource>().clip = pistolSounds[2];
                    pistolHoldingTransform.GetChild(0).GetComponent<AudioSource>().Play();
                }
                else if (pistolAimingTransform.childCount != 0)
                {
                    pistolAimingTransform.GetChild(0).GetComponent<AudioSource>().clip = pistolSounds[2];
                    pistolAimingTransform.GetChild(0).GetComponent<AudioSource>().Play();
                }
                    
                animator.SetBool("ReloadPistol", true);
            }
        }

    }
    void ReloadPistolFalse()
    {
        animator.SetBool("ReloadPistol", false);
    }

    void FoodSteps()
    {
        if(horizontal != 0 || vertical != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving) 
        {
            timer -= Time.deltaTime;

            if(timer <= 0)
            {
                timer = timeBetweenSteps;
                As.pitch = Random.Range(0.85f, 1.15f);
                if(!animator.GetBool("Crouching"))
                    As.Play();
            }
        }
        else
        {
            timer = timeBetweenSteps;
        }
    }

    public void FlashLight(SlotItem _slotItem)
    {
       
        bool girdi = false;
        if (!flashLightOpened)
        {
            
            flashLightOpened = true;
            girdi = true;
        }

        else if(flashLightOpened && !girdi)
        {
            flashLightOpened = false;
            
            
        }
                
        

        if (flashLightOpened)
        {
            flashLightHoldingTransform.GetChild(0).GetComponent<itemDestroyer>().destroy = true;
            GameObject _flash = Instantiate(_slotItem.itemToBeCreate, flashLightTransform.transform.position, flashLightTransform.transform.rotation);
            _flash.transform.SetParent(flashLightTransform.transform);
            flashLightTransform.transform.GetChild(0).localPosition = new Vector3(0, 0, 0);
            flashLightTransform.transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
            flashLightTransform.transform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
            flashLightTransform.transform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            flashLightTransform.GetComponent<AudioSource>().Play();
            flashLightTransform.transform.GetChild(0).GetComponent<FlashlightOnOff>().isOpened = true;
            animator.SetBool("FlashLight", true);
            animator.SetBool("isPistol", false);

        }
        else
        {
            if(flashLightTransform.transform.childCount != 0)
            {
                GameObject _flash = Instantiate(_slotItem.itemToBeCreate, flashLightHoldingTransform);
                _flash.transform.SetParent(flashLightHoldingTransform);
                flashLightHoldingTransform.GetChild(0).localPosition = new Vector3(0, 0, 0);
                flashLightHoldingTransform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
                flashLightHoldingTransform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
                flashLightHoldingTransform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                flashLightTransform.transform.GetChild(0).gameObject.GetComponent<itemDestroyer>().destroy = true;
            }
                
            animator.SetBool("FlashLight", false);
        }
    }

    public void FlashLightHolding(SlotItem _slotItem)
    {
        GameObject _flash = Instantiate(_slotItem.itemToBeCreate, flashLightHoldingTransform);
        _flash.transform.SetParent(flashLightHoldingTransform);
        animator.SetBool("isBaseballBat", false);
        animator.SetBool("isPistol", false);
        flashLightHoldingTransform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        flashLightHoldingTransform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        flashLightHoldingTransform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
        flashLightHoldingTransform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        
        
    }

    public void BaseballBatHolding(SlotItem _slotItem)
    {
        GameObject _baseballBat = Instantiate(_slotItem.itemToBeCreate, baseballBatHoldingTransform);
        _baseballBat.transform.SetParent(baseballBatHoldingTransform);
        baseballBatHoldingTransform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        baseballBatHoldingTransform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        baseballBatHoldingTransform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
        baseballBatHoldingTransform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        animator.SetBool("isPistol", false);
        animator.SetBool("isBaseballBat", true);

    }

    public void PistolAiming(SlotItem _slotItem)
    {
        GameObject _pistol = Instantiate(_slotItem.itemToBeCreate, pistolAimingTransform);
        animator.SetBool("Aiming", true);
        animator.SetBool("isPistol", true);
        crosshair.SetActive(true);
        _pistol.transform.SetParent(pistolAimingTransform);
        pistolAimingTransform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        pistolAimingTransform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        pistolAimingTransform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
        pistolAimingTransform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (pistolHoldingTransform.childCount != 0)
            pistolHoldingTransform.GetChild(0).gameObject.GetComponent<itemDestroyer>().destroy = true;

    }
    public void PistolHolding(SlotItem _slotItem)
    {
        GameObject _pistol = Instantiate(_slotItem.itemToBeCreate, pistolHoldingTransform);
        animator.SetBool("Aiming", false);
        animator.SetBool("isPistol", true);
        animator.SetBool("isBaseballBat", false);
        crosshair.SetActive(false);
        _pistol.transform.SetParent(pistolHoldingTransform);
        pistolHoldingTransform.GetChild(0).localPosition = new Vector3(0, 0, 0);
        pistolHoldingTransform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        pistolHoldingTransform.GetChild(0).GetComponent<Rigidbody>().useGravity = false;
        pistolHoldingTransform.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        if (pistolAimingTransform.childCount != 0)
            pistolAimingTransform.GetChild(0).gameObject.GetComponent<itemDestroyer>().destroy = true;
    }

    void InventoryOpenClose()
    {
        
        
            bool girdi = false;
            if (!inventoryOpened)
            {

                inventoryOpened = true;
                girdi = true;
            }

            else if (inventoryOpened && !girdi)
            {
                inventoryOpened = false;
                

            }
    }

    private void getInCar()
    {
        carCam.SetActive(true);
        this.gameObject.SetActive(false);
        isDriving = true;
        car.GetComponent<CarController2>().enabled = true;
        CarController2.instance.isDriving = true;
        
        
    }

    public int GetPlayerStealthProfile()
    {
        if (isWalking)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public void getDamage(float damage)
    {
        animator.SetTrigger("getDamage");
        stats.health -= damage;
        if (stats.health < 0)
            stats.health = 0;

        getHurtSource.PlayOneShot(getHurtSounds[Random.Range(0, 2)]);
    }

    public void RainingStatus()
    {

        if (!isRaining)
        {
            RainTimeInterval += Time.deltaTime;
            if (RainTimeInterval >= 3000)
            {
                RainTimeInterval = 0;
                RainXScale = 1;
                Rain.transform.GetChild(0).localScale = new Vector3(RainXScale, 1, 1);
                RainSound.volume = 0.5f;
                Rain.transform.GetChild(0).gameObject.SetActive(true);
                isRaining = true;
                RainingTemp = false;
                

            }
            else if (RainTimeInterval >= 1800)
            {

                Rain.SetActive(true);
                sunLight.intensity = Mathf.MoveTowards(Sun.GetComponent<Light>().intensity, RainIntensity, 70);
                

            }
        }
        else
        {
            RainTimeInterval2 += Time.deltaTime;
            if (RainTimeInterval2 >= 30)
            {
                RainTimeInterval2 = 0;
                
                RainingTemp = true;
                
            }
            if (RainingTemp)
            {
                sunLight.intensity = Mathf.MoveTowards(Sun.GetComponent<Light>().intensity, SunIntensity, 40);
                RainXScale = Mathf.MoveTowards(RainXScale, 0, 0.005f);
                Rain.transform.GetChild(0).localScale = new Vector3(RainXScale, 1, 1);
                RainSound.volume = Mathf.Clamp(RainXScale,0,0.5f);
                if (sunLight.intensity == SunIntensity)
                {
                    Rain.SetActive(false);
                    Rain.transform.GetChild(0).gameObject.SetActive(false);
                    isRaining = false;
                }
            }
            

        }


    }

    

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "car")
        {
            Debug.Log("araba");
            drivable = true;
            if (Input.GetKey(KeyCode.F))
            {
                getInCar();
            }
        }
        else
        {
            drivable = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zombie" && isBaseballBatPushing)
        {
            if (other.transform.gameObject.GetComponent<ZombieAI>() != null)
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
