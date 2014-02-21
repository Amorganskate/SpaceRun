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
    public class Enemy
    {
        public Rectangle boundingBox;
        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int health, speed, bulletDelay, currentDifficultylevel;
        public bool isVisable;
        public List<Bullet> bulletList;


        //Constructor
        public Enemy(Texture2D newTexutre, Vector2 newPosition, Texture2D newbulletTexture)
        {
            bulletList = new List<Bullet>();
            texture = newTexutre;
            bulletTexture = newbulletTexture;
            health = 5;
            position = newPosition;
            currentDifficultylevel = 1;
            bulletDelay = 40;
            speed = 5;
            isVisable = true;

        }

        public void Update(GameTime gameTime)
        {
            //Update Collsion Rectangle
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            //Update enemy movement
            position.Y += speed ;

            //Move enemy back to top of the screen if flies off bottom
            if(position.Y >= 950)
                position.Y = - 75;

            EnemyShoot();
            UpdateBullets(); 

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw enemy
            spriteBatch.Draw(texture, position, Color.White);
            
           //Draw enemy Bullets
           foreach (Bullet b in bulletList)
           {
               b.Draw(spriteBatch );

           }
            

        }
        //Update Bullets function
        public void UpdateBullets()
        {
            //for each bullet in list update movement and if bullet hits top screen remove it from list
            foreach (Bullet b in bulletList)
            {
                //Bounding Box for every bullet in bulletlist
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);
                //Set movement bullet
                b.position.Y = b.position.Y + b.speed;

                if (b.position.Y >= 950)
                    b.isVisable = false;

            }
            for (int i = 0; i < bulletList.Count; i++)
            {
                if (!bulletList[i].isVisable)
                {
                    bulletList.RemoveAt(i);
                    i--;
                }
            }

        }

       // Enemy Shoot Function
        public void EnemyShoot()
        {
            //Shoot only if bulletDelay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            if (bulletDelay <= 0)
            {
                //Create new bullet and position in front center enemy ship
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + texture.Width / 2 - newBullet.texture.Width / 2, position.Y - 30);

                newBullet.isVisable = true;

                if (bulletList.Count() < 20)
                    bulletList.Add(newBullet);
            

            }

            //
            if (bulletDelay == 0)
                bulletDelay = 40;


        }
    }
}
