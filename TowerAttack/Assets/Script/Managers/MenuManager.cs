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
    float currentPlaySoundCD = 0;

    Resolution[] resolutions;

    public Slider slider_volume_bgm;
    public Slider slider_volume_effect;
    public Toggle toggle_fullScreen;
    public Dropdown dropdown_resolution;
    public Dropdown dropdown_quality;

    public Transform panel_players;
    public GameObject prefab_playerItem;

    public List<Color> playerColor;

    public GameObject panel_option;
    public GameObject panel_option_mobile;

    public Slider slider_volume_bgm_mobile;
    public Slider slider_volume_effect_mobile;
    public Dropdown dropdown_quality_mobile;

    public void Start()
    {
        SoundManager.Instance().Play("BGM");

        //玩家名获取
        if(PlayerPrefs.GetString("Username") == "")
        {
            PlayerPrefs.SetString("Username", "战斗之软泥怪");
        }

        //添加分辨率选项
        resolutions = Screen.resolutions;
        dropdown_resolution.ClearOptions();

        List<string> options = new List<string>();

        //int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
               resolutions[i].height == Screen.currentResolution.height)
            {
                //currentResolutionIndex = i;
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

        slider_volume_bgm_mobile.value = PlayerPrefs.GetFloat("volume_bgm");
        slider_volume_effect_mobile.value = PlayerPrefs.GetFloat("volume_effect");
        dropdown_quality_mobile.value = PlayerPrefs.GetInt("quality");
    }

    //场景加载
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
        if (Time.time - currentPlaySoundCD > playSoundCD)
        {
            currentPlaySoundCD = Time.time;
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

    //进入大厅
    public void EnterLobby()
    {
        ClearLobby();
        AddPlayerItem("玩家1", 0, 2);
    }
    //清空已有玩家
    void ClearLobby()
    {
        for (int i = panel_players.childCount; i > 0; i--)
        {
            DestroyImmediate(panel_players.GetChild(i - 1).gameObject);
        }
    }

    //添加电脑玩家
    public void AddComputer()
    {
        AddPlayerItem("电脑", 0, 1, true);
    }
    //添加玩家项
    public void AddPlayerItem(string _name, int _skin, int _team, bool _isAI = false)
    {
        GameObject go = Instantiate(prefab_playerItem, panel_players);
        go.GetComponent<PlayerItem>().text_name.text = _name;
        go.GetComponent<PlayerItem>().dropdown_skin.value = _skin;
        go.GetComponent<PlayerItem>().dropdown_team.value = _team;
        go.GetComponent<PlayerItem>().dropdown_color.value = panel_players.childCount - 1;
        go.GetComponent<PlayerItem>().isAI = _isAI;
    }

    //游戏开始
    public void GameStart()
    {
        PlayerManager.Instance().players.Clear();

        for (int i = 0; i < panel_players.childCount; i++)
        {
            PlayerItem item = panel_players.GetChild(i).GetComponent<PlayerItem>();
            Player player = new Player(i, item.dropdown_team.value, playerColor[item.dropdown_color.value], new Vector2Int(0, 0), item.isAI);
            PlayerManager.Instance().players.Add(player);
        }
        LoadScene("Game");
    }

    public void Button_Option()
    {
#if UNITY_IPHONE || UNITY_ANDROID
        panel_option_mobile.SetActive(true);
#else
        panel_option.SetActive(true);
#endif

    }

}
