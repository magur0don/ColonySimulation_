using UnityEngine;

public class SEManager : MonoBehaviour
{
    /// <summary>
    /// SEManagerをどこからでも呼べるようにstatic変数を用意します
    /// static修飾子をつけると、ゲーム実行時にどこからでも参照することができます。
    /// </summary>
    public static SEManager Instance;

    /// <summary>
    /// AudioSourceは音を鳴らすためのスピーカーの役割をするコンポーネント
    /// </summary>
    private AudioSource SEAudioSource;

    /// <summary>
    /// Startが実行されるより前に実行されるメソッド
    /// 主に初期化などを行うときに使われる
    /// </summary>
    private void Awake()
    {
        Instance = this;
        if (SEAudioSource == null)
        {
            // AddComponentはこのクラスが追加されたGameObjectに、
            // 指定したコンポーネントを追加したいときに使います。
            SEAudioSource = this.gameObject.AddComponent<AudioSource>();
        }
    }

    /// <summary>
    /// SEを再生するためのメソッド
    /// 引数のAudioClip(mp3ファイルなど)の音源をAudioSourceに再生させる
    /// </summary>
    /// <param name="audioClip"></param>
    public void PlaySE(AudioClip audioClip)
    {
        SEAudioSource.PlayOneShot(audioClip);
    }

    /// <summary>
    /// 外部のSliderから音量を調整する
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSEVolume(float value)
    {
        SEAudioSource.volume = value;

    }
}
