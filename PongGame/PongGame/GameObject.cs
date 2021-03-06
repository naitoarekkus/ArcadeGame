﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame
{
    abstract class GameObject
    {
        // Fields
        protected Texture2D texture;
        protected Texture2D boxTexture;
        protected Vector2 position = Vector2.Zero;
        protected Vector2 origin = Vector2.Zero;
        protected Vector2 velocity;
        protected float speed = 200;
        private Rectangle[] rectangles;
        private Vector2 offset;
        private int currentIndex;
        private float rotation = 0;
        private float timeElapsed;
        private float fps = 30;
        protected float layer = 0.0f;
        private float scale = 1f;
        private Color color = Color.White;


        private SpriteEffects effects = new SpriteEffects();
        private Dictionary<string, Animation> animations = new Dictionary<string, Animation>();

        // Properties
        public Rectangle CollisionRect
        {
            get
            {
                return new Rectangle
                (
                    (int)(position.X + offset.X),
                    (int)(position.Y + offset.Y),
                    rectangles[0].Width, rectangles[0].Height
                );
            }
        }
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        // Constructor
        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        // Methods
        public virtual void LoadContent(ContentManager content)
        {
            boxTexture = content.Load<Texture2D>(@"CollisionTexture");
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, rectangles[currentIndex], color, rotation, origin, scale, effects, layer);
#if DEBUG
            Rectangle topLine = new Rectangle(CollisionRect.X, CollisionRect.Y, CollisionRect.Width, 1);
            Rectangle rightLine = new Rectangle(CollisionRect.X + CollisionRect.Width, CollisionRect.Y, 1, CollisionRect.Height);
            Rectangle bottomLine = new Rectangle(CollisionRect.X, CollisionRect.Y + CollisionRect.Height, CollisionRect.Width, 1);
            Rectangle leftLine = new Rectangle(CollisionRect.X, CollisionRect.Y, 1, CollisionRect.Height);

            spriteBatch.Draw(boxTexture, topLine, Color.Red);
            spriteBatch.Draw(boxTexture, rightLine, Color.Red);
            spriteBatch.Draw(boxTexture, bottomLine, Color.Red);
            spriteBatch.Draw(boxTexture, leftLine, Color.Red);
#endif
        }
        public virtual void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            currentIndex = (int)(timeElapsed * fps);

            if (currentIndex > rectangles.Length - 1)
            {
                timeElapsed = 0;
                currentIndex = 0;
            }

            HandleCollision();
        }
        protected void CreateAnimation(string name, int frames, int yPos, int xStartFrame, int width, int height, Vector2 offset, float fps)
        {
            if (!animations.ContainsKey(name))
                animations.Add(name, new Animation(frames, yPos, xStartFrame, width, height, offset, fps));
        }
        public void PlayAnimation(string name)
        {
            rectangles = animations[name].Rectangle;
            fps = animations[name].Fps;
            offset = animations[name].Offset * scale;
        }        
        private void HandleCollision()
        {
            foreach (GameObject go in GameWorld.Objects)
            {
                if (go != this)
                {
                    if (go.CollisionRect.Intersects(this.CollisionRect))
                    {
                        OnCollision(go);
                    }
                    else if ((!go.CollisionRect.Intersects(this.CollisionRect)))
                    {
                        ExitCollision(go);
                    }
                }
            }
        }
        public abstract void OnCollision(GameObject other);
        public abstract void ExitCollision(GameObject other);
    }
}
