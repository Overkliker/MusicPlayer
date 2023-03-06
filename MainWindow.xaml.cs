using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
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

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool start = false;


        private bool RepeatIsEnbled = false;
        private bool RandomIsEnbled = false;
        TimeSpan? lastSpan;
        public MainWindow()
        {

            Interface.player.MediaEnded += Player_MediaEnded;
            Interface.player.MediaOpened += Player_MediaOpened;
            InitializeComponent();
            Interface.player.Volume = Sound.Value / 100;
            Thread th = new Thread(_ =>
            {
                while (true)
                {
                    while (start)
                    {
                        Dispatcher.Invoke((Action)(() =>
                        {
                            SlideMusic.Value = (double)Interface.player.Position.TotalSeconds;
                            Timer.Content = $"{(int)Interface.player.Position.Minutes}:{Interface.player.Position.Seconds:00}";
                        }));
                        Thread.Sleep(10);
                    }
                }
            });
            th.Start();
        }

        private void Player_MediaOpened(object? sender, EventArgs e)
        {
            start = true;
            Timer.Content = $"{(int)Interface.player.NaturalDuration.TimeSpan.TotalMinutes}:{Interface.player.NaturalDuration.TimeSpan.Seconds:00}";
            SlideMusic.Maximum = (double)Interface.player.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void Player_MediaEnded(object? sender, EventArgs e)
        {
            SlideMusic.Value = 0;
            start = false;
            if (RepeatIsEnbled)
            {
                Interface.Repeat();
            }
            else if (RandomIsEnbled)
            {

                Interface.Random();
            }
            else
            {
                Interface.Next();
            }
        }

        private void OpenFold_Click(object sender, RoutedEventArgs e)
        {
            var currentFile = Interface.Open();
            MusicList.ItemsSource = currentFile;
            if (currentFile.Count != 0) Interface.Play_M(0);
            
        }

        private void Sound_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Interface.player.Volume = Sound.Value / 100;
            Persents.Content = $"{(int)Sound.Value * 10}%";
        }

        private void MusicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            Interface.Play_M(MusicList.SelectedIndex);
        }

        private void SlideMusic_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Interface.player.Position = TimeSpan.FromSeconds(SlideMusic.Value);
        }

        private void Play_btn_Click(object sender, RoutedEventArgs e)
        {
            if (start)
            {
                lastSpan = (TimeSpan)Interface.player.Position;
                Interface.Stop();
                start = false;
            }
            else
            {
                Interface.Play_M(Interface.lastIndex, lastSpan);
                start = true;
            }
        }

        private void CycleMusic_Click(object sender, RoutedEventArgs e)
        {
            if (RepeatIsEnbled)
            {
                RepeatIsEnbled = false;
                CycleMusic.Background = new SolidColorBrush(Color.FromArgb(200, 103, 58, 183)); ;
                CycleMusic.BorderBrush = new SolidColorBrush(Color.FromArgb(200, 103, 58, 183)); ;
            }
            else
            {
                RepeatIsEnbled = true;
                CycleMusic.Background = new SolidColorBrush(Color.FromArgb(250, 196, 136, 232));
                CycleMusic.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 196, 136, 232));

            }
        }

        private void Random_btn_Click(object sender, RoutedEventArgs e)
        {
            if (RandomIsEnbled && !RepeatIsEnbled)
            {
                RandomIsEnbled = false;
                Random_btn.Background = new SolidColorBrush(Color.FromArgb(200, 103, 58, 183)); ;
                Random_btn.BorderBrush = new SolidColorBrush(Color.FromArgb(200, 103, 58, 183)); ;

            }
            else
            {
                RandomIsEnbled = true;

                Random_btn.Background = new SolidColorBrush(Color.FromArgb(250, 196, 136, 232));
                Random_btn.BorderBrush = new SolidColorBrush(Color.FromArgb(250, 196, 136, 232));
            }
        }

        private void LastMusic_Click(object sender, RoutedEventArgs e)
        {
            Interface.Back();
        }

        private void NextMusic_Click(object sender, RoutedEventArgs e)
        {
            Interface.Next();
        }
    }   
}
