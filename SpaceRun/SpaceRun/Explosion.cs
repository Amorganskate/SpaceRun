using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceRun
{
    public class Explosion
    {
        public Texture2D texture;
        public Vector2 position;
        public float timer;
        public float interval;
        public Vector2 origin;
        public int currentFrame,spriteWidth,spriteHeight;
        public Rectangle sourceRect;
        public bool isVisable;


        //Constructor
        public Explosion(Texture2D newTexture, Vector2 newPosition)
        {
            position = newPosition;
            texture = newTexture;
            timer = 0f;
            interval = 20f;
            currentFrame = 1;
            spriteWidth = 128;
            spriteHeight = 128;
            isVisable = true;
        }

        public void LoadContent(ContentManager Content)
        {

        }

        public void Update(GameTime gameTime)
        {
            //Increase timer by number of milseconds since update last called
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            //Check the timer is more then chosen interval
            if (timer > interval)
            {
                //show next frame
                currentFrame++;
                //reset timer
                timer = 0f;
            }

            //If last frame , make explision invisable and reset currentFrame to begin spritesheet
            if (currentFrame == 17)
            {
                isVisable = false;
                currentFrame = 0;
            }
            sourceRect = new Rectangle(currentFrame * spriteWidth, 0, spriteWidth, spriteHeight);
            origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);

                
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            //If visable then draw
            if (isVisable == true)
                spriteBatch.Draw(texture, position, sourceRect, Color.White, 0f,  origin, 1.0f, SpriteEffects.None, 0);


        }
    }
}
