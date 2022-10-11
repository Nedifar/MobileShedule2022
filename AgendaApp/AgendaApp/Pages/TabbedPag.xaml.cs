using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Net.Http;
using System.Net;
using System.IO;

namespace AgendaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class TabbedPag : Xamarin.Forms.TabbedPage
    {
        ContentPage groupPage;
        ContentPage cabinetPage;
        ContentPage teacherpage;
        [Obsolete]
        public TabbedPag()
        {
            
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom)
             .SetBarItemColor(Color.FromRgb(0, 198, 174))
             .SetBarSelectedItemColor(Color.FromRgb(0, 83, 153));
            Models.DateSave.dateGet();
            groupPage = new MainPage();
            groupPage.IconImageSource = "group.png";
            groupPage.Title = "Group";
            cabinetPage = new CabinetPage();
            cabinetPage.IconImageSource = "cab.png";
            cabinetPage.Title = "Cabinet";
            teacherpage = new TeacherPage();
            teacherpage.IconImageSource = "teach.png";
            teacherpage.Title = "Teacher";
            Children.Add(groupPage);
            Children.Add(cabinetPage);
            Children.Add(teacherpage);
            Sign();
            Task.WaitAll();
            GetNewSheduleInformation();
        }

        private async void GetNewSheduleInformation()
        {
            
            HttpClient http = new HttpClient();
            while (true)
            {
                try
                {
                    var resnew = await http.GetAsync("http://bsite.net/Abobus/api/lastdance/getnes");
                    if (resnew.StatusCode == HttpStatusCode.OK)
                    {
                        Preferences.Set("haveWeek", true);
                    }
                    else
                    {
                        Preferences.Set("haveWeek", false);
                    }
                    break;
                }
                catch(Exception ex)
                {
                    continue;
                }
            }
            MessagingCenter.Send<Page>(this, "Hi");
        }

        async void Sign()
        {
            string data = "";
            var backing = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "appConfigMod.txt");
            try
            {
                using (var reader = new StreamReader(backing))
                {
                    data = await reader.ReadToEndAsync();
                    reader.Close();
                    if (data.Trim() != "Mat'NeTrogai")
                        throw new Exception();
                }
            }
            catch
            {
                using (HttpClient client = new HttpClient())
                {
                    string result = await DisplayPromptAsync("Scaning", "Please, enter password.(ggwp)");
                    var resulti = await client.GetAsync($"https://bsite.net/Abobus/api/lastdance/getSignData/{result}");
                    if (resulti.StatusCode == HttpStatusCode.BadRequest)
                    {
                        await DisplayAlert("Imposter", "А вот неть, а вот тебя я не пущу, с уважением", "OK");
                        Environment.Exit(0);
                    }
                    else
                    {
                        using (var sw = new StreamWriter(backing, false, Encoding.Default))
                        {
                            sw.Write(result);
                            data = result;
                        }
                    }
                }
            }
            if (data.Trim() == "")
            {
                await DisplayAlert("Imposter", "Защита от Егора на месте.", "OK");
                Environment.Exit(0);
            }
        }
    }
}