using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace datebookwpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string _fileName = "daily_planner.json";
        private DailyPlanner _dailyPlanner;
        private DateTime _currentDate;
        public MainWindow()
        {
            InitializeComponent();

            _dailyPlanner = LoadDailyPlanner();

            // Если ежедневник не удалось загрузить, создаем новый
            if (_dailyPlanner == null)
            {
                _dailyPlanner = new DailyPlanner();
            }

            // Устанавливаем DatePicker на текущую дату
            datePicker.SelectedDate = DateTime.Today;

            // При первом запуске устанавливаем текущую дату на сегодняшний день
            _currentDate = DateTime.Today;

            // Отображаем события на выбранную дату
            RefreshEvents();
        }
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentDate = datePicker.SelectedDate.Value;
            RefreshEvents();
        }

        // Обновление списка событий на выбранную дату
        private void RefreshEvents()
        {
            // Получаем список событий на выбранную дату
            var events = _dailyPlanner.Events.Where(e => e.Date == _currentDate).ToList();

            // Очищаем список событий
            eventList.Items.Clear();

            // Добавляем события в список
            foreach (var ev in events)
            {
                eventList.Items.Add(ev);
            }
        }

        // Обработчик события нажатия на кнопку "Добавить"
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новое событие
            var newEvent = new Event
            {
                Date = _currentDate,
                Title = "Новое событие",
                Description = ""
            };

            // Добавляем событие в ежедневник
            _dailyPlanner.Events.Add(newEvent);

            // Добавляем событие в список событий на текущую дату
            eventList.Items.Add(newEvent);

            // Сохраняем ежедневник
            SaveDailyPlanner();
        }

        // Обработчик события нажатия на кнопку "Удалить"
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Получаем выделенное событие
            var selectedEvent = eventList.SelectedItem as Event;

            // Если ни одно событие не выбрано, просто выходим
            if (selectedEvent == null)
            {
                return;
            }

            // Удаляем событие из списка событий на текущую дату
            event
        eventList.Items.Remove(selectedEvent);

        // Удаляем событие из ежедневника
        _dailyPlanner.Events.Remove(selectedEvent);

        // Сохраняем ежедневник
        SaveDailyPlanner();
    }

    // Обработчик события нажатия на кнопку "Сохранить"
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        // Получаем выделенное событие
        var selectedEvent = eventList.SelectedItem as Event;

        // Если ни одно событие не выбрано, просто выходим
        if (selectedEvent == null)
        {
            return;
        }

        // Обновляем свойства события
        selectedEvent.Title = titleTextBox.Text;
        selectedEvent.Description = descriptionTextBox.Text;

        // Обновляем отображение списка событий
        RefreshEvents();

        // Сохраняем ежедневник
        SaveDailyPlanner();
    }

    // Метод для загрузки ежедневника из файла
    private DailyPlanner LoadDailyPlanner()
    {
        try
        {
            // Если файл не существует, возвращаем null
            if (!File.Exists(_fileName))
            {
                return null;
            }

            // Читаем содержимое файла
            string json = File.ReadAllText(_fileName);

            // Десериализуем ежедневник из JSON
            return JsonConvert.DeserializeObject<DailyPlanner>(json);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось загрузить ежедневник: {ex.Message}");
            return null;
        }
    }

    // Метод для сохранения ежедневника в файл
    private void SaveDailyPlanner()
    {
        try
        {
            // Сериализуем ежедневник в JSON
            string json = JsonConvert.SerializeObject(_dailyPlanner);

            // Записываем JSON в файл
            File.WriteAllText(_fileName, json);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось сохранить ежедневник: {ex.Message}");
        }
    }
}

// Класс для хранения события
public class Event
{
    public DateTime Date { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public override string ToString()
    {
        return Title;
    }
}

// Класс для хранения ежедневника
public class DailyPlanner
{
    public List<Event> Events { get; set; }

    public DailyPlanner()
    {
        Events = new List<Event>();
    }
}
    }
}
