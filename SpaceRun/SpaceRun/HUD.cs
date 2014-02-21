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
    public class HUD
    {
        public Rectangle healthRectangle;
        public Texture2D healthTexture;
        public int playerScore, healthbarHeight,HealthbarWidth, health;
        public SpriteFont playerScoreFont;
        public Vector2 playerScrorePos, healthbarPosition;
        public bool showHud;

        //Constructor
        public HUD()
        {
            playerScore = 0;
            showHud = true;
            healthbarHeight = 25;
            
            playerScoreFont = null;
            playerScrorePos = new Vector2(800 / 2, 50);
            healthbarPosition = new Vector2(50, 50);
            health = 200;
            healthTexture = null;
        }

        //Load Content
        public void LoadContent(ContentManager Content)
        {
            playerScoreFont = Content.Load<SpriteFont>("MyFont");
            healthTexture = Content.Load<Texture2D>("healthbar");
        }

        public void Update(GameTime gameTime)
        {
            //Get Keystate 
            KeyboardState keystate = Keyboard.GetState();

            healthRectangle = new Rectangle((int)healthbarPosition.X, (int)healthbarPosition.Y, Player.player.health,healthbarHeight  );
        }

        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            //If we are showing HUD then display HUD
            if (showHud)
                spriteBatch.DrawString(playerScoreFont, "Score - " + playerScore , playerScrorePos, Color.Red);

            spriteBatch.Draw(healthTexture, healthRectangle, Color.White); 

        }


    }
}
