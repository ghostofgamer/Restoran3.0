using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Enums;
using LoadingSceneContent;
using TMPro;
using UI.Screens.ShopContent.ShopPages.PageContents.WorksPage;
using UnityEngine;
using UnityEngine.UI;

namespace RestaurantContent.MenuContent
{
    public class MenuInitializer : MonoBehaviour
    {
        private const string MenuKey = "MenuList";

        [SerializeField] private MenuScrollContent _menuScrollContent;
        [SerializeField] private MenuCounter _menuCounter;
        [SerializeField] private LoadingGame _loadingGame;
        [SerializeField] private Image _image;
        
        [SerializeField] private TMP_Text _saveStatusText; // Для отображения статуса сохранения
        [SerializeField] private TMP_Text _loadStatusText;
        
        private List<ItemType> _menuList = new List<ItemType>();

        private void Awake()
        {
            /*_menuScrollContent.Init();*/
        }

        private void OnEnable()
        {
            _menuCounter.ChangeMenuList += ChangeListMenu;
            _loadingGame.OnLoadingComplete += Initialization;
        }

        private void OnDisable()
        {
            _menuCounter.ChangeMenuList -= ChangeListMenu;
            _loadingGame.OnLoadingComplete -= Initialization;
        }

        private void Start()
        {
            // _menuScrollContent.Init();
        }

        private void Initialization()
        {
            _image.color = Color.green;
            _menuScrollContent.Init();
            _menuList = LoadMenu();

            foreach (var t in _menuList)
                _menuScrollContent.AddItem(t);
        }

        private void ChangeListMenu(List<ItemType> menuList)
        {
            SaveMenu(menuList);
        }

        /*private void SaveMenu(List<ItemType> menuList)
        {
            List<string> stringList = new List<string>();

            foreach (var item in menuList)
            {
                stringList.Add(item.ToString());
            }

            string json = JsonUtility.ToJson(new Serialization<string>(stringList));
            PlayerPrefs.SetString(MenuKey, json);
            PlayerPrefs.Save();
        }

        private List<ItemType> LoadMenu()
        {
            if (PlayerPrefs.HasKey(MenuKey))
            {
                string json = PlayerPrefs.GetString(MenuKey);
                List<string> stringList = JsonUtility.FromJson<Serialization<string>>(json).target;
                List<ItemType> menuList = new List<ItemType>();

                foreach (var str in stringList)
                {
                    menuList.Add((ItemType)System.Enum.Parse(typeof(ItemType), str));
                }

                return menuList;
            }

            return new List<ItemType>();
        }*/

        
        
        
        public async void SaveMenu(List<ItemType> menuList)
        {
            try
            {
                // Преобразуем список ItemType в список идентификаторов (например, (int)ItemType)
                List<int> menuItemsToSave = menuList.Select(item => (int)item).ToList();

                // Сериализуем данные в JSON
                string jsonData = JsonUtility.ToJson(new MenuDataWrapper(menuItemsToSave));
                string path = Path.Combine(Application.persistentDataPath, "menuData.json");

                // Асинхронная запись в файл
                await File.WriteAllTextAsync(path, jsonData);
                Debug.Log($"Menu saved successfully to {path}");
                
                
                // Обновляем текст о статусе сохранения
                string savedItemsInfo = "Сохранено:\n";
                foreach (var item in menuList)
                {
                    savedItemsInfo += $"- {item} (ID: {(int)item})\n";
                }

                // Обновляем текст о статусе сохранения
                if (_saveStatusText != null)
                {
                    _saveStatusText.text = $"{savedItemsInfo}";
                }

            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save menu: {ex.Message}");
                _saveStatusText.text = $"Failed to save menu: {ex.Message}";
            }
        }
        
        public List<ItemType> LoadMenu()
        {
            string path = Path.Combine(Application.persistentDataPath, "menuData.json");
            
            if (File.Exists(path))
            {
                string jsonData = File.ReadAllText(path);
                MenuDataWrapper wrapper = JsonUtility.FromJson<MenuDataWrapper>(jsonData);
                // Преобразуем идентификаторы обратно в ItemType
                List<ItemType> loadedItems = wrapper.menuItems.Select(id => (ItemType)id).ToList();
                
                
                string loadedItemsInfo = "Загружено:\n";
                foreach (var item in loadedItems)
                {
                    loadedItemsInfo += $"- {item} (ID: {(int)item})\n";
                }

                // Обновляем текст о статусе загрузки
                if (_loadStatusText != null)
                {
                    _loadStatusText.text = $"{loadedItemsInfo}";
                }
                
                return loadedItems;
            }
            return new List<ItemType>();
        }
        
        [ContextMenu("ClearSavedMenu")]
        public void ClearSavedMenu()
        {
            string path = Path.Combine(Application.persistentDataPath, "menuData.json");
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Saved menu cleared.");
            }
            else
            {
                Debug.Log("No saved menu found.");
            }
        }
        
        
        /*private void SaveMenu(List<ItemType> menuList)
        {
            List<int> intList = menuList.Select(item => (int)item).ToList();
            string joinedString = string.Join(",", intList);
            PlayerPrefs.SetString(MenuKey, joinedString);
            PlayerPrefs.Save();
            Debug.Log($"Menu saved: {joinedString}");
        }
        
        private List<ItemType> LoadMenu()
        {
            if (PlayerPrefs.HasKey(MenuKey))
            {
                string joinedString = PlayerPrefs.GetString(MenuKey);
                List<int> intList = joinedString.Split(',').Select(int.Parse).ToList();
                List<ItemType> menuList = intList.Select(intValue => (ItemType)intValue).ToList();
                return menuList;
            }
            return new List<ItemType>();
        }*/
        
        
        
        /*private void SaveMenu(List<ItemType> menuList)
        {
            Debug.Log($"Saving menu with {menuList.Count} items");
            List<string> stringList = new List<string>();
            foreach (var item in menuList)
            {
                stringList.Add(item.ToString());
                Debug.Log($"Adding item: {item}");
            }

            string json = JsonUtility.ToJson(new Serialization<string>(stringList));
            PlayerPrefs.SetString(MenuKey, json);
            PlayerPrefs.Save();
            Debug.Log($"Menu saved to PlayerPrefs: {json}");
        }

        private List<ItemType> LoadMenu()
        {
            if (PlayerPrefs.HasKey(MenuKey))
            {
                Debug.Log("Loading menu from PlayerPrefs");
                string json = PlayerPrefs.GetString(MenuKey);
                Debug.Log($"Loaded JSON: {json}");
                try
                {
                    List<string> stringList = JsonUtility.FromJson<Serialization<string>>(json).target;
                    List<ItemType> menuList = new List<ItemType>();

                    foreach (var str in stringList)
                    {
                        menuList.Add((ItemType)System.Enum.Parse(typeof(ItemType), str));
                        Debug.Log($"Loaded item: {str}");
                    }

                    return menuList;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to load menu: {ex.Message}");
                    return new List<ItemType>();
                }
            }

            Debug.Log("No saved menu found");
            return new List<ItemType>();
        }*/

        [System.Serializable]
        public class Serialization<T>
        {
            public List<T> target;

            public Serialization(List<T> target)
            {
                this.target = target;
            }
        }
        
        [System.Serializable]
        public class MenuDataWrapper
        {
            public List<int> menuItems; // или List<ItemType>, если ItemType сериализуем
            public MenuDataWrapper(List<int> items)
            {
                menuItems = items;
            }
        }
    }
}