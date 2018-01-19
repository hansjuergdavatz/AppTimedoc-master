using AppTimedoc.Data;
using AppTimedoc.Helpers;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppTimedoc
{
  public partial class App : Application
  {
    static TimedocDatabase database;
    public static RestManager restManager { get; set; }

    public App()
    {
      InitializeComponent();

      // MainPage = new AppTimedoc.MainPage();
      SetMainPage();
    }

    protected override void OnStart()
    {
      // Handle when your app starts
      CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
    }

    void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
    {
      Type currentPage = this.MainPage.GetType();
      if (e.IsConnected && currentPage != typeof(MainTabbedPage))
        this.MainPage = new MainTabbedPage();
      else if (!e.IsConnected && currentPage != typeof(MainTabbedPage))
        this.MainPage = new MainTabbedPage();
    }
    
    protected override void OnSleep()
    {
      // Handle when your app sleeps
    }

    protected override void OnResume()
    {
      // Handle when your app resumes
    }
    public static TimedocDatabase Database
    {
      get
      {
        if (database == null)
        {
          string s = DependencyService.Get<IFileHelper>().GetLocalFilePath("TimedocSQLite.db3");
          database = new TimedocDatabase(s);
        }
        return database;
      }
    }

    public static void SetMainPage()
    {
      Current.MainPage = new MainTabbedPage();
    }

  }


}
