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
        Eat,        // 食事 
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
    [SerializeField]
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

    /// <summary>
    /// ベーカリー（食事する場所）の位置
    /// </summary>
    public Transform BakeryPosition;

    /// <summary>
    /// ベーカリーの機能
    /// </summary>
    public Bakery Bakery;

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
        // コロニストの年齢が30まで
        if (ColonistAge < 30)
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
        else if (ColonistAge < 50)// else ifは"そう(if分の条件)じゃなくって"
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
        { // 50より上
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
        stress += 1f * Time.deltaTime;

        // ストレスが100を越えたら勝手に休憩に入る
        if (stress >= 100f)
        {
            Debug.Log($"{name}はストレスが限界！休憩に入ります！");
            State = ColonistState.Rest;
        }
        else if (hunger <= 30f) // 空腹度が30を下回ったら
        {
            Debug.Log($"{name}はおなかが減ったので、休憩に入ります");
            State = ColonistState.Eat;
        }

        // 小括弧の中の値(変数)を使って処理を分岐(switch)させます
        switch (State)
        {
            case ColonistState.Idle:
                HandleIdle();
                break;
            case ColonistState.Move:
                HandleMove();
                break;
            case ColonistState.Mine:
                HandleMine();
                break;
            case ColonistState.Carry:// 運ぶ状態
                HandleCarry();
                break;
            case ColonistState.Rest: // 休憩
                HandleRest();
                break;
            case ColonistState.Eat: // 食事
                HandleEat();
                break;
            case ColonistState.Sleep:
                HandleSleep();
                break;
        }
        // 値の制限
        // Mathf.Clamp(値,最小値,最大値)で最小値から最大値までの値に
        // 制限してくれます
        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth);
    }

    /// <summary>
    ///  待機中の行動
    /// </summary>
    private void HandleIdle()
    {
        // 現在の体力をじわじわっと回復させる 
        currentHealth += RecoveryRate * 2f * Time.deltaTime;

        // もしタイマーが0秒を下回ったら
        if (timer <= 0f)
        {
            // コロニスト君の状態を動くという状態に変更
            State = ColonistState.Move;
            // ターゲットポジションを決めてあげる
            targetPosition = MinePoint;
            timer = 2f;
        }
    }

    /// <summary>
    /// 移動中の行動
    /// </summary>
    private void HandleMove()
    {
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

    }

    /// <summary>
    /// 採掘中の行動
    /// </summary>
    private void HandleMine()
    {
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
    }

    /// <summary>
    /// 倉庫への移動行動
    /// </summary>
    private void HandleCarry()
    {
        transform.position = Vector3.MoveTowards(
              transform.position, targetPosition, MoveSpeed * Time.deltaTime);

        // 体力が回復するまで休ませるか。
        // 休憩する場所に行って、休憩する。
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // 倉庫に資源を置く
            // まず倉庫のコンポーネント(機能)を取得する
            Warehouse warehouse = Warehouse.GetComponent<Warehouse>();

            // もし、倉庫のコンポーネントが見つかったら
            if (warehouse != null)
            {
                // 倉庫に採掘した量を追加する
                warehouse.Store(MinedResource);
                // 倉庫に置いたので、手持ちの採掘量を0にする
                MinedResource = 0;
            }

            // 体力があった場合
            if (currentHealth > 50)
            {
                targetPosition = MinePoint;
                //採掘のための移動
                State = ColonistState.Move;
            }
            else // 体力が危ない場合
            {
                targetPosition = MarketPosition.position;
                // 次の行動を行う(休憩)
                State = ColonistState.Rest;
            }
            timer = Random.Range(1f, 5f);
        }

    }

    /// <summary>
    /// 休憩中の行動
    /// </summary>
    private void HandleRest()
    {
        // ターゲットポジションが市場じゃなかったら市場に変更する
        if (targetPosition != MarketPosition.position)
        {
            targetPosition = MarketPosition.position;
        }

        transform.position = Vector3.MoveTowards(
     transform.position, targetPosition, MoveSpeed * Time.deltaTime);
        // 市場の位置に近づいたら
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ストレスも1秒間に5ポイント緩和
            stress -= 5f * Time.deltaTime;
            // 現在の体力をじわじわっと回復させる 
            currentHealth += RecoveryRate * 2f * Time.deltaTime;

            // 体力が80より回復し、ストレスがなくなったら
            if (currentHealth > 80f && stress <= 0)
            {
                stress = 0f;
                timer = 1f;
                // 状態を待機状態に戻す
                State = ColonistState.Idle;
            }
        }
    }

    /// <summary>
    /// 睡眠行動
    /// </summary>
    private void HandleSleep()
    {
        // 1秒間に8ポイント回復させる
        currentHealth += hunger * 8f * Time.deltaTime;

        // ストレスも1秒間に5ポイントずつ減っていく
        stress -= 5f * Time.deltaTime;

        // もし、コロニストの体力が完全に回復
        if (currentHealth >= MaxHealth)
        {
            State = ColonistState.Idle;
            // timerを1秒から5秒で設定してください。
            timer = Random.Range(1f, 5f);
        }
    }

    /// <summary>
    /// 食事の行動
    /// </summary>
    private void HandleEat()
    {
        if (targetPosition != BakeryPosition.position)
        {
            targetPosition = BakeryPosition.position;
        }

        transform.position = Vector3.MoveTowards(
           transform.position, targetPosition, MoveSpeed * Time.deltaTime);

        // ベーカリーの位置に近づいたら
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // ベーカリーで食事ができた場合
            if (Bakery.CanEat())
            {
                // 食事をして、FoodStockを減らします
                Bakery.Eat();

                // 食事の場所にいったら1秒で20ポイント空腹度が回復する
                hunger += 20f * Time.deltaTime;
                // ストレスも1秒間に5ポイントずつ減っていく
                stress -= 5f * Time.deltaTime;
                // 体力も回復させる
                currentHealth += 2f * hunger * Time.deltaTime;
            }
            else // 食べ物がない……
            {
                currentHealth += 2f * hunger * Time.deltaTime;
            }

            if (hunger >= 99f)
            {
                hunger = 100f;
                Debug.Log($"{name}は満腹になりました ");
                State = ColonistState.Idle;
            }
        }
    }
}
