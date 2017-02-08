﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public class Collectable : GameObject
    {
        private const int score = 1;
        private const float AnimationLength = 0.4f;
        private const float AnimationDecrement = 0.05f;

        private Point Position;
        private static readonly Point Size = new Point(32, 32);

        private int _currentFrame;
        private float _timer;
        private List<Rectangle> Animation { get; } = new List<Rectangle>
        {
            new Rectangle(0, 0, 32, 32),
            new Rectangle(32, 0, 32, 32),
            new Rectangle(64, 0, 32, 32),
            new Rectangle(96, 0, 32, 32)
        };

        public static Texture2D CollectableSheet { get; set; }
        public Rectangle Bounds => new Rectangle(Position.X, Position.Y, Size.X, Size.Y);
        public Color Color { get; set; } = Color.Purple;

        public Collectable(Vector2 position)
        {
            const float scale = Game1.BackBufferWidth / (float)20;
            Position = new Point((int)(position.X * scale), (int)(position.Y * scale));
        }

        public override void Update(List<GameObject> objects)
        {
            objects.OfType<Player>().ToList().ForEach(player =>
            {
                if (player.Bounds.Intersects(Bounds))
                {
                    Game1.Instance.IncrementScore(1);
                    CollectableManager.Instance.CollectableCollected();
                    Destroy();
                }
            });

            if (_currentFrame >= Animation.Count - 1)
                _currentFrame = 0;

            if (_timer <= 0)
                _currentFrame++;

            if (_timer <= 0)
                _timer = AnimationLength;
            else
                _timer -= AnimationDecrement;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(CollectableSheet, Bounds, Animation[_currentFrame], Color);
        }

        public Collectable Clone()
        {
            return this.MemberwiseClone() as Collectable;
        }
    }
}