using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowCharacterInformation : MonoBehaviour
{
    [SerializeField]
    [Header("角色圖示")]
    private Sprite[] m_ShowRole;
    [SerializeField]
    [Header("角色顯示")]
    private Image m_RoleUIImage;
    
    [SerializeField]
    [TextArea(3, 10)]
    [Header("角色資料")]
    private string[] m_ShowRoleInformation;

    [SerializeField]
    [Header("角色解鎖")]
    private int[] m_CharacterUnlock;

    [SerializeField]
    [Header("角色資料顯示")]
    private Text m_InformationRoleUI;

    [SerializeField]
    [Header("招募按鈕")]
    private GameObject m_RecruitButton;

    [SerializeField]
    [Header("所有角色")]
    private GameObject[] m_AllRoles;

    [SerializeField]
    [Header("招募角色")]
    private GameObject m_RecruitingRole;

    [SerializeField]
    [Header("更新重生石")]
    private MemberSystem m_UploadRebirthStone;

    [SerializeField]
    [Header("重生石數量顯示")]
    private Text m_RebirthStoneQuantityDisplay;

    private void Start()
    {
        m_RebirthStoneQuantityDisplay.text = PlayerPrefs.GetInt("Spirit", 0) + "";
    }
    
    public void Initialization()
    {
        m_RoleUIImage.gameObject.SetActive(false);
        m_RecruitButton.SetActive(false);
        m_InformationRoleUI.text = "";
    }

    // Update is called once per frame
    
    public void News(int _num){
        m_RoleUIImage.gameObject.SetActive(true);
        m_RebirthStoneQuantityDisplay.text = PlayerPrefs.GetInt("Spirit", 0) + "";
        

        m_RecruitingRole = m_AllRoles[_num];
        m_RoleUIImage.sprite = m_ShowRole[_num];
        m_InformationRoleUI.text = m_ShowRoleInformation[_num];

        if(RoleStorageSystem.NowHasRoleFileName.IndexOf(m_RecruitingRole.name)>-1 || PlayerPrefs.GetInt("Checkpoint", 0) < m_CharacterUnlock[_num] + 2){
            m_RecruitButton.SetActive(false);
        }else{
            m_RecruitButton.SetActive(true);
        }
    }
    public void Recruit(){
        int _i = PlayerPrefs.GetInt("Spirit", 0);
        if(_i > 0){
            PlayerPrefs.SetInt("Spirit", _i - 1);
            m_RebirthStoneQuantityDisplay.text = (_i - 1) + "";
            m_UploadRebirthStone.Record();

            RoleStorageSystem.AddRole(m_RecruitingRole);
            m_RecruitButton.SetActive(false);
        }else{
            m_RecruitButton.SetActive(true);
        }
    }
}
