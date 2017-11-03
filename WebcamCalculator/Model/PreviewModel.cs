using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Expression.Encoder.Devices;

namespace WebcamCalculator.Model
{
    class PreviewModel
    {

        public Collection<EncoderDevice> VideoDevices { get; set; }
        public Collection<EncoderDevice> AudioDevices { get; set; }
        public PreviewModel()
        {
            CameraCapture();
        }

        private void CameraCapture()
        {
            
        }
    }
}
