namespace PdfSharp.Drawing.BarCodes
{
    using System;
    using System.Collections.Generic;
    /// <summary>
    /// A Class to be able to render a Code 128 bar code
    /// </summary>
    /// <remarks>
    /// For a more detailed explanation of the Code 128, please visit the following web site: http://www.barcodeman.com/info/c128.php3 or http://www.adams1.com/128code.html
    /// </remarks>
    public class Code128 : BarCode
    {
        #region Constants
        #region private
        /// <summary>
        /// The cod e 128_ stopcode.
        /// </summary>
        private const int CODE128_STOPCODE = 106;
        #endregion
        #endregion
        #region Fields
        #region private
        /// <summary>
        /// The code 128 code.
        /// </summary>
        private Code128Type code128Code;
        private Code128Type Code128Code
        {
            get { return this.code128Code; }
            set
            {
                this.code128Code = value;
                this.CheckTypeC(this.Text);
            }
        }
        /// <summary>
        /// The values.
        /// </summary>
        private Byte[] Values;
        #endregion
        #region public
        /// <summary>
        /// A static place holder for the patterns to draw the code 128 barcode
        /// </summary>
        public static readonly Dictionary<Byte, Byte[]> Patterns = new Dictionary<Byte, Byte[]>
        {
            { 0, new Byte[] { 2, 1, 2, 2, 2, 2 } },
            { 1, new Byte[] { 2, 2, 2, 1, 2, 2 } },
            { 2, new Byte[] { 2, 2, 2, 2, 2, 1 } },
            { 3, new Byte[] { 1, 2, 1, 2, 2, 3 } },
            { 4, new Byte[] { 1, 2, 1, 3, 2, 2 } },
            { 5, new Byte[] { 1, 3, 1, 2, 2, 2 } },
            { 6, new Byte[] { 1, 2, 2, 2, 1, 3 } },
            { 7, new Byte[] { 1, 2, 2, 3, 1, 2 } },
            { 8, new Byte[] { 1, 3, 2, 2, 1, 2 } },
            { 9, new Byte[] { 2, 2, 1, 2, 1, 3 } },
            { 10, new Byte[] { 2, 2, 1, 3, 1, 2 } },
            { 11, new Byte[] { 2, 3, 1, 2, 1, 2 } },
            { 12, new Byte[] { 1, 1, 2, 2, 3, 2 } },
            { 13, new Byte[] { 1, 2, 2, 1, 3, 2 } },
            { 14, new Byte[] { 1, 2, 2, 2, 3, 1 } },
            { 15, new Byte[] { 1, 1, 3, 2, 2, 2 } },
            { 16, new Byte[] { 1, 2, 3, 1, 2, 2 } },
            { 17, new Byte[] { 1, 2, 3, 2, 2, 1 } },
            { 18, new Byte[] { 2, 2, 3, 2, 1, 1 } },
            { 19, new Byte[] { 2, 2, 1, 1, 3, 2 } },
            { 20, new Byte[] { 2, 2, 1, 2, 3, 1 } },
            { 21, new Byte[] { 2, 1, 3, 2, 1, 2 } },
            { 22, new Byte[] { 2, 2, 3, 1, 1, 2 } },
            { 23, new Byte[] { 3, 1, 2, 1, 3, 1 } },
            { 24, new Byte[] { 3, 1, 1, 2, 2, 2 } },
            { 25, new Byte[] { 3, 2, 1, 1, 2, 2 } },
            { 26, new Byte[] { 3, 2, 1, 2, 2, 1 } },
            { 27, new Byte[] { 3, 1, 2, 2, 1, 2 } },
            { 28, new Byte[] { 3, 2, 2, 1, 1, 2 } },
            { 29, new Byte[] { 3, 2, 2, 2, 1, 1 } },
            { 30, new Byte[] { 2, 1, 2, 1, 2, 3 } },
            { 31, new Byte[] { 2, 1, 2, 3, 2, 1 } },
            { 32, new Byte[] { 2, 3, 2, 1, 2, 1 } },
            { 33, new Byte[] { 1, 1, 1, 3, 2, 3 } },
            { 34, new Byte[] { 1, 3, 1, 1, 2, 3 } },
            { 35, new Byte[] { 1, 3, 1, 3, 2, 1 } },
            { 36, new Byte[] { 1, 1, 2, 3, 1, 3 } },
            { 37, new Byte[] { 1, 3, 2, 1, 1, 3 } },
            { 38, new Byte[] { 1, 3, 2, 3, 1, 1 } },
            { 39, new Byte[] { 2, 1, 1, 3, 1, 3 } },
            { 40, new Byte[] { 2, 3, 1, 1, 1, 3 } },
            { 41, new Byte[] { 2, 3, 1, 3, 1, 1 } },
            { 42, new Byte[] { 1, 1, 2, 1, 3, 3 } },
            { 43, new Byte[] { 1, 1, 2, 3, 3, 1 } },
            { 44, new Byte[] { 1, 3, 2, 1, 3, 1 } },
            { 45, new Byte[] { 1, 1, 3, 1, 2, 3 } },
            { 46, new Byte[] { 1, 1, 3, 3, 2, 1 } },
            { 47, new Byte[] { 1, 3, 3, 1, 2, 1 } },
            { 48, new Byte[] { 3, 1, 3, 1, 2, 1 } },
            { 49, new Byte[] { 2, 1, 1, 3, 3, 1 } },
            { 50, new Byte[] { 2, 3, 1, 1, 3, 1 } },
            { 51, new Byte[] { 2, 1, 3, 1, 1, 3 } },
            { 52, new Byte[] { 2, 1, 3, 3, 1, 1 } },
            { 53, new Byte[] { 2, 1, 3, 1, 3, 1 } },
            { 54, new Byte[] { 3, 1, 1, 1, 2, 3 } },
            { 55, new Byte[] { 3, 1, 1, 3, 2, 1 } },
            { 56, new Byte[] { 3, 3, 1, 1, 2, 1 } },
            { 57, new Byte[] { 3, 1, 2, 1, 1, 3 } },
            { 58, new Byte[] { 3, 1, 2, 3, 1, 1 } },
            { 59, new Byte[] { 3, 3, 2, 1, 1, 1 } },
            { 60, new Byte[] { 3, 1, 4, 1, 1, 1 } },
            { 61, new Byte[] { 2, 2, 1, 4, 1, 1 } },
            { 62, new Byte[] { 4, 3, 1, 1, 1, 1 } },
            { 63, new Byte[] { 1, 1, 1, 2, 2, 4 } },
            { 64, new Byte[] { 1, 1, 1, 4, 2, 2 } },
            { 65, new Byte[] { 1, 2, 1, 1, 2, 4 } },
            { 66, new Byte[] { 1, 2, 1, 4, 2, 1 } },
            { 67, new Byte[] { 1, 4, 1, 1, 2, 2 } },
            { 68, new Byte[] { 1, 4, 1, 2, 2, 1 } },
            { 69, new Byte[] { 1, 1, 2, 2, 1, 4 } },
            { 70, new Byte[] { 1, 1, 2, 4, 1, 2 } },
            { 71, new Byte[] { 1, 2, 2, 1, 1, 4 } },
            { 72, new Byte[] { 1, 2, 2, 4, 1, 1 } },
            { 73, new Byte[] { 1, 4, 2, 1, 1, 2 } },
            { 74, new Byte[] { 1, 4, 2, 2, 1, 1 } },
            { 75, new Byte[] { 2, 4, 1, 2, 1, 1 } },
            { 76, new Byte[] { 2, 2, 1, 1, 1, 4 } },
            { 77, new Byte[] { 4, 1, 3, 1, 1, 1 } },
            { 78, new Byte[] { 2, 4, 1, 1, 1, 2 } },
            { 79, new Byte[] { 1, 3, 4, 1, 1, 1 } },
            { 80, new Byte[] { 1, 1, 1, 2, 4, 2 } },
            { 81, new Byte[] { 1, 2, 1, 1, 4, 2 } },
            { 82, new Byte[] { 1, 2, 1, 2, 4, 1 } },
            { 83, new Byte[] { 1, 1, 4, 2, 1, 2 } },
            { 84, new Byte[] { 1, 2, 4, 1, 1, 2 } },
            { 85, new Byte[] { 1, 2, 4, 2, 1, 1 } },
            { 86, new Byte[] { 4, 1, 1, 2, 1, 2 } },
            { 87, new Byte[] { 4, 2, 1, 1, 1, 2 } },
            { 88, new Byte[] { 4, 2, 1, 2, 1, 1 } },
            { 89, new Byte[] { 2, 1, 2, 1, 4, 1 } },
            { 90, new Byte[] { 2, 1, 4, 1, 2, 1 } },
            { 91, new Byte[] { 4, 1, 2, 1, 2, 1 } },
            { 92, new Byte[] { 1, 1, 1, 1, 4, 3 } },
            { 93, new Byte[] { 1, 1, 1, 3, 4, 1 } },
            { 94, new Byte[] { 1, 3, 1, 1, 4, 1 } },
            { 95, new Byte[] { 1, 1, 4, 1, 1, 3 } },
            { 96, new Byte[] { 1, 1, 4, 3, 1, 1 } },
            { 97, new Byte[] { 4, 1, 1, 1, 1, 3 } },
            { 98, new Byte[] { 4, 1, 1, 3, 1, 1 } },
            { 99, new Byte[] { 1, 1, 3, 1, 4, 1 } },
            { 100, new Byte[] { 1, 1, 4, 1, 3, 1 } },
            { 101, new Byte[] { 3, 1, 1, 1, 4, 1 } },
            { 102, new Byte[] { 4, 1, 1, 1, 3, 1 } },
            { 103, new Byte[] { 2, 1, 1, 4, 1, 2 } },
            { 104, new Byte[] { 2, 1, 1, 2, 1, 4 } },
            { 105, new Byte[] { 2, 1, 1, 2, 3, 2 } },
            { 106, new Byte[] { 2, 3, 3, 1, 1, 1, 2 } }
        };

