using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class ChlebISer : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Vector2 textVector;
        Vector2 scoretextVector;
        Vector2 livestextVector;
        Color textcolor;
        Color livescolor;


        struct GameSpriteStruck
        {
            public Texture2D SpriteTexture;
            public Rectangle SpriteRectangle;
            public float X;
            public float Y;
            public float XSpeed;
            public float YSpeed;
            public float WidthFactor;
            public float TicksToCrossScreen;
            public bool Visible;
        }

        Texture2D tomatoTexture;
        GameSpriteStruck[] tomatoes;
        GameSpriteStruck chleb, ser;
        int numberoftomatoes = 10;
        int score = 0;
        int lives = 3;
        int tomatoesDestoied = 0;
        float displayWidth;
        float displayHeight;
        float tomatoHeight;
        float tomatoHeightLimit;
        float tomatoStepFactor;
        
        public ChlebISer()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            displayWidth = GraphicsDevice.Viewport.Width;
            displayHeight = GraphicsDevice.Viewport.Height;

            textVector = new Vector2(displayWidth / 2 , displayHeight / 2);
            scoretextVector = new Vector2(displayWidth - 120,displayHeight - 40);
            livestextVector = new Vector2(20, displayHeight - 40);
            textcolor = new Color(0, 0, 0, 20);
            livescolor = new Color(100, 0, 0, 100);


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

            // TODO: use this.Content to load your game content here

            chleb.SpriteTexture = Content.Load<Texture2D>("Images/chlebe³");
