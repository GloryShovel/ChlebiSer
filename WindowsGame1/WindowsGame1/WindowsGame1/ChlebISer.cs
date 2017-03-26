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
        }

        
        
            Texture2D tomatoTexture;
            GameSpriteStruck[] tomatoes;
            int numberoftomatoes = 30;
        

        float displayWidth;
        float displayHeight;
//        float saveSpeed;
        GameSpriteStruck chleb, ser;
        
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

            //wykonywanie metody podaj¹cej wartoœci pocz¹tkowe
            setupSprites();

        }

        void setupSprite(ref GameSpriteStruck sprite, float widthFactor, float ticksToCrossScreen, float initialX, float initialY)
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
        }

        void setupSprites()
        {
            setupSprite(ref chleb, 0.15f, 120.0f, displayWidth / 2, displayHeight - 100);
            setupSprite(ref ser, 0.05f, 200.0f, 0, 0);
            tomatoes = new GameSpriteStruck[numberoftomatoes];

            int tomatoesinrow = numberoftomatoes / 3;
            float tomatoSpacing = displayWidth / tomatoesinrow;  //(0 - 0) / numberoftomatoes;

            for (int i = 0; i < numberoftomatoes; i++)
            {
                tomatoes[i].SpriteTexture = tomatoTexture;

                if (i < tomatoesinrow)
                {
                    setupSprite(ref tomatoes[i], 0.05f, 0, 5 + (i * tomatoSpacing), 0);
                }
                else if (i > tomatoesinrow && i <= tomatoesinrow * 2)
                {
                    setupSprite(ref tomatoes[i], 0.05f, 0, 0 + (i * tomatoSpacing), 40);
                }
                else 
                {
                    setupSprite(ref tomatoes[i], 0.05f, 0, 0 + (i * tomatoSpacing), 80);
                }
            }
        }

        public void reset()
        {
            ser.X = 0;
            ser.Y = 0;
            chleb.X = displayWidth / 2;
            chleb.Y = displayHeight - 100;
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

            //poruszanie serem
            ser.X = ser.X + ser.XSpeed;
            ser.Y = ser.Y + ser.YSpeed;
            ser.SpriteRectangle.X = (int)(ser.X + 0.5f);
            ser.SpriteRectangle.Y = (int)(ser.Y + 0.5f);
            
            //wspó³rzêdne pomidorów
            for (int i = 0; i < numberoftomatoes; i++)
            {
                tomatoes[i].SpriteRectangle.X = (int)tomatoes[i].X;
                tomatoes[i].SpriteRectangle.Y = (int)tomatoes[i].Y;
            }

                //ograniczenie sera wewn¹trz pola widzenia
                if (ser.X + ser.SpriteRectangle.Width <= displayWidth)
                {
                    ser.XSpeed = ser.XSpeed * -1;
                }
            if (ser.X >= 0)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }

            if (ser.Y + ser.SpriteRectangle.Height <= displayHeight)
            {
                ser.YSpeed = ser.YSpeed * -1;
            }
            if (ser.Y >= 0)
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

            //Dodatkowe poruszanie chlebem
            /*if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                chleb.Y = chleb.Y - chleb.XSpeed;
                chleb.SpriteRectangle.Y = (int)chleb.Y;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                chleb.Y = chleb.Y + chleb.YSpeed;
                chleb.SpriteRectangle.Y = (int)chleb.Y;
            }*/

            //zatrzymanie chleba w ekranie
            if(chleb.X > displayWidth - chleb.SpriteRectangle.Width)
            {
                chleb.X = chleb.X - chleb.XSpeed;
            }
            if (chleb.X < 0)
            {
                chleb.X = chleb.X + chleb.XSpeed;
            }
            
            //Je¿eli w³¹czysz poruszanie góra i dó³
            if (chleb.Y > displayHeight - chleb.SpriteRectangle.Height)
            {
                chleb.Y = chleb.Y - chleb.YSpeed;
            }
            if (chleb.Y < 0)
            {
                chleb.Y = chleb.Y + chleb.YSpeed;
            }


            //kolizja

            //prawa krawêdŸ chleba
            if (ser.X + ser.SpriteRectangle.Width > chleb.X && 
                ser.X + ser.SpriteRectangle.Width < chleb.X + chleb.SpriteRectangle.Width && 
                ser.Y >= chleb.Y && 
                ser.Y <= chleb.Y + chleb.SpriteRectangle.Height)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }

            //lewa krawêdŸ chleba
            if (ser.X < chleb.X + chleb.SpriteRectangle.Width && 
                ser.X > chleb.X &&
                ser.Y >= chleb.Y &&
                ser.Y <= chleb.Y + chleb.SpriteRectangle.Height)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }

            //górna krawêdŸ chleba
            if (ser.Y + ser.SpriteRectangle.Height > chleb.Y && 
                ser.Y < chleb.Y && 
                ser.X + ser.SpriteRectangle.Width > chleb.X && 
                ser.X < chleb.X + chleb.SpriteRectangle.Width)
            {
                ser.YSpeed = ser.YSpeed * -1;
            }

            //dolna krawêdŸ chleba
            if (ser.Y < chleb.Y + chleb.SpriteRectangle.Height && 
                ser.Y + ser.SpriteRectangle.Height > chleb.Y + chleb.SpriteRectangle.Height && 
                ser.X + ser.SpriteRectangle.Width > chleb.X && 
                ser.X < chleb.X + chleb.SpriteRectangle.Width)
            {
                ser.YSpeed = ser.YSpeed * -1;
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

            spriteBatch.Draw(chleb.SpriteTexture, chleb.SpriteRectangle, Color.White);
            spriteBatch.Draw(ser.SpriteTexture, ser.SpriteRectangle, Color.White);
            for (int i = 0; i < numberoftomatoes; i++) {
                spriteBatch.Draw(tomatoes[i].SpriteTexture,
                tomatoes[i].SpriteRectangle, Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
