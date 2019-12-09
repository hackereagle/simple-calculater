using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator.ViewModels.Specific
{
    class CalculatorVM : SimpleCalculator.ViewModels.Base.ViewModelBase
    {
        #region Private Field
        private double FirstValue;
        private double SecondValue;
        private char Operator;
        private string _displayText = "";
        private bool flagHaveBeenCalculated = false;

        private SimpleCalculator.Commands.Generic.RelayCommand<object> _equalCommand;
        private SimpleCalculator.Commands.Generic.RelayCommand<object> _buttonClick;
        private SimpleCalculator.Commands.Generic.RelayCommand<object> _clearCommand;
        private SimpleCalculator.Commands.Generic.RelayCommand<object> _signFlipFlopCommand;

        private SimpleCalculator.BusinessLogic.Calculate _calculator;
        #endregion Private Field

        public CalculatorVM()
        {
            _calculator = new BusinessLogic.Calculate();
            _calculator.Result.Subscribe(x => { DisplayText = x; });

            _buttonClick = new Commands.Generic.RelayCommand<object>(button_Click);
            _equalCommand = new Commands.Generic.RelayCommand<object>(equalButtonClick);
            _clearCommand = new Commands.Generic.RelayCommand<object>(clearButtonClick);
            _signFlipFlopCommand = new Commands.Generic.RelayCommand<object>(signFlipFlop);
        }

        #region Public Properties
        public string DisplayText
        {
            get { return _displayText; }

            set
            {
                _displayText += value;
                OnPropertyChanged("DisplayText");
            }
        }


        #endregion Public Properties

        #region Commands
        public System.Windows.Input.ICommand ButtonClick => _buttonClick;
        public System.Windows.Input.ICommand EqualClick => _equalCommand;
        public System.Windows.Input.ICommand ClearButtonClick => _clearCommand;
        public System.Windows.Input.ICommand SignFlipFlopButtonClick => _signFlipFlopCommand;
        #endregion Commands


        internal void button_Click(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("In CalculatorVM button_Click");

            byte[] charArr = Encoding.ASCII.GetBytes(obj.ToString());

            if (charArr.Length == 1 && IsNum(charArr[0]))
            {
                if (flagHaveBeenCalculated == true)               
                    _displayText = "";
                _displayText += (char)charArr[0];
            }
            else if (charArr.Length == 1 && IsOperator(charArr[0]))
            {
                if (_displayText == "")
                    throw new ArgumentException("In CalculatorVM button_Click ");

                FirstValue = Convert.ToDouble(_displayText);
                _displayText = "";
                Operator = (char)charArr[0];
            }
            else if (charArr.Length == 1 && (char)charArr[0] == '.')
            {
                if (_displayText == "")
                    throw new ArgumentException("In CalculatorVM button_Click ");

                double temp;
                if (!double.TryParse(_displayText + obj, out temp))
                    return;

                _displayText += (char)charArr[0];
            }
            else
            {
                Console.WriteLine("Catching other character: " + obj);
                //throw new ArgumentException("In CalculatorVM button_Click ");
            }
            OnPropertyChanged("DisplayText");
        }

        private bool IsNum(byte _char)
        {
            try
            {
                if (_char >= 48 && _char <= 57)
                    return true;
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("In CalculatorVM.IsNum: " + ex.ToString());
                return false;
            }
        }
        private bool IsOperator(byte _char)
        {
            try
            {
                if (_char == 42 || _char == 43 || _char == 45 || _char == 47)
                    return true;
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("In CalculatorVM.IsOperator: " + ex.ToString());
                return false;
            }
        }

        internal void equalButtonClick(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("In CalculatorVM equalButtonClick ");

            if (_displayText == "")
                throw new ArgumentException("In CalculatorVM equalButtonClick ");

            SecondValue = Convert.ToDouble(_displayText);
            _displayText = "";
            switch (Operator)
            {
                case '+':
                    _calculator.Add(FirstValue, SecondValue);
                    break;
                case '-':
                    _calculator.Substract(FirstValue, SecondValue);
                    break;
                case '*':
                    _calculator.Mutiply(FirstValue, SecondValue);
                    break;
                case '/':
                    _calculator.Divide(FirstValue, SecondValue);
                    break;
                default:
                    return;
            }
            Operator = ' ';
            flagHaveBeenCalculated = true;
            OnPropertyChanged("DisplayText");
        }

        internal void clearButtonClick(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("In CalculatorVM clearButtonClick ");

            this.FirstValue = 0;
            this.SecondValue = 0;
            _displayText = "";
            this.Operator = ' ';
            flagHaveBeenCalculated = false;
            OnPropertyChanged("DisplayText");
        }

        internal void signFlipFlop(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("In CalculatorVM clearButtonClick");

            _displayText = (Convert.ToDouble(_displayText) * Convert.ToDouble(obj)).ToString();
            OnPropertyChanged("DisplayText");
        }
    }

    class Test
    {
        public Test()
        {
            System.Windows.Forms.MessageBox.Show("Test");
        }
    }
}
