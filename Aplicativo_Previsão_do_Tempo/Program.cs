using Aplicativo_Previsão_do_Tempo.Entities;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; // Importação do Newtonsoft.Json

namespace AplicativoPrevisaoTempo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao Aplicativo de Previsão do Tempo!");

            Console.Write("Digite o nome da cidade: ");
            string cidade = Console.ReadLine().ToUpper();

            string apiKey = "e26cfc441e065c35fe5e661c3eeb975e"; // Certifique-se que a chave está correta

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={apiKey}&units=metric"; // Adicionado units=metric para mostrar temperatura em Celsius

            using(HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string responseBody = await response.Content.ReadAsStringAsync();

                    PrevisaoTempo previsao = JsonConvert.DeserializeObject<PrevisaoTempo>(responseBody);

                    if(previsao != null && previsao.Main != null && previsao.Weather != null)
                    {
                        Console.WriteLine("Cidade: " + previsao.Name);
                        Console.WriteLine("Temperatura: " + previsao.Main.Temp.ToString("f1",CultureInfo.InvariantCulture) + "°C");
                        Console.WriteLine("Condição: " + previsao.Weather[0].Description);
                    }
                    else
                    {
                        Console.WriteLine("Os dados de previsão não foram encontrados.");
                    }
                }
                catch(HttpRequestException e)
                {
                    Console.WriteLine("Erro ao obter a previsão do tempo: " + e.Message);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Erro inesperado: " + e.Message);
                }

                Console.ReadKey();
            }
        }
    }
}