        #endregion
        #endregion
        #region Methods
        #region private
        /// <summary>
        /// The calculate parity.
        /// </summary>
        /// <returns>
        /// The calculate parity.
        /// </returns>
        private int CalculateParity()
        {
            long parityValue = (int)this.Code128Code;
            for (int x = 1; x <= this.Values.Length; x++)
                parityValue += (this.Values[x - 1]) * x;
            parityValue %= 103;
            return (int)parityValue;
        }
        /// <summary>
        /// The get pattern.
        /// </summary>
        /// <param name="codeValue">
        /// The code value.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        private byte[] GetPattern(int codeValue)
        {
            if (codeValue < 0)
                throw new ArgumentOutOfRangeException("Parameter ch (int) can not be less than 32 (space).");
            if (codeValue > 106)
                throw new ArgumentOutOfRangeException("Parameter ch (int) can not be greater than 138.");
            return Patterns[(byte)codeValue];
        }
        /// <summary>
        /// Renders a single line of the character. Each character has three lines and three spaces
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="barWidth">
        /// Indicates the thickness of the line/bar to be rendered. 
        /// </param>
        /// <param name="brush">
        /// Indicates the brush to use to render the line/bar. 
        /// </param>
        private void RenderBar(BarCodeRenderInfo info, double barWidth, XBrush brush)
        {
            double height = this.Size.Height;
            double yPos = info.CurrPos.Y;
            switch (this.TextLocation)
            {
                case TextLocation.Above:
                    yPos = info.CurrPos.Y + (height / 5);
                    height *= 4.0 / 5;
                    break;
                case TextLocation.Below:
                    height *= 4.0 / 5;
                    break;
                case TextLocation.AboveEmbedded:
                case TextLocation.BelowEmbedded:
                case TextLocation.None:
                    break;
            }
            XRect rect = new XRect(info.CurrPos.X, yPos, barWidth, height);
            info.Gfx.DrawRectangle(brush, rect);
            info.CurrPos.X += barWidth;
        }
        /// <summary>
        /// The render start.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        private void RenderStart(BarCodeRenderInfo info) { this.RenderValue(info, (int)this.Code128Code); }
        /// <summary>
        /// The render stop.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        private void RenderStop(BarCodeRenderInfo info)
        {
            this.RenderValue(info, this.CalculateParity());
            this.RenderValue(info, CODE128_STOPCODE);
        }
        /// <summary>
        /// The render text.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        private void RenderText(BarCodeRenderInfo info)
        {
            if (info.Font == null)
                info.Font = new XFont("Courier New", this.Size.Height / 6);
            XPoint center = info.Position + CodeBase.CalcDistance(this.anchor, AnchorType.TopLeft, this.size);
            if (this.TextLocation == TextLocation.Above)
                info.Gfx.DrawString(this.text, info.Font, info.Brush, new XRect(center, this.Size), XStringFormats.TopCenter);
            else if (this.TextLocation == TextLocation.AboveEmbedded)
            {
                XSize textSize = info.Gfx.MeasureString(this.text, info.Font);
                textSize.Width += this.Size.Width * .15;
                XPoint point = info.Position;
                point.X += (this.Size.Width - textSize.Width) / 2;
                XRect rect = new XRect(point, textSize);
                info.Gfx.DrawRectangle(XBrushes.White, rect);
                info.Gfx.DrawString(this.text, info.Font, info.Brush, new XRect(center, this.Size), XStringFormats.TopCenter);
            }
            else if (this.TextLocation == TextLocation.Below)
                info.Gfx.DrawString(this.text, info.Font, info.Brush, new XRect(center, this.Size), XStringFormats.BottomCenter);
            else if (this.TextLocation == TextLocation.BelowEmbedded)
            {
                XSize textSize = info.Gfx.MeasureString(this.text, info.Font);
                textSize.Width += this.Size.Width * .15;
                XPoint point = info.Position;
                point.X += (this.Size.Width - textSize.Width) / 2;
                point.Y += this.Size.Height - textSize.height;
                XRect rect = new XRect(point, textSize);
                info.Gfx.DrawRectangle(XBrushes.White, rect);
                info.Gfx.DrawString(this.text, info.Font, info.Brush, new XRect(center, this.Size), XStringFormats.BottomCenter);
            }
        }
        /// <summary>
        /// The render value.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <param name="chVal">
        /// The ch val.
        /// </param>
        private void RenderValue(BarCodeRenderInfo info, int chVal)
        {
            byte[] pattern = this.GetPattern(chVal);
            XBrush space = XBrushes.White;
            for (int idx = 0; idx < pattern.Length; idx++)
                if ((idx % 2) == 0)
                    this.RenderBar(info, info.ThinBarWidth * pattern[idx]);
                else
                    this.RenderBar(info, info.ThinBarWidth * pattern[idx], space);
        }
        #endregion
        #region internal
        /// <summary>
        /// The init rendering.
        /// </summary>
        /// <param name="info">
        /// The info.
        /// </param>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        internal override void InitRendering(BarCodeRenderInfo info)
        {
            if (this.Values == null)
                throw new InvalidOperationException(BcgSR.BarCodeNotSet);
            if (this.Values.Length == 0)
                throw new InvalidOperationException(BcgSR.EmptyBarCodeSize);
            int numberOfBars = this.Values.Length + 3; // The length of the string plus the start, stop, and parity value
            numberOfBars *= 11; // Each character has 11 bars
            numberOfBars += 2; // Add two more because the stop bit has two extra bars
            // Calculating the width of a bar
            info.ThinBarWidth = this.Size.Width / numberOfBars;
        }
        /// <summary>
        /// Renders a single line of the character. Each character has three lines and three spaces
        /// </summary>
        /// <param name="info">
        /// </param>
        /// <param name="barWidth">
        /// Indicates the thickness of the line/bar to be rendered. 
        /// </param>
        internal void RenderBar(BarCodeRenderInfo info, double barWidth) { this.RenderBar(info, barWidth, info.Brush); }
        #endregion
        #region protected internal
        /// <summary>
        /// Renders the content found in Text
        /// </summary>
        /// <param name="gfx">
        /// XGraphics - Instance of the drawing surface 
        /// </param>
        /// <param name="brush">
        /// XBrush - Line and Color to draw the bar code 
        /// </param>
        /// <param name="font">
        /// XFont - Font to use to draw the text string 
        /// </param>
        /// <param name="position">
        /// XPoint - Location to render the bar code 
        /// </param>
        protected internal override void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position)
        {
            // Create the array to hold the values to be rendered
            this.Values = this.Code128Code == Code128Type.C ? new byte[this.text.Length / 2] : new byte[this.text.Length];
            String buffer = String.Empty;
            for (Int32 index = 0; index < text.Length; index++)
                switch (this.Code128Code)
                {
                    case Code128Type.A:
                        if (text[index] < 32)
                            this.Values[index] = (byte)(text[index] + 64);
                        else if ((text[index] >= 32) && (text[index] < 64))
                            this.Values[index] = (byte)(text[index] - 32);
                        else
                            this.Values[index] = (byte)text[index];
                        break;
                    case Code128Type.B:
                        this.Values[index] = (byte)(text[index] - 32);
                        break;
                    case Code128Type.C:
                        if ((text[index] >= '0') && (text[index] <= '9'))
                        {
                            buffer += text[index];
                            if (buffer.Length == 2)
                            {
                                this.Values[index / 2] = byte.Parse(buffer);
                                buffer = String.Empty;
                            }
                        }
                        else
                            throw new ArgumentOutOfRangeException("Parameter text (string) can only contain numeric characters for Code 128 - Code C");
                        break;
                }
            if (this.Values == null)
                throw new InvalidOperationException("Text or Values must be set");
            if (this.Values.Length == 0)
                throw new InvalidOperationException("Text or Values must have content");
            for (int x = 0; x < this.Values.Length; x++)
                if (this.Values[x] > 102)
                    throw new ArgumentOutOfRangeException(BcgSR.InvalidCode128(x));
            XGraphicsState state = gfx.Save();
            BarCodeRenderInfo info = new BarCodeRenderInfo(gfx, brush, font, position);
            this.InitRendering(info);
            info.CurrPosInString = 0;
            info.CurrPos = position - CodeBase.CalcDistance(AnchorType.TopLeft, this.anchor, this.size);
            this.RenderStart(info);
            foreach (byte c in this.Values)
                this.RenderValue(info, c);
            this.RenderStop(info);
            if (this.TextLocation != TextLocation.None)
                this.RenderText(info);
            gfx.Restore(state);
        }
        #endregion
        #region protected
        /// <summary>
        /// Validates the text string to be coded
        /// </summary>
        /// <param name="text">
        /// String - The text string to be coded 
        /// </param>
        protected override void CheckCode(String text)
        {
            if (text == null)
                throw new ArgumentNullException("Parameter text (string) can not be null");
            if (text.Length == 0)
                throw new ArgumentException("Parameter text (string) can not be empty");
            this.CheckTypeC(text);
        }
        #endregion
        #endregion
        #region Constructors
        #region public
        /// <summary>
        /// Initializes a new instance of the <see cref="Code128"/> class. 
        /// Constructor
        /// </summary>
        public Code128()
            : base("", XSize.Empty, CodeDirection.LeftToRight)
        {
            this.code128Code = Code128Type.B;
        }
        /// <summary>
        /// Ensure that the text is an even length.
        /// </summary>
        /// <param name="codeC">Code to check.</param>
        private void CheckTypeC(String codeC)
        {
            if (this.Code128Code == Code128Type.C && (codeC.Length % 2) == 1)
                throw new ArgumentOutOfRangeException("Parameter text (string) must have an even length for Code 128 - Code C");
        }
        #endregion
        #endregion
    }
}