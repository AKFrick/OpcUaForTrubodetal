using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.ComponentModel;

using System.Runtime.CompilerServices;

namespace OpcUaForTrubodetal
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            OutputList = new ObservableCollection<OutputItem>();
            MyCommand = new WpfCommand( (o) => { SendTask(); } );                        
        }

        public void SendTask()
        {
            ProductionTask task = new ProductionTask()
            {
                ID = 13,
                TaskNumber = "MyTaskNumber123",
                Diameter = 2,
                Thickness = 3,
                PieceLength1 = 4,
                PieceQuantity1 = 5
            };

            OpcUa opc = new OpcUa();
            opc.SendTask(new ProductionTaskExtended(task));
        }

        private void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public string MyText { get; set; } = "Hi there!";
        public WpfCommand MyCommand { get; set; }

        public ObservableCollection<OutputItem> OutputList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
    public class OutputItem
    {
        public DateTime TimeStamp { get; private set; }
        public string Message { get; private set; } 
        public OutputItem(string message)
        {
            TimeStamp = DateTime.Now;
            Message = message;
        }
    }
    
}
