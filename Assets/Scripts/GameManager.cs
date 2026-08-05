using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int triforce;
    public int bombs;
    public int keys;
    public int chests;

    public TMP_Text triforceText;
    public TMP_Text bombText;
    public TMP_Text keyText;
    public TMP_Text chestText;
    
    public int keyLocksDestroyed = 0;
    [SerializeField] private GameObject objectToDisable; 
   
    public void Awake()
    {
        //Instance = this;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

    }

    private void start()
    {
        UpdateUI();
    }

    public void AddTriforce()
    {
        triforce++;
        UpdateUI();
    }


    public void AddBomb()
    {
        bombs++;
        UpdateUI();
    }


    public void AddKey()
    {
        keys++;
        UpdateUI();
    }


    public void AddChest()
    {
        chests++;
        UpdateUI();
    }

    public bool UseBomb()
    {
        if (bombs <= 0)
            return false;

        bombs--;
        UpdateUI();
        return true;
    }

    public bool UseKey()
    {
        if (keys <= 0)
            return false;

        keys--;
        UpdateUI();
        return true;
    }


    public void UpdateUI()
    {
        triforceText.text = triforce + "/3";
        bombText.text = bombs.ToString();
        keyText.text = keys + "/3";
        chestText.text = chests + "/4";
    }

    

public void OnKeyLockDestroyed()
{
    keyLocksDestroyed++;
    
    if (keyLocksDestroyed >= 4 && objectToDisable != null)
    {
        objectToDisable.SetActive(false);
    }
}


}
