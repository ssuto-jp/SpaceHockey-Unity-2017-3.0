using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceHockey.Players
{
    public class PlayerId : Photon.MonoBehaviour
    {
        public static PlayerId Instance;
        public int Player_Id { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void OnInitialize()
        {
            Player_Id = PhotonNetwork.player.ID;
        }
    }
}