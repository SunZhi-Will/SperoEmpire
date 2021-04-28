using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public static class RoleStorageSystem
{
    public static bool StartB=false;//是否初始化
    public static List<string> AllRoleFileName=new List<string>();
    public static List<GameObject> AllRole = new List<GameObject>(); //所有角色
    public static List<string> NowHasRoleFileName=new List<string>();
    public static List<GameObject> NowHasRole=new List<GameObject>();//現在擁有角色
    public static GameObject[] RoleB;
    public static GameObject RoleDGO;
    public static int Spirit = 0; //精神角色 (就是null角色數量)
    public static void Again(){
        StartB=false;
        AllRoleFileName=new List<string>();
        AllRole = new List<GameObject>();
        NowHasRoleFileName=new List<string>();
        NowHasRole=new List<GameObject>();//現在擁有角色
        Spirit = 0;
    }
    public static void RoleImport(GameObject[] RoleButton,GameObject RoleDirectoryGO){ //初始化
        StartB=true; //確定初始化
        RoleB=RoleButton; //將按鈕保存起來(比較方便呼叫)
        RoleDGO=RoleDirectoryGO;
        foreach (GameObject RB in RoleB) //利用按鈕名稱與物件名稱相同方式 導入角色物件
        {
            RB.GetComponent<Graphic>().color=new Color(1f, 1f, 1f, 0.5f);//當角色為未獲得時候 變成半透明
            AllRoleFileName.Add(RB.name); //存取角色名稱
            AllRole.Add(Resources.Load(RB.name) as GameObject);//存取角色物件
            if((PlayerPrefs.HasKey(RB.name) || NowHasRoleFileName.IndexOf(RB.name)>-1) &&PlayerPrefs.GetString(RB.name,"null")!="null"){
                NowHasRoleFileName.Add(RB.name);
                NowHasRole.Add(AllRole[AllRoleFileName.IndexOf(RB.name)]);
                //RB.transform.GetChild(0).gameObject.SetActive(false);
                RB.GetComponent<Graphic>().color=new Color(1f, 1f, 1f, 1f);
            }
        }

        
        if(!PlayerPrefs.HasKey("ArchiveRecords")){
            PlayerPrefs.SetInt("ArchiveRecords",1);
            //存取第一隻初始角色
            NowHasRoleFileName.Add(AllRoleFileName[0]);
            NowHasRole.Add(AllRole[0]);
            
            RoleB[0].GetComponent<Graphic>().color=new Color(1f, 1f, 1f, 1f);
            RoleB[0].transform.GetChild(0).gameObject.SetActive(false);

            //存檔
            PlayerPrefs.SetString(AllRoleFileName[0],AllRoleFileName[0]);
        }

        

        //這是以防物件還沒存取完就被隱藏
        RoleDGO.SetActive(false);
    }
    public static void AddRole(GameObject Role){ //新增擁有角色
        
        NowHasRoleFileName.Add(Role.name);
        NowHasRole.Add(Role);
        
        int x=AllRoleFileName.IndexOf(Role.name);
        if(x>-1 && RoleB[x] != null){
            //RoleB[x].transform.GetChild(0).gameObject.SetActive(false);
            RoleB[x].GetComponent<Graphic>().color=new Color(1f, 1f, 1f, 1f);
            
        }
        //存檔
        PlayerPrefs.SetString(Role.name,Role.name);
            
    }
    public static void RemoveRole(GameObject Role){
        if(NowHasRoleFileName.IndexOf(Role.name)>-1){
            
            NowHasRole.Remove(NowHasRole[NowHasRoleFileName.IndexOf(Role.name)]);
            NowHasRoleFileName.Remove(Role.name);
            PlayerPrefs.DeleteKey(Role.name);
        }
        
    }
    public static void EffectAdjustment(GameObject[] RoleButton,GameObject RoleDirectoryGO){
        
        RoleB=RoleButton; //將按鈕保存起來(比較方便呼叫)
        RoleDGO=RoleDirectoryGO;
        foreach (GameObject RB in RoleB){
            if(NowHasRoleFileName.IndexOf(RB.name)>-1 && PlayerPrefs.GetString(RB.name,"null")!="null"){
                //RB.transform.GetChild(0).gameObject.SetActive(false);
                RB.GetComponent<Graphic>().color=new Color(1f, 1f, 1f, 1f);
                
            }else{
                //RB.transform.GetChild(0).gameObject.SetActive(true);
                //RB.GetComponent<Graphic>().color=new Color(1f, 1f, 1f, 0.5f);
            }
        }
        RoleDGO.SetActive(false);
    }
}
