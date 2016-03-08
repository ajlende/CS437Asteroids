﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CS437
{
    class Scene
    {
        private Camera camera;
        private Skybox skybox;

        private Spaceship playerShip;
        private List<Torpedo> torpedos;

        private List<Asteroid> asteroids;

        public Scene(Game Game)
        {
            camera = new Camera(fieldOfView: MathHelper.PiOver4,
                aspectRatio: Game.GraphicsDevice.Viewport.AspectRatio);

            skybox = new Skybox(Game.Content, "Textures/Skybox");

            torpedos = new List<Torpedo>();

            asteroids = new List<Asteroid>();
            asteroids.Add(new Asteroid(Game.Content, new Vector3(0f, 0f, 20f), Asteroid.Size.SMALL));
            initializeAsteroids(Game.Content);

            playerShip = new Spaceship(Game.Content,
                Position: new Vector3(0.0f, 0.0f, 0.0f),
                CameraOffset: new Vector3(0.0f, 300f, 600f),
                scale: 0.25f,
                fireTorpedo: () =>
                {
                    var torpedo = new Torpedo(Game, Vector3.Zero);
                    torpedos.Add(torpedo);
                    Console.WriteLine("BOOM! <" + torpedos.Count + ">");
                    return torpedo;
                });
        }

        private void initializeAsteroids(ContentManager Content)
        {
            float spaceBetween = 500f;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        asteroids.Add(new Asteroid(Content, new Vector3(i * spaceBetween, j * spaceBetween, k * spaceBetween), Asteroid.Size.MEDIUM));
                    }
                }
            }
        }

        public void Update(GameTime gameTime, GameInput input)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            playerShip.Update(gameTime, input);

            foreach (var asteroid in asteroids)
                asteroid.Update(gameTime);

            foreach (var torpedo in torpedos)
                torpedo.Update(gameTime);

            camera.Update(playerShip);
        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            ////////////////////////// SkyBox //////////////////////////
            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            skybox.Draw(camera.View, camera.Projection, camera.Position);

            graphics.GraphicsDevice.RasterizerState = originalRasterizerState;

            ////////////////////////// Asteroids //////////////////////////
            foreach (var asteroid in asteroids)
                asteroid.Draw(camera);

            ////////////////////////// Torpedos //////////////////////////
            foreach (var torpedo in torpedos)
                torpedo.Draw(camera);

            ////////////////////////// Ship //////////////////////////
            playerShip.Draw(camera);

        }
    }
}
