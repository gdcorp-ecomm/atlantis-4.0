using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Chess Captcha Image Generator
    /// </summary>
    internal class ChessImageGenerator : ImageGenerator, IImageGenerator
	{
        // first set of tiles on the chessboard
        protected Color tile1Color
        {
            get
            {
                return colors["tile1Color"];
            }
            set
            {
                colors["tile1Color"] = value;
            }
        }

        // second set of tiles on the chessboard
        protected Color tile2Color
        {
            get
            {
                return colors["tile2Color"];
            }
            set
            {
                colors["tile2Color"] = value;
            }
        }

        // black text on white tiles
        protected ShapeCollection tiles1Clip = new ShapeCollection();

        // white text on black tiles
        protected ShapeCollection tiles2Clip = new ShapeCollection();

        protected int coin = RandomGenerator.Next(0, 100) % 2;

        /// <summary>
        /// pre-defined color scheme
        /// </summary>
        protected override void InitColors()
        {
            switch (coin) // the top left tile is randomly light or dark
            {
                case 0:
                    tile1Color = Color.Rgb(255, 255, 255); // white chess tiles
                    tile2Color = Color.Rgb(0, 0, 0); // black chess tiles
                    break;
                case 1:
                    tile2Color = Color.Rgb(255, 255, 255); // white chess tiles
                    tile1Color = Color.Rgb(0, 0, 0); // black chess tiles
                    break;
            }

            // default colors are not used, so we remove them just in case
            backColor = null;
            textColor = null;
            outlineColor = null;
        }

        /// <summary>
        /// apply user-defined color scheme
        /// </summary>
        protected override void OverrideColors()
        {
            if (null != customLightColor)
            {
                switch (coin) // the top left tile is randomly light or dark
                {
                    case 0:
                        tile1Color = customLightColor;
                        break;
                    case 1:
                        tile2Color = customLightColor;
                        break;
                }
            }

            if (null != customDarkColor)
            {
                switch (coin) // the top left tile is randomly light or dark
                {
                    case 0:
                        tile2Color = customDarkColor;
                        break;
                    case 1:
                        tile1Color = customDarkColor;
                        break;
                }
            }
        }

        protected override void InitFonts()
        {
            base.InitFonts();

            // use a fixed font family, so the black and white fragments are not
            // too different and hard to read
            FontCollection fontSelection = new FontCollection();
            fontSelection[0] = Font.From("Courier New", FontCase.Uppercase, FontWeight.Bold);
            fontSelection[1] = Font.From("Times New Roman", FontCase.Uppercase, FontWeight.Normal);
            fontSelection[2] = Font.From("Microsoft Sans Serif", FontCase.Uppercase, FontWeight.Normal);

            fonts = fontSelection;
            fonts.RandomizationStyle = RandomizationStyle.RandomizeOnce;
        }

        protected override void InitGlyphTransform()
        {
            // reduced glyph transformation set to make the character fragments easier to read
            transform = Transform.None;
            transform.Scaling.xScalingPercentageRange = new RandomRange(92, 97);
            transform.Scaling.yScalingPercentageRange = new RandomRange(95, 105);
            transform.Rotation.AngleRange = new RandomRange(-3, 3);
        }

        protected override void CustomInit()
        {
            // randomize the chessboard
            int gridSize = RandomGenerator.Next((int)Math.Round(graphics.Height / 2.2), graphics.Height / 2);
            
            // randomize the number of columns a bit
            int vLines = (int) Math.Round(RandomGenerator.Next(0.35, 0.70) * graphics.Width / gridSize); 
            int hLines = graphics.Height / gridSize;

            for (int i = 0; i < hLines; i++)
            {
                for (int j = 0; j < vLines; j++)
                {
                    // current tile top left corner
                    Point tilePosition = new Point(j * graphics.Width / vLines, i * graphics.Height / hLines);

                    // the current tile
                    Rectangle currentTile = new Rectangle(tilePosition, graphics.Width / vLines, graphics.Height / hLines);

                    // alternate between black and white tiles
                    if ((i + j) % 2 == 0)
                    {
                        tiles1Clip.Add(currentTile);
                    }
                    else
                    {
                        tiles2Clip.Add(currentTile);
                    }
                }
            }
        }

        protected override void DrawBackground()
        {
            // white image background
            graphics.Fill(tile1Color);

            // black tile background
            graphics.Fill(tile2Color, tiles2Clip);
        }

        protected override void DrawText()
        {
            // white letter fragments on black tiles
            textRenderer.Prototype.FillColor = tile1Color;
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 20, tile1Color)
            );
            textRenderer.Draw(graphics, tiles2Clip);

            // black letter fragments on white tiles
            textRenderer.Prototype.FillColor = tile2Color;
            textRenderer.Prototype.Outline = LineStyle.Single(
                LineLayer.Solid(graphics.Height / 20, tile2Color)
            );
            textRenderer.Draw(graphics, tiles1Clip);
        }
    }
}
