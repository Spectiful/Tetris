using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris_C_Sharp_Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] Imagecontrols;
        private readonly int maxdelay = 1000;
        private readonly int mindelay = 75;
        private readonly int delaydecrease = 25;

        private Gamestate gameState = new Gamestate();

        public MainWindow()
        {
            InitializeComponent();
            Imagecontrols = Setupgamecanvas(gameState.TetrisGrid);
        }

        private Image[,] Setupgamecanvas(TetrisGrid grid)
        {
            Image[,] Imagecontrols = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int a = 0; a < grid.Rows; a++)
            {
                for (int b = 0; b < grid.Columns; b++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (a - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, b * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    Imagecontrols[a, b] = imageControl;
                }
            }

            return Imagecontrols;
        }

        private void DrawGrid(TetrisGrid grid)
        {
            for (int a = 0; a < grid.Rows; a++)
            {
                for (int b = 0; b < grid.Columns; b++)
                {
                    int id = grid[a, b];
                    Imagecontrols[a, b].Opacity = 1;
                    Imagecontrols[a, b].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                Imagecontrols[p.Row, p.Column].Opacity = 1;
                Imagecontrols[p.Row, p.Column].Source = tileImages[block.id];
            }
        }

        private void DrawNextBlock(BlockQue blockQueue)
        {
            Block next = blockQueue.Nextblock;
            NextImage.Source = blockImages[next.id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.Blockdropdistance();

            foreach (Position p in block.TilePositions())
            {
                Imagecontrols[p.Row + dropDistance, p.Column].Opacity = 0.25;
                Imagecontrols[p.Row + dropDistance, p.Column].Source = tileImages[block.id];
            }
        }

        private void Draw(Gamestate gameState)
        {
            DrawGrid(gameState.TetrisGrid);
            DrawGhostBlock(gameState.Currentblock2);
            DrawBlock(gameState.Currentblock2);
            DrawNextBlock(gameState.blockQue);
            DrawHeldBlock(gameState.Heldblock);
            ScoreText.Text = $"Score: {gameState.Score}";
        }

        private async Task GameLoop()
        {
            Draw(gameState);

            while (!gameState.GameOver)
            {
                int delay = Math.Max(mindelay, maxdelay - (gameState.Score * delaydecrease));
                await Task.Delay(delay);
                gameState.Moveblockdown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score: {gameState.Score}";
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    gameState.Moveblockleft();
                    break;
                case Key.Right:
                    gameState.Moveblockright();
                    break;
                case Key.Down:
                    gameState.Moveblockdown();
                    break;
                case Key.Up:
                    gameState.Rotateblockclockwise();
                    break;
                case Key.Z:
                    gameState.Rotateblockcounterclockwise();
                    break;
                case Key.C:
                    gameState.Holdblock();
                    break;
                case Key.Space:
                    gameState.Dropblock();
                    break;
                default:
                    return;
            }

            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new Gamestate();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }
}
