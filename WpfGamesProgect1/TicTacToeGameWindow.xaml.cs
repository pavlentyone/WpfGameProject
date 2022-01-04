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
using System.Windows.Shapes;

namespace WpfGamesProgect1
{
    /// <summary>
    /// Interaction logic for TicTacToeGameWindow.xaml
    /// </summary>
    public partial class TicTacToeGameWindow : Window
    {
        private int _verticalNumber = 3;
        public int VerticalNumber {
            get { return _verticalNumber; }
            set
            {
                _verticalNumber = value < _matchNumber ?
                    _verticalNumber :
                    value > 100 ?
                    _verticalNumber :
                    value;
            }
        }

        private int _horizontalNumber = 3;
        public int HorizontalNumber
        {
            get { return _horizontalNumber; }
            set
            {
                _horizontalNumber = value < _matchNumber ?
                    _horizontalNumber :
                    value > 100 ?
                    _horizontalNumber :
                    value;
            }
        }

        private int _matchNumber = 3;
        public int MatchNumber
        {
            get { return _matchNumber; }
            set
            {
                _matchNumber = value > _verticalNumber || value > _horizontalNumber ?
                    _matchNumber :
                    _matchNumber < 2 ?
                    _matchNumber :
                    value;
            }
        }

        private Dictionary<Players, Label> _playerSelectedLabel;
        private Dictionary<Players, Label> _playerSettingsLabel;
        private Label _standardSettingsLabel;

        private enum Marks : int { nothing, cross, toe }
        private enum Players : int { player1, player2 }

        private Dictionary<Players, Marks> _playerMarks;
        private Players _currentTurn;


        private Tuple<Label, Marks>[,] _ticTacToeLableMatrix;

