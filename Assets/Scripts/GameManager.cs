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
   
    public void Awake()
    {
        Instance = this;
    }

    public void UpdateUI()
    {
        triforceText.text = triforce + "/3";
        bombText.text = bombs.ToString();
        keyText.text = keys + "/3";
        chestText.text = chests + "/4";
    }
}
