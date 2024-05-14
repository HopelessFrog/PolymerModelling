using ChemModel.ViewModels.ViewModelsData;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Messeges
{
    public class DataExcelMessage : ValueChangedMessage<DataExcel>
    {
        public DataExcelMessage(DataExcel message) : base(message) { }
    }
}
