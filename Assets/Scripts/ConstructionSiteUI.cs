using UnityEngine;
using UnityEngine.UI;

public class ConstructionSiteUI : MonoBehaviour
{
    public Image FillImage;

    public ConstructionSite ConstructionSite;

    // Update is called once per frame
    void Update()
    {
        FillImage.fillAmount = ConstructionSite.GetProgress;
    }
}
