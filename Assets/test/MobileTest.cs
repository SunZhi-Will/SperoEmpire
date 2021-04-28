using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class MobileTest : MonoBehaviour
{
    [SerializeField]
    [Header("場上ID")]
    public int MobileID;
    
    [SerializeField]
    [Header("現在所在位置")]
    public Vector3Int nowPoint;

    [SerializeField]
    [Header("存放可移動位置Tilemaps")]
    public Tilemap floorMap;

    [SerializeField]
    [Header("所使用的移動樣式")]
    public TileBase floorTile;

    public TileBase spowTile;//初始點地板
    public Tilemap wilMap;//牆壁位置Tilemaps
    public int seep = 3; //可移動距離
    private List<Vector3Int> fmtest = new List<Vector3Int>(); //可移動範圍
    public Vector3Int removePos;//移動位置

    private Vector3 prePos = new Vector3(); //點擊位置轉世界位置
    private Vector3 mouse; //滑鼠點擊位置
    public Camera m_CameraTwo; //攝影機
    public Vector3 Gamestartxyz; //自己位置
    public bool tfgxyz=false; //是否回歸原位
    public float tfspeed=5f; //回歸速度
    public Vector3 Robjectpion; //回歸距離

    public GameObject AttackRange; // 攻擊顯示地圖
    public TileBase AttackBase; // 攻擊顯示方塊
    public int setAttackDistance; //初始攻擊距離
    public int addAttackDistance; //改變攻擊距離
    public static int AttackDistance; //現在攻擊距離
    List<Vector3Int> AttackArea = new List<Vector3Int>();//攻擊區域

    public Animator animator;
    


    void Start()
    {
        floorMap=GameObject.Find("mobile").GetComponent<Tilemap>();
        nowPoint=floorMap.WorldToCell(transform.position);
        Gamestartxyz = floorMap.CellToWorld(nowPoint)+new Vector3(0.5f,0.3f,0);
        transform.position = Gamestartxyz;
        addAttackDistance=setAttackDistance;
        AttackRange=GameObject.Find("AttackRange");
    }

    // Update is called once per frame
    void Update()
    {
        
        

        if(tfgxyz){
            Robjectpion = new Vector3(Gamestartxyz.x - transform.position.x, Gamestartxyz.y - transform.position.y);
            transform.Translate(
                Robjectpion * tfspeed * Time.deltaTime
            );
            if(transform.position == Gamestartxyz){
                tfgxyz = false;
            }
        }
        
        
    }
    void OnMouseDrag() {
        tfgxyz = false;
        mouse = Input.mousePosition;
        mouse.z = 10f;    
        Vector3 newPow = m_CameraTwo.ScreenToWorldPoint(mouse);
        Vector3 offset = newPow - prePos;
        /*transform.position += offset;
        prePos = m_CameraTwo.ScreenToWorldPoint(mouse);*/
        Vector3Int rtest=floorMap.WorldToCell(transform.position+offset);
        if(fmtest.Contains(rtest)){
            transform.position=floorMap.CellToWorld(rtest)+new Vector3(0.5f,0.3f,0);
            AttackRange.transform.position=floorMap.CellToWorld(rtest);
            AttackRange.SetActive(true);
        }else{
            transform.position=Gamestartxyz;
            AttackRange.transform.position=Gamestartxyz-new Vector3(0.5f,0.3f,0);
        }
        prePos = transform.position;    
        
    }
    void OnMouseUp() {
        
        if(nowPoint==removePos || !(fmtest.Contains(removePos))){
            tfgxyz = true;
        }else{
            Gamestartxyz = floorMap.CellToWorld(removePos)+new Vector3(0.5f,0.3f,0);
            nowPoint=removePos;
            tfgxyz = true;
        }
            animator.SetBool("raed", false);
        floorMap.ClearAllTiles();
        
        foreach (var item in AttackArea)
        {
            print(System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],item+nowPoint)>-1);
            if(System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],item+nowPoint)>-1){
                
                AttackRange.SetActive(true);
                break;
            }else{
                AttackRange.SetActive(false);
            }
        }


    }

    void OnMouseDown() {
        
        animator.SetBool("raed", true);

        mouse = Input.mousePosition;
        mouse.z = 1f;
        //prePos = m_CameraTwo.ScreenToWorldPoint(mouse);
        prePos = transform.position;

        fmtest.Clear();
        setTbase(seep,nowPoint);
        
        if(AttackDistance!=addAttackDistance)
        {
            AttackRange.SetActive(false);
            setAttack(addAttackDistance,new Vector3Int());
        }
        foreach (var item in fmtest)
        {
            floorMap.SetTile(item,floorTile);
        }
        floorMap.SetTile(nowPoint,spowTile);
    }

    
    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag=="MobileFloor"){
            removePos = floorMap.WorldToCell(transform.position);
        }
        //map.SetTile(removePos, null);//測試
    }

    void setTbase(int TimeSeeps,Vector3Int nowPoints){
        List<Vector3Int> fmtest1 = new List<Vector3Int>(); //暫時 判斷中的位置
        List<Vector3Int> fmtest2 = new List<Vector3Int>(); //新增的位置
        Vector3Int[] bataV3I;
        fmtest1.Add(nowPoints);
        for(int i=TimeSeeps;i>=0;i--){
            foreach (var item in fmtest1)
            {
                bataV3I = new Vector3Int[4]{item+Vector3Int.up,item+Vector3Int.down,item+Vector3Int.right,item+Vector3Int.left};
                foreach (var itemtest in bataV3I){
                    int rms=System.Array.IndexOf(RMS_1_1_2.RoleLocation[0],itemtest)+System.Array.IndexOf(RMS_1_1_2.RoleLocation[1],itemtest);
                    if(!wilMap.GetTile(itemtest)&&!(fmtest.Contains(itemtest))&&rms==-2){
                        fmtest2.Add(itemtest);
                    }
                }
            }
            
            fmtest.AddRange(fmtest1);
            fmtest1.Clear();
            fmtest1.AddRange(fmtest2);
            fmtest2.Clear();
        }
    }
    void setAttack(int TimeSeeps,Vector3Int nowPoints){
        AttackRange.GetComponent<Tilemap>().ClearAllTiles();
        List<Vector3Int> fmtest1 = new List<Vector3Int>(); //暫時 判斷中的位置
        List<Vector3Int> fmtest2 = new List<Vector3Int>(); //新增的位置
        fmtest1.Add(nowPoints);
        for(int i=TimeSeeps;i>=0;i--){
            foreach (var item in fmtest1)
            {
                Vector3Int[] bataV3I = new Vector3Int[4]{item+Vector3Int.up,item+Vector3Int.down,item+Vector3Int.right,item+Vector3Int.left};
                foreach (var itemtest in bataV3I){
                    if(!(AttackArea.Contains(itemtest))){
                        fmtest2.Add(itemtest);
                    }
                }
            }
            
            AttackArea.AddRange(fmtest1);
            fmtest1.Clear();
            fmtest1.AddRange(fmtest2);
            fmtest2.Clear();
        }
        AttackArea.Remove(nowPoints);
        foreach (var item in AttackArea)
        {
            AttackRange.GetComponent<Tilemap>().SetTile(item,AttackBase);
        }
    }
}
