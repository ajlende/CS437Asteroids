using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CS437
{
    class Scene
    {
        private Camera _camera;
        private Skybox _skybox;

        private Spaceship _playerShip;
        private List<Torpedo> _torpedos;

        private List<Asteroid> _asteroids;

        public Scene(Game Game)
        {
            _camera = new Camera(fieldOfView: MathHelper.PiOver4,
                aspectRatio: Game.GraphicsDevice.Viewport.AspectRatio);

            _skybox = new Skybox(Game.Content, "Textures/Skybox");

            _torpedos = new List<Torpedo>();

            _asteroids = new List<Asteroid>();
            _asteroids.Add(new Asteroid(Game.Content, new Vector3(0f, 0f, 20f), Asteroid.AsteroidSize.SMALL));
            initializeAsteroids(Game.Content);

            _playerShip = new Spaceship(Game.Content,
                position: new Vector3(0.0f, 0.0f, 0.0f),
                cameraOffset: new Vector3(0.0f, 150f, 300f),
                scale: 0.125f,
                fireTorpedo: () =>
                {
                    var torpedo = new Torpedo(Game.Content, _playerShip.Position, -_playerShip.Forward * 1000f);
                    _torpedos.Add(torpedo);
                    Console.WriteLine("BOOM! <" + _torpedos.Count + ">");
                    return torpedo;
                });
        }

        private void initializeAsteroids(ContentManager Content)
        {
            Array values = Enum.GetValues(typeof(Asteroid.AsteroidSize));
            Random random = new Random();
            float spaceBetween = 500f;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        Asteroid.AsteroidSize randomSize = (Asteroid.AsteroidSize)values.GetValue(random.Next(values.Length));
                        Asteroid newAsteroid = new Asteroid(Content, new Vector3(i * spaceBetween, j * spaceBetween, k * spaceBetween), randomSize);
                        _asteroids.Add(newAsteroid);
                    }
                }
            }
        }

        public void Update(GameTime gameTime, GameInput input)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _playerShip.Update(gameTime, input);

            foreach (var asteroid in _asteroids)
                asteroid.Update(gameTime);

            foreach (var torpedo in _torpedos)
                torpedo.Update(gameTime);

            _camera.Update(_playerShip);
        }

        public void Draw(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            ////////////////////////// SkyBox //////////////////////////
            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            _skybox.Draw(_camera.View, _camera.Projection, _camera.Position);

            graphics.GraphicsDevice.RasterizerState = originalRasterizerState;

            ////////////////////////// Asteroids //////////////////////////
            foreach (var asteroid in _asteroids)
                asteroid.Draw(_camera);

            ////////////////////////// Torpedos //////////////////////////
            foreach (var torpedo in _torpedos)
                torpedo.Draw(_camera);

            ////////////////////////// Ship //////////////////////////
            _playerShip.Draw(_camera);

        }
    }
}
