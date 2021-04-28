using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoading : MonoBehaviour
{
    [SerializeField]
    [Header("關卡")]
    private int m_Checkpoint;

    [SerializeField]
    [Header("關卡場景")]
    private GameObject[] m_CheckpointLoad;

    [SerializeField]
    [Header("活動")]
    private bool m_ActivityBool;
    
    void Start()
    {
        int _i = PlayerPrefs.GetInt("Activity", -1);
        if(!m_ActivityBool && m_Checkpoint-2 < m_CheckpointLoad.Length){
            m_Checkpoint = PlayerPrefs.GetInt("Checkpoint", 2) - 1;
            if(m_Checkpoint > 1){
                Instantiate(m_CheckpointLoad[m_Checkpoint - 2], transform.position, transform.rotation);
                //m_CheckpointLoad[m_Checkpoint - 2].SetActive(true);
            }
        }else if(_i >= 0){
            m_Checkpoint = Random.Range(_i * 3, _i * 3 + 3);
            Instantiate(m_CheckpointLoad[m_Checkpoint], transform.position, transform.rotation);
            m_Checkpoint += 2;
        }else{
            GetComponent<RMS_1_1_2>().BackMain();
        }
    }

    // Update is called once per frame
    public void EarnRewards(){
        m_CheckpointLoad[m_Checkpoint - 2].GetComponent<SceneReward>().EarnRewards();
        
    }
}
