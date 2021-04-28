using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyIntelligenceSystem : MonoBehaviour
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
    public int ControlTimes=0; //該角色可移動次數
    
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
    [Header("距離初始化")]
    public static bool DistanceInitialization=false;
    [SerializeField]
    [Header("與角色的各個距離")]
    private static Vector3Int[] DistanceFromSquare;


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
        
        //MobileMap=GameObject.Find("mobile").GetComponent<Tilemap>();
        nowPoint=TerrainMap.WorldToCell(transform.position);//將世界座標轉格子
        CharacterRealPosition = TerrainMap.CellToWorld(nowPoint)+new Vector3(0.5f,0.3f,0);
        transform.position = CharacterRealPosition;
        addAttackDistance=setAttackDistance;
        
        
        ClickOnMapLocation=nowPoint;
        //AttackRange=GameObject.FindGameObjectWithTag("AttackRange");
        
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
            if(ControlTimes>0 && !(DistanceInitialization)){
                StartCoroutine(BufferBudget());
            }
            Glide();
        }
    }
    private IEnumerator BufferBudget(){
        ControlTimes-=1;
        yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));   
        int Dt=0; //哪個角色最近(測試)
        int t=0; //用來測量哪個角色離該角色最近
        DistanceFromSquare=new Vector3Int[RMS_1_1_2.AllPlayingCharacters[0].Length];
        for(int i=0;i<RMS_1_1_2.AllPlayingCharacters[0].Length;i++){
            int x=(RMS_1_1_2.RoleLocation[0][i].x-nowPoint.x);
            int y=(RMS_1_1_2.RoleLocation[0][i].y-nowPoint.y);
            DistanceFromSquare[i]=new Vector3Int(x,y,0);
            int ts = 
                (x > 0 ? x : -1*x) + 
                (y > 0 ? y : -1*y);
            if(t>ts || t==0){
                t=ts;
                Dt=i;
            }
        }
        setTbase(speed,nowPoint,DistanceFromSquare[Dt]);
        Attack();
        RMS_1_1_2.TotalRounds-=1;
    }
    void Glide(){ //滑行模組
        MovingTaxiDistance = TerrainMap.CellToWorld(ClickOnMapLocation)+new Vector3(0.5f,0.3f,0);
        MovingTaxiDistance = 
            new Vector3(MovingTaxiDistance.x - transform.position.x, MovingTaxiDistance.y - transform.position.y);
        transform.Translate(MovingTaxiDistance * TaxiSpeed);//角色滑行至滑鼠地圖位置
        AttackRange.transform.position=RMS_1_1_2.SelectRole.transform.position-new Vector3(0.5f,0.3f,0);
        nowPoint=ClickOnMapLocation;
        RMS_1_1_2.RoleLocation[1][RoleID[1]]=nowPoint;
    }

    void setTbase(int Speeds,Vector3Int nowPoints,Vector3Int TargetLocation){
        RangeOfActivities.Clear();
        NewMobileBox=true;
        List<Vector3Int> InitialRange = new List<Vector3Int>(); //暫時 判斷中的位置
        List<Vector3Int> ExpandTheScopeOf = new List<Vector3Int>(); //新增的位置
        Vector3Int[] ExpandTemporaryStorage;
        InitialRange.Add(nowPoints);

        Vector3Int ClosestGridToTarget=nowPoints;//離目標最近格子
        int tss=0;//最短距離
        //print(tss);
        for(int i=Speeds;i>0;i--){
            foreach (var item in InitialRange)
            {
                ExpandTemporaryStorage = new Vector3Int[4]{item+Vector3Int.up,item+Vector3Int.down,item+Vector3Int.right,item+Vector3Int.left};
                foreach (var ETS in ExpandTemporaryStorage){
                    int rms=System.Array.IndexOf(RMS_1_1_2.RoleLocation[0],ETS)+System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],ETS);
                    if(!WallMap.GetTile(ETS)&&!(RangeOfActivities.Contains(ETS))&&rms==-2){
                        ExpandTheScopeOf.Add(ETS);
                        Vector3Int t=TargetLocation-(ETS-nowPoints);
                        int ts= (t.x > 0 ? t.x : -1*t.x) + (t.y > 0 ? t.y : -1*t.y);
                        if((tss==0 || tss>ts) && ETS!=nowPoints){
                            tss=ts;
                            ClosestGridToTarget=ETS;
                            
                        }
                        //print(tss);
                    }
                }
            }
            
            RangeOfActivities.AddRange(InitialRange);
            InitialRange.Clear();
            InitialRange.AddRange(ExpandTheScopeOf);
            ExpandTheScopeOf.Clear();
        }
        ClickOnMapLocation=ClosestGridToTarget;
        //print(ClickOnMapLocation);
        /*foreach (var item in RangeOfActivities)
        {
            MobileMap.SetTile(item,floorTile);
        }
        MobileMap.SetTile(nowPoint,spowTile);*/
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

        /*
        foreach (var item in AttackArea)
        {
            AttackRange.GetComponent<Tilemap>().SetTile(item,AttackTile);
        }
        AttackRange.transform.position=transform.position-new Vector3(0.5f,0.3f,0);//攻擊地圖跟著角色移動
        AttackRange.SetActive(true);*/
    }
    void Attack(){
        setAttack(addAttackDistance,new Vector3Int());
        foreach (var item in AttackArea)//偵測敵人是否在範圍內
        {
            int EnemyPosition=System.Array.IndexOf(RMS_1_1_2.RoleLocation[0],item+nowPoint);
            //print("test1"+EnemyPosition);
            if(EnemyPosition>-1){
                GameObject AttackC=RMS_1_1_2.AllPlayingCharacters[0][EnemyPosition];
                transform.GetChild(0).GetComponent<RoleStatusModule>().AttackChange(AttackC);
                m_CameraPositionAnimator.SetTrigger("Trigger");
                /*AttackRange.SetActive(true);
                AttackConfirmation = true;*/
                break;
            }else{
                //AttackRange.SetActive(false);
            }
        }
    }
}
