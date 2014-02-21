using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace SpaceRun
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        //State Enum
        public enum State
        {
            Menu,
            Playing,
            Gameover

        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random ();
        public int enemyBulletDamage;
        public Texture2D menuImage;
        public Texture2D gameoverImage;

        // List
        List<Asteroid > asteroidList = new List<Asteroid>() ;
        List<Enemy> enemyList = new List<Enemy>();
        List<Explosion> explosionList = new List<Explosion>();

        //Player & Starfield
        Player P = new Player();
        Starfield sf = new Starfield();
        HUD hud = new HUD();
        SoundManager sm = new SoundManager();

        //Set First State
        State gameState = State.Menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 950;
            graphics.ApplyChanges();
            this.Window.Title = "Space Run";
            Content.RootDirectory = "Content";
            enemyBulletDamage = 10;
            menuImage = null;
            gameoverImage = null;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            P.LoadContent(Content); 
            sf.LoadContent (Content);
            hud.LoadContent(Content);
            sm.LoadContent(Content);
            menuImage = Content.Load<Texture2D>("Menu");
            gameoverImage = Content.Load<Texture2D>("GameOver");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //UPDATING PLAYING STATE
            switch (gameState)
            {
                case State.Playing:
                    {
                        

                        sf.speed = 5;
                        //Updating Enemies & Checking Collsion
                        foreach (Enemy e in enemyList)
                        {
                            //Check if ship colliding with player
                            if (e.boundingBox.Intersects(P.boundingBox))
                            {
                                P.health -= 40;
                                e.isVisable = false;

                            }

                            //Check enemy bullet collision with playship
                            for (int i = 0; i < e.bulletList.Count; i++)
                            {
                                if (P.boundingBox.Intersects(e.bulletList[i].boundingBox))
                                {
                                    P.health -= enemyBulletDamage;
                                    e.bulletList[i].isVisable = false;
                                }

                            }


                            //Check player bullet collision (Enemy)
                            for (int i = 0; i < P.bulletList.Count; i++)
                            {
                                if (P.bulletList[i].boundingBox.Intersects(e.boundingBox))
                                {
                                    sm.explodeSound.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(e.position.X, e.position.Y)));
                                    hud.playerScore += 20;
                                    P.bulletList[i].isVisable = false;
                                    e.isVisable = false;
                                }
                            }
                            e.Update(gameTime);
                            hud.Update(gameTime);
                        }
                        //Update Explsions
                        foreach (Explosion ex in explosionList)
                        {
                            ex.Update(gameTime);
                        }
                        //For each asteroid in our list update, and check for collisions
                        foreach (Asteroid a in asteroidList)
                        {
                            //Check to see if any asteroids are colliding with player if are asteroids are set not visable
                            if (a.boundingBox.Intersects(P.boundingBox))
                            {
                                //
                                P.health -= 20;
                                a.isVisable = false;

                            }

                            //Irerate through bulletlist if any asteroids come in contact with bullets, destroy bullet and asteroid.
                            for (int i = 0; i < P.bulletList.Count; i++)
                            {
                                if (a.boundingBox.Intersects(P.bulletList[i].boundingBox))
                                {
                                    sm.explodeSound.Play();
                                    explosionList.Add(new Explosion(Content.Load<Texture2D>("explosion3"), new Vector2(a.position.X, a.position.Y)));
                                    hud.playerScore += 5;
                                    a.isVisable = false;
                                    P.bulletList.ElementAt(i).isVisable = false;
                                }

                            }
                            a.Update(gameTime);

                        }

                        //hud.Update(gameTime);
                        //If Player health hits 0 then go to gameover state
                        if(P.health <= 0)
                            gameState = State.Gameover;
                        

                        P.Update(gameTime);
                        sf.Update(gameTime);
                        ManageExplosions();
                        LoadAsteroids();
                        LoadEnemies();
                        break;

                    }
                    //Updating Menu
                case State.Menu:
                    {
                        //Get Keyboard Input (Enter)
                        KeyboardState keystate = Keyboard.GetState();

                        if (keystate.IsKeyDown(Keys.Enter))
                        {
                            gameState = State.Playing;
                            MediaPlayer.Play(sm.bgMusic);
                        }
                        sf.Update(gameTime);
                        sf.speed = 1;
                        break;
                    }    
                    //Updating Gameover
                case State.Gameover:
                        {
                            KeyboardState keystate = Keyboard.GetState();

                            if (keystate.IsKeyDown(Keys.Escape))
                            {
                                P.position = new Vector2 (400, 900);
                                enemyList.Clear();
                                asteroidList.Clear();
                                P.bulletList.Clear();
                                explosionList.Clear();
                                hud.playerScore = 0;
                                P.health = 200;
                                gameState = State.Menu;
                            }
                            //Stop Music
                               MediaPlayer.Stop();
                            
                            break;
                        }
                
            }
               
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            switch (gameState)
            {
                    //Drawing PlayingState 
                case State.Playing:
                    {
                        sf.Draw(spriteBatch);
                        P.Draw(spriteBatch);
                        hud.Draw(spriteBatch);

                        foreach (Explosion ex in explosionList)
                        {
                            ex.Draw(spriteBatch);

                        }
                        foreach (Asteroid a in asteroidList)
                        {
                            a.Draw(spriteBatch);
                        }

                        foreach (Enemy e in enemyList)
                        {
                            e.Draw(spriteBatch);
                        }
                        break;

                    }
                    //Drawing Menu State
                case State.Menu:
                    {
                        sf.Draw(spriteBatch);
                        spriteBatch.Draw(menuImage, new Vector2(0, 0), Color.White);

                        break;
                    }
                    //Drawing Gameover State
                case State.Gameover:
                    {
                        spriteBatch.Draw(gameoverImage, new Vector2(0, 0), Color.White);
                        spriteBatch.DrawString(hud.playerScoreFont, "Your Final Score was - " + hud.playerScore.ToString(), new Vector2(235, 450), Color.Red);
                        break;
                    }
            }
            



            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        //Load Asteroids
        public void LoadAsteroids()
        {
            //Creating Random Variables for X and Y axis

            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 750);

            //If there are less then 5 asteroids on screen then create more untill 5 again
            if (asteroidList.Count < 5)
            {
                asteroidList.Add (new Asteroid (Content.Load<Texture2D >("asteroid"), new Vector2 (randX,randY ))); 

            }
            //If any asteroids in list were destroyed or invisable then remove them from list
            for (int i = 0; i < asteroidList.Count; i++)
            {
                if (!asteroidList[i].isVisable)
                {
                    asteroidList.RemoveAt(i);
                    i--;
                }
            }

        }
        //Load Enemies
        public void LoadEnemies()
        {
            //Creating Random Variables for X and Y axis

            int randY = random.Next(-600, -50);
            int randX = random.Next(0, 750);

            //If there are less then 3 enemy on screen then create more untill 3 again
            if (enemyList.Count < 3)
            {
                enemyList .Add(new Enemy (Content.Load<Texture2D>("enemyship"), new Vector2(randX, randY ), Content.Load<Texture2D >("lazer")));

            }

            //If any enemy in list were destroyed or invisable then remove them from list
            for (int i = 0; i < enemyList.Count; i++)
            {
                if (!enemyList[i].isVisable)
                {
                    enemyList.RemoveAt(i);
                    i--;
                }
            }

        }

        //Manage Explosions
        public void ManageExplosions()
        {
            for (int i = 0; i < explosionList.Count; i++)
            {
                if (!explosionList[i].isVisable)
                {
                    explosionList.RemoveAt(i);
                    i--;

                }

            }
        }
        
    }
}
