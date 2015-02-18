using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Dark_Lyrics_Launcher
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow
  {
    public MainWindow()
    {
      InitializeComponent();

      Icon = Imaging.CreateBitmapSourceFromHBitmap(Properties.Resources.DarklyricsFavicon1.GetHbitmap(), IntPtr.Zero,
        Int32Rect.Empty,
        BitmapSizeOptions.FromEmptyOptions());
    }

    private string Artist { get { return ArtistComboBox.Text; } }
    private string Song { get { return SongComboBox.Text; } }

    private void ArtistComboBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (Artist == string.Empty)
      {
        UrlLabel.Content = string.Empty;
        SongComboBox.IsEnabled = false;
        return;
      }
      if (e.Key == Key.Enter)
      {
        if (SongComboBox.IsEnabled)
        {
          SongComboBox.Focus();
        }
      }
    }

    private void ArtistComboBox_TextInput(object sender, TextCompositionEventArgs e)
    {

    }

    private string _lastArtist;
    private void ArtistComboBox_KeyUp(object sender, KeyEventArgs e)
    {
      if (_lastArtist == Artist) return;
      _lastArtist = Artist;

      if (Artist == string.Empty)
      {
        UrlLabel.Content = string.Empty;
        SongComboBox.IsEnabled = false;
        return;
      }
      if (Song == string.Empty)
      {
        UrlLabel.Content = GenerateArtistUrl();
        if (ArtistComboBox.Items.Cast<ListBoxItem>().Any(x => (string)x.Content == Artist))
        {
          StartSongBoxFill();
        }
      }
    }

    private string _lastSong;
    private void SongComboBox_KeyUp(object sender, KeyEventArgs e)
    {
      if (_lastSong == Song) return;
      _lastSong = Song;

      if (Song == string.Empty)
      {
        UrlLabel.Content = GenerateArtistUrl();
        return;
      }

      var songs = SongComboBox.Items.Cast<Tuple<string, string>>();

      var songName = Song.Trim();
      var song = songs.FirstOrDefault(x => x.Item2 == songName);
      if (song == null)
      {
        UrlLabel.Content = string.Empty;
        return;
      }

      UrlLabel.Content = song.Item1;
    }

    private string GenerateArtistUrl()
    {
      return WebUtil.GenerateArtistUrl(Artist);
    }

    private void SongComboBox_GotFocus(object sender, RoutedEventArgs e)
    {
      StartSongBoxFill();
    }

    private void StartSongBoxFill()
    {
      SongComboBox.DisplayMemberPath = "Item2";
      var songs = WebUtil.ArtistSongs(GenerateArtistUrl());
      
      SongComboBox.ItemsSource = songs;

      SongComboBox.IsEnabled = true;
    }

    private void SongComboBox_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter) return;
      if ((string)UrlLabel.Content == string.Empty) return;

      Process.Start(UrlLabel.Content.ToString());
      Close();
    }


  }
}
