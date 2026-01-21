using NSBLib.EventChannelSystem;
using UnityEngine;

public class Scene2GameManager : MonoBehaviour
{
    public int points = 0;

    [SerializeField] private IntEventChannel OnUpdatePointText;
    
    public void AddPoints(int point)
    {
        points += point;
        OnUpdatePointText.Invoke(points);
    }
}
