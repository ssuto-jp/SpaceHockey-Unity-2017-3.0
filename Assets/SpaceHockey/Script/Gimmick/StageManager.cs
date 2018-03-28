using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceHockey.Gimmicks
{
    [RequireComponent(typeof(MeteorGenerator))]
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private Image alartImage;

        private MeteorGenerator meteorGenerator;
        private PhotonView photonView;

        private void Awake()
        {
            meteorGenerator = GetComponent<MeteorGenerator>();
            photonView = GetComponent<PhotonView>();
        }

        public void ResetStage()
        {
            meteorGenerator.enabled = false;
        }

        public IEnumerator ChangeStage(int i)
        {
            photonView.RPC("Alart", PhotonTargets.AllViaServer, null);

            yield return new WaitForSeconds(2);

            switch (i)
            {
                case 1:
                    Debug.Log("1");
                    meteorGenerator.enabled = true;
                    break;
                case 2:
                    Debug.Log("2");
                    //
                    break;
                case 3:
                    Debug.Log("3");
                    //
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

