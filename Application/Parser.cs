using System.Linq.Expressions;
using Inpunter;

namespace Pars
{
    public class Parser
    {
        private List<Token> _tokens;
        private int _pos;
        private ParameterExpression _param;

        public Parser(List<Token> tokens, int pos, ParameterExpression param)
        {
            _tokens = tokens;
            _pos = pos;
            _param = param;
        }

        private Token Peek() => _tokens[_pos];
        private Token Next() => _tokens[_pos++];

        public Expression Parse()
        {
            _pos = 0;
            Expression expr = ParseExpression();
            if (Peek().Type != TokenType.End)
                throw new Exception("Unexpected token after expression");
            return expr;
        }

        private Expression ParseExpression()
        {
            Expression left = ParseTerm();
            while(Peek().Type == TokenType.Add || Peek().Type == TokenType.Subtract)
            {
                Token op = Next();
                Expression right = ParseTerm();
                if (op.Type == TokenType.Add) left = Expression.Add(left, right);
                else left = Expression.Subtract(left, right);
            }
            return left;
        }

        private Expression ParseTerm()
        {
            Expression left = ParseFactory();
            while(true)
            {
                Token next = Peek();
                if (next.Type == TokenType.Multiply || next.Type == TokenType.Divide)
                {
                    Token op = Next();
                    Expression right = ParseFactory();
                    left = op.Type == TokenType.Multiply ? Expression.Multiply(left, right) : Expression.Divide(left, right);
                }
                else if (next.Type == TokenType.Number || next.Type == TokenType.Variable)
                {
                    Expression right = ParseFactory(); 
                    left = Expression.Multiply(left, right);
                }
                else
                {
                    break;
                }
            }
            return left;
        }

        private Expression ParseFactory()
        {
            Expression left;
            Token token = Peek();
            if (token.Type == TokenType.Subtract)
            {
                Next();
                return Expression.Negate(ParseFactory());
            }
            if (token.Type == TokenType.Add)
            {
                Next();
                return ParseFactory();
            }
            if(token.Type == TokenType.Number)
            {
                Next();
                left = Expression.Constant(token.NumberValue);
            }
            else if (token.Type == TokenType.Variable)
            {
                Next();
                left = _param;
            }
            else if(token.Type == TokenType.LParen)
            {
                Next();
                Expression expr = ParseExpression();
                if(Peek().Type != TokenType.RParen)
                {
                    throw new Exception("Missing closing parenthesis");
                }
                Next();
                left = expr;
            }
            else
            {
                throw new Exception("Unexpected token in factor");
            }
            if(Peek().Type == TokenType.Power)
            {
                Next();
                Expression right = ParseFactory();
                left = Expression.Power(left, right);
            }
            return left;
        }
    }

    public class PolyParser
    {
        private List<Token> _tokens;
        private int _pos;

        public PolyParser(List<Token> tokens, int pos)
        {
            _tokens = tokens;
            _pos = pos;
        }

        private Token Peek() => _tokens[_pos];
        private Token Next() => _tokens[_pos++];

        public double[] Parse()
        {
            double[] coeffs = ParseExpression();
            if (Peek().Type != TokenType.End) throw new Exception("Unexpected token after expression");
            return Trim(coeffs);
        }

        private double[] ParseExpression()
        {
            double[] left = ParseTerm();
            while (Peek().Type == TokenType.Add || Peek().Type == TokenType.Subtract)
            {
                Token op = Next();
                double[] right = ParseTerm();
                left = op.Type == TokenType.Add ? Add(left, right) : Sub(left, right);
            }
            return left;
        }

        private double[] ParseTerm()
        {
            double[] left = ParseFactor();
            while (true)
            {
                Token next = Peek();
                if(next.Type == TokenType.Multiply || next.Type == TokenType.Divide)
                {
                    Token op = Next();
                    double[] right = ParseFactor();
                    left = op.Type == TokenType.Multiply ? Mul(left, right) : Div(left, right);
                }
                else if (next.Type == TokenType.Number || next.Type == TokenType.Variable)
                {
                    left = Mul(left, ParseFactor());
                }
                else break;
            }
            return left;
        }

        private double[] ParseFactor()
        {
            Token token = Peek();
            if(token.Type == TokenType.Subtract) { Next(); return Neg(ParseFactor()); }
            if (token.Type == TokenType.Add) { Next(); return ParseFactor(); }

            double[] result;
            if(token.Type == TokenType.Number) { Next(); result = new double[] { token.NumberValue }; }
            else if(token.Type == TokenType.Variable) { Next(); result = new double[] { 0, 1}; }
            else if(token.Type == TokenType.LParen)
            {
                Next();
                result = ParseExpression();
                if(Peek().Type != TokenType.RParen) throw new Exception("Missing closing parenthesis");
                Next();
            }
            else throw new Exception("Unexpected token in factor");
            if (Peek().Type == TokenType.Power)
            {
                Next();
                int k = ToIntExponent(ParseFactor());
                result = Pow(result, k);
            }
            return result;
        }

        private double[] Add(double[] a, double[] b)
        {
            double[] r = new double[Math.Max(a.Length, b.Length)];
            for (int i = 0; i < a.Length; i++) r[i] += a[i];
            for (int i = 0; i < b.Length; i++) r[i] += b[i];
            return r;
        }

        private double[] Sub(double[] a, double[] b)
        {
            double[] r = new double[Math.Max(a.Length, b.Length)];
            for (int i = 0; i < a.Length; i++) r[i] += a[i];
            for (int i = 0; i < b.Length; i++) r[i] -= b[i];
            return r;
        }

        private double[] Neg(double[] a)
        {
            double[] r = new double[a.Length];
            for (int i = 0; i < a.Length; i++) r[i] = -a[i];
            return r;
        }

        private static double[] Mul(double[] a, double[] b)
        {
            var r = new double[a.Length + b.Length - 1];
            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < b.Length; j++)
                    r[i + j] += a[i] * b[j];
            return r;
        }

        private static double[] Pow(double[] a, int k)
        {
            var r = new double[] { 1 };
            for (int t = 0; t < k; t++) r = Mul(r, a);
            return r;
        }

        private static double[] Div(double[] a, double[] b)
        {
            double[] bt = Trim(b);
            if (bt.Length != 1) throw new Exception("Division by a polynomial (rational function) is not supported");
            if (bt[0] == 0) throw new Exception("Division by zero");
            var r = new double[a.Length];
            for (int i = 0; i < a.Length; i++) r[i] = a[i] / bt[0];
            return r;
        }

        private static int ToIntExponent(double[] exp)
        {
            double[] t = Trim(exp);
            if (t.Length != 1) throw new Exception("The degree must be a constant");
            double v = t[0];
            if (v < 0 || v != Math.Floor(v)) throw new Exception("The degree must be an integer >= 0");
            return (int)v;
        }

        private static double[] Trim(double[] a)
        {
            int n = a.Length;
            while (n > 1 && Math.Abs(a[n - 1]) < 1e-12) n--;
            if (n == a.Length) return a;
            var r = new double[n];
            Array.Copy(a, r, n);
            return r;
        }
    }
}