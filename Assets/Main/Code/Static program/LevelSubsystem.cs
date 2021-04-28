using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelSubsystem
{
    //保存等待物件
    public static GameObject Loadgo=(Resources.Load("Loading",typeof(GameObject)) as GameObject);

    //加載卡頓時的等待物件
    public static GameObject LoadingGo;

    //正在加載的關卡
    public static AsyncOperation LoadDetective;


    //載入物件
    public static IEnumerator DetectiveLoading(string Detective,float waitTime)//等待  
    {  
        LoadingGo.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        LoadDetective = SceneManager.LoadSceneAsync(Detective);
        while (!LoadDetective.isDone)
        {
            LoadingGo.SetActive(true);
            //Instantiate(Loadgo,Thgo);
            yield return null;
        }
        
    }
}
