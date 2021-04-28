using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RMS_1_1_2 : MonoBehaviour
{
    


    public float  hp;
    [SerializeField]
    [Header("場上所有角色")]
    public static GameObject[][] AllPlayingCharacters;



    [SerializeField]
    [Header("場上所有角色位置")]
    public static Vector3Int[][] RoleLocation;

    [SerializeField]
    [Header("敵人數量上限")]
    public static int MaximumNumberOfEnemies;

    [SerializeField]
    [Header("我方數量上限")]
    public static int MaximumOurQuantity;

    [SerializeField]
    [Header("場上敵人數量")]
    public static int NumberOfEnemies;
    
    [SerializeField]
    [Header("場上我方數量")]
    public static int OurQuantity;

    [SerializeField]
    [Header("剩餘次數")]
    public static int TotalRounds;

    [SerializeField]
    [Header("是否開始遊戲")]
    public static bool StartTheGame=false; //是否開始遊戲

    [SerializeField]
    [Header("行動隊伍")]
    public static bool ActionTeam=false; //真代表玩家 假代表敵方

    [SerializeField]
    [Header("選取角色")]
    public static GameObject SelectRole;

    [SerializeField]
    [Header("攻擊顯示地圖")]
    public static GameObject AttackRange;

    public GameObject WIN;

    int Rounds=1;

    [SerializeField]
    [Header("回合切換提示")]
    private Image m_RoundSwitch;
    [SerializeField]
    [Header("回合切換文字")]
    private Text m_RoundSwitchText;

    [SerializeField]
    [Header("總回合數")]
    private Text m_RoundText;

    
    [SerializeField]
    [Header("攝影機")]
    private Camera m_CameraPosition;

    [SerializeField]
    [Header("攝影機動畫")]
    private Animator m_CameraPositionAnimator;

    [SerializeField]
    [Header("是否為活動")]
    private bool _ActivityBool;
    
    void Start()
    {
        StartTheGame=false;
        AttackRange=GameObject.Find("AttackRange");
        m_CameraPositionAnimator = m_CameraPosition.GetComponent<Animator>();
        //AttackRange.SetActive(false);
    }
    
    public void StartLevel(){
        ActionTeam = false;


        GameObject[] RoleLattice=GameObject.FindGameObjectsWithTag("RoleLattice-Sheet");
        
        AllPlayingCharacters=new GameObject[2][];
        AllPlayingCharacters[0]=GameObject.FindGameObjectsWithTag("RolePlay");
        AllPlayingCharacters[1]=GameObject.FindGameObjectsWithTag("HostileRole");
        
        RoleLocation=new Vector3Int[2][];
        RoleLocation[0]=new Vector3Int[AllPlayingCharacters[0].Length];
        RoleLocation[1]=new Vector3Int[AllPlayingCharacters[1].Length];

        OurQuantity=AllPlayingCharacters[0].Length;
        NumberOfEnemies=AllPlayingCharacters[1].Length;
        for(int i=0;i<RoleLattice.Length;i++){
            Destroy(RoleLattice[i]);
        }
        for(int i=0;i<AllPlayingCharacters[0].Length;i++){
            RoleLocation[0][i] = AllPlayingCharacters[0][i].GetComponent<CharacterControlModule>().InitialDataTransfer(new int[2]{0,i});
            AllPlayingCharacters[0][i].GetComponent<CharacterControlModule>().StartLevel(m_CameraPosition, m_CameraPositionAnimator);
            
        }
        for(int i=0;i<AllPlayingCharacters[1].Length;i++){
            RoleLocation[1][i] = AllPlayingCharacters[1][i].GetComponent<EnemyIntelligenceSystem>().InitialDataTransfer(new int[2]{1,i});
            AllPlayingCharacters[1][i].GetComponent<EnemyIntelligenceSystem>().StartLevel(m_CameraPosition, m_CameraPositionAnimator);
        }
        
        
        SelectRole=AllPlayingCharacters[0][0];
        
        SwitchTeams(ActionTeam);//開始回合
        StartTheGame=true;
    }
    
    // Update is called once per frame
    private bool _win= false;
    void Update()
    {
        if(StartTheGame){
            if(TotalRounds==0){
                StartCoroutine(WaitAndPrint(0.5f));
            }
            

            if(OurQuantity==0){ //關卡失敗
                print("關卡失敗");
                SceneManager.LoadScene(1);
            }else if(NumberOfEnemies==0 && !_win){ //闖關成功
                _win = true;
                print("關卡成功");
                WIN.SetActive(true);
                if(!_ActivityBool){
                    PlayerPrefs.SetInt("Checkpoint", (PlayerPrefs.GetInt("Checkpoint", 2)+1));
                }
                

                if(GetComponent<LevelLoading>() != null)
                    GetComponent<LevelLoading>().EarnRewards();
                

                GetComponent<MemberSystem>().Record();



                
            }
        }

    }
    IEnumerator WaitAndPrint(float waitTime)//等待  
    {  
        yield return new WaitForSeconds(waitTime);  
        //等待之後
        if(TotalRounds==0){ //二度確認
            SwitchTeams(ActionTeam);
        }
    }  

    private bool m_Switching = false; //正在切換
    private void SwitchTeams(bool ActionTeams){
        if(!m_Switching && !_win){
            m_Switching = true;
            Rounds++;
            TotalRounds=0;


            m_RoundSwitch.gameObject.SetActive(true);
            if(ActionTeams){
                ActionTeam=false;
                m_RoundSwitch.color = Color.red;
                m_RoundSwitchText.text = "敵方回合";

                StartCoroutine(RoundSwitchingAnimationEnemy());

            }else{
                ActionTeam=true;

                m_RoundSwitch.color = Color.cyan;
                m_RoundSwitchText.text = "我方回合";
                StartCoroutine(RoundSwitchingAnimationPlaying());
                
            }
            m_RoundText.text = "Rounds:" + (int)(Rounds/2);
        }
    }
    private IEnumerator RoundSwitchingAnimationEnemy(){

        yield return new WaitForSeconds(1f);  
        m_RoundSwitch.gameObject.SetActive(false);
        for(int i=0;i<AllPlayingCharacters[1].Length;i++){
                AllPlayingCharacters[1][i].GetComponent<EnemyIntelligenceSystem>().ControlTimes = 1;
        }
        TotalRounds=NumberOfEnemies;
        m_Switching = false;
    }
    private IEnumerator RoundSwitchingAnimationPlaying(){
        
        yield return new WaitForSeconds(1f);  
        m_RoundSwitch.gameObject.SetActive(false);
        for(int i=0;i<AllPlayingCharacters[0].Length;i++){
            AllPlayingCharacters[0][i].GetComponent<CharacterControlModule>().RoundReply();
            TotalRounds++;
        }
        TotalRounds=OurQuantity;
        Debug.Log(TotalRounds);
        m_Switching = false;
    }
    /*int GetRounds(){
        TotalRounds=0;
        if(ActionTeam){
            for(int i=0;i<AllPlayingCharacters[0].Length;i++){
                TotalRounds += AllPlayingCharacters[0][i].GetComponent<CharacterControlModule>().ControlTimes;
            }
        }else{
            for(int i=0;i<AllPlayingCharacters[1].Length;i++){
                TotalRounds += AllPlayingCharacters[1][i].GetComponent<CharacterControlModule>().ControlTimes;
            }
        }
        return TotalRounds;
    }*/
    public void EndRoundManually(){
        //Debug.Log("AA");
        TotalRounds=0;
    }
    public void BackMain(){
        SceneManager.LoadScene(1);
    }
    public void CharacterKilled(GameObject Playing){
        int Playingx=0; //物件一維陣列位置
        int Playingy=0; //物件二維陣列位置
        if(System.Array.IndexOf(AllPlayingCharacters[0],Playing)>-1){
            Playingx=0;
            Playingy=System.Array.IndexOf(AllPlayingCharacters[0],Playing);
            //print(Playing.transform.GetChild(0).gameObject.name);
            RoleStorageSystem.RemoveRole(Playing.transform.GetChild(0).gameObject);
            OurQuantity-=1;
        }else if(System.Array.IndexOf(AllPlayingCharacters[1],Playing)>-1){
            Playingx=1;
            Playingy=System.Array.IndexOf(AllPlayingCharacters[1],Playing);
            NumberOfEnemies-=1;
        }
        //print("test"+System.Array.IndexOf(AllPlayingCharacters[0],Playing)+" test"+System.Array.IndexOf(AllPlayingCharacters[1],Playing));
        RoleLocation[Playingx][Playingy]=new Vector3Int(999,999,999);
        Playing.SetActive(false);
    }
    
}
