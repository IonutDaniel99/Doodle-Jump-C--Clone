using UnityEngine;

public class Manager : MonoBehaviour
{

    public static Manager Instance { set; get; }

    public Texture2D Locked;
    public Texture2D[] Platforms = new Texture2D[11];
    public Texture2D[] Upgrades = new Texture2D[6];
    public string[] PlatformType = new string[10];
    public string[] UpgradeType = new string[5];
    public bool IsReturningFromGame = false;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

}
