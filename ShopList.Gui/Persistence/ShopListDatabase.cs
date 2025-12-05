using SQLite;
using ShopList.Gui.Persistence.Configuration;
using ShopList.Gui.Models;

namespace ShopList.Gui.Persistence
{
    public class ShopListDataBase
    {
        private SQLiteAsyncConnection? _connection;

        private async Task InitAsync()
        {
            if (_connection != null)
            {
                return;
            }
            
            _connection = new SQLiteAsyncConnection( //establecemos una conexion y nos da las opciones
                Constants.DatabasePath, 
                Constants.Flags); //"Asyn" de asincrono
            await _connection.CreateTableAsync<Item>(); //"await" invoca este y sigue tu camino, crea una tabla de manera asincrona toma los datos de la clase item
        }
        public async Task<int> SaveItemAsync(Item item)
        {
            await InitAsync();
            return await _connection!.InsertAsync(item);
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            await InitAsync();
            return await _connection.Table<Item>().ToListAsync();
        }

        public async Task<int> RemoveItemAsync(Item item)
        {
            await InitAsync();
            return await _connection!.DeleteAsync(item);
        }
    }
}
