namespace DragDropPhoneApp
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;

    using Build.DataLayer.Enum;

    using DragDropPhoneApp.ApiConsumer;
    using DragDropPhoneApp.Service;

    using Microsoft.Phone.Controls;

    #endregion

    public partial class LoadingImagesPage : PhoneApplicationPage
    {
        #region Fields

        private LinearGradientBrush TransformBrush;

        private ScaleTransform scale;

        private TransformGroup transformGroup;

        private TranslateTransform translation;

        #endregion

        #region Constructors and Destructors

        public LoadingImagesPage()
        {
            this.InitializeComponent();

            /*   transformGroup = new TransformGroup();
            translation = new TranslateTransform();
            scale = new ScaleTransform();
            transformGroup.Children.Add(scale);
            transformGroup.Children.Add(translation);
            myrect.RenderTransform = transformGroup;*/
        }

        #endregion

        #region Public Methods and Operators

        public void SolidColorBrushExample()
        {
            this.Title = "SolidColorBrush Animation Example";

            // Create a NameScope for the page so 
            // that Storyboards can be used.

            // Create a Rectangle.
            Rectangle aRectangle = new Rectangle();
            aRectangle.Width = 100;
            aRectangle.Height = 100;

            // Create a SolidColorBrush to paint 
            // the rectangle's fill. The Opacity 
            // and Color properties of the brush 
            // will be animated.
            SolidColorBrush myAnimatedBrush = new SolidColorBrush();
            myAnimatedBrush.Color = Colors.Orange;
            aRectangle.Fill = myAnimatedBrush;

            // Register the brush's name with the page 
            // so that it can be targeted by storyboards. 

            // Animate the brush's color to gray when 
            // the mouse enters the rectangle. 
            ColorAnimation mouseEnterColorAnimation = new ColorAnimation();
            mouseEnterColorAnimation.To = Colors.Gray;
            mouseEnterColorAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(mouseEnterColorAnimation, "MyAnimatedBrush");
            Storyboard.SetTargetProperty(mouseEnterColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
            Storyboard mouseEnterStoryboard = new Storyboard();
            mouseEnterStoryboard.Children.Add(mouseEnterColorAnimation);

            // Animate the brush's color to orange when 
            // the mouse leaves the rectangle. 
            ColorAnimation mouseLeaveColorAnimation = new ColorAnimation();
            mouseLeaveColorAnimation.To = Colors.Orange;
            mouseLeaveColorAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(mouseLeaveColorAnimation, "MyAnimatedBrush");
            Storyboard.SetTargetProperty(mouseLeaveColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
            Storyboard mouseLeaveStoryboard = new Storyboard();
            mouseLeaveStoryboard.Children.Add(mouseLeaveColorAnimation);

            // Animate the brush's opacity to 0 and back when 
            // the left mouse button is pressed over the rectangle. 
            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.To = 0.0;
            opacityAnimation.Duration = TimeSpan.FromSeconds(0.5);
            opacityAnimation.AutoReverse = true;
            Storyboard.SetTargetName(opacityAnimation, "MyAnimatedBrush");
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(Brush.OpacityProperty));
            Storyboard mouseLeftButtonDownStoryboard = new Storyboard();
            mouseLeftButtonDownStoryboard.Children.Add(opacityAnimation);

            StackPanel mainPanel = new StackPanel();
            mainPanel.Margin = new Thickness(20);
            mainPanel.Children.Add(aRectangle);
            this.Content = mainPanel;
        }

        #endregion

        #region Methods

        

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            new CustomProgressBar(this.ContentPanel);

            var valuesAsArray = Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>().ToList();
            foreach (var value in valuesAsArray)
            {

                ApiService<Imag>.DownloadImage(value.ToString());
            }
            return;

            LinearGradientBrush myVerticalGradient = new LinearGradientBrush();
            myVerticalGradient.StartPoint = new Point(0, 0.5);
            myVerticalGradient.EndPoint = new Point(1, 0.5);

            GradientStop stop2 = new GradientStop { Color = Colors.Red, Offset = 0.25 };
            myVerticalGradient.GradientStops.Add(new GradientStop { Color = Colors.Yellow, Offset = 0.0 });

            myVerticalGradient.GradientStops.Add(stop2);

            myVerticalGradient.GradientStops.Add(new GradientStop { Color = Colors.Black, Offset = 0.50 });

            myVerticalGradient.GradientStops.Add(new GradientStop { Color = Colors.Black, Offset = 0.75 });
            var rect = new Rectangle
                           {
                               Name = "ProgressBarRect", 
                               Fill = new SolidColorBrush(Colors.Black), 
                               HorizontalAlignment = HorizontalAlignment.Left, 
                               Height = 100, 
                               Margin = new Thickness(10, 275, 0, 0), 
                               Stroke = new SolidColorBrush(Colors.White), 
                               VerticalAlignment = VerticalAlignment.Top, 
                               Width = 436, 
                               StrokeThickness = 10, 
                               RadiusY = 1
                           };
            rect.Fill = myVerticalGradient;
            this.ContentPanel.Children.Add(rect);

            /*     ColorAnimation mouseEnterColorAnimation = new ColorAnimation();
            mouseEnterColorAnimation.To = Colors.Gray;
            mouseEnterColorAnimation.Duration = TimeSpan.FromSeconds(1);
            Storyboard.SetTargetName(mouseEnterColorAnimation, "myVerticalGradient");
            Storyboard.SetTargetProperty(
                mouseEnterColorAnimation, new PropertyPath(SolidColorBrush.ColorProperty));
            Storyboard mouseEnterStoryboard = new Storyboard();
            mouseEnterStoryboard.Begin();
            */
            ColorAnimation gradientStopColorAnimation = new ColorAnimation();
            gradientStopColorAnimation.From = Colors.Purple;
            gradientStopColorAnimation.To = Colors.Yellow;
            gradientStopColorAnimation.Duration = TimeSpan.FromSeconds(1.5);
            gradientStopColorAnimation.AutoReverse = true;

            Storyboard.SetTargetProperty(gradientStopColorAnimation, new PropertyPath(GradientStop.ColorProperty));

            Storyboard.SetTarget(gradientStopColorAnimation, stop2);
            Storyboard gradientStopAnimationStoryboard = new Storyboard();
            gradientStopAnimationStoryboard.Children.Add(gradientStopColorAnimation);
            gradientStopAnimationStoryboard.Begin();

            // mouseEnterStoryboard.Children.Add(mouseEnterColorAnimation);
        }

        private void asd()
        {
            GradientStop stop1 = new GradientStop { Color = Colors.Yellow, Offset = 0.0 };
            GradientStop stop2 = new GradientStop { Color = Colors.Yellow, Offset = 0.5 };
            GradientStop stop3 = new GradientStop { Color = Colors.Yellow, Offset = 0.7 };

            // Register a name for each gradient stop with the 
            // page so that they can be animated by a storyboard. 
            /*this.RegisterName("GradientStop1", stop1);
            this.RegisterName("GradientStop2", stop2);
            this.RegisterName("GradientStop3", stop3);

            // Add the stops to the brush.
            gradientBrush.GradientStops.Add(stop1);
            gradientBrush.GradientStops.Add(stop2);
            gradientBrush.GradientStops.Add(stop3);

            // Apply the brush to the rectangle.
            aRectangle.Fill = gradientBrush;*/

            // Animate the first gradient stop's offset from 
            // 0.0 to 1.0 and then back to 0.0. 
            DoubleAnimation offsetAnimation = new DoubleAnimation();
            offsetAnimation.From = 0.0;
            offsetAnimation.To = 1.0;
            offsetAnimation.Duration = TimeSpan.FromSeconds(1.5);
            offsetAnimation.AutoReverse = true;
            Storyboard.SetTargetName(offsetAnimation, "GradientStop1");
            Storyboard.SetTargetProperty(offsetAnimation, new PropertyPath(GradientStop.OffsetProperty));

            // Animate the second gradient stop's color from 
            // Purple to Yellow and then back to Purple. 
            ColorAnimation gradientStopColorAnimation = new ColorAnimation();
            gradientStopColorAnimation.From = Colors.Purple;
            gradientStopColorAnimation.To = Colors.Yellow;
            gradientStopColorAnimation.Duration = TimeSpan.FromSeconds(1.5);
            gradientStopColorAnimation.AutoReverse = true;
            Storyboard.SetTargetName(gradientStopColorAnimation, "GradientStop2");
            Storyboard.SetTargetProperty(gradientStopColorAnimation, new PropertyPath(GradientStop.ColorProperty));

            // Set the animation to begin after the first animation 
            // ends.
            gradientStopColorAnimation.BeginTime = TimeSpan.FromSeconds(3);

            // Animate the third gradient stop's color so 
            // that it becomes transparent. 
            ColorAnimation opacityAnimation = new ColorAnimation();

            // Reduces the target color's alpha value by 1,  
            // making the color transparent.
            opacityAnimation.By = Color.FromArgb(4, 1, 23, 4);
            opacityAnimation.Duration = TimeSpan.FromSeconds(1.5);
            opacityAnimation.AutoReverse = true;
            Storyboard.SetTargetName(opacityAnimation, "GradientStop3");
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(GradientStop.ColorProperty));

            // Set the animation to begin after the first two 
            // animations have ended. 
            opacityAnimation.BeginTime = TimeSpan.FromSeconds(6);

            // Create a Storyboard to apply the animations.
            Storyboard gradientStopAnimationStoryboard = new Storyboard();
            gradientStopAnimationStoryboard.Children.Add(offsetAnimation);
            gradientStopAnimationStoryboard.Children.Add(gradientStopColorAnimation);
            gradientStopAnimationStoryboard.Children.Add(opacityAnimation);

            // Begin the storyboard when the left mouse button is 
            // pressed over the rectangle.
            StackPanel mainPanel = new StackPanel();
            mainPanel.Margin = new Thickness(10);

            // mainPanel.Children.Add(aRectangle);
            // Content = mainPanel;
        }

        private void myRectangle_MouseEnter(object sender, RoutedEventArgs e)
        {
            // Set the animation
            var oColorAnimation = new ColorAnimation();

            // The initial brush state
            oColorAnimation.From = Colors.Yellow;

            // The final brush state
            oColorAnimation.To = Colors.Red;

            // The animation duration
            oColorAnimation.Duration = TimeSpan.FromSeconds(8);
            SolidColorBrush TransformBrush1;

            // Trigger the animation
            // TransformBrush.BeginAnimation(SolidColorBrush.ColorProperty, oColorAnimation);
        }

        #endregion

        public class CustomProgressBar
        {
            #region Static Fields

            private static List<SolidColorBrush> colors = new List<SolidColorBrush>
                                                              {
                                                                  new SolidColorBrush(Colors.Red), 
                                                                  new SolidColorBrush(Colors.Blue), 
                                                                  new SolidColorBrush(Colors.Yellow), 
                                                                  new SolidColorBrush(
                                                                      Colors.Magenta), 
                                                                  new SolidColorBrush(Colors.Orange), 
                                                                  new SolidColorBrush(Colors.Blue), 
                                                                  new SolidColorBrush(Colors.Green)
                                                              };

            private static List<GradientStop> gradientStops = new List<GradientStop>
                                                                  {
                                                                      new GradientStop
                                                                          {
                                                                              Color =
                                                                                  Colors
                                                                                  .Black, 
                                                                              Offset =
                                                                                  0.0
                                                                          }, 
                                                                      new GradientStop
                                                                          {
                                                                              Color =
                                                                                  Colors
                                                                                  .Black, 
                                                                              Offset =
                                                                                  0.25
                                                                          }, 
                                                                      new GradientStop
                                                                          {
                                                                              Color =
                                                                                  Colors
                                                                                  .Black, 
                                                                              Offset =
                                                                                  0.5
                                                                          }, 
                                                                      new GradientStop
                                                                          {
                                                                              Color =
                                                                                  Colors
                                                                                  .Black, 
                                                                              Offset =
                                                                                  0.75
                                                                          }, 
                                                                  };

            private static Random rnd = new Random();

            #endregion

            #region Fields

            private int NextStop;

            private int prevColorNum;

            private GradientStop stop2;

            #endregion

            #region Constructors and Destructors

            public CustomProgressBar(Grid parent)
            {
                this.Initialize(parent);
                for (int i = 0; i < 4; i++)
                {
                    if (!App.DataContext.DownloadImageUnderNumberCompleted.ContainsKey(i))
                    App.DataContext.DownloadImageUnderNumberCompleted.Add(i, false);
                }
                var z = DataService.GetImagesNamesList(false);
                var b = z;
                this.AnimateNext();

                // Thread.Sleep(1500);
           //     this.AnimateNext();

                // Thread.Sleep(1500);
            //    this.AnimateNext();

                // Thread.Sleep(1500);
                // AnimateNext();
            }

            #endregion

            #region Public Methods and Operators

            public void AnimateNext()
            {
                int rndColorNum = rnd.Next(colors.Count);
                while (this.prevColorNum == rndColorNum)
                {
                    rndColorNum = rnd.Next(colors.Count);
                }

                this.prevColorNum = rndColorNum;
                var color1 = colors.ElementAt(rndColorNum);
                if (gradientStops.Count > this.NextStop)
                {
                    this.AnimateLoad(color1.Color, gradientStops.ElementAt(this.NextStop), NextStop);
                    this.NextStop++;
                }
            }

            #endregion

            #region Methods

            private int imgDownloaded = 0;
            private void AnimateLoad(Color color, GradientStop stop21,int animateNumb)
            {
                ColorAnimation gradientStopColorAnimation = new ColorAnimation();
                gradientStopColorAnimation.From = Colors.Black;

                gradientStopColorAnimation.To = color;
                gradientStopColorAnimation.Duration = TimeSpan.FromSeconds(1.25);
                gradientStopColorAnimation.RepeatBehavior = new RepeatBehavior(1);
                gradientStopColorAnimation.AutoReverse = true;

                Storyboard.SetTargetProperty(gradientStopColorAnimation, new PropertyPath(GradientStop.ColorProperty)); 
                Storyboard.SetTarget(gradientStopColorAnimation, stop21);
                Storyboard gradientStopAnimationStoryboard = new Storyboard();
                gradientStopAnimationStoryboard.Children.Add(gradientStopColorAnimation);
                gradientStopAnimationStoryboard.Begin();
                bool asd = true;
                gradientStopColorAnimation.Completed += (sender, args) =>
                    {
                       
                        if (!App.DataContext.DownloadImageUnderNumberCompleted[animateNumb])
                        {
                            (sender as ColorAnimation).RepeatBehavior = new RepeatBehavior(1);
                            gradientStopAnimationStoryboard.Begin();
                        }
                        else
                        {
                            if (asd)
                            {
                                imgDownloaded++;
                                textBox.Text = string.Format("{0} of {1}", imgDownloaded, 4);
                            (sender as ColorAnimation).AutoReverse = false;

                            (sender as ColorAnimation).RepeatBehavior = new RepeatBehavior(1.5);
                            gradientStopAnimationStoryboard.Begin();
                                asd = false;
                                this.AnimateNext();//animate next
                            }
                        }
                    };
                
             
                 
            }

            private TextBlock textBox;
            private void Initialize(Grid grid)
            {
                LinearGradientBrush myVerticalGradient = new LinearGradientBrush();
                myVerticalGradient.StartPoint = new Point(0, 0.5);
                myVerticalGradient.EndPoint = new Point(1, 0.5);

                foreach (var gradientStop in gradientStops)
                {
                    myVerticalGradient.GradientStops.Add(gradientStop);
                }

                 textBox = new TextBlock
                                  {
                                      Name = "TextBoxProgressBar", 
                                      Height = 100,
                                      Margin = new Thickness(10, 375, 0, 0),
                                      VerticalAlignment = VerticalAlignment.Top,
                                      Width = 436,
                                      TextAlignment = TextAlignment.Center
                                  };

                 textBox.Text = string.Format("{0} of {1}", imgDownloaded, 4);
                var rect = new Rectangle
                               {
                                   Name = "ProgressBarRect", 
                                   Fill = new SolidColorBrush(Colors.Black), 
                                   HorizontalAlignment = HorizontalAlignment.Left, 
                                   Height = 100, 
                                   Margin = new Thickness(10, 275, 0, 0), 
                                   Stroke = new SolidColorBrush(Colors.White), 
                                   VerticalAlignment = VerticalAlignment.Top, 
                                   Width = 436, 
                                   StrokeThickness = 10, 
                                   RadiusY = 1
                               };

                rect.Fill = myVerticalGradient;
                grid.Children.Add(rect);
                grid.Children.Add(textBox);
            }

            #endregion
        }
    }
}