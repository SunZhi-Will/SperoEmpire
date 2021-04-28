using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text;

public class MemberSystem : MonoBehaviour
{
    [SerializeField]
    [Header("登入按鈕")]
    private GameObject m_DiscountButton;
    [SerializeField]
    [Header("登入畫面")]
    private GameObject m_SignInInterface;
    [SerializeField]
    [Header("等待畫面")]
    private GameObject m_Logingo;
    [SerializeField]
    [Header("處理畫面")]
    private GameObject m_ROutGO;
    [SerializeField]
    [Header("處理畫面文字")]
    private Text m_ROutText;
    [SerializeField]
    [Header("序號文字")]
    private Text SerialNumberText;
    /*[SerializeField]
    [Header("名字UI")]
    public Text NameUI;
    [SerializeField]
    [Header("點數UI")]
    public Text PintUI;*/

    string str = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    System.Random next = new System.Random();
    
    /*[SerializeField]
    [Header("序號")]
    string SerialNumber;*/

    [SerializeField]
    [Header("輸入名字")]
    public string RName;
    [SerializeField]
    [Header("輸入帳號")]
    public string RAccountnumber;
    [SerializeField]
    [Header("輸入密碼")]
    public string RPassword;

    [SerializeField]
    [Header("保留名字")]
    public string LName;
    [SerializeField]
    [Header("保留帳號")]
    public string LAccountnumber;
    [SerializeField]
    [Header("保留密碼")]
    public string LPassword;
    /*[SerializeField]
    [Header("點數")]
    public int LPint;*/

    private bool m_display;

    [SerializeField]
    [Header("紀錄資料")]
    private bool m_WhetherToLogIn = false; //紀錄資料

    [SerializeField]
    [Header("所有角色存檔")]
    private GameObject[] m_AllRolesArchive;
    
    void Start()
    {
        //StartCoroutine(UploadRegistered());
        
        RAccountnumber = PlayerPrefs.GetString("Accountnumber", null);
        RPassword = PlayerPrefs.GetString("Password", null);
        StartCoroutine(LoadEvent());
        if(PlayerPrefs.HasKey("Accountnumber") && PlayerPrefs.HasKey("Password") && !m_WhetherToLogIn){
            
            m_Logingo.SetActive(true);
            m_display = false;
            StartCoroutine(UploadSignIn());
        }
        
            
        
    }


    void Update()
    {
        //SerialNumberText.text = SerialNumber;
        /*if(LName != ""){
            NameUI.text = LName;
            PintUI.text = "$"+LPint;
        }*/
    }

