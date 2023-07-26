using System.Transactions;

namespace btr.nuna.Application
{
    public static class TransHelper
    {
        public static TransactionScope NewScope(IsolationLevel isolationLevel)
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = isolationLevel
                });
        }

        public static TransactionScope NewScope()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                });
        }
    }
}