using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {

        public Slider slider;
        public Gradient gradient;
        public Image fill;
        public Player player;

        public void Update()
        {
            slider.maxValue = player.HealthSystem.maxHealth;
            slider.value = player.HealthSystem.health;
            fill.color = gradient.Evaluate(slider.normalizedValue);
        }
    }
}