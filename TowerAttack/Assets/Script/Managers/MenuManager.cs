using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    public GameObject panel_loading;
    public Slider slider;
    public AudioMixer audioMixer;

    float playSoundCD = 0.2f;
    float currentPlaySoundCD;

    public Dropdown dropdown_resolution;

    Resolution[] resolutions;

    public void Start()
    {
        SoundManager.Instance().Play("BGM");

        resolutions = Screen.resolutions;

        //添加分辨率选项
        dropdown_resolution.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdown_resolution.AddOptions(options);
        dropdown_resolution.value = currentResolutionIndex;
    }

    private void Update()
    {
        if (currentPlaySoundCD > 0)
            currentPlaySoundCD -= Time.deltaTime;
    }

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

    public void Quit()
    {
        Application.Quit();
    }
    //设置BGM音量
    public void SetVolumeBGM(float _value)
    {
        audioMixer.SetFloat("volume_bgm", _value);
    }

    public void SetVolumeEffect(float _value)
    {
        audioMixer.SetFloat("volume_effect", _value);

        if(currentPlaySoundCD <= 0)
        {
            currentPlaySoundCD = playSoundCD;
            SoundManager.Instance().Play("Boom");
        }
    }

    public void ClickButton()
    {
        SoundManager.Instance().Play("Boom");
    }

    public void SetQuality(int _value)
    {
        QualitySettings.SetQualityLevel(_value);
    }

    public void SetFullScreen(bool _isFullScreen)
    {
        Screen.fullScreen = _isFullScreen;
    }

    public void SetResolution(int _value)
    {
        Resolution resolution = resolutions[_value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
