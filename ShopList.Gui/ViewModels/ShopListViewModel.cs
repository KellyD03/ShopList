using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopList.Gui.Models;
using System.Collections.ObjectModel;
using ShopList.Gui.Persistence;


namespace ShopList.Gui.ViewModels
{
    public partial class ShopListViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _nombreDelArticulo = string.Empty;

        [ObservableProperty]
        private int _cantidadAComprar = 1;

        [ObservableProperty]
        private Item? _id = null;


        [ObservableProperty]
        private ObservableCollection<Item>? _items = null;

        private ShopListDataBase? _database = null;

        public ShopListViewModel()
        {
            _database = new ShopListDataBase();
            Items = new ObservableCollection<Item>();
            GetItems();
            //CargarDatos();

            //AgregarShopListItemCommand = new Command(AgregarShopListItem);
        }

        private async void GetItems()
        {

            IEnumerable<Item> itemsFromDb = await _database.GetAllItemsAsync();

            Items = new ObservableCollection<Item>(itemsFromDb);

            /* 
            foreach (Item item in itemsFromDb)
            {
                Items.Add(item);
            }*/
        }

        [RelayCommand]

        public async Task AgregarShopListItem()
        {
            if (string.IsNullOrEmpty(NombreDelArticulo) || CantidadAComprar <= 0)
            {
                return;
            }
            // Random generador = new Random();    

            var item = new Item
            {
                // Id =generador.Next(),
                Nombre = NombreDelArticulo,
                Cantidad = CantidadAComprar,
                Comprado = false,
            };
            await _database.SaveItemAsync(item);
            // Items.Add(item)
            GetItems();
            NombreDelArticulo = string.Empty;
            CantidadAComprar = 1;
            Id = item;
        }

        [RelayCommand]
        public void EliminarShopListItem()
        {
            if (Id == null)
                return;

            // Obtener índice del elemento seleccionado
            int indice = Items.IndexOf(Id);

            // Determinar nuevo elemento seleccionado
            Item? nuevoSeleccionado = null;

            if (Items.Count > 1)
            {
                if (indice < Items.Count - 1)
                {
                    // Seleccionar el siguiente
                    nuevoSeleccionado = Items[indice + 1];
                }
                else
                {
                    // Si era el último, seleccionar el anterior
                    nuevoSeleccionado = Items[indice - 1];
                }
            }

            // Eliminar el item actual
            Items.Remove(Id);

            // Actualizar selección
            Id = nuevoSeleccionado;
        }

        private void CargarDatos()
        {

            Items.Add(new Item()
            {
                Id = 1,
                Nombre = "Leche",
                Cantidad = 2,
                Comprado = false,
            });

            Items.Add(new Item()
            {
                Id = 2,
                Nombre = "Pan de caja",
                Cantidad = 2,
                Comprado = true,
            });

            Items.Add(new Item()
            {
                Id = 3,
                Nombre = "Jamon",
                Cantidad = 2,
                Comprado = false,
            });
        }
    }
}