//            papryka.SpriteTexture = Content.Load<Texture2D>("Images/papryke³a");
            tomatoTexture = Content.Load<Texture2D>("Images/pomidore³");
            ser.SpriteTexture = Content.Load<Texture2D>("Images/sere³");
            font = Content.Load<SpriteFont>("font/SpriteFont1");

            //wykonywanie metody podaj¹cej wartoœci pocz¹tkowe
            setupSprites();

        }

        void setupSprite(ref GameSpriteStruck sprite, float widthFactor, float ticksToCrossScreen, float initialX, float initialY, bool initialVisibility)
        {
            sprite.WidthFactor = widthFactor;
            sprite.TicksToCrossScreen = ticksToCrossScreen;
            sprite.SpriteRectangle.Width = (int)((displayWidth * widthFactor) + 0.5f);
            float aspectRaito = (float)sprite.SpriteTexture.Width / sprite.SpriteTexture.Height;
            sprite.SpriteRectangle.Height = (int)((sprite.SpriteRectangle.Width / aspectRaito) + 0.5f);
            sprite.SpriteRectangle.X = (int)initialX;
            sprite.SpriteRectangle.Y = (int)initialY;
            sprite.X = initialX;
            sprite.Y = initialY;
            sprite.XSpeed = displayWidth / ticksToCrossScreen;
            sprite.YSpeed = sprite.XSpeed;
            sprite.Visible = initialVisibility;
        }

        void setupSprites()
        {
            setupSprite(ref chleb, 0.15f, 140.0f, displayWidth / 2, displayHeight - 100, true);
            setupSprite(ref ser, 0.05f, 140.0f, 30, 30, true);

            //przygotowywanie pomidorów oraz limity ich wysokoœci rozmieszczenia
            tomatoHeight = 5;
            tomatoHeightLimit = 5 + (displayHeight / 2);
            tomatoes = new GameSpriteStruck[numberoftomatoes];
            float tomatoSpacing = displayWidth / 10;

            for (int i = 0; i < numberoftomatoes; i++)
            {
                tomatoes[i].SpriteTexture = tomatoTexture;
                setupSprite(ref tomatoes[i], 0.05f, 0, 5 + (i * tomatoSpacing), 5, true);
            }
        }

        public void resetTomatoDisplay()
        {
            tomatoStepFactor = displayHeight / 10;
            tomatoHeight = tomatoHeight +  tomatoStepFactor;

            if (tomatoHeight > tomatoHeightLimit)
            {
                tomatoHeight = 0;
            }

            for (int i = 0; i < numberoftomatoes; i++)
            {
                tomatoes[i].Visible = true;
                tomatoes[i].Y = tomatoHeight;
            }

            tomatoesDestoied = 0;
        }

        public void reset()
        {
            ser.X = 0;
            ser.Y = 30;
            chleb.X = displayWidth / 2;
            chleb.Y = displayHeight - 100;
            chleb.SpriteRectangle.X = (int)chleb.X;
            chleb.SpriteRectangle.Y = (int)chleb.Y;
            score = 0;
            for (int i = 0; i < numberoftomatoes; i++)
            {
                tomatoes[i].Visible = true;
                tomatoes[i].Y = tomatoHeight;
            }
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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            //sprawdzanie zakoñczenia gry
            if (lives <= 0)
            {
                return;
            }

            //poruszanie serem
            ser.X = ser.X + ser.XSpeed;
            ser.Y = ser.Y + ser.YSpeed;
            ser.SpriteRectangle.X = (int)(ser.X);
            ser.SpriteRectangle.Y = (int)(ser.Y);

            //wspó³rzêdne pomidorów
            for (int i = 0; i < numberoftomatoes; i++)
            {

                tomatoes[i].SpriteRectangle.X = (int)tomatoes[i].X;
                tomatoes[i].SpriteRectangle.Y = (int)tomatoes[i].Y;

                if (tomatoes[i].Visible)
                {
                    if (ser.SpriteRectangle.Intersects(tomatoes[i].SpriteRectangle))
                    {
                        tomatoes[i].Visible = false;
                        ser.YSpeed = ser.YSpeed * -1;
                        score += 10;
                        tomatoesDestoied++;
                        break;
                    }
                }

            }

            //zwiêkszanie poziomu po znikniêciu wszystkich pomidorów
            if (tomatoesDestoied == numberoftomatoes)
            {
                resetTomatoDisplay();
            }

            //ograniczenie sera wewn¹trz pola widzenia oraz zmniejszanie iloœci ¿yæ
            if (ser.X + ser.SpriteRectangle.Width <= displayWidth)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }
            if (ser.X >= 0)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }

            if (ser.Y + ser.SpriteRectangle.Height >= displayHeight)
            {
                ser.YSpeed = ser.YSpeed * -1;
                if (lives > 0)
                {
                    lives--;
                }
            }
            if (ser.Y <= 0)
            {
                ser.YSpeed = ser.YSpeed * -1;
            }


            //poruszanie siê chleba 
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                chleb.X = chleb.X + chleb.XSpeed;
                chleb.SpriteRectangle.X = (int)chleb.X;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                chleb.X = chleb.X - chleb.XSpeed;           
                chleb.SpriteRectangle.X = (int)chleb.X;
            }

            //zatrzymanie chleba w ekranie
            if(chleb.X > displayWidth - chleb.SpriteRectangle.Width)
            {
                chleb.X = chleb.X - chleb.XSpeed;
            }
            if (chleb.X < 0)
            {
                chleb.X = chleb.X + chleb.XSpeed;
            }

/*        //kolizja 
            //górna krawêdŸ chleba 
            if (ser.Y + ser.SpriteRectangle.Height > chleb.Y &&
                ser.Y < chleb.Y &&
                ser.X + ser.SpriteRectangle.Width > chleb.X  &&
                ser.X < chleb.X + chleb.SpriteRectangle.Width )
            {
                ser.YSpeed = ser.YSpeed * -1;
            }

            //lewa krawêdŸ chleba
            if (ser.X + ser.SpriteRectangle.Width + ser.XSpeed > chleb.X &&
                ser.X + ser.SpriteRectangle.Width + ser.XSpeed < chleb.X + chleb.SpriteRectangle.Width &&
                ser.Y + ser.YSpeed >= chleb.Y &&
                ser.Y + ser.YSpeed <= chleb.Y + chleb.SpriteRectangle.Height)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }

            // prawa krawêdŸ chleba
            if (ser.X + ser.XSpeed < chleb.X + chleb.SpriteRectangle.Width &&
                ser.X + ser.XSpeed > chleb.X &&
                ser.Y + ser.YSpeed >= chleb.Y &&
                ser.Y + ser.YSpeed <= chleb.Y + chleb.SpriteRectangle.Height)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }
            */

