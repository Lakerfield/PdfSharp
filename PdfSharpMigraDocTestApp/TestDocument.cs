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
      var fb = CurrentSection.Footers.Primary.AddBarcode();
      fb.Type = BarcodeType.Barcode25i;
      fb.Text = true;
      fb.Code = "00248163264128";
      fb.Width = new Unit(8, UnitType.Centimeter);
      fb.Height = new Unit(1, UnitType.Centimeter);

      CurrentSection.AddParagraph("Interleaved 2 of 5: v4");

      var b2 = CurrentSection.Elements.AddBarcode();
      b2.Type = BarcodeType.Barcode25i;
      b2.Text = true;
      b2.Code = "01234567890123456789";
      b2.Width = new Unit(8, UnitType.Centimeter);
      b2.Height = new Unit(1, UnitType.Centimeter);
      //b2.BearerBars = true;
      //b2.LineHeight = 10;
      //b2.LineRatio = 2;

      //return;
      
      CurrentSection.AddParagraph("Code 128: v1");
      //CurrentSection.AddBarcode("123");


      var b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode128;
      b.Text = true;
      b.Code = "0123456789";
      b.Width = "5cm";
      b.Height = "1cm";
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Code 128: v2");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode128;
      b.Text = false;
      b.Code = "01234567899876543210";
      b.Width = new Unit(10, UnitType.Centimeter);
      b.Height = new Unit(0.5, UnitType.Centimeter);
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Code 128: v3");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode128;
      b.Text = false;
      b.Code = "01234567899876543210";
      b.Width = new Unit(5, UnitType.Centimeter);
      b.Height = new Unit(1, UnitType.Centimeter);
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Interleaved 2 of 5: v1");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode25i;
      b.Text = true;
      b.Code = "01234567890123456789";
      b.Width = "10cm";
      b.Height = "1cm";
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Interleaved 2 of 5: v2");
      CurrentSection.AddParagraph("Interleaved 2 of 5: v2");
      CurrentSection.AddParagraph("Interleaved 2 of 5: v2");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode25i;
      b.Text = true;
      b.Code = "01234567890123456789";
      b.Width = new Unit(10, UnitType.Centimeter);
      b.Height = new Unit(0.5, UnitType.Centimeter);
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Interleaved 2 of 5: v3");
      CurrentSection.AddParagraph("Interleaved 2 of 5: v3");
      CurrentSection.AddParagraph("Interleaved 2 of 5: v3");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode25i;
      b.Text = true;
      b.Code = "01234567890123456789";
      b.Width = new Unit(10, UnitType.Centimeter);
      b.Height = new Unit(1, UnitType.Centimeter);
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Interleaved 2 of 5: v4");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode25i;
      b.Text = true;
      b.Code = "01234567890123456789";
      b.Width = new Unit(8, UnitType.Centimeter);
      b.Height = new Unit(1, UnitType.Centimeter);
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Interleaved 2 of 5: v5");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode25i;
      //b.Text = true;
      b.Code = "01234567890123456789";
      b.Width = "6cm";
      b.Height = "1cm";
      //b.BearerBars = true;
      //b.LineHeight = 10;
      //b.LineRatio = 2;

      CurrentSection.AddParagraph("Code 39: v1");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode39;
      b.Text = true;
      b.Code = "9876543210";
      b.Width = "10cm";
      b.Height = "1cm";
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 2;

      CurrentSection.AddParagraph("Code 39: v2");

      b = CurrentSection.Elements.AddBarcode();
      b.Type = BarcodeType.Barcode39;
      b.Text = true;
      b.Code = "9876543210";
      b.Width = "10cm";
      b.Height = "1cm";
      b.BearerBars = true;
      b.LineHeight = 10;
      b.LineRatio = 4;

      CurrentSection.AddParagraph("Lorem ipsum");
    }
  }
}
