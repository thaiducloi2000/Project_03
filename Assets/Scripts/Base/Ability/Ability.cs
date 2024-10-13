using System;
using UnityEngine;
using UnityEngine.Pool;

[Serializable]
public struct AbilityInfor
{
    public int amountWave;
    public float startDealDamageTime;
    public float delayPerWaveTime;
    public float damage;
    public float countDown;
    public float critRate;
    public float critDamage;
    public int rangeUse;
}


public abstract class Ability : IAbility
{
    protected AbilityInfor _infor;
    public Ability(AbilityInfor infor)
    {
        _infor = infor;
    }

    public abstract void UpgradeAbility(AbilityInfor newInfor);

    public abstract void UseAbility(ITarget caster, ITarget target);

    protected abstract T OnCreate<T>() where T : Component;
    protected abstract void OnReleaseToPool<T>(T pooledObject) where T : Component;
    protected abstract void OnGetFromPool<T>(T pooledObject) where T : Component;

    protected abstract void OnDestroyPooledObject<T>(T pooledObject) where T : Component;
}
