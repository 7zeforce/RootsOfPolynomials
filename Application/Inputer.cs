using System.Linq.Expressions;
using Pars;

namespace Inpunter
{
    public class Inputer
    {
        private List<Token> _tokens;

        public string Input()
        {
            Console.WriteLine("Enter a mathematical Polynomial format (e.g., 2*x + 3):");
            string input = Console.ReadLine();
            _tokens = Tokenize(input);
            return input;
        }

        public Func<double, double> CreateFunc()
        {
            ParameterExpression param = Expression.Parameter(typeof(double), "x");
            Parser parser = new Parser(_tokens, 0, param);
            Expression expr = parser.Parse();
            var lambda = Expression.Lambda<Func<double, double>>(expr, param);
            return lambda.Compile();
        }

        public double[] CreateCoificents()
        {
            PolyParser parser = new PolyParser(_tokens, 0);
            return parser.Parse();
        }

        private List<Token> Tokenize(string input)
        {
            List<Token> tokens = new List<Token>();
            int i = 0;
            while (i < input.Length)
            {
                char c = input[i];
                if (char.IsWhiteSpace(c))
                {
                    i++;
                    continue;
                }
                if (char.IsDigit(c) || c == '.')
                {
                    string number = "";
                    while (i < input.Length && (char.IsDigit(input[i]) || input[i] == '.'))
                        number += input[i++];
                    tokens.Add(new Token { Type = TokenType.Number, Value = number, NumberValue = double.Parse(number) });
                    continue;
                }
                if (c == 'x' || c == 'X')
                {
                    tokens.Add(new Token { Type = TokenType.Variable, Value = c.ToString() });
                    i++;
                    continue;
                }
                if(c == 'c' || c == 's' || c == 't')
                {
                    string func = "";
                    while (i < input.Length && char.IsLetter(input[i]))
                    {
                        func += input[i];
                        i++;
                    }
                    tokens.Add(new Token { Type = TokenType.Func, Value = func });
                    continue;
                }
                if (c == 'e' || c == 'E')
                {
                    tokens.Add(new Token { Type = TokenType.Number, NumberValue = Math.E });
                    i++;
                    continue;
                }
                switch (c)
                {
                    case '+': tokens.Add(new Token { Type = TokenType.Add }); break;
                    case '-': tokens.Add(new Token { Type = TokenType.Subtract }); break;
                    case '*': tokens.Add(new Token { Type = TokenType.Multiply }); break;
                    case '/': tokens.Add(new Token { Type = TokenType.Divide }); break;
                    case '^': tokens.Add(new Token { Type = TokenType.Power }); break;
                    case '(': tokens.Add(new Token { Type = TokenType.LParen }); break;
                    case ')': tokens.Add(new Token { Type = TokenType.RParen }); break;
                    default: throw new Exception($"Unexpected character: {c}");
                }
                i++;
            }
            tokens.Add(new Token { Type = TokenType.End });
            return tokens;
        }
    }

    public enum TokenType
    {
        Number, Variable, Add, Subtract, Multiply, Divide, Power, Func, LParen, RParen, End
    }

    public struct Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public double NumberValue { get; set; }
    }
}