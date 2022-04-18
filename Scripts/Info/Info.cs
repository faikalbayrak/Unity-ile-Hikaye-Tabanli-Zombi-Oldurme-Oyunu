using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    #region singleton

    public static Info instance;
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    [SerializeField]
    public GameObject InfoPanel;
    Text info;
    void Start()
    {
        InfoPanel.SetActive(false);
        info = InfoPanel.transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage(string message,float delay)
    {
        StartCoroutine(MessageLifeTime(message,delay));
    }

    IEnumerator MessageLifeTime(string message,float delay)
    {
        info.text = message;
        InfoPanel.SetActive(true);
        InfoPanel.transform.GetChild(0).GetComponent<Animator>().SetBool("Active", true);
        yield return new WaitForSeconds(delay);
        InfoPanel.transform.GetChild(0).GetComponent<Animator>().SetBool("Active", false);
        InfoPanel.SetActive(false);
    }
}
