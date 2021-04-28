using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greedy : MonoBehaviour
{
    int TemporaryNumber;
    Vector3Int x;
    float y;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Special(GameObject Enemy){

        x=transform.parent.gameObject.GetComponent<CharacterControlModule>().nowPoint-Enemy.GetComponent<EnemyIntelligenceSystem>().nowPoint;

        
        //print("GOOGLD "+x);
        gameObject.transform.parent.gameObject.GetComponent<CharacterControlModule>().SpecialMovement(x);
        //gameObject.transform.parent.gameObject.GetComponent<CharacterControlModule>().ClickOnMapLocation=new Vector3Int(transform.position.x-x,transform.position.y-y);
        //Enemyposition_y=TemporaryNumber>0?TemporaryNumber:-(TemporaryNumber);
    }
}
