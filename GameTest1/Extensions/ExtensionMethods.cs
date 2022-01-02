using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static GameTest1.GameObject;

namespace GameTest1.Extensions
{
    public static class ExtensionMethods
    {
        private static Texture2D _blankTexture;
        public static Texture2D BlankTexture(this SpriteBatch s)
        {
            if (_blankTexture == null)
            {
                _blankTexture = new Texture2D(s.GraphicsDevice, 1, 1);
                _blankTexture.SetData(new[] { Color.White });
            }
            return _blankTexture;
        }
        public static Texture2D Crop(this Texture2D image, Rectangle source)
        {
            //var graphics = image.GraphicsDevice;
            //var ret = new RenderTarget2D(graphics, source.Width, source.Height);
            //Texture2D retimg = (Texture2D)ret;
            //return retimg
            Texture2D cropTexture = new Texture2D(Game2.Graphics.GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            image.GetData(0, source, data, 0, data.Length);
            cropTexture.SetData(data);
            return cropTexture;
        }
        public static Vector2 CalcOffsets(this Rectangle source, Rectangle comp)
        {
            Vector2 ret = new Vector2();
            ret.X = source.Width - comp.Width;
            ret.Y = source.Height - comp.Height;
            return ret;
        }
        public static CollisionType GetOpposite(this CollisionType type)
        {
            switch (type)
            {
                case CollisionType.Top:
                    return CollisionType.Bottom;
                case CollisionType.Bottom:
                    return CollisionType.Top;
                case CollisionType.Right:
                    return CollisionType.Left;
                case CollisionType.Left:
                    return CollisionType.Right;
            }
            return type;
        }

        //van https://stackoverflow.com/questions/4144778/get-properties-and-values-from-unknown-object
        static public object GetValObjDy(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
