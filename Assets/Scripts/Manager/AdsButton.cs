using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdsButton : MonoBehaviour,IUnityAdsListener
{
#if UNITY_IOS
      private string gameId = "4012676";
#elif UNITY_ANDROID
    private string gameId = "4012677";
#endif

    Button adsButton;
    public string myPlacementId = "rewardedVideo";
    // Start is called before the first frame update
    void Start()
    {
        adsButton = GetComponent<Button>();
        //adsButton.interactable = Advertisement.IsReady(myPlacementId);
        if(adsButton)
        {
            adsButton.onClick.AddListener(ShowRewardedVideo);
        }

        Advertisement.AddListener(this);
        Advertisement.Initialize(gameId, true);
    }

    void ShowRewardedVideo()
    {
        Advertisement.Show(myPlacementId);
    }
    public void OnUnityAdsDidError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        switch (showResult)
        {
            case ShowResult.Failed:
                break;
            case ShowResult.Skipped:
                break;
            case ShowResult.Finished:
                Debug.Log("奖励");
                FindObjectOfType<PlayerController>().health = 3;
                FindObjectOfType<PlayerController>().isDead = false;
                UIManager.instance.UpdateHealth(FindObjectOfType<PlayerController>().health);
                break;
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
       
    }

    public void OnUnityAdsReady(string placementId)
    {
        if(Advertisement.IsReady(myPlacementId))
        {
            Debug.Log("广告准备好了");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
