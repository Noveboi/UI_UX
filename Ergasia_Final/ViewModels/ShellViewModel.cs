using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ergasia_Final.ViewModels
{
    public class ShellViewModel : Screen
    {
        private string _firstName = "Mary";
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                NotifyOfPropertyChange();
            }
        }

        private string _lastName = "Dimi";
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
