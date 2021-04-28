using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;

public class SubordinateInterfaceSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [Header("角色陣列")]
    public GameObject[] HaveARole;//擁有角色

    public static bool[] UseOrNot;

    [SerializeField]
    [Header("選單按鈕物件")]
    public GameObject MenuButtonObject;

    [SerializeField]
    [Header("角色內容")]
    public GameObject CharacterContent;

    [SerializeField]
    [Header("暫存物件")]
    GameObject TemporaryObjects;

    public GameObject Return;//需要回傳角色
    Vector3 ButtonPosition=new Vector3(-50f,-40f,0f);
    void Start()
    {
        
        
        //CharacterContent=GameObject.Find("CharacterContent");
        
    }
    public void ManualStart(){
        //CharacterContent=GameObject.Find("CharacterContent");
        HaveARole = RoleStorageSystem.NowHasRole.ToArray();
        ButtonPosition = MenuButtonObject.transform.position - new Vector3(100f,0f,0f);
        foreach(GameObject HAR in HaveARole){
            TemporaryObjects=Instantiate(MenuButtonObject);
            TemporaryObjects.transform.parent=CharacterContent.transform;
            ButtonPosition+=new Vector3(100f,0f,0f);
            TemporaryObjects.transform.localPosition=ButtonPosition;
            TemporaryObjects.transform.localScale=new Vector3(1f,1f,1f);
            TemporaryObjects.GetComponent<ButtonObject>().ButtonsStart(HAR,gameObject);
            TemporaryObjects.GetComponent<Image>().sprite = HAR.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            //TemporaryObjects.transform.GetChild(0).GetComponent<Text>().text = HAR.transform.GetChild(0).GetComponent<RoleStatusModule>().g_RoleName;
        }
        UseOrNot=new bool[HaveARole.Length];
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public GameObject ReturnCharacter(){

        return Return;
    }
    public void SetReturn(GameObject Roles){
        if(System.Array.IndexOf(HaveARole,Roles)!=-1&&UseOrNot[System.Array.IndexOf(HaveARole,Roles)]==false){
            UseOrNot[System.Array.IndexOf(HaveARole,Roles)]=true;
            Return=Roles;
        }else{
            Return=null;
            //UseOrNot[System.Array.IndexOf(HaveARole,Roles)]=false;
        }
        //print(UseOrNot[System.Array.IndexOf(HaveARole,Roles)]);
    }
    public void RemoveReturn(GameObject Roles){
        if(System.Array.IndexOf(HaveARole,Roles)!=-1&&UseOrNot[System.Array.IndexOf(HaveARole,Roles)]==true){
            UseOrNot[System.Array.IndexOf(HaveARole,Roles)]=false;
            //Return=null;
        }else{
            //Return=null;
            //UseOrNot[System.Array.IndexOf(HaveARole,Roles)]=false;
        }
        //print(UseOrNot[System.Array.IndexOf(HaveARole,Roles)]);
    }
}
