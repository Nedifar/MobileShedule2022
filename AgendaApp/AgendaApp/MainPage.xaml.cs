using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Globalization;
using AgendaApp.Models;
using Xamarin.Essentials;
using Xamanimation;

namespace AgendaApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        HttpClient http = new HttpClient();
        Date dateSchedule = new Date();
        int counter = 0;
        public MainPage()
        {
            InitializeComponent();
            Subscribe();
            dpDateSchedule.Date = DateSave.date.SelectedDate;
            dpDateSchedule_DateSelected(null, null);
            GetGroupList();
            this.BindingContext = this;
        }

        private async void dpDateSchedule_DateSelected(object sender, DateChangedEventArgs e)
        {
            while (true)
            {
                try
                {
                    loading.IsVisible = true;
                    loading.IsAnimationPlaying = true;
                    cvSchedule.IsVisible = false;
                    DateTime lastDate = DateSave.date.SelectedDate;
                    dateSchedule.GetDate(dpDateSchedule.Date);
                    DateSave.date.SelectedDate = dpDateSchedule.Date;
                    var packLists = await http.GetAsync($"https://bsite.net/Abobus/api/lastdance/getdate/{DateSave.date.SelectedDate.ToString("MM.dd.yyyy")}");
                    packLists.EnsureSuccessStatusCode();
                    ListPack lp = packLists.Content.ReadAsAsync<ListPack>().Result;
                    if (lp == null)
                    {
                        await DisplayAlert("Ошибка", "Данного расписания на сервере не найдено.", "Continue");
                        loading.IsVisible = false;
                        loading.IsAnimationPlaying = false;
                        dateSchedule.GetDate(lastDate);
                        DateSave.date.SelectedDate = lastDate;
                        cvSchedule.IsVisible = true;
                        dpDateSchedule.Date = lastDate;
                        break;
                    }
                    if (counter != 0)
                    {
                        MessagingCenter.Send<Page>(this, "DateChange");
                    }
                    cvSchedule.ItemsSource = null;
                    ObservableCollection<string> vs = new ObservableCollection<string>(lp.Groups);
                    pGroup.ItemsSource = vs;
                    MessagingCenter.Send<Page, ListPack>(this, "PackList", lp);
                    while (true)
                        try
                        {
                            string res = Preferences.Get("groupSelected", "");
                            if (res != "")
                            {
                                vs.Insert(0, res);
                            }
                            if (Preferences.Get("loadGroup", false))
                            {
                                pGroup.SelectedItem = res;
                            }
                            break;
                        }
                        catch
                        {
                            continue;
                        }
                    lbFirstDay.Text = dateSchedule.downDay.ToString();
                    lbSecondDay.Text = dateSchedule.upDay.ToString();
                    lbSecondMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateSchedule.DupDay.Month);
                    lbFirstMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateSchedule.DdownDay.Month);
                    pGroup_SelectedIndexChanged(null, null);
                    cvSchedule.IsVisible = true;
                    loading.IsAnimationPlaying = false;
                    loading.IsVisible = false;
                    counter++;
                    break;
                }
                catch
                {
                    continue;
                }
            }

        }

        private async void pGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (5 > 3)
            {
                try
                {
                    if (pGroup.SelectedIndex != -1)
                    {
                        loading.IsVisible = true;
                        loading.IsAnimationPlaying = true;
                        cvSchedule.IsVisible = false;
                        var resGroup = await http.GetAsync(new Uri($"https://bsite.net/Abobus/api/lastdance/getgroup/{pGroup.SelectedItem.ToString().Replace("★", "")}?date={DateSave.date.SelectedDate.ToString("MM.dd.yyyy")}"));
                        resGroup.EnsureSuccessStatusCode();
                        var groupShedule = resGroup.Content.ReadAsAsync<List<DayWeek>>();
                        List<DayWeek> list = await groupShedule;
                        Agenda agenda = new Agenda();
                        cvSchedule.ItemsSource = agenda.MyAgenda(dateSchedule.DdownDay, list);
                        cvSchedule.IsVisible = true;
                        loading.IsAnimationPlaying = false;
                        loading.IsVisible = false;

                    }
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }

        private async void GetGroupList()
        {

        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            while (true)
            {
                try
                {
                    if (NewSheduleBt.Source.ToString() == "File: gg.png")
                    {
                        string s = "Новое расписание!!!";
                        DateSave.date.SelectedDate = DateTime.Today.AddDays(7);
                        pGroup.SelectedIndex = -1;
                        dpDateSchedule.Date = DateTime.Today.AddDays(5);
                        NewSheduleBt.Source = "fr.png";
                    }
                    else
                    {
                        NewSheduleBt.Source = "gg.png";
                        DateSave.date.SelectedDate = DateTime.Today;
                        pGroup.SelectedIndex = -1;
                        dpDateSchedule.Date = DateTime.Today;
                    }
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.Settings());
        }
        private void Subscribe()
        {
            MessagingCenter.Subscribe<Page>(
                this, "Hi",
                (sender) =>
                {
                    bool rel = Preferences.Get("haveWeek", false);
                    if (rel == true)
                        NewSheduleBt.IsVisible = true;
                });
        }

       

        

        private async void CvSchedule_Scrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            
        }

        private void MainExpander_Tapped(object sender, EventArgs e)
        {
           
        }

    }
}
