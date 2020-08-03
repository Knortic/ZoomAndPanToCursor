using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ZoomAndPanToCursor
{
    public class KnorticMatrix
    {
        public double scaleX, skewX,
                      skewY, scaleY,
                      translateX, translateY;

        private static Point startingPoint;
        private static Point movingPoint;
        private static bool isPanning = false;

        public enum MatrixType
        {   IDENTITY  };

        public KnorticMatrix() { }

        public static KnorticMatrix GetIdentityMatrix()
        {
            return new KnorticMatrix(1, 0,
                                     0, 1,
                                     0, 0);
        }

        public KnorticMatrix(double scaleX, double skewX, double skewY, double scaleY, double translateX, double translateY)
        {
            this.scaleX = scaleX;
            this.skewX = skewX;
            this.skewY = skewY;
            this.scaleY = scaleY;
            this.translateX = translateX;
            this.translateY = translateY;
        }

        public static void Zoom(Image image, MouseWheelEventArgs e, double maxZoomAmount)
        {
            double positionRelativeToImageX = e.GetPosition(image).X;
            double positionRelativeToImageY = e.GetPosition(image).Y;
            double actualWidthOfImage = image.ActualWidth;
            double actualHeightOfImage = image.ActualHeight;
            double minimumZoomAmount = 1.0;
            double maximumZoomAmount = maxZoomAmount;

            var imageTransform = image.RenderTransform.Value;

            double deltaScrollAmount = (e.Delta > 0) ? 1.1 : 0.9;

            double newScaleForImageX = image.RenderTransform.Value.M11 * deltaScrollAmount;
            double newScaleForImageY = image.RenderTransform.Value.M22 * deltaScrollAmount;

            if (newScaleForImageX >= maximumZoomAmount && newScaleForImageY >= maximumZoomAmount || newScaleForImageX <= minimumZoomAmount && newScaleForImageY <= minimumZoomAmount)
                return;

            double newTranslateForImageX = (deltaScrollAmount > 1) ? (imageTransform.OffsetX - (positionRelativeToImageX * 0.1 * imageTransform.M11))
                : (imageTransform.OffsetX - (positionRelativeToImageX * -0.1 * imageTransform.M11));
            double newTranslateForImageY = (deltaScrollAmount > 1) ? (imageTransform.OffsetY - (positionRelativeToImageY * 0.1 * imageTransform.M22))
                : (imageTransform.OffsetY - (positionRelativeToImageY * -0.1 * imageTransform.M22));

            if (newScaleForImageX <= minimumZoomAmount | newScaleForImageY <= minimumZoomAmount)
            {
                newScaleForImageY = minimumZoomAmount; newTranslateForImageX = 0;
                newScaleForImageX = minimumZoomAmount; newTranslateForImageY = 0;
            }

            image.RenderTransform = new MatrixTransform(newScaleForImageX, image.RenderTransform.Value.M12, image.RenderTransform.Value.M21, newScaleForImageY, newTranslateForImageX, newTranslateForImageY);
        }

        public static void HandleMouseDown(Image image, MouseEventArgs e)
        {
            isPanning = true;
            startingPoint = new Point(e.GetPosition(image).X - movingPoint.X,
                                      e.GetPosition(image).Y - movingPoint.Y);
        }

        public static void HandleMouseUp() => isPanning = false;

        public static void Pan(Image image, MouseEventArgs e)
        {
            if (isPanning)
            {
                movingPoint = new Point(e.GetPosition(image).X - startingPoint.X,
                                        e.GetPosition(image).Y - startingPoint.Y);

                image.RenderTransform = new MatrixTransform(image.RenderTransform.Value.M11, 0,
                                                            0, image.RenderTransform.Value.M22,
                                                            image.RenderTransform.Value.OffsetX + movingPoint.X, image.RenderTransform.Value.OffsetY + movingPoint.Y);
            }
        }
    }
}
