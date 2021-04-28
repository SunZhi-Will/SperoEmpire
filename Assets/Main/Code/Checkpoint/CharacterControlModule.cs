using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CharacterControlModule : MonoBehaviour
{
    [SerializeField]
    [Header("角色ID")]
    public int[] RoleID;

    [SerializeField]
    [Header("攝影機")]
    public Camera CameraPosition; //攝影機
    [SerializeField]
    [Header("動畫")]
    private Animator m_CameraPositionAnimator;

    [SerializeField]
    [Header("控制次數")]
    public int ControlTimes=1; //該角色可移動次數
    
    [SerializeField]
    [Header("角色所在格子")]
    public Vector3Int nowPoint;
    [SerializeField]
    [Header("角色所在x y z")]
    public Vector3 CharacterRealPosition; //自己位置

    [SerializeField]
    [Header("地形Tilemaps")]
    public Tilemap TerrainMap;

    [SerializeField]
    [Header("移動位置Tilemaps")]
    public Tilemap MobileMap;

    [SerializeField]
    [Header("牆壁位置Tilemaps")]
    public Tilemap WallMap;//牆壁位置Tilemaps

    [SerializeField]
    [Header("移動地板樣式")]
    public TileBase floorTile;
    [SerializeField]
    [Header("初始點地板樣式")]
    public TileBase spowTile;
    [SerializeField]
    [Header("移動距離")]
    public int speed = 3; 
    [SerializeField]
    [Header("移動位置")]
    public Vector3Int removePos;

    [SerializeField]
    [Header("鎖定位置")]
    private Vector3Int m_LockMovement = new Vector3Int(99,99,99);
    
    /*[SerializeField]
    [Header("回歸原位")]
    public bool BackToPlace=false;*/
    [SerializeField]
    [Header("滑行速度")]
    public float TaxiSpeed=0.3f;
    [SerializeField]
    [Header("回歸距離")]
    public Vector3 Robjectpion;


    [SerializeField]
    [Header("攻擊顯示地圖")]
    public GameObject AttackRange; // 這邊使用GameObject的原因，是因為需要角色移動時跟著一動
    [SerializeField]
    [Header("攻擊顯示方塊")]
    public TileBase AttackTile; // 攻擊顯示方塊
    [SerializeField]
    [Header("初始攻擊距離")]
    public int setAttackDistance; //初始攻擊距離
    [SerializeField]
    [Header("改變攻擊距離")]
    public int addAttackDistance; //改變攻擊距離


    [SerializeField]
    [Header("待機提示")]
    private SpriteRenderer m_StandbyReminder;
    

    //public static int AttackDistance; //現在攻擊距離 之後可能會用到
    

    //public Animator animator; //移動動畫
    

    private List<Vector3Int> RangeOfActivities = new List<Vector3Int>(); //可移動範圍
    private Vector3 prePos = new Vector3(); //點擊位置轉世界位置
    private Vector3 mouse; //滑鼠點擊位置
    private List<Vector3Int> AttackArea = new List<Vector3Int>();//攻擊區域
    private bool AttackConfirmation = false;

    

    Vector3 MovingTaxiDistance=new Vector3();
    Vector3Int ClickOnMapLocation;//滑鼠座標轉滑鼠地圖位置

    bool NewMobileBox=false; //防止未生成板塊，就移動


    

    void Start()
    {
        GameObject _go = transform.parent.gameObject;
        TerrainMap = _go.transform.GetChild(0).GetComponent<Tilemap> ();
        MobileMap = _go.transform.GetChild(1).GetComponent<Tilemap> ();
        WallMap = _go.transform.GetChild(2).GetComponent<Tilemap> ();
        AttackRange = _go.transform.GetChild(3).gameObject;

        //TerrainMap=GameObject.Find("Tilemap").GetComponent<Tilemap> ();
        //MobileMap=GameObject.Find("Mobile").GetComponent<Tilemap> ();
        //WallMap=GameObject.Find("Wall").GetComponent<Tilemap> ();
        
        //AttackRange=GameObject.FindGameObjectWithTag("AttackRange");

        nowPoint=TerrainMap.WorldToCell(transform.position);//將世界座標轉格子
        CharacterRealPosition = TerrainMap.CellToWorld(nowPoint)+new Vector3(0.5f,0.3f,0);
        transform.position = CharacterRealPosition;
        addAttackDistance=setAttackDistance;
        
        ClickOnMapLocation=nowPoint;
        
        MobileMap.ClearAllTiles();
        //AttackRange.SetActive(false);
        NewMobileBox=false;
        //MobileMap=GameObject.Find("mobile").GetComponent<Tilemap>();
        
        //AttackRange=GameObject.Find("AttackRange");
        //AttackRange=RMS_1_1_2.AttackRange;
    }
    public void StartLevel(Camera _MainCamera, Animator _CameraPositionAnimator){
        //AttackRange=RMS_1_1_2.AttackRange;
        CameraPosition = _MainCamera;
        m_CameraPositionAnimator = _CameraPositionAnimator;

    }

    public Vector3Int InitialDataTransfer(int[] RoleIDs)
    {
        RoleID=RoleIDs;
        return TerrainMap.WorldToCell(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(RMS_1_1_2.StartTheGame){
            if (Input.GetMouseButtonDown (0)) {
                Click();
            }
            MovingTaxiDistance = TerrainMap.CellToWorld(ClickOnMapLocation)+new Vector3(0.5f,0.3f,0);
            if(MovingTaxiDistance.x-transform.position.x>=0.1 || MovingTaxiDistance.y-transform.position.y>=0.1 ||
                MovingTaxiDistance.x-transform.position.x<=-0.1 || MovingTaxiDistance.y-transform.position.y<=-0.1){
                Glide();
            }else if(MovingTaxiDistance!=transform.position){
                transform.position=MovingTaxiDistance;
                AttackRange.transform.position=RMS_1_1_2.SelectRole.transform.position-new Vector3(0.5f,0.3f,0);
            }
        } //讓滑行到接近的位置

        /*if(BackToPlace){ //移動滑行
            Robjectpion = new Vector3(CharacterRealPosition.x - transform.position.x, CharacterRealPosition.y - transform.position.y);
            transform.Translate(
                Robjectpion * TaxiSpeed
            );
            if(Robjectpion.x<0.01f &&Robjectpion.y<0.01f && Robjectpion.x>-0.01f &&Robjectpion.y>-0.01f){
                BackToPlace = false;
            } //讓滑行到接近的位置
            
        }*/
        
        
    }
    void Click(){ //點擊模組
        
        mouse = Input.mousePosition; //滑鼠點擊的位置
        mouse.z = 10f; //攝影機的Z座標    
        prePos = CameraPosition.ScreenToWorldPoint(mouse);


        if(ControlTimes>0 && TerrainMap.WorldToCell(prePos)==nowPoint){
                ClickOnTheRoleModule();
        }
        else if(ControlTimes>0 && RMS_1_1_2.SelectRole==gameObject){
            
            if(TerrainMap.WorldToCell(prePos)==ClickOnMapLocation && TerrainMap.WorldToCell(prePos)!=nowPoint){
                ControlTimes-=1;
                DefiniteControl(); //確定點擊
                ClickOnTheRoleModule();
                //m_StandbyReminder.enabled = true;
                 
            }else if(RangeOfActivities.Contains(TerrainMap.WorldToCell(prePos)) && NewMobileBox){
                if(m_LockMovement == TerrainMap.WorldToCell(prePos) || m_LockMovement == new Vector3(99,99,99)){
                    ClickOnMapLocation = TerrainMap.WorldToCell(prePos);//滑鼠座標轉滑鼠地圖位置
                }

            }/*else if(TerrainMap.WorldToCell(prePos)==nowPoint){
                ClickOnTheRoleModule();
            }*/
            else if(AttackConfirmation && EnemyDetection(ClickOnMapLocation) && System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],TerrainMap.WorldToCell(prePos))>-1){
                DefiniteControl();
                Debug.Log("攻擊");

                m_CameraPositionAnimator.SetTrigger("Trigger");

                GameObject AttackC=RMS_1_1_2.AllPlayingCharacters[1][System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],TerrainMap.WorldToCell(prePos))];
                transform.GetChild(0).GetComponent<RoleStatusModule>().AttackChange(AttackC);
                //RMS_1_1_2.AllPlayingCharacters[1][System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],TerrainMap.WorldToCell(prePos))].
                //    GetComponent<RoleStatusModule>().HP-=1;

                //攻擊位移
                Robjectpion = new Vector3(TerrainMap.WorldToCell(prePos).x - transform.position.x, TerrainMap.WorldToCell(prePos).y - transform.position.y)
                    + new Vector3(0.5f,0.3f,0);
                transform.Translate(Robjectpion);
                ClickOnMapLocation=nowPoint;

                //攻擊結束
                Cancel();
                AttackConfirmation = false;
                m_StandbyReminder.enabled = true;
                gameObject.transform.GetChild(0).GetComponent<RoleStatusModule>().NoDisplayText();
                ControlTimes-=2; 
                RMS_1_1_2.TotalRounds-=1;
                
            }else{
                Cancel(); //取消
            }

            /*if(AttackConfirmation && System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],TerrainMap.WorldToCell(prePos))>-1){
                
                print("攻擊");
                GameObject AttackC=RMS_1_1_2.AllPlayingCharacters[1][System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],TerrainMap.WorldToCell(prePos))];
                transform.GetChild(0).GetComponent<RoleStatusModule>().AttackChange(AttackC);
                //RMS_1_1_2.AllPlayingCharacters[1][System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],TerrainMap.WorldToCell(prePos))].
                //    GetComponent<RoleStatusModule>().HP-=1;

                //攻擊位移
                Robjectpion = new Vector3(TerrainMap.WorldToCell(prePos).x - transform.position.x, TerrainMap.WorldToCell(prePos).y - transform.position.y)
                    + new Vector3(0.5f,0.3f,0);
                transform.Translate(Robjectpion);
                ClickOnMapLocation=nowPoint;

                //攻擊結束
                Cancel();
                AttackConfirmation = false;
                ControlTimes-=2; 
                RMS_1_1_2.TotalRounds-=1;
                
            }*/
        }
        
    }
    void Glide(){ //滑行模組
        
        MovingTaxiDistance = 
            new Vector3(MovingTaxiDistance.x - transform.position.x, MovingTaxiDistance.y - transform.position.y);
        transform.Translate(MovingTaxiDistance * TaxiSpeed);//角色滑行至滑鼠地圖位置
        AttackRange.transform.position=RMS_1_1_2.SelectRole.transform.position-new Vector3(0.5f,0.3f,0);
    }
    void DefiniteControl(){ //確認移動
        
        //RMS_1_1_2.TotalRounds-=1;
        nowPoint=removePos;
        CharacterRealPosition=TerrainMap.CellToWorld(removePos)+new Vector3(0.5f,0.3f,0);
        //animator.SetBool("raed", false);//動畫
        RMS_1_1_2.RoleLocation[RoleID[0]][RoleID[1]]=nowPoint;
        //print(RMS_1_1_2.RoleLocation[RoleID[0]][RoleID[1]]);
        Cancel();
        //ClickOnTheRoleModule();
    }
    public void Cancel(){//取消選取
        ClickOnMapLocation=nowPoint;

        MobileMap.ClearAllTiles();
        AttackRange.SetActive(false);
        AttackConfirmation = false;
        NewMobileBox=false;
        
    }



    void OnMouseDrag() { //拖移
        if(ControlTimes>1 && NewMobileBox){
            mouse = Input.mousePosition; //滑鼠點擊的位置
            mouse.z = 10f; //攝影機的Z座標    
            Vector3 newPow = CameraPosition.ScreenToWorldPoint(mouse);
            if(RangeOfActivities.Contains(TerrainMap.WorldToCell(newPow))){
                ClickOnMapLocation=TerrainMap.WorldToCell(newPow);//滑鼠座標轉滑鼠地圖位置
            }else{
                ClickOnMapLocation=nowPoint;
            }
            
            //else{
                //BackToPlace = true;
                Glide();
            //}   
        }
        
    }

    private void ClickOnTheRoleModule() { //點擊到角色時
        
        
        gameObject.transform.GetChild(0).GetComponent<RoleStatusModule>().DisplayText();
        if(RMS_1_1_2.SelectRole!=gameObject){
            //MobileMap.ClearAllTiles();
            NewMobileBox=false;
            RMS_1_1_2.SelectRole.GetComponent<CharacterControlModule>().Cancel();
            RMS_1_1_2.SelectRole=gameObject;
        }
        //animator.SetBool("raed", true);
        if(ControlTimes>1 && NewMobileBox!=true){
            //mouse = Input.mousePosition;
            //mouse.z = 1f;
            //prePos = CameraPosition.ScreenToWorldPoint(mouse);
            prePos = transform.position;
            RangeOfActivities.Clear();
            setTbase(speed,nowPoint);
            setAttack(addAttackDistance,new Vector3Int());
        }else if(RangeOfActivities.Contains(TerrainMap.WorldToCell(prePos)) && NewMobileBox){
            if(m_LockMovement == TerrainMap.WorldToCell(prePos) || m_LockMovement == new Vector3(99,99,99)){
                ClickOnMapLocation = TerrainMap.WorldToCell(prePos);//滑鼠座標轉滑鼠地圖位置
            }

        }else{
            m_StandbyReminder.enabled = !EnemyDetection(nowPoint);
            if(!EnemyDetection(nowPoint)){
                gameObject.transform.GetChild(0).GetComponent<RoleStatusModule>().NoDisplayText();
                ControlTimes-=1;
                RMS_1_1_2.TotalRounds-=1;
            }
            NewMobileBox=false;
        }
    }

    private bool EnemyDetection(Vector3Int _nowPoint){
        foreach (var item in AttackArea)//偵測敵人是否在範圍內
        {
            print(System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],item+_nowPoint)>-1);
            if(System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],item+_nowPoint)>-1){
                AttackRange.SetActive(true);
                AttackConfirmation = true;
                return true;
                
            }else{
                //AttackRange.SetActive(false);
            }
        }
        
        return false;
    }


    
    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag=="MobileFloor"){
            removePos = TerrainMap.WorldToCell(transform.position);
        }
        
        //map.SetTile(removePos, null);//測試
    }

    void setTbase(int Speeds,Vector3Int nowPoints){
        
        List<Vector3Int> InitialRange = new List<Vector3Int>(); //暫時 判斷中的位置
        List<Vector3Int> ExpandTheScopeOf = new List<Vector3Int>(); //新增的位置
        Vector3Int[] ExpandTemporaryStorage;
        InitialRange.Add(nowPoints);
        for(int i=Speeds;i>=0;i--){
            foreach (var item in InitialRange)
            {
                ExpandTemporaryStorage = new Vector3Int[4]{item+Vector3Int.up,item+Vector3Int.down,item+Vector3Int.right,item+Vector3Int.left};
                foreach (var ETS in ExpandTemporaryStorage){
                    int rms=System.Array.IndexOf(RMS_1_1_2.RoleLocation[0],ETS)+System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],ETS);
                    if(!WallMap.GetTile(ETS)&&!(RangeOfActivities.Contains(ETS))&&rms==-2){
                        ExpandTheScopeOf.Add(ETS);
                    }
                }
            }
            
            RangeOfActivities.AddRange(InitialRange);
            InitialRange.Clear();
            InitialRange.AddRange(ExpandTheScopeOf);
            ExpandTheScopeOf.Clear();
        }
        foreach (var item in RangeOfActivities)
        {
            MobileMap.SetTile(item,floorTile);
        }
        MobileMap.SetTile(nowPoint,spowTile);
        NewMobileBox=true;
    }
    void setAttack(int Speeds,Vector3Int nowPoints){
        AttackConfirmation = false;
        AttackRange.SetActive(false);
        AttackRange.GetComponent<Tilemap>().ClearAllTiles();
        List<Vector3Int> InitialRange = new List<Vector3Int>(); //暫時 判斷中的位置
        List<Vector3Int> ExpandTheScopeOf = new List<Vector3Int>(); //新增的位置
        InitialRange.Add(nowPoints);
        for(int i=Speeds;i>=0;i--){
            foreach (var item in InitialRange)
            {
                Vector3Int[] ExpandTemporaryStorage = new Vector3Int[4]{item+Vector3Int.up,item+Vector3Int.down,item+Vector3Int.right,item+Vector3Int.left};
                foreach (var ETS in ExpandTemporaryStorage){
                    if(!(AttackArea.Contains(ETS))){
                        ExpandTheScopeOf.Add(ETS);
                    }
                }
            }
            
            AttackArea.AddRange(InitialRange);
            InitialRange.Clear();
            InitialRange.AddRange(ExpandTheScopeOf);
            ExpandTheScopeOf.Clear();
        }
        AttackArea.Remove(nowPoints);
        foreach (var item in AttackArea)
        {
            AttackRange.GetComponent<Tilemap>().SetTile(item,AttackTile);
        }
        AttackRange.transform.position=transform.position-new Vector3(0.5f,0.3f,0);//攻擊地圖跟著角色移動
        AttackConfirmation = true;
        AttackRange.SetActive(true);
        //AttackConfirmation = true;
    }
    Vector3Int num;//暫存數字
    public void SpecialMovement(Vector3Int Special){
        num=nowPoint+Special;
        if(!WallMap.GetTile(num)&&System.Array.IndexOf(RMS_1_1_2.RoleLocation[0],num)+System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],num)==-2){
            ClickOnMapLocation+=Special;
            removePos+=Special;
            DefiniteControl();
            //CharacterRealPosition=Special+new Vector3(0.5f,0.3f,0);
        }else{
            print("WallMap");
        }
    }

    public void RoundReply(){
        ControlTimes = 2;
        m_StandbyReminder.enabled = false;
    }
}
