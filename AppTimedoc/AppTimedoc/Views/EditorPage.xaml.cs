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