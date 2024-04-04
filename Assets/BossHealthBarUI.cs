using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public Slider healthBar;

    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }

    public void SetHealth(int health)
    {
        healthBar.value = health;
    }
}

