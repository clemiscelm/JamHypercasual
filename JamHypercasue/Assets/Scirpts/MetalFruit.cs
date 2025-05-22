using IIMEngine.SFX;
using UnityEngine;

public class MetalFruit : MonoBehaviour
{
   public GameObject whole;
    public GameObject sliced;
    public bool isSliced = false;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem juiceEffect;
    public bool isLucky = false;

    public int points = 1;
    public int pv = 5;

    private void Awake()
    {
        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        juiceEffect = transform.parent.GetComponentInChildren<ParticleSystem>();
    }

    private void OnDestroy()
    {
        if (!isSliced && FindAnyObjectByType<GameManager>().isGameRunning)
        {
            FindAnyObjectByType<Life>().TakeDamage(1);
        }
    }

    private void Slice(Vector3 direction, Vector3 position, float force)
    {
        GameManager.Instance.IncreaseScore(points);

        // Disable the whole fruit
        fruitCollider.enabled = false;
        whole.SetActive(false);

        // Enable the sliced fruit
        sliced.SetActive(true);
        isSliced = true;
        juiceEffect.Play();

        // Rotate based on the slice angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //sliced.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();
        sliced.transform.position = transform.position;
        sliced.transform.rotation = transform.rotation;
        // Add a force to each slice based on the blade direction
        foreach (Rigidbody slice in slices)
        {
            slice.linearVelocity = fruitRigidbody.linearVelocity;
            slice.angularVelocity = fruitRigidbody.angularVelocity;
            //slice.AddForceAtPosition(direction * force, position, ForceMode.Impulse);
            slice.AddExplosionForce(100, position + Vector3.up * 1.5f, 15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InGameUI.Instance.IncrementCombo();
            print("slice");
            pv--;
            juiceEffect.Play();
            SFXsManager.Instance.PlaySound("Slice");

            if(pv > 0)
            {
                return;
            }
            Blade blade = other.GetComponent<Blade>();
            FindAnyObjectByType<GameManager>().time += points;
            Slice(blade.direction, blade.transform.position, blade.sliceForce);
            if(isLucky)
            {
                FindAnyObjectByType<Life>().AddLife();
            }
        }
    }
   
}
