using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Runtime.Serialization;

using BotDetect.Drawing;
using BotDetect.Serialization;

namespace BotDetect
{
    [Serializable]
    public class ImageSize : IComparable, ISerializable
    {
        private int _width;
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (0 > value)
                {
                    throw new DrawingException("Image width must be a positive number", value);
                }

                _width = value;
            }
        }

        private int _height;
        public int Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (0 > value)
                {
                    throw new DrawingException("Image height must be a positive number", value);
                }

                _height = value;
            }
        }

        public ImageSize(int width, int height)
        {
            if (0 > width)
            {
                throw new DrawingException("Image width must be a positive number", width);
            }

            if (0 > height)
            {
                throw new DrawingException("Image height must be a positive number", height);
            }

            _width = width;
            _height = height;
        }

        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{{ width: {0}, height: {1} }}", _width, _height);
        }

        public ImageSize(System.Drawing.Size size)
        {
            _width = size.Width;
            _height = size.Height;
        }

        public System.Drawing.Size GdiSize
        {
            get
            {
                return new System.Drawing.Size(_width, _height);
            }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            ImageSize other = obj as ImageSize;

            if (this.Width == other.Width && this.Height == other.Height)
            {
                return 0;
            }

            if (this.Width * this.Height > other.Width * other.Height)
            {
                return 1;
            }

            return -1;
        }

        /*public override bool Equals(object obj)
        {
            bool equal = false;

            ImageSize other = obj as ImageSize;

            if (this.Width == other.Width && this.Height == other.Height)
            {
                equal = true;
            }

            return equal;
        }*/

        #endregion

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            SerializationWriter writer = new SerializationWriter();
            writer.WriteOptimized(_width);
            writer.WriteOptimized(_height);

            info.AddValue("data", writer.ToArray());
        }

        protected ImageSize(SerializationInfo info, StreamingContext context)
        {
            SerializationReader reader = new SerializationReader((byte[])info.GetValue("data", typeof(byte[])));
            _width = reader.ReadOptimizedInt32();
            _height = reader.ReadOptimizedInt32();
        }

        #endregion
    }
}
