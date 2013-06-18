using System;

using BotDetect.Drawing;

namespace BotDetect.CaptchaImage
{
    /// <summary>
    /// Summary description for ImageGeneratorFactory.
    /// </summary>
    internal abstract class ImageGeneratorFactory
    {
        public static IImageGenerator CreateGenerator(ImageStyle imageStyle)
        {
            IImageGenerator generator = null;

            switch (imageStyle)
            {
                case ImageStyle.Bubbles:
                    {
                        generator = new BubblesImageGenerator();
                        break;
                    }
                case ImageStyle.Ripple:
                    {
                        generator = new RippleImageGenerator();
                        break;
                    }
                case ImageStyle.Ripple2:
                    {
                        generator = new Ripple2ImageGenerator();
                        break;
                    }
                case ImageStyle.Electric:
                    {
                        generator = new ElectricImageGenerator();
                        break;
                    }
                case ImageStyle.Radar:
                    {
                        generator = new RadarImageGenerator();
                        break;
                    }



                case ImageStyle.Neon:
                    {
                        generator = new NeonImageGenerator();
                        break;
                    }
                case ImageStyle.Neon2:
                    {
                        generator = new Neon2ImageGenerator();
                        break;
                    }

                case ImageStyle.BlackOverlap:
                    {
                        generator = new BlackOverlapImageGenerator();
                        break;
                    }
                case ImageStyle.Chess:
                    {
                        generator = new ChessImageGenerator();
                        break;
                    }
                case ImageStyle.Chess3D:
                    {
                        generator = new Chess3DImageGenerator();
                        break;
                    }
                case ImageStyle.Chipped:
                    {
                        generator = new ChippedImageGenerator();
                        break;
                    }
                case ImageStyle.Circles:
                    {
                        generator = new CirclesImageGenerator();
                        break;
                    }
                case ImageStyle.Corrosion:
                    {
                        generator = new CorrosionImageGenerator();
                        break;
                    }
                case ImageStyle.Distortion:
                    {
                        generator = new DistortionImageGenerator();
                        break;
                    }
                case ImageStyle.Flash:
                    {
                        generator = new FlashImageGenerator();
                        break;
                    }
                case ImageStyle.Jail:
                    {
                        generator = new JailImageGenerator();
                        break;
                    }
                case ImageStyle.Mass:
                    {
                        generator = new MassImageGenerator();
                        break;
                    }
                case ImageStyle.Negative:
                    {
                        generator = new NegativeImageGenerator();
                        break;
                    }
                case ImageStyle.Overlap:
                    {
                        generator = new OverlapImageGenerator();
                        break;
                    }
                case ImageStyle.Overlap2:
                    {
                        generator = new Overlap2ImageGenerator();
                        break;
                    }
                case ImageStyle.Rough:
                    {
                        generator = new RoughImageGenerator();
                        break;
                    }
                case ImageStyle.Snow:
                    {
                        generator = new SnowImageGenerator();
                        break;
                    }
                case ImageStyle.Split:
                    {
                        generator = new SplitImageGenerator();
                        break;
                    }
                case ImageStyle.Split2:
                    {
                        generator = new Split2ImageGenerator();
                        break;
                    }
                case ImageStyle.Stitch:
                    {
                        generator = new StitchImageGenerator();
                        break;
                    }
                case ImageStyle.WantedCircular:
                    {
                        generator = new WantedCircularImageGenerator();
                        break;
                    }
                case ImageStyle.Wave:
                    {
                        generator = new WaveImageGenerator();
                        break;
                    }
                case ImageStyle.Darts:
                    {
                        generator = new DartsImageGenerator();
                        break;
                    }
                case ImageStyle.Fingerprints:
                    {
                        generator = new FingerprintsImageGenerator();
                        break;
                    }
                case ImageStyle.Lego:
                    {
                        generator = new LegoImageGenerator();
                        break;
                    }
                case ImageStyle.Strippy:
                    {
                        generator = new StrippyImageGenerator();
                        break;
                    }
                case ImageStyle.CrossShadow:
                    {
                        generator = new CrossShadowImageGenerator();
                        break;
                    }
                case ImageStyle.CrossShadow2:
                    {
                        generator = new CrossShadow2ImageGenerator();
                        break;
                    }
                case ImageStyle.ThickThinLines:
                    {
                        generator = new ThickThinLinesImageGenerator();
                        break;
                    }
                case ImageStyle.ThickThinLines2:
                    {
                        generator = new ThickThinLines2ImageGenerator();
                        break;
                    }
                case ImageStyle.Sunrays:
                    {
                        generator = new SunraysImageGenerator();
                        break;
                    }
                case ImageStyle.Sunrays2:
                    {
                        generator = new Sunrays2ImageGenerator();
                        break;
                    }
                case ImageStyle.ThinWavyLetters:
                    {
                        generator = new ThinWavyLettersImageGenerator();
                        break;
                    }
                case ImageStyle.Chalkboard:
                    {
                        generator = new ChalkboardImageGenerator();
                        break;
                    }
                case ImageStyle.WavyColorLetters:
                    {
                        generator = new WavyColorLettersImageGenerator();
                        break;
                    }
                case ImageStyle.AncientMosaic:
                    {
                        generator = new AncientMosaicImageGenerator();
                        break;
                    }
                case ImageStyle.Vertigo:
                    {
                        generator = new VertigoImageGenerator();
                        break;
                    }
                case ImageStyle.WavyChess:
                    {
                        generator = new WavyChessImageGenerator();
                        break;
                    }
                case ImageStyle.MeltingHeat:
                    {
                        generator = new MeltingHeatImageGenerator();
                        break;
                    }
                case ImageStyle.MeltingHeat2:
                    {
                        generator = new MeltingHeat2ImageGenerator();
                        break;
                    }
                case ImageStyle.SunAndWarmAir:
                    {
                        generator = new SunAndWarmAirImageGenerator();
                        break;
                    }
                case ImageStyle.Graffiti:
                    {
                        generator = new GraffitiImageGenerator();
                        break;
                    }
                case ImageStyle.Graffiti2:
                    {
                        generator = new Graffiti2ImageGenerator();
                        break;
                    }
                case ImageStyle.Halo:
                    {
                        generator = new HaloImageGenerator();
                        break;
                    }
                case ImageStyle.Bullets:
                    {
                        generator = new BulletsImageGenerator();
                        break;
                    }
                case ImageStyle.Bullets2:
                    {
                        generator = new Bullets2ImageGenerator();
                        break;
                    }
                case ImageStyle.CaughtInTheNet:
                    {
                        generator = new CaughtInTheNetImageGenerator();
                        break;
                    }
                case ImageStyle.CaughtInTheNet2:
                    {
                        generator = new CaughtInTheNet2ImageGenerator();
                        break;
                    }
                case ImageStyle.Cut:
                    {
                        generator = new CutImageGenerator();
                        break;
                    }
                case ImageStyle.Ghostly:
                    {
                        generator = new GhostlyImageGenerator();
                        break;
                    }
                case ImageStyle.InBandages:
                    {
                        generator = new InBandagesImageGenerator();
                        break;
                    }
                case ImageStyle.PaintMess:
                    {
                        generator = new PaintMessImageGenerator();
                        break;
                    }
                case ImageStyle.Collage:
                    {
                        generator = new CollageImageGenerator();
                        break;
                    }
                case ImageStyle.SpiderWeb:
                    {
                        generator = new SpiderWebImageGenerator();
                        break;
                    }
                case ImageStyle.SpiderWeb2:
                    {
                        generator = new SpiderWeb2ImageGenerator();
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException("Image Generator not implemented!");
                    }
            }

            return generator;
        }
    }
}