        private Dictionary<Label, Players> turns;
        public TicTacToeGameWindow()
        {
            _playerSelectedLabel = new Dictionary<Players, Label>();
            foreach (Players player in Enum.GetValues(typeof(Players)))
                _playerSelectedLabel.Add(player, null);

            _playerSettingsLabel = new Dictionary<Players, Label>();
            _playerMarks = new Dictionary<Players, Marks>();
            foreach (Players player in Enum.GetValues(typeof(Players)))
            {
                Label l = new Label();
                Marks m = Marks.cross;
                switch (player)
                {
                    case Players.player1:
                        l.Background = new SolidColorBrush(Color.FromArgb(150, 255,150,150));
                        l.BorderThickness = new Thickness(2);
                        l.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 150, 150));
                        m = Marks.cross;
                        break;
                    case Players.player2:
                        l.Background = new SolidColorBrush(Color.FromArgb(150, 150, 255, 150));
                        l.BorderThickness = new Thickness(2);
                        l.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 150, 255, 150));
                        m = Marks.toe;
                        break;
                }
                _playerSettingsLabel.Add(player, l);
                _playerMarks.Add(player, m);
            }

            _standardSettingsLabel = new Label();
            _standardSettingsLabel.BorderThickness = new Thickness(2);
            _standardSettingsLabel.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            _standardSettingsLabel.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            

            _ticTacToeLableMatrix = new Tuple<Label, Marks>[_horizontalNumber, _verticalNumber];

            _currentTurn = Players.player1;

            turns = new Dictionary<Label, Players>();


            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _horizontalNumber; i++ )
                TicTacToeGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < _verticalNumber; i++)
                TicTacToeGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < _horizontalNumber; i++)
                for (int j = 0; j < _verticalNumber; j++)
                {
                    Label l = new Label();
                    l.BorderThickness = _standardSettingsLabel.BorderThickness;
                    l.BorderBrush = _standardSettingsLabel.BorderBrush;
                    l.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                    l.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                    l.MouseMove += new MouseEventHandler(Label_MouseMove);
                    TicTacToeGrid.Children.Add(l);
                    Grid.SetColumn(l, i);
                    Grid.SetRow(l, j);
                    _ticTacToeLableMatrix[i, j] = new Tuple<Label, Marks>() { Item1 = l, Item2 = Marks.nothing };
                }

            if (_ticTacToeLableMatrix.GetLength(0) > 0 && _ticTacToeLableMatrix.GetLength(1) > 0)
                foreach (Players player in Enum.GetValues(typeof(Players)))
                    _playerSelectedLabel[player] = _ticTacToeLableMatrix[0, 0].Item1;
        }

        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            ChooseSelected(sender, _currentTurn);
        }

        private void ChooseSelected(object sender, Players currentPlayer)
        {
            Label senderLabel = sender as Label;

            if (_playerSelectedLabel[currentPlayer] != senderLabel)
            {
                _playerSelectedLabel[currentPlayer].Background = _standardSettingsLabel.Background;

                foreach (KeyValuePair<Players, Label> player in _playerSelectedLabel)
                    if (player.Key != currentPlayer && player.Value == _playerSelectedLabel[currentPlayer])
                        _playerSelectedLabel[currentPlayer].Background = _playerSettingsLabel[player.Key].Background;


                _playerSelectedLabel[currentPlayer] = senderLabel;
                _playerSelectedLabel[currentPlayer].Background = _playerSettingsLabel[currentPlayer].Background;
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChooseEnter();
        }

        private void ChooseEnter()
        {
            foreach (KeyValuePair<Label, Players> turn in turns)
                if (turn.Key == _playerSelectedLabel[_currentTurn])
                    return;

            Marks currentMark = _playerMarks[_currentTurn];

            object content = null;

            switch (currentMark)
            {
                case Marks.cross:
                    content = new Image();
                    (content as Image).Source = new BitmapImage(new Uri(@"C:\Users\USeR-050716\documents\visual studio 2013\Projects\WpfGamesProgect1\WpfGamesProgect1\Images\cross.png"));
                    break;
                case Marks.toe:
                    content = new Image();
                    (content as Image).Source = new BitmapImage(new Uri(@"C:\Users\USeR-050716\documents\visual studio 2013\Projects\WpfGamesProgect1\WpfGamesProgect1\Images\toe.png"));
                    break;
            }

            _playerSelectedLabel[_currentTurn].Content = content;

            if (turns.Count > 0)
                turns.Last().Key.BorderBrush = _standardSettingsLabel.BorderBrush;
            turns.Add(_playerSelectedLabel[_currentTurn], _currentTurn);
            turns.Last().Key.BorderBrush = _playerSettingsLabel[_currentTurn].BorderBrush;

            for (int i = 0; i < _ticTacToeLableMatrix.GetLength(0); i++)
                for (int j = 0; j < _ticTacToeLableMatrix.GetLength(1); j++)
                    if (turns.Last().Key == _ticTacToeLableMatrix[i, j].Item1)
                        _ticTacToeLableMatrix[i, j].Item2 = currentMark;

            switch (_currentTurn)
            {
                case Players.player1:
                    _currentTurn = Players.player2;
                    break;
                case Players.player2:
                    _currentTurn = Players.player1;
                    break;
            }

            CheckMatches();
        }

        private void CheckMatches()
        {
            int column = -1;
            int row = -1;

            for (int i = 0; i < _ticTacToeLableMatrix.GetLength(0); i++)
                for (int j = 0; j < _ticTacToeLableMatrix.GetLength(1); j++)
                    if (turns.Last().Key == _ticTacToeLableMatrix[i, j].Item1)
                    {
                        column = i;
                        row = j;
                        goto AllFors;            
                    }

            AllFors:
            //if(column > -1 && row > -1 && column < _ticTacToeLableMatrix.GetLength(0))
            //{
            //    int startColumn = column - (_matchNumber - 1);
            //    int startRow = row - (_matchNumber - 1);

            //    Marks matchedMask = Marks.nothing;

            //    for (int i = 0; i < _matchNumber; i++, startColumn++, startRow++)
            //    {
            //        bool localMatch = true;

            //        if (!(startColumn > -1 && startRow > -1 &&
            //            startColumn < _ticTacToeLableMatrix.GetLength(0) &&
            //            startRow < _ticTacToeLableMatrix.GetLength(1)))
            //        {
            //            localMatch = false; continue;
            //        }

            //        Marks startMark = _ticTacToeLableMatrix[startColumn, startRow].Value;

            //        if (startMark == Marks.nothing)
            //        {
            //            localMatch = false;
            //            continue;
            //        }

            //        int currColumn = startColumn;
            //        int currRow = startRow;

            //        for (int j = 0; j < _matchNumber; j++, currColumn++, currRow++)
            //        {
            //            if (!(currColumn > -1 && currRow > -1 &&
            //                currColumn < _ticTacToeLableMatrix.GetLength(0) &&
            //                currRow < _ticTacToeLableMatrix.GetLength(1)))
            //            {
            //                localMatch = false; continue;
            //            }

            //            Marks currMark = _ticTacToeLableMatrix[currColumn, currRow].Value;

            //            if (currMark != startMark)
            //            {
            //                localMatch = false; continue;
            //            }
            //        }

            //        if (localMatch)
            //        {
            //            matchedMask = startMark; break;
            //        }
            //    }

            //    if (matchedMask != Marks.nothing)
            //    {
            //        InfoTextBlock.Visibility = System.Windows.Visibility.Visible;
            //        InfoTextBlock.Text = String.Format(@"Команда '{0}' победила!", matchedMask);
            //    }
            //}

            if (column > -1 && row > -1 && column < _ticTacToeLableMatrix.GetLength(0))
            {
                int maxLength = 2 * _matchNumber - 1;

                Tuple<Label, Marks>[] upLeft = new Tuple<Label, Marks>[maxLength];
                Tuple<Label, Marks>[] downLeft = new Tuple<Label, Marks>[maxLength];
                Tuple<Label, Marks>[] upDown = new Tuple<Label, Marks>[maxLength];
                Tuple<Label, Marks>[] leftRight = new Tuple<Label, Marks>[maxLength];


                int upLeftStartColumn = column - (_matchNumber - 1);
                int upLeftStartRow = row - (_matchNumber - 1);

                int downLeftColumn = column + (_matchNumber - 1);
                int downLeftRow = row - (_matchNumber - 1);


                int upDownColumn = column;
                int upDownRow = row - (_matchNumber - 1);


                int leftRightColumn = column - (_matchNumber - 1);
                int leftRightRow = row;
                for (int i = 0; i < maxLength; i++)
                {
                    upLeft[i] = 
                        upLeftStartColumn > -1 && upLeftStartRow > -1 &&
                        upLeftStartColumn < _ticTacToeLableMatrix.GetLength(0) &&
                        upLeftStartRow < _ticTacToeLableMatrix.GetLength(1) ?
                        new Tuple<Label, Marks>() { 
                            Item1 = _ticTacToeLableMatrix[upLeftStartColumn, upLeftStartRow].Item1, 
                            Item2 = _ticTacToeLableMatrix[upLeftStartColumn, upLeftStartRow].Item2 
                        } :
                        new Tuple<Label, Marks>() { Item1 = null, Item2 = Marks.nothing};

                    downLeft[i] = downLeftColumn > -1 && downLeftRow > -1 &&
                        downLeftColumn < _ticTacToeLableMatrix.GetLength(0) &&
                        downLeftRow < _ticTacToeLableMatrix.GetLength(1) ?
                        new Tuple<Label, Marks>()
                        {
                            Item1 = _ticTacToeLableMatrix[downLeftColumn, downLeftRow].Item1,
                            Item2 = _ticTacToeLableMatrix[downLeftColumn, downLeftRow].Item2
                        } :
                        new Tuple<Label, Marks>() { Item1 = null, Item2 = Marks.nothing};

                    upDown[i] = upDownColumn > -1 && upDownRow > -1 &&
                        upDownColumn < _ticTacToeLableMatrix.GetLength(0) &&
                        upDownRow < _ticTacToeLableMatrix.GetLength(1) ?
                        new Tuple<Label, Marks>()
                        {
                            Item1 = _ticTacToeLableMatrix[upDownColumn, upDownRow].Item1,
                            Item2 = _ticTacToeLableMatrix[upDownColumn, upDownRow].Item2
                        } :
                        new Tuple<Label, Marks>() { Item1 = null, Item2 = Marks.nothing};

                    leftRight[i] = leftRightColumn > -1 && leftRightRow > -1 &&
                        leftRightColumn < _ticTacToeLableMatrix.GetLength(0) &&
                        leftRightRow < _ticTacToeLableMatrix.GetLength(1) ?
                        new Tuple<Label, Marks>()
                        {
                            Item1 = _ticTacToeLableMatrix[leftRightColumn, leftRightRow].Item1,
                            Item2 = _ticTacToeLableMatrix[leftRightColumn, leftRightRow].Item2
                        } :
                        new Tuple<Label, Marks>() { Item1 = null, Item2 = Marks.nothing};

                    upLeftStartColumn++;
                    upLeftStartRow++;
                    downLeftColumn--;
                    downLeftRow++;
                    upDownRow++;
                    leftRightColumn++;
                }

                for (int i = 0; i < _matchNumber; i++)
                {
                    bool upLeftMatch = true;
                    bool downLeftMatch = true;
                    bool upDownMatch = true;
                    bool leftRightMatch = true;
                    for (int j = 0; j < _matchNumber; j++)
                    {
                        if (upLeft[i].Item2 == Marks.nothing || upLeft[i].Item2 != upLeft[i + j].Item2)
                            upLeftMatch = false;
                        if (downLeft[i].Item2 == Marks.nothing || downLeft[i].Item2 != downLeft[i + j].Item2)
                            downLeftMatch = false;
                        if (upDown[i].Item2 == Marks.nothing || upDown[i].Item2 != upDown[i + j].Item2)
                            upDownMatch = false;
                        if (leftRight[i].Item2 == Marks.nothing || leftRight[i].Item2 != leftRight[i + j].Item2)
                            leftRightMatch = false;

                        if (upLeftMatch == false && downLeftMatch == false && upDownMatch == false && leftRightMatch == false)
                            break;
                    }

                    if (upLeftMatch | downLeftMatch | upDownMatch | leftRightMatch)
                    {
                        RestartButton.Visibility = System.Windows.Visibility.Visible;
                        RestartButton.Width = 70;
                        this.Height += RestartButton.Width;
                        this.Width += RestartButton.Width;
                        InfoTextBlock.Height = RestartButton.Width;
                        InfoTextBlock.Visibility = System.Windows.Visibility.Visible;
                        TicTacToeGrid.IsEnabled = false;

                        if (upLeftMatch)
                            InfoTextBlock.Content = String.Format(@"Команда '{0}' победила!", upLeft[i].Item2);
                        else if (downLeftMatch)
                            InfoTextBlock.Content = String.Format(@"Команда '{0}' победила!", downLeft[i].Item2);
                        else if (upDownMatch)
                            InfoTextBlock.Content = String.Format(@"Команда '{0}' победила!", upDown[i].Item2);
                        else if (leftRightMatch)
                            InfoTextBlock.Content = String.Format(@"Команда '{0}' победила!", leftRight[i].Item2);
                    }
                }
            }
        }

        class Tuple<t1, t2>
        {
            public t1 Item1 { get; set; }
            public t2 Item2 { get; set; }

            public Tuple() { }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {

            Dictionary<Players, int> playerColumn = new Dictionary<Players,int>();
            Dictionary<Players, int> playerRow = new Dictionary<Players,int>();

            foreach (Players player in Enum.GetValues(typeof(Players)))
            {
                int column, row;
                FindInMatrix(_playerSelectedLabel[player], out column, out row);
                playerColumn.Add(player, column);
                playerRow.Add(player, row);
            }

            switch(e.Key){
                case Key.W:
                    playerRow[Players.player2]--;
                    if (playerRow[Players.player2] > -1)
                        ChooseSelected(_ticTacToeLableMatrix[playerColumn[Players.player2], playerRow[Players.player2]].Item1, Players.player2);
                    break;
                case Key.A:
                    playerColumn[Players.player2]--;
                    if (playerColumn[Players.player2] > -1)
                        ChooseSelected(_ticTacToeLableMatrix[playerColumn[Players.player2], playerRow[Players.player2]].Item1, Players.player2);
                    break;
                case Key.S:
                    playerRow[Players.player2]++;
                    if (playerRow[Players.player2] < _ticTacToeLableMatrix.GetLength(1))
                        ChooseSelected(_ticTacToeLableMatrix[playerColumn[Players.player2], playerRow[Players.player2]].Item1, Players.player2);
                    break;
                case Key.D:
                    playerColumn[Players.player2]++;
                    if (playerColumn[Players.player2] < _ticTacToeLableMatrix.GetLength(0))
                        ChooseSelected(_ticTacToeLableMatrix[playerColumn[Players.player2], playerRow[Players.player2]].Item1, Players.player2);
                    break;
                case Key.Up:
                    playerRow[Players.player1]--;
                    if (playerRow[Players.player1] > -1)
                        ChooseSelected(_ticTacToeLableMatrix[playerColumn[Players.player1], playerRow[Players.player1]].Item1, Players.player1);
                    break;
                case Key.Left:
                    playerColumn[Players.player1]--;
                    if (playerColumn[Players.player1] > -1)
                        ChooseSelected(_ticTacToeLableMatrix[playerColumn[Players.player1], playerRow[Players.player1]].Item1, Players.player1);
                    break;
                case Key.Down:
                    playerRow[Players.player1]++;
                    if (playerRow[Players.player1] < _ticTacToeLableMatrix.GetLength(1))
                        ChooseSelected(_ticTacToeLableMatrix[playerColumn[Players.player1], playerRow[Players.player1]].Item1, Players.player1);
                    break;
                case Key.Right:
                    playerColumn[Players.player1]++;
                    if (playerColumn[Players.player1] < _ticTacToeLableMatrix.GetLength(0))
                        ChooseSelected(_ticTacToeLableMatrix[playerColumn[Players.player1], playerRow[Players.player1]].Item1, Players.player1);
                    break;
                case Key.Space:
                    if (_currentTurn == Players.player2)
                        ChooseEnter();
                    break;
                case Key.Enter:
                    if (_currentTurn == Players.player1)
                        ChooseEnter();
                    break;
                default:
                    break;
            }
        }

        private bool FindInMatrix(Label label, out int column, out int row)
        {
            column = -1;
            row = -1;

            for(int i = 0; i < _ticTacToeLableMatrix.GetLength(0); i++)
                for(int j = 0; j < _ticTacToeLableMatrix.GetLength(1); j++)
                    if (_ticTacToeLableMatrix[i, j].Item1 == label)
                    {
                        column = i;
                        row = j;
                        return true;
                    }

            return false;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            _currentTurn = Players.player1;

            RestartButton.Visibility = System.Windows.Visibility.Collapsed;
            InfoTextBlock.Visibility = System.Windows.Visibility.Collapsed;
            this.Height -= RestartButton.Width;
            this.Width -= RestartButton.Width;
            TicTacToeGrid.IsEnabled = true;
            //this.Height = 300;
            //this.Width = 300;

            for(int i = 0; i < _ticTacToeLableMatrix.GetLength(0); i++)
                for(int j = 0; j < _ticTacToeLableMatrix.GetLength(1); j++)
                    _ticTacToeLableMatrix[i, j].Item2 = Marks.nothing;

            turns.Clear();

            foreach(object child in TicTacToeGrid.Children)
                if (child.GetType() == typeof(Label))
                {
                    (child as Label).Content = _standardSettingsLabel.Content;
                    (child as Label).BorderBrush = _standardSettingsLabel.BorderBrush;
                }
        }
    }
}
