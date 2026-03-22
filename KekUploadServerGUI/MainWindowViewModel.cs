using System.Collections.ObjectModel;
using KekUploadServerApi.Uploads;

namespace KekUploadServerGUI;

public class MainWindowViewModel
{
    public ObservableCollection<IUploadedItem> Uploads { get; set; } = new();
}