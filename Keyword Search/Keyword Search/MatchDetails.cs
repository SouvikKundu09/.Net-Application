using System.Collections.Generic;

namespace KeywordSearch
{
  internal class MatchDetails
  {
    public string SearchedText { get; set; }

    public List<MatchedLinesDetails> MatchedLines { get; set; }

    public int MatchingLinesCount { get; set; }

    public int MatchingFilesCount { get; set; }

    public static int TotalFilesScanned { get; set; }
  }
}
