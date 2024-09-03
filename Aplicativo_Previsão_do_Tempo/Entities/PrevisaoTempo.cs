using AplicativoPrevisaoTempo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicativo_Previsão_do_Tempo.Entities
{
    internal class PrevisaoTempo
    {
        public string Name { get; set; }
        public MainData Main { get; set; }
        public WeatherData[] Weather { get; set; }
    }
}
