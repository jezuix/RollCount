using System.Diagnostics;

namespace RollCount
{

    public class Program
    {

        public static readonly int _indicePuloTexto = 7;

        static void Main()
        {
            Console.CursorVisible = false;
            Executar();
        }

        public static void Executar()
        {
            var rolls = new List<RollModel>()
            {
                new(4),
                new(6),
                new(8),
                new(10),
                new(12),
                new(20),
            };

            var timer = new Stopwatch();
            var resposta = PerguntaTipoRolagem();

            Console.Clear();
            Console.WriteLine($"Iniciando as rolagens {(resposta ? "Sequencia Completa" : "Sequencia de Grupo")}");
            Console.WriteLine();

            for (int i = 0; i < rolls.Count; i++)
            {
                var resultado = Rolagems(rolls[i], i, resposta);

                if (!resultado)
                    i = 0;
            }
            timer.Stop();
            Console.WriteLine();
            Console.WriteLine($"Tempo Total: {timer.Elapsed}");
        }

        public static bool PerguntaTipoRolagem()
        {
            Console.WriteLine("As rolagens serão sequencial ou em grupo?");
            Console.WriteLine("1 - Sequencia de Grupo (Uma rolagem errada retorna do inicio do grupo atual)");
            Console.WriteLine("2 - Sequencia Completa (Uma rolagem errada retorna do inicio do primeiro grupo)");

            var resposta = Console.ReadLine() == "2";

            return resposta;
        }

        public static bool Rolagems(RollModel rollModel, int indice, bool sequenciaCompleta = false)
        {
            Int128 tentativas = 0;
            long timer = Stopwatch.GetTimestamp();

            Console.SetCursorPosition(0, (indice * _indicePuloTexto) + 2);

            Console.WriteLine($"Rolagem : d{rollModel.RollMaxNumber}");

            var debugger = new List<(int, int)>();

            if (rollModel.Rolls.Any(x => x > rollModel.RollMaxNumber)) throw new Exception("Número inválido");

            int? maior = null;
            TimeSpan? tempoDoMaior = null;

            var listaRolagem = new List<int>();
            for (int i = 0; i < rollModel.RollMaxNumber; i++)
            {
                var retorna = false;
                tentativas++;

                int rolagem = new Random().Next(1, rollModel.RollMaxNumber + 1);

                debugger.Add((rolagem, rollModel.Rolls[i]));
                if (rolagem != rollModel.Rolls[i])
                {
                    listaRolagem = [];
                    if (sequenciaCompleta)
                        retorna = true;

                    i = -1;
                }
                else
                {
                    listaRolagem.Add(rolagem);

                    if (rolagem >= (maior ?? 0))
                    {
                        maior = rolagem;
                        tempoDoMaior = Stopwatch.GetElapsedTime(timer);
                    }
                }

                Console.WriteLine($"Tentativas: {tentativas}".PadRight(Console.BufferWidth));
                Console.WriteLine($"Tempo: {Stopwatch.GetElapsedTime(timer):dd\\ hh\\:mm\\:ss\\.fffffff}".PadRight(Console.BufferWidth));
                Console.WriteLine($"Maior rolagem: {maior?.ToString() ?? ""} em {tempoDoMaior:dd\\ hh\\:mm\\:ss\\.fffffff}".PadRight(Console.BufferWidth));
                Console.WriteLine($"Rolagem Atual: {string.Join(",", rolagem)}".PadRight(Console.BufferWidth));
                Console.WriteLine($"Rolagens em Sêquencia: {string.Join(",", listaRolagem)}".PadRight(Console.BufferWidth));

                Console.SetCursorPosition(0, (indice * _indicePuloTexto) + 3);

                if (retorna)
                    return false;
            }

            return true;
        }
    }
}