using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace SpaceRun
{
    public class SoundManager
    {
        public SoundEffect playerShootSound;
        public SoundEffect explodeSound;
        public Song bgMusic;

        //Constor
        public SoundManager()
        {
            playerShootSound = null;
            explodeSound = null;
            bgMusic = null;


        }

        public void LoadContent(ContentManager Content)
        {
            playerShootSound = Content.Load<SoundEffect>("playershoot");
            explodeSound = Content.Load<SoundEffect>("explodemini");
            bgMusic = Content.Load<Song>("bgmusic");

        }





    }
}
