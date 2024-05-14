
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace ChemModel.ValidationRules
{
    public class ValidateMoreThanZero : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var stringValue = GetBoundValue(value) as string;
            if (string.IsNullOrEmpty(stringValue))
            {
                return new ValidationResult(false, "Введите значение");
            }
            if (!double.TryParse(stringValue, out double res) || res <= 0)
            {
                return new ValidationResult(false, "Значение должно быть больше нуля");
            }
            return new ValidationResult(true, null);
        }

        private object GetBoundValue(object value)
        {
            if (value is BindingExpression)
            {
                // ValidationStep was UpdatedValue or CommittedValue (Validate after setting)
                // Need to pull the value out of the BindingExpression.
                BindingExpression binding = (BindingExpression)value;

                // Get the bound object and name of the property
                object dataItem = binding.DataItem;
                string propertyName = binding.ParentBinding.Path.Path;

                // Extract the value of the property.
                object propertyValue = dataItem.GetType().GetProperty(propertyName).GetValue(dataItem, null);

                // This is what we want.
                return propertyValue;
            }
            else
            {
                // ValidationStep was RawProposedValue or ConvertedProposedValue
                // The argument is already what we want!
                return value;
            }
        }
    }
}
