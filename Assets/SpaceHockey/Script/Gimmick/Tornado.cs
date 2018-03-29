using UnityEngine;

namespace SpaceHockey.Gimmicks
{
    public class Tornado : MonoBehaviour
    {
        [SerializeField] private float airResistance;
        [SerializeField] private float windForce;
        [SerializeField] private float windSpeed;

        private void Update()
        {
            transform.RotateAround(Vector3.zero, Vector3.up, windSpeed);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Ball"))
            {
                var rb = other.GetComponent<Rigidbody>();
                var velocity = (other.transform.position - transform.position).normalized * windForce;
                rb.AddForce(airResistance * velocity);
            }
        }
    }
}

