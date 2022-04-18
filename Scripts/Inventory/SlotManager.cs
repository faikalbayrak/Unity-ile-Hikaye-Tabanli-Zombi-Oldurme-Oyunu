using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    #region singleton

    public static SlotManager instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion
    public List<GameObject> Slots;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToSlot2(Transform item)
    {
        Transform createItem = Instantiate(item, Vector3.zero, Quaternion.identity);

        foreach (var _item in Slots)
        {
            if (!_item.GetComponent<SlotHover>().isFull)
            {
                createItem.transform.SetParent(_item.transform);
                createItem.transform.localPosition = new Vector3(0, 0, 0);
                _item.GetComponent<SlotHover>().isFull = true;
                break;
            }
            else
            {
                continue;
            }
        }
    }

    public void AddItemToSlot(GameObject item)
    {
        GameObject createItem = Instantiate(item);

        foreach (var _item in Slots)
        {
            if (!_item.GetComponent<SlotHover>().isFull)
            {
                Info.instance.ShowMessage(item.GetComponent<SlotItem>().itemToBeCreate.name + " added", 4f);
                createItem.transform.SetParent(_item.transform);
                createItem.transform.localPosition = new Vector3(0, 0, 0);
                _item.GetComponent<SlotHover>().isFull = true;
                break;
            }
            else
            {
                continue;
            }
        }
    }

    public bool isThereItem(string itemName)
    {
        bool result = false;
        foreach (var _item in Slots)
        {
            if(_item.transform.childCount != 0)
            {
                if (_item.transform.GetChild(0).name.Equals(itemName))
                {

                    result = true;
                    break;
                }
                else
                {
                    result = false;
                }
            }
            
        }

        Debug.Log(result);
        return result;
        
        
    }

    public void DeleteKey(string itemName)
    {

        foreach (var _item in Slots)
        {
            if (_item.transform.childCount != 0)
            {
                if (_item.transform.GetChild(0).name.Equals(itemName))
                {

                    _item.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
                    break;
                }
                else
                {

                }
            }

        }




    }
}
