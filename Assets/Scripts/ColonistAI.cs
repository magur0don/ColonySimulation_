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
        Sleep,      // 就寝
        Carry,      // 運ぶ
        Rest,       // 休憩
        Dead       // 死亡
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
    /// 採掘場所の位置
    /// </summary>
    public Vector3 MinePoint;

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

    /// <summary>
    /// 採掘スキルで高いほど速い
    /// </summary>
    [Range(0.5f, 3f)]
    public float MiningSkill = 1f;

    /// <summary>
    /// 採掘量
    /// </summary>
    public int MinedResource = 0;

    /// <summary>
    /// 空腹度
    /// </summary>
    private float hunger = 100f;

    /// <summary>
    /// ストレス
    /// </summary>
    private float stress = 0f;

    /// <summary>
    /// 生きているかの判定
    /// </summary>
    public bool IsAlive
    {
        // boolは真偽の判定になるので、条件を作ることができます
        // 今回は体力があって、空腹度も飢えていない状態とします
        // ||は日本語で言うと、"か"とか"もしくは”です。
        get { return currentHealth > 0 || hunger > 0; }
    }

    /// <summary>
    /// 倉庫
    /// </summary>
    public Transform Warehouse;

    /// <summary>
    /// 市場の位置
    /// </summary>
    public Transform MarketPosition;


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
            MiningSkill = 3f;
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
            MiningSkill = 2f;
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
            MiningSkill = 1f;
            foreach (var renderer in coloniostMeshRenderers)
            {
                renderer.material = OldMaterial;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // !は否定の意味です。!bool型の変数で、
        // bool型の変数の反対の判定をします
        // 生存していなかったら
        if (!IsAlive)
        {
            State = ColonistState.Dead;
            Debug.Log($"死亡しました");
            return;
        }

        // 1フレームにかかった時間をtimerから減算していきます
        timer -= Time.deltaTime;

        // 1秒間に2ポイントずつ、空腹になっていきます
        hunger -= 2f * Time.deltaTime;

        // 1秒間に1ポイントずつ、ストレスがかかっていきます
        stress += 1f + Time.deltaTime;

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
                    targetPosition = MinePoint;
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
                // 1秒間にminingSkillが3の人は360°一回転できる
                transform.Rotate(Vector3.up * 120f * MiningSkill * Time.deltaTime);

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
                    int mined = Mathf.RoundToInt(10 * MiningSkill);
                    MinedResource += mined;
                    Debug.Log($"採掘完了{mined}(合計{MinedResource})");

                    timer = Random.Range(1f, 15f);
                    // 掘り終わったら運ぶという状態にします
                    State = ColonistState.Carry;
                    // 移動先を倉庫の位置にする
                    targetPosition = Warehouse.position;
                }
                break;
            case ColonistState.Carry:// 運ぶ状態
                transform.position = Vector3.MoveTowards(
              transform.position, targetPosition, MoveSpeed * Time.deltaTime);

                // 体力が回復するまで休ませるか。
                // 体力があったらもう一回Moveにして採掘場に向かわせるか。
                // 休憩する場所に行って、休憩する。
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    targetPosition = MarketPosition.position;
                    // 次の行動を行う(休憩)
                    State = ColonistState.Rest;
                    timer = Random.Range(1f, 5f);
                }
                break;

            case ColonistState.Rest: // 休憩
                transform.position = Vector3.MoveTowards(
         transform.position, targetPosition, MoveSpeed * Time.deltaTime);
                // 市場の位置に近づいたら
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    // 空腹度も1秒間に5ポイントずつ回復
                    hunger += 5f * Time.deltaTime;
                    // ストレスも1秒間に5ポイント緩和
                    stress -= 5f * Time.deltaTime;
                    // 現在の体力をじわじわっと回復させる 
                    currentHealth += RecoveryRate * 2f * Time.deltaTime;

                    // 体力と空腹度が80より回復したら
                    if (currentHealth > 80f && hunger > 80)
                    {
                        timer = 1f;
                        // 状態を待機状態に戻す
                        State = ColonistState.Idle;
                    }
                }
                break;

            case ColonistState.Sleep:

                // 1秒間に8ポイント回復させる
                currentHealth += RecoveryRate * 8f * Time.deltaTime;

                // ストレスも1秒間に5ポイントずつ減っていく
                stress -= 5f * Time.deltaTime;

                // もし、コロニストの体力が完全に回復
                if (currentHealth >= MaxHealth )
                {
                    State = ColonistState.Idle;
                    // timerを1秒から5秒で設定してください。
                    timer = Random.Range(1f, 5f);
                }
                break;
        }
    }
}
