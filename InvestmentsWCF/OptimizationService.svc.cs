using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace InvestmentsWCF
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IOptimizationService
    {
        public List<Double> DoSimplex(int value)
        {
            List<Double> result = new List<Double>();

            return result;
        }

        public List<Double> DoBionic(int value)
        {
            List<Double> result = new List<Double>();

            return result;
        }
    }
}
