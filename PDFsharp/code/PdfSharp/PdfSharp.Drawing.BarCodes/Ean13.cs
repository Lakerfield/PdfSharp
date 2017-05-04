#region PDFsharp - A .NET library for processing PDF
//
// Authors:
//   Sven Rymenants (mailto:sven.rymenants@gmail.com)
//
// http://www.pdfsharp.com / http://forum.pdfsharp.de/viewtopic.php?f=2&t=1524
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;

namespace PdfSharp.Drawing.BarCodes
{
  /// <summary>
  /// Implementation of the EAN13 bar code.
  /// </summary>
  public class Ean13 : BarCode
  {
    private XRect m_leftBlock = new XRect();
    private XRect m_rightBlock = new XRect();

    /// <summary>
    /// Initializes a new instance of EAN13.
    /// </summary>
    public Ean13()
      : base("", XSize.Empty, CodeDirection.LeftToRight)
    {
    }

    /// <summary>
    /// Initializes a new instance of EAN13.
    /// </summary>
    public Ean13(string code)
      : base(code, XSize.Empty, CodeDirection.LeftToRight)
    {
    }

    /// <summary>
    /// Initializes a new instance of EAN13.
    /// </summary>
    public Ean13(string code, XSize size)
      : base(code, size, CodeDirection.LeftToRight)
    {
    }

    /// <summary>
    /// Initializes a new instance of EAN13.
    /// </summary>
    public Ean13(string code, XSize size, CodeDirection direction)
      : base(code, size, direction)
    {
    }

    static bool[] Quite = new bool[] { false, false, false, false, false, false, false, false, false };
    static bool[] Leading = new bool[] { true, false, true };
    static bool[] Separator = new bool[] { false, true, false, true, false };

    static bool[][] OddLeftLines = new bool[][]
    {
      new bool[] { false, false, false, true, true, false, true },
      new bool[] { false, false, true, true, false, false, true },
      new bool[] { false, false, true, false, false, true, true },
      new bool[] { false, true, true, true, true, false, true },
      new bool[] { false, true, false, false, false, true, true },
      new bool[] { false, true, true, false, false, false, true },
      new bool[] { false, true, false, true, true, true, true },
      new bool[] { false, true, true, true, false, true, true },
      new bool[] { false, true, true, false, true, true, true },
      new bool[] { false, false, false, true, false, true, true }
    };

    static bool[][] EvenLeftLines = new bool[][]
    {
      new bool[] { false, true, false, false, true, true, true },
      new bool[] { false, true, true, false, false, true, true },
      new bool[] { false, false, true, true, false, true, true },
      new bool[] { false, true, false, false, false, false, true },
      new bool[] { false, false, true, true, true, false, true },
      new bool[] { false, true, true, true, false, false, true },
      new bool[] { false, false, false, false, true, false, true },
      new bool[] { false, false, true, false, false, false, true },
      new bool[] { false, false, false, true, false, false, true },
      new bool[] { false, false, true, false, true, true, true }
    };

    static bool[][] RightLines = new bool[][]
    {
      new bool[] { true, true, true, false, false, true, false},
      new bool[] { true, true, false, false, true, true, false},
      new bool[] { true, true, false, true, true, false, false},
      new bool[] { true, false, false, false, false, true, false},
      new bool[] { true, false, true, true, true, false, false},
      new bool[] { true, false, false, true, true, true, false},
      new bool[] { true, false, true, false, false, false, false},
      new bool[] { true, false, false, false, true, false, false},
      new bool[] { true, false, false, true, false, false, false},
      new bool[] { true, true, true, false, true, false, false}
    };

    /// <summary>
    /// Renders the bar code.
    /// </summary>
    protected internal override void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position)
    {
      XGraphicsState state = gfx.Save();

      BarCodeRenderInfo info = new BarCodeRenderInfo(gfx, brush, font, position);
      InitRendering(info);
      info.CurrPosInString = 0;
      info.CurrPos = position - CodeBase.CalcDistance(AnchorType.TopLeft, this.anchor, this.size);

      //   EAN13 Barcode should be a total of 113 modules wide.
      int numberOfBars = 12; // The length - country code
      numberOfBars *= 7; // Each character has 7 bars
      numberOfBars += 2 * (Quite.Length + Leading.Length);
      numberOfBars += Separator.Length;
      info.ThinBarWidth = ((double)this.Size.Width / (double)numberOfBars);

      RenderStart(info);

      m_leftBlock.x = info.CurrPos.x + info.ThinBarWidth / 2;
      RenderLeft(info);
      m_leftBlock.Width = info.CurrPos.x - m_leftBlock.x;

      RenderSeparator(info);

      m_rightBlock.x = info.CurrPos.x;
      RenderRight(info);
      m_rightBlock.Width = info.CurrPos.x - m_rightBlock.x - info.ThinBarWidth / 2;

      RenderStop(info);

      if (this.TextLocation == TextLocation.BelowEmbedded)
        RenderText(info);

      gfx.Restore(state);
    }

    private void RenderStart(BarCodeRenderInfo info)
    {
      RenderValue(info, Quite);
      RenderValue(info, Leading);
    }

    private void RenderLeft(BarCodeRenderInfo info)
    {
      int country = (int)(this.text[0] - '0');
      string text = this.text.Substring(1, 6);

      switch (country)
      {
        case 0:
          foreach (char ch in text)
            RenderDigit(info, ch, OddLeftLines);
          break;

        case 1:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], OddLeftLines);
          RenderDigit(info, text[2], EvenLeftLines);
          RenderDigit(info, text[3], OddLeftLines);
          RenderDigit(info, text[4], EvenLeftLines);
          RenderDigit(info, text[5], EvenLeftLines);
          break;

