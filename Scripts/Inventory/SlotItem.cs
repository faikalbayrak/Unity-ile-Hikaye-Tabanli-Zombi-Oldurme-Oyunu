using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotItem : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    #region singleton

    public static SlotItem instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion
    [SerializeField] private Item scriptableItem;
    public Image mainIcon;
    public GameObject description_canvas;
    public Text description;
    public GameObject itemToBeCreate;
    Transform dropPos;
    // Start is called before the first frame update
    void Start()
    {
        description_canvas.SetActive(false);
        mainIcon.sprite = scriptableItem.itemImage;
        description.text = scriptableItem.itemDescription;
        dropPos = Player.instance.gameObject.transform.GetChild(12).transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UseItem()
    {
        
        
        Debug.Log(scriptableItem.itemName + " kullanýldý");
        Player.instance.selectedItem.GetComponent<Image>().sprite = scriptableItem.itemImage;
        Player.instance.selectedItemName = scriptableItem.itemName;

        #region Pistol
            if (Player.instance.selectedItemName.Contains("Pistol"))
            {
                int i = 0;
                foreach (var slot in SlotManager.instance.Slots)
                {
                    if (slot.transform.childCount != 0)
                    {
                        if (slot.transform.GetChild(0).name == "Slot_Item_Pistol")
                            Player.instance.PistolHolding(SlotManager.instance.Slots[i].transform.GetChild(0).GetComponent<SlotItem>());
                    }
                    i++;


                }

            }
            else
            {
                if(Player.instance.pistolHoldingTransform.childCount != 0)
                    Player.instance.pistolHoldingTransform.GetChild(0).GetComponent<itemDestroyer>().destroy = true;
            }
        #endregion
        #region Flashlight
        if (Player.instance.selectedItemName.Contains("Flashlight"))
        {
            int i = 0;
            foreach (var slot in SlotManager.instance.Slots)
            {
                if (slot.transform.childCount != 0)
                {
                    if (slot.transform.GetChild(0).name == "Slot_Item_flashlight")
                        Player.instance.FlashLightHolding(SlotManager.instance.Slots[i].transform.GetChild(0).GetComponent<SlotItem>());
                }
                i++;


            }

        }
        else
        {
            if (Player.instance.flashLightHoldingTransform.childCount != 0)
                Player.instance.flashLightHoldingTransform.GetChild(0).GetComponent<itemDestroyer>().destroy = true;
        }
        #endregion
        #region BaseballBat
        if (Player.instance.selectedItemName.Contains("Baseball"))
        {
            int i = 0;
            foreach (var slot in SlotManager.instance.Slots)
            {
                if (slot.transform.childCount != 0)
                {
                    if (slot.transform.GetChild(0).name == "Slot_Item_BaseballBat(Clone)")
                        Player.instance.BaseballBatHolding(SlotManager.instance.Slots[i].transform.GetChild(0).GetComponent<SlotItem>());
                }
                i++;


            }

        }
        else
        {
            if (Player.instance.baseballBatHoldingTransform.childCount != 0)
                Player.instance.baseballBatHoldingTransform.GetChild(0).GetComponent<itemDestroyer>().destroy = true;
        }
        #endregion
    }

    public void DeleteItem()
    {
        GetComponentInParent<SlotHover>().isFull = false;
        if (scriptableItem.itemName.Contains("Flashlight"))
        {
            if(Player.instance.flashLightTransform.transform.childCount != 0)
            {
                Player.instance.flashLightTransform.transform.GetChild(0).gameObject.GetComponent<itemDestroyer>().destroy = true;
            }
            else if(Player.instance.flashLightHoldingTransform.childCount != 0)
            {
                Player.instance.flashLightHoldingTransform.GetChild(0).gameObject.GetComponent<itemDestroyer>().destroy = true;
            }
        }   
        if (scriptableItem.itemName.Contains("Pistol") && (Player.instance.pistolHoldingTransform.childCount != 0 || Player.instance.pistolAimingTransform.childCount != 0))
        {
            Player.instance.pistolHoldingTransform.GetChild(0).gameObject.GetComponent<itemDestroyer>().destroy = true;
            Player.instance.animator.SetBool("isPistol", false);
        } 
        if (scriptableItem.itemName.Contains("Baseball"))
        {
            if (Player.instance.baseballBatHoldingTransform.transform.childCount != 0)
            {
                Player.instance.baseballBatHoldingTransform.transform.GetChild(0).gameObject.GetComponent<itemDestroyer>().destroy = true;
                Player.instance.animator.SetBool("isBaseballBat", false);
            }
           
        }
        Debug.Log(scriptableItem.itemName + " silindi");
        Instantiate(itemToBeCreate, dropPos.position,Quaternion.Euler(90,0,0));
        itemToBeCreate.GetComponent<Rigidbody>().useGravity = true;
        itemToBeCreate.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

        if (Player.instance.selectedItemName == scriptableItem.itemName)
            Player.instance.selectedItem.GetComponent<Image>().sprite = null;

        Destroy(gameObject);
    }
    //public void KeyDeleter()
    //{
        
        
    //    Debug.Log(scriptableItem.itemName + " silindi");
        

    //    if (Player.instance.selectedItemName == scriptableItem.itemName)
    //        Player.instance.selectedItem.GetComponent<Image>().sprite = null;

    //    Destroy(gameObject);
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        description_canvas.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description_canvas.SetActive(false);
    }

    
}
