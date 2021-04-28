using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneReward : MonoBehaviour
{
    [SerializeField]
    [Header("獎勵角色條件")]
    private GameObject m_RewardRoleIf;
    [SerializeField]
    [Header("獎勵角色")]
    private GameObject m_RewardRole;
    [SerializeField]
    [Header("獎勵復活石數量")]
    private int m_RewardResurrectionStone;
    public void EarnRewards(){
        if(m_RewardRoleIf != null){
            RoleStorageSystem.AddRole(m_RewardRole);
        }
        PlayerPrefs.SetInt("Spirit", (PlayerPrefs.GetInt("Spirit", 0)+m_RewardResurrectionStone));
    }
}
