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
   public class Asteroid
    {
        public Rectangle boundingBox;
        public Texture2D texture;
        public Vector2 position;
        public Vector2 origin;
        public float rotationangle;
        public int speed;

        public bool isVisable;
        Random random = new Random();
        public float randX, randY;

        //Constructor 
        public Asteroid(Texture2D newTexture, Vector2 newPosition)
        {
            texture = newTexture ;
            position = newPosition;
            speed = 4;

            randX = random.Next(0, 750);
            randY = random.Next(-600, -50);
            isVisable = true; 


        }

        //Public Load
        public void LoadContent(ContentManager Content)
        {
            





        }

        //Update 
        public void Update(GameTime gameTime)
        {
            //Set Bounding Box
            boundingBox = new Rectangle((int)position.X, (int)position.Y, 45, 45);

            //Orgin for rotation 
            origin.X = texture.Width / 2;
            origin.Y = texture.Height / 2;
            //Update Movement
            position.Y = position.Y + speed;
            if (position.Y >= 950)
                position.Y = -50;

            //Rotate Asteroid 
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            rotationangle += elapsed;
            float circle = MathHelper.Pi * 2;
            rotationangle = rotationangle % circle;




        }

        //Draw 
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisable )
                spriteBatch.Draw(texture, position, null, Color.White); 
        }
        
    }
}
