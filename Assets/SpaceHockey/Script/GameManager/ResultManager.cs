using SpaceHockey.Players;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SpaceHockey.GameManagers
{
    public class ResultManager : MonoBehaviour
    {
        private PhotonView photonView;
        private BattleManager battleManager;
        private bool isDisplay = false;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private Button titleButton;
        [SerializeField] private TextMeshProUGUI resultText;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            battleManager = GetComponent<BattleManager>();

            this.UpdateAsObservable()
                .Subscribe(_ =>
                {
                    if (isDisplay == true)
                    {
                        photonView.RPC("DisplayResult", PhotonTargets.AllViaServer);
                    }
                });

            titleButton.OnClickAsObservable()
                .First()
                .Subscribe(_ =>
                {
                    photonView.RPC("LoadTitleScene", PhotonTargets.AllViaServer);
                });
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

            if (battleManager._score[0].Value == battleManager.MaxScore && PlayerId.Instance.Player_Id == 1)
            {
                resultText.text = "WIN";
                //resultText.color = Color.red;
                resultText.faceColor = Color.red;
            }
            else if (battleManager._score[1].Value == battleManager.MaxScore && PlayerId.Instance.Player_Id == 2)
            {
                resultText.text = "WIN";
                //resultText.color = Color.red;
                resultText.faceColor = Color.red;
            }
            else
            {
                resultText.text = "LOSE";
                //resultText.color = Color.blue;
                resultText.faceColor = Color.blue;
            }
        }

        [PunRPC]
        private void LoadTitleScene()
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene("Title");
        }
    }
}

