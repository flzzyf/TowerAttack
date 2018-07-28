using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    public GameObject panel_loading;
    public Slider slider_loading;
    public AudioMixer audioMixer;

    float playSoundCD = 0.2f;
    float currentPlaySoundCD = 0.2f;

    Resolution[] resolutions;

    public Slider slider_volume_bgm;
    public Slider slider_volume_effect;
    public Toggle toggle_fullScreen;
    public Dropdown dropdown_resolution;
    public Dropdown dropdown_quality;

    public void Start()
    {
        SoundManager.Instance().Play("BGM");

        //添加分辨率选项
        resolutions = Screen.resolutions;
        dropdown_resolution.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdown_resolution.AddOptions(options);
        //dropdown_resolution.value = currentResolutionIndex;

        //根据用户数据进行初始设置
        toggle_fullScreen.isOn = PlayerPrefs.GetInt("fullScreen") == 1;
        slider_volume_bgm.value = PlayerPrefs.GetFloat("volume_bgm");
        slider_volume_effect.value = PlayerPrefs.GetFloat("volume_effect");
        dropdown_resolution.value = PlayerPrefs.GetInt("resolution");
        dropdown_quality.value = PlayerPrefs.GetInt("quality");


        
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
            slider_loading.value = progress;

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
        PlayerPrefs.SetFloat("volume_bgm", _value);

    }
    //设置音效音量
    public void SetVolumeEffect(float _value)
    {
        audioMixer.SetFloat("volume_effect", _value);
        PlayerPrefs.SetFloat("volume_effect", _value);
        if (currentPlaySoundCD <= 0)
        {
            currentPlaySoundCD = playSoundCD;
            SoundManager.Instance().Play("Boom");
        }
    }

    //点击按钮，所有按钮都加上该事件
    public void ClickButton()
    {
        SoundManager.Instance().Play("Boom");
    }

    //设置画面质量
    public void SetQuality(int _value)
    {
        QualitySettings.SetQualityLevel(_value);
        PlayerPrefs.SetInt("quality", _value);
    }
    //设置全屏
    public void SetFullScreen(bool _isFullScreen)
    {
        Screen.fullScreen = _isFullScreen;
        int bool2Int = _isFullScreen ? 1 : 0;
        PlayerPrefs.SetInt("fullScreen", bool2Int);
    }
    //设置分辨率
    public void SetResolution(int _value)
    {
        Resolution resolution = resolutions[_value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", _value);
    }
}
