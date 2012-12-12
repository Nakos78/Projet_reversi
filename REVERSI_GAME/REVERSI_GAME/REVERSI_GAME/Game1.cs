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

namespace REVERSI_GAME
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //appel de la fonction TileMap

        TileMap myMap = new TileMap();
        int squaresAcross = 5;//largeur x
        int squaresDown = 10;//hauteur y

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
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
            Tile.TileSetTexture = Content.Load<Texture2D>(@"Textures\TileSets\part1_tileset");
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            KeyboardState Etatclavier = Keyboard.GetState();
            if (Etatclavier.IsKeyDown(Keys.Left))
            {
                Camera.Position.X = MathHelper.Clamp(Camera.Position.X - 2, 0, (myMap.MapWidth - squaresAcross) * 32);
            }

            if (Etatclavier.IsKeyDown(Keys.Right))
            {
                Camera.Position.X = MathHelper.Clamp(Camera.Position.X + 2, 0, (myMap.MapWidth - squaresAcross) * 32);
            }

            if (Etatclavier.IsKeyDown(Keys.Up))
            {
                Camera.Position.Y = MathHelper.Clamp(Camera.Position.Y - 2, 0, (myMap.MapHeight - squaresDown) * 32);
            }

            if (Etatclavier.IsKeyDown(Keys.Down))
            {
                Camera.Position.Y = MathHelper.Clamp(Camera.Position.Y + 2, 0, (myMap.MapHeight - squaresDown) * 32);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
            //la camera s'affiche dans le coin sup�rieur gauche.
            #region
            /*
             La variable Camera.Position maintient le point sur la carte qui doit �tre affich� dans le coin sup�rieur 
               gauche  de la carte. Nous devons d�terminer ce carr� sur la carte ce qui correspond �, 
               si nous divisons X de la cam�ra et les composants Y par la taille d'un seul carreau(32 dans notre cas,). 
               Le r�sultat est un vecteur de coordonn�es Mapsquare qui pointe vers le carreau que la camera indique.
               Quand notre camera est en(0,0) (La valeur par d�faut) Nous divisons 0 par 32 , et toujours avoir (0,0),
               Donc par d�faut le carr� en haut � gauche de la carte sera affich�e donc le carr� dans le coin sup�rieur gauche de l'�cran. 
               Si nous devions d�placer la cam�ra droite par 32 pixels (X = 32), Le r�sultat serait (32/32, 0/32) Ou (1, 0). 
               Cela signifie que la place juste � droite du carr� sup�rieur gauche serait le carr� de d�part de notre �cran. 
               Une fois que nous savons ce que nous voulons pour commencer � dessiner notre tuile, 
               nous avons besoin de savoir o� le dessiner. Encore une fois, si notre camera est en (0,0) 
               Nous voulons simplement attirer notre premi�re tuile � (0,0). Mais comme notre cam�ra se d�place par incr�ments de moins d'un carrelage, 
               il nous faut passer o� nous commencer � dessiner la tuile de plus en plus hors de l'�cran. 
               Si nous sommes � mi-chemin � travers un carreau � droite(Camera.Position = (16,0)),
               Nous voulons d�placer tout ce que nous attirer 16 pixels vers la gauche afin que la partie droite de la tuile appara�t
               dans le coin sup�rieur gauche de l'�cran. Maintenant, nous avons juste besoin d'une boucle pour dessiner chacune des tuiles 
               dont nous avons besoin pour afficher sur le �cran. Dans l'appel SpriteBatch.Draw() , le premier param�tre 
               rectangle ( new Rectangle((X * 32) - OffsetX, (Y * 32) - OffsetY, 32, 32)) D�termine o� sur l'�cran la tuile sera �tabli. 
               Chaque tuile que nous attirons est d�cal�e en fonction de son emplacement par rapport � la tuile de d�part. 
               C'est pourquoi nous multiplions les valeurs x et y de la boucle par  32. 
               La premi�re tuile nous nous appuyons sur une ligne sera par la tuile 0 que nous avons �labor�, ainsi 0 * 32 = 0. 
               La deuxi�me tuile sur la ligne sera de 1 * 32 = 32, donc d�cal� de 32 pixels. Chaque colonne fonctionne de la m�me mani�re.
               Les valeurs offsetX et offsetY d�placer le dessin de la tuile par la quantit� calcul�e ci-dessus, 
               nous rendre compte de l'appareil �tant comprise entre les marqueurs de carreaux entiers.
               La deuxi�me rectangle( Tile.GetSourceRectangle(MyMap.Rows [y] + Firsty. Colonnes [x + firstX]. TileID)) 
               Provient directement de notre static class Tile. 
               Ici, nous utilisons les premi�res valeurs carr�s nous avons calcul� au d�but de la M�thode Draw()  pour rechercher 
               la ligne et de colonne sur la carte. Notez que puisque nous utilisons une liste d'articles MapRow, 
               nous regardons la premi�re coordonn�e Y(La ligne), Puis la coordonn�e X (La colonne dans la ligne). 
          
             */


            #endregion
            Vector2 firstSquare = new Vector2(Camera.Position.X / 32, Camera.Position.Y / 32);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            Vector2 squareOffset = new Vector2(Camera.Position.X % 32, Camera.Position.Y % 32);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            for (int y = 0; y < squaresDown; y++)
            {
                for (int x = 0; x < squaresAcross; x++)
                {
                    spriteBatch.Draw(
                        Tile.TileSetTexture,
                        new Rectangle((x * 32) - offsetX, (y * 32) - offsetY, 32, 32),
                        Tile.GetSourceRectangle(myMap.Lignes[y + firstY].Colonnes[x + firstX].TileID),
                        Color.White);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
