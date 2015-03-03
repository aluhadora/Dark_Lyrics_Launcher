using System.Collections.Generic;
using System.Xml;

namespace Dark_Lyrics_Launcher
{
  public static class ArtistsReader
  {
    public static IList<string> Artists()
    {
      var doc = new XmlDocument();
      doc.Load("Artists.xml");

      if (doc.DocumentElement == null) return new List<string>();

      var list = new List<string>();

      foreach (XmlNode node in doc.DocumentElement.ChildNodes)
      {
        if (node.Attributes == null) continue;
        list.Add(node.Attributes["name"].Value);
      }

      return list;
    }
  }
}
