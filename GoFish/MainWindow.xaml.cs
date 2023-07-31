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

namespace GoFish
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameController controller;
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Restart a game with returning to main grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (firstGrid.Visibility == Visibility.Visible)
            {
                secondGrid.Visibility = Visibility.Visible;
                firstGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                firstGrid.Visibility = Visibility.Visible;
                secondGrid.Visibility = Visibility.Collapsed;
                AskAPlayer.Visibility = Visibility.Visible;
                BackToMainMenu.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// choose AI player in PlayersList and card in YourHandListBox to make a request
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ask(object sender, RoutedEventArgs e)
        {
            if (YourHandListBox.SelectedItem != null)
            {
                controller.PlayersTurnLogic(controller.Players[PlayersList.SelectedIndex + 1], YourHandListBox.SelectedIndex);
                RefreshListBoxes();
                GameOverCheck();
            }
            else
                MessageBox.Show("Select a card in your hand");

        }
        /// <summary>
        /// Start game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            firstGrid.Visibility = Visibility.Collapsed;
            secondGrid.Visibility= Visibility.Visible;
            controller = new GameController((int)NumberOfPlayers.Value);
            RefreshListBoxes();
            
        }
        /// <summary>
        /// Refresh all listboxes information
        /// </summary>
        private void RefreshListBoxes()
        {
            YourHandListBox.ItemsSource = null;
            ScoreListBox.ItemsSource = null;
            PlayersList.ItemsSource = null;
            GameProgressListBox.ItemsSource = null;

            GameProgressListBox.ItemsSource = controller.GameLog.Split("\n");
            YourHandListBox.ItemsSource = controller.Players[0].Hand;
            YourHandListBox.SelectedItem = null;
            PlayersList.ItemsSource = controller.Players.TakeLast(controller.Players.Count - 1);
            PlayersList.SelectedItem = controller.Players[1];
            ScoreListBox.ItemsSource = controller.ScoresString;
        }
        /// <summary>
        /// method, that check GameOver status, and if GameOver is true - change a button to possibility make a new game
        /// </summary>
        private void GameOverCheck()
        {
            if (controller.GameOver == true)
            {
                AskAPlayer.Visibility = Visibility.Collapsed;
                BackToMainMenu.Visibility = Visibility.Visible;
            }
        }

    }
}
