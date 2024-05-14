using ChemModel.Data.DbTables;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Messeges
{
    public class UserMessage : ValueChangedMessage<User>
    {
        public UserMessage(User message) : base(message) { }
    }
}
