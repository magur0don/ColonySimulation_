using UnityEngine;

public class ColonistAI : MonoBehaviour
{
    /// <summary>
    /// enum型で宣言したコロニストの状態
    /// </summary>
    public enum ColonistState
    {
        Idle,        // 待機       
        Move,      // 移動
        Mine,       // 掘削
        Sleep       // 就寝
    }

    public ColonistState State;

    /// <summary>
    /// コロニストの状態を変更するためのタイマー
    /// [SerializeField]のようなものを属性(Attribute)って言います
    /// </summary>
    [SerializeField]
    private float timer = 2f;

    public float MoveSpeed = 2.0f;
    private Vector3 targetPosition = new Vector3(2, 0, 2);

    /// <summary>
    /// 最大体力値
    /// </summary>
    public float MaxHealth = 100f;

    /// <summary>
    /// 現在の体力値
    /// </summary>
    [SerializeField]
    private float currentHealth;

    /// <summary>
    ///  外部から現在の体力を取得させるためのプロパティ
    /// </summary>
    public float GetCurrentHealth
    {
        get { return currentHealth; }
    }

    /// <summary>
    /// 疲労回復速度
    /// </summary>
    public float RecoveryRate = 1f;

    /// <summary>
    /// 疲れやすさ
    /// </summary>
    public float FatigueRate = 1f;

    /// <summary>
    /// コロニストの年齢 
    /// </summary>
    public int ColonistAge = 20;

    /// <summary>
    /// 年齢によって、色を変更する
    /// </summary>
    public Material YoungMaterial;
    public Material NormalMaterial;
    public Material OldMaterial;

    /// <summary>
    /// Colonistの3Dモデル表示部分
    /// </summary>
    private MeshRenderer[] coloniostMeshRenderers = new MeshRenderer[2];

    void Start()
    {
        // コロニストの状態をIdle(待機)から始める
        State = ColonistState.Idle;
        // 現在の体力をMAXにする
        currentHealth = MaxHealth;

        // 3D表示部分を取得
        coloniostMeshRenderers = GetComponentsInChildren<MeshRenderer>();

        // コロニストの年齢を決める
        ColonistAge = Random.Range(18, 70);
        // コロニストの年齢が20まで
        if (ColonistAge < 20)
        {
            RecoveryRate = 2f;
            FatigueRate = 0.5f;
            MoveSpeed = 5f;

            // foreach文は配列に対して、全ての要素に変更を加えたい時に使います
            foreach (var renderer in coloniostMeshRenderers)
            {
                renderer.material = YoungMaterial;
            }
        }
        else if (ColonistAge < 40)// else ifは"そう(if分の条件)じゃなくって"
        {
            RecoveryRate = 1f;
            FatigueRate = 1f;
            MoveSpeed = 2f;
            foreach (var renderer in coloniostMeshRenderers)
            {
                renderer.material = NormalMaterial;
            }
        }
        else // if文の条件でもなく、else if文の条件でもない場合
        { // 40より上
            RecoveryRate = 0.5f;
            FatigueRate = 2f;
            MoveSpeed = 1f;

            foreach (var renderer in coloniostMeshRenderers)
            {
                renderer.material = OldMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 1フレームにかかった時間をtimerから減算していきます
        timer -= Time.deltaTime;

        // 小括弧の中の値(変数)を使って処理を分岐(switch)させます
        switch (State)
        {
            case ColonistState.Idle:

                // 現在の体力をじわじわっと回復させる 
                currentHealth += RecoveryRate * 2f * Time.deltaTime;

                // caseとbreakの間に、caseの場合の処理を書く
                // もしタイマーが0秒を下回ったら
                if (timer <= 0f)
                {
                    // コロニスト君の状態を動くという状態に変更
                    State = ColonistState.Move;
                    // ターゲットポジションを決めてあげる
                    targetPosition = new Vector3(
            Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
                    timer = 2f;
                }
                break;
            case ColonistState.Move:
                transform.position = Vector3.MoveTowards(
                    transform.position, targetPosition, MoveSpeed * Time.deltaTime);

                // 現在の体力値から1秒間で5ポイント体力を減らします
                currentHealth -= FatigueRate * 5f * Time.deltaTime;

                // 現在の体力が20ポイントを下回ったら
                if (currentHealth <= 20f)
                {
                    // 回復するために寝る状態にする
                    State = ColonistState.Sleep;
                }

                // if文はもし、小括弧内の条件だったら、中括弧の中の処理を行う
                // 自分の位置と、ターゲットの位置が10cmより近くなったら
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    // 次の行動を行う
                    State = ColonistState.Mine;
                    // 掘削時間は1秒から5秒までのランダム
                    timer = Random.Range(1f, 5f);
                }
                break;
            case ColonistState.Mine:
                // 仮で採掘アニメーション再生の代わりにログを出力します
                Debug.Log("Colonist is mining!");

                // 毎フレーム回転させ続ける
                transform.Rotate(Vector3.up * 30f * Time.deltaTime);

                // 現在の体力を1秒間に10ポイント減らします
                currentHealth -= FatigueRate * 10f * Time.deltaTime;

                // 現在の体力が20ポイントより少なくなったら
                if (currentHealth <= 20f)
                {
                    // 体力を回復させるためにSleepにする
                    State = ColonistState.Sleep;
                }

                if (timer <= 0f)
                {
                    // State = ColonistState.Idle;
                    // timer = 2f;
                    // timerを1秒～15秒で設定します。
                    timer = Random.Range(1f, 15f);
                    // もし小かっこ内の条件だったら
                    State = ColonistState.Idle;
                }
                break;
            case ColonistState.Sleep:

                // 1秒間に8ポイント回復させる
                currentHealth += RecoveryRate * 8f * Time.deltaTime;

                // もし、コロニストの体力が完全に回復したら
                if (currentHealth >= MaxHealth)
                {
                    State = ColonistState.Idle;
                    // timerを1秒から5秒で設定してください。
                    timer = Random.Range(1f, 5f);
                }
                break;
        }
    }
}
