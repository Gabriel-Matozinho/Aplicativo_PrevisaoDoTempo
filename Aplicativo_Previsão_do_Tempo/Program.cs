using Aplicativo_Previsão_do_Tempo.Entities; // Importa as classes do projeto que estão na pasta Entities
using System; // Importa funcionalidades básicas do sistema
using System.Globalization; // Permite trabalhar com formatação de dados baseada em culturas, como números e datas
using System.Net.Http; // Permite o uso de requisições HTTP, necessário para fazer chamadas à API de previsão do tempo
using System.Threading.Tasks; // Facilita o uso de tarefas assíncronas (async/await)
using Newtonsoft.Json; // Biblioteca utilizada para manipulação de JSON, convertendo o retorno da API para objetos C#

namespace AplicativoPrevisaoTempo
{
    class Program
    {
        // Método principal e assíncrono da aplicação
        static async Task Main(string[] args)
        {
            Console.WriteLine("Bem-vindo ao Aplicativo de Previsão do Tempo!"); // Exibe uma mensagem de boas-vindas

            // Solicita ao usuário que insira o nome da cidade
            Console.Write("Digite o nome da cidade: ");
            string cidade = Console.ReadLine().ToUpper(); // Lê a entrada do usuário e converte para maiúsculas

            string apiKey = "API_KEY_HERE"; // Substitua pela chave de API real para acessar o OpenWeatherMap

            // Monta a URL da API com a cidade, chave da API e especifica que a unidade de medida deve ser Celsius
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&appid={apiKey}&units=metric";

            // Cria uma instância de HttpClient para enviar a requisição HTTP
            using(HttpClient client = new HttpClient())
            {
                try
                {
                    // Envia uma requisição GET à API de previsão do tempo e espera a resposta
                    HttpResponseMessage response = await client.GetAsync(url);
                    // Verifica se a resposta foi bem-sucedida, ou lança uma exceção
                    response.EnsureSuccessStatusCode();

                    // Lê o corpo da resposta como uma string
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Converte o JSON da resposta em um objeto PrevisaoTempo
                    PrevisaoTempo previsao = JsonConvert.DeserializeObject<PrevisaoTempo>(responseBody);

                    // Verifica se os dados de previsão foram carregados corretamente
                    if(previsao != null && previsao.Main != null && previsao.Weather != null)
                    {
                        // Exibe o nome da cidade, a temperatura e a condição climática
                        Console.WriteLine("Cidade: " + previsao.Name);
                        Console.WriteLine("Temperatura: " + previsao.Main.Temp.ToString("f1",CultureInfo.InvariantCulture) + "°C");
                        Console.WriteLine("Condição: " + previsao.Weather[0].Description);
                    }
                    else
                    {
                        // Se os dados de previsão não foram encontrados, exibe uma mensagem de erro
                        Console.WriteLine("Os dados de previsão não foram encontrados.");
                    }
                }
                // Trata erros relacionados à requisição HTTP
                catch(HttpRequestException e)
                {
                    Console.WriteLine("Erro ao obter a previsão do tempo: " + e.Message);
                }
                // Trata qualquer outro tipo de erro inesperado
                catch(Exception e)
                {
                    Console.WriteLine("Erro inesperado: " + e.Message);
                }

                // Aguarda uma tecla ser pressionada para encerrar o programa
                Console.ReadKey();
            }
        }
    }
}
