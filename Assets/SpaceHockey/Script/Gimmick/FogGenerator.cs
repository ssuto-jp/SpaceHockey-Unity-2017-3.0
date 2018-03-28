using UnityEngine;

namespace SpaceHockey.Gimmicks
{
    public class FogGenerator : MonoBehaviour
    {
        [SerializeField] private Transform fogSpawnPos;
        private GameObject fogParticle;

        private void OnEnable()
        {
            fogParticle = PhotonNetwork.Instantiate("Fog", fogSpawnPos.transform.position, Quaternion.identity, 0);
        }

        private void OnDisable()
        {
            PhotonNetwork.Destroy(fogParticle);
        }
    }
}

