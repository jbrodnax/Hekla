using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public string ServerAddress {get;set;}
    public SessionInfo Info;
    public SessionInfo.Stripped Info_stripped;

    public static SessionManager Instance;
    void Awake(){
        if (Instance == null){
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public bool SaveSessionInfo(string json){
        try{
            Info = SessionInfo.CreateFromJSON(json);
        }catch (Exception e){
            Debug.LogError($"Caught Exception during deserialization: {e}");
        }

        if (Info.success == false)
            return false;

        try{
            Info_stripped = SessionInfo.Stripped.CreateFromJSON(json);
        }catch (Exception e){
            Debug.LogError($"Caught Exception during deserialization: {e}");
        }

        return true;
    }

    public string GetToken(){
        return Info.token;
    }

    public string GetTokenJson(){
        return Info_stripped.ExtractAsJson();
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

    public class Stripped
    {
        public string token;

        public static Stripped CreateFromJSON(string json){
            return JsonUtility.FromJson<Stripped>(json);
        }

        public string ExtractAsJson(){
            return JsonUtility.ToJson(this);
        }
    }
}