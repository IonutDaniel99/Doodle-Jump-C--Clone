using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdCaller : MonoBehaviour, IUnityAdsListener
{

    private string GooglePlay_ID = "3630285";
    public bool TestMode = true;
    public int[] moneyGive = { 10, 15, 25, 50 };
    public Text MoneyText;
    public GameObject AdPanel;

    private string myPlacementId = "rewardedVideo";

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(GooglePlay_ID, TestMode);
    }

    public void DisplayVideoAd()
    {
        Advertisement.Show(myPlacementId);
    }

    public void CloseAdsPanel()
    {
        AdPanel.SetActive(false);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
        if (showResult == ShowResult.Finished)
        {
            AdPanel.SetActive(true);
            int randomMoney = RandomMoneyNumber();
            MoneyText.text = randomMoney.ToString();
            SaveManager.Instance.state.money += randomMoney;
            MenuScript.Instance.UpdateGoldText();
            SaveManager.Instance.Save();

        }
        else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {

        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    private int RandomMoneyNumber()
    {
        int a = Random.Range(0, 100);
        Debug.Log("Money rand Chance ads = " + a);
        if (a <= 15)
        {
            return moneyGive[3];
        }
        else if (a > 15 && a <= 25)
        {
            return moneyGive[2];
        }
        else if (a > 25 && a <= 50)
        {
            return moneyGive[1];
        }
        else
        {
            return moneyGive[0];
        }
    }
}
