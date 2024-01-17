using System.Windows.Controls;
using System.Windows.Media;

namespace Frontend.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для Button.xaml
    /// </summary>
    public partial class Button : UserControl
    {
        public Button()
        {
            InitializeComponent();
        }

        private Brush _color;
        public Brush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                button.Background = _color;
            }
        }
    }
}
