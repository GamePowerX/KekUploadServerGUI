using System.Collections.Generic;
using KekUploadServerApi.Uploads;

namespace KekUploadServerGUI;

public class MainWindowViewModel
{
    public List<IUploadedItem> Uploads { get; set; } = new();
}