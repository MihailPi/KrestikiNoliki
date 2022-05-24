using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Media;


namespace KrestikiNoliki
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        //  Пути к звукам
        static SoundPlayer soundWin = new SoundPlayer(@"Sound/Win.wav");
        static SoundPlayer soundKrestik = new SoundPlayer(@"Sound/Krestik.wav");
        static SoundPlayer soundNolik = new SoundPlayer(@"Sound/Nolik.wav");
        static SoundPlayer soundLine = new SoundPlayer(@"Sound/Line.wav");
        //  Пути к изображениям
        Dictionary<string, Uri> uris = new Dictionary<string, Uri>()
        {
            {"Nolik", new Uri("Image/Nolik.png", UriKind.Relative) },
            {"Krestik", new Uri("Image/Krestik.png", UriKind.Relative) },
            {"Now", new Uri("Image/Now.png", UriKind.Relative)},
            {"Win", new Uri("Image/Winner.png", UriKind.Relative)},
            {"D1", new Uri("Image/Diagonal1.png", UriKind.Relative) },
            {"D2", new Uri("Image/Diagonal2.png", UriKind.Relative) },
            {"H1", new Uri("Image/Horizontal1.png", UriKind.Relative)},
            {"H2", new Uri("Image/Horizontal2.png", UriKind.Relative)},
            {"H3", new Uri("Image/Horizontal3.png", UriKind.Relative)},
            {"V1", new Uri("Image/Vertical1.png", UriKind.Relative)},
            {"V2", new Uri("Image/Vertical2.png", UriKind.Relative)},
            {"V3", new Uri("Image/Vertical3.png", UriKind.Relative)},
            {"Game1", new Uri("Image/NewGame2.png", UriKind.Relative)},
            {"Game2", new Uri("Image/NewGame1.png", UriKind.Relative)},
            {"Nul", new Uri("Image/NullImage.png", UriKind.Relative)}
        };

        List<Uri> numbers = new List<Uri>()
        {
            new Uri("Image/Number1.png", UriKind.Relative),
            new Uri("Image/Number2.png", UriKind.Relative),
            new Uri("Image/Number3.png", UriKind.Relative),
            new Uri("Image/Number4.png", UriKind.Relative),
            new Uri("Image/Number5.png", UriKind.Relative),
            new Uri("Image/Number6.png", UriKind.Relative),
            new Uri("Image/Number7.png", UriKind.Relative),
            new Uri("Image/Number8.png", UriKind.Relative),
            new Uri("Image/Number9.png", UriKind.Relative)
        };

        //  Пустое изображение (для замены на нужное)
        const string DEF_SOURCE = "pack://application:,,,/KrestikiNoliki;component/Image/NullImage.png";
        const char SIMV_X = 'X';
        const char SIMV_O = 'O';
        //  Поле
        static char[,] feal = new char[3,3];
        //  Флаги
        static bool flagXorO = false;
        static bool gameOver = false;
        static byte countKrestik = 0;
        static byte countNolik = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //  Получаем объект, преобразовываем в Image
            var image = (Image)sender;
            //  Пока не найдено выйграшной комбинации у одного из игроков
            if (!gameOver)
            {
                //  Если ячейка еще не занята
                if (image.Source.ToString() == DEF_SOURCE)
                {   //  Ставим поочереди крестик и нолик
                    if (!flagXorO)
                    {
                        image.Source = new BitmapImage(uris["Krestik"]);
                        AddToFeal(image.Uid, true);
                        soundKrestik.Play();
                        flagXorO = true;
                        //  Проверяем на выйграшную комбинацию
                        var chek = CheckWin(SIMV_X);
                        if (chek != "")
                            WinKrestik(chek);
                        else
                        {
                            MessageImage.Source = new BitmapImage(uris["Now"]);
                            PlayerImage.Source = new BitmapImage(uris["Nolik"]);
                        }
                    }
                    else
                    {
                        image.Source = new BitmapImage(uris["Nolik"]);
                        AddToFeal(image.Uid, false);
                        soundNolik.Play();
                        flagXorO = false;
                        var chek = CheckWin(SIMV_O);
                        if (chek != "")
                            WinNolik(chek);
                        else
                        {
                            MessageImage.Source = new BitmapImage(uris["Now"]);
                            PlayerImage.Source = new BitmapImage(uris["Krestik"]);
                        }
                    }
                }
                else
                    return;
            }
            else
                return;
        }


        //  Обработчики победы
        private void WinNolik(string chek)
        {
            soundLine.Play();
            gameOver = true;
            MessageImage.Source = new BitmapImage(uris["Win"]);
            PlayerImage.Source = new BitmapImage(uris["Nolik"]);
            WinLineImage.Source = new BitmapImage(uris[chek]);
            countNolik++;
            if (countNolik < 10)
                CountNolik.Source = new BitmapImage(numbers[countNolik - 1]);
            else
                EndGame(true, "Nolik");
        }

        private void WinKrestik(string chek)
        {
            soundLine.Play();
            gameOver = true;
            MessageImage.Source = new BitmapImage(uris["Win"]);
            PlayerImage.Source = new BitmapImage(uris["Krestik"]);
            WinLineImage.Source = new BitmapImage(uris[chek]);
            countKrestik++;
            if (countKrestik < 10)
                CountKrestik.Source = new BitmapImage(numbers[countKrestik - 1]);
            else
                EndGame(true, "Krestik");
        }

        private void EndGame(bool endAllGame, string player="player")
        {
            gameOver = false;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    feal[i, j] = ' ';
                }
            }
            WinLineImage.Source = new BitmapImage(uris["Nul"]);
            Kletka1.Source = new BitmapImage(uris["Nul"]);
            Kletka2.Source = new BitmapImage(uris["Nul"]);
            Kletka3.Source = new BitmapImage(uris["Nul"]);
            Kletka4.Source = new BitmapImage(uris["Nul"]);
            Kletka5.Source = new BitmapImage(uris["Nul"]);
            Kletka6.Source = new BitmapImage(uris["Nul"]);
            Kletka7.Source = new BitmapImage(uris["Nul"]);
            Kletka8.Source = new BitmapImage(uris["Nul"]);
            Kletka9.Source = new BitmapImage(uris["Nul"]);

            if (!endAllGame)
            {
                // Если это не первая игра первым в новой игре ходит проигравший предыдущую
                if (flagXorO)
                {
                    MessageImage.Source = new BitmapImage(uris["Now"]);
                    PlayerImage.Source = new BitmapImage(uris["Nolik"]);
                }
                else
                {
                    MessageImage.Source = new BitmapImage(uris["Now"]);
                    PlayerImage.Source = new BitmapImage(uris["Krestik"]);
                }
            }
            else
            {
                soundWin.Play();
                CountKrestik.Source = new BitmapImage(uris["Nul"]);
                CountNolik.Source = new BitmapImage(uris["Nul"]);
                MessageImage.Source = new BitmapImage(uris["Win"]);
                PlayerImage.Source = new BitmapImage(uris[player]);
            }

        }

        private void AddToFeal(string uid, bool krestOrNull)
        {   //  Преобразование uid в координаты
            var coord = uid.Split('|');
            int x = Convert.ToInt32(coord[0]);
            int y = Convert.ToInt32(coord[1]);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if( x == i && y == j)
                    {   //  Добавляем в массив нужный символ
                        if (krestOrNull)
                            feal[i, j] = 'X';
                        else
                            feal[i, j] = 'O';
                    }
                        
                }
            }
        }

        //Выйграшные комбинации
        private string CheckWin(char simv)
        {
            if (feal[0, 0] == simv && feal[0, 1] == simv && feal[0, 2] == simv)
                return "H1";
            else if (feal[1, 0] == simv && feal[1, 1] == simv && feal[1, 2] == simv)
                return "H2";
            else if (feal[2, 0] == simv && feal[2, 1] == simv && feal[2, 2] == simv)
                return "H3";
            else if (feal[0, 0] == simv && feal[1, 0] == simv && feal[2, 0] == simv)
                return "V1";
            else if (feal[0, 1] == simv && feal[1, 1] == simv && feal[2, 1] == simv)
                return "V2";
            else if (feal[0, 2] == simv && feal[1, 2] == simv && feal[2, 2] == simv)
                return "V3";
            else if (feal[0, 0] == simv && feal[1, 1] == simv && feal[2, 2] == simv)
                return "D1";
            else if (feal[0, 2] == simv && feal[1, 1] == simv && feal[2, 0] == simv)
                return "D2";
            else
                return "";
        }

        //  Изменение кнопки Новая игра при наведении мышки
        private void NewGameImage_MouseEnter(object sender, MouseEventArgs e)
        {
            NewGameImage.Source = new BitmapImage(uris["Game1"]);
        }
        private void NewGameImage_MouseLeave(object sender, MouseEventArgs e)
        {
            NewGameImage.Source = new BitmapImage(uris["Game2"]);
        }

        //  Сбрасываем все в ноль
        private void NewGameImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EndGame(false);   
        }
    }
}
