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
                if (currentPaths.Count != 0)
                {
                    currentFiles = currentPaths;
                    return currentNames;
                }

            }
            return null;
        }

        public static void Play_M(int ind, TimeSpan? PositionTimeMedia = null)
        {
            try
            {
                lastIndex = ind;
                player.Open(new Uri(currentFiles[ind].Replace(@"\", @"\\").ToString(), UriKind.Absolute));
                player.Position = (TimeSpan)(PositionTimeMedia == null ? TimeSpan.Zero : PositionTimeMedia);
                player.Play();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public static void Repeat()
        {
            Play_M(lastIndex, TimeSpan.Zero);
        }

        public static void Next()
        {
            int ind = lastIndex + 1;
            if (ind <= currentFiles.Count - 1)
            {
                Debug.WriteLine(ind);
                Play_M(ind, TimeSpan.Zero);
            }
            
        }

        public static void Back()
        {
            int ind = lastIndex - 1;

            if (ind >= 0)
            {
                Debug.WriteLine(ind);
                Play_M(ind, TimeSpan.Zero);
            }
                
        }

        public static void Random()
        {
            Random rnd = new Random();
            int ind = rnd.Next(0, currentFiles.Count - 1);
            Debug.WriteLine(currentFiles[ind].Replace(@"\", @"\\"));
            Debug.WriteLine(ind);
            Play_M(ind, TimeSpan.Zero);
        }

        public static void Stop()
        {
            player.Stop();
        }
    }
}
