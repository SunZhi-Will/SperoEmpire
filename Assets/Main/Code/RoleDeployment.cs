using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleDeployment : MonoBehaviour
{
    [SerializeField]
    [Header("部屬介面")]
    public static GameObject SubordinateInterface;

    // Start is called before the first frame update
    void Start()
    {
        SubordinateInterface=GameObject.Find("SubordinateInterface");
        
        SubordinateInterface.GetComponent<SubordinateInterfaceSystem> ().ManualStart();
        SubordinateInterface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){//是否按下離開鍵
            Escape();
        }
    }
    public void Escape(){
        if(SubordinateInterface.activeInHierarchy){ //從角色目錄離開
            SubordinateInterface.SetActive(false);
        }else{
            LeaveMenu();
        }
        
    }
    public void LeaveMenu(){
        if(SubordinateInterface.activeInHierarchy){ //從角色目錄離開
            SubordinateInterface.SetActive(false);
        }
        
    }
}
