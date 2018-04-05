using UnityEngine;

namespace SpaceHockey.Gimmicks
{
    public class FogGenerator : MonoBehaviour
    {
        [SerializeField] private Transform fogSpawnPos;
        private GameObject fogParticle;

        private void OnEnable()
        {
            fogParticle = PhotonNetwork.Instantiate("Fog", fogSpawnPos.transform.position, fogSpawnPos.transform.rotation, 0);
        }

        private void OnDisable()
        {
            PhotonNetwork.Destroy(fogParticle);
        }
    }
}

