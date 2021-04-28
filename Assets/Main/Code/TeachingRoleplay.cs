using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeachingRoleplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("RoleManagementSubsystem").GetComponent<RMS_1_1_2>().StartLevel();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(RMS_1_1_2.RoleLocation[0][0]==new Vector3()&&GameObject.FindWithTag("Teaching").GetComponent<Teaching>().TeachingInt==2){
            GameObject.FindWithTag("Teaching").GetComponent<Teaching>().AttackTeaching();
        }
        if(!(RMS_1_1_2.ActionTeam)&&GameObject.FindWithTag("Teaching").GetComponent<Teaching>().TeachingInt==3){
            GameObject.FindWithTag("Teaching").GetComponent<Teaching>().SwitchRound();
        }
        if(RMS_1_1_2.NumberOfEnemies==0){
            PlayerPrefs.SetInt("Teaching",6);
        }
    }
    void OnMouseDown() {
        if(GameObject.FindWithTag("Teaching").GetComponent<Teaching>().TeachingInt==0)
            GameObject.FindWithTag("Teaching").GetComponent<Teaching>().MobileTeaching();

    }
}
