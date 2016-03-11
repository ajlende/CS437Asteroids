using BulletSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CS437
{
    /// <summary>
    /// Measurments are in SI Units (meters, Newtons)
    /// </summary>
    class Scene
    {
        private Camera _camera;
        private Skybox _skybox;

        private Spaceship _playerShip;
        private List<Torpedo> _torpedos;

        private List<Asteroid> _asteroids;

        private GamePhysics _physics;

        private Func<string, Texture2D> _textureLoader;
        private Func<string, Model> _modelLoader;

        private BasicEffect _basicEffect;
        private GamePhysics.PhysicsDebugDraw DebugDrawer;

        public Scene(Game Game)
        {
            ////// DEBUG DRAW
            _basicEffect = new BasicEffect(Game.GraphicsDevice);
            DebugDrawer = new GamePhysics.PhysicsDebugDraw(Game.GraphicsDevice, _basicEffect);

            _textureLoader = (s) => Game.Content.Load<Texture2D>(s);
            _modelLoader = (s) => Game.Content.Load<Model>(s);

            _physics = new GamePhysics();

            _camera = new Camera(fieldOfView: MathHelper.PiOver4,
                aspectRatio: Game.GraphicsDevice.Viewport.AspectRatio);

            _skybox = new Skybox(Game.Content, "Textures/Skybox");

            _torpedos = new List<Torpedo>();

            _asteroids = new List<Asteroid>();
            initializeAsteroids(Game.Content);

            DynamicsWorld dynamicsWorld = _physics.DynamicsWorld;

            _playerShip = new Spaceship(dynamicsWorld,
                _modelLoader,
                _textureLoader,
                cameraOffset: new Vector3(0.0f, 5f, 50f),
                cameraRotation: Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.Pi / 64f),
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
            float spaceBetween = 50f;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    for (int k = 0; k < 5; k++)
                    {
                        Asteroid.AsteroidSize randomSize = (Asteroid.AsteroidSize)values.GetValue(random.Next(values.Length));
                        // var randomSize = Asteroid.AsteroidSize.SMALL;

                        Asteroid newAsteroid = new Asteroid(_physics.DynamicsWorld,
                            _modelLoader,
                            _textureLoader,
                            new Vector3(i * spaceBetween, j * spaceBetween, k * spaceBetween), randomSize);
                        newAsteroid.Body.Activate();
                        newAsteroid.Body.AngularVelocity += new Vector3(random.Next(1200) / 4f, random.Next(1200) / 4f, random.Next(1200) / 4f);
                        // newAsteroid.Body.LinearVelocity = new Vector3(random.Next(120) / 4f, random.Next(120) / 4f, random.Next(120) / 4f);
                        _asteroids.Add(newAsteroid);
                    }
                }
            }
        }

        public void Update(GameTime gameTime, GameInput input)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _playerShip.Update(gameTime, input);

            foreach (var asteroid in _asteroids)
                asteroid.Update(gameTime);

            foreach (var torpedo in _torpedos)
                torpedo.Update(gameTime);

            _physics.Update(time);

            _camera.Update(_playerShip);

            DebugDrawer.DebugMode = DebugDrawModes.DrawAabb;
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

            ////// DEBUG DRAW
            _basicEffect.View = _camera.View;
            _basicEffect.VertexColorEnabled = true;
            _basicEffect.LightingEnabled = false;
            _basicEffect.World = Matrix.Identity;
            _basicEffect.CurrentTechnique.Passes[0].Apply();
            DebugDrawer.DrawDebugWorld(_physics.DynamicsWorld);
        }
    }
}
