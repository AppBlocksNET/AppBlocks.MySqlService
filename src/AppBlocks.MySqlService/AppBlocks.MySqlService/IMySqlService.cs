using AppBlocks.Models;

namespace AppBlocks.MySqlService
{
    public interface IMySqlService
    {
        List<Item> Get();
        Item Get(string id);
        Item Create(Item student);
        void Update(string id, Item student);
        void Remove(string id);
    }
}