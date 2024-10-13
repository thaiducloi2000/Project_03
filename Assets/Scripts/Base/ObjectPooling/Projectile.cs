using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public Transform SpawnObject;
    [SerializeField] private float speed;
    [SerializeField] private float fireRate;
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private List<GameObject> trails;
    [SerializeField] private float lifeTime;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float maxDistanceFlyByFrame;

    private Vector3 startPos;
    private float speedRandomness;
    private float maxSpeed;

    private Transform muzzleVFX;
    private ParticleSystem muzzlePs;
    private Rigidbody rb;

    private Transform hitVFX;
    private ParticleSystem hitPs;
    private IObjectPool<Projectile> _pool;
    private Vector3 direction;

    private Quaternion rot;
    private Vector3 pos;
    private bool isActive = false;

    void Awake()
    {
        maxSpeed = speed;
        rb = GetComponent<Rigidbody>();
        SpawnObject = this.transform;
    }

    public void Fly(Vector3 forward)
    {
        isActive = true;
        speed = maxSpeed;
        startPos = transform.position;
        direction = forward.normalized;

        SpawnObject.forward = direction;

        if (muzzlePrefab != null)
        {
            if (muzzleVFX == null)
            {
                muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity).transform;
            }
            else
            {
                muzzleVFX.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
            muzzleVFX.transform.forward = direction;
            if (muzzlePs == null)
            {
                muzzlePs = muzzleVFX.GetComponent<ParticleSystem>();
                if (muzzlePs == null)
                {
                    muzzlePs = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                }
            }

            muzzleVFX.root.gameObject.SetActive(true);
            Invoke(nameof(HideMuzzleVFX), muzzlePs.main.duration);
        }

        if (trails.Count > 0)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                trails[i].SetActive(true);
            }
        }

        Invoke(nameof(ReleaseToPool), lifeTime * speed);
    }

    public void SetupPool(IObjectPool<Projectile> pool)
    {
        _pool = pool;
    }

    void FixedUpdate()
    {
        if (speed != 0)
            rb.position += (direction * (speed * Time.deltaTime));
    }

    void OnCollisionEnter(Collision co)
    {
        if (trails.Count > 0)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                trails[i].SetActive(false);
            }
        }

        speed = 0;

        ContactPoint point = co.contacts[0];
        rot = Quaternion.FromToRotation(Vector3.up, point.normal);
        pos = point.point;

        if (hitPrefab != null)
        {
            if (hitVFX == null)
            {
                hitVFX = Instantiate(hitPrefab, pos, rot).transform;
            }
            else
            {
                hitVFX.SetLocalPositionAndRotation(pos, rot);
            }

            if (hitPs == null)
            {
                hitPs = hitVFX.GetComponent<ParticleSystem>();
                if (hitPs == null)
                {
                    hitPs = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                }
            }

            hitVFX.gameObject.SetActive(false);
            hitVFX.gameObject.SetActive(true);

            Invoke(nameof(HideHitVFX), hitPs.main.duration + lifeTime);
        }

        StartCoroutine(DestroyParticle(1f));
    }

    public IEnumerator DestroyParticle(float waitTime)
    {

        if (transform.childCount > 0 && waitTime != 0)
        {
            List<Transform> tList = new List<Transform>();

            foreach (Transform t in transform.GetChild(0).transform)
            {
                tList.Add(t);
            }

            while (transform.GetChild(0).localScale.x > 0)
            {
                yield return new WaitForSeconds(0.01f);
                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for (int i = 0; i < tList.Count; i++)
                {
                    tList[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }

        yield return new WaitForSeconds(waitTime);
        ReleaseToPool();
        StopAllCoroutines();
    }

    private void HideMuzzleVFX()
    {
        muzzleVFX.gameObject.SetActive(false);
    }

    private void HideHitVFX()
    {
        hitVFX.gameObject.SetActive(false);
    }

    private void ReleaseToPool()
    {
        if (isActive)
        {
            speed = 0;
            _pool.Release(this);
            isActive = false;
        }
        else
        {
            CancelInvoke();
        }
    }
}
