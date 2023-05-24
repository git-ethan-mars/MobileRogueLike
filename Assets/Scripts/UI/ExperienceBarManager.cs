using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ExperienceBarManager : MonoBehaviour
    {
        [SerializeField] private Image[] experienceBars;
        [SerializeField] private Player player;
        

        private void Update()
        {
            for (var i = 0; i < experienceBars.Length; i++)
            {
                experienceBars[i].fillAmount = player.ExperienceSystem.GetExperiencePercentage(i);
            }
            
        }
    }
}