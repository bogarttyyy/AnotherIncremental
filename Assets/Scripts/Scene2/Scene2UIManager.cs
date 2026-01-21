using TMPro;
using UnityEngine;

public class Scene2UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    
    public void UpdatePointsText(int points)
    {
        pointsText.text = $"{points}";
    }
}
