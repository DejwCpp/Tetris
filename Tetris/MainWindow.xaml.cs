﻿using System.Text;
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
        public double GameBoardWidth { get; set; } = 350;
        private GameBoard gameBoard;

        public MainWindow()
        {
            InitializeComponent();

            // thank to this, xaml can use GameBoardWidth
            DataContext = this;

            gameBoard = new GameBoard(18, 10);
        }

        
    }
}