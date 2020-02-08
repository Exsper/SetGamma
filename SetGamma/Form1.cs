using System;
using System.Windows.Forms;

namespace SetGamma
{
    public partial class SetGammaForm : Form
    {
        Gamma _gamma = new Gamma();

        public SetGammaForm()
        {
            InitializeComponent();
        }

        private void SetTrackBar(int gammaValue)
        {
            if (gammaValue < 0) GammaValueTrackBar.Value = 0;
            else if (gammaValue > 100) GammaValueTrackBar.Value = 100;
            else GammaValueTrackBar.Value = gammaValue;
        }

        private void SetTrackBarLabel()
        {
            ValueLabel.Text = GammaValueTrackBar.Value.ToString();
        }


        private void SetGammaForm_Load(object sender, EventArgs e)
        {
            int gammaValue = _gamma.GetGamma();
            SetTrackBar(gammaValue);
            SetTrackBarLabel();
        }


        private void ResetButton_Click(object sender, EventArgs e)
        {
            _gamma.SetDefaultGamma();
            int gammaValue = _gamma.GetGamma();
            SetTrackBar(gammaValue);
            SetTrackBarLabel();
        }


        private void GammaValueTrackBar_Scroll(object sender, EventArgs e)
        {
            SetTrackBarLabel();
            _gamma.SetGamma(GammaValueTrackBar.Value);
        }
    }
}
