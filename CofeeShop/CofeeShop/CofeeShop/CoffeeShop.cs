//Author: William Pu
//Created Date: 2015/12/7
//Modified Date: 2015/12/14
//File Name: CoffeeShop.cs
//Project Name: A5_Simulation
//Description: This class is responsible resoponsible for aspects of the simulation.
//             this is odne by having an instance the shop sim class which does all the 
//             logic and drawing
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

namespace CofeeShop
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class CoffeeShop : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //the coffee shop
        private ShopSim coffeeShop; 

        //window's size
        private const int WINDOWH = 600;
        private const int WINDOWW = 800;

        //determiens if the simulation is paused
        private bool isPaused = false;

        //keyboard inputs
        KeyboardState keyBoard;
        KeyboardState keyBoard2;



        public CoffeeShop()
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
            graphics.SynchronizeWithVerticalRetrace = false;

            //apply changes to the window
            graphics.PreferredBackBufferHeight = WINDOWH;
            graphics.PreferredBackBufferWidth = WINDOWW;

            //applying graphical changes
            this.graphics.ApplyChanges();

            //making it os that mouse is visible
            IsMouseVisible = true;

            //initializing the keyboard 2
            keyBoard2 = Keyboard.GetState();

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

            //initializig the shop
            coffeeShop = new ShopSim(Content);

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
            //updating keyboard input
            keyBoard = Keyboard.GetState();

            //if the user presses the pause button
            if (keyBoard.IsKeyDown(Keys.Space) && keyBoard2.IsKeyUp(Keys.Space)) 
            {
                //if puased then un pause and vice versa
                isPaused = !isPaused;
            }

            //if the program is onot paused
            if (!isPaused)
            {
                //uupdting the cofee shop
                coffeeShop.UpdateShop(gameTime);
            }

            //updating keyboard input
            keyBoard2 = Keyboard.GetState();

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //drawing the shop
            coffeeShop.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
