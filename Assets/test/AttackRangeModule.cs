using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeModule : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    [Header("開啟護盾?")]
    private bool m_WhetherToActivateTheShield;
    void Start()
    {
        m_WhetherToActivateTheShield = transform.parent.gameObject.tag.IndexOf("Rig") > -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag.IndexOf("RolePlay") > -1 && m_WhetherToActivateTheShield){
            other.transform.GetChild(0).GetComponent<RoleStatusModule>().g_Roleprotected = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        
        if(other.gameObject.tag.IndexOf("RolePlay") > -1 && m_WhetherToActivateTheShield){
            other.transform.GetChild(0).GetComponent<RoleStatusModule>().g_Roleprotected = false;
        }
    }
}
