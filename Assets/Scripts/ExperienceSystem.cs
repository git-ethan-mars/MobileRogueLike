using System;
using UnityEngine;

public class ExperienceSystem : MonoBehaviour
{
    private int[] _experienceScore;

    private bool[,] _availableSkills =
    {
        { true, false, false },
        { true, false, false },
        { true, false, false }
    };
    
    private void Start()
    {
        _experienceScore = new int[Enum.GetNames(typeof(ExperienceTypes)).Length];
    }

    public void AddExperienceByType(ExperienceTypes type, int value)
    {
        _experienceScore[(int) type] += value;
    }
    
    public void DecreaseExperienceByType(ExperienceTypes type, int value)
    {
        _experienceScore[(int) type] -= value;
    }
    
    public float GetExperiencePercentage(int index)
    {
        return Math.Min(1, (float) _experienceScore[index] / 10);
    }

    public int GetExperienceScoreByType(ExperienceTypes type)
    {
        return _experienceScore[(int) type];
    }

    public void LearnSkill(Skill skill, int branch, int skillNumber)
    {
        if (_experienceScore[(int)skill.type] >= skill.cost && _availableSkills[branch, skillNumber])
        {
            _availableSkills[branch, skillNumber == 2 ? skillNumber : skillNumber + 1] = true;
            skill.effect();
            _experienceScore[(int) skill.type] -= skill.cost;
        }
    }
    
}