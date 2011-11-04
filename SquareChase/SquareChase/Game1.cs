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

namespace SquareChase
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        
        // Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        Random rand = new Random();
        Texture2D squareTexture;
        Rectangle currentSquare;
        int playerScore = 0;
        int height = 25;
        int width = 25;
        float timeRemaining = 0.0f;
        const float TimePerSquare = 0.75f;
        Color[] colors = new Color [3] { Color.Red, Color.Green, Color.Blue };


        KeyboardState keys;
        GamePadState pad1;
        MouseState mouse;


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Shows the mouse
            this.IsMouseVisible = true;

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

            squareTexture = Content.Load<Texture2D>("Sprites/Square");

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
            // Custom
            keys = Keyboard.GetState();
            pad1 = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (pad1.Buttons.Back == ButtonState.Pressed || keys.IsKeyDown(Keys.Escape))
                this.Exit();


            if (timeRemaining == 0.0f)
            {
                currentSquare = new Rectangle(
                    rand.Next(0, this.Window.ClientBounds.Width - (height / 2)),
                    rand.Next(0, this.Window.ClientBounds.Height - (width / 2)),
                    width, height);
                 timeRemaining = TimePerSquare;
            }

            mouse = Mouse.GetState();

            if ((mouse.LeftButton == ButtonState.Pressed) &&
                (currentSquare.Contains(mouse.X, mouse.Y)))
            {
                playerScore++;
                timeRemaining = 0.0f;
                // Add on that has a chance to shrink the cube when you catch it, then it esplodes it. 
                int shrink = rand.Next(1, 10);
                if (shrink == 3 && currentSquare.Height >= 10 && currentSquare.Width >= 10)
                {
                    height -= shrink;
                    width -= shrink;
                }
                if (currentSquare.Height <= 10 && currentSquare.Width <= 10)
                {
                    int temp = rand.Next(200, 480);
                    height = temp;
                    width = temp;
                    playerScore += 100;
                }
            }
            // Shrinks cube back down
            if (currentSquare.Height > 25 && currentSquare.Width > 25)
            {
                // Provides zoom effect
                currentSquare.Height--;
                currentSquare.Width--;
                //TODO: Fix this Work around
                 height--;
                 width--;
            }



            timeRemaining = MathHelper.Max(0, timeRemaining -
                (float)gameTime.ElapsedGameTime.TotalSeconds);
            this.Window.Title = "Score : " + playerScore.ToString();











            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);

            spriteBatch.Begin();
            spriteBatch.Draw(
                squareTexture,
                currentSquare,
                colors[playerScore % 3]);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
