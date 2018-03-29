using UnityEngine;

namespace SpaceHockey.Gimmicks
{
    public class TornadoGenerator : MonoBehaviour
    {
        [SerializeField] private Transform tornadoSpawnPos;
        private GameObject tornadoParticle;

        private void OnEnable()
        {
            tornadoParticle = PhotonNetwork.Instantiate("Tornado", tornadoSpawnPos.transform.position, Quaternion.identity, 0);
        }

        private void OnDisable()
        {
            PhotonNetwork.Destroy(tornadoParticle);
        }
    }
}

