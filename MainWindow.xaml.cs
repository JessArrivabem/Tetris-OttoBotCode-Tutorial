using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[] // arrawy containing the tiles images
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)), // index 0
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)), // index 1
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)), // index 2
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)), // index 3
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)), // index 4
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)), // index 5
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)), // index 6
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)), // index 7
        };

        private readonly ImageSource[] blockImages = new ImageSource[] // arrawy for the blocks images for health block and next block
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)), // index 0
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)), // index 1
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)), // index 2
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)), // index 3
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)), // index 4
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)), // index 5
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)), // index 6
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)), // index 7
        };
        private readonly Image[,] imageControls; // two dimensional arrawys of image controls
        // increase difficulty:
        private readonly int maxDelay = 1000; 
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;

        private GameState gameState = new GameState();

        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
        }
        private Image[,] SetupGameCanvas(GameGrid grid) // method to set up the image controls correctly in the canvas
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize,
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10); // the "+ 10" is to see the rows above and the block that makes player lose
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;

                }
            }

            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0;r < grid.Rows; r++)
            {
                for (int c = 0;c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r,c].Source = tileImages[id];
                }
            }
        }
        private void DrawBlock(Block block)
        {
            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if(heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            } else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();

            foreach(Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }
        
        private void Draw (GameState gameState) // draws both grid and current block
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score: {gameState.Score}";
        }


        private async Task GameLoop()
        {
            Draw(gameState); 

            while(!gameState.GameOver) // loop until the game is over
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease)); // difficulty variable - Increasing Score = more difficult
                await Task.Delay(delay);
                await Task.Delay(500);
                gameState.MoveBlockDown();
                Draw(gameState);
            }

            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Final Score: {gameState.Score}";
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(gameState.GameOver)
            {
                return;
            }

            switch(e.Key)
            {
                case Key.Left: gameState.MoveBlockLeft();
                    break;
                case Key.Right: gameState.MoveBlockRight();
                    break;
                case Key.Down: gameState.MoveBlockDown();
                    break;
                case Key.Up: gameState.RotateBlockCW();
                    break;
                case Key.Z: gameState.RotateBlockCCW();
                    break;
                case Key.C: gameState.HoldBlock();
                    break;
                case Key.Space: gameState.DropBlock();
                    break;
                default:
                    break;
            }

            Draw(gameState);
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
           await GameLoop();
        }

        private async void PlayAgain_Click(object sender, RoutedEventArgs e)
        {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }
    }

}