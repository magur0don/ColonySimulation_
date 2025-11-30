using UnityEngine;

public class Warehouse : MonoBehaviour
{
    /// <summary>
    /// 倉庫が保持する資源量
    /// </summary>
    public int StoredResources = 0;

    /// <summary>
    /// 在庫の最大値
    /// </summary>
    private int maxStockAmount = 1000;

    /// <summary>
    ///  外部から倉庫の在庫の最大値を取得する
    /// </summary>
    public int GetMaxStockAmount
    {
        get { return maxStockAmount; }
    }

    // 交換できるかの判定用のフラグ
    public bool HasEnough(float amount)
    {
        // 引数の個数より在庫数が多ければtrueを返す
        return StoredResources >= amount;
    }

    public bool IsFull()
    {
        // 最大個数より、StoredResourcesが多かったら
        return maxStockAmount <= StoredResources;
    }


    /// <summary>
    /// 引数の数、倉庫が保持する資源量を増やします
    /// </summary>
    /// <param name="amount"></param>
    public void Store(int amount)
    {
        StoredResources += amount;
        Debug.Log($"倉庫に{amount}納品" +
            $"（合計:{StoredResources}）");
    }

    /// <summary>
    /// 倉庫からamount分引き出します
    /// </summary>
    public void Withdraw(int amount)
    {
        // amountをintに型に変更して、0を下回らないようにする
        StoredResources = Mathf.Max(0, StoredResources - amount);
    }
}
