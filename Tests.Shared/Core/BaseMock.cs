using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Easy.Transfers.Tests.Shared.Core
{
    public abstract class BaseMock<T> where T : class
    {

        public Mock<T> Mock { get; set; }

        public BaseMock()
        {
            Mock = new Mock<T>();
        }

        public void Setup(Expression<Action<T>> expression)
        {
            Mock.Setup(expression);
        }

        public void Setup<TResult>(Expression<Func<T, TResult>> expression, TResult objetoRetorno)
        {
            Mock.Setup(expression).Returns(objetoRetorno);
        }

        public void Setup<TResult>(Expression<Func<T, Task<TResult>>> expression, TResult objetoRetorno)
        {
            Mock.Setup(expression).ReturnsAsync(objetoRetorno);
        }

        public abstract Mock<T> GetDefaultInstance();
    }
}
