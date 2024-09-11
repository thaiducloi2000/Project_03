using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface ISpawnable
{
    public Transform SpawnObject { get; }
    public ISpawnable OnCreate();

    public void OnRelease(ISpawnable obj);

    public void OnGet(ISpawnable obj);

    public void OnDestroyFromPool(ISpawnable obj);

    public void SetUpPool(IObjectPool<ISpawnable> pool);
}
