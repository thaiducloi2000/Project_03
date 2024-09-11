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
    protected IObjectPool<GameObject> poolVfx;
    protected GameObject _vfx;
    protected AbilityInfor _infor;
    public Ability(AbilityInfor infor, GameObject vfx = null,bool CheckCondition = true,int defaultCapacity = 10, int maxSize = 150) 
    {
        _infor = infor;
        _vfx = vfx;
        poolVfx = new ObjectPool<GameObject>(OnCreate, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, CheckCondition, defaultCapacity, maxSize);
    }

    public abstract void UseAbility(ITarget target);

    protected abstract GameObject OnCreate();
    protected abstract void OnReleaseToPool(GameObject pooledObject);
    protected abstract void OnGetFromPool(GameObject pooledObject);

    protected abstract void OnDestroyPooledObject(GameObject pooledObject);
}