        case 2:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], OddLeftLines);
          RenderDigit(info, text[2], EvenLeftLines);
          RenderDigit(info, text[3], EvenLeftLines);
          RenderDigit(info, text[4], OddLeftLines);
          RenderDigit(info, text[5], EvenLeftLines);
          break;

        case 3:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], OddLeftLines);
          RenderDigit(info, text[2], EvenLeftLines);
          RenderDigit(info, text[3], EvenLeftLines);
          RenderDigit(info, text[4], EvenLeftLines);
          RenderDigit(info, text[5], OddLeftLines);
          break;

        case 4:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], EvenLeftLines);
          RenderDigit(info, text[2], OddLeftLines);
          RenderDigit(info, text[3], OddLeftLines);
          RenderDigit(info, text[4], EvenLeftLines);
          RenderDigit(info, text[5], EvenLeftLines);
          break;

        case 5:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], EvenLeftLines);
          RenderDigit(info, text[2], EvenLeftLines);
          RenderDigit(info, text[3], OddLeftLines);
          RenderDigit(info, text[4], OddLeftLines);
          RenderDigit(info, text[5], EvenLeftLines);
          break;

        case 6:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], EvenLeftLines);
          RenderDigit(info, text[2], EvenLeftLines);
          RenderDigit(info, text[3], EvenLeftLines);
          RenderDigit(info, text[4], OddLeftLines);
          RenderDigit(info, text[5], OddLeftLines);
          break;

        case 7:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], EvenLeftLines);
          RenderDigit(info, text[2], OddLeftLines);
          RenderDigit(info, text[3], EvenLeftLines);
          RenderDigit(info, text[4], OddLeftLines);
          RenderDigit(info, text[5], EvenLeftLines);
          break;

        case 8:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], EvenLeftLines);
          RenderDigit(info, text[2], OddLeftLines);
          RenderDigit(info, text[3], EvenLeftLines);
          RenderDigit(info, text[4], EvenLeftLines);
          RenderDigit(info, text[5], OddLeftLines);
          break;

        case 9:
          RenderDigit(info, text[0], OddLeftLines);
          RenderDigit(info, text[1], EvenLeftLines);
          RenderDigit(info, text[2], EvenLeftLines);
          RenderDigit(info, text[3], OddLeftLines);
          RenderDigit(info, text[4], EvenLeftLines);
          RenderDigit(info, text[5], OddLeftLines);
          break;
      }
    }

    private void RenderDigit(BarCodeRenderInfo info, char digit, bool[][] lines)
    {
      int index = digit - '0';
      RenderValue(info, lines[index]);
    }

    private void RenderSeparator(BarCodeRenderInfo info)
    {
      RenderValue(info, Separator);
    }

    private void RenderRight(BarCodeRenderInfo info)
    {
      string text = this.text.Substring(7);

      if (this.text.Length == 12)
        text += CalculateChecksumDigit(this.text);

      foreach (char ch in text)
        RenderDigit(info, ch, RightLines);
    }

    private void RenderStop(BarCodeRenderInfo info)
    {
      RenderValue(info, Leading);
      RenderValue(info, Quite);
    }

    private void RenderValue(BarCodeRenderInfo info, bool[] value)
    {
      foreach (bool bar in value)
      {
        if (bar)
        {
          XRect rect = new XRect(info.CurrPos.X, info.CurrPos.Y, info.ThinBarWidth, Size.Height);
          info.Gfx.DrawRectangle(info.Brush, rect);
        }
        info.CurrPos.X += info.ThinBarWidth;
      }
    }

    private void RenderText(BarCodeRenderInfo info)
    {
      if (info.Font == null)
        info.Font = new XFont("Courier New", Size.Height / 4);
      XPoint center = info.Position + CodeBase.CalcDistance(this.anchor, AnchorType.TopLeft, this.size);
      XSize textSize = info.Gfx.MeasureString(this.text, info.Font);

      double height = textSize.height;
      double y = info.Position.y + Size.Height - textSize.height;

      m_leftBlock.Height = height;
      m_leftBlock.y = y;

      m_rightBlock.Height = height;
      m_rightBlock.y = y;

      XPoint pos = new XPoint(info.Position.x, y);
      info.Gfx.DrawString(this.text.Substring(0, 1), info.Font, info.Brush, new XRect(pos, Size), XStringFormats.TopLeft);

      info.Gfx.DrawRectangle(XBrushes.White, m_leftBlock);
      info.Gfx.DrawString(this.text.Substring(1, 6), info.Font, info.Brush, m_leftBlock, XStringFormats.TopCenter);

      info.Gfx.DrawRectangle(XBrushes.White, m_rightBlock);

      string text = this.text.Substring(7);
      if (this.text.Length == 12)
        text += CalculateChecksumDigit(this.text);

      info.Gfx.DrawString(text, info.Font, info.Brush, m_rightBlock, XStringFormats.TopCenter);
    }

    private string CalculateChecksumDigit(string text)
    {
      bool odd = false;
      int sum = 0;
      foreach (char ch in text)
      {
        sum += (ch - '0') * (odd ? 3 : 1);
        odd = !odd;
      }
      int result = (10 - (sum % 10)) % 10;
      return result.ToString();
    }

    /// <summary>Validates the text string to be coded</summary>
    /// <param name="text">String - The text string to be coded</param>
    protected override void CheckCode(string text)
    {
      if (text == null) throw new ArgumentNullException("Parameter text (string) can not be null");
      if (text.Length != 12) throw new ArgumentException("Parameter text (string) can not have more or less than 12 characters");
    }
  }
}

