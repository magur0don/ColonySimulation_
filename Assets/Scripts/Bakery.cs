using UnityEngine;

public class Bakery : MonoBehaviour
{
    public float FoodStock = 100;

    /// <summary>
    ///  倉庫資源10 → 食料1に変えるレート
    /// </summary>
    public float ExchangeRate = 100f;

    /// <summary>
    /// 生産速度（毎秒）
    /// </summary>
    public float ProduceRate = 20f;

    /// <summary>
    /// 倉庫の中見を見たいので参照
    /// </summary>
    public Warehouse Warehouse;

    /// <summary>
    /// 時間を測るのに必要なタイマー
    /// </summary>
    private float timer = 0f;

    private void Update()
    {
        // 時間を足していって
        timer += Time.deltaTime;
        // 1秒超えたら交換するように調整
        if (ProduceRate <= timer)
        {
            ExchangeWithWarehouse();
            // timerをリセットします
            timer = 0f;
        }
    }

    public void ExchangeWithWarehouse()
    {
        if (Warehouse == null)
        {
            // ログの説明
            //参照されてなかったりすると困るので、Warningで注意喚起する
            Debug.LogWarning("WarehouseがUnityで設定されていません");
            // LogErrorにすると、ゲーム実行がストップします。
            // Debug.LogError("WarehouseがUnityで設定されていません");
            return;
        }
        // 倉庫に十分な在庫があった時
        if (Warehouse.HasEnough(ExchangeRate))
        {
            // 倉庫から交換を行う
            Warehouse.Withdraw((int)ExchangeRate);
            // 毎秒、FoodStockをProduceRateに合わせて加算していく
            FoodStock += ProduceRate;
        }
    }
    /// <summary>
    /// ベーカリーで食事ができるかどうか
    /// </summary>
    /// <returns></returns>
    public bool CanEat()
    {
        return FoodStock > 0;
    }

    public void Eat()
    {
        FoodStock -= Time.deltaTime;
    }
}
