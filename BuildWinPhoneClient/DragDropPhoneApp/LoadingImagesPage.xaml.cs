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
    using Build.DataLayer.Model;

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
        }

        #endregion

        #region Methods

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
         //   TwitterApi.PostMessageToTwitter("MessageToTwitter");



           
            new CustomProgressBar(this.ContentPanel);

            var valuesAsArray = Enum.GetValues(typeof(ActivityType)).Cast<ActivityType>().ToList();
            var images = DataService.GetImagesNamesList(false);
            if (images.Count >= valuesAsArray.Count)
            {
                this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                return;
            }

            var imagesToDownload = valuesAsArray.Where(c => !images.Any(v => v == c.ToString() + ".jpg"));
            foreach (var value in imagesToDownload)
            {
                ApiService<Imag>.DownloadImage(value.ToString());

                // Thread.Sleep(1500);
            }

            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
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

            private int imgDownloaded;

            private int prevColorNum;

            private GradientStop stop2;

            private TextBlock textBox;

            #endregion

            #region Constructors and Destructors

            public CustomProgressBar(Grid parent)
            {
                this.Initialize(parent);
                for (int i = 0; i < 4; i++)
                {
                    if (!App.DataContext.DownloadImageUnderNumberCompleted.ContainsKey(i))
                    {
                        App.DataContext.DownloadImageUnderNumberCompleted.Add(i, false);
                    }
                }

                this.AnimateNext();

             
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
                    this.AnimateLoad(color1.Color, gradientStops.ElementAt(this.NextStop), this.NextStop);
                    this.NextStop++;
                }
            }

            #endregion

            #region Methods

            private void AnimateLoad(Color color, GradientStop stop21, int animateNumb)
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
                                this.imgDownloaded++;
                                this.textBox.Text = string.Format("{0} of {1}", this.imgDownloaded, 4);
                                (sender as ColorAnimation).AutoReverse = false;

                                (sender as ColorAnimation).RepeatBehavior = new RepeatBehavior(1.5);
                                gradientStopAnimationStoryboard.Begin();
                                asd = false;
                                this.AnimateNext(); 
                            }
                        }
                    };
            }

            private void Initialize(Grid grid)
            {
                LinearGradientBrush myVerticalGradient = new LinearGradientBrush();
                myVerticalGradient.StartPoint = new Point(0, 0.5);
                myVerticalGradient.EndPoint = new Point(1, 0.5);

                foreach (var gradientStop in gradientStops)
                {
                    myVerticalGradient.GradientStops.Add(gradientStop);
                }

                this.textBox = new TextBlock
                                   {
                                       Name = "TextBoxProgressBar", 
                                       Height = 100, 
                                       Margin = new Thickness(10, 375, 0, 0), 
                                       VerticalAlignment = VerticalAlignment.Top, 
                                       Width = 436, 
                                       TextAlignment = TextAlignment.Center
                                   };

                this.textBox.Text = string.Format("{0} of {1}", this.imgDownloaded, 4);
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
                grid.Children.Add(this.textBox);
            }

            #endregion
        }
    }
}