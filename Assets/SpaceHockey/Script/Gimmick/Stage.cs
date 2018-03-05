using System.Collections.Generic;
using SpaceHockey.Balls;
using SpaceHockey.GameManagers;
using UniRx;
using UnityEngine;

namespace SpaceHockey.Gimmicks
{
    public class Stage : MonoBehaviour
    {
        private BattleManager battleManager;
        private Ball ballScript;
        private PhotonView photonView;      
        private Renderer r;
        public GameObject ballPrefab;
        public GameObject wallPrefab;
        public GameObject gameManager;
        private bool isChangeColor;
        private readonly List<Color> stageColor = new List<Color>()
        {
            Color.white,
            Color.red,
            Color.blue,
            Color.yellow
        };

        private void Awake()
        {
            ColorSerializer.Register();
        }

        private void Start()
        {
            battleManager = gameManager.GetComponent<BattleManager>();
            ballScript = ballPrefab.GetComponent<Ball>();
            photonView = GetComponent<PhotonView>();            
            r = wallPrefab.GetComponent<Renderer>();


            battleManager.IsBallSet
                .Where(b => b == true)
                .Subscribe(_ =>
                {
                    photonView.RPC("ChangeStageColor", PhotonTargets.All, stageColor[0]);
                    isChangeColor = false;
                });

            battleManager.CurrentTime
                .Where(t => t > 10)
                .Subscribe(_ =>
                {
                    if (isChangeColor == false)
                    {
                        photonView.RPC("ChangeStageColor", PhotonTargets.All, stageColor[Random.Range(1, 4)]);
                        isChangeColor = true;
                    }
                });
        }
        
        [PunRPC]
        private void ChangeStageColor(Color color)
        {
            r.sharedMaterial.EnableKeyword("_EMISSION");           
            r.sharedMaterial.SetColor("_EmissionColor", color);
                        
            ballScript.FetchChangedColor(color);
        }
    }
}

