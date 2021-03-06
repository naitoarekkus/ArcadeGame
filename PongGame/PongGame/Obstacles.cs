﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame
{
    class Obstacles : GameObject
    {
        // Fields
        private bool isMiddleLine = false;

        // Properties
        public bool IsMiddleLine
        {
            get { return isMiddleLine; }
        }

        // Constructor
        public Obstacles(Vector2 position, bool isMiddleLine)
            : base(position)
        {
            this.isMiddleLine = isMiddleLine;
            this.Position = position;
            this.layer = 0.0f;
        }

        // Methods
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"white");

            CreateAnimation("IdleBorder", 1, 0, 1, 1000, 20, new Vector2(0, 0), 1);
            CreateAnimation("IdleMiddleLine", 1, 0, 1, 10, 25, new Vector2(0, 0), 1);

            if (!isMiddleLine)
            {
                PlayAnimation("IdleBorder");
            }                
            else if (isMiddleLine)
            {
                PlayAnimation("IdleMiddleLine");
            }                

            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void OnCollision(GameObject other)
        {

        }

        public override void ExitCollision(GameObject other)
        {
        }

        public void HandlePickUp(PickUp pickUp)
        {
            if (pickUp.PickUpPowerUp == PickUpType.ColorChange)
            {
                this.Color = new Color(
                         (byte)RandomPicker.Rnd.Next(0, 255),
                         (byte)RandomPicker.Rnd.Next(0, 255),
                         (byte)RandomPicker.Rnd.Next(0, 255));
            }
        }
    }
}
