﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xam.Plugin.SimpleAppIntro
{
   /// <summary>
   /// App intro
   /// </summary>
   [XamlCompilation(XamlCompilationOptions.Compile)]
   public partial class SimpleAppIntro : ContentPage
   {
      #region Variables

      private readonly string[] DefaultColors = new string[20] { "#E91E63", "#F44336", "#9C27B0", "#2196F3", "#3F51B5", "#009688", "#4CAF50", "#FFEB3B", "#FF9800", "#607D8B", "#E91E63", "#F44336", "#9C27B0", "#2196F3", "#3F51B5", "#009688", "#4CAF50", "#FFEB3B", "#FF9800", "#607D8B" };
      private List<string> UsedColors = new List<string>();
      private int _position;
      private bool _carouselViewLoaded = false;
      private bool _showSkip = true;
      private bool _shipIndicator = true;
      private string _barColor = "#607D8B";
      private string _DoneButtonBackgroundColor = "#8BC34A";
      private string _SkipButtonBackgroundColor = "#8BC34A";
      private string _skipText = "Skip";
      private string _doneText = "Done";
      private List<Slide> _slides;

      #endregion

      #region Constructor

      /// <summary>
      /// Default Constructor
      /// </summary>
      public SimpleAppIntro()
      {
         InitializeComponent();
         if (_slides == null)
            _slides = new List<Slide>();
         _slides.Add(new Slide("", "", "", "#009688"));
         BindingContext = this;
      }

      #endregion

      #region Delegates

      /// <summary>
      /// Delegate for the callback method
      /// </summary>
      public delegate void CallbackDelegate();

      #endregion

      #region Properties

      /// <summary>
      /// Callback on skip button
      /// </summary>
      public CallbackDelegate OnSkipButtonClicked { get; set; }

      /// <summary>
      /// Callback on Done button
      /// </summary>
      public CallbackDelegate OnDoneButtonClicked { get; set; }

      /// <summary>
      /// List of slides
      /// </summary>
      public List<Slide> Slides { get { return _slides; } set { _slides = value; OnPropertyChanged(); } }

      /// <summary>
      /// Bottom bar color
      /// </summary>
      public string BarColor { get { return _barColor; } set { _barColor = value; OnPropertyChanged(); } }

      /// <summary>
      /// Done Button Background Color
      /// </summary>
      public string DoneButtonBackgroundColor { get { return _DoneButtonBackgroundColor; } set { _DoneButtonBackgroundColor = value; OnPropertyChanged(); } }

      /// <summary>
      /// Skip Button Background Color
      /// </summary>
      public string SkipButtonBackgroundColor { get { return _SkipButtonBackgroundColor; } set { _SkipButtonBackgroundColor = value; OnPropertyChanged(); } }

      /// <summary>
      /// Current position
      /// </summary>
      public int Position { get { return _position; } set { _position = value; OnPropertyChanged(); PositionChangedAsync(); } }

      /// <summary>
      /// Show position indicator
      /// </summary>
      public bool ShowPositionIndicator { get { return _shipIndicator; } set { _shipIndicator = value; OnPropertyChanged(); } }

      /// <summary>
      /// Show skip button
      /// </summary>
      public bool ShowSkipButton { get { return _showSkip; } set { _showSkip = value; OnPropertyChanged(); } }

      /// <summary>
      /// Show skip text
      /// </summary>
      public string SkipText { get { return _skipText; } set { _skipText = value; OnPropertyChanged(); } }

      /// <summary>
      /// Show done text
      /// </summary>
      public string DoneText { get { return _doneText; } set { _doneText = value; OnPropertyChanged(); } }

      #endregion

      #region Protected

      /// <summary>
      /// On page appearing
      /// </summary>
      protected override async void OnAppearing()
      {
         base.OnAppearing();
         if (ShowSkipButton) await skip.FadeTo(1, 200);
      }

      #endregion

      #region Public

      /// <summary>
      /// Add slide with properties
      /// </summary>
      /// <param name="title">Title</param>
      /// <param name="description">Description</param>
      /// <param name="icon">Icon source</param>
      /// <param name="color">Color of the slide</param>
      public void AddSlide(String title, String description, String icon, String color = null)
      {
         color = GetColor(color);
         if (!_carouselViewLoaded)
         {
            _slides[0] = new Slide(title, description, icon, color);
            _carouselViewLoaded = true;
         }
         else
            _slides.Add(new Slide(title, description, icon, color));
         carouselIndicators.ItemsSource = _slides;
         OnPropertyChanged();
         PositionChangedAsync();
      }

      #endregion

      #region private

      /// <summary>
      /// Get the new color of a slide
      /// </summary>
      private string GetColor(string color)
      {
         if (String.IsNullOrEmpty(color))
         {
            do
            {
               color = DefaultColors[new Random().Next(0, 20)];
            } while (UsedColors.Contains(color));
         }
         UsedColors.Add(color);
         return color;
      }

      /// <summary>
      /// Skip button clicked, pop navigation
      /// </summary>
      private void Skip_Clicked(object sender, EventArgs e)
      {
         OnSkipButtonClicked?.Invoke();
         Navigation.PopModalAsync();
      }

      /// <summary>
      /// Done clicked
      /// </summary>
      private void Done_Clicked(object sender, EventArgs e)
      {
         OnDoneButtonClicked?.Invoke();
         Navigation.PopModalAsync();
      }

#pragma warning disable S3168
      /// <summary>
      /// Position Changed
      /// </summary>
      private async void PositionChangedAsync()
#pragma warning restore S3168 
      {
         if (Position == Slides.Count - 1)
         {
            done.IsVisible = true;
            await Task.WhenAll(done.FadeTo(1, 200), skip.FadeTo(0, 200));
            skip.IsVisible = false;
         }
         else
         {
            if (ShowSkipButton)
            {
               skip.IsVisible = true;
               await Task.WhenAll(skip.FadeTo(1, 200), done.FadeTo(0, 200));
               done.IsVisible = false;
            }
            else
            {
               await done.FadeTo(0, 200);
               done.IsVisible = false;
            }
         }
      }

      #endregion
   }
}