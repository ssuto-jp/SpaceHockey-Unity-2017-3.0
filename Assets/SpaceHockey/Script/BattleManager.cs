using System.Collections;
using System.Collections.Generic;
using SpaceHockey.Balls;
using UniRx;
using UnityEngine;

namespace SpaceHockey.GameManagers
{
    public class BattleManager : Photon.MonoBehaviour
    {
        private BallCore ballCore;
        public GameObject ball;
        [SerializeField] private GameObject respawnPoint;
        
        private BoolReactiveProperty IsBallSet = new BoolReactiveProperty();
             
        private void Start()
        {           
            ballCore = ball.GetComponent<BallCore>();
                     
            IsBallSet
                .Where(b => b == true)
                .Subscribe(_ => ballCore.ShootBall());           
        }
        
        public void StartBattle()
        {          
            IsBallSet.Value = true;
        }
            
        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(IsBallSet.Value);
            }
            else
            {
                IsBallSet.Value = (bool)stream.ReceiveNext();
            }
        }
    }
}

