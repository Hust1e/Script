using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private int damage;

    public float _speed;

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider collider)
    {
        //if (didHit) return;
        //didHit = true;

        if (collider.tag == "Enemy")
        {
            var health = collider.GetComponent<HealthController>();
            health.ApplyDamage(damage);
            var enemyChase = collider.GetComponent<EnemyAI>();
            enemyChase.playerAttackMe = true;

        }
        if (collider.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
    private void Start() 
    {
        Destroy(this.gameObject, 4f);
    }
}
