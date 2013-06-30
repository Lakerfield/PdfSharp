using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;

namespace PdfSharpMigraDocTestApp
{
  public class TestDocument : BaseDocument
  {
    public override void DrawDocument()
    {
      CurrentSection.AddParagraph("Lorem ipsum");
      //CurrentSection.AddBarcode("123");

      var b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode25i;
      b.Text = true;
      b.Code = "0123456789";
      b.Width = "5cm";
      b.Height = "1cm";
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Lorem ipsum");
    }
  }
}
