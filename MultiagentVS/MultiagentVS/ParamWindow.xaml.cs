using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MultiagentVS
{
    public delegate void ParamsChanged(ParamWindow sender, Params parameters);

    /// <summary>
    /// Logique d'interaction pour ParamWindow.xaml
    /// </summary>
    public partial class ParamWindow : Window
    {
        private Params _newParams = new Params();
        private Params _oldParams = null;
        public event ParamsChanged ParamsChangedEvent;

        #region bindings

        private int _fps;

        public string Fps
        {
            get { return _fps.ToString(); }
            set { if (!int.TryParse(value, out _fps)) _fps = 0; }
        }
        #endregion


        public ParamWindow(Params currentParams)
        {
            InitializeComponent();


            _oldParams = currentParams;
            //_newParams = _oldParams;

            fpsTbo.Text = currentParams.FPS.ToString();

            //paramGrid.DataContext = _newParams;
        }

        private void validerB_Click(object sender, RoutedEventArgs e)
        {
            int fps;

            if (!fpsTbo.Text.Any())
            {
                return;
            }

            if (!int.TryParse(fpsTbo.Text, out fps))
                return;

            //_newParams = new Params
            //{
            //    FPS = fps
            //};

            ParamsChangedEvent?.Invoke(this,
                new Params
                {
                    FPS = fps
                });
        }
    }
}
