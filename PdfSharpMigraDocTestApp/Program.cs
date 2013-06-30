using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfSharpMigraDocTestApp
{
  class Program
  {
    static void Main(string[] args)
    {
      KillAcroRd32();

      var doc = new TestDocument();

      GenerateAndOpen(doc);
    }

    private static void KillAcroRd32()
    {
      const string program = "AcroRd32";
      var processes = Process.GetProcessesByName(program);
      foreach (var process in processes)
        process.CloseMainWindow();// Kill();
    }

    private static void GenerateAndOpen(BaseDocument document)
    {
      var pdfStream = new MemoryStream();
      try
      {
        RenderPdfToStream(pdfStream, document);

        // Reset stream
        pdfStream.Position = 0;
        var documentPath = new FileInfo("test.pdf");
        File.WriteAllBytes(documentPath.FullName, pdfStream.ToArray());

        //Start the process 
        Process proc = Process.Start(documentPath.FullName);
      }
      finally
      {
        pdfStream.Dispose();
      }
    }

    internal static void RenderPdfToStream(MemoryStream streamToWriteTo, BaseDocument document)
    {
      document.DrawDocument();

      var printer = new MigraDoc.Rendering.PdfDocumentRenderer();
      printer.Document = document.Document;
      printer.RenderDocument();
      printer.PdfDocument.Save(streamToWriteTo, false);
    }
  }
}
