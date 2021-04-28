using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleStatusModule : MonoBehaviour
{
    [SerializeField]
    [Header("管理物件")]
    public GameObject RMS;
    
    [SerializeField]
    [Header("名稱")]
    public string Name;

    [SerializeField]
    [Header("血量")]
    public int HP=2;

    [SerializeField]
    [Header("血量上限")]
    public int HP_Max=2;

    public GameObject[] HP_UI;

    [SerializeField]
    [Header("攻擊力")]
    public int AttackInitialValue=1;

    [SerializeField]
    [Header("受傷特效")]
    private GameObject m_InjuryEffects;


    [SerializeField]
    [Header("護盾值")]
    private int m_Shield;
    [SerializeField]
    [Header("護盾UI")]
    private GameObject[] m_Shield_UI;

    


    [SerializeField]
    [Header("角色名稱")]
    public string g_RoleName;

    
    [Header("角色名稱顯示")]
    private Text m_RoleNameText;

    [SerializeField]
    [Header("受到保護")]
    public bool g_Roleprotected;



    //public static int AttackPower=1;
    
    // Start is called before the first frame update
    void Start()
    {
        Name=gameObject.name;
        HP_UI=new GameObject[5];
        m_Shield_UI=new GameObject[5];
        for(int i=0; i<5; i++){
            HP_UI[i]=transform.GetChild(0).gameObject.transform.GetChild(i).gameObject;
            m_Shield_UI[i]=transform.GetChild(1).gameObject.transform.GetChild(i).gameObject;
        }
        for(int i=HP_UI.Length; (i>HP && i-1>0); i--){
            if(i>HP_Max){
                HP_UI[i-1].gameObject.SetActive(false);
            }else{
                HP_UI[i-1].transform.GetChild(0).gameObject.SetActive(false);
            }
            
        }
        for(int i=m_Shield_UI.Length; i>m_Shield; i--){
            m_Shield_UI[i-1].gameObject.SetActive(false);
        }
        RMS=GameObject.Find("RoleManagementSubsystem");

        m_RoleNameText = GameObject.Find("Role_Name").GetComponent<Text>();
        //print(AttackPower);
    }

    public void DisplayText()
    {
        m_RoleNameText.text = g_RoleName;
    }

    public void NoDisplayText(){
        if(m_RoleNameText.text.IndexOf(g_RoleName) > -1){
            m_RoleNameText.text = "";
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        if(HP<=0){
            //transform.GetChild(0).gameObject.SetActive(false);
           RMS.GetComponent<RMS_1_1_2>().CharacterKilled(gameObject.transform.parent.gameObject);
            
        }
        
    }
    public void AttackChange(GameObject Enemy){
        //print(AttackPower);
        
        Enemy.transform.GetChild(0).GetComponent<RoleStatusModule>().BeAttacked(AttackInitialValue);
        if(Name=="Greedy"){
           
            this.BeAttacked(-1);
            this.gameObject.GetComponent<Greedy>().Special(Enemy);
        }
        
        //Enemy.transform.GetChild(1).gameObject.SetActive(true);
        //Enemy.transform.GetChild(1).GetComponent<Animator>().SetBool("Attack",true);
        //StartCoroutine(WaitAndPrint(1f,Enemy));
        
        
        //Enemy.GetComponent<RoleStatusModule>().BeAttacked(AttackPower);
    }
    IEnumerator WaitAndPrint(float waitTime,GameObject Enemy)//等待  
    {  
        yield return new WaitForSeconds(waitTime);  
        //等待之後
        Enemy.transform.GetChild(1).GetComponent<Animator>().SetBool("Attack",false);
        Enemy.transform.GetChild(1).gameObject.SetActive(false);
    }
    public void BeAttacked(int Injured){
        
        if(Injured > 0 && !g_Roleprotected){
            StartCoroutine(BufferEffects(m_InjuryEffects));
        }
        if((!g_Roleprotected || Injured < 0) && m_Shield == 0){
            HP-=Injured;
            if(HP>=HP_Max)
                HP=HP_Max;
            for(int i=0;i<HP_UI.Length;i++){
                if(HP>i){
                    HP_UI[i].transform.GetChild(0).gameObject.SetActive(true);
                }else{
                    HP_UI[i].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }
        else if(m_Shield > 0){
            ProtectionDefense(Injured);
        }
        else{
            GameObject.FindGameObjectWithTag("Rig").GetComponent<RoleStatusModule>().ProtectionDefense(Injured);

        }
        
        
        /*for(int i=HP_UI.Length; (i>HP && i-1>0); i--){
            HP_UI[i-1].transform.GetChild(0).gameObject.SetActive(false);
        }*/
        
    }
    public void ProtectionDefense(int _Injured){
        m_Shield-=_Injured;
        if(m_Shield < 0){
            HP+=m_Shield;
        }
        for(int i=0;i<m_Shield_UI.Length;i++){
            if(m_Shield>i){
                m_Shield_UI[i].gameObject.SetActive(true);
            }else{
                m_Shield_UI[i].gameObject.SetActive(false);
            }
        }

    }



    private IEnumerator BufferEffects(GameObject _IE){
        Debug.Log("WW");
        _IE.SetActive(true);
        yield return new WaitForSeconds(0.40f);
        _IE.SetActive(false);
    }
}
