using System.Collections.ObjectModel;
namespace ConcurSolutionz.Models;

internal class Entry
{
    public string Filename { get; set; }
    public ObservableCollection<Receipt> Receipts { get; set; } = new ObservableCollection<Receipt>();

}