    public void SetRName(string _Vel){
        RName = _Vel;
    }
    public void SetRAccountnumber(string _Vel){
        RAccountnumber = _Vel;
    }
    public void SetRPassword(string _Vel){
        RPassword = _Vel;
    }
    public void Registered(){
        m_Logingo.SetActive(true);
        m_display = true;
        if(RName == ""){
            SetROutSystem("姓名未填入");
        }else if(RAccountnumber == ""){
            SetROutSystem("帳號未填入");
        }else if(RPassword == ""){
            SetROutSystem("密碼未填入");
        }else{
            StartCoroutine(UploadRegistered());
        }
    }
    public void SignIns(){
        m_Logingo.SetActive(true);
        m_display = true;
        if(RAccountnumber == ""){
            SetROutSystem("帳號未填入");
        }else if(RPassword == ""){
            SetROutSystem("密碼未填入");
        }else{
            StartCoroutine(UploadSignIn());
        }
    }
    public void Record(){
        if(RAccountnumber != "" && RPassword != ""){
            m_Logingo.SetActive(true);
            StartCoroutine(UploadRecord());
        }
    }
    
    
    IEnumerator UploadRegistered()
    {
        m_ROutText.text = "處理中...";
        /*StringBuilder builder = new StringBuilder();
        for(var i = 0; i < 6; i++)
        {
            builder.Append(str[next.Next(0, str.Length)]);
        }*/
        
        WWWForm form = new WWWForm();
        form.AddField("method","registered");
        form.AddField("name", RName);
        form.AddField("accountnumber", RAccountnumber);
        form.AddField("password", RPassword);
        form.AddField("checkpoint", PlayerPrefs.GetInt("Checkpoint", 2) - 2);
        form.AddField("allrolesarchive", RoleRead());
        form.AddField("resurrectionstone", PlayerPrefs.GetInt("Spirit", 0));
        
        
        //form.AddField("serialnumber", builder.ToString());

        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyHE-CreEnv_E6_VXS4m9M7OMa39HpzggESh9WMMK454X0pxC7H0kma/exec", form))
        {
            yield return www.SendWebRequest();
            if(www.isNetworkError || www.isHttpError){
                SetROutSystem("錯誤!!\n請檢查網路狀況");
                Debug.Log(www.error);
            }else{
                print(www.downloadHandler.text);


                Debug.Log("Form upload Complete!");
                if(www.downloadHandler.text.IndexOf("註冊失敗") == -1){
                    SetROutSystem("註冊成功\n(自動登入...)");
                    GoActiveHierarchy();
                    //SerialNumber = builder.ToString();
                    LName = RName;
                    LAccountnumber = RAccountnumber;
                    LPassword = RPassword;
                    PlayerPrefs.SetString("LName", LName);
                    PlayerPrefs.SetString("Accountnumber", LAccountnumber);
                    PlayerPrefs.SetString("Password", LPassword);
                }else{
                    SetROutSystem("註冊失敗\n(帳號已被使用過...)");
                }
            }
        }
    }
    IEnumerator UploadSignIn()
    {
        m_ROutText.text = "處理中...";
        WWWForm form = new WWWForm();
        form.AddField("method","SignIn");
        form.AddField("accountnumber",RAccountnumber);
        form.AddField("password", RPassword);

        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyHE-CreEnv_E6_VXS4m9M7OMa39HpzggESh9WMMK454X0pxC7H0kma/exec", form))
        {
            yield return www.SendWebRequest();
            if(www.isNetworkError || www.isHttpError){
                SetROutSystem("錯誤!!\n請檢查網路狀況");
            }else{
                print(www.downloadHandler.text);
                if(www.downloadHandler.text.Split('\n')[0].Length == 4){
                    m_WhetherToLogIn = true;
                    SetROutSystem("登入成功");
                    
                    LName = www.downloadHandler.text.Split('\n')[1];
                    PlayerPrefs.SetString("LName", LName);
                    PlayerPrefs.SetInt("Checkpoint", Convert.ToInt32(www.downloadHandler.text.Split('\n')[2]) + 2);
                    PlayerPrefs.SetInt("Spirit", Convert.ToInt32(www.downloadHandler.text.Split('\n')[4]));
                    Debug.Log(www.downloadHandler.text.Split('\n')[3].Split('、'));
                    foreach (var item in m_AllRolesArchive)
                    {
                        PlayerPrefs.DeleteKey(item.name);
                        
                    }
                    foreach (var item in www.downloadHandler.text.Split('\n')[3].Split('、'))
                    {
                        PlayerPrefs.SetString(item, item);
                    }
                    
                    //SerialNumber = www.downloadHandler.text.Split('\n')[1];
                    //LPint = Int32.Parse(www.downloadHandler.text.Split('\n')[2]);
                    LAccountnumber = RAccountnumber;
                    LPassword = RPassword;
                    PlayerPrefs.SetString("Accountnumber", LAccountnumber);
                    PlayerPrefs.SetString("Password", LPassword);
                    GoActiveHierarchy();
                }else{
                    SetROutSystem("帳號或密碼錯誤!");
                }
                Debug.Log("Form upload Complete!");
            }
        }
    }
    IEnumerator UploadRecord()
    {
        WWWForm form = new WWWForm();
        form.AddField("method","Record");
        form.AddField("accountnumber",PlayerPrefs.GetString("Accountnumber", RAccountnumber));
        form.AddField("password", PlayerPrefs.GetString("Password", RPassword));
        form.AddField("checkpoint", PlayerPrefs.GetInt("Checkpoint", 2)-2);
        form.AddField("allrolesarchive", RoleRead());
        form.AddField("resurrectionstone", PlayerPrefs.GetInt("Spirit", 0));
        

        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyHE-CreEnv_E6_VXS4m9M7OMa39HpzggESh9WMMK454X0pxC7H0kma/exec", form))
        {
            yield return www.SendWebRequest();
            
            if(www.isNetworkError || www.isHttpError){
                m_display = true;
                SetROutSystem("錯誤!!\n請檢查網路狀況");
            }
        }
    }

    IEnumerator LoadEvent()
    {
        WWWForm form = new WWWForm();
        form.AddField("method","LoadEvent");
        

        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyHE-CreEnv_E6_VXS4m9M7OMa39HpzggESh9WMMK454X0pxC7H0kma/exec", form))
        {
            yield return www.SendWebRequest();
            if(www.isNetworkError || www.isHttpError){
                m_display = true;
                SetROutSystem("錯誤!!\n請檢查網路狀況");
            }else{
                Debug.Log(www.downloadHandler.text);
                PlayerPrefs.SetInt("Activity",Convert.ToInt32(www.downloadHandler.text));
            }
        }
    }
    
    private string RoleRead(){
        string _rolename = "";
        foreach (var item in m_AllRolesArchive)
        {
            _rolename += PlayerPrefs.GetString(item.name, "");
            if(PlayerPrefs.HasKey(item.name)){
                _rolename += "、";
            }
            
        }
        if(PlayerPrefs.GetInt("Checkpoint", 2) == 2){
            _rolename += m_AllRolesArchive[0].name;
        }
        return _rolename;
    }
    void SetROutSystem(string _str){
        m_Logingo.SetActive(false);
        if(m_display){
            m_ROutGO.SetActive(true);
            m_ROutText.text = _str;
        }
    }

    void GoActiveHierarchy(){
        if(m_SignInInterface.activeInHierarchy){
            m_SignInInterface.SetActive(false);
        }
        if(m_DiscountButton.activeInHierarchy){
            m_DiscountButton.SetActive(false);
        }
    }
}
