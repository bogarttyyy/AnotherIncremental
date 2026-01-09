using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TMP_Text cashText;
    [SerializeField] private Image timeProgressBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateCashText(int cash)
    {
        cashText.text = $"${cash}";
    }

    public void UpdateTime(float time)
    {
        timeProgressBar.fillAmount = time;
    }
}
