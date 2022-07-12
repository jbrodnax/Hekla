using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public string Token {get;set;}
    public string Address {get;set;}
    
    public static SessionManager Instance;
    void Awake(){
        if (Instance == null){
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void ExtractToken(SessionInfo info){
        Token = info.token;
    }
}

[System.Serializable]
public class SessionInfo
{
    public bool success;
    public string token;

    public static SessionInfo CreateFromJSON(string jsonstring){
        return JsonUtility.FromJson<SessionInfo>(jsonstring);
    }
}