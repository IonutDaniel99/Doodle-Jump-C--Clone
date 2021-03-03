using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public static MenuScript Instance { get; set; }

    public CanvasGroup fadeGroup;
    private float fadeInSpeed = 1f;

    public RectTransform menuContainer;
    public float boxSize = 1.125f;

    public Transform platformPanel;
    public Transform upgradesPanel;
    public TextMeshProUGUI moneyText;

    public Button tiltControlButton;
    public Color tiltControlEnable;
    public Color tiltControlDisable;

    public TextMeshProUGUI platformTypeText;
    public TextMeshProUGUI platformBuySetText;
    public TextMeshProUGUI upgradeTypeText;
    public TextMeshProUGUI upgradesBuyText;

    private int[] platformCost = new int[] { 50, 75, 100, 150, 200, 250, 3500, 500, 750, 1000 };
    private int[] upgradesCost = new int[] { 250, 250, 250, 250, 250 };

    private int selectedPlatformIndex;
    private int selectedUpgradeIndex;
    private int activePlatformIndex;
    private int activeUpgradeIndex;

    private Vector3 desiredMenuPosition;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        //check if we have accelerometer
        if (SystemInfo.supportsAccelerometer)
        {
            tiltControlButton.GetComponent<Image>().color = (SaveManager.Instance.state.usingAccelerometer) ? tiltControlEnable : tiltControlDisable;
        }
        else
        {
            tiltControlButton.gameObject.SetActive(false);
        }

        //Tell our gold text how much should display
        UpdateGoldText();
        //Grab CanvasGroup in scene
        fadeGroup = FindObjectOfType<CanvasGroup>();
        //Start with a white screen;
        fadeGroup.alpha = 1;

        //Add button on-clicl events to shop buttons
        InitShop();

        //Set Player preferences for platform and upgrade

        OnPlatformSelect(SaveManager.Instance.state.activePlatform);
        SetPlatform(SaveManager.Instance.state.activePlatform);

        OnUpgradeSelect(SaveManager.Instance.state.activeUpgrade);
        SetUpgrades(SaveManager.Instance.state.activeUpgrade);

        //Make the button bigger
        platformPanel.GetChild(SaveManager.Instance.state.activePlatform).GetComponent<RectTransform>().localScale = Vector3.one * boxSize;
        upgradesPanel.GetChild(SaveManager.Instance.state.activeUpgrade).GetComponent<RectTransform>().localScale = Vector3.one * boxSize;

    }

    private void Update()
    {
        if (Manager.Instance.IsReturningFromGame == true)
        {
            fadeGroup.alpha = 0;
        }
        else
        {
            fadeGroup.alpha = 1 - Time.timeSinceLevelLoad * fadeInSpeed;
        }

        //Menu nav (smooth)
        menuContainer.anchoredPosition3D = Vector3.Lerp(menuContainer.anchoredPosition3D, desiredMenuPosition, 0.1f);

    }

    private void InitShop()
    {
        //For every children transform under platform color,find button and add on click event
        int i = 0;
        foreach (Transform t in platformPanel)
        {

            int currentIndex = i;

            Button b = t.GetComponent<Button>();

            b.onClick.AddListener(() => OnPlatformSelect(currentIndex));

            //Set Platform of the image based on if owned or not
            RawImage img = t.GetComponent<RawImage>();
            img.texture = SaveManager.Instance.IsPlatformOwned(i) ? Manager.Instance.Platforms[currentIndex] : Manager.Instance.Locked;

            i++;
            if (currentIndex >= 10)
                break;
        }

        //Reset index
        i = 0;
        //Same for upgrades panel

        foreach (Transform t in upgradesPanel)
        {
            int currentIndex = i;


            Button b = t.GetComponent<Button>();

            b.onClick.AddListener(() => OnUpgradeSelect(currentIndex));

            //Set Platform of the image based on if owned or not
            RawImage img = t.GetComponent<RawImage>();
            img.texture = SaveManager.Instance.IsUpgradeOwned(i) ? Manager.Instance.Upgrades[currentIndex] : Manager.Instance.Locked;

            i++;
            if (currentIndex >= 6)
                break;
        }

    }

    private void NavigateTo(int menuIndex)
    {
        switch (menuIndex)
        {
            default:
            case 0:
                desiredMenuPosition = Vector3.zero;
                break;

            case 1:
                desiredMenuPosition = Vector3.right * -1280;
                break;
            case 2:
                desiredMenuPosition = Vector3.right * 1280;
                break;
        }
    }

    private void SetPlatform(int index)
    {
        //Set active platform
        activePlatformIndex = index;
        SaveManager.Instance.state.activePlatform = index;

        //Change platform in game;

        //Change buy/Set button text
        platformBuySetText.text = "Current";
        SaveManager.Instance.Save();
    }

    private void SetUpgrades(int index)
    {
        //Set active platform
        activeUpgradeIndex = index;
        SaveManager.Instance.state.activeUpgrade = index;

        //Change upgrades in game;

        //Change buy/Set button text
        upgradesBuyText.text = "Current";
        SaveManager.Instance.Save();
    }

    public void UpdateGoldText()
    {
        moneyText.text = SaveManager.Instance.state.money.ToString();
    }

    public void OnPlayClick()
    {

        SceneManager.LoadSceneAsync("Game");
    }

    public void OnShopClick()
    {
        NavigateTo(1);
    }

    public void OnSettingsClick()
    {
        NavigateTo(2);
    }

    public void OnBackClick()
    {
        NavigateTo(0);
    }

    private void OnPlatformSelect(int currentIndex)
    {
        Debug.Log("Select platform button : " + currentIndex);

        //if button clicked is already selected ,exit
        if (selectedPlatformIndex == currentIndex)
            return;
        //Make icon bigger
        platformPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * boxSize;
        //Put previous to normal scale
        platformPanel.GetChild(selectedPlatformIndex).GetComponent<RectTransform>().localScale = Vector3.one;
        // Set the selected Color
        selectedPlatformIndex = currentIndex;
        // Change the content of the buy set button. depending on the state of color;

        platformTypeText.text = Manager.Instance.PlatformType[currentIndex];

        if (SaveManager.Instance.IsPlatformOwned(currentIndex))
        {
            //platform is owned

            //is it already cyrrent color
            if (activePlatformIndex == currentIndex)
            {
                platformBuySetText.text = "Current";
            }
            else
            {
                platformBuySetText.text = "Select";
            }

        }
        else
        {
            //platform not owned
            platformBuySetText.text = "Buy: " + platformCost[currentIndex];
        }
    }

    private void OnUpgradeSelect(int currentIndex)
    {
        Debug.Log("Select upgrades button : " + currentIndex);

        if (selectedUpgradeIndex == currentIndex)
            return;

        //get image

        //Make icon bigger
        upgradesPanel.GetChild(currentIndex).GetComponent<RectTransform>().localScale = Vector3.one * boxSize;
        //Put previous to normal scale
        upgradesPanel.GetChild(selectedUpgradeIndex).GetComponent<RectTransform>().localScale = Vector3.one;

        // Set the selected upgrade
        selectedUpgradeIndex = currentIndex;
        // Change the content of the buy set button. depending on the state of color;

        upgradeTypeText.text = Manager.Instance.UpgradeType[currentIndex];

        if (SaveManager.Instance.IsUpgradeOwned(currentIndex))
        {
            //platform is owned
            if (activeUpgradeIndex == currentIndex)
            {
                upgradesBuyText.text = "Current";
            }
            else
            {
                upgradesBuyText.text = "Select";
            }
        }
        else
        {
            //platform not owned
            upgradesBuyText.text = "Buy: " + upgradesCost[currentIndex];
        }
    }

    public void OnPlatformBuySet()
    {
        Debug.Log("Set/buy platform");
        //Is the selected platform owned
        if (SaveManager.Instance.IsPlatformOwned(selectedPlatformIndex))
        {
            SetPlatform(selectedPlatformIndex);
        }
        else
        {
            //buy it
            if (SaveManager.Instance.BuyPlatform(selectedPlatformIndex, platformCost[selectedPlatformIndex]))
            {
                //Succes
                SetPlatform(selectedPlatformIndex);
                //Change Color of button
                platformPanel.GetChild(selectedPlatformIndex).GetComponent<RawImage>().texture = Manager.Instance.Platforms[selectedPlatformIndex];

                //UpdateGoldText;
                UpdateGoldText();
            }
            else
            {
                //No Gold
                Debug.Log("No money");
            }
        }
    }

    public void OnUpgradeBuy()
    {
        Debug.Log("Set/buy upgrades");
        //Is the selected upgrade owned
        if (SaveManager.Instance.IsUpgradeOwned(selectedUpgradeIndex))
        {
            SetUpgrades(selectedUpgradeIndex);
        }
        else
        {
            //buy it
            if (SaveManager.Instance.BuyUpgrade(selectedUpgradeIndex, upgradesCost[selectedUpgradeIndex]))
            {
                //Succes
                SetUpgrades(selectedUpgradeIndex);

                upgradesPanel.GetChild(selectedUpgradeIndex).GetComponent<RawImage>().texture = Manager.Instance.Upgrades[selectedUpgradeIndex];

                UpdateGoldText();
            }
            else
            {
                //No Gold
                Debug.Log("No money");
            }
        }
    }

    public void OnTiltControl()
    {
        //toggle bool acc
        SaveManager.Instance.state.usingAccelerometer = !SaveManager.Instance.state.usingAccelerometer;

        SaveManager.Instance.Save();
        Debug.Log("Salvar");

        //change display
        tiltControlButton.GetComponent<Image>().color = (SaveManager.Instance.state.usingAccelerometer) ? tiltControlEnable : tiltControlDisable;

    }
}
