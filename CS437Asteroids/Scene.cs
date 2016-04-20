using BulletSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CS437
{
    /// <summary>
    /// Measurments are in SI Units (meters, killograms, Newtons)
    /// </summary>
    class Scene
    {
        private Camera _camera;
        private Skybox _skybox;

        private const float _worldRadius = 800f;

        private WorldSphere _worldSphere;

        private Spaceship _playerShip;
        private List<Torpedo> _torpedos;

        private List<Asteroid> _asteroids;

        private GamePhysics _physics;

        private Func<string, Texture2D> _textureLoader;
        private Func<string, Model> _modelLoader;

        private GamePhysics.PhysicsDebugDraw DebugDrawer;

        private const double safeMs = 500;
        private double safeCtr = 0;

        public Scene(Game Game)
        {
            _textureLoader = (s) => Game.Content.Load<Texture2D>(s);
            _modelLoader = (s) => Game.Content.Load<Model>(s);

            _physics = new GamePhysics();

            ////// DEBUG DRAW //////
            // DebugDrawer = new GamePhysics.PhysicsDebugDraw(Game.GraphicsDevice);
            // _physics.DynamicsWorld.DebugDrawer = DebugDrawer;
            // DebugDrawer.DebugMode = DebugDrawModes.DrawAabb | DebugDrawModes.DrawWireframe;
            ////////////////////////

            _camera = new Camera(fieldOfView: MathHelper.PiOver4,
                aspectRatio: Game.GraphicsDevice.Viewport.AspectRatio);

            _skybox = new Skybox(Game.Content, "Textures/Skybox");

            _worldSphere = new WorldSphere(_physics.DynamicsWorld, _modelLoader, _worldRadius);

            _torpedos = new List<Torpedo>();

            _asteroids = new List<Asteroid>();
            initializeAsteroids(Game.Content);

            _playerShip = new Spaceship(_physics.DynamicsWorld,
                _modelLoader,
                _textureLoader,
                cameraOffset: new Vector3(0.0f, 5f, 50f),
                cameraRotation: Quaternion.CreateFromAxisAngle(Vector3.Forward, MathHelper.Pi / 64f),
                fireTorpedo: () =>
                {
                    var torpedo = new Torpedo(_physics.DynamicsWorld,
                        _modelLoader,
                        _playerShip.Position,
                        _playerShip.Forward * 1000f);
                    torpedo.Body.Activate();
                    torpedo.Body.LinearVelocity = -_playerShip.Forward * 500f;
                    _torpedos.Add(torpedo);
                    Console.WriteLine("BOOM! <" + _torpedos.Count + ">");
                    return torpedo;
                });
        }

        private void initializeAsteroids(ContentManager Content)
        {
            Array values = Enum.GetValues(typeof(Asteroid.AsteroidSize));
            Random random = new Random();
            float spaceBetween = 60f;
            for (int i = -3; i < 3; i++)
            {
                for (int j = -3; j < 3; j++)
                {
                    for (int k = -3; k < 3; k++)
                    {
                        if (i == 0 && j == 0 && k == 0) continue;

                        Asteroid.AsteroidSize randomSize = (Asteroid.AsteroidSize)values.GetValue(random.Next(values.Length));
                        // var randomSize = Asteroid.AsteroidSize.SMALL;

                        Asteroid newAsteroid = new Asteroid(_physics.DynamicsWorld,
                            _modelLoader,
                            _textureLoader,
                            new Vector3(i * spaceBetween, j * spaceBetween, k * spaceBetween), randomSize);
                        newAsteroid.Body.Activate();
                        newAsteroid.Body.AngularVelocity += new Vector3((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f);
                        newAsteroid.Body.LinearVelocity += new Vector3((float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f, (float)random.NextDouble() - 0.5f) * 100f;
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

            _physics.DynamicsWorld.PerformDiscreteCollisionDetection();
            int numManifolds = _physics.DynamicsWorld.Dispatcher.NumManifolds;
            //For each contact manifold
            for (int i = 0; i < numManifolds; i++)
            {
                PersistentManifold contactManifold = _physics.DynamicsWorld.Dispatcher.GetManifoldByIndexInternal(i);
                CollisionObject obA = contactManifold.Body0;
                CollisionObject obB = contactManifold.Body1;
                contactManifold.RefreshContactPoints(obA.WorldTransform, obB.WorldTransform);
                int numContacts = contactManifold.NumContacts;
                if (numContacts > 0)
                {
                    var collA = (GamePhysics.CollisionTypes)obA.UserIndex;
                    var collB = (GamePhysics.CollisionTypes)obB.UserIndex;
                    if (safeCtr >= safeMs)
                    {
                        if (collA == GamePhysics.CollisionTypes.Asteroid
                            && collB == GamePhysics.CollisionTypes.Ship)
                        {
                            _playerShip.lives--;
                            Console.WriteLine(_playerShip.lives);
                            safeCtr = 0;
                        }
                    }
                    else
                    {
                        safeCtr += gameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                }

                //For each contact point in that manifold
                // for (int j = 0; j < numContacts; j++)
                // {
                //     //Get the contact information
                //     ManifoldPoint pt = contactManifold.GetContactPoint(j);
                //     Vector3 ptA = pt.PositionWorldOnA;
                //     Vector3 ptB = pt.PositionWorldOnB;
                //     double ptdist = pt.Distance;
                //     Console.WriteLine(ptdist);
                // }
                if (_playerShip.lives == 0)
                {
                    Console.WriteLine("YOU LOSE!!!");
                    _playerShip.lives = 5;
                }
            }

            // _camera.Update(_playerShip, Vector3.Zero, Quaternion.Identity);
            _camera.Update(_playerShip, _playerShip.CameraOffset, _playerShip.CameraRotation);
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

            ////////////////////////// WorldSphere //////////////////////////
            _worldSphere.Draw(_camera);

            ////// DEBUG DRAW
            var basicEffect = new BasicEffect(graphics.GraphicsDevice);
            // basicEffect.VertexColorEnabled = true;
            // basicEffect.LightingEnabled = false;
            // basicEffect.World = Matrix.CreateWorld(_camera.Position, _camera.Forward, _camera.Up);
            basicEffect.World = Matrix.Identity;
            basicEffect.View = _camera.View;
            // DebugDrawer.DrawDebugWorld(_physics.DynamicsWorld);
            // DebugDrawer.DrawDebugWorld(_physics.DynamicsWorld);
            // _physics.DynamicsWorld.DebugDrawer = DebugDrawer;
            // _physics.DynamicsWorld.DebugDrawWorld();
            basicEffect.VertexColorEnabled = false;
            basicEffect.LightingEnabled = true;
        }

        public void ExitPhysics()
        {
            // foreach (var asteroid in _asteroids)
            //     asteroid.Dispose();
            // foreach (var torpedo in _torpedos)
            //     torpedo.Dispose();
            // _playerShip.Dispose();
            // _physics.ExitPhysics();
        }
    }
}
