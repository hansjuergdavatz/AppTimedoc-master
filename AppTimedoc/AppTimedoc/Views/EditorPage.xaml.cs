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
	public partial class EditorPage : ContentPage
	{
    public string _remark = string.Empty;

		public EditorPage (string remark)
		{
			InitializeComponent ();

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

      _remark = remark;
      EditorInfo.Text = _remark;
		}

    private async void btnSave_Clicked(object sender, EventArgs e)
    {
      _remark = EditorInfo.Text;
      await Navigation.PopModalAsync();
    }
    private async void btnDelete_Clicked(object sender, EventArgs e)
    {
      _remark = string.Empty;
      await Navigation.PopModalAsync();
    }

    private async void btnAbort_Clicked(object sender, EventArgs e)
    {
      await Navigation.PopModalAsync();
    }

  }
}