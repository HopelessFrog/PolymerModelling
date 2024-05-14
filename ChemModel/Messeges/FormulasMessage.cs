using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChemModel.Messeges
{
    public class FormulasMessage : ValueChangedMessage<FormulaConstructResult>
    {
        public FormulasMessage(FormulaConstructResult value) : base(value)
        {

        }
    }
}
