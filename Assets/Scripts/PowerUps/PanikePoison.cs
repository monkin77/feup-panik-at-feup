using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanikePoison : MonoBehaviour
{
    
    [SerializeField] private float _poisonDuration = 7.0f;
    [SerializeField] private int _damage = 20;
    public float damageInterval = 0.2f;
    private float _damageTimer = 0f;
    private List<Collider2D> _collidersInArea = new List<Collider2D>();

    private Collider2D _collider;
    void Start()
    {
        _collider = GetComponent<Collider2D>();
        StartCoroutine(SelfDestruct());
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Utils.ENEMY_TAG))
        {
            _collidersInArea.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Utils.ENEMY_TAG))
        {
            _collidersInArea.Remove(other);
        }
    }

    private void Update()
    {
        _damageTimer += Time.deltaTime;

        if (_damageTimer >= damageInterval)
        {
            foreach (Collider2D collider in _collidersInArea)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(_damage);
                }
            }
            _damageTimer -= damageInterval;
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(_poisonDuration);
        Destroy(gameObject);
    }
}
