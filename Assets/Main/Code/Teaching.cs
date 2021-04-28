using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teaching : MonoBehaviour
{
    // Start is called before the first frame update
    public bool TeachingBool=false;//是否啟動新手教學
    public GameObject Teachings;//新手教學文字
    public int TeachingInt=0;
    //public GameObject Mask;
    void Start()
    {
        //新手教學
        Teachings=GameObject.FindWithTag("Teaching");
        Teachings.transform.GetChild(0).gameObject.SetActive(true);
        
        TeachingBool=false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown (0)&&TeachingInt==5) {
            Teachings.transform.GetChild(5).gameObject.SetActive(false);
            Teachings.transform.GetChild(6).gameObject.SetActive(true);
        }
    }
    public void MobileTeaching(){
        Teachings.transform.GetChild(0).gameObject.SetActive(false);
        Teachings.transform.GetChild(1).gameObject.SetActive(true);
        Teachings.transform.GetChild(2).gameObject.SetActive(true);
        //Mask.transform.GetChild(0).gameObject.SetActive(true);
        TeachingInt=2;
    }
    public void AttackTeaching(){
        Teachings.transform.GetChild(1).gameObject.SetActive(false);
        Teachings.transform.GetChild(2).gameObject.SetActive(false);
        Teachings.transform.GetChild(3).gameObject.SetActive(true);
        //Mask.transform.GetChild(0).gameObject.SetActive(true);
        TeachingInt=3;
        
    }
    public void SwitchRound(){
        Teachings.transform.GetChild(3).gameObject.SetActive(false);
        Teachings.transform.GetChild(4).gameObject.SetActive(false);
        Teachings.transform.GetChild(5).gameObject.SetActive(true);
        //Mask.transform.GetChild(0).gameObject.SetActive(true);
        TeachingInt=5;
        
    }
    
}
