using System;
public class Skill
{
    public readonly int cost;
    public readonly ExperienceTypes type;
    public readonly Action effect;
    public readonly string description;

    public Skill(int cost, ExperienceTypes type, string description, Action effect)
    {
        this.cost = cost;
        this.type = type;
        this.description = description;
        this.effect = effect;
    }
}