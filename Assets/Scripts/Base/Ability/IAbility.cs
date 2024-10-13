using System;

public interface IAbility
{
    public void UseAbility(ITarget caster, ITarget target);
}

[AttributeUsage(AttributeTargets.Class)]
public class AbilityAtribute : Attribute
{
    public Type Type { get; private set; }

    public AbilityAtribute(Type type)
    {
        this.Type = type;
    }
}

public enum AbilityType
{
    ProjectTile,
    PlayerInputHandler,
    Enemy,
}