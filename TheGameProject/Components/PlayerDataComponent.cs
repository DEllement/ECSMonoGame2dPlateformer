﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TheGameProject.Components
{
    public class PlayerDataComponent
    {
        public int Life { get; set; }
        public bool IsJumping { get; set; }
        public int PlayerId { get; set; }

        public PlayerDataComponent(int playerId, int life=3)
        {
            PlayerId = playerId;
            Life = life;
        }
    }
}
