using AppTimedoc.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppTimedoc.Helpers
{
  public class MainTabbedPage : TabbedPage
  {
    public MainTabbedPage()
    {
      var timePage = new Time() { Title = "Zeiterfassung", Icon = null };
      var workPage = new NavigationPage(new Work() { Title = "Leistungserfassung", Icon = null });
      var settingsPage = new AboutPage() { Title = "Info", Icon = null };
      var loginPage = new Login() { Title = "Benutzer", Icon = null };

      switch (Device.RuntimePlatform)
      {
        case Device.iOS:
          timePage.Icon = "ic_access_time.png";
          timePage.Icon = "ic_list.png";
          workPage.Title = "Leistungserfassung";
          settingsPage.Icon = "ic_error_outline.png";
          loginPage.Icon = "ic_accessibility.png";
          break;
        case Device.Android:
          timePage.Title = string.Empty;
          timePage.Icon = "ic_access_time_black_24dp.png";
          workPage.Title = string.Empty;
          workPage.Icon = "ic_list_black_24dp.png";
          settingsPage.Title = string.Empty;
          settingsPage.Icon = "ic_error_outline_black_24dp.png";
          loginPage.Title = string.Empty;
          loginPage.Icon = "ic_accessibility_black_24dp.png";
          break;
        default:
          break;
      }

      Children.Add(timePage);
      Children.Add(workPage);
      Children.Add(settingsPage);
      Children.Add(loginPage);
    }

    public void SwitchTab(int index)
    {
      CurrentPage = this.Children[index];
    }
  }
}
