using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace Dark_Lyrics_Launcher
{
  public class WebUtil
  {
    private static WebUtil _instance;
    private readonly WebClient _client;

    private WebUtil()
    {
      _client = new WebClient
      {
        Proxy = null,
      };
    }

    private static WebUtil Instance()
    {
      return _instance ?? (_instance = new WebUtil());
    }

    public static string GenerateArtistUrl(string artist)
    {
      var artistString = artist.Trim().Replace(" ", string.Empty).ToLower();
      return string.Format("http://www.darklyrics.com/{0}/{1}.html", artistString.Substring(0, 1), artistString);
    }

    public static IList<Tuple<string, string>> ArtistSongs(string url)
    {
      return Instance().ArtistSongsFromInstance(url);
    }

    private IList<Tuple<string, string>> ArtistSongsFromInstance(string url)
    {
      var doc = new HtmlDocument();
      doc.LoadHtml(_client.DownloadString(new Uri(url)));

      var songs = new List<Tuple<string, string>>();

      foreach (var album in doc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("album")))
      {
        foreach (var songLink in album.Descendants("a"))
        {
          var text = songLink.InnerText;
          var link = "www.darklyrics.com";
          link += songLink.Attributes["href"].Value.Trim('.');

          var item = Tuple.Create(link, text);

          songs.Add(item);
        }
      }

      return songs;
    }
  }
}
