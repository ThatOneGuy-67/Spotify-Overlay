using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace SpotifyOverlay.Helpers
{
    public static class AnimationHelper
    {
        public static void FadeIn(UIElement element, double seconds = 0.25)
        {
            element.Opacity = 0;

            var animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(seconds),
                EasingFunction = new QuadraticEase()
            };

            element.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        public static void FadeOut(UIElement element, double seconds = 0.25)
        {
            var animation = new DoubleAnimation
            {
                From = element.Opacity,
                To = 0,
                Duration = TimeSpan.FromSeconds(seconds),
                EasingFunction = new QuadraticEase()
            };

            element.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        public static void Scale(UIElement element, double scale)
        {
            if (element.RenderTransform is not ScaleTransform transform)
            {
                transform = new ScaleTransform(1, 1);
                element.RenderTransform = transform;
                element.RenderTransformOrigin = new Point(0.5, 0.5);
            }

            var xAnimation = new DoubleAnimation
            {
                To = scale,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            var yAnimation = new DoubleAnimation
            {
                To = scale,
                Duration = TimeSpan.FromMilliseconds(150)
            };

            transform.BeginAnimation(ScaleTransform.ScaleXProperty, xAnimation);
            transform.BeginAnimation(ScaleTransform.ScaleYProperty, yAnimation);
        }

        public static void SlideDown(FrameworkElement element, double pixels)
        {
            var transform = new TranslateTransform();
            element.RenderTransform = transform;

            var animation = new DoubleAnimation
            {
                From = -pixels,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new CubicEase()
            };

            transform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        public static void SlideUp(FrameworkElement element, double pixels)
        {
            var transform = new TranslateTransform();
            element.RenderTransform = transform;

            var animation = new DoubleAnimation
            {
                From = 0,
                To = -pixels,
                Duration = TimeSpan.FromMilliseconds(250),
                EasingFunction = new CubicEase()
            };

            transform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        public static void AnimateProgress(FrameworkElement progressBar, double from, double to)
        {
            if (progressBar is System.Windows.Controls.ProgressBar bar)
            {
                var animation = new DoubleAnimation
                {
                    From = from,
                    To = to,
                    Duration = TimeSpan.FromMilliseconds(600),
                    EasingFunction = new QuadraticEase()
                };

                bar.BeginAnimation(
                    System.Windows.Controls.ProgressBar.ValueProperty,
                    animation);
            }
        }
    }
}