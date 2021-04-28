using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class OpeningSystem : MonoBehaviour
{
    
    public Animator animator; //進入動畫
    public AudioSource audioData;
    public GameObject ConfirmToClearUI;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){//是否按下離開鍵
            Escape();
        }
    }
    public void PressSpace(){
        animator.SetBool("Opening", true);
        audioData.Play(0);
        
        LevelSubsystem.LoadingGo=Instantiate(LevelSubsystem.Loadgo);
        StartCoroutine(LevelSubsystem.DetectiveLoading("MainMenu",1f));
    }
    public void DeleteUI(){
        ConfirmToClearUI.SetActive(true);
    }
    public void DropOut(GameObject GOUI){
        GOUI.SetActive(false);
    }
    public void Delete(){
        PlayerPrefs.DeleteAll();
        RoleStorageSystem.Again();
    }
    public void Escape(){
        if(ConfirmToClearUI.activeInHierarchy){
            ConfirmToClearUI.SetActive(false);
        }else{
            Application.Quit();
        }
    }
}
