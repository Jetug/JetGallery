using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace PicEditor.Behaviours
{
    class MVVMBehaviour : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty MousePosProperty = DependencyProperty.Register(
            "MousePos", typeof(Point), typeof(MVVMBehaviour), new PropertyMetadata(default(Point)));

        public static readonly DependencyProperty ActualHeightProperty = DependencyProperty.Register(
            "ActualHeight", typeof(double), typeof(MVVMBehaviour), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty ActualWidthProperty = DependencyProperty.Register(
            "ActualWidth", typeof(double), typeof(MVVMBehaviour), new PropertyMetadata(default(double)));

        public Point MousePos
        {
            get { return (Point)GetValue(MousePosProperty); }
            set { SetValue(MousePosProperty, value); }
        }

        public double ActualHeight
        {
            get { return (double)GetValue(ActualHeightProperty); }
            set { SetValue(ActualHeightProperty, value); }
        }

        public double ActualWidth
        {
            get { return (double)GetValue(ActualWidthProperty); }
            set { SetValue(ActualWidthProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.MouseMove += AssociatedObjectOnMouseMove;
            ActualHeight = AssociatedObject.ActualHeight;
            ActualWidth = AssociatedObject.ActualWidth;
        }

        private void AssociatedObjectOnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            var pos = mouseEventArgs.GetPosition(AssociatedObject);
            MousePos = pos;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseMove -= AssociatedObjectOnMouseMove;
        }
    }
}
