using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;

namespace PdfSharpMigraDocTestApp
{
  public class TestDocument : BaseDocument
  {
    public override void DrawDocument()
    {
      CurrentSection.AddParagraph("Lorem ipsum");
      //CurrentSection.AddBarcode("123");
      CurrentSection.AddParagraph("Lorem ipsum");
    }
  }
}
