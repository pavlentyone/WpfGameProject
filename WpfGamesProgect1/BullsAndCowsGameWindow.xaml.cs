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
using System.Text.RegularExpressions;

namespace WpfGamesProgect1
{
    /// <summary>
    /// Interaction logic for BullsAndCowsGameWindow.xaml
    /// </summary>
    public partial class BullsAndCowsGameWindow : Window
    {
        private char[] _allowedSymbols = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public char[] AllowedSymbols
        {
            get { return _allowedSymbols; }
            set { _allowedSymbols = value; }
        }

        private char[] _target = new char[4];

        public char[] Target
        {
            get { return _target; }
            set { _target = value; }
        }

        private bool _allowRepeatedTargetSymbols = true;
        public bool AllowRepeatedTargetSymbols
        {
            get { return _allowRepeatedTargetSymbols; }
            set { _allowRepeatedTargetSymbols = value; }
        }

        private bool _allowRepeatedAnswerSymbols = true;
        public bool AllowRepeatedAnswerSymbols
        {
            get { return _allowRepeatedAnswerSymbols; }
            set { _allowRepeatedAnswerSymbols = value; }
        }

        private Random _random;

        public BullsAndCowsGameWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _allowedSymbols = _allowedSymbols ?? new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            _target = _target ?? new char[4];

            _random = new Random();

            InitGame();
        }

        private void InitGame()
        {
            AnswerMaskedTextBox.Text = "";

            ResultTextBox.Text = "Введите свой ответ ниже:";

            //инициализация списка всех допустимых значений
            List<char> symbolsLeft = _allowedSymbols.ToList();

            //инициализируем случайными неповторяющимися значениями
            for (int i = 0; i < _target.Length && i < _target.Length; i++)
            {
                int randomChoise = _random.Next(0, symbolsLeft.Count() - 1);

                _target[i] = symbolsLeft[randomChoise];

                if(!_allowRepeatedTargetSymbols)
                    symbolsLeft.RemoveAt(randomChoise);
            }

            //генерируем поле ввода
            AnswerMaskedTextBox.Mask = new string('0', _target.Length);

            RestartButton.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            //получаем значения нашего ответа и правильного ответа
            string answer = AnswerMaskedTextBox.Text;
            string target = new string(_target);

            //если рассинхронизация
            if(answer.Length != target.Length)
                MessageBox.Show(String.Format("Количество символов в ответе ({0}) и количество символов в цели({1}) не совпадают", answer.Length, target.Length));

            //подсчет быков и коров
            int bullsCounter = 0;
            int cowsCounter = 0;
            for (int i = 0; i < answer.Length && i < target.Length; i++)
                if (answer[i] == target[i])
                    bullsCounter++;
                else if(target.Contains(answer[i]))
                    cowsCounter++;

            ResultTextBox.Text = String.Format("Количество быков: {0}. Количество коров: {1}", bullsCounter, cowsCounter);

            //если количество быков совпадает с количеством символов в правильном ответе - победа!
            if (bullsCounter == target.Length)
                RestartButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            InitGame();
        }

        private void AnswerMaskedTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!_allowRepeatedAnswerSymbols)
            {
                string changed = e.Text;

                foreach (char c in changed)
                {
                    Regex regex = new Regex(c.ToString());

                    if (regex.Matches(AnswerMaskedTextBox.Text).Count > 0)
                    {
                        e.Handled = true;
                        break;
                    }
                }
            }
        }
    }
}
