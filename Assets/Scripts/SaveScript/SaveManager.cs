using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();

        //Are we using accelerometer and use it
        if (state.usingAccelerometer && !SystemInfo.supportsAccelerometer)
        {
            state.usingAccelerometer = false;
            Save();
        }
    }

    //Save state of this saveSatte script to player pref

    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveState>(state));
    }

    // Load the previous saved state from player prefs
    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
            Debug.Log("No Save Fond!Create new one");
        }
    }

    // Check if the platform is owned
    public bool IsPlatformOwned(int index)
    {
        //Check if the bit is set,if so the platform is owned
        return (state.platformOwned & (1 << index)) != 0;
    }

    public bool IsUpgradeOwned(int index)
    {
        //Check if the bit is set,if so the platform is owned
        return (state.upgradesOwned & (1 << index)) != 0;
    }

    //Attempt buyting a platform,return true/false
    public bool BuyPlatform(int index, int cost)
    {
        if (state.money >= cost)
        {
            //Enough money,remove money from stack
            state.money -= cost;
            UnlockPlatform(index);
            //save progers
            Save();

            return true;
        }
        else
        {
            //not enough money.return false
            return false;
        }
    }

    public bool BuyUpgrade(int index, int cost)
    {
        if (state.money >= cost)
        {
            //Enough money,remove money from stack
            state.money -= cost;
            UnlockUpgrade(index);
            //save progers
            Save();

            return true;
        }
        else
        {
            //not enough money.return false
            return false;
        }
    }

    //Unlock a platform in the ''platformOwned'' int
    public void UnlockPlatform(int index)
    {
        //Toggle on the bit at index
        state.platformOwned |= 1 << index;
    }
    //Unlock a upgrade in the ''upgradeOwned'' int
    public void UnlockUpgrade(int index)
    {
        //Toggle on the bit at index
        state.upgradesOwned |= 1 << index;
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }

}
