using AgendaApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AgendaApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CabinetPage : ContentPage
    {
        HttpClient http = new HttpClient();
        Date dateSchedule = new Date();
        public CabinetPage()
        {
            InitializeComponent();
            Subscribe();
            PackSubscribe();
            SubscribeDate();
            dpDateSchedule.Date = DateSave.date.SelectedDate;
            dpDateSchedule_DateSelected(null, null);
            GetGroupList();
            this.BindingContext = this;
        }

        private void PackSubscribe()
        {
            MessagingCenter.Subscribe<Page, ListPack>(
                this, "PackList",
                (sender, str) =>
                {
                    ObservableCollection<string> vs = new ObservableCollection<string>(str.Cabinets);
                    pCabinet.ItemsSource = vs;
                    while (true)
                        try
                        {
                            string res = Preferences.Get("cabinetSelected", "");
                            if (res != "")
                            {
                                vs.Insert(0, res);
                            }
                            if (Preferences.Get("loadCabinet", false))
                            {
                                pCabinet.SelectedItem = res;
                            }
                            break;
                        }
                        catch
                        {
                            continue;
                        }
                });
        }

        private async void dpDateSchedule_DateSelected(object sender, DateChangedEventArgs e)
        {
            while (5 > 3)
            {
                try
                {
                    loading.IsVisible = true;
                    loading.IsAnimationPlaying = true;
                    cvSchedule.IsVisible = false;
                    cvSchedule.ItemsSource = null;
                    dateSchedule.GetDate(dpDateSchedule.Date);
                    lbFirstDay.Text = dateSchedule.downDay.ToString();
                    lbSecondDay.Text = dateSchedule.upDay.ToString();
                    lbSecondMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateSchedule.DupDay.Month);
                    lbFirstMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateSchedule.DdownDay.Month);
                    pCabinet_SelectedIndexChanged(null, null);
                    cvSchedule.IsVisible = true;
                    loading.IsAnimationPlaying = false;
                    loading.IsVisible = false;
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }

        private async void pCabinet_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (true)
            {
                try
                {
                    if (pCabinet.SelectedIndex != -1)
                    {
                        loading.IsVisible = true;
                        loading.IsAnimationPlaying = true;
                        cvSchedule.IsVisible = false;
                        var resGroup = await http.GetAsync(new Uri($"https://bsite.net/Abobus/api/lastdance/getcabinet/{pCabinet.SelectedItem.ToString().Replace("★", "")}?date={DateSave.date.SelectedDate.ToString("MM.dd.yyyy")}"));
                        resGroup.EnsureSuccessStatusCode();
                        var groupShedule = resGroup.Content.ReadAsAsync<List<DayWeek>>();
                        List<DayWeek> list = await groupShedule;
                        Agenda agenda = new Agenda();
                        cvSchedule.ItemsSource = agenda.MyAgenda(DateSave.date.DdownDay, list);
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
                        pCabinet.SelectedIndex = -1;
                        dpDateSchedule.Date = DateTime.Today.AddDays(5);
                        NewSheduleBt.Source = "fr.png";
                    }
                    else
                    {
                        NewSheduleBt.Source = "gg.png";
                        DateSave.date.SelectedDate = DateTime.Today;
                        pCabinet.SelectedIndex = -1;
                        dpDateSchedule.Date = DateTime.Today;
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        private async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Settings());
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

        private void SubscribeDate()
        {
            MessagingCenter.Subscribe<Page>(
                this, "DateChange",
                (sender) =>
                {
                    dpDateSchedule.Date = DateSave.date.SelectedDate;
                    lbFirstDay.Text = dateSchedule.downDay.ToString();
                    lbSecondDay.Text = dateSchedule.upDay.ToString();
                    lbSecondMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateSchedule.DupDay.Month);
                    lbFirstMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateSchedule.DdownDay.Month);
                });
        }

        private async void SearchCabinet_Clicked(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync("Поиск пустого кабинета", "Введите номер пары.", maxLength: 1, keyboard: Keyboard.Numeric);
            if (int.TryParse(result, out int numericResult))
            {
                if (numericResult >= 1 && numericResult <= 6)
                {
                    var reqesut = await http.GetAsync($"https://bsite.net/Abobus/api/lastdance/searchemptycabinet/{numericResult}");
                    reqesut.EnsureSuccessStatusCode();
                    var response = reqesut.Content.ReadAsAsync<List<string>>().Result;
                    string resultList = String.Empty;
                    foreach (var item in response)
                    {
                        resultList += item + "\n";
                    }
                    await DisplayAlert($"Список пустых кабинетов на {numericResult} паре.", resultList, "Ok");
                }
                else
                {
                    await DisplayAlert("Ошибка", "Некорректно введенные данные.", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Некорректно введенные данные.", "Ok");
            }
        }
    }
}