using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Messeges
{
    public class NewUserMessage : ValueChangedMessage<NewUser>
    {
        public NewUserMessage(NewUser message ) : base( message ) { }
    }
}
