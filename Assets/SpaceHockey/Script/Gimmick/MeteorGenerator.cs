using System.Collections;
using UnityEngine;

namespace SpaceHockey.Gimmicks
{
    public class MeteorGenerator : MonoBehaviour
    {
        [SerializeField] private Transform meteorSpawnPos;
        [SerializeField] private Transform[] meteorLandingPos;
        private bool isMeteor;

        private void OnEnable()
        {
            isMeteor = true;
            StartCoroutine(Meteor());
        }

        private void OnDisable()
        {
            isMeteor = false;
        }

        public IEnumerator Meteor()
        {
            while (isMeteor)
            {
                var meteor = PhotonNetwork.Instantiate("Meteor", meteorSpawnPos.position, Quaternion.identity, 0);
                //meteor.transform.position = Vector3.Lerp(meteorSpawnPos.position, meteorLandingPos[Random.Range(0, 5)].position, 0.8f);
                meteor.transform.position = meteorLandingPos[Random.Range(0, 5)].position;

                yield return new WaitForSeconds(1);

                if (!isMeteor)
                {
                    break;
                }
            }
        }


    }
}

