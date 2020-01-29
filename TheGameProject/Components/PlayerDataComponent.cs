using System;
using System.Collections.Generic;
using System.Text;

namespace TheGameProject.Components
{
    public class PlayerDataComponent
    {
        public int Life { get; set; }
        public bool allGemCollected { get; set; }
        public bool IsJumping { get; set; }
        public int PlayerId { get; set; }

        public int totalGreenGem { get; set; }
        public int totalRedGem { get; set; }
        public int totalBlueGem { get; set; }
        public int totalGoldGem { get; set; }

        public PlayerDataComponent(int playerId, int life=3)
        {
            PlayerId = playerId;
            Life = life;
        }
        public PlayerDataComponent(int playerId, int life, int TGG, int TRG, int TBG, int TGoG)
        {
            PlayerId = playerId;
            Life = life;
            totalGreenGem = TGG;
            totalRedGem = TRG;
            totalBlueGem = TBG;
            totalGoldGem = TGoG;
            allGemCollected = false;
        }
    }
}
