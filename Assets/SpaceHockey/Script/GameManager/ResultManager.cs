using SpaceHockey.Players;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceHockey.GameManagers
{
    public class ResultManager : MonoBehaviour
    {
        private PhotonView photon;
        private BattleManager battleManager;
        private bool isDisplay = false;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private Text resultText;
        [SerializeField] private Button titleButton;

        private void Start()
        {
            photon = GetComponent<PhotonView>();
            battleManager = GetComponent<BattleManager>();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (isDisplay == true)
                    {
                        photon.RPC("DisplayResult", PhotonTargets.All);
                    }
                });

            titleButton.OnClickAsObservable()
                .First()
                .Subscribe(_ => SceneManager.LoadScene("Title"));
        }

        public void StartResult()
        {
            isDisplay = true;
        }

        [PunRPC]
        private void DisplayResult()
        {
            resultPanel.SetActive(true);
            isDisplay = false;

            if (battleManager._score[0].Value == battleManager.finalScore && PlayerId.Instance.Player_Id == 1)
            {
                resultText.text = "WIN";
                resultText.color = Color.red;
            }
            else if (battleManager._score[1].Value == battleManager.finalScore && PlayerId.Instance.Player_Id == 2)
            {
                resultText.text = "WIN";
                resultText.color = Color.red;
            }
            else
            {
                resultText.text = "LOSE";
                resultText.color = Color.blue;
            }
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(isDisplay);
            }
            else
            {
                isDisplay = (bool)stream.ReceiveNext();
            }
        }
    }
}

