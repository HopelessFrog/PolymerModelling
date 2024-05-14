using ChemModel.ViewModels;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Messeges
{
    public class DataMessage : ValueChangedMessage<TableData[]>
    {
        public DataMessage(TableData[] message) : base(message) { } 
    }
}
