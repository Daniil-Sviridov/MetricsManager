using System.Data.SQLite;

namespace Core
{
    public interface IConnectionManager
    {
        SQLiteConnection CreateOpenedConnection();
    }
}
