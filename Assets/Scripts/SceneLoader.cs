using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    void Awake(){
        if (Instance == null){
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void Load(string scenename){
        SceneManager.LoadScene(scenename);
    }
}
