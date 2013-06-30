using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;

namespace PdfSharpMigraDocTestApp
{
  public abstract class BaseDocument
  {
    public Document Document { get; private set; }
    public Section CurrentSection { get; private set; }

    protected BaseDocument()
    {
      InitDocument();
    }

    protected void InitDocument()
    {
      Document = new Document();

      CurrentSection = Document.AddSection();
      CurrentSection.PageSetup.PageFormat = PageFormat.A4;
      CurrentSection.PageSetup.TopMargin = Unit.FromMillimeter(10);
      CurrentSection.PageSetup.LeftMargin = Unit.FromMillimeter(10);
      CurrentSection.PageSetup.RightMargin = Unit.FromMillimeter(10);
      CurrentSection.PageSetup.BottomMargin = Unit.FromMillimeter(10);
    }

    public abstract void DrawDocument();

  }
}