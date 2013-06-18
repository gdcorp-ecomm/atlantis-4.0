using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
    internal class Perspective : Effect, IEffect
    {
        public override void Apply(IGraphics graphics, Rectangle bounds)
        {
            // todo: implement level
            // todo: refactor

            int width = bounds.Width;
            int height = bounds.Height;

            double old_t_x = 0;
            double old_b_x = width;
            double old_l_x = 0;
            double old_r_x = width;

            double new_t_x = 0;
            double new_b_x = width;
            double new_l_x = 0;
            double new_r_x = width;

            int coin = RandomGenerator.Next(0, 2);
            if (0 == coin)
            {
                new_r_x = RandomGenerator.Next(0.85, 0.95) * width;
            }
            else
            {
                new_l_x = RandomGenerator.Next(0.05, 0.15) * width;
            }

            double x_scale_top = (old_r_x - old_t_x) / (new_r_x - new_t_x);
            double x_scale_bottom = (old_b_x - old_l_x) / (new_b_x - new_l_x);

            double x_dist_top = (old_r_x - old_t_x) - (new_r_x - new_t_x);
            double x_dist_bottom = (old_b_x - old_l_x) - (new_b_x - new_l_x);

            graphics.LockPixels();

            for (int y1 = 0; y1 < height; y1++)
            {
                double x_scale = (y1 / (height * 1.00)) * x_scale_bottom + ((height - y1) / (height * 1.00)) * x_scale_top;
                int x_offset = (int) Math.Round(((y1 / (height * 1.00)) * x_dist_bottom + ((height - y1) / (height * 1.00)) * x_dist_top) / 2);
                for (int x1 = 0; x1 < width; x1++)
                {
                    // to the left of cutoff border
                    if (x1 - x_offset <= 0)
                    {
                        graphics.SwapPixels(x1, y1, 1, height-1);
                        continue;
                    }

                    // to the right of cutoff border
                    int newX = (int)Math.Round(x1 * x_scale);
                    if (newX <= 0 || newX >= width)
                    {
                        if (x1 + x_offset < width)
                        {
                            graphics.SwapPixels(x1 + x_offset, y1, 1, height-1);
                        }
                        continue;
                    }

                    // within resizable area
                    graphics.SwapPixels(x1 + x_offset, y1, newX, y1);
                }
            }

            graphics.UnlockPixels();
        }

        /*public void Apply(System.Drawing.Drawing2D.GraphicsPath path)
        {
            float width = path.GetBounds().Width;
            float height = path.GetBounds().Height;

            double factor = this.Level * width / 5;
            int offset = factor * RandomGenerator.Next(1, 3);

            System.Drawing.PointF[] points = new System.Drawing.PointF[4];
            points[0] = new System.Drawing.PointF(offset, 0);
            points[1] = new System.Drawing.PointF(width - offset, 0);
            points[2] = new System.Drawing.PointF(0, height);
            points[3] = new System.Drawing.PointF(width, height);

            path.Warp(points, path.GetBounds(), new System.Drawing.Drawing2D.Matrix(), System.Drawing.Drawing2D.WarpMode.Perspective);
        }*/
    }
}
