using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Markup;
using System.Diagnostics;
using System.Windows;
using System.Runtime.CompilerServices;

namespace MusicPlayer
{
    internal class Interface
    {
        public static List<string> currentFiles = new List<string>();
        public static MediaPlayer player = new MediaPlayer();
        public static int lastIndex;

        public static List<string> Open()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {
                string[] files = Directory.GetFiles(dialog.FileName);
                List<string> currentPaths = new List<string>();
                List<string> currentNames = new List<string>();


                foreach (string file in files)
                {
                    string ext = System.IO.Path.GetExtension(file);
                    if (ext == ".mp3")
                    {
                        currentPaths.Add(file);
                        string[] name = file.Split(new string[] { "\\" }, StringSplitOptions.None);
                        currentNames.Add(name[name.Length - 1]);
                    }
                }
                currentFiles = currentPaths;
                return currentNames;
            }
            return null;
        }

        public static void Play(int ind, TimeSpan? PositionTimeMedia = null)
        {
            lastIndex = ind;
            player.Open(new Uri(currentFiles[ind].Replace(@"\", @"\\").ToString(), UriKind.Absolute));
            player.Position = (TimeSpan)(PositionTimeMedia == null ? TimeSpan.Zero : PositionTimeMedia);
            player.Play();
            Debug.WriteLine(currentFiles[ind].Replace(@"\", @"\\"));
        }

        public static void Repeat()
        {
            Play(lastIndex);
        }

        public static void Next(int ind)
        {
            Play(ind);
        }

        public static void Random()
        {
            Random rnd = new Random();
            int ind = rnd.Next(0, currentFiles.Count - 1);
            Play(ind);
        }

        public static void Stop()
        {
            player.Stop();
        }
    }
}
