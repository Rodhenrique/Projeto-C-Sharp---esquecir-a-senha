using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace EsqueceuSenha
{
    class Program
    {
        class user
        {
            public string ID;
            public string Email;
            public string Senha;

        }
        static void Main(string[] args)
        {
            var use = new user
            {
                ID = "1",
                Email = "henrique@email.com",
                Senha = "Henrique2121"
            };

            string email = Console.ReadLine();

            string CodigoRedefinir = GerarCodigo(use.ID,use.Senha);
            string cone = ValidarCodigo(CodigoRedefinir);
            Console.WriteLine(CodigoRedefinir);
            Console.WriteLine(cone);
            string senhaNova = "senai132";

            string resultado = ModifyPass(cone,senhaNova);

            Console.WriteLine(resultado);
            EnviarEmail(email);
        }

        private static void EnviarEmail(string email)
        {
            //Exemplo de usuário que pediram para redefinir a senha
            string ID = "1";
            string senha = "RealVagas12345";
            string nome = "Marcos Rubens";

            //email do destinatário
            string to = email;
            //coloque seu email abaixo para efetuar o envio do código
            string from = "seu email para enviar";

            //Elaborar o envio da mensagem para o email
            MailMessage message = new MailMessage(from, to);
            //Titulo do email
            message.Subject = "Redefinir senha Real Vagas - Não Responda!!!";
            //método para gerar o código que será enviado para o email aonde passar o ID e a Senha do usuário
            string CodigoRedefinir = GerarCodigo(ID,senha);
            //Aqui será o mensagem que terá dentro do email enviado

            message.Body = @$"Olá senhor(a) {nome} solicitação para redefinir sua senha, codigo para \n redefinir sua senha: 
            {CodigoRedefinir}. Não espalhe para ninguem usei para alterar sua senha.";
            //Aqui e a porta aonde o programa acessaram o email, nesse exemplo e para outlook 
            //mas caso for outro provendor de email precisar proucurar a porta
            SmtpClient client = new SmtpClient("smtp.live.com", Convert.ToInt32("587"));
           
            client.UseDefaultCredentials = true;
            //Seu email e senha para enviar o email apartir dele 
            client.Credentials = new NetworkCredential("Seu email", "Sua senha");
            client.EnableSsl = true;

            try
            {
                //Enviar o email 
                client.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                    ex.ToString());
            }
        }

        private static string GerarCodigo(string ID, string Senha)
        {
            Random rnd = new Random();
            //string que será enviada para o usuário
            DateTime data = DateTime.Now.AddMinutes(5);
            string inicial = $"Rodhenrique:ID='{ID}±Pass:{Senha}¢data:{data};";
            string Codigo = "";
            //letra para descobrir qual hash usar;
            string[] lets = new string[] { "V", "R" };
            int num = rnd.Next(lets.Length);
            string LetraSecurity = lets[num];

            //herda os arrays da outra classe para deixa o codigo mais light 
            string[] letras = (LetraSecurity == "V") ? RedefinirRepository.PrimeiroArray() : RedefinirRepository.SegundoArray();
            string hash = (LetraSecurity == "V") ? RedefinirRepository.Hash_1() : RedefinirRepository.Hash_2();
            string[] Numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            //limpa o hash para preparação do codigo
            string hashModi = "";
            for (int i = 0; i < hash.Length; i++)
            {
                for (int h = 0; h < Numbers.Length; h++)
                {
                    string let = hash.Substring(i, 1);
                    if (let == Numbers[h])
                    {
                        hashModi += string.Concat(let);
                        break;
                    }
                }
            }

            List<string> hashs = new List<string>();

            for (int i = 0; i < hashModi.Length; i += 3)
            {
                int startIndex = i;
                int length = 3;
                string substring = hashModi.Substring(startIndex, length);
                hashs.Add(substring);
            }

            //através do hash ele transformar a string inicial em cryptografia
            char[] cod = inicial.ToArray();

            for (int i = 0; i < cod.Length; i++)
            {
                for (int j = 0; j < letras.Length; j++)
                {
                    if (cod[i].ToString() == letras[j])
                    {
                        Codigo += string.Concat(hashs[j]);
                        break;
                    }
                }
            }

            //proteger o codigo para ficar mais dificil de descodificar
            string[] letters = RedefinirRepository.Letras();
            for (int i = 0; i < Codigo.Length; i++)
            {
                int rand = rnd.Next(letters.Length);
                int go = rnd.Next(1, 11);
                if (go >= 5)
                {
                    Codigo = Codigo.Insert(i,letters[rand]);
                }
            }

            Codigo = Codigo.Insert(0,LetraSecurity);
            return Codigo;
        }

        private static string ValidarCodigo(string str)
        {
            string codigo = "";
            string LetraSecurity = str.Substring(0, 1);

            //herda os arrays da outra classe para deixa o codigo mais light 
            string[] letras = (LetraSecurity == "V") ? RedefinirRepository.PrimeiroArray() : RedefinirRepository.SegundoArray();
            string hash = (LetraSecurity == "V") ? RedefinirRepository.Hash_1() : RedefinirRepository.Hash_2();
            string[] Numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            //limpa o hash para preparação do codigo
            string hashModi = "";
            for (int i = 0; i < hash.Length; i++)
            {
                for (int h = 0; h < Numbers.Length; h++)
                {
                    string let = hash.Substring(i, 1);
                    if (let == Numbers[h])
                    {
                        hashModi += string.Concat(let);
                        break;
                    }
                }
            }
            //limpa a string recebida 
            string Strlimpo = "";
            for (int i = 0; i < str.Length; i++)
            {
                for (int h = 0; h < Numbers.Length; h++)
                {
                    string let = str.Substring(i, 1);
                    if (let == Numbers[h])
                    {
                        Strlimpo += string.Concat(let);
                        break;
                    }
                }
            }

            List<string> max = new List<string>();
            for (int i = 0; i < hashModi.Length; i += 3)
            {
                int startIndex = i;
                int length = 3;
                string substring = hashModi.Substring(startIndex, length);
                max.Add(substring);
            }
            List<string> hulk = new List<string>();
            for (int i = 0; i < Strlimpo.Length; i += 3)
            {
                int startIndex = i;
                int length = 3;
                string substring = Strlimpo.Substring(startIndex, length);
                hulk.Add(substring);
            }

            string nova = "";
            for (int i = 0; i < hulk.Count; i++)
            {
                for (int h = 0; h < max.Count; h++)
                {
                    if (hulk[i].ToString() == max[h].ToString())
                    {
                        nova += letras[h];
                        break;
                    }
                }
            }
            codigo = nova;

            return codigo;
        }
        public static string ModifyPass(string mody, string senha)
        {
            mody.Trim();
            //Buscar o ID na string
            string rappi = mody.Substring(mody.IndexOf("ID") + 4);
            int ho = rappi.IndexOf('±');
            int id = Convert.ToInt32(rappi.Substring(0, ho));

            //Buscar a senha na string
            string pos = mody.Substring(mody.IndexOf("Pass") + 5);
            int tilo = pos.IndexOf('¢');
            string pass = pos.Substring(0, tilo);

            //Buscar a data na string
            string jun = mody.Substring(mody.IndexOf("data") + 5, 19);
            DateTime data = Convert.ToDateTime(jun);

            if (DateTime.Now < data)
            {
                //aqui pode trocar a senha do usuário no sistema
                var use = new user();
                use.Senha = senha;
                return "Senha alterada com sucesso!!!";
            }
            else
            {
                return "Não autenticado";
            }
        }
    }
}
