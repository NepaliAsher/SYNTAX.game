using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float damage = 10f;
    private Transform planetCenter;
    private float difficulty = 1f;
    private PlayerController player;

    private void Start()
    {
        planetCenter = FindObjectOfType<PlanetManager>().transform;
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        Vector3 direction = (planetCenter.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * difficulty * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, planetCenter.position) < 1f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public void SetDifficulty(float diff)
    {
        difficulty = diff;
    }
}