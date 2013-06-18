using System;
using System.Collections.Generic;
using System.Text;

namespace BotDetect.Drawing
{
	[Serializable]
    internal class Transform
    {
        private Rotation _rotation = new Rotation();
        private Scaling _scaling = new Scaling();
        private Translation _translation = new Translation();
        private Warp _warp = new Warp();

        public Rotation Rotation
        {
            get
            {
                return _rotation;
            }

            set
            {
                _rotation = value;
            }
        }

        public Scaling Scaling
        {
            get
            {
                return _scaling;
            }

            set
            {
                _scaling = value;
            }
        }

        public Translation Translation
        {
            get
            {
                return _translation;
            }

            set
            {
                _translation = value;
            }
        }

        public Warp Warp 
        {
            get
            {
                return _warp;
            }

            set
            {
                _warp = value;
            }
        }

        public static Transform None
        {
            get
            {
                return new Transform();
            }
        }
    }
}
