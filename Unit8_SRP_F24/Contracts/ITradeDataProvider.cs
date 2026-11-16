using System;
using System.Threading.Tasks;

namespace Unit8_SRP_F24.Contracts;

public interface ITradeDataProvider
{
    public Task<IEnumerable<string>> ReadTradeDataAsync();
}
