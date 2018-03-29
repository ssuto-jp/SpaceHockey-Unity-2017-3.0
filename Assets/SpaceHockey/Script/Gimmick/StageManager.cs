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

        private MeteorGenerator meteorGenerator;
        private FogGenerator fogGenerator;
        private TornadoGenerator tornadoGenerator;
        private PhotonView photonView;

        private void Awake()
        {
            meteorGenerator = GetComponent<MeteorGenerator>();
            fogGenerator = GetComponent<FogGenerator>();
            tornadoGenerator = GetComponent<TornadoGenerator>();
            photonView = GetComponent<PhotonView>();
        }

        public void ResetStage()
        {
            meteorGenerator.enabled = false;
            fogGenerator.enabled = false;
            tornadoGenerator.enabled = false;
        }

        public IEnumerator ChangeStage(int i)
        {
            photonView.RPC("Alart", PhotonTargets.AllViaServer, null);

            yield return new WaitForSeconds(2);

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
        private void Alart()
        {
            StartCoroutine(AlartCoroutine());
        }

        private IEnumerator AlartCoroutine()
        {
            alartImage.enabled = true;
            var count = 0;
            while (count < 7)
            {
                ++count;
                alartImage.enabled = !alartImage.enabled;

                yield return new WaitForSeconds(0.3f);
                if (count == 6)
                {
                    alartImage.enabled = false;
                    break;
                }
            }
        }
    }
}

