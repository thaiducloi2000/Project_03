using UnityEngine;
using UnityEngine.Pool;

public class ProjecttileAbility : Ability
{
    protected ITrajectorStrategy _trajector;
    protected int amount;
    protected Projectile projectileFx;
    protected IObjectPool<Projectile> poolVfx;

    public ProjecttileAbility(AbilityInfor infor, ITrajectorStrategy trajector, Projectile vfx = null, bool CheckCondition = true, int defaultCapacity = 10, int maxSize = 150) : base(infor)
    {
        _trajector = trajector;

        poolVfx = new ObjectPool<Projectile>(OnCreate<Projectile>, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject, CheckCondition, defaultCapacity, maxSize);
        projectileFx = vfx;
    }

    public override void UpgradeAbility(AbilityInfor newInfor)
    {
        _infor = newInfor;
    }

    public override void UseAbility(ITarget caster, ITarget target)
    {
        Vector3 direction = target.CenterPosition - caster.CenterPosition;
        foreach (Vector3 dir in _trajector.Directions(direction))
        {
            Projectile projectiles = poolVfx.Get() as Projectile;

            projectiles.transform.forward = dir;
            projectiles.transform.position = caster.CenterPosition;

            projectiles.Fly(dir);
        }
    }

    protected override T OnCreate<T>()
    {
        Projectile obj = GameObject.Instantiate(projectileFx) as Projectile;
        obj.SetupPool(poolVfx);
        return obj as T;
    }

    protected override void OnDestroyPooledObject<T>(T pooledObject)
    {
        Object.Destroy(pooledObject);
    }

    protected override void OnGetFromPool<T>(T pooledObject)
    {
        pooledObject.gameObject.SetActive(true);
    }

    protected override void OnReleaseToPool<T>(T pooledObject)
    {
        pooledObject.gameObject.SetActive(false);
    }
}
