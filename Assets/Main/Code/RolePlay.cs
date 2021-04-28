using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RolePlay : MonoBehaviour
{
    [SerializeField]
    [Header("地形Tilemaps")]
    Tilemap TerrainMap;

    /*[SerializeField]
    [Header("部屬介面")]
    public static GameObject SubordinateInterface;
*/
    [SerializeField]
    [Header("介面角色按鈕")]
    public GameObject[] RoleButton;

    
    private GameObject RoleGameObject;
    GameObject TemporaryStorage;

    // Start is called before the first frame update
    void Start()
    {
        TerrainMap=GameObject.Find("Tilemap").GetComponent<Tilemap> ();
        transform.position = TerrainMap.CellToWorld(TerrainMap.WorldToCell(transform.position))+new Vector3(0.5f,0.5f,0);
        
        
        //SubordinateInterface.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown() {
        RoleDeployment.SubordinateInterface.SetActive(true);
        RoleButton=GameObject.FindGameObjectsWithTag("InterfaceRole");
        
        for(int i=0;i<RoleButton.Length;i++){
            RoleButton[i].GetComponent<Button>().onClick.RemoveAllListeners();
            RoleButton[i].GetComponent<ButtonObject>().ButtonsSet();
            RoleButton[i].GetComponent<Button>().onClick.AddListener(Subordinate);
            
            
        }
        

    }
    public void Subordinate(){
        
        if(RoleGameObject != null){
            RoleDeployment.SubordinateInterface.GetComponent<SubordinateInterfaceSystem>().RemoveReturn(TemporaryStorage);
            Destroy(RoleGameObject);
        }
        TemporaryStorage=RoleDeployment.SubordinateInterface.GetComponent<SubordinateInterfaceSystem>().ReturnCharacter();
        if(TemporaryStorage!=null){
            RoleGameObject = (GameObject)Instantiate(TemporaryStorage, transform.position,transform.rotation);
            RoleGameObject.transform.parent=gameObject.transform.parent;
        }
        //transform.parent = RoleGameObject.transform;
        RoleDeployment.SubordinateInterface.SetActive(false);
    }
}
