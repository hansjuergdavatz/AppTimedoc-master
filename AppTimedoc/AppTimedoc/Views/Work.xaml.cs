using AppTimedoc.Data;
using AppTimedoc.Helpers;
using AppTimedoc.Models;
using Plugin.Connectivity;
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
  public partial class Work : ContentPage
  {
    bool _setActDay = false;
    bool _hasCostUnit = false;
    bool _signatureAktiv = false;
    Coworker _user = null;
    DateTime _dateSelected = DateTime.MinValue;
    OrderAchievement _actOrderAchievement = null;
    List<OrderAchievement> list = null;

    public Work()
    {
      InitializeComponent();

      DayDate.Date = DateTime.Now;
      _dateSelected = DayDate.Date;

    }

    private async void ReadSetting()
    {
      // Basic-http
      App.restManager = new RestManager(new Web.RestService());
      Setting setting = await App.restManager.GetSettingAsync("CostUnit");
      if (setting != null)
      {
        if (setting.Value == "1")
        {
          _hasCostUnit = true;  // Kostenträger in Verwendung
        }

        if (_hasCostUnit)
        {
          OrderAchievementListView.IsVisible = true;
          OrderAchievementListView.RowHeight = 100;
          OrderAchievementListViewSmall.IsVisible = false;
          OrderAchievementListViewSmall.RowHeight = 0;
        }
        else
        {
          OrderAchievementListViewSmall.IsVisible = true;
          OrderAchievementListViewSmall.RowHeight = 80;
          OrderAchievementListView.IsVisible = false;
          OrderAchievementListView.RowHeight = 0;
        }
      }
      setting = await App.restManager.GetSettingAsync("SignatureMobile");
      if (setting != null)
      {
        if (setting.Value == "1")
        {
          _signatureAktiv = true;
        }
      }
    }

    protected async override void OnAppearing()
    {
      base.OnAppearing();

      //CoworkerStorage coStore = new CoworkerStorage();
      //_user = coStore.LoadCoworker();
      _user = await App.Database.GetCoworker();

      if (_user == null)
      {
        var tabbedPage = this.Parent;

        var tabbedPage2 = tabbedPage.Parent as MainTabbedPage;
        tabbedPage2.SwitchTab(3);
      }

      if (_user != null)
      {
        if (_actOrderAchievement != null)
        {
          _setActDay = _actOrderAchievement.IsActDay;
          if (_setActDay)
            DayDate.Date = DateTime.Now;
        }

        if (_dateSelected == DateTime.MinValue || _setActDay)
        {
          _dateSelected = DayDate.Date;
          _actOrderAchievement.IsActDay = false;
        }

        ReadSetting();
        await LoadList(true);
      }
    }

    async Task LoadList(bool detail)
    {
      bool isOnline = await IsApiReachableAndRunning();
      if (isOnline == false)
      {
        return;
      }

      // Basic-http
      App.restManager = new RestManager(new Web.RestService());

      try
      {
        list = await App.restManager.GetListOrderAchievementAsync(_dateSelected, detail);
        if (list != null)
        {
          if (list.Count > 0)
          {
            SetDisplayText();
            if (_hasCostUnit)
            {
              OrderAchievementListView.ItemsSource = null;
              OrderAchievementListView.ItemsSource = list;
              OrderAchievementListView.IsVisible = true;
              OrderAchievementListViewSmall.IsVisible = false;
              OrderAchievementListViewSmall.HeightRequest = 0;
            }
            else
            {
              OrderAchievementListViewSmall.ItemsSource = null;
              OrderAchievementListViewSmall.ItemsSource = list;
              OrderAchievementListViewSmall.IsVisible = true;
              OrderAchievementListView.IsVisible = false;
              OrderAchievementListView.HeightRequest = 0;
            }
          }
          else
          {
            if (_hasCostUnit)
              OrderAchievementListView.ItemsSource = null;
            else
              OrderAchievementListViewSmall.ItemsSource = null;
          }
        }
      }
      catch (Exception)
      {
        if (_hasCostUnit)
          OrderAchievementListView.ItemsSource = null;
        else
          OrderAchievementListViewSmall.ItemsSource = null;
      }
    }

    private void SetDisplayText()
    {
      foreach (OrderAchievement item in list)
      {
        string menge = " Menge: " + item.Amount.ToString("0.00") + item.Unit;
        item.TxtLarge = item.OrderNrDesc;
        item.TxtSmall = item.AchieName;
        item.TxtSmall3 = item.CostNrDesc;
        item.TxtSmall2 = item.AmountInfo + menge;

        if (item.Status == 100)
        {
          if (Device.RuntimePlatform == Device.iOS)
            item.Image = "ic_update.png";
          else
            item.Image = "ic_update_black_24dp.png";
        }
        else
        {
          if (Device.RuntimePlatform == Device.iOS)
            item.Image = "ic_check_circle.png";
          else
            item.Image = "ic_check_circle_black_24dp.png";
        }
      }


    }

    private async void btnStart_Clicked(object sender, EventArgs e)
    {
      waitCursor.IsVisible = true;
      waitCursor.IsRunning = true;

      // Basic-http
      App.restManager = new RestManager(new Web.RestService());

      try
      {
        DayDate.Date = DateTime.Now;
        list = await App.restManager.GetNewOrderAchievementAsync(string.Empty, string.Empty, true, true, string.Empty);

        await LoadList(true);

        //if (list != null)
        //{
        //  SetDisplayText();
        //  OrderAchievementListView.ItemsSource = list;
        //}
      }
      catch (Exception)
      {
        OrderAchievementListView.ItemsSource = null;
      }

      waitCursor.IsVisible = false;
      waitCursor.IsRunning = false;

    }

    private async void DayDate_DateSelected(object sender, DateChangedEventArgs e)
    {
      _dateSelected = e.NewDate;
      if (e.NewDate != e.OldDate)
        await LoadList(true);
    }

    private async void OrderAchievement_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
      try
      {
        _actOrderAchievement = e.SelectedItem as OrderAchievement;
        if (_actOrderAchievement == null)
          return;
        //        await Navigation.PushAsync(new WorkDetail(_actOrderAchievement, _hasCostUnit, _signatureAktiv));
        await Navigation.PushAsync(new OADetail(_actOrderAchievement, _hasCostUnit, _signatureAktiv));

      }
      catch (Exception ex)
      {
        string s = ex.Message;
        throw;
      }
    }
    private async void Refresh()
    {
      await LoadList(true);
    }

    private async void btnCreate_Clicked(object sender, EventArgs e)
    {
      waitCursor.IsVisible = true;
      waitCursor.IsRunning = true;

      // Basic-http
      App.restManager = new RestManager(new Web.RestService());

      try
      {
        DayDate.Date = DateTime.Now;
        list = await App.restManager.GetNewOrderAchievementAsync(string.Empty, string.Empty, false, true, string.Empty);

        await LoadList(true);

        //if (list != null)
        //{
        //  SetDisplayText();
        //  OrderAchievementListView.ItemsSource = list;
        //}
      }
      catch (Exception)
      {
        OrderAchievementListView.ItemsSource = null;
      }

      waitCursor.IsVisible = false;
      waitCursor.IsRunning = false;

    }

    private async void btnConnect2_Clicked(object sender, EventArgs e)
    {
      bool isOnline = await IsApiReachableAndRunning();
    }
    public async Task<bool> IsApiReachableAndRunning()
    {
      try
      {
        var connectivity = CrossConnectivity.Current;
        if (!connectivity.IsConnected)
        {
          btnConnect2.IsVisible = true;
          btnConnect2.Text = "-- OFFLINE --";
          btnConnect2.BackgroundColor = Color.Red;
          return false;
        }

        /// <summary>
        /// Tests if a remote host name is reachable (no http:// or www.)
        /// </summary>
        /// <param name="host">Host name can be a remote IP or URL of website</param>
        /// <param name="port">Port to attempt to check is reachable.</param>
        /// <param name="msTimeout">Timeout in milliseconds.</param>
        /// <returns></returns>
        var reachable = await connectivity.IsRemoteReachable("caprex.ddns.net", 5505, 2000);
        if (reachable == false)
        {
          btnConnect2.IsVisible = true;
          btnConnect2.Text = "-- TIMEDOC OFFLINE --";
          btnConnect2.BackgroundColor = Color.Red;
          return false;
        }

        btnConnect2.IsVisible = false;
        return reachable;

      }
      finally
      {
        CrossConnectivity.Dispose();
      }

    }


    //private async void btnList_Clicked(object sender, EventArgs e)
    //{
    //    // TODO immer Detail-Liste
    //    if (btnList.Text == "Detail")
    //    {
    //        await LoadList(true);
    //        btnList.Text = "Komp.";
    //    }
    //    else
    //    {
    //        await LoadList(false);
    //        btnList.Text = "Detail";
    //    }
    //}
  }
}