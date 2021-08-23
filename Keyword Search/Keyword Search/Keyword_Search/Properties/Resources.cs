using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Keyword_Search.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Keyword_Search.Properties.Resources.resourceMan == null)
          Keyword_Search.Properties.Resources.resourceMan = new ResourceManager("Keyword_Search.Properties.Resources", typeof (Keyword_Search.Properties.Resources).Assembly);
        return Keyword_Search.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return Keyword_Search.Properties.Resources.resourceCulture;
      }
      set
      {
        Keyword_Search.Properties.Resources.resourceCulture = value;
      }
    }

    internal Resources()
    {
    }
  }
}
