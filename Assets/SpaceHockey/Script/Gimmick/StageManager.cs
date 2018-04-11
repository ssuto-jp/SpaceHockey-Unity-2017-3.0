using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceHockey.Gimmicks
{
    [RequireComponent(typeof(MeteorGenerator))]
    [RequireComponent(typeof(FogGenerator))]
    [RequireComponent(typeof(TornadoGenerator))]
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private Image alartImage;
        [SerializeField] private AudioClip alartAudio;
        [SerializeField] private Light stageLight;
        private MeteorGenerator meteorGenerator;
        private FogGenerator fogGenerator;
        private TornadoGenerator tornadoGenerator;
        private AudioSource audioSource;
        private PhotonView photonView;

        private void Awake()
        {
            meteorGenerator = GetComponent<MeteorGenerator>();
            fogGenerator = GetComponent<FogGenerator>();
            tornadoGenerator = GetComponent<TornadoGenerator>();
            audioSource = GetComponent<AudioSource>();
            photonView = GetComponent<PhotonView>();
        }

        public void ResetStage()
        {
            meteorGenerator.enabled = false;
            fogGenerator.enabled = false;
            tornadoGenerator.enabled = false;

            photonView.RPC("UpBrightness", PhotonTargets.AllViaServer, null);
        }

        public IEnumerator ChangeStage(int i)
        {
            photonView.RPC("Alart", PhotonTargets.AllViaServer, null);

            yield return new WaitForSeconds(2);

            photonView.RPC("DownBrightness", PhotonTargets.AllViaServer, null);
            switch (i)
            {
                case 0:
                    Debug.Log("0");
                    meteorGenerator.enabled = true;
                    break;
                case 1:
                    Debug.Log("1");
                    fogGenerator.enabled = true;
                    break;
                case 2:
                    Debug.Log("2");
                    tornadoGenerator.enabled = true;
                    break;
                default:
                    break;
            }
        }

        [PunRPC]
        private void UpBrightness()
        {
            stageLight.intensity = 1f;
        }

        [PunRPC]
        private void DownBrightness()
        {
            stageLight.intensity = 0.1f;
        }

        [PunRPC]
        private void Alart()
        {
            StartCoroutine(AlartCoroutine());
        }

        private IEnumerator AlartCoroutine()
        {
            alartImage.enabled = true;
            audioSource.PlayOneShot(alartAudio);
            var count = 0;
            while (count < 9)
            {
                ++count;
                alartImage.enabled = !alartImage.enabled;

                yield return new WaitForSeconds(0.3f);
                if (count == 8)
                {
                    alartImage.enabled = false;
                    break;
                }
            }
        }
    }
}

