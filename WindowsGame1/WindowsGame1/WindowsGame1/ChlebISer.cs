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

        float displayWidth;
        float displayHeight;
        GameSpriteStruck chleb, pomidor, papryka, ser;
        
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
            papryka.SpriteTexture = Content.Load<Texture2D>("Images/papryke³a");
            pomidor.SpriteTexture = Content.Load<Texture2D>("Images/pomidore³");
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
            sprite.X = initialX;
            sprite.Y = initialY;
            sprite.XSpeed = displayWidth / ticksToCrossScreen;
            sprite.YSpeed = sprite.XSpeed;

        }

        void setupSprites()
        {
            setupSprite(ref chleb, 0.15f, 120.0f, displayWidth / 2, displayHeight / 2);
            setupSprite(ref ser, 0.05f, 200.0f, 0, 0);
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

            //poruszanie sera
            ser.X = ser.X + ser.XSpeed;
            ser.Y = ser.Y + ser.YSpeed;
            ser.SpriteRectangle.X = (int)(ser.X + 0.5f);
            ser.SpriteRectangle.Y = (int)(ser.Y + 0.5f);
            if (ser.X + ser.SpriteRectangle.Width >= displayWidth)
            {
                ser.XSpeed = ser.XSpeed * -1;
            }
            if (ser.X >= 0)
            {
                ser.XSpeed = ser.XSpeed * -1;
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
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
