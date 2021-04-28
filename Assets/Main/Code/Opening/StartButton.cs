using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainCamera;

    public GameObject Starts;
    public GameObject Storys;
    [SerializeField]
    [Header("活動按鈕")]
    private GameObject ButtonActivity;
    public GameObject GoAway; 
    public GameObject RoleDirectoryButton;//角色目錄按鈕


    [SerializeField]
    [Header("名字")]
    private Text m_LName;

    [SerializeField]
    [Header("排行版")]
    private GameObject m_GloryMonument;
    //public GameObject RoleContent;
    [SerializeField]
    [Header("關卡按鈕動畫")]
    private Animator[] StoryButtonAnimator; //關卡按鈕動畫
    [SerializeField]
    [Header("角色目錄物件")]
    private GameObject RoleDirectoryGO;//角色目錄物件
    [SerializeField]
    [Header("現在關卡關卡")]
    private int Checkpoint=2;//關卡

    [SerializeField]
    [Header("切換")]
    private Animator[] m_CloudSwitching;


    [SerializeField]
    [Header("所有角色目錄按鈕圖")]
    private GameObject[] m_RolePTS;


    private Vector3 tp; //轉換畫面位置
    private Vector3 TaxiPosition;//確定轉換位置，並滑行數值

    [SerializeField]
    [Header("新手教學物件")]
    private GameObject Teaching;//新手教學物件
    private bool TeachingBool=false;//是否經歷過新手教學
    private Animator StartsAnimator; //開始按鈕動畫
    private Animator RDBAnimator;//角色按鈕動畫
    private Animator m_GloryMonumentAnimator;//角色按鈕動畫

    void Start()
    {
        
        tp=new Vector3(0f,0f);

        GameObject.FindWithTag("LevelDisplay").GetComponent<Text>().text=(int)((PlayerPrefs.GetInt("Checkpoint", Checkpoint)-2)/10)+"-"+(PlayerPrefs.GetInt("Checkpoint", Checkpoint)-2) % 10;

        
        Storys.SetActive(false);
        ButtonActivity.SetActive(false);
        Starts.SetActive(true);
        

        m_LName.text = PlayerPrefs.GetString("LName", "軍師");

        //按鈕動畫
        StartsAnimator = Starts.GetComponent<Animator>();
        RDBAnimator = RoleDirectoryButton.GetComponent<Animator>();
        //m_GloryMonumentAnimator = m_GloryMonument.GetComponent<Animator>();
        StartsAnimator.SetBool("FadeOut", true);
        RDBAnimator.SetBool("FadeOut",true);
        //m_GloryMonumentAnimator.SetBool("FadeOut",true);
        

        if(PlayerPrefs.HasKey("Teaching") || PlayerPrefs.GetInt("Checkpoint", 2) > 2){
            //Teaching.SetActive(false);
            TeachingBool=true;
            RoleDirectoryButton.SetActive(true); //新手教學完，才可開啟角色系統。
            //m_GloryMonument.SetActive(true); 
        }else{
            Teaching.transform.GetChild(0).gameObject.SetActive(true);
            TeachingBool=false;
            RoleDirectoryButton.SetActive(false); 
            //m_GloryMonument.SetActive(false); 
        }
        
        if(!RoleStorageSystem.StartB || !PlayerPrefs.HasKey("ArchiveRecords")){
            Debug.Log("yes");
            RoleStorageSystem.RoleImport(m_RolePTS,RoleDirectoryGO);
        }else{
            RoleStorageSystem.EffectAdjustment(m_RolePTS,RoleDirectoryGO);
            //RoleDirectoryGO.SetActive(false);
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        TaxiPosition=tp;//傳入滑行位置
        
        TaxiPosition= //計算滑行距離
            new Vector3(TaxiPosition.x - MainCamera.transform.position.x, TaxiPosition.y - MainCamera.transform.position.y);
        MainCamera.transform.Translate(TaxiPosition * 0.05f);//攝影機位置改變
        
        if(Input.GetKeyDown(KeyCode.Escape)){//是否按下離開鍵
            Escape();
        }
        
    }
    public void Escape(){//離開方法
        if(RoleDirectoryGO.activeInHierarchy){ //從角色目錄離開
            gameObject.GetComponent<ShowCharacterInformation>().Initialization();
            RoleDirectoryGO.SetActive(false);
            GoAway.SetActive(false);
        }else if(Storys.activeInHierarchy){//從選關回主城
            m_CloudSwitching[0].SetTrigger("ATrigger");
            m_CloudSwitching[1].SetTrigger("ATrigger");
            StartCoroutine(toMainCity());
        }else{//回開頭畫面
            print("ESC");
            SceneManager.LoadScene(0);
        }
    }
    public void click(){//從城鎮切換到選關畫面
        m_CloudSwitching[0].SetTrigger("ATrigger");
        m_CloudSwitching[1].SetTrigger("ATrigger");

        StartCoroutine(toCheckpoint());
    }
    private IEnumerator toCheckpoint(){
        yield return new WaitForSeconds(0.6f);
        Storys.SetActive(true);
        ButtonActivity.SetActive(true);

        StartsAnimator.SetBool("FadeOut", false);
        foreach (var item in StoryButtonAnimator)
        {
            item.SetBool("FadeOut", true);
        }
        
        tp=new Vector3(2.48f,-10.04f);
        if(!(TeachingBool)){
            Teaching.transform.GetChild(0).gameObject.SetActive(false);
            Teaching.transform.GetChild(1).gameObject.SetActive(true);
        }else{
            GoAway.SetActive(true);
        }

        Starts.SetActive(false);
        RoleDirectoryButton.SetActive(false);
        //m_GloryMonument.SetActive(false);

        StartCoroutine(Buffer());
    }
    private IEnumerator toMainCity(){
        yield return new WaitForSeconds(0.6f);
        tp=new Vector3(0f,0f);

        Starts.SetActive(true);
        StartsAnimator.SetBool("FadeOut", true);
        
        RoleDirectoryButton.SetActive(true);
        RDBAnimator.SetBool("FadeOut", true);

        //m_GloryMonument.SetActive(true);
        //m_GloryMonumentAnimator.SetBool("FadeOut", true);

        foreach (var item in StoryButtonAnimator)
        {
            item.SetBool("FadeOut", false);
        }
        Storys.GetComponent<CanvasGroup>().alpha = 0f;
        Storys.SetActive(false);
        GoAway.SetActive(false);
        StartCoroutine(Buffer());
    }



    public void click_Checkpoint(){//進入故事關卡按鈕
        if(PlayerPrefs.GetInt("Checkpoint", Checkpoint) >= 3){
            SceneManager.LoadScene(3);
        }else{
            SceneManager.LoadScene(PlayerPrefs.GetInt("Checkpoint", Checkpoint));
        }
        
        
    }
    public void click_Activity(){//進入故事關卡按鈕
        if(PlayerPrefs.GetInt("Activity", -1) >= 0){
            SceneManager.LoadScene("Activity");
        }
        
        
    }
    public void RoleDirectory(){
        //SceneManager.LoadScene("HallOfHeroes",LoadSceneMode.Additive);
        RoleDirectoryGO.SetActive(true);
        GoAway.SetActive(true);
    }


    private IEnumerator Buffer(){
        yield return new WaitForSeconds(0.6f);
        m_CloudSwitching[0].SetTrigger("ATrigger");
        m_CloudSwitching[1].SetTrigger("ATrigger");
    }




    //以廢
    public void AddRole(GameObject Role){
        
        RoleStorageSystem.AddRole(Role);
    }
}
