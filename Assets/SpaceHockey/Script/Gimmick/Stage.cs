using System.Collections.Generic;
using SpaceHockey.GameManagers;
using UniRx;
using UnityEngine;

namespace SpaceHockey.Gimmicks
{
    public class Stage : MonoBehaviour
    {
        private Material m;
        private BattleManager battleManager;
        private PhotonView photonView;
        public GameObject wallPrefab;
        public GameObject gameManager;
        private bool isChangeColor;
        private readonly List<Color> stageColor = new List<Color>() { Color.white, Color.red, Color.blue, Color.yellow };

        private void Awake()
        {
            ColorSerializer.Register();
        }

        private void Start()
        {
            battleManager = gameManager.GetComponent<BattleManager>();
            photonView = GetComponent<PhotonView>();
            m = wallPrefab.GetComponent<Renderer>().sharedMaterial;

            battleManager.IsBallSet
                .Where(b => b == true)
                .Subscribe(_ =>
                {
                    photonView.RPC("ChangeColor", PhotonTargets.All, stageColor[0]);
                    isChangeColor = false;
                });

            battleManager.CurrentTime
                .Where(t => t > 10)
                .Subscribe(_ =>
                {
                    if (isChangeColor == false)
                    {
                        photonView.RPC("ChangeColor", PhotonTargets.All, stageColor[Random.Range(1, 4)]);
                        isChangeColor = true;
                    }
                });
        }

        [PunRPC]
        private void ChangeColor(Color color)
        {
            m.EnableKeyword("_EMISSION");
            m.SetColor("_EmissionColor", color);
        }
    }
}

