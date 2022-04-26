using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class UnityAdScript : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    GameManager gameManager;
    string gameId = "4043017";
    bool testMode = true;
    bool perPlacementMode = false;
    bool intersitialRequested = false;
    bool interstitialReady = false;
    bool rewardedReady = false;

    // For the purpose of this example, these buttons are for functionality testing:
    [SerializeField] Button _loadBannerButton;
    [SerializeField] Button _showBannerButton;
    [SerializeField] Button _hideBannerButton;
    [SerializeField] Button _showRewardedAdButton;

    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    
    [SerializeField] string _androidRewardedAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSRewardedAdUnitId = "Rewarded_iOS";

    [SerializeField] string _androidBannerAdUnitId = "Banner_Android";
    [SerializeField] string _iOSBannerAdUnitId = "Banner_iOS";
    [SerializeField] string _androidInterstitialAdUnitId = "Interstitial_Android";
    [SerializeField] string _iOSInterstitialAdUnitId = "Interstitial_iOS";

    string _interstitialAdUnitId = null; // This will remain null for unsupported platforms.
    string _bannerAdUnitId = null;
    string _rewardedAdUnitId = null;

    // Start is called before the first frame update
    void Start()
    {
        Advertisement.Initialize(gameId, testMode, perPlacementMode);
        gameManager = FindObjectOfType<GameManager>();
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _rewardedAdUnitId = _iOSAdUnitId;
        _interstitialAdUnitId = _iOSAdUnitId;
        _bannerAdUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _rewardedAdUnitId = _androidRewardedAdUnitId;
        _interstitialAdUnitId = _androidInterstitialAdUnitId;
        _bannerAdUnitId = _androidBannerAdUnitId;
#endif

        //_bannerAdUnitId = _androidInterstitialAdUnitId;

        // Disable the button until an ad is ready to show:
        _showBannerButton.interactable = false;
        _hideBannerButton.interactable = false;
        _showRewardedAdButton.interactable = false;

        // Set the banner position:
        Advertisement.Banner.SetPosition(_bannerPosition);

        // Configure the Load Banner button to call the LoadBanner() method when clicked:
        _loadBannerButton.onClick.AddListener(LoadBanner);
        _loadBannerButton.interactable = true;


        //LoadRewardedAd();
        //LoadAd();
        LoadBanner();
        ShowBannerAd();
        _showBannerButton.enabled = false;
    }

    public void LoadBanner()
    {
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_bannerAdUnitId, options);
    }

    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");

        // Configure the Show Banner button to call the ShowBannerAd() method when clicked:
        _showBannerButton.onClick.AddListener(ShowBannerAd);
        // Configure the Hide Banner button to call the HideBannerAd() method when clicked:
        _hideBannerButton.onClick.AddListener(HideBannerAd);

        // Enable both buttons:
        _showBannerButton.interactable = true;
        _hideBannerButton.interactable = true;
    }

    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
    }

    // Implement a method to call when the Show Banner button is clicked:
    void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_bannerAdUnitId, options);
    }

    // Implement a method to call when the Hide Banner button is clicked:
    void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }

    void OnDestroy()
    {
        // Clean up the listeners:
        _loadBannerButton.onClick.RemoveAllListeners();
        _showBannerButton.onClick.RemoveAllListeners();
        _hideBannerButton.onClick.RemoveAllListeners();
        _showRewardedAdButton.onClick.RemoveAllListeners();
    }

    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _interstitialAdUnitId);
        Advertisement.Load(_interstitialAdUnitId, this);
    }

    //Show the loaded content in the Ad Unit:
    public void ShowAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _interstitialAdUnitId);
        Advertisement.Show(_interstitialAdUnitId, this);
    }

    public void LoadRewardedAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _rewardedAdUnitId);
        Advertisement.Load(_rewardedAdUnitId, this);
    }

    public void ShowRewardedAd()
    {
        // Disable the button:
        _showRewardedAdButton.interactable = false;
        // Then show the ad:
        Advertisement.Show(_rewardedAdUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        Debug.Log("Ad Loaded: " + adUnitId);

        //GameObject CUBE = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (adUnitId.Equals(_interstitialAdUnitId))
        {
            interstitialReady = true;
        }
        if (adUnitId.Equals(_rewardedAdUnitId))
        {
            // Configure the button to call the ShowAd() method when clicked:
            _showRewardedAdButton.onClick.AddListener(ShowRewardedAd);
            // Enable the button for users to click:
            _showRewardedAdButton.interactable = true;
            rewardedReady = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }

    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) 
    {
        if (adUnitId.Equals(_rewardedAdUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            // Grant a reward.
            Debug.Log("You get 50 Score!");
            // Load another ad:
            Advertisement.Load(_rewardedAdUnitId, this);
            gameManager.IncrementScore(50);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.realtimeSinceStartup > 6 && !intersitialRequested )
        {

            ShowAd();
            Debug.Log("Interstitial Requested");
            intersitialRequested = true;
        }
        if (Time.realtimeSinceStartup > 12)
        {
            _showRewardedAdButton.onClick.AddListener(ShowRewardedAd);
            // Enable the button for users to click:
            _showRewardedAdButton.interactable = true;
            rewardedReady = true;
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Initialised");
        LoadAd();
        LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        throw new System.NotImplementedException();
    }
}
