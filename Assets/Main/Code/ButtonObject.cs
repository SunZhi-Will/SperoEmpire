using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonObject : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField]
    [Header("按鈕存放角色")]
    public GameObject ButtonsStoreRoles;

    [SerializeField]
    [Header("回傳管理物件")]
    public GameObject SubordinateInterface;

    void Start()
    {
        
    }
    public void ButtonsStart(GameObject Roles,GameObject SIS)
    {
        ButtonsStoreRoles=Roles;
        //this.gameObject.GetComponent<Button>().RemoveAllListeners();
        SubordinateInterface=SIS;
        this.gameObject.GetComponent<Button>().onClick.AddListener (() => SIS.GetComponent<SubordinateInterfaceSystem>().SetReturn(ButtonsStoreRoles));
    }
    public void ButtonsSet()
    {
        
        gameObject.GetComponent<Button>().onClick.AddListener (() => SubordinateInterface.GetComponent<SubordinateInterfaceSystem>().SetReturn(ButtonsStoreRoles));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
