using UnityEngine;

public class BGMManager : MonoBehaviour
{

    public static BGMManager Instance;

    private AudioSource bgmAudioSource;

    /// <summary>
    /// InGameというのはゲーム本編のことです。
    /// InGameの反対はOutGameで、
    /// 例えば装備画面やクエストの選択画面など、遊び本来の部分以外のことを指します。
    /// </summary>
    public AudioClip InGameBGM;

    private void Awake()
    {
        Instance = this;
        if (bgmAudioSource == null)
        {
            bgmAudioSource = this.gameObject.AddComponent<AudioSource>();
            // "BGMVolume"という文字列が鍵となっているので、鍵を使ってfloatを呼び出す
            bgmAudioSource.volume = PlayerPrefs.GetFloat("BGMVolume");
        }
    }

    private void Start()
    {
        PlayBGM(InGameBGM);
    }

    public void PlayBGM(AudioClip bgmClip)
    {
        if (bgmAudioSource.clip == bgmClip)
        {
            return;
        }
        // 再生する音源を設定
        bgmAudioSource.clip = bgmClip;
        // ループ再生できるように設定
        bgmAudioSource.loop = true;
        // 音源を再生
        bgmAudioSource.Play();
    }

    /// <summary>
    /// 外部のSliderから音量を調整する
    /// </summary>
    /// <param name="value"></param>
    public void ChangeBGMVolume(float value)
    {
        bgmAudioSource.volume = value;
        SaveBGMVolume();
    }

    /// <summary>
    /// PlayerPrefsを使って音量の値を保存する。
    /// </summary>
    private void SaveBGMVolume()
    {
        // "BGMVolume"という文字列を鍵にして、floatの値を保存します
        PlayerPrefs.SetFloat("BGMVolume", bgmAudioSource.volume);
        PlayerPrefs.Save();
    }
}
