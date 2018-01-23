using AppTimedoc.Data;
using AppTimedoc.Helpers;
using AppTimedoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppTimedoc.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimeDetail : ContentPage
	{
    WorkingTime _actWorkingTime = null;

    public TimeDetail(WorkingTime wt)
    {
      InitializeComponent();

      switch (Device.RuntimePlatform)
      {
        case Device.iOS:
          btnDelete.Image = "ic_delete.png";
          btnSave.Image = "ic_save.png";
          btnAbort.Image = "ic_arrow_back.png";
          break;
        case Device.Android:
          //playPage.Title = string.Empty;
          //playPage.Icon = "ic_access_time_black_24dp.png";
          //workPage.Title = string.Empty;
          //workPage.Icon = "ic_list_black_24dp.png";
          //settingsPage.Title = string.Empty;
          //settingsPage.Icon = "ic_error_outline_black_24dp.png";
          //aboutPage.Title = string.Empty;
          //aboutPage.Icon = "ic_accessibility_black_24dp.png";
          break;
        default:
          break;
      }
      _actWorkingTime = wt;
      SetData();
    }

    private void SetData()
    {
      ComeDate.Date = _actWorkingTime.ComeTime.Date;
      ComeTime.Time = new TimeSpan(_actWorkingTime.ComeTime.TimeOfDay.Ticks);
      Remark.Text = _actWorkingTime.Remark;

      DateTime d = (DateTime)_actWorkingTime.GoTime;
      GoDate.Date = d.Date;
      if (_actWorkingTime.GoTime == DateTime.MinValue)
        GoDate.Date = _actWorkingTime.ComeTime.Date;
      GoTime.Time = new TimeSpan(d.TimeOfDay.Ticks);
    }

    private void ComeDate_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void ComeTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
      if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
      {
        //do something

      }
    }

    private void GoDate_DateSelected(object sender, DateChangedEventArgs e)
    {

    }

    private void GoTime_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

    }

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
      _actWorkingTime.ComeTime = new DateTime(ComeDate.Date.Year, ComeDate.Date.Month, ComeDate.Date.Day, ComeTime.Time.Hours, ComeTime.Time.Minutes, 0);
      if (GoTime.Time.Hours > 0 || GoTime.Time.Minutes > 0)
        _actWorkingTime.GoTime = new DateTime(GoDate.Date.Year, GoDate.Date.Month, GoDate.Date.Day, GoTime.Time.Hours, GoTime.Time.Minutes, 0);
      _actWorkingTime.Remark = Remark.Text;

      // Basic-http
      App.restManager = new RestManager(new Web.RestService());
      await App.restManager.UpdateWorkingTimeAsync(_actWorkingTime);

      DependencyService.Get<IMessage>().ShortAlert("Daten gespeichert.");

      await Navigation.PopModalAsync();

    }

    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
      // Basic-http
      App.restManager = new RestManager(new Web.RestService());
      await App.restManager.DeleteWorkingTimeAsync(_actWorkingTime.Id);

      await Navigation.PopModalAsync();

    }

    private async void btnAbort_Clicked(object sender, EventArgs e)
    {
      await Navigation.PopModalAsync();

    }
  }
}