// Rozszerzony wachlarz ruchów

 // Dodatkowe poruszanie chlebem i zatrzymanie go wewn¹trz ekranu
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                chleb.Y = chleb.Y - chleb.XSpeed;
                chleb.SpriteRectangle.Y = (int)chleb.Y;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                chleb.Y = chleb.Y + chleb.YSpeed;
                chleb.SpriteRectangle.Y = (int)chleb.Y;
            }
 
            if (chleb.Y > displayHeight - chleb.SpriteRectangle.Height)
            {
                chleb.Y = chleb.Y - chleb.YSpeed;
            }
            if (chleb.Y < 0)
            {
                chleb.Y = chleb.Y + chleb.YSpeed;
            }

 
 // kolizja
            //górna krawêdŸ chleba 
            if (ser.Y + ser.YSpeed + ser.SpriteRectangle.Height > chleb.Y &&
                ser.Y + ser.YSpeed < chleb.Y &&
                ser.X + ser.XSpeed + ser.SpriteRectangle.Width > chleb.X &&
                ser.X + ser.XSpeed < chleb.X + chleb.SpriteRectangle.Width )
            {
                ser.YSpeed = ser.YSpeed * -1;
            }

            //dolna krawêdŸ chleba
            if (ser.Y + ser.YSpeed < chleb.Y + chleb.SpriteRectangle.Height &&
                ser.Y + ser.YSpeed + ser.SpriteRectangle.Height > chleb.Y + chleb.SpriteRectangle.Height &&
                ser.X + ser.XSpeed + ser.SpriteRectangle.Width > chleb.X &&
                ser.X + ser.XSpeed < chleb.X + chleb.SpriteRectangle.Width)
            {
                ser.YSpeed = ser.YSpeed * -1;
            }

            //lewa krawêdŸ chleba
            if (ser.X + ser.SpriteRectangle.Width + ser.XSpeed > chleb.X &&
                ser.X + ser.SpriteRectangle.Width + ser.XSpeed < chleb.X + chleb.SpriteRectangle.Width &&
                ser.Y + ser.YSpeed >= chleb.Y &&
                ser.Y + ser.YSpeed <= chleb.Y + chleb.SpriteRectangle.Height)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }

            // prawa krawêdŸ chleba
            if (ser.X + ser.XSpeed < chleb.X + chleb.SpriteRectangle.Width &&
                ser.X + ser.XSpeed > chleb.X &&
                ser.Y + ser.YSpeed >= chleb.Y &&
                ser.Y + ser.YSpeed <= chleb.Y + chleb.SpriteRectangle.Height)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }        

            // refresh gry
            if (Keyboard.GetState().IsKeyDown(Keys.F5))
            {
                reset();
            }


            base.Update(gameTime);
          }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //rysowanie chlebka i sera
            spriteBatch.Draw(chleb.SpriteTexture, chleb.SpriteRectangle, Color.White);
            spriteBatch.Draw(ser.SpriteTexture, ser.SpriteRectangle, Color.White);

            //rysowanie widzialnych pomidorów
            for (int i = 0; i < numberoftomatoes; i++) 
            {
                if (tomatoes[i].Visible)
                {
                    spriteBatch.Draw(tomatoes[i].SpriteTexture, tomatoes[i].SpriteRectangle, Color.White);
                }
            }
            
            //rysowanie punktów zdobytych
            string sscore = score.ToString();
            spriteBatch.DrawString(font, "Punkty: " + sscore, scoretextVector, textcolor);
 
            //rysowanie ¿yæ
            string slives = lives.ToString();
            spriteBatch.DrawString(font, "Zycia: "+slives, livestextVector, livescolor);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
