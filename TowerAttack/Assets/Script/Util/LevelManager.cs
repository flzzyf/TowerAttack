using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour 
{
    public GameObject panel_loading;
    public Slider slider;

    public void LoadScene(string _name)
    {
        panel_loading.SetActive(true);

        StartCoroutine(LoadSceneAsync(_name));
    }

    IEnumerator LoadSceneAsync(string _name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_name);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;

            yield return null;
        }
    }

}
