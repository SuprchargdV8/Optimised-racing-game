using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void playCareer(){
        SceneManager.LoadScene(1);
    }

    public void clearSave(){
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void mainMenu(){
        SceneManager.LoadScene(0);
    }

}
