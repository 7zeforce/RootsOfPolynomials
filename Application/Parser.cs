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
}