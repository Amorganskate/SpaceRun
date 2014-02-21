using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace SpaceRun
{
    //Main
    public class Player
    {

        public Texture2D texture, bulletTexture;
        public Vector2 position;
        public int speed, health;
        public float bulletDelay; 
        //Collision Vars 
        public Rectangle boundingBox;
        public bool isColliding;
        public List<Bullet> bulletList;
        SoundManager sm = new SoundManager();
        public static Player player;

        public Player()
        {
            bulletList = new List<Bullet>();
            texture = null;
            position = new Vector2(400 , 800);
            player = this;

            speed = 10;
            isColliding = false;
            bulletDelay = 1;
            health = 200;
            



        }

        //Load Content 
        public void LoadContent(ContentManager Content)
        {

            texture = Content.Load<Texture2D>("spaceship2");
            bulletTexture = Content.Load<Texture2D>("playerbullet");
            
            sm.LoadContent(Content);

        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
             spriteBatch.Draw(texture, position, Color.White);
             

             foreach (Bullet b in bulletList)
                 b.Draw(spriteBatch);

        }

        //Update
        public void Update(GameTime gameTime)
        {
            //Getting Keyboard State 
            KeyboardState keyState = Keyboard.GetState();

            //BoundingBox for playership
            boundingBox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); 

            //Set bounding box for health bar
            
            //Fire Bullets
            if (keyState.IsKeyDown(Keys.Space))
            {
                Shoot();

            }
            UpdateBullets(); 
            //Ship Controls
            if(keyState.IsKeyDown(Keys.W))
                position.Y = position.Y - speed;

            if (keyState.IsKeyDown(Keys.A))
                position.X = position.X - speed;

            if (keyState.IsKeyDown(Keys.S))
                position.Y = position.Y + speed;

            if (keyState.IsKeyDown(Keys.D))
                position.X = position.X + speed;

            //Keep Player Ship In Screen Bounds
            if(position.X  <= 0)
                position.X = 0;
            if (position.X >= 800 - texture.Width)
                position.X = 800 - texture.Width;
            if (position.Y <= 0) 
                position.Y = 0;
            if (position.Y >= 950 - texture.Height)
                position.Y = 950 - texture.Height;

        }

        //Shoot Method: used to set starting spot for bullets
        public void Shoot()
        {
            //Shoot only if bullet delay resets
            if (bulletDelay >= 0)
                bulletDelay--;

            //If bulletDelay is at 0 then create new bullet player position make visable. Then add bullet to list
            if (bulletDelay <= 0)
            {
                sm.playerShootSound.Play();
                Bullet newBullet = new Bullet(bulletTexture);
                newBullet.position = new Vector2(position.X + 75 - newBullet.texture.Width / 2, position.Y + 54);

                //making bullet Visable
                newBullet.isVisable = true;

                if (bulletList.Count < 20)
                    bulletList.Add(newBullet);

            }

            //Reset bullet Delay
            if (bulletDelay == 0)
                bulletDelay = 10; 


        }

        //Update Bullets function
        public void UpdateBullets()
        {
            //for each bullet in list update movement and if bullet hits top screen remove it from list
            foreach (Bullet b in bulletList)
            {
                //Bounding Box for every bullet in bulletlist
                b.boundingBox = new Rectangle((int)b.position.X, (int)b.position.Y, b.texture.Width, b.texture.Height);
                
                b.position.Y  = b.position.Y - b.speed;

                if (b.position.Y <= 0)
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
    }
       
}

